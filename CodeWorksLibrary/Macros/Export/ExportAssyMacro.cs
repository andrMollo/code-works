﻿using static CADBooster.SolidDna.SolidWorksEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeWorksLibrary.Helpers;
using SolidWorks.Interop.sldworks;

namespace CodeWorksLibrary.Macros.Export
{
    internal class ExportAssyMacro
    {
        internal static void ExportAssembly()
        {
            #region Validation
            var model = Application.ActiveModel;

            var isAssemblyOpen = CwValidation.AssemblyIsOpen(model);

            if (isAssemblyOpen == false)
            {
                return;
            }
            #endregion

            // Get the assembly object
            var swAssy = (AssemblyDoc)model.UnsafeObject;

            // Resolve lightweight components
            var resResolve = swAssy.ResolveAllLightWeightComponents(false);

            // Get the active configuration
            var swConf = model.UnsafeObject.ConfigurationManager.ActiveConfiguration;

            // Get the root component
            var rootComp = swConf.GetRootComponent3(true);

            // Get the flat BOM
            List<CwBomManager.Bom> bom = new List<CwBomManager.Bom>();
            CwBomManager.GetFlatBOM(rootComp, bom);
        }
    }
}
