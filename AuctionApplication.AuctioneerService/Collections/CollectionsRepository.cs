using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace AuctionApplication.AuctioneerService.Collections
{
    public class CollectionsRepository<T> : ICollectionsRepository<T>
    {
        public ConcurrentDictionary<Guid, List<T>> _bidderCollections;

        public CollectionsRepository()
        {
            _bidderCollections = new ConcurrentDictionary<Guid, List<T>>(1, 101);
        }

        public ConcurrentDictionary<Guid, List<T>> ReadAll()
        {
            return _bidderCollections;
        }

        public List<T> Read(Guid bidderId)
        {
            if (_bidderCollections.TryGetValue(bidderId, out List<T> collection))
            {
                return collection;
            }
            return null;
        }

        public void Add(Guid bidderId, T item)
        {
            if (_bidderCollections.ContainsKey(bidderId))
            {
                var collection = _bidderCollections[bidderId];
                collection.Add(item);
            }
            else
            {
                if(!_bidderCollections.TryAdd(bidderId, new List<T>()))
                {
                    throw new Exception($"Error adding new collection for bidder {bidderId}");
                }

                _bidderCollections[bidderId].Add(item);
            }
        }

        public void DeleteCollection(Guid bidderId)
        {
            if (_bidderCollections.ContainsKey(bidderId))
            {
                if(!_bidderCollections.TryRemove(bidderId, out List<T> collection))
                {
                    throw new Exception($"Error deleting collection for bidder for {bidderId}");
                }
            }
        }

        public void RemoveFromCollection(Guid bidderId, T item)
        {
            if (_bidderCollections.ContainsKey(bidderId))
            {
                var collection = _bidderCollections[bidderId];
                collection.Remove(item);
            }
        }
    }
}
