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
            Console.WriteLine("Going Once..."); //TODO: this is for console implementation but must be removed once other implementations exist
            Thread.Sleep(2500); //TODO not sure if waiting should be a side effect of state machine
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
