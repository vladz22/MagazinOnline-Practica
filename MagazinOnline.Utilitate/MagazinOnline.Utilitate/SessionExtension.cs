using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazinOnline.Utilitate
{
   public static class SessionExtension
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));//set la session
        }
        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            if (value == null)
            {
               return default(T);
            }
            else
            {
               return JsonConvert.DeserializeObject<T>(value);
            }

        }

    }
}
