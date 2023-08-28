using System;
using System.Collections.Generic;
using System.Linq;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.Bidders.CollectionBuildingStrategies.RulesEnforcement
{
    public class FantasyRosterEnforcer : IRulesEnforcer<Player>
    {
        // TODO: this ultimately shouldn't have to be hard coded it should be configurable
        private readonly int ROSTER_SIZE = 14;
        private readonly Dictionary<string, int> _rosterMins = new Dictionary<string, int>()
        {
            { "QB", 1 },
            { "RB", 2 },
            { "WR", 2 },
            { "TE", 1 },
            { "D/ST", 1 },
            { "K", 1 }
        };
        private readonly Dictionary<string, int> _rosterMaxes = new Dictionary<string, int>()
        {
            { "QB", 4 },
            { "RB", 8 },
            { "WR", 8 },
            { "TE", 4 },
            { "D/ST", 3 },
            { "K", 3 }
        };

        private readonly Dictionary<Player, bool> _cache;

        public FantasyRosterEnforcer()
        {
            _cache = new Dictionary<Player, bool>();
        }

        public bool IsAddAllowed(Player item, List<Player> collection)
        {
            if (_cache.TryGetValue(item, out bool isAllowed))
            {
                return isAllowed;
            }

            isAllowed = DoesNotViolateRosterMaxes(item, collection)
                && WillHaveSpaceToCompleteRoster(item, collection)
                && WillHaveRosterSpace(item, collection);

            _cache.Add(item, isAllowed);

            return isAllowed;
        }

        private bool DoesNotViolateRosterMaxes(Player item, List<Player> collection)
        {
            if (collection != null)
            {
                foreach (var rosterSlot in _rosterMaxes.Keys)
                {
                    var max = _rosterMaxes[rosterSlot];
                    var collected = collection.ToList().Where(x => x?.Position == rosterSlot).Count();
                    if (item.Position == rosterSlot)
                        collected++;

                    if (collected >= max)
                        return false;
                }
            }

            return true;
        }

        private bool WillHaveSpaceToCompleteRoster(Player item, List<Player> collection)
        {
            if (collection != null)
            {
                int requiredSpots = _rosterMins.Select(x => x.Value).Sum();
                int unfulfilledMinimums = 0;
                foreach (var rosterSlot in _rosterMins.Keys)
                {
                    var required = _rosterMins[rosterSlot];
                    var collected = collection.ToList().Where(x => x?.Position == rosterSlot).Count(); // TODO: debug why x.Position throws NRE sometimes w/o tolist
                    if (item.Position == rosterSlot)
                        collected++;

                    unfulfilledMinimums += (required - collected);
                }

                int availableSlots = _rosterSize - collection.Count;
                return availableSlots > unfulfilledMinimums;
            }
            return true;
        }

        private bool WillHaveRosterSpace(Player item, List<Player> collection)
        {
            return ROSTER_SIZE >= (collection == null ? 1 : collection.Count + 1);
        }
    }
}
