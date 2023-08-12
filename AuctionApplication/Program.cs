using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionApplication.AuctioneerClients.Clients;
using AuctionApplication.AuctioneerService;
using AuctionApplication.Bidders;
using AuctionApplication.Bidders.BiddingStrategies;
using AuctionApplication.Bidders.CollectionBuildingStrategies;
using AuctionApplication.Bidders.CollectionBuildingStrategies.Greedy;
using AuctionApplication.Bidders.NominationStrategies;
using AuctionApplication.Bidders.ReservationPriceCalculators;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Setting up");

            //TEST CODE
            //TODO: Abstract Factory Pattern needed here
            List<Painting> availableItems = new List<Painting>()
            {
                new Painting() {Name="Mona Lisa" },
                new Painting() {Name="Starry Night" },
                new Painting() {Name="Girl with a Pearl Earring" },
                new Painting() {Name="American Gothic" }

            };
            AuctioneerService<Painting> auctioneerService = new AuctioneerService<Painting>(availableItems);
            LocalConsoleAuctioneerClient<Painting> auctioneerClient = new LocalConsoleAuctioneerClient<Painting>(auctioneerService);

            var botCollectionBuildingStrategy = new GreedyCollectionBuildingStrategy<Painting>();
            var botReservationPriceCalculator = new RandomReservationPriceCalculator(5);
            var botNominationStrategy = new DefaultNominationStrategy<Painting>();
            var botBiddingStrategy = new BotBiddingStrategy<Painting>(botReservationPriceCalculator, botCollectionBuildingStrategy);
            var bot2BiddingStrategy = new BotBiddingStrategy<Painting>(new RandomReservationPriceCalculator(10), botCollectionBuildingStrategy);
            List<Bidder<Painting>> bidders = new List<Bidder<Painting>>()
            {
                new Bidder<Painting>(botBiddingStrategy, botNominationStrategy, auctioneerClient),
                new Bidder<Painting>(bot2BiddingStrategy, botNominationStrategy, auctioneerClient)
            };

            Auctioneer<Painting> auctioneer = new Auctioneer<Painting>(auctioneerService);
            Task.Factory.StartNew(auctioneer.Run);

            Parallel.ForEach(bidders, bidder => bidder.Run());
        }
    }
}
