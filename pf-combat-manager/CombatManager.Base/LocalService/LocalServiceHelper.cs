using EmbedIO;

namespace CombatManager.LocalService
{
    public static class LocalServiceHelper
    {
        public static string RequestHeader(this IHttpContext con, string header)
        {
            return con.Request.Headers[header];
        }

        public static bool HasRequestHeader(this IHttpContext con, string header)
        {
            return con.Request.Headers[header] != null;
        }

    }
}
