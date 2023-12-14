using CodeWorksLibrary.Helpers;
using CodeWorksLibrary.Macros.Files;
using CodeWorksLibrary.Macros.Properties;
using CodeWorksLibrary.Models;
using SolidWorks.Interop.sldworks;
using System.Collections.Generic;
using System.IO;
using static CADBooster.SolidDna.SolidWorksEnvironment;

namespace CodeWorksLibrary.Macros.Export
{
    internal class ExportAssyMacro
    {
        /// <summary>
        /// Export the assembly and all its components
        /// </summary>
        internal static void ExportAssembly()
        {
            var model = Application.ActiveModel;
            #region Validation
            // Check if there is an open assembly
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

            // Get the assembly quantity
            AssemblyModel assemblyModel = new AssemblyModel();
            assemblyModel.Model = model.UnsafeObject;

            var prpManager = new CwPropertyManager();
            assemblyModel.Quantity = prpManager.GetCustomProperty(model.UnsafeObject, GlobalConfig.QuantityProperty);

            // Show export assembly form
            var expAsmForm = new CodeWorksUI.ExportAssemblyForm();
            var expAsmFormRes = expAsmForm.ShowDialog();

            // Get the flat BOM
            List<CwBomManager.Bom> bom = new List<CwBomManager.Bom>();
            CwBomManager.ComposeFlatBOM(rootComp, bom);

            // Export all component in the BOM
            if (bom != null)
            {
                ExportAllComponent(bom, assemblyModel);
            }
        }

        /// <summary>
        /// Export all components in the BOM
        /// </summary>
        /// <param name="bom">The instance of the Bill of Material</param>
        private static void ExportAllComponent(List<CwBomManager.Bom> bom, AssemblyModel assembly)
        {
            if (bom != null)
            {
                for (int i = 0; i < bom.Count; i++)
                {
                    // Write quantity
                    WriteQuantityMacro.WriteQuantity(bom[i].model, bom[i].quantity, assembly.Quantity);

                    // Get model path
                    var modelPath = bom[i].model.GetPathName();

                    // Get drawing path
                    // It assumes drawing and model have the same name and are in the same folder
                    var drwPath = Path.ChangeExtension(modelPath, "SLDDRW");

                    // Export drawing and model preview
                    ExportFileMacro.ExportDrawingAndPreview(drwPath);
                }
            }
        }
    }
}
