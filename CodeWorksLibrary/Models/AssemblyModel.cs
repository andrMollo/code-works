using SolidWorks.Interop.sldworks;

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
