using System;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.Bidders.ReservationPriceCalculators
{
    public interface IReservationPriceCalculator
    {
        public decimal GetReservationPrice(IItem item);
    }
}
