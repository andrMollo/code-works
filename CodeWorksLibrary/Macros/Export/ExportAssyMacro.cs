using CADBooster.SolidDna;
using CodeWorksLibrary.Helpers;
using CodeWorksLibrary.Macros.Files;
using CodeWorksLibrary.Macros.Properties;
using CodeWorksLibrary.Models;
using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static CADBooster.SolidDna.SolidWorksEnvironment;

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

            // Get the assembly quantity
            AssemblyModel assemblyModel = new AssemblyModel();
            assemblyModel.Model = model.UnsafeObject;

            var prpManager = new CwPropertyManager();
            assemblyModel.Quantity = prpManager.GetCustomProperty(model.UnsafeObject, GlobalConfig.QuantityProperty);

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

                    // Initialize drawing model
                    Model drwModel = null;

                    // Check if drawing exists
                    if (File.Exists(drwPath))
                    {
                        // Open the drawing model
                        drwModel = Application.OpenFile(drwPath, options: OpenDocumentOptions.Silent);

                        if (drwModel != null)
                        {
                            // Get the list of open model
                            List<Model> models = Application.OpenDocuments().ToList();

                            // If the drawing model is already open activate it
                            if (models.Contains(drwModel) != true)
                            {
                                int activeErr = 0;
                                Application.UnsafeObject.IActivateDoc3(Path.GetFileName(drwPath), true, activeErr);
                            }

                            // Export drawing
                            ExportFileMacro.ExportDrawing(drwModel);

                            // Get drawing root model
                            Model rootModel = ExportFileMacro.GetRootModel(drwModel);

                            // Export preview
                            ExportFileMacro.ExportModelAsPng(rootModel);

                            // Close the model
                            Application.CloseFile(drwPath);
                        }
                    }
                }
            }
        }
    }
}
