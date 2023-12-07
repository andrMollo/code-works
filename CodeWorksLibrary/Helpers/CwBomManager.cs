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

        internal static void GetFlatBOM(Component2 swParentComp, Bom bom)
        {
            bom.configuration = "new config";
        }
    }
}
