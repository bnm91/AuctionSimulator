using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionApplication.AuctioneerService.StateMachine.States
{
    public class GoingOnceState : IState
    {
        public void BidMissed(AuctionStateContext ctx)
        {
            ctx.SetState(new GoingTwiceState());
        }

        public void BidProcessed(AuctionStateContext ctx)
        {
            ctx.SetState(new ActiveState());
        }

        public void EndAuction(AuctionStateContext ctx)
        {
            ctx.SetState(new ClosedState());
        }
    }
}
