﻿using CADBooster.SolidDna;
using System.IO;
using static CADBooster.SolidDna.SolidWorksEnvironment;

namespace CodeWorksLibrary.Helpers
{
    public static class CwValidation
    {
        /// <summary>
        /// Check is there is a open document: part, assembly or drawing
        /// </summary>
        /// <param name="model">The pointer to the model</param>
        /// <returns>True if there is a file open and saved</returns>
        public static bool ModelIsOpen(Model model)
        {
            // Check if there is an open document
            if (model == null)
            {
                CwMessage.OpenAFile();

                return false;
            }

            // Check if the open file has already been saved
            if (model.HasBeenSaved == false)
            {
                CwMessage.SaveFile();

                return false;
            }

            return true;
        }

        /// <summary>
        /// Check is there is an open assembly
        /// </summary>
        /// <param name="model">The pointer to the model</param>
        /// <returns>True if there is an assembly open and saved</returns>
        public static bool AssemblyIsOpen(Model model)
        {
            // Check if there is an open document
            if (model == null)
            {
                CwMessage.OpenAFile();

                return false;
            }

            // Check if the open file has already been saved
            if (model.HasBeenSaved == false)
            {
                CwMessage.SaveFile();

                return false;
            }

            if (model.IsAssembly != true)
            {
                CwMessage.OpenAssembly();

                return false;
            }

            return true;
        }

        /// <summary>
        /// Check if there is an open drawing
        /// </summary>
        /// <param name="model">The pointer to the model</param>
        /// <returns>True if there is a drawing open and saved</returns>
        public static bool DrawingIsOpen(Model model)
        {
            // Check if there is an open document
            if (model == null)
            {
                CwMessage.OpenAFile();

                return false;
            }

            // Check if the open file has already been saved
            if (model.HasBeenSaved == false)
            {
                CwMessage.SaveFile();

                return false;
            }

            // Check if the open file is not a drawing
            if (model.IsDrawing != true)
            {
                CwMessage.OpenADrawing();

                return false;
            }

            return true;
        }

        /// <summary>
        /// Check if there is an open part o assembly
        /// </summary>
        /// <param name="model">The pointer to the model</param>
        /// <returns>True if there is a part or assembly open and saved</returns>
        public static bool Model3dIsOpen(Model model)
        {
            // Check if there is an open document
            if (model == null)
            {
                CwMessage.OpenAFile();

                return false;
            }

            // Check if the open file has already been saved
            if (model.HasBeenSaved == false)
            {
                CwMessage.SaveFile();

                return false;
            }

            // Check if the open file is a drawing
            if (model.IsDrawing == true)
            {
                CwMessage.OpenAModel();

                return false;
            }

            return true;
        }

        /// <summary>
        /// Remove invalid path characters
        /// </summary>
        /// <param name="filename">The string to be checked</param>
        /// <returns>The input string with the invalid characters removed</returns>
        public static string RemoveInvalidPathChars(string filename)
        {
            return string.Concat(filename.Split(Path.GetInvalidPathChars()));
        }

        /// <summary>
        /// Remove invalid filename characters
        /// </summary>
        /// <param name="filename">The string to be checked</param>
        /// <returns>The input string with the invalid characters removed</returns>
        public static string RemoveInvalidFileNameChars(string filename)
        {
            return string.Concat(filename.Split(Path.GetInvalidFileNameChars()));
        }
    }
}
