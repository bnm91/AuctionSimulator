using System;
using System.Collections.Generic;
using System.Text;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.Bidders.ReservationPriceCalculators
{
    public class NoisyExpertReservationPriceCalculator : IReservationPriceCalculator
    {
        private readonly IExpertReservationPriceProvider _expertReservationPriceProivder;
        private readonly INoiseGenerator _noiseGenerator;
        private readonly Dictionary<string, decimal> _cache;

        public NoisyExpertReservationPriceCalculator(
            IExpertReservationPriceProvider expertReservationPriceProvider,
            INoiseGenerator noiseGenerator
            )
        {
            _expertReservationPriceProivder = expertReservationPriceProvider;
            _noiseGenerator = noiseGenerator;
            _cache = new Dictionary<string, decimal>();
        }

        public decimal GetReservationPrice(IItem item)
        {
            decimal reservationPrice;
            if (!_cache.TryGetValue(item.Name, out reservationPrice))
            {
                var expertPrice = GetExpertReservationPrice(item);
                var noise = GetNoiseFactor();
                reservationPrice = expertPrice + noise;

                _cache.Add(item.Name, reservationPrice);
                return reservationPrice;
            }
            return reservationPrice;
        }

        private decimal GetExpertReservationPrice(IItem item)
        {
            return _expertReservationPriceProivder.GetReservationPrice(item);
        }

        private decimal GetNoiseFactor()
        {
            return _noiseGenerator.Generate();
        }

        
    }
}
