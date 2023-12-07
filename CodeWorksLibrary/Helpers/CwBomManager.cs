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
            ModelDoc2 model;

            String configuration;

            double quantity;
        }

        internal static void GetFlatBOM(Bom Bom)
        {

        }
    }
}
