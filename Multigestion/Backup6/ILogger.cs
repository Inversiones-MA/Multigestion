using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiOperacion
{
    class ILogger : EventLog
    {
        public void Debug(string text)
        {
            EventLog.WriteEntry("ProgramaOthers", text, EventLogEntryType.Information);
        }

        public void Warn(string text)
        {
            EventLog.WriteEntry("ProgramaOthers", text, EventLogEntryType.Warning);
        }

        public void Error(string text)
        {
            EventLog.WriteEntry("ProgramaOthers", text, EventLogEntryType.Error);
        }

        public void Error(string text, Exception ex)
        {
            Error(text);
            Error(ex.StackTrace);
        }
    }
}
