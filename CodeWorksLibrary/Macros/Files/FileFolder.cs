using CADBooster.SolidDna;
using CodeWorksLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            if (selectedModels.Count == 0)
            {
                // Open the current model folder

            }
            else
            {
                // Open the folder of the first selected component
                 
            }
        }
    }
}
