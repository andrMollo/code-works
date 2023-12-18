using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWorksLibrary.Models
{
    internal class AssemblyModel
    {
        /// <summary>
        /// The pointer to the ModelDoc object
        /// </summary>
        internal ModelDoc2 Model {  get; set; }
        
        /// <summary>
        /// The quantity of the assembly
        /// </summary>
        internal string Quantity { get; set; }
    }
}
