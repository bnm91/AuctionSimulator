using System;
using System.Collections.Generic;
using System.Text;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.Bidders.ReservationPriceCalculators
{
    public class DynoRandoFixedReservationPriceCalculator : IReservationPriceCalculator
    {
        public int MaxPrice { get; set; } = int.MaxValue;
        private Dictionary<IItem, int> _lookup;

        public DynoRandoFixedReservationPriceCalculator(int maxPrice)
        {
            MaxPrice = maxPrice;
            _lookup = new Dictionary<IItem, int>();
        }

        public decimal GetReservationPrice(IItem item)
        {
            if(_lookup.TryGetValue(item, out int price))
            {
                return price;
            }
            _lookup[item] = new Random().Next(0, MaxPrice);
            return _lookup[item];
        }
    }
}
