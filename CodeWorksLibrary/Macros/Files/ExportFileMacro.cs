// Ignore Spelling: Pdf Dwg drw

using CADBooster.SolidDna;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.IO;
using static CADBooster.SolidDna.SolidWorksEnvironment;

namespace CodeWorksLibrary.Macros.Files
{
    internal class ExportFileMacro
    {
        /// <summary>
        /// Export the open file in different format
        /// </summary>
        public static void ExportFile()
        {
            #region Validation
            // Check if there is an open document and if there is it can't be a drawing

            var model = Application.ActiveModel;

            if (model == null)
            {
                Application.ShowMessageBox("Open a file", SolidWorksMessageBoxIcon.Stop);

                return;
            }

            // Check if the open file has already been saved
            if (model.HasBeenSaved == false)
            {
                Application.ShowMessageBox("Save the file to run the macro", SolidWorksMessageBoxIcon.Stop);

                return;
            }
            #endregion

            // Check the type of file open
            if (model.IsDrawing)
            {
                // TODO Update format

                // Export drawing
                ExportDrawing(model);

                // Get the 3d model referenced in the drawing
                Model swModel = GetRootModel(model);

                // Export model
                ExportModel(swModel);
            }
            else
            {
                // Export model
                // Open drawing
                // Export drawing
            }
        }

        /// <summary>
        /// Export a 3d model to STEP and PNG (with SolidWorks Document Manager API
        /// </summary>
        /// <param name="model"> The model object for the model</param>
        public static void ExportModel(Model model)
        {
            // Export STEP
            ExportModelAsStep(model);

            // Export PNG with Document Manager API
            ExportModelAsPng(model);

        }

        /// <summary>
        /// Export a model to PNG using the Document Manger API
        /// </summary>
        /// <param name="model">The model object for the model</param>
        private static void ExportModelAsPng(Model model)
        {
            
        }

        /// <summary>
        /// Export a model to STEP
        /// </summary>
        /// <param name="model">The model object for the model</param>
        public static void ExportModelAsStep(Model model)
        {
            // Check that the model is a part or an assembly
            if (model?.IsPart != true && model?.IsAssembly != true)
            {
                Application.ShowMessageBox("Export to STEP allowed only for parts or assemblies", SolidWorksMessageBoxIcon.Stop);

                return;
            }

            // Get the full path for the export
            var path = ComposeOutFileName("STP", "STEP");

            // Check is the export full path is empty or null
            if (string.IsNullOrEmpty(path))
            {
                Application.ShowMessageBox("The export path isn't valid", SolidWorksMessageBoxIcon.Stop);

                return;
            }

            // Save new file
            var result = model.SaveAs(
                path,
                options: SaveAsOptions.Silent | SaveAsOptions.Copy | SaveAsOptions.UpdateInactiveViews,
                pdfExportData: null);


            if (!result.Successful)
                Application.ShowMessageBox("Failed to export drawing as STEP", SolidWorksMessageBoxIcon.Stop);
        }

        /// <summary>
        /// Export a drawing to PDF and DWG
        /// </summary>
        /// <param name="drwModel">The Model object of the drawing</param>
        public static void ExportDrawing(Model drwModel)
        {
            // Export to PDF
            ExportDrawingAsPdf(drwModel);

            // Export to DWG
            ExportDrawingAsDwg(drwModel);

        }

        /// <summary>
        /// Save the active drawing as PDF in a sub-folder "\PDF\" of GlobalConfig.ExportPath
        /// </summary>
        /// <param name="model">The Model object of the drawing</param>
        public static void ExportDrawingAsPdf(Model model)
        {
            // Check if the model is a drawing
            if (model?.IsDrawing != true)
            {
                Application.ShowMessageBox("Export to PDF allowed only for drawings", SolidWorksMessageBoxIcon.Stop);

                return;
            }

            // Get the full path for the export
            var path = ComposeOutFileName("PDF");

            // Check is the export full path is empty or null
            if (string.IsNullOrEmpty(path))
            {
                Application.ShowMessageBox("The export path isn't valid", SolidWorksMessageBoxIcon.Stop);

                return;
            }

            // Get sheet names
            var sheetNames = new List<string>((string[])model.AsDrawing().GetSheetNames());

            var exportData = new PdfExportData();
            exportData.SetSheets(PdfSheetsToExport.ExportSpecifiedSheets, sheetNames);

            // Save new file
            var result = model.SaveAs(
                path,
                options: SaveAsOptions.Silent | SaveAsOptions.Copy | SaveAsOptions.UpdateInactiveViews,
                pdfExportData: exportData);

            if (!result.Successful)
                Application.ShowMessageBox("Failed to export drawing as PDF", SolidWorksMessageBoxIcon.Stop);
        }

        /// <summary>
        /// Save the active drawing as DWG in a sub-folder "\DWG\"  of GlobalConfig.ExportPath
        /// </summary>
        /// <param name="model">The Model object of the drawing</param>
        public static void ExportDrawingAsDwg(Model model)
        {
            // Check if the model is a drawing
            if (model?.IsDrawing != true)
            {
                Application.ShowMessageBox("Export to PDF allowed only for drawings", SolidWorksMessageBoxIcon.Stop);

                return;
            }

            // Get the full path for the export
            var path = ComposeOutFileName("DWG");

            // Check is the export full path is empty or null
            if (string.IsNullOrEmpty(path))
            {
                Application.ShowMessageBox("The export path isn't valid", SolidWorksMessageBoxIcon.Stop);

                return;
            }

            // Save new file
            var result = model.SaveAs(
                path,
                options: SaveAsOptions.Silent | SaveAsOptions.Copy | SaveAsOptions.UpdateInactiveViews,
                pdfExportData: null);


            if (!result.Successful)
                Application.ShowMessageBox("Failed to export drawing as PDF", SolidWorksMessageBoxIcon.Stop);
        }

        /// <summary>
        /// Compose the export path by combining the GlobalConfig.ExportPath, the filename of the active model and the input extension
        /// </summary>
        /// <param name="extension">The file extension to append at the end of the path</param>
        /// <returns>The string corresponding to the full path where the export will be saved</returns>
        public static string ComposeOutFileName(string extension)
        {
            var swModel = Application.ActiveModel;

            // Get file path
            var modelPath = swModel.FilePath;

            // Get file name without extension
            var fileName = Path.GetFileNameWithoutExtension(modelPath);

            // Compose the path to the folder
            var folderPath = Path.Combine(GlobalConfig.ExportPath, extension);

            // Check if output path exists, if not create it
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Compose the filename with the selected extension
            var fileWithExtension = fileName + "." + extension;

            // Compose the full path to the export file
            var fullPath = Path.Combine(folderPath, fileWithExtension);

            return fullPath;

        }

        /// <summary>
        /// Compose the export path by combining the GlobalConfig.ExportPath, the filename of the active model and the input extension
        /// </summary>
        /// <param name="extension">The file extension to append at the end of the path</param>
        /// <param name="subFolder">The name of the sub-folder to use</param>
        /// <returns>The string corresponding to the full path where the export will be saved</returns>
        public static string ComposeOutFileName(string extension, string subFolder)
        {
            var swModel = Application.ActiveModel;

            // Get file path
            var modelPath = swModel.FilePath;

            // Get file name without extension
            var fileName = Path.GetFileNameWithoutExtension(modelPath);

            // Compose the path to the folder
            var folderPath = Path.Combine(GlobalConfig.ExportPath, subFolder);

            // Check if output path exists, if not create it
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Compose the filename with the selected extension
            var fileWithExtension = fileName + "." + extension;

            // Compose the full path to the export file
            var fullPath = Path.Combine(folderPath, fileWithExtension);

            return fullPath;

        }

        /// <summary>
        /// Get the model of the component referenced in the first view of the drawing model
        /// </summary>
        /// <param name="model">The Model object of the drawing</param>
        /// <returns>The pointer to the Model object</returns>
        public static Model GetRootModel(Model drawingModel)
        {
            // Cast the SolidDNA model to a DrawingDoc object from the SolidWorks API
            DrawingDoc drwModel = drawingModel.AsDrawing();

            // Get the first view of the model
            // Be careful it can be a sheet
            View firstView = (View)drwModel.GetFirstView();

            // Loop through all the view to get the first one that is not a sheet
            while (firstView != null)
            {
                if (firstView.Type != (int)swDrawingViewTypes_e.swDrawingSheet)
                {
                    goto exitLoop;
                }

                firstView = (View)firstView.GetNextView();
            }

        exitLoop:

            var model = new Model((ModelDoc2)firstView.ReferencedDocument);

            return model;
        }
    }
}
