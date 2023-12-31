﻿using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace CodeWorksLibrary.Helpers
{
    public class CwLayerManager
    {

        /// <summary>
        /// Toggle the layer visibility
        /// </summary>
        /// <param name="swModel">The pointer to the model</param>
        /// <param name="layerName">THe name of the layer to be changed</param>
        /// <returns>True if the layer is made visible, false if the layer is hidden</returns>
        public static bool ToggleLayerVisibility(ModelDoc2 swModel, string layerName)
        {
            // Get the model layer manager
            LayerMgr swLayerMgr = (LayerMgr)swModel.GetLayerManager();

            // Try to get the layer
            Layer swLayer = (Layer)swLayerMgr.GetLayer(layerName);

            if (swLayer == null)
            {
                // Create the layer
                swLayer = CreateLayer(swModel, layerName);
            }

            // Change layer visibility
            if (swLayer != null)
            {
                if (swLayer.Visible == true)
                {
                    swLayer.Visible = false;
                    return false;
                }
                else
                {
                    swLayer.Visible = true;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Toggle the layer visibility
        /// </summary>
        /// <param name="swModel">The pointer to the model</param>
        /// <param name="layerName">THe name of the layer to be changed</param>
        /// <param name="layerVisibility">True to make the layer visible, false to hidden it</param>
        /// <returns>True if the layer is made visible, false if the layer is hidden</returns>
        public static bool ChangeLayerVisibility(ModelDoc2 swModel, string layerName, bool layerVisibility)
        {
            // Get the model layer manager
            LayerMgr swLayerMgr = (LayerMgr)swModel.GetLayerManager();

            // Try to get the layer
            Layer swLayer = (Layer)swLayerMgr.GetLayer(layerName);

            if (swLayer == null)
            {
                // Create the layer
                swLayer = CreateLayer(swModel, layerName);
            }

            // Change layer visibility
            if (swLayer != null)
            {
                if (layerVisibility == true)
                {
                    swLayer.Visible = true;
                    return true;
                }
                else if (layerVisibility == false)
                {
                    swLayer.Visible = false;
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Create a new layer
        /// </summary>
        /// <param name="swModel">The pointer to the model</param>
        /// <param name="layerName">THe name of the layer to be changed</param>
        /// <returns>The pointer to the layer object</returns>
        public static Layer CreateLayer(ModelDoc2 swModel, string layerName)
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
