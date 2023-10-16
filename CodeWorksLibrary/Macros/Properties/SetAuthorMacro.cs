using CADBooster.SolidDna;
using CodeWorksLibrary.Helpers;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using static CADBooster.SolidDna.SolidWorksEnvironment;

namespace CodeWorksLibrary
{
    internal class SetAuthorMacro
    {
        /// <summary>
        /// Write the author in a custom property of the selected components or of the active model if the selection is empty
        /// </summary>
        public static void SetAuthor()
        {
            // The instance to the active model
            ModelDoc2 swModel = (ModelDoc2)AddIn.swApp.ActiveDoc;

            #region Validation
            // Check if there is an open document and if there is it can't be a drawing
            if (swModel is null)
            {
                Application.ShowMessageBox("Open a document to set the author",SolidWorksMessageBoxIcon.Stop);
                return;
            }
            if (swModel.GetType() == (int)swDocumentTypes_e.swDocDRAWING)
            {
                Application.ShowMessageBox("Open a part or an assembly to set the author", SolidWorksMessageBoxIcon.Stop);
                return;
            }
            #endregion

            // Get the list of selected models
            // If nothing is selected add the active model to the list of model object
            List<ModelDoc2> models = CwSelectionManager.GetSelectedModels(swModel);
        }
    }
}
