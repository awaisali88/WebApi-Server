using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class RequestUrlKeywords
    {
        private const string ConfirmationToken = "[TOKEN]";
        private const string UserId = "[USERID]";


        public static string CreateRequestUrl(this string url, string confirmationToken = "", string userId = "")
        {
            url = url.Replace(ConfirmationToken, confirmationToken);
            url = url.Replace(UserId, userId);

            return url;
        }

    }

    public static class OpenUrls
    {
        public static List<string> WebUrls = new List<string>()
        {
            "/",
            "/css",
            "/images",
            "/js",
            "/vendor",
            "/fonts",
            "/home"
        };

        public static List<string> Urls = new List<string>()
        {
            "/EmailTemplates",
            "/errorlog",
            "/api-doc",
            "/swagger"
        };
    }

    public class AppConstants
    {
        public const string RemoteIp = "rip";
        public const string SessionErrorKey = "ApplicationError";
    }

    public class ApiVersionNumber
    {
        public const string V1 = "1.0";
        public const string V2 = "2.0";

    }
}
