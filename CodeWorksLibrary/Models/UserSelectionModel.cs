using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWorksLibrary.Models
{
    /// <summary>
    /// Store the user selection for export option
    /// </summary>
    public class UserSelectionModel
    {
        /// <summary>
        /// True to enable export
        /// </summary>
        public bool Export {  get; set; }

        /// <summary>
        /// True to enable print
        /// </summary>
        public bool Print {  get; set; }

        /// <summary>
        /// True to enable quantity update
        /// </summary>
        public bool QtyUpdate { get; set; }

        /// <summary>
        /// True to export again all components
        /// </summary>
        public bool ExportAgain { get; set; }

        /// <summary>
        /// The string with the quantity value selected by the user
        /// </summary>
        public string Quantity { get; set; }
    }
}
