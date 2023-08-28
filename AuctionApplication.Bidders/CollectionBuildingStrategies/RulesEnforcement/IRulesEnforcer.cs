using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionApplication.Bidders.CollectionBuildingStrategies.RulesEnforcement
{
    public interface IRulesEnforcer<T>
    {
        bool IsAddAllowed(T item, List<T> collection);
    }
}
