using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWorksWpfLibrary.Interfaces
{
    internal interface ICloseWindow
    {
        /// <summary>
        /// Action that close the window
        /// </summary>
        Action Close { get; set; }

        /// <summary>
        /// Action that cancel the window
        /// </summary>
        Action Cancel { get; set; }
    }
}
