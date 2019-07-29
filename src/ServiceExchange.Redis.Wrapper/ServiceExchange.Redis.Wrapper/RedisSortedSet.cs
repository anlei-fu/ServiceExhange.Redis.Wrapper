using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackExchange.Redis.Wrapper
{
    /// <summary>
    /// <see cref="SortedSet{T}"/>
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public class RedisSortedSet<TElement> : RedisComponent, IReadOnlyCollection<TElement>
    {
        internal RedisSortedSet(RedisComponetsProvider provider, string name, ISerializer serializer) : base(provider, name, serializer)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public TElement Min
        {
            get
            {
                var result = _db.SortedSetRangeByRank(Name, 0, 1);

                if (result.Length == 0)
                    return default(TElement);

                return deserialize<TElement>(result[0]);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public TElement Max
        {
            get
            {
                var result = _db.SortedSetRangeByRank(Name, 0, 1, Order.Descending);

                if (result.Length == 0)
                    return default(TElement);

                return deserialize<TElement>(result[0]);
            }
        }
        public bool Add(TElement element)
        {
           return _db.SortedSetAdd(Name, serialize(element), 0,When.NotExists);
        }

        public bool AddOrUpdate(TElement element)
        {
            return _db.SortedSetAdd(Name, serialize(element), 0, When.Always);
        }

        public Task<bool> AddAsync(TElement element)
        {
            return _db.SortedSetAddAsync(Name, serialize(element), 0, When.NotExists);
        }

        public Task<bool> AddOrUpdateAsync(TElement element)
        {
            return _db.SortedSetAddAsync(Name, serialize(element), 0, When.Always);
        }


        /// <summary>
        ///  will  add a element, if element not exists
        /// </summary>
        /// <param name="elemnt"></param>
        /// <returns></returns>
        public bool Contains(TElement elemnt)
        {
            return !_db.SortedSetAdd(Name, serialize(elemnt), 0);
        }

        public async Task<bool> ContainsAsync(TElement elemnt)
        {
            return ! await _db.SortedSetAddAsync(Name, serialize(elemnt), 0)
                              .ConfigureAwait(false);
        }

        public bool Remove(TElement elemrnt)
        {
            return _db.SortedSetRemove(Name, serialize(elemrnt));
        }
        public Task<bool> RemoveAsync(TElement elemrnt)
        {
            return _db.SortedSetRemoveAsync(Name, serialize(elemrnt));
        }

        public double Increment(TElement element,double increment)
        {
          return   _db.SortedSetIncrement(Name, serialize(element), increment);
        }
        public Task<double> IncrementAsync(TElement element, double increment)
        {
            return _db.SortedSetIncrementAsync(Name, serialize(element), increment);
        }

        public Task<double> DecrementAsync(TElement element,double decrement)
        {
            return _db.SortedSetDecrementAsync(Name, serialize(element), decrement);
        }
      
        /// <summary>
        /// 
        /// </summary>
        public int Count => (int)_db.SortedSetLength(Name);

        public void Clear()
        {
            RemoveRangeByRank(0, Count);
        }
        public Task ClearAsync()
        {
           return   RemoveRangeByRankAsync(0, Count);
        }


        public void RemoveRangeByScore(double start,double end)
        {
            _db.SortedSetRemoveRangeByScore(Name, start, end);
        }
        public Task RemoveRangeByScoreAsync(double start, double end)
        {
           return  _db.SortedSetRemoveRangeByScoreAsync(Name, start, end);
        }
        public void RemoveRangeByRank(int start,int end, bool asc=true)
        {
            _db.SortedSetRemoveRangeByRank(Name, start, end);
        }

        public Task RemoveRangeByRankAsync(int start, int end, bool asc = true)
        {
           return _db.SortedSetRemoveRangeByRankAsync(Name, start, end);
        }

        public IEnumerable<TElement> GetRangeByRank(int start,int count, bool asc=true)
        {
            var order = asc ? Order.Ascending : Order.Descending;

            return _db.SortedSetRangeByRank(Name, start, start + count, order)
                      .Cast(x => deserialize<TElement>(x));
        }

        public async Task<IEnumerable<TElement>> GetRangeByRankAsync(int start, int count, bool asc = true)
        {
            var order = asc ? Order.Ascending : Order.Descending;

            var result = await _db.SortedSetRangeByRankAsync(Name, start, start + count, order)
                                 .ConfigureAwait(false);

            return result.Cast(x => deserialize<TElement>(x));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TElement> GetRangeByScore(double start,double end)
        {
            return _db.SortedSetRangeByScore(Name, start, end)
                  .Cast(x => deserialize<TElement>(x));
        }


        public async Task<IEnumerable<TElement>> GetRangeByScoreAsync(double start, double end)
        {
            var result = await _db.SortedSetRangeByScoreAsync(Name, start, end);

            return result.Cast(x => deserialize<TElement>(x));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TElement> GetEnumerator()
        {
          foreach(var item in _db.SortedSetRangeByRank(Name)
                                 .Cast(x=>deserialize<TElement>(x)))
            {
                yield return item;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _db.SortedSetRangeByRank(Name)
                      .Cast(x => deserialize<TElement>(x))
                      .GetEnumerator();
        }
    }
}
