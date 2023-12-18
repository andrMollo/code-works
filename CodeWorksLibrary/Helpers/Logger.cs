using System;
using System.IO;

namespace CodeWorksLibrary.Helpers
{
    internal class Logger
    {
        /// <summary>
        /// The full path to the log file
        /// </summary>
        internal string LogPath { get; set; }

        /// <summary>
        /// Write a message to file
        /// </summary>
        /// <param name="message">The string to be written in the log</param>
        internal void WirteLog(string message)
        {
            string logPath = LogPath;

            using (StreamWriter sw = new StreamWriter(logPath, true))
            {
                sw.WriteLine(message);
            }
        }

        /// <summary>
        /// Write a massage to the log file followed by the current date
        /// </summary>
        /// <param name="message">The string to be written int the log</param>
        internal void WirteLogWithDate(string message)
        {
            string logPath = LogPath;

            using(StreamWriter sw = new StreamWriter(logPath, true))
            {
                sw.WriteLine($"{message}; {DateTime.Now}");
            }
        }
    }
}
