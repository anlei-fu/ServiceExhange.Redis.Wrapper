using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StackExchange.Redis.Wrapper
{
    public class RedisQueue<TElement> : RedisComponent,IReadOnlyCollection<TElement>
    {
        public int Count => (int)_db.ListLength(Name);

        internal RedisQueue(RedisComponetsProvider provider, string name) : base(provider, name)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public void Enqueue(TElement element)
        {
            _db.ListRightPush(Name, serialize(element));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public Task EnqueueAsync(TElement element)
        {
           return _db.ListRightPushAsync(Name, serialize(element));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TElement Dequeue()
        {
            return deserialize<TElement>(_db.ListLeftPop(Name));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<TElement> DequeueAsync()
        {
            var result = await  _db.ListLeftPopAsync(Name)
                                   .ConfigureAwait(false);

            return deserialize<TElement>(result);
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
        /// <param name="element"></param>
        /// <returns></returns>
        public bool TryPeek(out TElement element)
        {
            try
            {
                element = deserialize<TElement>(_db.ListGetByIndex(Name, 0));

                return true;
            }
            catch
            {
                element = default(TElement);

                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<TElement> PeekAsync()
        {
            var result = await _db.ListGetByIndexAsync(Name, -1)
                                  .ConfigureAwait(false);

            return deserialize<TElement>(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool TryDequeue(out TElement element)
        {
            try
            {
                var result = _db.ListRightPop(Name);

                if (result.IsNull)
                {
                    element = default(TElement);

                    return false;
                }

                try
                {
                    element = deserialize<TElement>(result);

                    return true;
                }
                catch
                {
                    element = default(TElement);

                    return false;
                }
            }
            catch
            {
                element = default(TElement);

                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TElement> GetEnumerator()
        {
            foreach (var item in _db.ListRange(Name, 0, -1))
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
            return _db.ListRange(Name, 0, -1).GetEnumerator();
        }
    }
}
