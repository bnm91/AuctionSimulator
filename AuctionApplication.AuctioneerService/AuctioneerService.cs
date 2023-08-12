using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AuctionApplication.Common.Models;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.AuctioneerService
{
    public class AuctioneerService<T> where T : IItem
    {
        private object _lock;

        //NMB TODO: this thing has to be thread safe
        //NMB TOOD: this can't be the only way to determine what's currently up for auction unless the Auctioneer state machine is air tight and threadsafe 
        //      currently we have a race to nominate where they both succeed
        public AuctionStatus<T> AuctionStatus;

        private BlockingCollection<Bid<T>> _incomingBids;
        private readonly List<T> _availableItems;

        public AuctioneerService(List<T> availableItems) 
        {
            _availableItems = availableItems;
            AuctionStatus = new AuctionStatus<T>()
            {
                //WinnerId = null,
                WinningBid = null,
                AuctionState = AuctionStates.AwaitingNomination
            };
            _incomingBids = new BlockingCollection<Bid<T>>();
            _lock = new object();
        }

        public AuctionStatus<T> GetStatus()
        {
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
            if (AuctionStatus.AuctionState == AuctionStates.Active) // TODO: is there anyway to get state management out of here entirely?
            {
                _incomingBids.Add(bid);
            }
            if (AuctionStatus.AuctionState == AuctionStates.AwaitingNomination)
            {
                _incomingBids.Add(bid);
            }
        }

        public void ConsumeBid(Bid<T> bid)
        {
            //Console.WriteLine("bid consumed");
            if (AuctionStatus.AuctionState == AuctionStates.Active)
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
            else if (AuctionStatus.AuctionState == AuctionStates.AwaitingNomination)
            {
                _availableItems.Remove(bid.Item);
                SetCurrentWinningBid(new Tuple<Guid, Bid<T>>(bid.BidderId, bid));

                // TODO: remove/cleanup
                //Temp state management code for testing
                Console.WriteLine($"Now bidding on {bid.Item.Name}");
                AuctionStatus.AuctionState = AuctionStates.Active; //TODO: this class should not manage state
            }
        }

        // TODO: remove this? unused at time of writing
        public void ProcessNomination(Bid<T> bid)
        {
            if(AuctionStatus.AuctionState == AuctionStates.AwaitingNomination)
            {
                lock (_lock)
                {
                    if (AuctionStatus.WinningBid != null && !AuctionStatus.WinningBid.Equals(bid))
                    {
                        //remove from available
                        _availableItems.Remove(bid.Item);

                        //set as current
                        //_currentItem = bid.Item;
                        SetCurrentWinningBid(new Tuple<Guid, Bid<T>>(bid.BidderId, bid)); // TODO: clean up this object -- bidderId is not part of bid
                    }
                    else if (AuctionStatus.WinningBid == null && bid != null)
                    {
                        //remove from available
                        _availableItems.Remove(bid.Item);

                        //set as current
                        SetCurrentWinningBid(new Tuple<Guid, Bid<T>>(bid.BidderId, bid));
                    }
                }
            }

        }

        private void SetCurrentWinningBid(Tuple<Guid, Bid<T>> newWinningBid)
        {
            //_mutex.WaitOne();
            AuctionStatus.WinningBid = new Bid<T>()
            {
                BidderId = newWinningBid.Item1,
                Amount = newWinningBid.Item2.Amount,
                Item = newWinningBid.Item2.Item,
            };
            //_mutex.ReleaseMutex();
        }
    }
}
