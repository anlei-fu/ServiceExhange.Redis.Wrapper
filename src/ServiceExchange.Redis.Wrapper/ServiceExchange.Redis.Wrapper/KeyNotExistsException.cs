using System;

namespace Stack.Exchange.Redis.Wrapper
{
    public class KeyNotExistsException:Exception
    {
        public KeyNotExistsException(string key):base($"the key ({key}) not exists!")
        {

        }
    }
}
