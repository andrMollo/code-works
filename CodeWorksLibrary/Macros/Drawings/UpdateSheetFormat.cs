﻿using CADBooster.SolidDna;
using CodeWorksLibrary.Helpers;
using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static CADBooster.SolidDna.SolidWorksEnvironment;


namespace CodeWorksLibrary.Macros.Drawings
{
    public static class UpdateSheetFormat
    {
        #region Public properties
        /// <summary>
        /// Pointer to the SolidDNA DrawingDocument
        /// </summary>
        public static DrawingDocument DrawDoc {  get; set; }

        /// <summary>
        /// True to always replace the format, false to replace only if the format full path is different
        /// </summary>
        public static bool AlwaysReplace { get; set; }
        #endregion

        /// <summary>
        /// Update the sheet format on all sheets of the active drawings.
        /// Sheets with only one view containing a flat pattern configuration are not updated.
        /// The sheet format is updated regardless of the current format name
        /// </summary>
        public static void UpdateSheetsFormatMacro()
        {
            Model model = Application.ActiveModel;

            #region Validation
            // Check if there is an open document, if the documents has been saved and if it is a drawing
            var isDrawingOpen = CwValidation.DrawingIsOpen(model);

            if (isDrawingOpen == false)
            {
                return;
            }
            #endregion

            // Set the SolidDNA drawing document
            DrawDoc = model.Drawing;

            // Set to always update the format
            AlwaysReplace = true;

            UpdateFormatAllSheet(model);                 
        }

        /// <summary>
        /// Update the sheet format for all sheet of the active model
        /// </summary>
        /// <param name="model">The pointer to the active SolidDNA Model object</param>
        public static void UpdateFormatAllSheet(Model model)
        {
            // Disable updates to the graphic view
            ModelView modelView = (ModelView)model.UnsafeObject.ActiveView;
            modelView.EnableGraphicsUpdate = false;

            // Get all the sheet names
            List<string> sheetNames = DrawDoc.SheetNames().ToList<string>();

            // Get the name of the active sheet
            string activeSheetName = DrawDoc.CurrentActiveSheet();

            // Get the active sheet number
            int activeSheetNumber = sheetNames.IndexOf(activeSheetName) + 1;

            // Loop through all the sheet starting form the active
            for (int i = 0; i < sheetNames.Count; i++)
            {
                // Offset required to start the loop from the active sheet
                int loopOffset = i + activeSheetNumber;

                if ((activeSheetNumber + i) >= sheetNames.Count)
                {
                    loopOffset = activeSheetNumber + i - sheetNames.Count;
                }

                // Active the sheet
                DrawDoc.ActivateSheet(sheetNames[loopOffset]);

                // Update the format
                UpdateActiveSheetFormat(DrawDoc.UnsafeObject, (Sheet)DrawDoc.UnsafeObject.GetCurrentSheet());
            }
            // Enable update to the graphic view
            modelView.EnableGraphicsUpdate = true;
        }

        /// <summary>
        /// Update the sheet format on the active sheet. The sheet format is updated regardless of the current format name.
        /// Sheets with only one view containing a flat pattern configuration are not updated.
        /// </summary>
        /// <param name="swDraw">The pointer to the DrawingDoc model</param>
        /// <param name="swSheet">The pointer to Sheet model</param>
        public static void UpdateActiveSheetFormat(DrawingDoc swDraw, Sheet swSheet)
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

                if (AlwaysReplace)
                {
                    // Replace with new one
                    ReplaceSheetFormat(swDraw, swSheet, newSheetFormatPath); 
                }
                else
                {
                    // Change the format if the current full name and the new one are different
                    if (currentSheetFormatPath != newSheetFormatPath)
                    {
                        // Replace with new one
                        ReplaceSheetFormat(swDraw, swSheet, newSheetFormatPath);
                    }
                }
            }            
        }       

        /// <summary>
        /// Check if the current sheet contains only one view that reference the flat pattern configuration
        /// </summary>
        /// <param name="sheet">The pointer to the current sheet object</param>
        /// <returns>Return true if the current sheet contains the flat pattern view</returns>
        public static bool CheckFlatPattern(Sheet sheet)
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
