using CADBooster.SolidDna;
using CodeWorksLibrary.Helpers;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Runtime.InteropServices;

namespace CodeWorksLibrary.Macros.Files
{
    internal static class CloseNoActive
    {
        /// <summary>
        /// A logger model for this add-in
        /// </summary>
        private static CwLogger _logger;

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

            _logger.Log("Loop through all the windows to close them");
            // Loop through all the windows to close them
            foreach (object window in vDocWindows)
            {
                ModelWindow swDocWin = (ModelWindow)window;

                ModelDoc2 swRefModel = swDocWin.ModelDoc;

                if (swRefModel != swModel)
                {
                    _logger.Log($"Try to close the file: {swRefModel.GetPathName()}");
                    if (swRefModel.GetSaveFlag())
                    {
                        _logger.Log($"File needs saving");

                        // Display the close confirmation dialog for unsaved files
                        AddIn.SwApp.ActivateDoc3(swRefModel.GetTitle(), false, (int)swRebuildOnActivation_e.swDontRebuildActiveDoc, 0);

                        int WM_COMMAND = 0x111;
                        int CMD_FileClose = 57602;

                        _logger.Log($"Send closing message");
                        SendMessage(swFrame.GetHWnd(), WM_COMMAND, CMD_FileClose, IntPtr.Zero);
                    }
                    else
                    {
                        _logger.Log($"Close the file");
                        AddIn.SwApp.CloseDoc(swDocWin.ModelDoc.GetTitle());
                    }
                }
            }

            // Active back the original file
            _logger.Log($"Activate the original model");
            AddIn.SwApp.ActivateDoc3(swModel.GetTitle(), true, (int)swRebuildOnActivation_e.swUserDecision, 0);
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(long hWnd, int Msg, int wParam, IntPtr lParam);
    }
}
