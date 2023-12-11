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
        internal ModelDoc2 Model {  get; set; }
        internal string Quantity { get; set; }
    }
}
