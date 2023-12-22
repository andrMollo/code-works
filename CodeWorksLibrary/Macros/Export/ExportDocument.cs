using CADBooster.SolidDna;
using CodeWorksLibrary.Helpers;
using CodeWorksLibrary.Macros.Drawings;
using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static CADBooster.SolidDna.SolidWorksEnvironment;

namespace CodeWorksLibrary
{
    internal static class ExportDocument
    {
        #region Public properties
        /// <summary>
        /// The name of the job to be used as export sub-folder
        /// </summary>
        internal static string JobNumber { get; set; }

        /// <summary>
        /// True to print the document, false to export it
        /// </summary>
        internal static bool PrintSelection { get; set; }

        #endregion

        #region Private fields
        /// <summary>
        /// The SolidDNA Model object of the active model
        /// </summary>
        static Model _model;

        /// <summary>
        /// The name of the file without the extension
        /// </summary>
        static string _modelNameNoExt;

        /// <summary>
        /// The path to the export folder, without filename
        /// </summary>
        static string _exportFolderPath;
        #endregion

        #region Public methods
        /// <summary>
        /// Export the active document to different format
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
            _model = model;

            // Set the job folder as empty string to export the document without any sub-folder
            JobNumber = string.Empty;

            // Set the selection to not print the document
            PrintSelection = false;

            // Export the document
            ExportModelDocument(_model);
        }

        /// <summary>
        /// Export the drawing and the model preview
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        internal static void ExportDrawingAndPreview()
        {
            // Export the drawing
            ExportDrawingDocument();       

        }

        #endregion

        #region Private methods
        /// <summary>
        /// Export the active drawing one sheet at a time
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private static void ExportDrawingDocument()
        {
            // Get the drawing model
            DrawingDocument drawingModel = _model.Drawing;

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

                ExportDrawingSheet(activeSheetName);
            }
        }

        /// <summary>
        /// Export the active sheet in different format
        /// </summary>
        /// <param name="sheetName">The name of the active sheet</param>
        private static void ExportDrawingSheet(string sheetName)
        {
            // Get the SolidWorks sheet object
            Sheet swSheet = _model.Drawing.UnsafeObject.get_Sheet(sheetName);

            // Check if the sheet contains a flat pattern
            if (!UpdateFormatMacro.CheckFlatPattern(swSheet))
            {
                // Upgrade sheet format
                UpdateFormatMacro.UpgradeSheetFormat(_model.Drawing.UnsafeObject, swSheet);

                // Export to PDF
                ExportSheetToPDF();
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
                new List<string>(_model.Drawing.SheetNames().ToList<string>()));

            string exportPath = ComposeExportFilePath("PDF");
        }

        /// <summary>
        /// Compose the full path for the exported file
        /// </summary>
        /// <param name="extension">The file extension, without dot ("PDF")</param>
        /// <returns>A string with the full path where to export the file</returns>
        private static string ComposeExportFilePath(string extension)
        {
            // Compose the final export folder adding a sub-folder with file extension
            string finalExportFolder = Path.Combine(_exportFolderPath, extension);

            // Check if the path to the final export folder exists
            if (!Directory.Exists(finalExportFolder))
            {
                Directory.CreateDirectory(finalExportFolder);
            }

            // Compose the filename with the extension
            string fileNameWithExtension = _modelNameNoExt + "." + extension;

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
            if (_model.IsDrawing)
            {
                ExportDrawingAndPreview();
            }
            else
            {

            }
        }

        /// <summary>
        /// Compose the folder name by combining the main export folder and the job number
        /// </summary>
        private static void ComposeExportFolderPath()
        {
            // Remove invalid characters form the job number string
            if (JobNumber != string.Empty)
            {
                JobNumber = CwValidation.RemoveInvalidChars(JobNumber);
            }

            _exportFolderPath = Path.Combine(GlobalConfig.ExportPath, JobNumber);            
        }
        #endregion
    }
}
