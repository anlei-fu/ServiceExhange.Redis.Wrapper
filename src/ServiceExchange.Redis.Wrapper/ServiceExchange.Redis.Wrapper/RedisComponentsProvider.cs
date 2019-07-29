namespace StackExchange.Redis.Wrapper
{
    /// <summary>
    /// wrap <see cref="IDatabase"/> to  some common usage datastructure
    /// create data strcutures and provide <see cref="IDatabase"/> to every component
    /// all components's '_db' field reference to <see cref="DataBase"/>
    /// so can change <see cref="DataBase"/>property to change every component created by it
    /// 全部以string 作为key 类型
    /// </summary>
    public class RedisComponetsProvider 
    {
        public RedisComponetsProvider(IDatabase db, string name,ISerializer serializer=null)
        {

        }

        private ISerializer _serizlier;

        internal IDatabase DataBase { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        public void ResetDataBase(IDatabase db)
        {
            DataBase = db;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public RedisDictionary<TValue> GetDictionary<TValue>(string name)
        {
            return new RedisDictionary<TValue>(this,name,_serizlier);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public RedisLock GetLock(string name)
        {
            return new RedisLock(this, name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="withPrefix"></param>
        /// <returns></returns>
        public RedisStore GetStore(string name,bool withPrefix=true)
        {
            return new RedisStore(this, name,_serizlier);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public  RedisList<TElement> GetList<TElement>(string name)
        {
            return new RedisList<TElement>(this, name,_serizlier);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public RedisQueue<TElement> GetQueue<TElement>(string name)
        {
            return new RedisQueue<TElement>(this,name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public RedisStack<TElement> GetStack<TElement>(string name)
        {
            return new RedisStack<TElement>(this, name,_serizlier);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public RedisSet<TElement> GetSet<TElement>(string name)
        {
            return new RedisSet<TElement>(this,name,_serizlier);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public RedisStore GetTopStore(string name)
        {
            return new RedisStore(this,name,_serizlier);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public RedisSortedSet<TElement> GetSortedSet<TElement>(string name)
        {
            return new RedisSortedSet<TElement>(this, name,_serizlier);
        }
    }
}
