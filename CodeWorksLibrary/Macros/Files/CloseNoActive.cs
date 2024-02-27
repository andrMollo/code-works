using CADBooster.SolidDna;
using CodeWorksLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWorksLibrary.Macros.Files
{
    internal static class CloseNoActive
    {
        /// <summary>
        /// Close all the open documents except the active one
        /// </summary>
        public static void CloseNoActiveMacro()
        {
            Model model = SolidWorksEnvironment.Application.ActiveModel;

            if (CwValidation.ModelIsOpen(model) == false)
            {
                CwMessage.OpenAFile();
                return;
            }
        }
    }
}
