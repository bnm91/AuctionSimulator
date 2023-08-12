using System;
using System.Collections.Generic;
using System.Text;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.Bidders.NominationStrategies
{
    public interface INominationStrategy<T> where T : IItem
    {
        public T SelectNominee(List<T> available);
        public bool TrySelectNominee(List<T> available, out T nominee);
    }
}
