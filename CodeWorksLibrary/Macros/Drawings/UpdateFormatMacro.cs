using CADBooster.SolidDna;
using CodeWorksLibrary.Helpers;
using SolidWorks.Interop.sldworks;
using System;
using System.IO;
using static CADBooster.SolidDna.SolidWorksEnvironment;


namespace CodeWorksLibrary.Macros.Drawings
{
    internal class UpdateFormatMacro
    {
        /// <summary>
        /// Update the sheet format on all sheets of the active drawings.
        /// Sheets with only one view containing a flat pattern configuration are not updated.
        /// The sheet format is updated regardless of the current format name
        /// </summary>
        /// <param name="updateCurrent"> True to update only the current sheet, False to update all the sheet of the drawing</param>
        internal static void UpdateFormat(bool updateCurrent)
        {
            var model = Application.ActiveModel;

            // Check if there is an open document, if the documents has been saved and if it is a drawing
            var isDrawingOpen = CwValidation.ModelIsDrawing(model);

            if (isDrawingOpen == false)
            {
                return;
            }

            DrawingDoc swDraw = model.AsDrawing();

            // Get the name of the active sheet
            var sheet = (Sheet)swDraw.GetCurrentSheet();
            var activeSheetName = sheet.GetName();

            // Disable updates to the graphic view
            ModelView modelView = (ModelView)model.UnsafeObject.ActiveView;
            modelView.EnableGraphicsUpdate = false;

            // Get the names of the sheet to update
            string[] sheetNames = GetDrawingSheetNames(swDraw, updateCurrent);

            for (int i = 0; i < sheetNames.Length; i++)
            {
                // Get the i-th sheet
                var swSheet = swDraw.get_Sheet(sheetNames[i]);

                // Check if the current sheet contains a flat pattern configuration
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

            // Activate the original sheet
            swDraw.ActivateSheet(activeSheetName);

            // Enable update to the graphic view
            modelView.EnableGraphicsUpdate = true;
        }

        /// <summary>
        /// Upgrade the sheet format on all sheets of the active drawings only if current and target format name are different.
        /// Sheets with only one view containing a flat pattern configuration are not updated.
        /// Use this method if you want to avoid updating the sheet format if there are no changes.
        /// </summary>
        /// <param name="updateCurrent"> True to update only the current sheet, False to update all the sheet of the drawing</param>
        internal static void UpgradeFormat(bool updateCurrent)
        {
            var model = Application.ActiveModel;

            // Check if there is an open document, if the documents has been saved and if it is a drawing
            var isDrawingOpen = CwValidation.ModelIsDrawing(model);

            if (isDrawingOpen == false)
            {
                return;
            }

            DrawingDoc swDraw = model.AsDrawing();

            // Get the name of the active sheet
            var sheet = (Sheet)swDraw.GetCurrentSheet();
            var activeSheetName = sheet.GetName();

            // Disable updates to the graphic view
            ModelView modelView = (ModelView)model.UnsafeObject.ActiveView;
            modelView.EnableGraphicsUpdate = false;

            // Get the names of the sheet to update
            string[] sheetNames = GetDrawingSheetNames(swDraw, updateCurrent);

            for (int i = 0; i < sheetNames.Length; i++)
            {
                // Get the i-th sheet
                var swSheet = swDraw.get_Sheet(sheetNames[i]);

                // Check if the current sheet contains a flat pattern configuration
                var containsFlatPattern = CheckFlatPattern(swSheet);

                if (containsFlatPattern == false)
                {
                    // Get the format for the i-th sheet
                    var currentSheetFormatName = swSheet.GetSheetFormatName();

                    // Get the name of the new format
                    var newSheetFormatPath = GetReplaceSheetFormat(swSheet);

                    // Get the full path of the current format
                    var currentSheetFormatPath = swSheet.GetTemplateName();

                    // Change the format if the current full name and the new one are different
                    if (currentSheetFormatPath != newSheetFormatPath)
                    {
                        // Activate i-th sheet
                        swDraw.ActivateSheet(sheetNames[i]);

                        // Replace with new one
                        ReplaceSheetFormat(swDraw, swSheet, newSheetFormatPath);
                    }
                }
            }

            // Activate the original sheet
            swDraw.ActivateSheet(activeSheetName);

            // Enable update to the graphic view
            modelView.EnableGraphicsUpdate = true;
        }

        /// <summary>
        /// Get the names of the sheets to be updated
        /// </summary>
        /// <param name="swDraw">The pointer to the DrawindDoc Model</param>
        /// <param name="onlyActive">True to get the name of the active sheet only, False to get the names of all the sheets</param>
        /// <returns>An array of strings with the names of the sheets to be updated</returns>
        internal static string[] GetDrawingSheetNames(DrawingDoc swDraw, bool onlyActive)
        {
            string[] sheetNames = new string[1];

            // Get the names of all the sheets
            if (onlyActive == false)
            {
                // Get the names of the sheets of the active drawing
                var vSheetNames = (string[])swDraw.GetSheetNames();

                var sheetCount = vSheetNames.Length;

                // Resize the string array
                Array.Resize(ref sheetNames, sheetCount);

                // Assign the names of the sheet to the string array
                sheetNames = vSheetNames;
            }
            // Get the name of the active sheet
            else if (onlyActive == true)
            {
                var sheet = (Sheet)swDraw.GetCurrentSheet();

                sheetNames[0] = (string)sheet.GetName();
            }

            return sheetNames;
        }

        /// <summary>
        /// Check if the current sheet contains only one view that reference the flat pattern configuration
        /// </summary>
        /// <param name="sheet">The pointer to the current sheet object</param>
        /// <returns>Return true if the current sheet contains the flat pattern view</returns>
        internal static bool CheckFlatPattern(Sheet sheet)
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
        internal static void ReplaceSheetFormat(DrawingDoc swDraw, Sheet swSheet, string newSheetFormatPath)
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
        internal static string GetReplaceSheetFormat(Sheet swSheet)
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
