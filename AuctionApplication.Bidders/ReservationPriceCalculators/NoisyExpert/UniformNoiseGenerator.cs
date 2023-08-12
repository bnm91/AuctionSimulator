using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionApplication.Bidders.ReservationPriceCalculators
{
    public class UniformNoiseGenerator : INoiseGenerator
    {
        private readonly int _range;

        public UniformNoiseGenerator(int range = 5)
        {
            _range = range;
        }

        public decimal Generate()
        {
            return new Random().Next(-_range, _range);
        }
    }
}
