using System;
using System.Collections.Generic;
using System.Text;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.Bidders.CollectionBuildingStrategies
{
    public interface ICollectionBuildingStrategy<T> where T: IItem
    {
        public bool WillBid(T item, decimal bid, decimal budget, List<T> collection = null);
    }
}
