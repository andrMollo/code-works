using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CADBooster.SolidDna.SolidWorksEnvironment;

namespace CodeWorksLibrary.Macros.Export
{
    internal class FastPrintMacro
    {
        /// <summary>
        /// Print all the sheet in the current drawing to the default printer
        /// </summary>
        public static void FastPrint()
        {
            Application.ShowMessageBox("Fast print!");
        }
    }
}
