using System;
using Xarial.XCad.Base;
using Xarial.XCad.Base.Enums;

namespace CodeWorksLibrary.Helpers
{
    internal class CwLogger : IXLogger
    {
        /// <summary>
        /// Display a text to the output window
        /// </summary>
        /// <param name="msg">The message for the output window</param>
        /// <param name="severity">The severity of the log</param>
        public void Log(string msg, LoggerMessageSeverity_e severity = LoggerMessageSeverity_e.Information)
        {
            this.Trace(msg, GlobalConfig.LoggerName, severity);
        }
    }
}
