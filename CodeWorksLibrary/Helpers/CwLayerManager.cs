using CADBooster.SolidDna;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
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
                swLayer = CreateLayer(swModel, layerName);
            }
            
            // Change layer visibility

            return true;
        }

        /// <summary>
        /// Create a new layer
        /// </summary>
        /// <returns>The pointer to the layer object</returns>
        internal static Layer CreateLayer(ModelDoc2 swModel, string layerName)
        {
            DrawingDoc swDraw = (DrawingDoc)swModel;

            var retCreateLeyer = swDraw.CreateLayer2(layerName,
                "",
                0,
                (int)swLineStyles_e.swLineCONTINUOUS,
                (int)swLineWeights_e.swLW_NORMAL,
                true,
                true);

            if (retCreateLeyer == true)
            {
                // Get the model layer manager
                LayerMgr swLayerMgr = (LayerMgr)swModel.GetLayerManager();

                // Try to get the layer
                var newLayer = (Layer)swLayerMgr.GetLayer(layerName);

                return newLayer;
            }
            else
            {
                return null;                
            }
        }
    }
}
