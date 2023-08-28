using System;
using System.Collections.Generic;
using System.Text;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.Bidders.BiddingStrategies
{
    public interface IBiddingStrategy<T> where T : IItem
    {
        public bool WillRaiseBid(T item, decimal currentBid, List<T> collection = null);
    }
}
