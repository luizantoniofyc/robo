﻿using Bexomex.Robo.Domain;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace Becomex.Robo.Infra.Caching
{
    public class MemoryCache<TEntity> : ICache<TEntity> where TEntity : class
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task AddAsync(string key, TEntity obj, DateTime expirationDate)
        {
            return Task.FromResult(_memoryCache.Set(key, obj, expirationDate));
        }

        public Task<TEntity> GetAsync(string key)
        {
            return Task.FromResult(_memoryCache.Get(key) as TEntity);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
