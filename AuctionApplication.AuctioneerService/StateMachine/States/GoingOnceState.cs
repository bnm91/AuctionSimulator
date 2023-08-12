using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AuctionApplication.AuctioneerService.StateMachine.States
{
    public class GoingOnceState : IState
    {
        public GoingOnceState()
        {
            Console.WriteLine("Going Once...");
            Thread.Sleep(1000);
        }

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
