using Newtonsoft.Json;
using VisualAuthentication.DataBaseModels;
using VisualAuthentication.DataModels;

namespace VisualAuthentication.Extensions
{
    public static class SessionExtensions
    {
        public static Key[] Keys(this Session session) 
            => JsonConvert.DeserializeObject<Key[]>(session.SerializedKeys);

        public static Key SecretKey(this Session session) 
            => session.Keys()[session.SecretKeyNumber];

        public static bool IsClose(this Session session)
            => session.CurrentIteration == VASecret.IterationsCount;

        public static bool IsSuccess(this Session session)
            => session.IsClose() && session.FirstErrorIteration == -1;
    }
}