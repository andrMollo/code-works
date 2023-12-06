using SolidWorks.Interop.sldworks;
using System.Collections.Generic;

namespace CodeWorksLibrary.Helpers
{
    internal class CwSelectionManager
    {
        /// <summary>
        /// Get the Component2 objects of the selection
        /// </summary>
        /// <param name="selectionMgr">The model selection manager object</param>
        /// <returns>A list of Component2</returns>
        internal static List<Component2> GetSelectedComponents(SelectionMgr selectionMgr)
        {
            List<Component2> swCompList = new List<Component2>();

            for (int i = 0; i < selectionMgr.GetSelectedObjectCount2(-1); i++)
            {
                Component2 swComponent = (Component2)selectionMgr.GetSelectedObjectsComponent4(i + 1, -1);
                swCompList.Add(swComponent);
            }

            return swCompList;
        }

        /// <summary>
        /// Get the list of selected models or the active model if the selection is empty
        /// </summary>
        /// <param name="model">The active ModelDoc2 instance</param>
        /// <returns>The list of selected models</returns>
        internal static List<ModelDoc2> GetSelectedModels(ModelDoc2 model)
        {
            // Get the list of selected components
            List<Component2> vComp = new List<Component2>();

            vComp = GetSelectedComponents((SelectionMgr)model.SelectionManager);

            List<ModelDoc2> models = new List<ModelDoc2>();

            if (vComp.Count == 0)
            {
                models.Add(model);

            }
            else
            {
                for (int i = 0; i < vComp.Count; i++)
                {
                    ModelDoc2 selModel = (ModelDoc2)vComp[i].GetModelDoc2();
                    models.Add(selModel);
                }
            }

            return models;
        }
    }
}
