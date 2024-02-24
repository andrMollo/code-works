using CADBooster.SolidDna;
using SolidWorks.Interop.sldworks;
using System.Collections.Generic;

namespace CodeWorksLibrary.Helpers
{
    public class CwSelectionManager
    {
        /// <summary>
        /// Get the Component2 objects of the selection
        /// </summary>
        /// <param name="selectionMgr">The model selection manager object</param>
        /// <returns>A list of Component2 objects</returns>
        private static List<Component2> GetSelectedComponents(SelectionMgr selectionMgr)
        {
            List<Component2> output = new List<Component2>();

            for (int i = 0; i < selectionMgr.GetSelectedObjectCount2(-1); i++)
            {
                Component2 swComponent = (Component2)selectionMgr.GetSelectedObjectsComponent4(i + 1, -1);
                output.Add(swComponent);
            }

            return output;
        }

        /// <summary>
        /// Get the list of selected models or the active model if the selection is empty
        /// </summary>
        /// <param name="model">The SolidDNA Model object of the active document</param>
        /// <returns>The list of selected models or the active Model if the selection is empty</returns>
        public static List<Model> GetSelectedModels(Model model)
        {
            List<Model> output = new List<Model>();
            
            // Get the list of selected components
            List<Component2> vComp = new List<Component2>();
            ModelDoc2 swModel = (ModelDoc2)model.UnsafeObject;

            vComp = GetSelectedComponents((SelectionMgr)swModel.SelectionManager);

            // If nothing is selected returns the active model
            if (vComp.Count == 0)
            {
                output.Add(model);

            }
            else
            {
                for (int i = 0; i < vComp.Count; i++)
                {
                    ModelDoc2 selectedSwModel = (ModelDoc2)vComp[i].GetModelDoc2();

                    Model selectedModel = new Model(selectedSwModel);

                    output.Add(selectedModel);
                }
            }

            return output;
        }
    }
}
