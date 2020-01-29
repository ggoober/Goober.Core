﻿using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace Goober.Core.Services.Implementation
{
    public class CacheProvider : ICacheProvider
    {
        private class EmptyResultClass
        {
        }

        #region fields

        private readonly IMemoryCache _memoryCache;

        #endregion

        #region ctor

        public CacheProvider(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        #endregion

        #region ICacheProvider

        public void Remove(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
        }

        public T Get<T>(string cacheKey, int cacheTimeInMinutes, Func<T> func)
        {
            var cachedObject = _memoryCache.Get(cacheKey);

            if (cachedObject != null)
            {
                if (cachedObject is EmptyResultClass)
                    return default(T);

                return (T)cachedObject;
            }

            var expensiveObject = func();

            var absoluteExpiration = new DateTimeOffset(DateTime.Now.AddMinutes(cacheTimeInMinutes));

            if (expensiveObject == null)
            {
                _memoryCache.Set(key: cacheKey,
                    value: new EmptyResultClass(),
                    absoluteExpiration: absoluteExpiration);

                return default(T);
            }

            _memoryCache.Set(cacheKey, expensiveObject, absoluteExpiration);

            return expensiveObject;
        }

        public async Task<T> GetAsync<T>(string cacheKey, int cacheTimeInMinutes, Func<Task<T>> func)
        {
            var cachedObject = _memoryCache.Get(cacheKey);

            if (cachedObject != null)
            {
                if (cachedObject is EmptyResultClass)
                    return default(T);

                return (T)cachedObject;
            }


            var expensiveObject = await func();

            var absoluteExpiration = new DateTimeOffset(DateTime.Now.AddMinutes(cacheTimeInMinutes));

            if (expensiveObject == null)
            {
                _memoryCache.Set(key: cacheKey,
                    value: new EmptyResultClass(),
                    absoluteExpiration: absoluteExpiration);

                return default(T);
            }

            _memoryCache.Set(cacheKey, expensiveObject, absoluteExpiration);

            return expensiveObject;
        }

        #endregion
    }
}
