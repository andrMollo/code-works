using CADBooster.SolidDna;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using static CADBooster.SolidDna.SolidWorksEnvironment;

namespace CodeWorksLibrary.Helpers
{
    internal class CwBomManager
    {
        /// <summary>
        /// Bill of material type class
        /// </summary>
        internal class Bom
        {
            internal ModelDoc2 model;

            internal String configuration;

            internal double quantity;
        }

        /// <summary>
        /// Get the flat Bill of Material
        /// </summary>
        /// <param name="swParentComp">The parent component of which to extract the BOM</param>
        /// <param name="bom">The Bill of Material object</param>
        internal static void GetFlatBOM(Component2 swParentComp, Bom bom)
        {
            // Get a list of component
            var vComps = (Component2[])swParentComp.GetChildren();

            // Loop through all components
            if (vComps.Length != 0 || vComps != null)
            {
                for (int i = 0; i < vComps.Length; i++)
                {
                    // Get the component
                    var swComp = vComps[i];

                    // Proceed only if the component is not suppressed or excluded from the Bill of Material
                    if ((swComp.GetSuppression() != (int)swComponentSuppressionState_e.swComponentSuppressed) && 
                        (swComp.ExcludeFromBOM == false))
                    {
                        ModelDoc2 swRefModel = (ModelDoc2)swComp.GetModelDoc2();

                        // Exit the method if the model isn't loaded
                        if (swRefModel == null)
                        {
                            Application.ShowMessageBox("Modello del componente non caricato", SolidWorksMessageBoxIcon.Stop);
                            return;
                        }

                        // Get the configuration of the model
                        Configuration swRefConfiguration = (Configuration)swRefModel.GetConfigurationByName(swComp.ReferencedConfiguration);

                        // Get the configuration option that handle the visibility of children of the evaluated components
                        int bomChildType = (int)swRefConfiguration.ChildComponentDisplayInBOM;

                        // If the children are not promoted in this configuration
                        if (bomChildType != (int)swChildComponentInBOMOption_e.swChildComponent_Promote)
                        {
                            // Find the BOM position of this component
                            int bomPos = FindBomPosition(bom, swComp);
                        }
                    } 
                }
            }
        }

        /// <summary>
        /// Find the position of a component in the Bil of Material
        /// </summary>
        /// <param name="bom">The Bill of Material object</param>
        /// <param name="swComp">The pointer to the component object</param>
        /// <returns></returns>
        private static int FindBomPosition(Bom bom, Component2 swComp)
        {
            int findBomPosition = -1;



            return findBomPosition;
        }
    }
}
