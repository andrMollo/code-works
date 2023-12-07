using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            }
        }
    }
}
