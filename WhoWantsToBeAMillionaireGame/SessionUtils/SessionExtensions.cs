using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace WhoWantsToBeAMillionaireGame.SessionUtils;

public static class SessionExtensions
{
    public static void Set<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }

    public static T? Get<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default : JsonConvert.DeserializeObject<T>(value);
    }

    public static bool TryGetValue<T>(this ISession session, string key, [NotNullWhen(true)] out T value)
    {
        var str = session.GetString(key);
        if (!String.IsNullOrEmpty(str))
        {
            var temp = JsonConvert.DeserializeObject<T>(str);
            if (temp != null)
            {
                value = temp;
                return true;
            }
        };
        value = default(T)!;
        
        return false;
    }
}