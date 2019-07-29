using System.Threading.Tasks;

namespace StackExchange.Redis.Wrapper
{
    public class RedisCounter:RedisComponent
    {
        internal RedisCounter(RedisComponetsProvider provider, string name, ISerializer serializer) 
                : base(provider, name, serializer)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public double Increment(double step=1)
        {
            return _db.StringIncrement(Name,step);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public Task<double> IncrementAsync(double step = 1)
        {
            return _db.StringIncrementAsync(Name,step);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public double Decrement(double step=1)
        {
            return _db.StringDecrement(Name,step);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public Task<double> DecrementAsync(double step = 1)
        {
            return _db.StringDecrementAsync(Name,step);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetCurrent()
        {
            return   deserialize<double>(_db.StringGet(Name));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetCurrentAsync()
        {
            var result = await _db.StringGetAsync(Name);

            return deserialize<double>(result);
        }

        public bool Reset(double value=0)
        {
            return  _db.StringSet(Name, value);
        }

        public Task<bool> ResetAsync(double value=0)
        {
            return _db.StringSetAsync(Name, value);
        }
    }
}
