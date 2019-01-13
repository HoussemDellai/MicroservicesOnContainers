using Basket.Api.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace Basket.Api
{
    public class RedisCacheClient
    {
        private static string RedisCacheConnection;

        private static Lazy<ConnectionMultiplexer> _lazyConnection = new Lazy<ConnectionMultiplexer>(
             () => ConnectionMultiplexer.Connect(RedisCacheConnection));

        public static ConnectionMultiplexer Connection => _lazyConnection.Value;

        public RedisCacheClient(IConfiguration configuration)
        {
            RedisCacheConnection = configuration.GetValue<string>("RedisCacheConnection");
        }

        public void AddBasketItem(BasketItem basketItem)
        {
            IDatabase cache = _lazyConnection.Value.GetDatabase();

            var existing = cache.StringGet("basket-cache");

            var items = new List<BasketItem>();

            if (!existing.IsNull)
            { 
                items = JsonConvert.DeserializeObject<List<BasketItem>>(existing);
            }
            
            items.Add(basketItem);

            var basketItems = JsonConvert.SerializeObject(items);

            cache.StringSet("basket-cache", basketItems);

            //_lazyConnection.Value.Dispose();
        }

        public List<BasketItem> GetAllBasketItems()
        {
            IDatabase cache = _lazyConnection.Value.GetDatabase();

            RedisValue redisValue = cache.StringGet("basket-cache");

            // _lazyConnection.Value.Dispose();

            if (redisValue.IsNullOrEmpty) return null;

            var basketItems = JsonConvert.DeserializeObject<List<BasketItem>>(redisValue);

            return basketItems;
        }
    }
}
