using CADBooster.SolidDna;
using static CADBooster.SolidDna.SolidWorksEnvironment;

namespace CodeWorksLibrary.Helpers
{
    internal class CwValidation
    {
        /// <summary>
        /// Check is there is a open document: part, assembly or drawing
        /// </summary>
        /// <param name="model">The pointer to the model</param>
        /// <returns>True if there is a file open and saved</returns>
        internal static bool ModelIsOpen(Model model)
        {
            // Check if there is an open document
            if (model == null)
            {
                Application.ShowMessageBox("Open a file", SolidWorksMessageBoxIcon.Stop);

                return false;
            }

            // Check if the open file has already been saved
            if (model.HasBeenSaved == false)
            {
                Application.ShowMessageBox("Save the file to run the macro", SolidWorksMessageBoxIcon.Stop);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Check is there is an open assembly
        /// </summary>
        /// <param name="model">The pointer to the model</param>
        /// <returns>True if there is an assembly open and saved</returns>
        internal static bool AssemblyIsOpen(Model model)
        {
            // Check if there is an open document
            if (model == null)
            {
                Application.ShowMessageBox("Open a file", SolidWorksMessageBoxIcon.Stop);

                return false;
            }

            // Check if the open file has already been saved
            if (model.HasBeenSaved == false)
            {
                Application.ShowMessageBox("Save the file to run the macro", SolidWorksMessageBoxIcon.Stop);

                return false;
            }

            if (model.IsAssembly != true)
            {
                Application.ShowMessageBox("Open an assembly to run the macro", SolidWorksMessageBoxIcon.Stop);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Check if there is an open drawing
        /// </summary>
        /// <param name="model">The pointer to the model</param>
        /// <returns>True if there is a drawing open and saved</returns>
        internal static bool DrawingIsOpen(Model model)
        {
            // Check if there is an open document
            if (model == null)
            {
                Application.ShowMessageBox("Open a file", SolidWorksMessageBoxIcon.Stop);

                return false;
            }

            // Check if the open file has already been saved
            if (model.HasBeenSaved == false)
            {
                Application.ShowMessageBox("Save the file to run the macro", SolidWorksMessageBoxIcon.Stop);

                return false;
            }

            // Check if the open file is not a drawing
            if (model.IsDrawing != true)
            {
                Application.ShowMessageBox("Open a drawing to run the macro", SolidWorksMessageBoxIcon.Stop);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Check if there is an open part o assembly
        /// </summary>
        /// <param name="model">The pointer to the model</param>
        /// <returns>True if there is a part or assembly open and saved</returns>
        internal static bool Model3dIsOpen(Model model)
        {
            // Check if there is an open document
            if (model == null)
            {
                Application.ShowMessageBox("Open a file", SolidWorksMessageBoxIcon.Stop);

                return false;
            }

            // Check if the open file has already been saved
            if (model.HasBeenSaved == false)
            {
                Application.ShowMessageBox("Save the file to run the macro", SolidWorksMessageBoxIcon.Stop);

                return false;
            }

            // Check if the open file is a drawing
            if (model.IsDrawing == true)
            {
                Application.ShowMessageBox("Open a part or assembly to run the macro", SolidWorksMessageBoxIcon.Stop);

                return false;
            }

            return true;
        }
    }
}
