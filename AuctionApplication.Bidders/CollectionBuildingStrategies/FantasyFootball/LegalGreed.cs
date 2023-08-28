using System;
using System.Collections.Generic;
using System.Text;
using AuctionApplication.Bidders.CollectionBuildingStrategies.RulesEnforcement;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.Bidders.CollectionBuildingStrategies.FantasyFootball
{
    public class LegalGreed : ICollectionBuildingStrategy<Player>
    {
        private readonly IRulesEnforcer<Player> _rulesEnforcer;

        public LegalGreed()
        {
            _rulesEnforcer = new FantasyRosterEnforcer();
        }

        public bool WillBid(Player item, decimal bid, decimal budget, List<Player> collection = null)
        {
            if (bid <= budget)
            {
                return _rulesEnforcer.IsAddAllowed(item, collection);
            }

            return false;
        }
    }
}
