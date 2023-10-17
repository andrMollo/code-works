using CADBooster.SolidDna;
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

            var swModel = Application.ActiveModel;

            // Check output path, create if necessary

            // Get file path
            var filePath = swModel.FilePath;

            // Get file name
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            // Check the type of file open
            if (Application.ActiveModel.IsDrawing)
            {
                // Update format
                // Export drawing
                    // Export to PDF
                    // Export to DWG
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
    }
}
