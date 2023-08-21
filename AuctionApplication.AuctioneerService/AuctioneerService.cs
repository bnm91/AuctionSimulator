using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AuctionApplication.AuctioneerService.StateMachine;
using AuctionApplication.AuctioneerService.StateMachine.States;
using AuctionApplication.Common.Models;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.AuctioneerService
{
    public class AuctioneerService<T> where T : IItem
    {
        public AuctionStatus<T> AuctionStatus;

        private readonly BlockingCollection<Bid<T>> _incomingBids;
        private readonly List<T> _availableItems;
        public AuctionStateContext AuctionStateContext;

        public AuctioneerService(List<T> availableItems) 
        {
            _availableItems = availableItems;
            AuctionStateContext = new AuctionStateContext();
            AuctionStatus = new AuctionStatus<T>()
            {
                WinningBid = null,
                AuctionState = GetAuctionStateDto()
            };
            _incomingBids = new BlockingCollection<Bid<T>>();
        }

        public AuctionStatus<T> GetStatus()
        {
            AuctionStatus.AuctionState = GetAuctionStateDto();
            return AuctionStatus;
        }

        public BlockingCollection<Bid<T>> GetIncomingBids()
        {
            return _incomingBids;
        }

        public List<T> GetAvailable()
        {
            return _availableItems;
        }

        public void PlaceBid(Bid<T> bid)
        {
            if (AuctionStateContext.GetState() is ActiveState) // TODO: is there anyway to get state management out of here entirely?
            {
                _incomingBids.Add(bid);
            }
            if (AuctionStateContext.GetState() is AwaitingNominationState)
            {
                _incomingBids.Add(bid);
            }
        }

        public void ConsumeBid(Bid<T> bid)
        {
            //Console.WriteLine("bid consumed");
            if (AuctionStateContext.GetState() is ActiveState)
            {
                var currentBid = AuctionStatus.WinningBid;
                if(currentBid.Amount < bid.Amount)
                {
                    AuctionStatus.WinningBid = bid;

                    //TODO: remove/cleanup
                    //Temp debugging code
                    Console.WriteLine($"Winning Bid on {bid.Item.Name} now {bid.Amount} by {bid.BidderId}");
                }
            }
            else if (AuctionStateContext.GetState() is AwaitingNominationState)
            {
                _availableItems.Remove(bid.Item);
                SetCurrentWinningBid(new Tuple<Guid, Bid<T>>(bid.BidderId, bid));

                // TODO: remove/cleanup
                Console.WriteLine($"Now bidding on {bid.Item.Name}");
            }
        }

        private void SetCurrentWinningBid(Tuple<Guid, Bid<T>> newWinningBid)
        {
            AuctionStatus.WinningBid = new Bid<T>()
            {
                BidderId = newWinningBid.Item1,
                Amount = newWinningBid.Item2.Amount,
                Item = newWinningBid.Item2.Item,
            };
        }

        private AuctionStates GetAuctionStateDto()
        {
            var state = AuctionStateContext.GetState();
            if(state is ActiveState)
            {
                return AuctionStates.Active;
            }
            else if(state is AwaitingNominationState)
            {
                return AuctionStates.AwaitingNomination;
            }
            else if (state is GoingOnceState)
            {
                return AuctionStates.GoingOnce;
            }
            else if (state is GoingTwiceState)
            {
                return AuctionStates.GoingTwice;
            }
            else if (state is ClosedState)
            {
                return AuctionStates.Closed;
            }
            else
            {
                throw new Exception("Invalid State reached");
            }
        }
    }
}
