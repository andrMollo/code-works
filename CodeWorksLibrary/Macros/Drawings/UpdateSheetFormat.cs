using CADBooster.SolidDna;
using CodeWorksLibrary.Helpers;
using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.IO;
using static CADBooster.SolidDna.SolidWorksEnvironment;


namespace CodeWorksLibrary.Macros.Drawings
{
    internal class UpdateSheetFormat
    {
        #region Public properties
        
        #endregion

        /// <summary>
        /// Update the sheet format on all sheets of the active drawings.
        /// Sheets with only one view containing a flat pattern configuration are not updated.
        /// The sheet format is updated regardless of the current format name
        /// </summary>
        internal static void UpdateFormatAllSheets()
        {
            var model = Application.ActiveModel;

            // Check if there is an open document, if the documents has been saved and if it is a drawing
            var isDrawingOpen = CwValidation.DrawingIsOpen(model);

            if (isDrawingOpen == false)
            {
                return;
            }

            DrawingDoc swDraw = model.AsDrawing();

            // Get the name of the active sheet
            // This is require to return to the active sheet at the end of the macro
            var sheet = (Sheet)swDraw.GetCurrentSheet();
            var activeSheetName = sheet.GetName();

            // Disable updates to the graphic view
            ModelView modelView = (ModelView)model.UnsafeObject.ActiveView;
            modelView.EnableGraphicsUpdate = false;

            // Get the names of the sheet to update
            List<string> sheetNames = GetDrawingSheetNames(swDraw);

            foreach (string sheetName in sheetNames)
            {
                // Get the i-th sheet
                var swSheet = swDraw.get_Sheet(sheetName);

                // Activate i-th sheet
                swDraw.ActivateSheet(sheetName);

                UpdateActiveSheetFormat(swDraw, swSheet);
            }

            // Activate the original sheet
            swDraw.ActivateSheet(activeSheetName);

            // Enable update to the graphic view
            modelView.EnableGraphicsUpdate = true;
        }

        /// <summary>
        /// Update the sheet format on the active sheet. The sheet format is updated regardless of the current format name.
        /// Sheets with only one view containing a flat pattern configuration are not updated.
        /// </summary>
        /// <param name="swDraw">The pointer to the DrawingDoc model</param>
        /// <param name="swSheet">The pointer to Sheet model</param>
        internal static void UpdateActiveSheetFormat(DrawingDoc swDraw, Sheet swSheet)
        {          
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

                // Replace with new one
                ReplaceSheetFormat(swDraw, swSheet, newSheetFormatPath);
            }            
        }

        /// <summary>
        /// Upgrade the sheet format on the active sheet only if current and target format name are different.
        /// Sheets with only one view containing a flat pattern configuration are not updated.
        /// </summary>
        /// <param name="swDraw">The pointer to the DrawingDoc model</param>
        /// <param name="swSheet">The pointer to Sheet model</param>
        internal static void UpgradeSheetFormat(DrawingDoc swDraw, Sheet swSheet)
        {
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
                    // Replace with new one
                    ReplaceSheetFormat(swDraw, swSheet, newSheetFormatPath);
                }
            }
        }

        /// <summary>
        /// Get the names of the sheets to be updated
        /// </summary>
        /// <param name="swDraw">The pointer to the DrawindDoc Model</param>
        /// <returns>An list of strings with the names of the sheets to be updated</returns>
        internal static List<string> GetDrawingSheetNames(DrawingDoc swDraw)
        {
            // Get the names of the sheets of the active drawing
            var sheetNames = (string[])swDraw.GetSheetNames();

            List<string> sheetList = new List<string>(sheetNames);

            return sheetList;
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
