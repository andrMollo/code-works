using CADBooster.SolidDna;
using CodeWorksLibrary.Helpers;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
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

            ModelDoc2 swModel = model.UnsafeObject;

            Frame swFrame = (Frame)AddIn.SwApp.Frame();

            // Get the array of the model windows
            object[] vDocWindows = (object[])swFrame.ModelWindows;

            // Loop through all the windows to close them
            foreach (object window in vDocWindows)
            {
                ModelWindow swDocWin = (ModelWindow)window;

                ModelDoc2 swRefModel = swDocWin.ModelDoc;

                if (swRefModel != swModel)
                {
                    if (swRefModel.GetSaveFlag())
                    {
                        // Display the close confirmation dialog for unsaved files
                        AddIn.SwApp.ActivateDoc3(swRefModel.GetTitle(), false, (int)swRebuildOnActivation_e.swDontRebuildActiveDoc, 0);

                        const int WM_COMMAND = 0x111;
                        const int CMD_FileClose = 0x0000B776;

                    }
                }
            }
        }
    }
}
