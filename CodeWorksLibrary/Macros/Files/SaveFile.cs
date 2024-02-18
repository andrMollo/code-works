﻿using CADBooster.SolidDna;
using CodeWorksLibrary.Helpers;
using System;
using System.IO;
using System.Windows.Forms;
using Xarial.XCad.Base.Enums;
using static CADBooster.SolidDna.SolidWorksEnvironment;

namespace CodeWorksLibrary.Macros.Files
{
    internal static class SaveFile
    {
        #region Private fields

        /// <summary>
        /// The full path of the file that need to be saved
        /// </summary>
        private static string _currentFilePath = string.Empty;

        /// <summary>
        /// The folder of the file that need to be saved
        /// </summary>
        private static string _currentFileFolder = string.Empty;

        /// <summary>
        /// The file name of the file that need to be saved
        /// </summary>
        private static string _currentFileName = string.Empty;

        /// <summary>
        /// The extension of the file that need to be saved
        /// </summary>
        private static string _currentFileExtension = string.Empty;

        private static CwLogger _logger = new CwLogger();

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
                    _logger.Log("Save the active part file", LoggerMessageSeverity_e.Information);

                    // Get current file info
                    _currentFilePath = model.FilePath;
                    _currentFileFolder = Path.GetDirectoryName(_currentFilePath);
                    _currentFileName = Path.GetFileName(_currentFilePath);
                    _currentFileExtension = Path.GetExtension(_currentFilePath);

                    // Get the new path
                    string fileFilter = FileFilter(_currentFileExtension);

                    string pathNewFile = GetNewFilePath(
                        "Make independent with drawing - Select new file position",
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
                string userInputFilePath = saveFileDialog.FileName;

                if (userInputFilePath.IsNullOrEmpty())
                {
                    return output;
                }

                // Get the folder path
                string folderPath = Path.GetDirectoryName(userInputFilePath);
                
                // Get the file name without extension
                string fileNameNoExt = string.Empty;

                if (getPdmSerial == false)
                {
                    fileNameNoExt = Path.GetFileNameWithoutExtension(userInputFilePath);
                }
                else
                {
                    fileNameNoExt = ComposePdmFileName(folderPath, _currentFileExtension);
                }

                // Compose new file name
                string fileName = fileNameNoExt + _currentFileExtension;

                string fileFullPath = Path.Combine(folderPath, fileName);

                output = fileFullPath;
            }

            return output;
        }

        /// <summary>
        /// Compose the file name according to the PDM schema
        /// </summary>
        /// <param name="path">A path to a folder</param>
        /// <param name="extension">The extension for the type of SolidWorks file</param>
        ///<returns>The file name without extension</returns>
        private static string ComposePdmFileName(string path, string extension)
        {
            string output = string.Empty;

            // TODO Implement method

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
