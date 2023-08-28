using System;
using System.Collections.Generic;
using System.Text;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.Bidders.CollectionBuildingStrategies.Greedy
{
    public class GreedyCollectionBuildingStrategy<T> : ICollectionBuildingStrategy<T> where T: IItem
    {
        public bool WillBid(T item, decimal bid, decimal budget, List<T> collection = null)
        {
            if(bid <= budget)
            { 
                return true;
            }
            return false;
        }
    }
}
