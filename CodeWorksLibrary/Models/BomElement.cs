using SolidWorks.Interop.sldworks;

namespace CodeWorksLibrary.Models
{
    public class BomElement
    {
        /// <summary>
        /// The pointer to the ModelDoc2 object
        /// </summary>
        public ModelDoc2 Model {  get; set; }

        /// <summary>
        /// The name of the referenced configuration
        /// </summary>
        public string Configuration { get; set; }

        /// <summary>
        /// The number of instances of the component
        /// </summary>
        public double Quantity { get; set; }

        /// <summary>
        /// The full path to the component
        /// </summary>
        public string Path { get; set; }
    }
}
