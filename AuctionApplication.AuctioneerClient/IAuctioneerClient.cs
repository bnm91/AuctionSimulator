using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionApplication.Common.Models;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.AuctioneerClient
{
    public interface IAuctioneerClient<T> where T: IItem
    {
        Task<AuctionStatus<T>> GetStatus();
        Task<List<T>> GetAvailable();
        Task RaiseBid(Guid bidderId, Bid<T> bid);
        Task Nominate(Guid bidderId, T nominee);
    }
}
