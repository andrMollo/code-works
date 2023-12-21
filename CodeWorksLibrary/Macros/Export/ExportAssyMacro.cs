using CADBooster.SolidDna;
using CodeWorksLibrary.Helpers;
using CodeWorksLibrary.Macros.Files;
using CodeWorksLibrary.Macros.Properties;
using CodeWorksLibrary.Models;
using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Logger = CodeWorksLibrary.Helpers.Logger;

namespace CodeWorksLibrary.Macros.Export
{
    internal class ExportAssyMacro
    {
        #region Public properties
        /// <summary>
        /// The job number
        /// </summary>
        public static string JobNumber { get; set; }

        /// <summary>
        /// The log object for the assembly export
        /// </summary>
        public static Logger AssExpLog { get; set; }
        #endregion

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

            // Get the AssemblyModel
            AssemblyModel assemblyModel = new AssemblyModel();
            assemblyModel.Model = model.UnsafeObject;

            // Resolve lightweight components
            var resResolve = swAssy.ResolveAllLightWeightComponents(false);

            // Get the active configuration
            var swConf = assemblyModel.Model.ConfigurationManager.ActiveConfiguration;

            // Get the root component
            var rootComp = swConf.GetRootComponent3(true);

            // Get the assembly quantity
            var prpManager = new CwPropertyManager();
            assemblyModel.Quantity = prpManager.GetCustomProperty(assemblyModel.Model, GlobalConfig.QuantityProperty);

            // Compose the full path to the logger
            AssExpLog = new Logger();
            AssExpLog.LogPath = Logger.ComposeLogPath(string.Empty);

            // Initiate form
            var expAsmForm = new CodeWorksUI.ExportAssemblyForm();

            // Check if log file already exists
            expAsmForm.LogFilePath = AssExpLog.LogPath;

            // Show assembly quantity in the form
            expAsmForm.AssemblyQty = assemblyModel.Quantity;

            // Show export assembly form
            var expAsmFormRes = expAsmForm.ShowDialog();

            // Check button click
            if (expAsmFormRes == DialogResult.OK)
            {
                // Start timer
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                // Rebuild assembly an all components
                var asmRebuildRet = assemblyModel.Model.ForceRebuild3(false);
                
                // An instance of user selection
                var userSel = new UserSelectionModel();

                // Compile user selection based on form selection
                userSel.Export = expAsmForm.ExportCheck;
                userSel.Print = expAsmForm.PrintCheck;
                userSel.QtyUpdate = expAsmForm.QuantityCheck;
                userSel.ExportAgain = expAsmForm.ExportAgain;

                // Get the assembly quantity back from the winform
                assemblyModel.Quantity = expAsmForm.AssemblyQty;

                // Write the assembly quantity back to the SolidWorks file
                // to update it in case it have been changed by the user
                prpManager.SetCustomProperty(assemblyModel.Model, GlobalConfig.QuantityProperty, assemblyModel.Quantity);

                // Get the job number
                JobNumber = expAsmForm.JobNumber;

                // Get the new log path
                AssExpLog.LogPath = expAsmForm.LogFilePath;

                // Validate job number
                JobNumber = CwValidation.RemoveInvalidChars(JobNumber);

                // Compose set the export path
                ExportFileMacro.ExportFolder = Path.Combine(GlobalConfig.ExportPath, JobNumber);

                // Get the flat BOM
                List<BomModel> bom = new List<BomModel>();
                bom = GetBomToExport(rootComp, bom, userSel.ExportAgain);

                // Add the assembly to the BoM
                bom.Add(new BomModel()
                {
                    Model = assemblyModel.Model,
                    Configuration = swConf.Name,
                    Quantity = Convert.ToDouble(assemblyModel.Quantity),
                    Path = assemblyModel.Model.GetPathName()
                });

                // Export all component in the BOM
                if (bom != null)
                {
                    ExportAllComponent(bom, assemblyModel, userSel);
                }

                // Stop the timer
                stopwatch.Stop();
                TimeSpan st = stopwatch.Elapsed;

                // Compose elapsed time
                string elapsedTIme = ComposeElepsedTime(st);

                SolidWorksEnvironment.Application.ShowMessageBox($"Macro completed in {elapsedTIme}", SolidWorksMessageBoxIcon.Information);
            }
            else if (expAsmFormRes == DialogResult.Cancel)
            {
                SolidWorksEnvironment.Application.ShowMessageBox("Macro terminated", SolidWorksMessageBoxIcon.Stop);
            }

        }

        /// <summary>
        /// Compose a string with the elapsed time in seconds or minutes and seconds
        /// </summary>
        /// <param name="ts">The TimeSpan object</param>
        /// <returns>A string with elapsed message</returns>
        private static string ComposeElepsedTime(TimeSpan ts)
        {
            string elapsed = string.Empty;

            if (ts != null)
            {
                if (ts.TotalSeconds < 60)
                {
                    elapsed = string.Format("{0:0} seconds", ts.TotalSeconds);
                }
                else
                {
                    elapsed = string.Format("{0} minutes and {0:0} seconds", (int)ts.TotalMinutes, ts.Seconds);
                }
            }

            return elapsed;
        }

        /// <summary>
        /// Get the Bill of Material to be processed
        /// </summary>
        /// <param name="rootComp">The parent component of which to extract the BOM</param>
        /// <param name="bom">The Bill of Material object</param>
        /// <param name="exportAgain">True to export again the whole Bill of material</param>
        /// <returns>The Bill of Material to be processed</returns>
        private static List<BomModel> GetBomToExport(Component2 rootComp, List<BomModel> bom, bool exportAgain)
        {
            // Compose the flat Bill of Material
            CwBomManager.ComposeFlatBOM(rootComp, bom);

            if (exportAgain == true || File.Exists(AssExpLog.LogPath) == false)
            {
                return bom;
            }
            else
            {
                // Read log file
                List<string> pathList = Logger.ReadLogFile(AssExpLog.LogPath);               

                // Filter the bom with the list from the log
                bom.RemoveAll(bomList => pathList.Contains(bomList.Path));
            }

            return bom;
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
                asmLog.LogFolderPath = GlobalConfig.LogPath;
                asmLog.LogFileName = $"log_{JobNumber}.txt";

                // Write log first life only if the log file doesn't exist already
                if (!File.Exists(AssExpLog.LogPath) )
                {
                    asmLog.WriteLog("File processati al " + DateTime.Now);
                }

                foreach (var comp in bom)
                {
                    // Update the component quantity is the user selected the option
                    // and if the component is not the assembly
                    if (userSelection.QtyUpdate == true && comp.Path != assembly.Model.GetPathName())
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
                        continue;
                    }

                    // If one between print and export option is selected open the drawing
                    if (userSelection.Export == true || userSelection.Print == true)
                    {
                        var drwModel = SolidWorksEnvironment.Application.OpenFile(drwPath, options: OpenDocumentOptions.Silent);

                        if (drwModel == null)
                        {
                            continue;
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

                        asmLog.WriteLogWithDate(modelPath);
                    }
                }
            }
        }
    }
}
