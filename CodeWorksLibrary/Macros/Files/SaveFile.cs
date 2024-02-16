using CADBooster.SolidDna;
using CodeWorksLibrary.Helpers;
using System;
using static CADBooster.SolidDna.SolidWorksEnvironment;

namespace CodeWorksLibrary.Macros.Files
{
    internal static class SaveFile
    {
        #region Public methods

        /// <summary>
        /// Save a copy of the active, or the selected component with its drawing. 
        /// Replace all the selected instances with the new component
        /// </summary>
        public static void MakeIndependentWithDrawingMacro()
        {
            Model model = Application.ActiveModel;

            if (CwValidation.Model3dIsOpen(model) == false)
            {
                CwMessage.OpenAModel();
                return;
            }

            if (model.IsPart)
            {
                // Get the new path
                string pathNewFile = GetNewFilePath(model);
                // Save the file as a copy
                // Update file properties **common**
                // Save drawing **common**
                // Replace drawing reference **common**
                // Replace reference to old part
            }
            else
            {
                // Check whether or not there are selected components
                // If nothing is selected
                    // Save the file as a copy
                    // Update file properties **common**
                    // Save drawing **common**
                    // Replace drawing reference **common**
                    // Replace reference to old part
                // If there are selected components
                    // Get all the selected model
                    // Check that the selection
                    // Save the file as a copy
                    // Update file properties **common**
                    // Save drawing **common**
                    // Replace drawing reference **common**
                    // Replace reference to old part
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Get the full path for the new file
        /// </summary>
        /// <param name="model">The pointer to the active SolidDNA.Model object</param>
        /// <returns>A string with the full path for the new file</returns>
        private static string GetNewFilePath(Model model)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
