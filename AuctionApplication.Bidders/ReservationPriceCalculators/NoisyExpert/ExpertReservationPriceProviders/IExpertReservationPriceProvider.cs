using System;
using System.Collections.Generic;
using System.Text;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.Bidders.ReservationPriceCalculators
{
    public interface IExpertReservationPriceProvider
    {
        public decimal GetReservationPrice(IItem item);
    }
}
