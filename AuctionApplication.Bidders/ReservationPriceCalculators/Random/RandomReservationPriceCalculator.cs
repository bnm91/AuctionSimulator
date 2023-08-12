using System;
using System.Collections.Generic;
using System.Text;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.Bidders.ReservationPriceCalculators
{
    public class RandomReservationPriceCalculator : IReservationPriceCalculator
    {
        public int MaxPrice { get; set; } = int.MaxValue;

        public RandomReservationPriceCalculator(int maxPrice)
        {
            MaxPrice = maxPrice;
        }

        public decimal GetReservationPrice(IItem item)
        {
            return new Random().Next(0, MaxPrice);
        }
    }
}
