namespace StackExchange.Redis.Wrapper
{
    public  interface ISerializer
    {
        string Serialize(object obj);
        T Deserizlize<T>(string rawString);
    }
}
