﻿using System;
using System.Collections.Generic;
using System.Text;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.Bidders.ReservationPriceCalculators
{
    public class RandomReservationPriceCalculator : IReservationPriceCalculator
    {
        public int MaxPrice { get; set; } = int.MaxValue;
        private int _fixedReservationPrice { get; set; }

        public RandomReservationPriceCalculator(int maxPrice)
        {
            MaxPrice = maxPrice;
            _fixedReservationPrice = new Random().Next(0, MaxPrice);
        }

        public decimal GetReservationPrice(IItem item)
        {
            return _fixedReservationPrice;
        }
    }
}
