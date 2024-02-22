using CADBooster.SolidDna;
using CodeWorksLibrary.Helpers;
using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// The SolidDNA Model object for the newly created componente
        /// </summary>
        private static Model _newModel = null;

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
        /// <param name="replaceInstances">True to replace all the instances of the old component with the new one in the current SolidWorks session</param>
        public static void MakeIndependentWithDrawingMacro(bool usePdmSerialNbr, bool replaceInstances)
        {
            Model model = SolidWorksEnvironment.Application.ActiveModel;

            if (CwValidation.Model3dIsOpen(model) == false)
            {
                CwMessage.OpenAModel();
                return;
            }

            try
            {
                // Check whether or not there are selected components
                List<Model> selectedModels = CwSelectionManager.GetSelectedModels(model);

                if (model.IsPart || selectedModels.Count == 1)
                {
                    _logger.Log("Save the active file", LoggerMessageSeverity_e.Information);

                    // Get the new path                  
                    string pathNewFile = GetNewFilePath(
                        model,
                        usePdmSerialNbr,
                        replaceInstances
                        );

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

                    // Save the new component
                    SaveNewComponent(model, pathNewFile, replaceInstances);

                    // Close the old file
                    SolidWorksEnvironment.Application.CloseFile(_oldFilePath);

                    // Open the new file
                    _newModel = SolidWorksEnvironment.Application.OpenFile(pathNewFile, OpenDocumentOptions.Silent);

                    // Save drawing and replace reference
                    string pathToNewDrawing = SaveNewDrawing(_oldFilePath, pathNewFile);

                    // Update file properties
                    NewModelPropertyUpdate(_newModel);

                    // Replace reference to old part
                }
                else if (selectedModels.Count > 1)
                {
                    // Check that all the models in the list are the same
                    string firstModelPath = selectedModels[1].FilePath;

                    bool allSelectedSamePath = selectedModels.All(selModel => selModel.FilePath == firstModelPath);

                    if (allSelectedSamePath)
                    {
                        _logger.Log("Save the first model in the selection", LoggerMessageSeverity_e.Information);

                        // Get the new path                  
                        string pathNewFile = GetNewFilePath(
                            selectedModels.First(),
                            usePdmSerialNbr,
                            replaceInstances
                            );

                        // Exists the method of the user left the filename empty
                        if (pathNewFile.IsNullOrEmpty())
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

                        // Save the first component of the list
                        SaveNewComponent(selectedModels.First(), pathNewFile, replaceInstances);

                        // Save drawing and replace reference
                        string pathToNewDrawing = SaveNewDrawing(_oldFilePath, pathNewFile);

                        // Replace instances of the old component with the new one
                    }
                    else
                    {
                        SolidWorksEnvironment.Application.ShowMessageBox("The selection can't contains different components", SolidWorksMessageBoxIcon.Stop);
                        return;
                    }                  
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
        /// Save the new component
        /// </summary>
        /// <param name="model">The SolidDNA Model object of the part to be saved</param>
        /// <param name="pathNewFile">The full path to the new file</param>
        /// <param name="replaceInstances">True to replace all instances of the model in the current SolidWorks session</param>
        private static void SaveNewComponent(Model model, string pathNewFile, bool replaceInstances)
        {
            if (replaceInstances == false)
            {
                // Save the file as a copy
                ModelSaveResult saveResult = model.SaveAs(
                    pathNewFile,
                    options: SaveAsOptions.Silent | SaveAsOptions.Copy
                    );

                if (saveResult.Successful == false)
                {
                    CwMessage.FailToSaveFile();
                }
            }            
        }

        /// <summary>
        /// Update the custom properties for the newly created files
        /// </summary>
        /// <param name="model">The SolidDNA Model object of the new componente</param>
        private static void NewModelPropertyUpdate(Model model)
        {
            // Delete legacy code
            model.SetCustomProperty("Codice BL", string.Empty);
            
            // Delete revision
            model.SetCustomProperty("Revisione", string.Empty);

            // Update author
            string authorName = CwPdmManager.GetPdmUserName();

            model.SetCustomProperty(GlobalConfig.AuthorPropName, authorName);

            // Set original code
            model.SetCustomProperty("Codice Origine", _oldFileName);

            // * Clean up

            // Update filename prop with formula
            model.SetCustomProperty("Nome File", "$PRP:\"SW-File Name\"");

            // Delete configuration specific properties
            List<string> modelConfigurations = model.ConfigurationNames;

            foreach ( string configurationName in modelConfigurations )
            {
                CustomPropertyEditor customPropertyEditor = model.Extension.CustomPropertyEditor(configurationName);
                List<CustomProperty> customProperties = customPropertyEditor.GetCustomProperties();

                foreach (CustomProperty item in customProperties)
                {
                    item.Delete();
                }
            }
        }

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

                // Replace the drawing reference
                if (SolidWorksEnvironment.Application.UnsafeObject.ReplaceReferencedDocument(newDrawingPath, oldFilePath, newFilePath)
                    == false)
                {
                    SolidWorksEnvironment.Application.ShowMessageBox("Unable to replace the drawing reference", SolidWorksMessageBoxIcon.Warning);
                }

                output = newDrawingPath;
            }

            return output;
        }

        /// <summary>
        /// Get the full path for the new file
        /// </summary>
        /// <param name="model">The SolidDNA Model object of the file to be save</param>
        /// <param name="getPdmSerial">True to get the file name from the PDM</param>
        /// <param name="replaceInstances">True to replace all the instances of the old component with the new one in the current SolidWorks session</param>
        /// <returns>A string with the full path for the new file</returns>
        private static string GetNewFilePath(Model model, bool getPdmSerial, bool replaceInstances)
        {
            string output = string.Empty;

            // Set class fields - Get old file info
            _oldFilePath = model.FilePath;
            _oldFileFolder = Path.GetDirectoryName(_oldFilePath);
            _oldFileName = Path.GetFileName(_oldFilePath);
            _oldFileExtension = Path.GetExtension(_oldFilePath);
            _oldModelType = model.ModelType;

            // Set SaveFileDialog title
            string titleWindow = string.Empty;

            if (replaceInstances)
            {
                titleWindow = "Save with drawing - Select new file position";
            }
            else
            {
                titleWindow = "Make independent with drawing - Select new file position";
            }

            string filter = FileFilter(_oldFileExtension);

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // FileDialog setup
            saveFileDialog.Title = titleWindow;
            saveFileDialog.InitialDirectory = _oldFileFolder;
            saveFileDialog.Filter = filter;
            saveFileDialog.OverwritePrompt = true;

            // Show a dummy filename if the part number is assigned by PDM
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
