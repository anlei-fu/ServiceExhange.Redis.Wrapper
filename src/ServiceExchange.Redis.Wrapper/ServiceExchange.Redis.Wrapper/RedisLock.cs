using System;

namespace StackExchange.Redis.Wrapper
{

    /// <summary>
    /// redis distribute lock
    /// 
    /// if take successfully,must do release
    /// e.g
    /// 
    /// if(lock.take("some token"))
    /// {
    /// try{
    ///   // your bussiness
    /// }
    /// finally{
    ///   lock.release("some token")
    /// }
    /// }
    /// 
    /// 
    /// </summary>
    public class RedisLock : RedisComponent
    {
        internal RedisLock(RedisComponetsProvider provider, string name) : base(provider, name)
        {
        }

        /// <summary>
        /// try hold the lock
        /// </summary>
        /// <param name="token">unique token set to the lock</param>
        /// <param name="duration">try hold lock timeout</param>
        /// <returns> success true, failed false </returns>
        public bool Take(string token,TimeSpan duration)
        {
            return _db.LockTake(Name, token, duration);
        }
        /// <summary>
        /// release the  lock have got,
        /// </summary>
        /// <param name="token">when hold the lock ,set to it  </param>
        /// <returns></returns>
        public bool Release(string token)
        {
            return _db.LockRelease(Name, token);
        }
        /// <summary>
        ///  query who is using the lock
        ///   if there's a token-user mapping
        /// </summary>
        /// <returns></returns>
        public string GetCurrentToken()
        {
            return _db.LockQuery(Name);
        }
        /// <summary>
        ///still  don't what is it using for 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public bool Extend(string token,TimeSpan duration)
        {
            return _db.LockExtend(Name,token, duration);
        }
    }
}
