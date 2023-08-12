using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionApplication.AuctioneerService.StateMachine.States
{
    public class ClosedState : IState
    {
        public void BidMissed(AuctionStateContext ctx)
        {
            ctx.SetState(new ClosedState());
        }

        public void BidProcessed(AuctionStateContext ctx)
        {
            ctx.SetState(new ClosedState());
        }

        public void EndAuction(AuctionStateContext ctx)
        {
            ctx.SetState(new ClosedState());
        }
    }
}
