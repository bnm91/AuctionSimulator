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
            Console.WriteLine("Going twice..."); //TODO: this is for console implementation but must be removed once other implementations exist
            Thread.Sleep(1000);
        }

        public void BidMissed(AuctionStateContext ctx)
        {
            Console.WriteLine("SOLD!"); //TODO: this is for console implementation but must be removed once other implementations exist
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
