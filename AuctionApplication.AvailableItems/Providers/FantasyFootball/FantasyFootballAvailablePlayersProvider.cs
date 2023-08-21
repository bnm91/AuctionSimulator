using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using AuctionApplication.Common.Models.Items;
using CsvHelper;
using CsvHelper.Configuration;

namespace AuctionApplication.AvailableItems.Providers.FantasyFootball
{
    public class FantasyFootballAvailablePlayersProvider : IAvailableItemsProvider<Player>
    {
        public IEnumerable<Player> GetAvailableItems()
        {
            var fileName = @"Providers\FantasyFootball\fpros_players.csv";
            var path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true
            };

            List<Player> availableItems = null;
            if (File.Exists(fileName))
            {
                using var streamReader = new StreamReader(fileName);
                using var csvReader = new CsvReader(streamReader, csvConfig);

                var values = csvReader.GetRecords<PlayerData>();
                availableItems = values.Select(x => new Player() { Name = x.Name, Position = x.Position }).ToList();
            }
            else
            {
                throw new Exception("file not found");
            }

            return availableItems;
        }

        public class PlayerData
        {
            public string Name { get; set; }
            public string Position { get; set; }
        }
    }
}
