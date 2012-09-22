//
// LX.EasyWeb.XmlRpc.Parser.DateTime8601Parser.cs
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
using System.Globalization;
using System.Text.RegularExpressions;

namespace LX.EasyWeb.XmlRpc.Parser
{
    class DateTime8601Parser : AtomicParser
    {
        static Regex dateTime8601Regex =
            new Regex(@"(((?<year>\d{4})-(?<month>\d{2})-(?<day>\d{2}))|((?<year>\d{4})(?<month>\d{2})(?<day>\d{2})))"
            + @"T"
            + @"(((?<hour>\d{2}):(?<minute>\d{2}):(?<second>\d{2}))|((?<hour>\d{2})(?<minute>\d{2})(?<second>\d{2})))"
            + @"(?<tz>$|Z|([+-]\d{2}:?(\d{2})?))");

        protected override Object Parse(String s)
        {
            DateTime result;
            if (!TryParseDateTime8601(s, out result))
            {
                result = DateTime.MinValue;
            }
            return result;
        }

        private static Boolean TryParseDateTime8601(String date, out DateTime result)
        {
            result = DateTime.MinValue;
            var m = dateTime8601Regex.Match(date);

            if (m == null)
                return false;

            String normalized = m.Groups["year"].Value + m.Groups["month"].Value + m.Groups["day"].Value
                + "T"
                + m.Groups["hour"].Value + m.Groups["minute"].Value + m.Groups["second"].Value
                + m.Groups["tz"].Value;
            var formats = new String[]
            {
                "yyyyMMdd'T'HHmmss",
                "yyyyMMdd'T'HHmmss'Z'",
                "yyyyMMdd'T'HHmmsszzz",
                "yyyyMMdd'T'HHmmsszz",
            };
            // Compact Framework doesn't support TryParseExact()
            try
            {
                result = DateTime.ParseExact(normalized, formats, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
