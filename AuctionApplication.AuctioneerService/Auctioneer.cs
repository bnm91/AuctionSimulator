using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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
                    //Bid<T> bid = null;
                    AuctionStates state = _auctioneerService.AuctionStatus.AuctionState;
                    if(_auctioneerService.GetIncomingBids().TryTake(out Bid<T> nextBid))
                    {
                        if(nextBid != null)
                        {
                            _auctioneerService.ConsumeBid(nextBid);
                        }
                    }
                    else
                    { 
                        //manage state
                        if(state == AuctionStates.Active)
                        {
                            _auctioneerService.AuctionStatus.AuctionState = AuctionStates.GoingOnce;
                            Console.WriteLine("Going once...");
                            Thread.Sleep(1000);
                        }
                        else if(state == AuctionStates.GoingOnce)
                        {
                            _auctioneerService.AuctionStatus.AuctionState = AuctionStates.GoingTwice;
                            Console.WriteLine("Going twice...");
                            Thread.Sleep(1000);
                        }
                        else if(state == AuctionStates.GoingTwice)
                        {
                            _auctioneerService.AuctionStatus.AuctionState = AuctionStates.AwaitingNomination;
                            Console.WriteLine("SOLD!");
                        }
                        else if(state == AuctionStates.AwaitingNomination)
                        {
                            //do nothing
                        }
                        else
                        {
                            Console.WriteLine("how did we get here?");
                        }
                    }

                    //try
                    //{
                    //    bid = _auctioneerService.GetIncomingBids().Take();
                    //}
                    //catch(InvalidOperationException)
                    //{ }

                    //if(bid != null)
                    //{
                    //    _auctioneerService.ConsumeBid(bid);
                    //}

                    //var status = _auctioneerService.GetStatus();
                    //if (status.AuctionState == AuctionStates.Active)
                    //{
                    //    Console.WriteLine($"Current bid: {status.WinningBid.Amount} for {status.WinningBid.Item.Name}");
                    //    ManageActiveState(status);
                    //}
                    //else if (status.AuctionState == AuctionStates.GoingOnce)
                    //{
                    //    ManageGoingOnceState(status);
                    //}
                    //else if (status.AuctionState == AuctionStates.GoingTwice)
                    //{
                    //    ManageGoingTwiceState(status);
                    //}
                    //else if (status.AuctionState == AuctionStates.AwaitingNomination)
                    //{
                    //    ManageAwaitingNominationState(status);
                    //}
                }

            }
        }

        private void ManageActiveState(AuctionStatus<T> status)
        {
            Thread.Sleep(1000);
            if (status.WinningBid.Equals(_auctioneerService.GetStatus().WinningBid))
            {
                _auctioneerService.AuctionStatus.AuctionState = AuctionStates.GoingOnce;
            }
            else
            {
                _auctioneerService.AuctionStatus.AuctionState = AuctionStates.Active;
            }
        }

        private void ManageGoingOnceState(AuctionStatus<T> status)
        {
            Console.WriteLine("Going once");
            Thread.Sleep(1000);
            if (status.WinningBid.Equals(_auctioneerService.GetStatus().WinningBid))
            {
                _auctioneerService.AuctionStatus.AuctionState = AuctionStates.GoingTwice;
            }
            else
            {
                _auctioneerService.AuctionStatus.AuctionState = AuctionStates.Active;
            }
        }

        private void ManageGoingTwiceState(AuctionStatus<T> status)
        {
            Console.Write("Going twice");
            Thread.Sleep(1000);
            if (status.WinningBid.Equals(_auctioneerService.GetStatus().WinningBid))
            {
                Console.WriteLine("SOLD!");
                _auctioneerService.AuctionStatus.AuctionState = AuctionStates.AwaitingNomination;
            }
            else
            {
                _auctioneerService.AuctionStatus.AuctionState = AuctionStates.Active;
            }
        }

        private void ManageAwaitingNominationState(AuctionStatus<T> status)
        {
            if (_auctioneerService.GetStatus() == null
                || _auctioneerService.GetStatus().WinningBid == null
                || status.WinningBid == null)
                //|| (status.WinningBid.Amount <= 0 && status.WinningBid.Item.Equals(_auctioneerService.GetStatus().WinningBid.Item)))
            {
                _auctioneerService.AuctionStatus.AuctionState = AuctionStates.AwaitingNomination;
            }
            else
            {
                Console.WriteLine($"Now bidding on {_auctioneerService.GetStatus().WinningBid.Item.Name}");
                _auctioneerService.AuctionStatus.AuctionState = AuctionStates.Active;
            }
        }
    }
}
