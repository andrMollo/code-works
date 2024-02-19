using CADBooster.SolidDna;
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
        /// The full path of the file that need to be copied
        /// </summary>
        private static string _oldFilePath = string.Empty;

        /// <summary>
        /// The folder of the file that need to be copied
        /// </summary>
        private static string _oldFileFolder = string.Empty;

        /// <summary>
        /// The file name of the file that need to be copied
        /// </summary>
        private static string _oldFileName = string.Empty;

        /// <summary>
        /// The extension of the file that need to be copied
        /// </summary>
        private static string _oldFileExtension = string.Empty;

        /// <summary>
        /// The SolidDNA ModelType of the file that need to be copied
        /// </summary>
        private static ModelType _oldModelType = ModelType.None;

        /// <summary>
        /// A logger model for this add-in
        /// </summary>
        private static CwLogger _logger = new CwLogger();

        #endregion

        #region Public methods

        /// <summary>
        /// Save a copy of the active, or the selected component with its drawing. 
        /// Replace all the selected instances with the new component
        /// </summary>
        /// <param name="usePdmSerialNbr">True to get the filename from PDM</param>
        public static void MakeIndependentWithDrawingMacro(bool usePdmSerialNbr)
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

                    // Get old file info
                    _oldFilePath = model.FilePath;
                    _oldFileFolder = Path.GetDirectoryName(_oldFilePath);
                    _oldFileName = Path.GetFileName(_oldFilePath);
                    _oldFileExtension = Path.GetExtension(_oldFilePath);
                    _oldModelType = model.ModelType;

                    // Get the new path
                    string fileFilter = FileFilter(_oldFileExtension);

                    string pathNewFile = GetNewFilePath(
                        "Make independent with drawing - Select new file position",
                        _oldFileFolder,
                        fileFilter,
                        usePdmSerialNbr);

                    // Exists the method of the user left the filename empty
                    if (pathNewFile.IsNullOrEmpty() )
                    {
                        CwMessage.NoValidPath();
                        return;
                    }

                    // Check if a file already exists
                    if (File.Exists(pathNewFile))
                    {
                        CwMessage.FileAlreadyExists();
                        return;
                    }

                    // Save the file as a copy
                    ModelSaveResult saveResult = model.SaveAs(
                        pathNewFile,
                        options: SaveAsOptions.Silent | SaveAsOptions.Copy
                        );

                    if (saveResult.Successful == false )
                    {
                        CwMessage.FailToSaveFile();
                    }

                    // Close the old file
                    SolidWorksEnvironment.Application.CloseFile(_oldFilePath);

                    // Open the new file
                    SolidWorksEnvironment.Application.OpenFile(pathNewFile, OpenDocumentOptions.Silent);

                    // Save drawing
                    string pathToNewDrawing = SaveNewDrawing(_oldFilePath, pathNewFile);

                    // Replace drawing reference **common**
                    // Update file properties **common**
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
                string errorMessage = $"Errore: {ex.Message}";
                SolidWorksEnvironment.Application.ShowMessageBox(errorMessage, SolidWorksMessageBoxIcon.Stop);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Make a copy of a drawing and replace its reference
        /// </summary>
        /// <param name="oldFilePath">The full path to the old component</param>
        /// <param name="newFilePath">The full path to the new component</param>
        /// <returns>The full path to the new drawing</returns>
        private static string SaveNewDrawing(string oldFilePath, string newFilePath)
        {
            string output = string.Empty;

            // Change component extension to the drawing one
            string oldDrawingPath = Path.ChangeExtension(oldFilePath, "SLDDRW");

            string newDrawingPath = Path.ChangeExtension(newFilePath, "SLDDRW");

            if (File.Exists(newDrawingPath))
            {
                throw new Exception("A drawing already exists in the directory");
            }

            // Create new drawing only if the old file already has a drawing
            if ( File.Exists(oldDrawingPath))
            {
                File.Copy(oldDrawingPath, newDrawingPath);

                // Try to remove the read-only flag from the new file
                FileAttributes fileAttributes = File.GetAttributes(newDrawingPath);

                fileAttributes &= ~FileAttributes.ReadOnly;

                File.SetAttributes(newDrawingPath, fileAttributes);
            }

            return output;
        }

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

            if (getPdmSerial)
            {
                saveFileDialog.FileName = "DO NO CHANGE";
            }

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
                    fileNameNoExt = ComposePdmFileName(folderPath, _oldFileExtension);
                }

                // Compose new file name
                string fileName = fileNameNoExt + _oldFileExtension;

                string fileFullPath = Path.Combine(folderPath, fileName);

                output = fileFullPath;
            }

            return output;
        }

        /// <summary>
        /// Compose the file name, without extension, according to the PDM schema
        /// </summary>
        /// <param name="path">A path to a folder</param>
        /// <param name="extension">The extension for the type of SolidWorks file</param>
        ///<returns>The file name without extension</returns>
        private static string ComposePdmFileName(string path, string extension)
        {
            string output = string.Empty;

            string serialNumber = CwPdmManager.GetPdmSerialNumber(path.ToLower(), _oldModelType);

            if ((path.ToLower()).StartsWith(GlobalConfig.LibraryRootFolder))
            {
                output = serialNumber;
            }
            else
            {
                string projectNumber = CwPdmManager.GetProjectNumber(path);

                output = projectNumber + "-" + serialNumber;
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
