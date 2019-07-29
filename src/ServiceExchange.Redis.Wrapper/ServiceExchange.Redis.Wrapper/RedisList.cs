using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackExchange.Redis.Wrapper
{
    public class RedisList<TElement> : RedisComponent, IList<TElement>, INameFearture
    {
        internal RedisList(RedisComponetsProvider provider, string name, ISerializer serializer) 
            : base(provider, name, serializer)
        {
        }

        public TElement this[int index]
        {
            get
            {
                return deserialize<TElement>(_db.ListGetByIndex(Name,index));
            }
            set
            {
                _db.ListSetByIndex(Name,index,serialize(value));
            }
        }

        public int Count => (int)_db.ListLength(Name);

        public bool IsReadOnly => false;


        public void Add(TElement item)
        {
            _db.ListLeftPush(Name, serialize(item));
        }

        public bool TryAdd(TElement item)
        {
            try
            {
                Add(item);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public Task AddAsync(TElement item)
        {
            return _db.ListLeftPushAsync(Name, serialize(item));
        }

        public long AddRange(IEnumerable<TElement> collection)
        {
            return _db.ListLeftPush(Name,
                                   collection
                                  .Cast( x=>(RedisValue)serialize(x))
                                  .ToArray());
        }
        public  Task<long> AddRangeAsync(IEnumerable<TElement> collection)
        {
            return  _db.ListLeftPushAsync(Name,
                                          collection
                                         .Cast(x => (RedisValue)serialize(x))
                                         .ToArray());
        }
        public TElement[] GetRange(int start,int count)
        {
            return _db.ListRange(Name,start,start+count)
                      .Cast(x=> deserialize<TElement>(x))
                      .ToArray();
        }

        public async Task<TElement[]> GetRangeAsync(int start, int count)
        {
            var result = await _db.ListRangeAsync(Name, start, start + count)
                                  .ConfigureAwait(false);

            return result.Cast(x => deserialize<TElement>(x))
                         .ToArray();
        }


        public void Clear()
        {
            _db.ListTrim(Name, 0, -1);
        }

        public Task ClearAsync()
        {
            return _db.ListTrimAsync(Name, 0, -1);
        }

        /// <summary>
        ///效率比较低,不支持
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(TElement item)
        {
            throw new NotSupportedException("this method is not surppoted!");
        }

        public void CopyTo(TElement[] array, int arrayIndex)
        {
            var ls = GetRange(0, Count);

            Array.Copy(ls, 0, array, arrayIndex, ls.Length);

        }

        public async Task CopyToAsync(TElement[] array,int arrayIndex)
        {

            var ls = await GetRangeAsync(0, Count);

            Array.Copy(ls, 0, array, arrayIndex, ls.Length);

        }

        public IEnumerator<TElement> GetEnumerator()
        {
            foreach (var item in GetRange(0,Count))
            {
                yield return item;
            }
        }

        /// <summary>
        /// 效率比较低,不支持
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(TElement item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// this mehod  redis not surpporte? 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, TElement item)
        {
            throw new NotSupportedException();
        }


        /// <summary>
        /// 效率比较低,不支持
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(TElement item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            _db.ListTrim(Name, index, index+1);
        }

        public Task RemoveAtAsync(int index)
        {
            return _db.ListTrimAsync(Name, index, index+1);
        }
        public void RemoveRange(int index,int count)
        {
             _db.ListTrim(Name, index, index + count);
        }

        public Task RemoveRangeAsync(int index, int count)
        {
            return _db.ListTrimAsync(Name, index,index+count);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            var ls = GetRange(0, Count);

            return ls.GetEnumerator();
        }
    }
}
