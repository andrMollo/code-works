using System;
using Xarial.XCad.Base;
using Xarial.XCad.Base.Enums;

namespace CodeWorksLibrary.Helpers
{
    internal class CwLogger : IXLogger
    {
        public void Log(string msg, LoggerMessageSeverity_e severity = LoggerMessageSeverity_e.Information)
        {
            this.Trace(msg, GlobalConfig.LoggerName, severity);
        }
    }
}
