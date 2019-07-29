using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackExchange.Redis.Wrapper
{
    /// <summary>
    /// simple wrapper of <see cref="IDatabase"/>,and serialization style is json
    /// </summary>
    public class RedisStore : RedisComponent
    {
        internal RedisStore(RedisComponetsProvider provider, string name, ISerializer serializer) : base(provider, name, serializer)
        {
        }

        /// <summary>
        /// Use a prefix to classify diffirent  store,avoid key repeat and overlay
        /// </summary>

        public  bool Store(string key,object data,TimeSpan? expiry)
        {
           return  _db.StringSet($"{Name}{key}", serialize(data), expiry,When.Always);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public Task<bool> StoreAsync(string key, object data, TimeSpan? expiry)
        {
            return _db.StringSetAsync($"{Name}{key}", serialize(data), expiry,When.Always);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entries"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool Store(IEnumerable<KeyValuePair<string,object>> entries,TimeSpan? expiry)
        {
            return _db.StringSet(entries.Cast(x=>new KeyValuePair<RedisKey,RedisValue>($"{Name}{x.Key}", serialize(x.Value)))
                                        .ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entries"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public Task<bool> StoreAsync(IEnumerable<KeyValuePair<string, object>> entries, TimeSpan? expiry)
        {
            return _db.StringSetAsync(entries.Cast(x => new KeyValuePair<RedisKey, RedisValue>($"{Name}{x.Key}", serialize(x.Value)))
                                     .ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            return _db.KeyExists($"{Name}{key}");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TimeSpan? GetTimeout(string key)
        {
            return _db.KeyIdleTime($"{Name}{key}");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<TimeSpan?> GetTimeoutAsync(string key)
        {
            return  _db.KeyIdleTimeAsync($"{Name}{key}");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return deserialize<T>(_db.StringGet($"{Name}{key}"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGet<T>(string key,out T value)
        {
            try
            {
               var raw=  _db.StringGet($"{Name}{key}");

                if(raw.IsNull)
                {
                    value = default(T);

                    return false;
                }

                value = deserialize<T>(raw);

                return true;
            }
            catch
            {
                value = default(T);
            }

            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key)
        {
            return deserialize<T>(await _db.StringGetAsync($"{Name}{key}"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        public IEnumerable<T> BulkGet<T>(IEnumerable<string> keys)
        {
            return  _db.StringGet(keys.Cast(x=>(RedisKey)$"{Name}{x}").ToArray())
                       .Cast(y=>deserialize<T>(y));
        }

        public async Task<IEnumerable<T>> BulkGetAsync<T>(IEnumerable<string> keys)
        {
            var result = await _db.StringGetAsync(keys.Cast(x => (RedisKey)$"{Name}{x}").ToArray());

           return result.Cast(y => deserialize<T>(y));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Unstore(string key)
        {
            return _db.KeyDelete(key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<bool> UnstoreAsync(string key)
        {
            return _db.KeyDeleteAsync(key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public long Unstore(IEnumerable<string> keys)
        {
            return  _db.KeyDelete(keys.Cast(x=>(RedisKey)x).ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public Task<long> UnstoreAsync(IEnumerable<string> keys)
        {
            return _db.KeyDeleteAsync(keys.Cast(x => (RedisKey)x).ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="_new"></param>
        /// <returns></returns>
        public bool ResetExpiry(string key,TimeSpan? _new)
        {
            return _db.KeyExpire($"{Name}{key}",_new);
        }

        public Task<bool> ResetExpiryAsync(string key,TimeSpan? _new)
        {
            return _db.KeyExpireAsync($"{Name}{key}", _new);
        }

     
    }
}
