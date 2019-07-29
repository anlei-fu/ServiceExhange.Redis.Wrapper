namespace StackExchange.Redis.Wrapper
{
    public class JsonSerializer : ISerializer
    {
        private JsonSerializer()
        {

        }

        public static readonly JsonSerializer Instance=new JsonSerializer();

        public T Deserizlize<T>(string rawString)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(rawString);
        }

        public string Serialize(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
    }
}
