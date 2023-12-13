using CodeWorksLibrary.Helpers;
using SolidWorks.Interop.sldworks;
using System.Collections.Generic;
using static CADBooster.SolidDna.SolidWorksEnvironment;

namespace CodeWorksLibrary
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

            ModelDoc2 swModel = model.UnsafeObject;

            // Get the list of selected models
            // If nothing is selected add the active model to the list of model object
            List<ModelDoc2> models = CwSelectionManager.GetSelectedModels(swModel);

            // Get the username connected to PDM
            string userName = CwPdmManager.GetPdmUserName();

            // Set the username in each member of the list of model
            for (int i = 0; i < models.Count; i++)
            {
                var prpManager = new CwPropertyManager();

                prpManager.SetCustomProperty(models[i], GlobalConfig.AuthorPropName, userName);
            }
        }
    }
}
