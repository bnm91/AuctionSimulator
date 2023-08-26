using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.Bidders.CollectionBuildingStrategies.FantasyFootball
{
    public class StartersFirstStrategy : ICollectionBuildingStrategy<Player>
    {
        private readonly Dictionary<string, int> _starterCounts = new Dictionary<string, int>()
        {
            { "QB", 1 },
            { "RB", 2 },
            { "WR", 2 },
            { "TE", 1 },
            { "D/ST", 1 },
            { "K", 1 }
        };
        

        public bool WillBid(Player item, decimal bid, decimal budget, IEnumerable<Player> collection = null)
        {
            if (collection != null)
            {
                var currentPositionCounts = collection.GroupBy(x => x.Position).Select(group => new
                {
                    Position = group.Key,
                    Count = group.Count()
                }).ToDictionary(x => x.Position, x => x.Count);

                if (HasAnyStartingLineupOpenings(currentPositionCounts)
                    && HasStartingLineupOpeningForPosition(currentPositionCounts, item))
                {
                     return true;
                }

                return false;
            }

            return true;
        }

        private bool HasAnyStartingLineupOpenings(Dictionary<string, int> currentPositionCounts)
        {
            int GetCurrentCount(string position)
            {
                if(currentPositionCounts.TryGetValue(position, out int count))
                {
                    return count;
                }
                return 0;
            };

            return _starterCounts.Any(x => x.Value > GetCurrentCount(x.Key));
        }

        private bool HasStartingLineupOpeningForPosition(Dictionary<string, int> currentPositionCounts, Player player)
        {
            if (!currentPositionCounts.ContainsKey(player.Position))
                return true;
            
            return _starterCounts[player.Position] > currentPositionCounts[player.Position];
        }
    }
}
