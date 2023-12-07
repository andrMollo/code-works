using static CADBooster.SolidDna.SolidWorksEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWorksLibrary.Macros.Export
{
    internal class ExportAssyMacro
    {
        internal static void ExportAssembly()
        {
            var model = Application.ActiveModel;
        }
    }
}
