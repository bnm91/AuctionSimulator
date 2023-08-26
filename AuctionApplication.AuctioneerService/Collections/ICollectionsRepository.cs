using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AuctionApplication.AuctioneerService.Collections
{
    public interface ICollectionsRepository<T>
    {
        void Add(Guid bidderId, T item);
        void DeleteCollection(Guid bidderId);
        List<T> Read(Guid bidderId);
        ConcurrentDictionary<Guid, List<T>> ReadAll();
        void RemoveFromCollection(Guid bidderId, T item);
    }
}