#region License, Terms and Author(s)
//
// ELMAH - Error Logging Modules and Handlers for ASP.NET
// Copyright (c) 2004-9 Atif Aziz. All rights reserved.
//
//  Author(s):
//
//      Atif Aziz, http://www.raboof.com
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

//[assembly: Elmah.Scc("$Id: ErrorRssHandler.cs 923 2011-12-23 22:02:10Z azizatif $")]

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElmahCore.Mvc.Xml;
using Microsoft.AspNetCore.Http;

namespace ElmahCore.Mvc.Handlers
{
    #region Imports

	#endregion

    /// <summary>
    /// Renders a XML using the RSS 0.91 vocabulary that displays, at most,
    /// the 15 most recent errors recorded in the error log.
    /// </summary>

    static class ErrorRssHandler
    {
        public static async Task ProcessRequest(HttpContext context, ElmahCore.ErrorLog errorLog, string elmahRoot)
        {
            const int pageSize = 15;
            var entries = new List<ErrorLogEntry>(pageSize);
            var log = errorLog;
            log.GetErrors(0, pageSize, entries);

            var response = context.Response;
            response.ContentType = "application/xml";

            var title = string.Format(@"Error log of {0} on {1}",
                                      log.ApplicationName, Environment.MachineName);


            var link = $"{context.Request.Scheme}://{context.Request.Host}{elmahRoot}";
            var baseUrl = new Uri(link.TrimEnd('/') + "/");

            var items =
                from entry in entries
                let error = entry.Error
                select RssXml.Item(
                    error.Message,
                    "An error of type " + error.Type + " occurred. " + error.Message,
                    error.Time,
                    baseUrl + "detail?id=" + Uri.EscapeDataString(entry.Id));
            
            var rss = RssXml.Rss(title, link, "Log of recent errors", items);

            await response.WriteAsync(XmlText.StripIllegalXmlCharacters(rss.ToString()));
        }
    }
}
