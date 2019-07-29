using System;

namespace StackExchange.Redis.Wrapper
{
    public  abstract  class RedisComponent:INameFearture
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="name"></param>
        internal RedisComponent(RedisComponetsProvider provider,string name,ISerializer serializer)
        {
            Name = name??
                throw new ArgumentNullException(nameof(name));

            Provider = provider
                 ?? throw new ArgumentNullException(nameof(provider));

            _serializer = serializer
                 ?? throw new ArgumentNullException(nameof(serializer));
           
        }

        private ISerializer _serializer;


        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 
        /// </summary>
        public RedisComponetsProvider Provider { get; }
        /// <summary>
        /// 
        /// </summary>
        protected IDatabase _db => Provider.DataBase;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected string serialize(object data)
        {
            return _serializer.Serialize(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        protected T deserialize<T>(string json)
        {
            return _serializer.Deserizlize<T>(json);
        }
        
    }
}
