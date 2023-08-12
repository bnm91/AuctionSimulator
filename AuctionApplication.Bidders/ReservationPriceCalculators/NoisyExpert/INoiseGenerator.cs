using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionApplication.Bidders.ReservationPriceCalculators
{
    public interface INoiseGenerator
    {
        public decimal Generate();
    }
}
