using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StackExchange.Redis.Wrapper
{
    public class RedisStack<TElement> :RedisComponent, IReadOnlyCollection<TElement>
    {
        public RedisStack(RedisComponetsProvider provider, string name, ISerializer serializer) 
              : base(provider, name, serializer)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count => (int)_db.ListLength(Name);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TElement> GetEnumerator()
        {
            return _db.ListRange(Name,0, -1)
                      .Cast(x => deserialize<TElement>(x))
                      .GetEnumerator();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _db.ListRange(Name, 0, -1)
                      .Cast(x => deserialize<TElement>(x))
                      .GetEnumerator();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            _db.ListTrim(Name, 0, -1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task ClearAsync()
        {
            return _db.ListTrimAsync(Name, 0, -1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TElement Peek()
        {
            return deserialize<TElement>(_db.ListGetByIndex(Name, 0));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryPeek(out TElement value)
        {
            try
            {
                var result = _db.ListGetByIndex(Name, 0);

                if (result.IsNull)
                {
                    value = default(TElement);
                    return false;
                }

                try
                {
                    value = deserialize<TElement>(result);

                    return true;
                }
                catch
                {
                    value = default(TElement);

                    return false;
                }
            }
            catch
            {
                value = default(TElement);

                return false;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<TElement> PeekAsync()
        {
            var result = await _db.ListGetByIndexAsync(Name, 0);

            return deserialize<TElement>(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TElement Pop()
        {
            return deserialize<TElement>(_db.ListLeftPop(Name, 0));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryPop(out TElement value)
        {
            try
            {
                var result = _db.ListLeftPop(Name);

                if(result.IsNull)
                {
                    value = default(TElement);
                    return false;
                }

                try
                {
                    value = deserialize<TElement>(result);

                    return true;
                }
                catch
                {
                    value = default(TElement);

                    return false;
                }
            }
            catch
            {
                value = default(TElement);

                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<TElement> PopAsync()
        {
            var result = await _db.ListLeftPopAsync(Name);

            return deserialize<TElement>(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Push(TElement item)
        {
            _db.ListLeftPush(Name, serialize(item));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task PushAsync(TElement item)
        {
            return _db.ListLeftPushAsync(Name,serialize(item));
        }



    }
}
