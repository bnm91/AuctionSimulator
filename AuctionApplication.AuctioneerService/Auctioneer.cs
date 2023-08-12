using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using AuctionApplication.AuctioneerService.StateMachine;
using AuctionApplication.Common.Models;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.AuctioneerService
{
    public class Auctioneer<T> where T : IItem
    {
        // when active and bids are changing        => active
        // when active and bids not changing        => going once
        // when going once and bids changing        => active
        // when going once and bids not changing    => going twice
        // when going twice and bids changing       => active
        // when going twice and bids not changing   => sold=>awaiting nom
        // when awaiting nom receive nomination     => active

        private readonly AuctioneerService<T> _auctioneerService;

        public Auctioneer(AuctioneerService<T> auctioneerService)
        {
            _auctioneerService = auctioneerService;
        }

        public void Run()
        {
            _auctioneerService.AuctionStatus.AuctionState = AuctionStates.AwaitingNomination;

            while (true)
            {
                //TODO: improve "end of auction" logic
                if(_auctioneerService.GetAvailable().Count <= 0)
                {
                    break;
                }
;
                while(true)
                {
                    //AuctionStates state = _auctioneerService.AuctionStatus.AuctionState; //TODO: do we need captured state?
                    if(_auctioneerService.GetIncomingBids().TryTake(out Bid<T> nextBid))
                    {
                        if(nextBid != null)
                        {
                            _auctioneerService.ConsumeBid(nextBid);
                            _auctioneerService.AuctionStateContext.BidProcessed();
                        }
                    }
                    else
                    {
                        _auctioneerService.AuctionStateContext.BidMissed();
                    }
                }

            }
        }

    }
}
