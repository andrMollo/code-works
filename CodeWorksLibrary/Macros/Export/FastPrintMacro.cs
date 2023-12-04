using CodeWorksLibrary.Helpers;
using CodeWorksLibrary.Macros.Drawings;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Drawing.Printing;
using static CADBooster.SolidDna.SolidWorksEnvironment;
using static CodeWorksLibrary.Helpers.CwLayerManager;
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

            // Get sheet names
            string[] sheetNames = UpdateFormatMacro.GetDrawingSheetNames(swDraw);

            // Loop through sheets
            for (int i = 0; i < sheetNames.Length; i++)
            {
                // Get the i-th sheet
                var swSheet = swDraw.get_Sheet(sheetNames[i]);

                swDraw.ActivateSheet(sheetNames[i]);

                UpdateFormatMacro.UpgradeSheetFormat(swDraw, swSheet);

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

                // Revert print setup to original

                retChangeLayerView = ChangeLayerVisibility((ModelDoc2)swDraw, noteLayer, false);
            }
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
