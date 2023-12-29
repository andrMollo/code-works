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
        #region Public Methods
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

        /// <summary>
        /// Show a message box for the end of the macro with the runtime
        /// </summary>
        /// <param name="ts">The TimeSpan object with the calculated runtime</param>
        public static void MacroCompletedTime(TimeSpan ts)
        {
            string elapsedTIme = ComposeElapsedTime(ts);

            Application.ShowMessageBox($"Macro completed in {elapsedTIme}", SolidWorksMessageBoxIcon.Information);
        }
        
        /// <summary>
        /// Show a message box to open a file
        /// </summary>
        public static void OpenAFile()
        {
            Application.ShowMessageBox("Open a file to run the macro.", SolidWorksMessageBoxIcon.Stop);
        }

        /// <summary>
        /// Show a message box to save the file
        /// </summary>
        public static void SaveFile()
        {
            Application.ShowMessageBox("Save the file to run the macro.", SolidWorksMessageBoxIcon.Stop);
        }

        /// <summary>
        /// Show a message box to open a assembly
        /// </summary>
        public static void OpenAssembly()
        {
            Application.ShowMessageBox("Open an assembly to run the macro.", SolidWorksMessageBoxIcon.Stop);
        }

        /// <summary>
        /// Show a message box to open a drawing
        /// </summary>
        public static void OpenADrawing()
        {
            Application.ShowMessageBox("Open a drawing to run the macro.", SolidWorksMessageBoxIcon.Stop);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Compose a string with the elapsed time in seconds or minutes and seconds
        /// </summary>
        /// <param name="ts">The TimeSpan object</param>
        /// <returns>A string with elapsed message</returns>
        private static string ComposeElapsedTime(TimeSpan ts)
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
        #endregion

    }
}
