using System;
using System.Collections.Generic;
using System.Text;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.Common.Models
{
    public class AuctionStatus<T> where T: IItem
    {
        public AuctionStates AuctionState { get; set; } //TODO refactor into state machine pattern
        //public Guid? WinnerId { get; set; }
        public Bid<T> WinningBid { get; set; } = null; //TODO: should this be called current and not winning
    }
}
