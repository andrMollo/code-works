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

            try
            {
                if (model.IsPart)
                {
                    // Get the new path
                    _currentFilePath = model.FilePath;
                    _currentFileFolder = Path.GetDirectoryName(_currentFilePath);
                    _currentFileExtension = Path.GetExtension(_currentFilePath);

                    string fileFilter = FileFilter(_currentFileExtension);

                    string pathNewFile = GetNewFilePath(
                        "Super indipendente - Seleziona nuovo percorso file",
                        _currentFileFolder,
                        fileFilter,
                        false);

                    if (pathNewFile.IsNullOrEmpty() )
                    {
                        CwMessage.NoValidPath();
                        return;
                    }

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
            catch (Exception ex)
            {
                string errorMessage = $"Errore: {ex.Message} \n {ex.StackTrace}";
                SolidWorksEnvironment.Application.ShowMessageBox(errorMessage, SolidWorksMessageBoxIcon.Stop);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Get the full path for the new file
        /// </summary>
        /// <param name="titleWindow">The string with the title to the SaveFileDialog window</param>
        /// <param name="initialDirectory">The folder where the SaveFileDialog starts</param>
        /// <param name="filter">The filter for the extensions shown in the SaveFileDialog window</param>
        /// <param name="getPdmSerial">True to get the file name from the PDM</param>
        /// <returns>A string with the full path for the new file</returns>
        private static string GetNewFilePath(string titleWindow, string initialDirectory, string filter, bool getPdmSerial)
        {
            string output = string.Empty;

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Title = titleWindow;
            saveFileDialog.InitialDirectory = initialDirectory;
            saveFileDialog.Filter = filter;
            saveFileDialog.OverwritePrompt = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string userInputFileName = saveFileDialog.FileName;

                if (userInputFileName.IsNullOrEmpty())
                {
                    output = string.Empty;

                    return output;
                }

                // Get the folder path
                string folderPath = Path.GetDirectoryName(userInputFileName);
                
                // Remove extension from filename
                string fileNameNoExt = Path.GetFileNameWithoutExtension(userInputFileName);

                // Compose new file name
                string fileName = fileNameNoExt + _currentFileExtension;

                string fileFullPath = Path.Combine(folderPath, fileName);

                output = fileFullPath;
            }

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
