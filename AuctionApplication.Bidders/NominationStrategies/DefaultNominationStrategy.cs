using System;
using System.Collections.Generic;
using System.Text;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.Bidders.NominationStrategies
{
    public class DefaultNominationStrategy<T> : INominationStrategy<T> where T : IItem
    {
        public T SelectNominee(List<T> available)
        {
            return available[0];
        }

        public bool TrySelectNominee(List<T> available, out T nominee)
        {
            if(available.Count > 0)
            {
                nominee = available[0];
                return true;
            }

            nominee = default;
            return false;
        }
    }
}
