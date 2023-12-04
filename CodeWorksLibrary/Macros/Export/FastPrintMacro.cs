using CodeWorksLibrary.Helpers;
using CodeWorksLibrary.Macros.Drawings;
using SolidWorks.Interop.sldworks;
using static CADBooster.SolidDna.SolidWorksEnvironment;
using static CodeWorksLibrary.Helpers.CwLayerManager;

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

                var retChangeLayerView = ChangeLayerVisibility((ModelDoc2)swDraw, noteLayer);


            }
        }
    }
}
