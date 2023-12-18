using SolidWorks.Interop.sldworks;

namespace CodeWorksLibrary.Models
{
    internal class BomModel
    {
        /// <summary>
        /// The pointer to the ModelDoc2 object
        /// </summary>
        internal ModelDoc2 Model {  get; set; }

        /// <summary>
        /// The name of the referenced configuration
        /// </summary>
        internal string Configuration { get; set; }

        /// <summary>
        /// The number of instances of the component
        /// </summary>
        internal double Quantity { get; set; }
    }
}
