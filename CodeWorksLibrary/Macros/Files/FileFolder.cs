using CADBooster.SolidDna;
using CodeWorksLibrary.Helpers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CodeWorksLibrary.Macros.Files
{
    internal class FileFolder
    {
        /// <summary>
        /// Open the folder containing the selected file
        /// </summary>
        public static void OpenFolderMacro()
        {
            Model model = SolidWorksEnvironment.Application.ActiveModel;

            if (CwValidation.ModelIsOpen(model) == false)
            {
                CwMessage.OpenAFile();
                return;
            }

            List<Model> selectedModels = CwSelectionManager.GetSelectedModels(model);

            string folderPath = string.Empty;

            // Get the path to the folder to open
            if (selectedModels.Count == 0)
            {
                // Open the current model folder
                folderPath = Path.GetDirectoryName(model.FilePath);
            }
            else
            {
                // Open the folder of the first selected component
                folderPath = Path.GetDirectoryName(selectedModels.First().FilePath);
            }

            Process.Start("explorer.exe", folderPath);
        }
    }
}
