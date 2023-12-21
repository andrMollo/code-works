using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWorksLibrary
{
    internal static class ExportDocument
    {
        #region Public properties
        /// <summary>
        /// The name of the job to be used as export sub-folder
        /// </summary>
        internal static string JobNumber { get; set; }

        /// <summary>
        /// True to print the document, false to export it
        /// </summary>
        internal static bool PrintSelection { get; set; }

        #endregion

        #region Private fields
        /// <summary>
        /// The name of the file without the extension
        /// </summary>
        static string _modelNameNoExt;

        /// <summary>
        /// The path to the export folder, without filename
        /// </summary>
        static string _exportFolderPath;
        #endregion
    }
}
