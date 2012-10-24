using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEP.Common.Utils
{
    public static class Extensions
    {
        public static string ToDebugString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return "{" + 
                string.Join(",", dictionary.Select(kv => kv.Key.ToString() + "=" + 
                    (kv.Value != null ? kv.Value.ToString() : "(null)")).ToArray()) +
                "}";
        }
    }
}
