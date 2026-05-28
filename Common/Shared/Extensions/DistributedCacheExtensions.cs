using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Shared.Extensions
{
    public static class DistributedCacheExtensions
    {
        /// <summary>
        /// Generic method for saving data in cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="recordId"></param>
        /// <param name="data"></param>
        /// <param name="absoluteExpirationTime"></param>
        /// <returns></returns>
        public static async Task SetRecordAsync<T>(this IDistributedCache cache, string recordId, T data, TimeSpan? absoluteExpirationTime = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpirationTime
            };

            var jsonData = JsonSerializer.Serialize(data);

            await cache.SetStringAsync(recordId, jsonData, options);
        }

        /// <summary>
        /// Generic method for retrieving data from cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public static async Task<T?> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);

            if (jsonData is null) 
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
