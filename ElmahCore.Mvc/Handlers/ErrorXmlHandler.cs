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

//[assembly: Elmah.Scc("$Id: ErrorXmlHandler.cs 640 2009-06-01 17:22:02Z azizatif $")]

using System.Linq;
using System.Xml;
using Microsoft.AspNetCore.Http;

namespace ElmahCore.Mvc.Handlers
{
    #region Imports

	#endregion

    /// <summary>
    /// Renders an error as an XML document.
    /// </summary>

    static class ErrorXmlHandler
    {
        public static void ProcessRequest(HttpContext context, ElmahCore.ErrorLog errorLog)
        {
            var response = context.Response;
            response.ContentType = "application/xml";

            //
            // Retrieve the ID of the requested error and read it from 
            // the store.
            //

            var errorId = context.Request.Query["id"].FirstOrDefault();

            if (string.IsNullOrEmpty(errorId))
                throw new ApplicationException("Missing error identifier specification.");

            var entry = errorLog.GetError(errorId);

            //
            // Perhaps the error has been deleted from the store? Whatever
            // the reason, pretend it does not exist.
            //

            if (entry == null)
            {
                context.Response.StatusCode = 404;
            }

            //
            // Stream out the error as formatted XML.
            //

            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = true;
            settings.CheckCharacters = false;
            var writer = XmlWriter.Create(response.Body, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("error");
            ErrorXml.Encode(entry?.Error, writer);
            writer.WriteEndElement(/* error */);
            writer.WriteEndDocument();
            writer.Flush();
        }
    }
}