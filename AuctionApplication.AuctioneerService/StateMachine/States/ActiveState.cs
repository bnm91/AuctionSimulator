﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionApplication.AuctioneerService.StateMachine.States
{
    public class ActiveState : IState
    {
        public void BidMissed(AuctionStateContext ctx)
        {
            ctx.SetState(new GoingOnceState());
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
