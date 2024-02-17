using CADBooster.SolidDna;
using CodeWorksLibrary.Helpers;
using System;
using System.IO;
using System.Windows.Forms;
using static CADBooster.SolidDna.SolidWorksEnvironment;

namespace CodeWorksLibrary.Macros.Files
{
    internal static class SaveFile
    {
        #region Private fields

        /// <summary>
        /// The full path of the file that need to be saved
        /// </summary>
        private static string _currentFilePath;

        /// <summary>
        /// The folder of the file that need to be saved
        /// </summary>
        private static string _currentFileFolder;

        /// <summary>
        /// The file name of the file that need to be saved
        /// </summary>
        private static string _currentFileName;

        /// <summary>
        /// The extension of the file that need to be saved
        /// </summary>
        private static string _currentFileExtension;

        #endregion

        #region Public methods

        /// <summary>
        /// Save a copy of the active, or the selected component with its drawing. 
        /// Replace all the selected instances with the new component
        /// </summary>
        public static void MakeIndependentWithDrawingMacro()
        {
            Model model = SolidWorksEnvironment.Application.ActiveModel;

            if (CwValidation.Model3dIsOpen(model) == false)
            {
                CwMessage.OpenAModel();
                return;
            }

            if (model.IsPart)
            {
                // Get the new path
                string pathNewFile = GetNewFilePath(model);

                // Save the file as a copy
                // Update file properties **common**
                // Save drawing **common**
                // Replace drawing reference **common**
                // Replace reference to old part
            }
            else
            {
                // Check whether or not there are selected components
                // If nothing is selected
                    // Save the file as a copy
                    // Update file properties **common**
                    // Save drawing **common**
                    // Replace drawing reference **common**
                    // Replace reference to old part
                // If there are selected components
                    // Get all the selected model
                    // Check that the selection
                    // Save the file as a copy
                    // Update file properties **common**
                    // Save drawing **common**
                    // Replace drawing reference **common**
                    // Replace reference to old part
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Get the full path for the new file
        /// </summary>
        /// <param name="model">The pointer to the active SolidDNA.Model object</param>
        /// <returns>A string with the full path for the new file</returns>
        private static string GetNewFilePath(Model model)
        {
            string output = string.Empty;

            _currentFilePath = model.FilePath;
            _currentFileFolder = Path.GetDirectoryName(_currentFilePath);
            _currentFileExtension = Path.GetExtension(_currentFilePath);

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Title = "Super indipendente - Seleziona nuovo percorso file";
            saveFileDialog.Filter = FileFilter(_currentFileExtension);
            saveFileDialog.InitialDirectory = _currentFileFolder;
            saveFileDialog.OverwritePrompt = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                output = saveFileDialog.FileName;
            }

            MessageBox.Show(output);

            return output;
        }

        /// <summary>
        /// Get the filter string for the SaveFileDialog form
        /// </summary>
        /// <param name="fileExtension">The current file extension</param>
        private static string FileFilter(string fileExtension)
        {
            string output = string.Empty;

            string fileType = string.Empty;

            fileExtension = fileExtension.ToLower();

            if (fileExtension == ".sldprt")
            {
                fileType = "SOLIDWORKS Parts";
            }
            else if (fileExtension == ".sldasm")
            {
                fileType = "SOLIDWORKS Assemblies";
            }

            output = $"{fileType} (*{fileExtension})|*{fileExtension}|Tutti i file (*.*)|*.*";

            return output;
        }

        #endregion

    }
}
