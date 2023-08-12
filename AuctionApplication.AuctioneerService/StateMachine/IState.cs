using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionApplication.AuctioneerService.StateMachine
{
    public interface IState
    {
        void BidProcessed(AuctionStateContext ctx);
        void BidMissed(AuctionStateContext ctx);
        void EndAuction(AuctionStateContext ctx);
    }
}
