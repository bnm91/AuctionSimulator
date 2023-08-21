using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
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
        private readonly string _year;
        private readonly string _month;
        private readonly string _day;

        private Dictionary<string, decimal> _valueLookup;

        public FProsExpertReservationPriceProvider(
            int leagueSize,
            int budget,
            string ppr,
            string year = null,
            string month = null,
            string day = null)
        {
            _leagueSize = leagueSize;
            _budget = budget;
            _ppr = ppr;
            _year = year;
            _month = month;
            _day = day;

            BuildValueLookup();
        }

        public decimal GetReservationPrice(IItem item)
        {
            if(_valueLookup.TryGetValue(item.Name, out decimal reservationPrice))
                return reservationPrice;

            return 0;
        }

        private void BuildValueLookup()
        {
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = false
            };

            if (_valueLookup == null)
            {
                _valueLookup = new Dictionary<string, decimal>();
                using var streamReader = File.OpenText(BuildFileName()); //TODO: this should not read every time
                using var csvReader = new CsvReader(streamReader, csvConfig);

                var rawRecords = csvReader.GetRecords<Record>();
                var values = rawRecords.Select(x =>
                {
                    var player = ParsePlayer(x.PlayerInfo);
                    return new
                    {
                        player.Name,
                        x.Price
                    };
                });

                foreach (var value in values)
                {
                    _valueLookup[value.Name] = value.Price;
                }
            }
        }

        private string BuildFileName()
        {
            StringBuilder fileNameParts = new StringBuilder(@"ReservationPriceCalculators\NoisyExpert\ExpertReservationPriceProviders\FProsExpertReservationPriceProvider\");
            fileNameParts.Append($"fpros_values_{_leagueSize}_{_budget}_{_ppr}");

            if (!string.IsNullOrEmpty(_year)
                && !string.IsNullOrEmpty(_month)
                && !string.IsNullOrEmpty(_day))
            {
                fileNameParts.Append($"_{_year}_{_month}_{_day}");
            }

            fileNameParts.Append(".csv");

            return fileNameParts.ToString();
        }

        private Player ParsePlayer(string input)
        {
            string[] splits = input.Split(new char[] { '(', ')', '-', ',' }, StringSplitOptions.RemoveEmptyEntries);


            Player output = new Player()
            {
                Name = splits[0].Trim(),
                Position = splits[2].Trim()
            };

            return output;
        }

        public class Record
        {
            public string PlayerInfo { get; set; }
            public decimal Price { get; set; }
        }
    }
}
