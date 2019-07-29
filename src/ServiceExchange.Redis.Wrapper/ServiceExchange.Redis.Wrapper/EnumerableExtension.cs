using System;
using System.Collections.Generic;

namespace StackExchange.Redis.Wrapper
{
    public static  class EnumerableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="In"></param>
        /// <param name="caster"></param>
        /// <returns></returns>
        public static IEnumerable<TOut> Cast<TIn,TOut>(this IEnumerable<TIn> In, Func<TIn,TOut> caster)
        {
            foreach (var item in In)
            {
                yield return caster(item);
            }
        }
    }
}
