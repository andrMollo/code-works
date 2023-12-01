using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWorksLibrary.Helpers
{
    internal class CwLayerManager
    {
        /// <summary>
        /// Toggle the layer visibility
        /// </summary>
        /// <param name="swModel"></param>
        /// <param name="layerName"></param>
        /// <returns></returns>
        internal static bool ChangeLayerVisibility(ModelDoc2 swModel, string layerName)
        {
            // Get the model layer manager
            LayerMgr swLayerMgr = (LayerMgr)swModel.GetLayerManager();

            // Try to get the layer
            var swLayer = swLayerMgr.GetLayer(layerName);

            if (swLayer == null)
            {
                // Create the layer
            }
            
            // Change layer visibility

            return true;
        }

        /// <summary>
        /// Create a new layer
        /// </summary>
        /// <returns></returns>
        internal static Layer CreateLayer()
        {
            return null;
        }
    }
}
