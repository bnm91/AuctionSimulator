using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AuctionApplication.Common.Models.Items;
using CsvHelper;
using CsvHelper.Configuration;

namespace AuctionApplication.Bidders.ReservationPriceCalculators.NoisyExpert.ExpertReservationPriceProviders.FProsExpertReservationPriceProvider
{
    public class FProsExpertReservationPriceProvider : IExpertReservationPriceProvider
    {
        private readonly int _leagueSize = 0;
        private readonly int _budget = 0;
        private readonly string _ppr = "standard";
        private Dictionary<string, decimal> _valueLookup;

        public FProsExpertReservationPriceProvider(
            int leagueSize,
            int budget,
            string ppr)
        {
            _leagueSize = leagueSize;
            _budget = budget;
            _ppr = ppr;
        }

        public decimal GetReservationPrice(IItem item)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = false
            };

            if (_valueLookup == null)
            {
                _valueLookup = new Dictionary<string, decimal>();
                using var streamReader = File.OpenText($"fpros_values_{_leagueSize}_{_budget}_{_ppr}.csv");
                using var csvReader = new CsvReader(streamReader, csvConfig);

                var values = csvReader.GetRecords<KeyValuePair<string, int>>();
                
                foreach (var value in values)
                {
                    _valueLookup[value.Key] = value.Value;
                }
            }

            return _valueLookup[item.Name];
        }
    }
}
