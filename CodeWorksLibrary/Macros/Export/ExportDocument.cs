using CADBooster.SolidDna;
using CodeWorksLibrary.Helpers;
using CodeWorksLibrary.Macros.Drawings;
using CodeWorksLibrary.Macros.Export;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swdocumentmgr;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using static CADBooster.SolidDna.SolidWorksEnvironment;

namespace CodeWorksLibrary
{
    internal static class ExportDocument
    {
        #region Public properties
        /// <summary>
        /// The SolidDNA Model object of the active model
        /// </summary>
        internal static Model ExportModel { get; set; }

        /// <summary>
        /// The path to the export folder, without filename
        /// </summary>
        internal static string ExportFolderPath { get; set; }

        /// <summary>
        /// The name of the job to be used as export sub-folder
        /// </summary>
        internal static string JobNumber { get; set; }

        /// <summary>
        /// True to print the document
        /// </summary>
        internal static bool PrintSelection { get; set; }

        /// <summary>
        /// True to export the document
        /// </summary>
        internal static bool ExportSelection { get; set; }
        #endregion

        #region Private fields
        /// <summary>
        /// The name of the file without the extension
        /// </summary>
        static string _modelNameNoExt;
        #endregion

        #region Public methods
        /// <summary>
        /// Export the active document and its drawing to different format
        /// </summary>
        internal static void ExportDocumentMacro()
        {
            Model model = Application.ActiveModel;

            #region Validation
            // Check if there is an open document and if there is it can't be a drawing
            if (!CwValidation.ModelIsOpen(model))
            {
                return;
            }
            #endregion

            // Set the active model
            ExportModel = model;

            // Set the job folder as empty string to export the document without any sub-folder
            JobNumber = string.Empty;

            // Set the selection to export the document
            ExportSelection = true;

            // Set the selection to not print the document
            PrintSelection = false;

            // Export the document
            ExportModelDocument(ExportModel);
        }

        /// <summary>
        /// Export and print the active document and its drawing to different format
        /// </summary>
        internal static void ExportPrintDocumentMacro()
        {
            Model model = Application.ActiveModel;

            #region Validation
            // Check if there is an open document and if there is it can't be a drawing
            if (!CwValidation.ModelIsOpen(model))
            {
                return;
            }
            #endregion

            // Set the active model
            ExportModel = model;

            // Set the job folder as empty string to export the document without any sub-folder
            JobNumber = string.Empty;

            // Set the selection to export the document
            ExportSelection = true;

            // Set the selection to not print the document
            PrintSelection = true;

            // Export the document
            ExportModelDocument(ExportModel);
        }

        /// <summary>
        /// Export the drawing and the model preview
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        internal static void ExportDrawingAndPreview()
        {
            // Export the drawing
            ExportDrawingDocument();

            // Get the parent model
            ExportModel = GetDrawingParentModel();

            // Export model preview
            ExportModelPreview();
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Get the 3d model referenced inf the first view of the model
        /// </summary>
        /// <returns>The SolidDNA Model object for the 3d model referenced in the drawing</returns>
        private static Model GetDrawingParentModel()
        {
            // Check if the drawing is a model
            if (ExportModel.IsDrawing)
            {
                // Get the SolidWorks DrawingDoc object
                DrawingDoc swDrwDoc = ExportModel.AsDrawing();

                // Get the first view of the model
                // Be careful it can also be a sheet
                View firsView = (View)swDrwDoc.GetFirstView();

                // Loop through all the view to get the first one that is not a sheet
                while (firsView != null)
                {
                    // If the view is not a sheet
                    if (firsView.Type != (int)swDrawingViewTypes_e.swDrawingSheet)
                    {
                        goto firstViewFound;
                    }

                    // Then the vie is actually a sheet so get the next view
                    firsView = (View)firsView.GetNextView();
                }

            firstViewFound:

                Model refModel = new Model((ModelDoc2)firsView.ReferencedDocument);
                return refModel;
            }
            else
            {
                throw new Exception("Unable to get the model referenced in the drawing");
            }
        }

        /// <summary>
        /// Export the active drawing one sheet at a time
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private static void ExportDrawingDocument()
        {
            // Get the drawing model
            DrawingDocument drawingModel = ExportModel.Drawing;

            // Get all the sheet names
            List<string> sheetNames = drawingModel.SheetNames().ToList<string>();

            // Get the name if the active sheet
            string activeSheetName = drawingModel.CurrentActiveSheet();

            /* Get the active sheet number
             * this allow the loop to start from the active one
             * and save a change of sheet
             */
            int activeSheetNumber = sheetNames.IndexOf(activeSheetName) +1 ;

            // Loop through sheets
            for (int i = 0; i < sheetNames.Count; i++)
            {
                /*
                 * Offset require to start the loop from the active sheet
                 */
                int loopOffset = i + activeSheetNumber;

                if ((activeSheetNumber + i) >= sheetNames.Count)
                {
                    loopOffset = activeSheetNumber + i - sheetNames.Count;
                }

                // Activate sheet
                drawingModel.ActivateSheet(sheetNames[loopOffset]);

                // Export the drawing
                ExportDrawingSheet(activeSheetName);
            }            
        }

        /// <summary>
        /// Export the part or assembly to STEP
        /// </summary>
        private static void Export3DModel()
        {
            if (ExportSelection == true)
            {
                // Export the model to step
                ExportModelToStep();
            }
        }

        /// <summary>
        /// Export the active sheet in different format
        /// </summary>
        /// <param name="sheetName">The name of the active sheet</param>
        private static void ExportDrawingSheet(string sheetName)
        {
            // Get the SolidWorks sheet object
            Sheet swSheet = ExportModel.Drawing.UnsafeObject.get_Sheet(sheetName);

            // Check if the sheet contains a flat pattern
            if (!UpdateFormatMacro.CheckFlatPattern(swSheet))
            {
                // Upgrade sheet format
                UpdateFormatMacro.UpgradeSheetFormat(ExportModel.Drawing.UnsafeObject, swSheet);

                if (ExportSelection == true)
                {
                    // Export to PDF
                    ExportSheetToPDF();
                }

                // Print she active sheet
                if (PrintSelection)
                {
                    FastPrintMacro.PrintDrawingSheet(
                        ExportModel.UnsafeObject,
                        swSheet);
                }
            }

            if (ExportSelection == true)
            {
                // Export to DWG
                ExportSheetToDWG();
            }
        }

        /// <summary>
        /// Export model to PNG using the Document Manager API
        /// </summary>
        private static void ExportModelPreview()
        {
            SwDMClassFactory classFactory = Activator.CreateInstance(
                Type.GetTypeFromProgID("SwDocumentMgr.SwDMClassFactory")) as SwDMClassFactory;

            if (classFactory != null)
            {
                SwDMApplication dmApp = classFactory.GetApplication(GlobalConfig.DmKey.Trim('"'));

                // Get the Document Manager document type
                SwDmDocumentType docType = ExportDocument.GetDmDocumentType(ExportModel);

                // Open the document
                SwDmDocumentOpenError dmOpenError = new SwDmDocumentOpenError();

                SwDMDocument swDmDoc = dmApp.GetDocument(ExportModel.FilePath, docType, true, out dmOpenError);

                if (swDmDoc != null)
                {
                    // Get the name of the active configuration
                    var activeConfigName = swDmDoc.ConfigurationManager.GetActiveConfigurationName();

                    // Get the active configuration
                    var activeConfig = swDmDoc.ConfigurationManager.GetConfigurationByName(activeConfigName);

                    // Get the preview object
                    var previewErr = new SwDmPreviewError();
                    var previewObject = activeConfig.GetPreviewBitmap(out previewErr);

                    if (previewErr != SwDmPreviewError.swDmPreviewErrorNoPreview)
                    {
                        // Convert preview object to image
                        Image imgPreview = PictureDispConverter.Convert(previewObject);

                        if (previewErr == SwDmPreviewError.swDmPreviewErrorNone)
                        {
                            // Get the output path for the preview
                            string exportPath = ComposeExportFilePath("PNG");

                            // Save preview as PNG
                            imgPreview.Save(exportPath, System.Drawing.Imaging.ImageFormat.Png);
                        }
                    }
                }
            }
            else
            {
                throw new NullReferenceException("Document Manager SDK not installed.");
            }
        }

        /// <summary>
        /// Get the Document Manager document type
        /// </summary>
        /// <param name="model">The SoldDNA Model object for the model</param>
        /// <returns></returns>
        internal static SwDmDocumentType GetDmDocumentType(Model model)
        {
            // Get the model file extension
            var modelExt = Path.GetExtension(model.FilePath).ToUpper();

            SwDmDocumentType dmDocType = new SwDmDocumentType();

            switch (modelExt)
            {
                case ".SLDPRT":
                    dmDocType = SwDmDocumentType.swDmDocumentPart;
                    break;
                case ".SLDASM":
                    dmDocType = SwDmDocumentType.swDmDocumentAssembly;
                    break;
                case ".SLDDRW":
                    dmDocType = SwDmDocumentType.swDmDocumentDrawing;
                    break;
                default:
                    Application.ShowMessageBox("The document ha no valid SolidWorks file extension", SolidWorksMessageBoxIcon.Stop);
                    break;
            }

            return dmDocType;
        }

        /// <summary>
        /// Export the active sheet to DWG
        /// </summary>
        private static void ExportSheetToDWG()
        {
            // Compose the full path for the exported file
            string exportPath = ComposeExportFilePath("DWG");

            // Get the original option for sheet export of DXF / DWG 
            int originalDxfSheetOption = Application.GetUserPreferencesInteger(swUserPreferenceIntegerValue_e.swDxfMultiSheetOption);

            // Change SolidWorks option to export single sheet to DWG
            Application.SetUserPreferencesInteger(swUserPreferenceIntegerValue_e.swDxfMultiSheetOption, (int)swDxfMultisheet_e.swDxfActiveSheetOnly);

            // Save the file
            ModelSaveResult exportResult = ExportModel.SaveAs(
                exportPath,
                options: SaveAsOptions.Silent | SaveAsOptions.Copy | SaveAsOptions.UpdateInactiveViews,
                pdfExportData: null
                );

            // Revert to the original option for sheet export of DXF / DWG
            Application.SetUserPreferencesInteger(swUserPreferenceIntegerValue_e.swDxfMultiSheetOption, originalDxfSheetOption);

            // Show message box if export fails
            if (!exportResult.Successful)
            {
                Application.ShowMessageBox($"Failed to export {_modelNameNoExt} to DWG.",SolidWorksMessageBoxIcon.Stop);
            }
        }

        /// <summary>
        /// Export the active sheet to PDF
        /// </summary>
        private static void ExportSheetToPDF()
        {
            // Set the sheet to be exported as the current one
            var exportData = new PdfExportData();
            exportData.SetSheets(PdfSheetsToExport.ExportCurrentSheet,
                new List<string>(ExportModel.Drawing.SheetNames().ToList<string>()));

            // Compose the full path for the exported file
            string exportPath = ComposeExportFilePath("PDF");

            // Save the file
            ModelSaveResult exportResult = ExportModel.SaveAs(
                exportPath,
                options: SaveAsOptions.Silent | SaveAsOptions.Copy | SaveAsOptions.UpdateInactiveViews,
                pdfExportData: exportData
                );

            // Show message box if export fails
            if (!exportResult.Successful)
            {
                Application.ShowMessageBox($"Failed to export {_modelNameNoExt} to PDF.", SolidWorksMessageBoxIcon.Question);
            }            
        }

        /// <summary>
        /// Export the active model as STEP
        /// </summary>
        private static void ExportModelToStep()
        {
            string exportPath = ComposeExportFilePath("STP");

            ModelSaveResult exportResult = ExportModel.SaveAs(
                exportPath,
                options: SaveAsOptions.Silent | SaveAsOptions.Copy,
                pdfExportData: null
                );

            // Show message box if export fails
            if (!exportResult.Successful)
            {
                Application.ShowMessageBox($"Failed to export {_modelNameNoExt} to STEP.", SolidWorksMessageBoxIcon.Stop);
            }
        }

        /// <summary>
        /// Compose the full path for the exported file
        /// </summary>
        /// <param name="extension">The file extension, without dot ("PDF")</param>
        /// <returns>A string with the full path where to export the file</returns>
        private static string ComposeExportFilePath(string extension)
        {
            // Compose the final export folder adding a sub-folder with file extension
            string finalExportFolder = Path.Combine(ExportFolderPath, extension);

            // Check if the path to the final export folder exists
            if (!Directory.Exists(finalExportFolder))
            {
                Directory.CreateDirectory(finalExportFolder);
            }

            // Add sheet name as file name suffix in there is more then one sheet
            string fileNameSuffix = string.Empty;

            // If Model is a drawing
            if (ExportModel.IsDrawing)
            {
                if (ExportModel.Drawing.SheetNames().ToList<string>().Count > 1)
                {
                    fileNameSuffix = "_" + CwValidation.RemoveInvalidFileNameChars(ExportModel.Drawing.CurrentActiveSheet());
                }
            }

            // Compose the filename with the extension
            string fileNameWithExtension = _modelNameNoExt + fileNameSuffix + "." + extension;

            // Compose the full path
            var path = Path.Combine(finalExportFolder, fileNameWithExtension);

            return path;
        }

        /// <summary>
        /// Export the model to different format
        /// </summary>
        /// <param name="model">The pointer to the active SolidDNA.Model object</param>
        private static void ExportModelDocument(Model model)
        {
            // Get the model name without extension       
            _modelNameNoExt = Path.GetFileNameWithoutExtension(model.FilePath);

            // Set the export folder by combining the main export folder and the job number
            ComposeExportFolderPath();

            // Check model type
            if (ExportModel.IsDrawing)
            {
                ExportDrawingAndPreview();

                Export3DModel();
            }
            else if (!ExportModel.IsDrawing)
            {
                Export3DModel();

                // Get the drawing model
                ExportModel = GetDrawingModel();
                
                // Save the drawing model to be closed later
                Model drwModel = ExportModel;

                // Export the drawing and preview
                ExportDrawingAndPreview();

                // Close the file
                drwModel.Close();
            }
        }

        /// <summary>
        /// Get and activate the SolidDNA Model object for the drawing associate to the active model
        /// Assume drawing and model in the same directory with the same filename
        /// </summary>
        /// <returns>The pointer to SolidDNA Model object</returns>
        private static Model GetDrawingModel()
        {
            /* Get the drawing path
                 * Assume drawing and model in the same directory with the same filename
                 */
            string drwPath = Path.ChangeExtension(ExportModel.FilePath, "SLDDRW");


            // Get the list of open document
            var listOpenDoc = Application.OpenDocuments().ToList();

            // Check if the drawing path is amid the open documents
            var openDrw = listOpenDoc.Where(p => p.FilePath == drwPath);

            // If the drawing is found between the open documents try to activate it
            Model model = null;

            if (openDrw.Any())
            {
                // Try to active the drawing
                int activateError = new int();
                var swDrawingModel = Application.UnsafeObject.ActivateDoc3(
                    Path.GetFileName(openDrw.First().FilePath),
                    false,
                    (int)swRebuildOnActivation_e.swRebuildActiveDoc,
                    ref activateError);

                model = Application.ActiveModel;
            }
            // If the drawing is not open
            else
            {
                model = Application.OpenFile(drwPath,
                    options: OpenDocumentOptions.Silent
                    );
            }

            if (model == null)
            {
                Application.ShowMessageBox("Unable to open the drawing.", SolidWorksMessageBoxIcon.Stop);
                throw new Exception("Unable to open the drawing");
            }

            return model;
        }

        /// <summary>
        /// Compose the folder name by combining the main export folder and the job number
        /// </summary>
        private static void ComposeExportFolderPath()
        {
            // Remove invalid characters form the job number string
            if (JobNumber != string.Empty)
            {
                JobNumber = CwValidation.RemoveInvalidPathChars(JobNumber);
            }

            ExportFolderPath = Path.Combine(GlobalConfig.ExportPath, JobNumber);            
        }
        #endregion
    }
}
