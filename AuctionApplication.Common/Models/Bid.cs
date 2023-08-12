using System;
using System.Collections.Generic;
using System.Text;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.Common.Models
{
    public class Bid<T> where T : IItem
    {
        public Guid BidderId { get; set; }
        public decimal Amount { get; set; }
        public T Item { get; set; }
    }
}
