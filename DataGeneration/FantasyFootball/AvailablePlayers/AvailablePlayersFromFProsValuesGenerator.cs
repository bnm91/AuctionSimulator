using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using AuctionApplication.Common.Models.Items;
using CsvHelper;
using CsvHelper.Configuration;

namespace DataGeneration.FantasyFootball.AvailablePlayers
{
    public class AvailablePlayersFromFProsValuesGenerator
    {
        public void Generate(string inputFilePath, string outputFilePath)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };

            if (File.Exists(inputFilePath))
            {
                using var streamReader = new StreamReader(inputFilePath);
                using var csvReader = new CsvReader(streamReader, csvConfig);

                var values = csvReader.GetRecords<PlayerData>();

                using var streamWriter = new StreamWriter(outputFilePath);
                using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    csvWriter.WriteRecords(values.Select(x => ParsePlayer(x.Name)));
                };
            }
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

        public class PlayerData
        {
            public string Name { get; set; }
            public string Price { get; set; }
        }
    }
}
