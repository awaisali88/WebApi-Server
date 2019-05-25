using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class HttpRequestHeaders
    {
        public const string ApiKeyValue = "pnqxdcABJPkKu0IvtjNvtWqD6iNUxqBaxivBrJwzUfNv4Nbq1S";

        public const string RequestUrl = "callback-url";
        public const string ApiKey = "access-token";
        public const string Authorization = "Authorization";
        public const string AntiForgeryTokenHeader = "X-XSRF-TOKEN";
        public const string Origin = "Origin";
        public const string UserAgent = "User-Agent";
        public const string Referer = "Referer";
    }


    public static class EmailFiles
    {
        public const string ConfirmEmail = "EmailTemplates/account-activation.html";
        public const string ResetPassword = "EmailTemplates/reset-password.html";
    }

    public static class EmailSubject
    {
        public const string ConfirmEmail = "Activate Your Account";
        public const string ResetPassword = "Reset Your Password";
    }

    public static class EmailContentKeywords
    {
        public const string ActivateLink = "[[ACTIVATEEMAILLINK]]";
    }
}
