using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Features;
using Newtonsoft.Json;


public static class SessionExtensions
{
    public static void SetObject(this ISession session, string key, object value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }
    public static Object GetObject<Object>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default(Object) : JsonConvert.DeserializeObject<Object>(value);
    }
}