using CodeWorksLibrary.Helpers;
using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CADBooster.SolidDna.SolidWorksEnvironment;

namespace CodeWorksLibrary.Macros.Properties
{
    internal class WriteQuantityMacro
    {
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
            List<CwBomManager.Bom> bom = new List<CwBomManager.Bom>();
            CwBomManager.ComposeFlatBOM(rootComp, bom);

            // Get the assembly quantity
            var prpManager = new CwPropertyManager();
            var assemblyQty = prpManager.GetCustomProperty(model.UnsafeObject, GlobalConfig.QuantityProperty);

            // Write quantity to components
            WriteQuantity(bom, assemblyQty);
        }

        /// <summary>
        /// Write the quantity to the components in the flat bom
        /// </summary>
        /// <param name="bom">A BOM instance with the components for the quantities to be updated</param>
        /// <param name="assemblyQty">The quantity of the main assembly</param>
        /// <exception cref="NotImplementedException"></exception>
        private static void WriteQuantity(List<CwBomManager.Bom> bom, string assemblyQty)
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
                        var bomQty = bom[i].quantity;

                        // Compose the component quantity multiplying the bom quantity for the assembly one
                        var componentQty = bomQty * assQty;

                        var prpQtyValue = componentQty.ToString();

                        // Write the in the custom properties
                        var propertyMgr = new CwPropertyManager();
                        propertyMgr.SetCustomProperty(bom[i].model, GlobalConfig.QuantityProperty, prpQtyValue);
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
    }
}
