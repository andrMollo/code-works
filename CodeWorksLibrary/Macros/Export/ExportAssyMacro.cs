using CADBooster.SolidDna;
using CodeWorksLibrary.Helpers;
using CodeWorksLibrary.Macros.Files;
using CodeWorksLibrary.Macros.Properties;
using CodeWorksLibrary.Models;
using SolidWorks.Interop.sldworks;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
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
            var model = SolidWorksEnvironment.Application.ActiveModel;
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

            // Show assembly quantity in the form
            var expAsmForm = new CodeWorksUI.ExportAssemblyForm();
            expAsmForm.AssemblyQty = assemblyModel.Quantity;

            // Show export assembly form
            var expAsmFormRes = expAsmForm.ShowDialog();

            // Check button click
            if (expAsmFormRes == DialogResult.OK)
            {
                // An instance of user selection
                var userSel = new UserSelectionModel();

                // Compile user selection based on form selection
                userSel.Export = expAsmForm.ExportCheck;
                userSel.Print = expAsmForm.PrintCheck;
                userSel.QtyUpdate = expAsmForm.QuantityCheck;

                // Get the assembly quantity back from the winform
                assemblyModel.Quantity = expAsmForm.AssemblyQty;

                // Get the flat BOM
                List<BomModel> bom = new List<BomModel>();
                CwBomManager.ComposeFlatBOM(rootComp, bom);

                // Export all component in the BOM
                if (bom != null)
                {
                    ExportAllComponent(bom, assemblyModel, userSel);
                }

                SolidWorksEnvironment.Application.ShowMessageBox("Macro complete", SolidWorksMessageBoxIcon.Information);
            }
            else if (expAsmFormRes == DialogResult.Cancel)
            {
                SolidWorksEnvironment.Application.ShowMessageBox("Macro terminated", SolidWorksMessageBoxIcon.Stop);
            }

        }

        /// <summary>
        /// Export all components in the BOM
        /// </summary>
        /// <param name="bom">The instance of the Bill of Material</param>
        /// <param name="assembly">The assembly model object</param>
        /// <param name="userSelection">The model with the option the user selected</param>
        private static void ExportAllComponent(List<BomModel> bom, AssemblyModel assembly, UserSelectionModel userSelection)
        {
            if (bom != null)
            {
                // Initiate a log model
                var asmLog = new Helpers.Logger();

                // Set the log path
                asmLog.LogPath = GlobalConfig.LogPath + @"log.txt";

                foreach (var comp in bom)
                {
                    // Update the components quantity is the user selected the option
                    if (userSelection.QtyUpdate == true)
                    {
                        // Write quantity
                        WriteQuantityMacro.WriteQuantity(comp.Model, comp.Quantity, assembly.Quantity);
                    }

                    // Get model path
                    var modelPath = comp.Model.GetPathName();

                    // Get drawing path
                    // It assumes drawing and model have the same name and are in the same folder
                    var drwPath = Path.ChangeExtension(modelPath, "SLDDRW");

                    if (File.Exists(drwPath) == false)
                    {
                        return;
                    }

                    // If one between print and export option is selected open the drawing
                    if (userSelection.Export == true || userSelection.Print == true)
                    {
                        var drwModel = SolidWorksEnvironment.Application.OpenFile(drwPath, options: OpenDocumentOptions.Silent);

                        if (drwModel == null)
                        {
                            return;
                        }

                        // Export the component drawing and preview if the user selected the option
                        if (userSelection.Export == true)
                        {
                            // Export drawing and model preview
                            ExportFileMacro.ExportDrawingAndPreview(drwModel);
                        }

                        // Print the drawing if the user selected the option
                        if (userSelection.Print == true)
                        {
                            FastPrintMacro.PrintFile(drwModel);
                        }

                        asmLog.WirteLogWithDate(modelPath);
                    }
                }
            }
        }
    }
}
