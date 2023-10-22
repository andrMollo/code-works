using CADBooster.SolidDna;
using SolidWorks.Interop.sldworks;
using System;
using System.IO;
using static CADBooster.SolidDna.SolidWorksEnvironment;


namespace CodeWorksLibrary.Macros.Drawings
{
    internal class UpdateFormatMacro
    {
        /// <summary>
        /// Update the sheet format on all sheets of the active drawings
        /// </summary>
        public static void UpdateFormat()
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

            if (model.IsDrawing != true)
            {
                Application.ShowMessageBox("Open a drawing to run the macro", SolidWorksMessageBoxIcon.Stop);
            }
            #endregion

            DrawingDoc swDraw = model.AsDrawing();

            // Get the names of the sheets of the active drawing
            string[] sheetNames = model.Drawing.SheetNames();

            for (int i = 0; i < sheetNames.Length; i++)
            {
                // Get the -th sheet
                var swSheet = swDraw.get_Sheet(sheetNames[i]);

                // Get the format for the i-th sheet
                var currentSheetFormatName = swSheet.GetSheetFormatName();

                // Get the name of the new format
                var newSheetFormatName = GetReplaceSheetFormat(swSheet);

                // Replace with new one

            }
        }

        /// <summary>
        /// Get the path of the new sheet format according to the replace map
        /// </summary>
        /// <param name="swSheet">The sheet that need a sheet format replace</param>
        /// <returns>The string with path to the new sheet format</returns>
        private static string GetReplaceSheetFormat(Sheet swSheet)
        {
            string targetTemplatePath = "";

            // Read replace map
            string[] replaceMap = File.ReadAllLines(GlobalConfig.SheetFormatMapPath);

            // Get sheet size
            var currentSize = swSheet.GetSize(-1, -1);

            // Loop through the replace map
            for (int i = 0; i < replaceMap.Length; i++)
            {
                var map = replaceMap[i];

                // Split the replace map line
                string[] mapParameters = map.Split('|');

                // Assign the paper size in the map to a variable
                Int32.TryParse(mapParameters[0].Trim(), out int mapPaperSize);

                // Check if there is match in the replace map with the current sheet size
                if (currentSize == mapPaperSize)
                {
                    // Assign the path of the new sheet format
                    targetTemplatePath = mapParameters[2];

                    Application.ShowMessageBox("Matching size found: " + mapParameters[2], SolidWorksMessageBoxIcon.Information);
                }
            }

            return targetTemplatePath;
        }
    }
}
