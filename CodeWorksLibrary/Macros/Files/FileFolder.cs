using CADBooster.SolidDna;
using CodeWorksLibrary.Helpers;
using System.Collections.Generic;
using System.Diagnostics;
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
            string filePath = string.Empty;

            // Get the path to the folder to open
            if (selectedModels.Count == 0)
            {
                // Get the path of the active model
                filePath = model.FilePath;
            }
            else
            {
                // Get the path of the first of the selected Models
                filePath = selectedModels.First().FilePath;
            }

            Process.Start("explorer.exe", $"/select,\"{filePath}\"");
        }
    }
}
