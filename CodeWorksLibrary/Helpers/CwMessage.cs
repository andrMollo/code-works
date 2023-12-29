using CADBooster.SolidDna;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CADBooster.SolidDna.SolidWorksEnvironment;

namespace CodeWorksLibrary.Helpers
{
    public static class CwMessage
    {
        /// <summary>
        /// Show a message box for the interruption of the macro
        /// </summary>
        public static void MacroStopped()
        {
            Application.ShowMessageBox("Macro canceled.", CADBooster.SolidDna.SolidWorksMessageBoxIcon.Stop);
        }

        /// <summary>
        /// Show a message box for the end of the macro
        /// </summary>
        public static void MacroCompleted()
        {
            Application.ShowMessageBox("Macro completed.", CADBooster.SolidDna.SolidWorksMessageBoxIcon.Information);
        }

        public static void MacroCompletedTime(TimeSpan ts)
        {
            string elapsedTIme = ComposeElapsedTime(ts);

            Application.ShowMessageBox($"Macro completed in {elapsedTIme}", SolidWorksMessageBoxIcon.Information);
        }

        /// <summary>
        /// Compose a string with the elapsed time in seconds or minutes and seconds
        /// </summary>
        /// <param name="ts">The TimeSpan object</param>
        /// <returns>A string with elapsed message</returns>
        public static string ComposeElapsedTime(TimeSpan ts)
        {
            string elapsed = string.Empty;

            if (ts != null)
            {
                if (ts.TotalSeconds < 60)
                {
                    elapsed = string.Format("{0:0} seconds", ts.TotalSeconds);
                }
                else
                {
                    elapsed = string.Format("{0} minutes and {0:0} seconds", (int)ts.TotalMinutes, ts.Seconds);
                }
            }

            return elapsed;
        }
    }
}
