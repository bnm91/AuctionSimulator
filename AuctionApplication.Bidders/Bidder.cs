using System;
using System.Collections.Generic;
using System.Text;
using AuctionApplication.AuctioneerClient;
using AuctionApplication.Bidders.BiddingStrategies;
using AuctionApplication.Bidders.NominationStrategies;
using AuctionApplication.Common.Models;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.Bidders
{
    public class Bidder<T> where T : IItem
    {
        public Guid Id { get; set; }
        private readonly IBiddingStrategy<T> _biddingStrategy;
        private readonly INominationStrategy<T> _nominationStrategy;
        private readonly IAuctioneerClient<T> _auctioneerClient;

        public Bidder(IBiddingStrategy<T> biddingStrategy,
            INominationStrategy<T> nominationStrategy,
            IAuctioneerClient<T> auctioneerClient
            )
        {
            Id = Guid.NewGuid();
            _biddingStrategy = biddingStrategy;
            _nominationStrategy = nominationStrategy;
            _auctioneerClient = auctioneerClient;
        }

        public async void Run()
        {
            AuctionStates state = AuctionStates.AwaitingNomination;
            while(state != AuctionStates.Closed)
            {
                var status = await _auctioneerClient.GetStatus();
                state = status.AuctionState;
                switch (state)
                {
                    case AuctionStates.Active:
                        if (status.WinningBid.Amount <= 0 ||
                            _biddingStrategy.WillRaiseBid(status.WinningBid.Item, status.WinningBid.Amount))
                        {
                            if (status.WinningBid.BidderId != Id)
                            {
                                var bid = new Bid<T>() { Item = status.WinningBid.Item, Amount = status.WinningBid.Amount + 1 };
                                await _auctioneerClient.RaiseBid(Id, bid);
                            }
                        }
                        break;
                    case AuctionStates.AwaitingNomination:
                    case AuctionStates.Nomination:
                        var availableNominees = await _auctioneerClient.GetAvailable();
                        if (_nominationStrategy.TrySelectNominee(availableNominees, out T nominee))
                        {
                            await _auctioneerClient.Nominate(Id, nominee);
                        }
                        break;
                    case AuctionStates.Closed:
                    default:
                        break;
                }
            }
        }

    }
}
