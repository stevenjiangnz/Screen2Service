using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Screen2.Utils
{
    /// <summary>
    /// This class provide helper function for string manipulate
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Toes the XML date string.
        /// </summary>
        /// <param name="inputDate">The input date.</param>
        /// <returns></returns>
        public static string ToXMLDateString(DateTime inputDate)
        {
            string resultDateString = string.Empty;

            if (inputDate != null)
            {
                resultDateString = inputDate.Year.ToString() + "-" + inputDate.Month.ToString() + "-" + inputDate.Day.ToString();
            }

            return resultDateString;
        }

        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
