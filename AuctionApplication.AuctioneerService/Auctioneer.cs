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
        private readonly AuctioneerService<T> _auctioneerService;

        public Auctioneer(AuctioneerService<T> auctioneerService)
        {
            _auctioneerService = auctioneerService;
        }

        public void Run()
        {
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
                        if (_auctioneerService.GetAvailable().Count > 0)
                        {
                            _auctioneerService.AuctionStateContext.BidMissed();
                        }
                        else
                        {
                            _auctioneerService.AuctionStateContext.EndAuction();
                            Console.ReadLine();
                        }
                    }
                }

            }
        }

    }
}
