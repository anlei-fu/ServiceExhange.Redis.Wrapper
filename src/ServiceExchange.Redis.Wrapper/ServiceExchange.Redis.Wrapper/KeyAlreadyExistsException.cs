using System;

namespace Stack.Exchange.Redis.Wrapper
{
    public class KeyAlreadyExistsException:Exception
    {
        public KeyAlreadyExistsException(string key):base($"the key ({key}) already exists!")
        {
            
        }
    }
}
