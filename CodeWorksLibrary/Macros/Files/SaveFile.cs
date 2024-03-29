﻿using CADBooster.SolidDna;
using CodeWorksLibrary.Helpers;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Xarial.XCad.Base.Enums;

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
        public static void SaveWithDrawing(bool usePdmSerialNbr, bool replaceInstances)
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

                if (model.IsPart || selectedModels.Count == 0)
                {
                    _logger.Log("Save the active file");

                    // Get the new path                  
                    string pathNewFile = GetNewFilePath(
                        model,
                        usePdmSerialNbr,
                        replaceInstances
                        );

                    // Exists the method of the user left the filename empty
                    if (pathNewFile.IsNullOrEmpty() )
                    {
                        return;
                    }

                    // Save the new component
                    SaveNewComponent(model, pathNewFile, replaceInstances);

                    // Close the old file
                    _logger.Log("Closing the old file");
                    SolidWorksEnvironment.Application.CloseFile(_oldFilePath);

                    // Open the new file
                    _logger.Log("Opening the file");
                    _newModel = SolidWorksEnvironment.Application.OpenFile(pathNewFile, OpenDocumentOptions.Silent);

                    // Save drawing and replace reference
                    string pathToNewDrawing = SaveNewDrawing(_oldFilePath, pathNewFile);

                    // Update file properties
                    ModelPropertyUpdate(_newModel);
                }
                // Process the selected components in the assembly
                else if (selectedModels.Count > 0)
                {
                    _logger.Log("Save the selected components");

                    // Check that all the models in the list are the same
                    string firstModelPath = selectedModels.First().FilePath;

                    bool allSelectedSamePath = selectedModels.All(selModel => selModel.FilePath == firstModelPath);

                    _logger.Log($"Are all selected components the same? {allSelectedSamePath}");

                    if (allSelectedSamePath)
                    {
                        // Get the selection list and suspend it
                        _logger.Log("Suspend the selection list");

                        SelectionMgr selectionMgr = (SelectionMgr)model.UnsafeObject.SelectionManager;

                        selectionMgr.SuspendSelectionList();

                        // Save the first model of the selection
                        _logger.Log("Save the first model in the selection");

                        // Get the new path                  
                        string pathNewFile = GetNewFilePath(
                            selectedModels.First(),
                            usePdmSerialNbr,
                            replaceInstances
                            );

                        // Exists the method of the user left the filename empty
                        if (pathNewFile.IsNullOrEmpty())
                        {
                            return;
                        }

                        // Save the first component of the list
                        SaveNewComponent(selectedModels.First(), pathNewFile, replaceInstances);

                        // Save drawing and replace reference
                        string pathToNewDrawing = SaveNewDrawing(_oldFilePath, pathNewFile);

                        // Restore the selection list
                        selectionMgr.ResumeSelectionList2(false);

                        // Replace instances of the old component with the new one
                        AssemblyDoc assemblyModel = (AssemblyDoc)model.Assembly.UnsafeObject;

                        _logger.Log("Replace selected components");
                        bool replaceResult = assemblyModel.ReplaceComponents2(
                            pathNewFile,
                            selectedModels.First().ActiveConfiguration.Name,
                            false,
                            (int)swReplaceComponentsConfiguration_e.swReplaceComponentsConfiguration_MatchName,
                            true
                            );

                        if (replaceResult == false)
                        {
                            _logger.Log("Replacing unsuccessful.", LoggerMessageSeverity_e.Warning);
                        }

                        // Get the new model from the assembly components
                        _newModel = GetModelByPath(SolidWorksEnvironment.Application.ActiveModel, pathNewFile);

                        // Update property for the new model
                        ModelPropertyUpdate(_newModel);
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

        #region Private methods

        /// <summary>
        /// Get a Model from path by traversing the assembly components
        /// </summary>
        /// <param name="model">The SolidDNA Model to be searched in for a component</param>
        /// <param name="path">The full path to a file</param>
        /// <returns>The SolidDNA Model object for the input location</returns>
        private static Model GetModelByPath(Model model, string path)
        {
            Model output = null;

            IEnumerable<(CADBooster.SolidDna.Component component, int depth)> components = model.Components().ToList();

            foreach (var comp in components)
            {
                Model componentModel = comp.component.AsModel;

                if (componentModel.FilePath == path)
                {
                    output = componentModel;
                }
            }

            return output;
        }

        #endregion

        /// <summary>
        /// Save the new component
        /// </summary>
        /// <param name="model">The SolidDNA Model object of the part to be saved</param>
        /// <param name="pathNewFile">The full path to the new file</param>
        /// <param name="replaceInstances">True to replace all instances of the model in the current SolidWorks session</param>
        private static void SaveNewComponent(Model model, string pathNewFile, bool replaceInstances)
        {
            ModelSaveResult result = null;

            _logger.Log("Saving new file");
            
            if (replaceInstances == false)
            {
                _logger.Log("Saving as copy");
                
                // Save the file as a copy
                result = model.SaveAs(
                    pathNewFile,
                    options: SaveAsOptions.Silent | SaveAsOptions.Copy
                    );
            }
            else
            {
                _logger.Log("Save as");

                // Save as the file
                result = model.SaveAs(
                    pathNewFile,
                    options: SaveAsOptions.Silent
                    );
            }

            _logger.Log($"Saving result: {result}");

            if (result.Successful == false)
            {
                _logger.Log("Saving failed", LoggerMessageSeverity_e.Error);

                CwMessage.FailToSaveFile();
            }
        }

        /// <summary>
        /// Update the custom properties for the model
        /// </summary>
        /// <param name="model">The SolidDNA Model object of the new componente</param>
        private static void ModelPropertyUpdate(Model model)
        {
            _logger.Log("Update custom properties for the new model");

            // Delete legacy code
            _logger.Log("Delete old code");
            model.SetCustomProperty("Codice BL", string.Empty);

            // Delete revision
            _logger.Log("Delete revision");
            model.SetCustomProperty("Revisione", string.Empty);

            // Update author
            string authorName = CwPdmManager.GetPdmUserName();

            _logger.Log($"Update author: {authorName}");
            model.SetCustomProperty(GlobalConfig.AuthorPropName, authorName);

            // Set original code
            _logger.Log($"Set originale code: {_oldFileName}");
            model.SetCustomProperty("Codice Origine", _oldFileName);

            // * Clean up

            // Update filename prop with formula
            _logger.Log("Update filename with formula");
            model.SetCustomProperty("Nome File", "$PRP:\"SW-File Name\"");

            // Delete configuration specific properties
            _logger.Log("Delete configuration custom properties");
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

            _logger.Log("Saving the new drawing");

            // Change component extension to the drawing one
            string oldDrawingPath = Path.ChangeExtension(oldFilePath, "SLDDRW");

            string newDrawingPath = Path.ChangeExtension(newFilePath, "SLDDRW");

            if (File.Exists(newDrawingPath))
            {
                _logger.Log("A drawing already exists in the directory");

                throw new Exception("A drawing already exists in the directory");
            }

            // Create new drawing only if the old file already has a drawing
            if (File.Exists(oldDrawingPath))
            {
                _logger.Log("The old file had a drawing");
                _logger.Log("Copying the old drawing");

                File.Copy(oldDrawingPath, newDrawingPath);

                // Try to remove the read-only flag from the new file
                _logger.Log("Remove the read only attribute from the new drawing");

                FileAttributes fileAttributes = File.GetAttributes(newDrawingPath);
                fileAttributes &= ~FileAttributes.ReadOnly;

                File.SetAttributes(newDrawingPath, fileAttributes);

                // Replace the drawing
                _logger.Log("Replace drawing reference"); 

                if (SolidWorksEnvironment.Application.UnsafeObject.ReplaceReferencedDocument(newDrawingPath, oldFilePath, newFilePath)
                    == false)
                {
                    SolidWorksEnvironment.Application.ShowMessageBox("Unable to replace the drawing reference", SolidWorksMessageBoxIcon.Warning);
                }

                _logger.Log($"Path to the new drawing: {newDrawingPath}");

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

            _logger.Log("Show save file dialog");

            // Exit the macro if the user click cancel
            if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                _logger.Log("The user exits the save file dialog", LoggerMessageSeverity_e.Warning);

                CwMessage.MacroStopped();
                
                return output;
            }

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string userInputFilePath = saveFileDialog.FileName;

                _logger.Log($"User selected path: {userInputFilePath}");

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

                _logger.Log($"File name without extension: {fileNameNoExt}");

                // Compose new file name
                string fileName = fileNameNoExt + _oldFileExtension;

                string fileFullPath = Path.Combine(folderPath, fileName);

                output = fileFullPath;
            }

            // Output validation
            if (output.IsNullOrEmpty())
            {
                _logger.Log("The selected path is empty", LoggerMessageSeverity_e.Error);

                CwMessage.NoValidPath();
            }

            if (File.Exists(output))
            {
                _logger.Log("There is already a file in the selected path", LoggerMessageSeverity_e.Error);

                CwMessage.FileAlreadyExists();

                output = string.Empty;
            }

            _logger.Log($"New path file: {output}");

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
