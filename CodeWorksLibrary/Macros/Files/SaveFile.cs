using CADBooster.SolidDna;
using CodeWorksLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CADBooster.SolidDna.SolidWorksEnvironment;

namespace CodeWorksLibrary.Macros.Files
{
    internal static class SaveFile
    {
        #region Public methods

        /// <summary>
        /// Save a copy of a component with its drawing
        /// </summary>
        public static void MakeIndependentWithDrawingMacro()
        {
            Model model = Application.ActiveModel;

            if (CwValidation.Model3dIsOpen(model) == false)
            {
                CwMessage.OpenAModel();
                return;
            }

            MakeIndependentWithDrawing(model);
        }

        #endregion

        /// <summary>
        /// Save a copy of a component with its drawing
        /// </summary>
        /// <param name="model">The pointer to the active SolidDNA.Model object</param>
        private static void MakeIndependentWithDrawing(Model model)
        {
            
        }

    }
}
