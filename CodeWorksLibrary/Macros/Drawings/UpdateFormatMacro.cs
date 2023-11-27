using CADBooster.SolidDna;
using SolidWorks.Interop.sldworks;
using System;
using System.IO;
using System.Windows.Documents;
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
                // Get the i-th sheet
                var swSheet = swDraw.get_Sheet(sheetNames[i]);

                var containsFlatPattern = CheckFlatPattern(swSheet);

                if (containsFlatPattern == false)
                {
                    // Get the format for the i-th sheet
                    var currentSheetFormatName = swSheet.GetSheetFormatName();

                    // Get the name of the new format
                    var newSheetFormatPath = GetReplaceSheetFormat(swSheet);

                    // Get the full path of the current format
                    var currentSheetFormatPath = swSheet.GetTemplateName();

                    // Activate i-th sheet
                    swDraw.ActivateSheet(sheetNames[i]);

                    // Replace with new one
                    ReplaceSheetFormat(swDraw, swSheet, newSheetFormatPath);                    
                }
            }
        }

        /// <summary>
        /// Check if the current sheet contains only one view that reference the flat pattern configuration
        /// </summary>
        /// <param name="sheet">The pointer to the current sheet object</param>
        /// <returns>Return true if the current sheet contains the flat pattern view</returns>
        private static bool CheckFlatPattern(Sheet sheet)
        {
            var views = (object[])sheet.GetViews();

            if ( views != null )
            {
                if ( views.Length == 1)
                {
                    var view = (View)views[0];

                    var refConfiguration = view.ReferencedConfiguration;

                    if ( refConfiguration != null && refConfiguration == GlobalConfig.FlatPatternConfigurationName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Replace the sheet format for the current sheet
        /// </summary>
        /// <param name="swDraw">The instance of the drawing doc</param>
        /// <param name="swSheet">The instance of the current sheet</param>
        /// <param name="newSheetFormatPath">The path to the new sheet format</param>
        private static void ReplaceSheetFormat(DrawingDoc swDraw, Sheet swSheet, string newSheetFormatPath)
        {
            // Get the properties of the current sheet
            var vProps = (double[])swSheet.GetProperties();

            // Assign sheet properties
            var paperSize = (int)vProps[0];
            var templateType = (int)vProps[1];
            var scale1 = (double)vProps[2];
            var scale2 = (double)vProps[3];
            var firstAngle = (bool)Convert.ToBoolean(vProps[4]);
            var width = (double)vProps[5];
            var height = (double)vProps[6];

            var custPrpView = swSheet.CustomPropertyView;

            // Set new sheet format
            try
            {
                var setupResult = swDraw.SetupSheet5(swSheet.GetName(), paperSize, templateType, scale1, scale2, firstAngle, newSheetFormatPath, width, height, custPrpView, true);
            }
            catch (Exception e)
            {
                Application.ShowMessageBox(e.Message, SolidWorksMessageBoxIcon.Stop);
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
                }
            }

            return targetTemplatePath;
        }
    }
}
