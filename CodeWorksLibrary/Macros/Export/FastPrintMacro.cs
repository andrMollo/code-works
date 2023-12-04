﻿using CodeWorksLibrary.Helpers;
using CodeWorksLibrary.Macros.Drawings;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Drawing.Printing;
using static CADBooster.SolidDna.SolidWorksEnvironment;
using static CodeWorksLibrary.Helpers.CwLayerManager;
using static CodeWorksLibrary.Macros.Drawings.UpdateFormatMacro;
using static System.Drawing.Printing.PrinterSettings;

namespace CodeWorksLibrary.Macros.Export
{
    internal class FastPrintMacro
    {
        /// <summary>
        /// Print all the sheet in the current drawing to the default printer
        /// </summary>
        public static void FastPrint()
        {
            var model = Application.ActiveModel;

            // Check if there is an open document, if the documents has been saved and if it is a drawing
            var isDrawingOpen = CwValidation.ModelIsDrawing(model);

            if (isDrawingOpen == false)
            {
                return;
            }

            // Assign layer name
            var noteLayer = GlobalConfig.PrintNoteLayer;

            // Get the SolidWorks model doc
            ModelDoc2 swModel = model.UnsafeObject;

            var prpManager = new CwPropertyManager();

            // Set the name of the user who is printing
            var retPrintedBy = prpManager.SetPrintedByProperty(swModel);

            // Set the date when the document is printed
            var retPrintedOn = prpManager.SetPrintedOnProperty(swModel);

            DrawingDoc swDraw = model.AsDrawing();

            // Get the name of the active sheet
            // This is require to return to the active sheet at the end of the macro
            var sheet = (Sheet)swDraw.GetCurrentSheet();
            var activeSheetName = sheet.GetName();

            // Get sheet names
            string[] sheetNames = GetDrawingSheetNames(swDraw);

            // Loop through sheets
            for (int i = 0; i < sheetNames.Length; i++)
            {
                // Get the i-th sheet
                var swSheet = swDraw.get_Sheet(sheetNames[i]);

                swDraw.ActivateSheet(sheetNames[i]);

                UpgradeSheetFormat(swDraw, swSheet);

                var retChangeLayerView = ChangeLayerVisibility((ModelDoc2)swDraw, noteLayer, true);

                // Get original print layout
                var swPageSetup = (PageSetup)swModel.PageSetup;

                var originalPrinter = swModel.Printer;
                var originalPrinterPaperSize = swPageSetup.PrinterPaperSize;
                var originalScaleToFit = swPageSetup.ScaleToFit;
                var originalScale = swPageSetup.Scale2;
                var originalOrientation = swPageSetup.Orientation;
                var originalPageSetup = swModel.Extension.UsePageSetup;

                // Get page dimension and printer name
                var currentSize = swSheet.GetSize(-1, -1);

                var pageDimension = string.Empty;

                // If sheet dimension is A4 the print to A4, otherwise print to A3
                if (currentSize == (int)swDwgPaperSizes_e.swDwgPaperA4size)
                {
                    pageDimension = GlobalConfig.A4FormatName;
                }
                else
                {
                    pageDimension= GlobalConfig.A3FormatName;
                }

                var printerName = GlobalConfig.DefaultPrinterName;

                // Set print parameters
                swModel.Printer = printerName;

                swPageSetup.PrinterPaperSize = GetPaper(printerName, pageDimension);

                swPageSetup.ScaleToFit = true;

                swPageSetup.Orientation = (int)swPageSetupOrientation_e.swPageSetupOrient_Landscape;

                swModel.Extension.UsePageSetup = (int)swPageSetupInUse_e.swPageSetupInUse_Document;

                PrintSpecification swPrintSpec = (PrintSpecification)swModel.Extension.GetPrintSpecification();

                // Get current sheet number
                var activeSheetNumber = GetSheetNumber(swDraw, swSheet);

                // Set the print range
                long[] printRangeArray = new long[2];

                printRangeArray[0] = activeSheetNumber;
                printRangeArray[1] = activeSheetNumber;

                swPrintSpec.PrintRange = printRangeArray;

                // Print the document
                swModel.Extension.PrintOut4(printerName, "", swPrintSpec);

                // Revert print setup to original

                retChangeLayerView = ChangeLayerVisibility((ModelDoc2)swDraw, noteLayer, false);
            }

            // Activate the original sheet
            swDraw.ActivateSheet(activeSheetName);
        }

        /// <summary>
        /// Get the number corresponding to the active sheet
        /// </summary>
        /// <param name="swDraw">The pointer to the drawing document</param>
        /// <param name="swSheet">The pointer to the active sheet</param>
        /// <returns>The integer of the active sheet</returns>
        private static int GetSheetNumber(DrawingDoc swDraw, Sheet swSheet)
        {
            // Get sheet names
            string[] sheetNames = GetDrawingSheetNames(swDraw);

            int sheetNumber = 0;

            for (int i = 1; i < (sheetNames.Length + 1); i++)
            {
                if (swSheet.GetName() == sheetNames[i - 1])
                {
                    sheetNumber = i;
                    break;
                }
            }

            return sheetNumber;
        }

        /// <summary>
        /// Get the paper parameter for the specified page dimension
        /// </summary>
        /// <param name="printerName">The name of the printer</param>
        /// <param name="pageDimension">The name of the page dimension</param>
        /// <returns>The code for the paper</returns>
        /// <exception cref="NotImplementedException"></exception>
        private static int GetPaper(string printerName, string pageDimension)
        {
            PrinterSettings settings = new PrinterSettings();

            settings.PrinterName = printerName;

            PaperSizeCollection paperSizes = settings.PaperSizes;

            foreach (PaperSize paperSize in paperSizes)
            {
                if (paperSize.PaperName == pageDimension)
                {
                    return paperSize.RawKind;
                }
            }

            Application.ShowMessageBox("\"No sizes available for the specified printer", CADBooster.SolidDna.SolidWorksMessageBoxIcon.Stop);

            throw new NotImplementedException();
        }
    }
}
