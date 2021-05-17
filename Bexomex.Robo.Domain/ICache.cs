using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bexomex.Robo.Domain
{
    public interface ICache<TEntity> where TEntity : class
    {
        Task<TEntity> GetAsync(string key);
        Task AddAsync(string key, TEntity obj, DateTime expirationDate);
        void Remove(string key);
    }
}
