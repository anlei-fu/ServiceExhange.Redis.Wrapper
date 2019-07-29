using Stack.Exchange.Redis.Wrapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackExchange.Redis.Wrapper
{
    /// <summary>
    /// <see cref="Name"/> the dictionary name
    /// 
    ///  Redis->Name->Key to match value
    /// 
    /// value  serialized with json style
    /// 
    /// redis raw value all is string ,guess stack exchange lib  also base on string
    /// <see cref="RedisKey"/> and <see cref="RedisValue"/> finally convert to string or from string
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class RedisDictionary<TValue> : RedisComponent, IDictionary<string, TValue>
    {
        public RedisDictionary(RedisComponetsProvider provider, string name, ISerializer serializer)
             : base(provider, name, serializer)
        {
        }

        /// <summary>
        /// accessor to redis
        /// </summary>
        public TValue this[string key]
        {
            get
            {
                var result = _db.HashGet(Name, key);

                if(result.IsNull)
                    throw new KeyNotFoundException(key);

                return deserialize<TValue>(result);
            }
            set
            {
                if (!_db.HashSet(Name, key, serialize(value), When.Always))
                    throw new KeyNotFoundException(key);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<string> Keys => _db.HashKeys(Name).Cast(x => (string)x).ToList();
        /// <summary>
        /// 
        /// </summary>
        public ICollection<TValue> Values => _db.HashValues(Name).Cast(x => deserialize<TValue>(x)).ToList();
        /// <summary>
        /// 
        /// </summary>
        public int Count => (int)_db.HashLength(Name);
        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly => true;

        /// <summary>
        /// 添加之前需要先判断,redis的处理是覆盖而不是报错
        /// 更C# 的dictionary 处理机制上不一致
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, TValue value)
        {
            var result = _db.HashSet(Name, key, serialize(value));

            if (!result)
                throw new Exception();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task AddAsync(string key, TValue value)
        {
            var result = await _db.HashSetAsync(Name, key, serialize(value))
                                  .ConfigureAwait(false);

            //should be strict with c# grammer,or ignore?
            if (!result)
                throw new Exception();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<string, TValue> item)
        {
            if (!_db.HashSet(Name, item.Key, serialize(item.Value), When.NotExists))
                throw new KeyAlreadyExistsException(item.Key);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task AddAsync(KeyValuePair<string, TValue> item)
        {
            if (!await _db.HashSetAsync(Name, item.Key, serialize(item.Value)))
                throw new KeyAlreadyExistsException(item.Key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryAdd(string key, TValue value)
        {
            try
            {
                return _db.HashSet(Name, key, serialize(value), When.NotExists);
            }
            catch
            {
                return false;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddOrUpdate(string key, TValue value)
        {
            _db.HashSet(Name, key, serialize(value), When.Always);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> TryAddAsync(string key, TValue value)
        {
            try
            {
                return await _db.HashSetAsync(Name, key, serialize(value), When.NotExists);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            _db.HashDelete(Name, Keys.Cast(x => (RedisValue)x).ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task CleatAsync()
        {
            return _db.HashDeleteAsync(Name, Keys.Cast(x => (RedisValue)x).ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<string, TValue> item)
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return _db.HashExists(Name, key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<bool> ContainsKeyAsync(string key)
        {
            return _db.HashExistsAsync(Name, key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
        {
            _db.HashGetAll(Name)
               .Cast(x => new KeyValuePair<string, TValue>(x.Name, deserialize<TValue>(x.Value)))
               .ToList()
               .CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        /// <returns></returns>
        public async Task CopyToAsync(KeyValuePair<string, TValue>[] array, int arrayIndex)
        {
            var result = await _db.HashGetAllAsync(Name);

            result.Cast(x => new KeyValuePair<string, TValue>(x.Name, deserialize<TValue>(x.Value)))
                  .ToList()
                  .CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            return _db.HashGetAll(Name)
                      .Cast(x => new KeyValuePair<string, TValue>(x.Name, deserialize<TValue>(x.Value)))
                      .GetEnumerator();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return _db.HashDelete(Name, key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<bool> RemoveAsync(string key)
        {
            return _db.HashDeleteAsync(Name, key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<string, TValue> item)
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string key, out TValue value)
        {
            try
            {
                var result = _db.HashGet(Name, key);

                if (result.IsNull)
                {
                    value = default(TValue);

                    return false;
                }

                try
                {
                    value = deserialize<TValue>(result);

                    return true;
                }
                catch
                {
                    value = default(TValue);

                    return false;
                }
            }
            catch
            {
                value = default(TValue);

                return false;
            }
        }
        /// <summary>
        /// async stream not surppot at this version ,
        /// c# 8.0 will do a update
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _db.HashGetAll(Name)
                      .Cast(x => new KeyValuePair<string, TValue>(x.Name, deserialize<TValue>(x.Value)))
                      .GetEnumerator();
        }


    }
}
