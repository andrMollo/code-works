using CodeWorksLibrary.Helpers;
using CodeWorksLibrary.Models;
using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CADBooster.SolidDna.SolidWorksEnvironment;
using static CodeWorksLibrary.Helpers.CwBomManager;

namespace CodeWorksLibrary.Macros.Properties
{
    internal class WriteQuantityMacro
    {
        /// <summary>
        /// Write the quantity to all the components in the flat bom
        /// </summary>
        internal static void WriteComponentsQuantity()
        {
            #region Validation
            var model = Application.ActiveModel;

            var isAssemblyOpen = CwValidation.AssemblyIsOpen(model);

            if (isAssemblyOpen == false)
            {
                return;
            }
            #endregion

            // Get the assembly object
            var swAssy = (AssemblyDoc)model.UnsafeObject;

            // Resolve lightweight components
            var resResolve = swAssy.ResolveAllLightWeightComponents(false);

            // Get the active configuration
            var swConf = model.UnsafeObject.ConfigurationManager.ActiveConfiguration;

            // Get the root component
            var rootComp = swConf.GetRootComponent3(true);

            // Get the flat BOM
            List<BomElement> bom = new List<BomElement>();
            CwBomManager.ComposeFlatBOM(rootComp, bom);

            // Get the assembly quantity
            var prpManager = new CwPropertyManager();
            var assemblyQty = prpManager.GetCustomProperty(model.UnsafeObject, GlobalConfig.QuantityProperty);

            // Write quantity to components
            WriteQuantityAllComponents(bom, assemblyQty);

            Application.ShowMessageBox("Macro terminated", CADBooster.SolidDna.SolidWorksMessageBoxIcon.Information);
        }

        /// <summary>
        /// Write the quantity to all the components in the flat bom
        /// </summary>
        /// <param name="bom">A BOM instance with the components for the quantities to be updated</param>
        /// <param name="assemblyQty">The quantity of the main assembly</param>
        private static void WriteQuantityAllComponents(List<BomElement> bom, string assemblyQty)
        {
            var assQty = 0.0;

            // Try to convert the assembly quantity to a double
            try
            {
                assQty = Convert.ToDouble(assemblyQty);
            }
            catch (Exception ex)
            {
                Application.ShowMessageBox("Assembly quantity can't be converted to a number " + ex.Message, CADBooster.SolidDna.SolidWorksMessageBoxIcon.Stop);
                goto finally_;
            }

            if (assQty > 0)
            {
                if (bom != null)
                {
                    for (int i = 0; i < bom.Count; i++)
                    {
                        // Get the quantity saved in the BOM
                        var bomQty = bom[i].Quantity;

                        // Compose the component quantity multiplying the bom quantity for the assembly one
                        var componentQty = bomQty * assQty;

                        var prpQtyValue = componentQty.ToString();

                        // Write the in the custom properties
                        var propertyMgr = new CwPropertyManager();
                        propertyMgr.SetCustomProperty(bom[i].Model, GlobalConfig.QuantityProperty, prpQtyValue);
                    }
                }   
            }
            else
            {
                Application.ShowMessageBox("Assembly quantity must be greater than 0", CADBooster.SolidDna.SolidWorksMessageBoxIcon.Stop);
            }

        finally_:
            return;
        }

        /// <summary>
        /// Write the quantity to a single component in the flat bom
        /// </summary>
        /// <param name="swModel">The pointer to the model</param>
        /// <param name="quantity">The quantity to be written tin the custom property</param>
        /// <param name="assemblyQty">The quantity of the main assembly</param>
        internal static void WriteQuantity(ModelDoc2 swModel, double quantity, double assemblyQty)
        {
            if (assemblyQty > 0)
            {
                var componentQty = quantity * assemblyQty;

                var prpQtyValue = componentQty.ToString();

                // Write the in the custom properties
                var propertyMgr = new CwPropertyManager();
                propertyMgr.SetCustomProperty(swModel, GlobalConfig.QuantityProperty, prpQtyValue);
            }
            else
            {
                Application.ShowMessageBox("Assembly quantity must be greater than 0", CADBooster.SolidDna.SolidWorksMessageBoxIcon.Stop);
            }
        }
    }
}
