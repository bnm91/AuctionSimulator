using System;
using System.Collections.Generic;
using System.Text;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.AvailableItems.Providers
{
    public interface IAvailableItemsProvider<IItem>
    {
        public IEnumerable<IItem> GetAvailableItems();
    }
}
