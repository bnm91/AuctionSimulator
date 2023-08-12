using System;
using System.Collections.Generic;
using System.Text;
using AuctionApplication.Bidders.CollectionBuildingStrategies;
using AuctionApplication.Bidders.ReservationPriceCalculators;
using AuctionApplication.Common.Models.Items;

namespace AuctionApplication.Bidders.BiddingStrategies
{
    public class BotBiddingStrategy<T> : IBiddingStrategy<T> where T : IItem
    {
        public IReservationPriceCalculator _reservationPriceCalculator;
        public ICollectionBuildingStrategy<T> _collectionBuildingStrategy;

        public BotBiddingStrategy(IReservationPriceCalculator reservationPriceCalculator,
            ICollectionBuildingStrategy<T> collectionBuildingStrategy)
        {
            _reservationPriceCalculator = reservationPriceCalculator;
            _collectionBuildingStrategy = collectionBuildingStrategy;
        }

        public bool WillRaiseBid(T item, decimal currentBid)
        {
            return _reservationPriceCalculator.GetReservationPrice(item) > currentBid
                && _collectionBuildingStrategy.WillBid(item, currentBid, 200);
        }
    }
}
