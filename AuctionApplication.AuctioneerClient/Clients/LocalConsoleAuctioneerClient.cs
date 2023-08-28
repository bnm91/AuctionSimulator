using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionApplication.AuctioneerClient;
using AuctionApplication.AuctioneerService;
using AuctionApplication.Common.Models;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.AuctioneerClients.Clients
{
    public class LocalConsoleAuctioneerClient<T> : IAuctioneerClient<T> where T : IItem
    {
        private readonly AuctioneerService<T> _auctioneerService;

        public LocalConsoleAuctioneerClient(AuctioneerService<T> auctioneerService)
        {
            _auctioneerService = auctioneerService;
        }

        public async Task<List<T>> GetAvailable()
        {
            return  _auctioneerService.GetAvailable();
        }

        public async Task<List<T>> GetCollection(Guid bidderId)
        {
            return _auctioneerService.GetCollection(bidderId);
        }

        public async Task<AuctionStatus<T>> GetStatus()
        {
            return _auctioneerService.GetStatus();
        }

        public async Task Nominate(Guid bidderId, T nominee)
        {
            var bid = new Bid<T>() { Amount = 0, Item = nominee, BidderId = bidderId };
            _auctioneerService.PlaceBid(bid);
        }

        public async Task RaiseBid(Guid bidderId, Bid<T> bid)
        {
            bid.BidderId = bidderId;
            _auctioneerService.PlaceBid(bid);
        }
    }
}
