using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AuctionApplication.AuctioneerService.StateMachine.States
{
    public class GoingTwiceState : IState
    {
        public GoingTwiceState()
        {
            Console.WriteLine("Going twice...");
            Thread.Sleep(1000);
        }

        public void BidMissed(AuctionStateContext ctx)
        {
            Console.WriteLine("SOLD!");
            ctx.SetState(new AwaitingNominationState());
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
