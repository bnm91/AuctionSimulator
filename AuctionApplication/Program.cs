﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionApplication.AuctioneerClients.Clients;
using AuctionApplication.AuctioneerService;
using AuctionApplication.AvailableItems.Providers.FantasyFootball;
using AuctionApplication.Bidders;
using AuctionApplication.Bidders.BiddingStrategies;
using AuctionApplication.Bidders.CollectionBuildingStrategies;
using AuctionApplication.Bidders.CollectionBuildingStrategies.FantasyFootball;
using AuctionApplication.Bidders.CollectionBuildingStrategies.Greedy;
using AuctionApplication.Bidders.NominationStrategies;
using AuctionApplication.Bidders.ReservationPriceCalculators;
using AuctionApplication.Bidders.ReservationPriceCalculators.NoisyExpert.ExpertReservationPriceProviders.FProsExpertReservationPriceProvider;
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
            List<Player> availableItems = new FantasyFootballAvailablePlayersProvider().GetAvailableItems().ToList();

            AuctioneerService<Player> auctioneerService = new AuctioneerService<Player>(availableItems);
            LocalConsoleAuctioneerClient<Player> auctioneerClient = new LocalConsoleAuctioneerClient<Player>(auctioneerService);

            var botCollectionBuildingStrategy = new StartersFirstStrategy(); 
            var botReservationPriceProvider = new FProsExpertReservationPriceProvider(14, 200, "half", "23", "08", "13");
            var botNominationStrategy = new DefaultNominationStrategy<Player>();
            var botBiddingStrategy = new BotBiddingStrategy<Player>(
                new NoisyExpertReservationPriceCalculator(botReservationPriceProvider, new UniformNoiseGenerator()), 
                botCollectionBuildingStrategy);
            var bot2BiddingStrategy = new BotBiddingStrategy<Player>(
                new NoisyExpertReservationPriceCalculator(botReservationPriceProvider, new UniformNoiseGenerator()), 
                botCollectionBuildingStrategy);
            var bot3BiddingStrategy = new BotBiddingStrategy<Player>(
                new NoisyExpertReservationPriceCalculator(botReservationPriceProvider, new UniformNoiseGenerator()), 
                botCollectionBuildingStrategy);
            List<Bidder<Player>> bidders = new List<Bidder<Player>>()
            {
                new Bidder<Player>(botBiddingStrategy, botNominationStrategy, auctioneerClient),
                new Bidder<Player>(bot2BiddingStrategy, botNominationStrategy, auctioneerClient),
                new Bidder<Player>(bot3BiddingStrategy, botNominationStrategy, auctioneerClient)
            };

            Auctioneer<Player> auctioneer = new Auctioneer<Player>(auctioneerService);
            Task.Factory.StartNew(auctioneer.Run);

            Parallel.ForEach(bidders, bidder => bidder.Run());
        }
    }
}
