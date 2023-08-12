using System;
using System.Collections.Generic;
using System.Text;
using AuctionApplication.AuctioneerService.StateMachine.States;

namespace AuctionApplication.AuctioneerService.StateMachine
{
    public class AuctionStateContext
    {
        private IState _currentState;

        public AuctionStateContext()
        {
            _currentState = new AwaitingNominationState();
        }

        public void BidProcessed()
        {
            _currentState.BidProcessed(this);
        }

        public void BidMissed()
        {
            _currentState.BidMissed(this);
        }

        public void EndAuction()
        {
            _currentState.EndAuction(this);
        }

        public void SetState(IState state)
        {
            _currentState = state;
        }

        public IState GetState()
        {
            return _currentState;
        }
    }
}
