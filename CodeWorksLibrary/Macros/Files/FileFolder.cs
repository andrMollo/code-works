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
        /// A logger model for this add-in
        /// </summary>
        private static CwLogger _logger;
        
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
                _logger.Log("Get the path of the active file");

                // Get the path of the active model
                filePath = model.FilePath;
            }
            else
            {
                _logger.Log($"Get the path of the first selected component: {selectedModels.First().FilePath}");

                // Get the path of the first of the selected Models
                filePath = selectedModels.First().FilePath;
            }

            _logger.Log($"Open the folder: {filePath}");

            Process.Start("explorer.exe", $"/select,\"{filePath}\"");
        }
    }
}
