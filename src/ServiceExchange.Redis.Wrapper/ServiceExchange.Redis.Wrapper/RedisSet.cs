using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackExchange.Redis.Wrapper
{
    public class RedisSet<TElement> : RedisComponent, IReadOnlyCollection<TElement>
    {
       internal RedisSet(RedisComponetsProvider provider, string name, ISerializer serializer) 
              : base(provider, name, serializer)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count => (int)_db.SetLength(Name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public bool Add(TElement element)
        {
            return _db.SetAdd(Name, serialize(element));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public  Task<bool> AddAsync(TElement element)
        {
            return _db.SetAddAsync(Name, serialize(element));
        }


        public long AddRange(IEnumerable<TElement> collection)
        {
            return _db.SetAdd(Name, collection.Cast(x => (RedisValue)serialize(x)).ToArray());
        }
        public Task<long> AddRangeAsync(IEnumerable<TElement> collection)
        {
            return _db.SetAddAsync(Name, collection.Cast(x => (RedisValue)serialize(x)).ToArray());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool TryAdd(TElement element)
        {
            try
            {
                return _db.SetAdd(Name, serialize(element));
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public Task<bool> TryAddAsync(TElement element)
        {
            try
            {
                return _db.SetAddAsync(Name, serialize(element));
            }
            catch
            {

                return Task.FromResult(false);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool Conatains(TElement element)
        {
            return _db.SetContains(Name, serialize(element));
        }

        public Task<bool> ContainsAsync(TElement element)
        {
            return _db.SetContainsAsync(Name, serialize(element));
        }

        public void Clear()
        {
            _db.SetPop(Name,Count);
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public bool Remove(TElement element)
        {
           return _db.SetRemove(Name, serialize(element));
        }

      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public  Task<bool> RemoveAsync(TElement element)
        {
            return _db.SetRemoveAsync(Name, serialize(element));
        }
        public long RemoveMany(IEnumerable<TElement> collection)
        {
            return _db.SetRemove(Name, collection.Cast(x => (RedisValue)serialize(x)).ToArray());
        }

        public Task<long> RemoveManyAsync(IEnumerable<TElement> collection)
        {
            return _db.SetRemoveAsync(Name, collection.Cast(x => (RedisValue)serialize(x)).ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool TryRemove(TElement element)
        {
            try
            {
                return _db.SetRemove(Name, serialize(element));
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>

        public Task<bool> TryRemoveAsync(TElement element)
        {
            try
            {
                return _db.SetRemoveAsync(Name, serialize(element));
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TElement> GetEnumerator()
        {
            foreach (var item in _db.SetMembers(Name))
            {
                yield return deserialize<TElement>(item);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _db.SetMembers(Name).Cast(x => deserialize<TElement>(x)).GetEnumerator();
        }
    }
}
