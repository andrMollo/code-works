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
            /*
             TODOs:                         
             Check if there is an open file 
             Check if the file is saved     
             Check what type of file is open
             Process drawing
              Export drawing
                  Update sheet format for all sheet
                  Export to PDF
                  Export to DWG
              Get root model
              Export model
             Process model
              Export model
                  Export model to STEP
                  Export model to PNG
              Check if drawing exists
              Open drawing
              Process drawing
             Export
              Compose output filename
              Create directory is not present
            */

            #region Validation
            // Check if there is an open document and if there is it can't be a drawing

            if (Application.ActiveModel == null)
            {
                Application.ShowMessageBox("Open a file", CADBooster.SolidDna.SolidWorksMessageBoxIcon.Stop);

                return;
            }

            // Check if the open file has already been saved
            if (Application.ActiveModel.HasBeenSaved == false)
            {
                Application.ShowMessageBox("Save the file to run the macro", CADBooster.SolidDna.SolidWorksMessageBoxIcon.Stop);

                return;
            }
            #endregion
        }
    }
}
