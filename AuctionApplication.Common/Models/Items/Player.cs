using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionApplication.Common.Models.Items
{
    public class Player : IItem
    {
        public string Name { get; set; }
        public string Position { get; set; }
    }
}
