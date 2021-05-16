using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace AppEx.Core.Extensions
{
    public static class ExpandoObjectExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expando"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static ExpandoObject Remove(this ExpandoObject expando, string keyName)
        {
            if (((IDictionary<string, object>)(expando as dynamic)).ContainsKey(keyName))
            {
                ((IDictionary<string, object>)(expando as dynamic)).Remove(keyName);
            }

            return expando;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="expando"></param>
        /// <returns></returns>
        public static TTarget ConvertTo<TTarget>(this ExpandoObject expando)
        {
            var json = JsonConvert.SerializeObject(expando);
            var obj = JsonConvert.DeserializeObject<TTarget>(json);
            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static ExpandoObject ToExpando<TSource>(this TSource source)
        {
            var json = JsonConvert.SerializeObject(source);
            dynamic expando = JsonConvert.DeserializeObject<ExpandoObject>(json);
            return expando;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expando"></param>
        /// <returns></returns>
        public static ICollection<string> GetKeys(this ExpandoObject expando)
        {
            var keys = new List<string>();
            if (expando != null)
            {
                var dict = (IDictionary<string, object>)expando;
                return dict.Keys;
            }
            return keys;
        }
    }
}
