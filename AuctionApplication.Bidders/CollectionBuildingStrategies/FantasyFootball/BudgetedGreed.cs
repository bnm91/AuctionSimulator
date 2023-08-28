using System;
using System.Collections.Generic;
using System.Text;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.Bidders.CollectionBuildingStrategies.FantasyFootball
{
    public class BudgetedGreed : ICollectionBuildingStrategy<Player>
    {
        public bool WillBid(Player item, decimal bid, decimal budget, List<Player> collection = null)
        {
            throw new NotImplementedException();
        }
    }
}
