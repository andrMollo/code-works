// Ignore Spelling: Pdf Dwg

using CADBooster.SolidDna;
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

            if (Application.ActiveModel == null)
            {
                Application.ShowMessageBox("Open a file", SolidWorksMessageBoxIcon.Stop);

                return;
            }

            // Check if the open file has already been saved
            if (Application.ActiveModel.HasBeenSaved == false)
            {
                Application.ShowMessageBox("Save the file to run the macro", SolidWorksMessageBoxIcon.Stop);

                return;
            }
            #endregion

            // Check the type of file open
            if (Application.ActiveModel.IsDrawing)
            {
                // TODO Update format

                // Export drawing
                ExportDrawing();

                // Get root model
                // Open model
                // Export model
                    // Export STEP
                    // Export PNG
            }
            else
            {
                // Export model
                // Open drawing
                // Export drawing
            }
        } 

        /// <summary>
        /// Export a drawing to PDF
        /// </summary>
        public static void ExportDrawing()
        {
            // Export to PDF
            ExportDrawingAsPdf();

            // Export to DWG
            ExportDrawingAsDwg();

        }

        /// <summary>
        /// Save the active drawing as PDF in a sub-folder "\PDF\" of GlobalConfig.ExportPath
        /// </summary>
        public static void ExportDrawingAsPdf()
        {
            // Check if the model is a drawing
            if (Application.ActiveModel?.IsDrawing != true)
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
            var sheetNames = new List<string>((string[])Application.ActiveModel.AsDrawing().GetSheetNames());

            var exportData = new PdfExportData();
            exportData.SetSheets(PdfSheetsToExport.ExportSpecifiedSheets, sheetNames);

            // Save new file
            var result = Application.ActiveModel.SaveAs(
                path,
                options: SaveAsOptions.Silent | SaveAsOptions.Copy | SaveAsOptions.UpdateInactiveViews,
                pdfExportData: exportData);

            if (!result.Successful)
                Application.ShowMessageBox("Failed to export drawing as PDF", SolidWorksMessageBoxIcon.Stop);
        }

        /// <summary>
        /// Save the active drawing as DWG in a sub-folder "\DWG\"  of GlobalConfig.ExportPath
        /// </summary>
        public static void ExportDrawingAsDwg()
        {
            // Check if the model is a drawing
            if (Application.ActiveModel?.IsDrawing != true)
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
            var result = Application.ActiveModel.SaveAs(
                path,
                options: SaveAsOptions.Silent | SaveAsOptions.Copy | SaveAsOptions.UpdateInactiveViews,
                pdfExportData: null);


            if (!result.Successful)
                Application.ShowMessageBox("Failed to export drawing as PDF", SolidWorksMessageBoxIcon.Stop);
        }

        /// <summary>
        /// Compose the export path by combining the GlobalConfig.ExportPath, the filename and the extension
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
    }
}
