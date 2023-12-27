using CADBooster.SolidDna;
using CodeWorksLibrary.Models;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using static CADBooster.SolidDna.SolidWorksEnvironment;

namespace CodeWorksLibrary.Helpers
{
    internal class CwBomManager
    {
        /// <summary>
        /// Get the flat Bill of Material
        /// </summary>
        /// <param name="swParentComp">The parent component of which to extract the BOM</param>
        /// <param name="bom">The Bill of Material object</param>
        internal static void ComposeFlatBOM(Component2 swParentComp, List<BomElement> bom)
        {
            // Get a list of component
            var vComps = (object[])swParentComp.GetChildren();

            // Loop through all components
            if (vComps.Length != 0 || vComps != null)
            {
                for (int i = 0; i < vComps.Length; i++)
                {
                    // Get the component
                    var swComp = (Component2)vComps[i];

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

                            // If the component is not found, then the component is added to the bom
                            if (bomPos == -1)
                            {
                                // Add the component to the bom
                                BomElement newBomElement = new BomElement();

                                newBomElement.Model = swRefModel;
                                newBomElement.Configuration = swComp.ReferencedConfiguration;
                                newBomElement.Quantity = 1;
                                newBomElement.Path = swRefModel.GetPathName();

                                bom.Add(newBomElement);
                            }
                            else
                            {
                                // Increment the quantity of the BOM element
                                bom[bomPos].Quantity = bom[bomPos].Quantity + 1;
                            }
                        }

                        // If the children are not promoted in this configuration
                        if (bomChildType != (int)swChildComponentInBOMOption_e.swChildComponent_Hide)
                        {
                            // Call again this method on the component to get its children
                            ComposeFlatBOM(swComp, bom);
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
        /// <returns>An integer with the position of the component in the BOM, return -1 if the component is not found</returns>
        private static int FindBomPosition(List<BomElement> bom, Component2 comp)
        {
            int findBomPosition = -1;

            if (bom != null)
            {
                for (int i = 0; i < bom.Count; i++)
                {
                    // Get the full path of the i-th model in the bom
                    var modelPath = bom[i].Model.GetPathName().ToLower();

                    // Get the full path of the analyzed model
                    var compPath = comp.GetPathName().ToLower();

                    // Compare the i-th path to the analyzed one
                    // Only path are compared: different configuration of the same component count as one
                    if (modelPath == compPath)
                    {
                        findBomPosition = i;
                        return findBomPosition;
                    }
                }
            }

            return findBomPosition;
        }
    }
}
