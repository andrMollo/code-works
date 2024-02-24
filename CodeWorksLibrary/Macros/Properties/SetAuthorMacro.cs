using CADBooster.SolidDna;
using CodeWorksLibrary.Helpers;
using System.Collections.Generic;
using static CADBooster.SolidDna.SolidWorksEnvironment;

namespace CodeWorksLibrary.Macros.Properties
{
    internal class SetAuthorMacro
    {
        /// <summary>
        /// Write the author in a custom property of the selected components or of the active model if the selection is empty
        /// </summary>
        internal static void SetAuthor()
        {
            // The instance to the active model
            var model = Application.ActiveModel;

            #region Validation
            // Check if there is an open document and if there is it can't be a drawing
            var isModelOpen = CwValidation.Model3dIsOpen(model);

            if (isModelOpen == false)
            {
                return;
            }
            #endregion

            // Get the list of selected models
            // If nothing is selected add the active model to the list of model object
            List<Model> models = CwSelectionManager.GetSelectedModels(model);

            // Get the username connected to PDM
            string userName = CwPdmManager.GetPdmUserName();

            foreach (Model selectedModel in  models)
            {
                selectedModel.SetCustomProperty(GlobalConfig.AuthorPropName, userName);
            }
        }
    }
}
