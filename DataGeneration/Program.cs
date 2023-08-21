using System;
using DataGeneration.FantasyFootball.AvailablePlayers;

namespace DataGeneration
{
    class Program
    {
        static void Main(string[] args)
        {
            var playerGenerator = new AvailablePlayersFromFProsValuesGenerator();

            playerGenerator.Generate(
                @"D:\Dev\AuctionApplication\DataGeneration\FantasyFootball\AvailablePlayers\fpros_values_14_200_half.csv",
                @"D:\Dev\AuctionApplication\AuctionApplication.AvailableItems\Providers\FantasyFootball\fpros_players.csv"
                );
        }
    }
}
