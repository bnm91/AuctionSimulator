using System;
using System.Collections.Generic;
using System.Text;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.Bidders.ReservationPriceCalculators
{
    public class NoisyExpertReservationPriceCalculator : IReservationPriceCalculator
    {
        private IExpertReservationPriceProvider _expertReservationPriceProivder;
        private INoiseGenerator _noiseGenerator;

        public NoisyExpertReservationPriceCalculator(
            IExpertReservationPriceProvider expertReservationPriceProvider,
            INoiseGenerator noiseGenerator
            )
        {
            _expertReservationPriceProivder = expertReservationPriceProvider;
            _noiseGenerator = noiseGenerator;
        }

        public decimal GetReservationPrice(IItem item)
        {
            var expertPrice = GetExpertReservationPrice(item);
            var noise = GetNoiseFactor();

            return expertPrice + noise;
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
