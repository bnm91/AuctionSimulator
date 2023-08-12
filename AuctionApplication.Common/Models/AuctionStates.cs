using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionApplication.Common.Models
{
    public enum AuctionStates
    {
        Closed,
        Nomination,
        AwaitingNomination,
        Active,
        GoingOnce,
        GoingTwice
    }
}
