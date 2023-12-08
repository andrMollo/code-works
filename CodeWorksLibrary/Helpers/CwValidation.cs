﻿using CADBooster.SolidDna;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CADBooster.SolidDna.SolidWorksEnvironment;

namespace CodeWorksLibrary.Helpers
{
    internal class CwValidation
    {
        /// <summary>
        /// Check is there is an open 
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
        /// Check is there is an open 
        /// </summary>
        /// <param name="model">The pointer to the model</param>
        /// <returns>True if there is a file open and saved</returns>
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
        /// Check if there is an open model and if it is a saved drawing
        /// </summary>
        /// <param name="model">The pointer to the model</param>
        /// <returns>True if the active file is a drawing</returns>
        internal static bool ModelIsDrawing(Model model)
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
            if (model.IsDrawing != true)
            {
                Application.ShowMessageBox("Open a drawing to run the macro", SolidWorksMessageBoxIcon.Stop);

                return false;
            }

            return true;
        }
    }
}
