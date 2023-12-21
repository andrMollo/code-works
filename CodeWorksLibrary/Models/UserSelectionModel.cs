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
    internal class UserSelectionModel
    {
        /// <summary>
        /// True to enable export
        /// </summary>
        internal bool Export {  get; set; }

        /// <summary>
        /// True to enable print
        /// </summary>
        internal bool Print {  get; set; }

        /// <summary>
        /// True to enable quantity update
        /// </summary>
        internal bool QtyUpdate { get; set; }

        /// <summary>
        /// True to export again all components
        /// </summary>
        internal bool ExportAgain { get; set; }
    }
}
