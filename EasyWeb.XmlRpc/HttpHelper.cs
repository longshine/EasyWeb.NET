//
// LX.EasyWeb.XmlRpc.HttpHelper.cs
//
// Authors:
//	Longshine He <longshinehe@users.sourceforge.net>
//
// Copyright (c) 2012 Longshine He
//
// This code is distributed in the hope that it will be useful,
// but WITHOUT WARRANTY OF ANY KIND.
//

using System;
using System.Collections;
using System.Runtime.Remoting.Channels;
using System.Text;

namespace LX.EasyWeb.XmlRpc
{
    /// <summary>
    /// Helper class for HTTP headers.
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// Content-Length Header
        /// </summary>
        public const String ContentLengthHeader = "Content-Length";
        /// <summary>
        /// Content-Encoding Header
        /// </summary>
        public const String ContentEncodingHeader = "Content-Encoding";
        /// <summary>
        /// Accept-Encoding Header
        /// </summary>
        public const String AcceptEncodingHeader = "Accept-Encoding";
        /// <summary>
        /// Authorization Header
        /// </summary>
        public const String AuthorizationHeader = "Authorization";

        /// <summary>
        /// Gets the value of Content-Length header.
        /// </summary>
        /// <param name="requestHeaders">the collection of headers</param>
        /// <returns>the header's value, or null if not found</returns>
        public static String GetContentLength(ITransportHeaders requestHeaders)
        {
            return (String)requestHeaders["Content-Length"];
        }

        /// <summary>
        /// Gets the value of Content-Encoding header.
        /// </summary>
        /// <param name="requestHeaders">the collection of headers</param>
        /// <returns>the header's value, or null if not found</returns>
        public static String GetContentEncoding(ITransportHeaders requestHeaders)
        {
            return (String)requestHeaders["Content-Encoding"];
        }

        /// <summary>
        /// Gets the value of Accept-Encoding header.
        /// </summary>
        /// <param name="requestHeaders">the collection of headers</param>
        /// <returns>the header's value, or null if not found</returns>
        public static String GetAcceptEncoding(ITransportHeaders requestHeaders)
        {
            return (String)requestHeaders["Accept-Encoding"];
        }

        /// <summary>
        /// Gets the value of Authorization header.
        /// </summary>
        /// <param name="requestHeaders">the collection of headers</param>
        /// <returns>the header's value, or null if not found</returns>
        public static String GetAuthorization(ITransportHeaders requestHeaders)
        {
            return (String)requestHeaders["Authorization"];
        }

        /// <summary>
        /// Checks if GZip is accepted in the headers.
        /// </summary>
        public static Boolean IsUsingGzipEncoding(ITransportHeaders requestHeaders)
        {
            return HasGzipEncoding(GetContentEncoding(requestHeaders));
        }

        /// <summary>
        /// Checks if GZip is accepted in the headers.
        /// </summary>
        public static Boolean IsAcceptingGzipEncoding(ITransportHeaders requestHeaders)
        {
            return HasGzipEncoding(GetAcceptEncoding(requestHeaders));
        }

        /// <summary>
        /// Parses Authorization header and stores user info in the config.
        /// </summary>
        public static void ParseAuthorization(XmlRpcHttpRequestConfig config, String line)
        {
            if (String.IsNullOrEmpty(line))
                return;

            line = line.Trim();
            IEnumerator it = line.Split(' ', '\t', '\n', '\r', '\f').GetEnumerator();
            if (!it.MoveNext())
                return;
            if (!String.Equals("basic", (String)it.Current, StringComparison.OrdinalIgnoreCase))
                return;
            if (!it.MoveNext())
                return;

            // TODO handle exception
            Byte[] bytes = Convert.FromBase64String((String)it.Current);
            Encoding enc = String.IsNullOrEmpty(config.BasicEncoding) ? Encoding.UTF8 : Encoding.GetEncoding(config.BasicEncoding);
            String s = enc.GetString(bytes);
            Int32 col = s.IndexOf(':');
            if (col >= 0)
            {
                config.BasicUserName = s.Substring(0, col);
                config.BasicPassword = s.Substring(col + 1);
            }
        }

        /// <summary>
        /// Checks if GZip is accepted in the header.
        /// </summary>
        public static Boolean HasGzipEncoding(String header)
        {
            if (String.IsNullOrEmpty(header))
                return false;

            foreach (String token in header.Split(','))
            {
                String encoding = token.Trim();
                Int32 offset = token.IndexOf(';');
                if (offset >= 0)
                    encoding = token.Substring(0, offset);
                if (String.Equals("gzip", encoding, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}
