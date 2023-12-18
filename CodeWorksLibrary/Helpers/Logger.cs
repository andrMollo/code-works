﻿using System;
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
        /// The name of the log file
        /// </summary>
        internal string LogFileName { get; set; }

        /// <summary>
        /// The path to the log folder
        /// </summary>
        internal string LogFolderPath { get; set; }

        /// <summary>
        /// Write a message to file
        /// </summary>
        /// <param name="message">The string to be written in the log</param>
        internal void WriteLog(string message)
        {
            string logFolder = LogFolderPath;

            string logFileName = LogFileName;

            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }

            string logPath = Path.Combine(logFolder, logFileName);

            using (StreamWriter sw = new StreamWriter(logPath, true))
            {
                sw.WriteLine(message);
            }
        }

        /// <summary>
        /// Write a massage to the log file followed by the current date
        /// </summary>
        /// <param name="message">The string to be written int the log</param>
        internal void WriteLogWithDate(string message)
        {
            string logFolder = LogFolderPath;

            string logFileName = LogFileName;

            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }

            string logPath = Path.Combine(logFolder, logFileName);

            using (StreamWriter sw = new StreamWriter(logPath, true))
            {
                sw.WriteLine($"{message}; {DateTime.Now}");
            }
        }

        /// <summary>
        /// Compose the log fill path
        /// </summary>
        /// <param name="token">The job number / sub folder</param>
        /// <returns>The full path to the log file</returns>
        internal static string ComposeLogPath(string token)
        {
            // The folder of the log file
            string logFolder = GlobalConfig.LogPath;

            // Create the log folder if doesn't exists
            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }

            // Resolve the token to compose the log name
            string logName = $"log_{token}.txt";

            return Path.Combine(logFolder, logName);
        }

        /// <summary>
        /// Check if a log file exist
        /// </summary>
        /// <param name="logPath">The path to the log file</param>
        /// <returns>True if the log file exists</returns>
        internal static bool CheckLogExist(string logPath)
        {
            return false;
        }
    }
}
