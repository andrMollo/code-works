using CADBooster.SolidDna;
using CodeWorksLibrary.Helpers;
using CodeWorksLibrary.Macros.Properties;
using CodeWorksLibrary.Models;
using CodeWorksUI;
using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Logger = CodeWorksLibrary.Helpers.Logger;

namespace CodeWorksLibrary.Macros.Export
{
    internal class ExportAssembly
    {
        #region Public properties

        /// <summary>
        /// The pointer to the SolidDNA Model object
        /// </summary>
        public static Model AssemblyToExport {  get; set; }

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
        internal static void ExportAssemblyMacro()
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

            AssemblyToExport = model;

            ExportAssemblyDocument();          
        }

        /// <summary>
        /// Export the assembly and all its components
        /// </summary>
        private static void ExportAssemblyDocument()
        {
            // Get the SolidWokrs AssemblyModel
            BomElement assemblyModel = new BomElement();
            assemblyModel.Model = AssemblyToExport.UnsafeObject;

            // Get the SolidWoks assembly doc object
            var swAssy = (AssemblyDoc)AssemblyToExport.UnsafeObject;

            // Resolve lightweight components
            var resResolve = swAssy.ResolveAllLightWeightComponents(false);

            // Get the assembly quantity
            var amsPrpManager = new CwPropertyManager();
            assemblyModel.Quantity = amsPrpManager.GetModelQuantity(assemblyModel.Model);

            // Compose the full path to the logger
            AssExpLog = new Logger();
            AssExpLog.LogPath = Logger.ComposeLogPath(string.Empty);

            // Initiate form
            var expAsmForm = AssemblyFormSetup();

            // Show export assembly form
            var expAsmFormRes = expAsmForm.ShowDialog();

            // Check button click
            if (expAsmFormRes == DialogResult.OK)
            {                           
                // Start timer
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                // Process the assembly
                ProcessAssembly(assemblyModel, expAsmForm);
                
                // Stop the timer
                stopwatch.Stop();
                TimeSpan st = stopwatch.Elapsed;

                // Compose elapsed time
                string elapsedTIme = ComposeElapsedTime(st);

                SolidWorksEnvironment.Application.ShowMessageBox($"Macro completed in {elapsedTIme}", SolidWorksMessageBoxIcon.Information);
            }
            else if (expAsmFormRes == DialogResult.Cancel)
            {
                SolidWorksEnvironment.Application.ShowMessageBox("Macro terminated", SolidWorksMessageBoxIcon.Stop);
            }
        }

        /// <summary>
        /// Process the assembly model: setup export parameters and custom properties
        /// </summary>
        /// <param name="assemblyModel">The pointer to the BomModel of the assembly</param>
        /// <param name="expAsmForm">The pointer to the instances of the export form</param>
        private static void ProcessAssembly(BomElement assemblyModel, ExportAssemblyForm expAsmForm)
        {
            // Rebuild assembly an all components
            var asmRebuildRet = assemblyModel.Model.ForceRebuild3(false);

            // Get values back from WinForm
            UserSelectionModel userSel = GetUserSelection(assemblyModel, expAsmForm);            

            // Compose set the export path
            ExportDocument.ExportFolderPath = Path.Combine(GlobalConfig.ExportPath, JobNumber);

            // Get the BoM to be processed
            List<BomElement> bom = GetBomToProcess(assemblyModel, userSel);            

            // Export all component in the BOM
            if (bom != null)
            {
                ExportAllComponent(bom, assemblyModel, userSel);
            }

        }

        /// <summary>
        /// Get the Bill of Material to be exported
        /// </summary>
        /// <param name="assemblyModel">The pointer to the BomModel of the assembly</param>
        /// <param name="userSel">The userselection</param>
        /// <returns></returns>
        private static List<BomElement> GetBomToProcess(BomElement assemblyModel, UserSelectionModel userSel)
        {
            // Get the active configuration
            var swConf = assemblyModel.Model.ConfigurationManager.ActiveConfiguration;

            // Get the root component
            var rootComp = swConf.GetRootComponent3(true);

            // Get the flat BOM
            List<BomElement> bom = new List<BomElement>();
            bom = GetBomToExport(rootComp, bom, userSel.ExportAgain);

            // Add the assembly to the BoM
            bom.Add(new BomElement()
            {
                Model = assemblyModel.Model,
                Configuration = swConf.Name,
                Quantity = Convert.ToDouble(assemblyModel.Quantity),
                Path = assemblyModel.Model.GetPathName()
            });

            return bom;
        }

        /// <summary>
        /// Get the user selection from the UI
        /// </summary>
        /// <param name="assemblyModel">The pointer to the BomModel of the assembly</param>
        /// <param name="expAsmForm">The pointer to the instances of the export form</param>
        /// <returns></returns>
        private static UserSelectionModel GetUserSelection(BomElement assemblyModel, ExportAssemblyForm expAsmForm)
        {
            UserSelectionModel userSel = new UserSelectionModel();

            // Compile user selection based on form selection
            userSel.Export = expAsmForm.ExportCheck;
            userSel.Print = expAsmForm.PrintCheck;
            userSel.QtyUpdate = expAsmForm.QuantityCheck;
            userSel.ExportAgain = expAsmForm.ExportAgain;
            userSel.Quantity = expAsmForm.AssemblyQty;

            // Get the assembly quantity back from the WinForm
            var retParse = double.TryParse(expAsmForm.AssemblyQty, out double formDoubleQty);
            assemblyModel.Quantity = formDoubleQty;

            // Write the assembly quantity back to the SolidWorks file
            // to update it in case it have been changed by the user
            Model model = new Model(assemblyModel.Model);

            model.SetCustomProperty(GlobalConfig.QuantityProperty, expAsmForm.AssemblyQty);

            // Get the job number
            JobNumber = CwValidation.RemoveInvalidPathChars(expAsmForm.JobNumber);

            // Get the new log path
            AssExpLog.LogPath = expAsmForm.LogFilePath;

            return userSel;
        }

        /// <summary>
        /// Setup the WinForm properties
        /// </summary>
        /// <returns>The pointer to the WinForm instances</returns>
        private static ExportAssemblyForm AssemblyFormSetup()
        {
            var expAsmForm = new CodeWorksUI.ExportAssemblyForm();

            // Check if log file already exists
            expAsmForm.LogFilePath = AssExpLog.LogPath;

            // Get assembly quantity custom property string
            string asmQtyString = AssemblyToExport.GetCustomProperty(GlobalConfig.QuantityProperty);

            // Check for empty string
            if (asmQtyString != null && asmQtyString != string.Empty)
            {
                // Show assembly quantity in the form
                expAsmForm.AssemblyQty = asmQtyString;
            }

            return expAsmForm;
        }

        /// <summary>
        /// Compose a string with the elapsed time in seconds or minutes and seconds
        /// </summary>
        /// <param name="ts">The TimeSpan object</param>
        /// <returns>A string with elapsed message</returns>
        private static string ComposeElapsedTime(TimeSpan ts)
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
        private static List<BomElement> GetBomToExport(Component2 rootComp, List<BomElement> bom, bool exportAgain)
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
        private static void ExportAllComponent(List<BomElement> bom, BomElement assembly, UserSelectionModel userSelection)
        {
            if (bom != null)
            {
                // Initiate a log model
                var asmLog = new Logger();

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
                    // Export component
                    ExportComponent(comp, assembly, userSelection, asmLog);                    
                }
            }
        }

        /// <summary>
        /// Export the assembly component
        /// </summary>
        /// <param name="comp">The pointer to th BoMElement to be exported</param>
        /// <param name="assembly">The pointer to the BomElement corresponding to the assembly</param>
        /// <param name="userSelection">The pointer to user selection object</param>
        /// <param name="asmLog">The pointer to the logger</param>
        private static void ExportComponent(BomElement comp, BomElement assembly, UserSelectionModel userSelection, Logger asmLog)
        {
            // Update the component quantity is the user selected the option
            // and if the component is not the assembly
            if (userSelection.QtyUpdate == true && comp.Path != assembly.Model.GetPathName())
            {
                // Write quantity
                WriteQuantityMacro.WriteQuantity(comp.Model, comp.Quantity, assembly.Quantity);
            }

            // If one between print and export option is selected open the drawing
            if (userSelection.Export == true || userSelection.Print == true)
            {
                // Get drawing model
                ExportDocument.ExportModel = new Model(comp.Model);
                ExportDocument.ExportModel = ExportDocument.GetDrawingModel();

                Model drwModel = ExportDocument.ExportModel;

                // Set the export job number
                ExportDocument.JobNumber = JobNumber;

                // Set the file name
                ExportDocument.ModelNameNoExt = Path.GetFileNameWithoutExtension(drwModel.FilePath);

                // Set the job number in the drawing model
                var drwPrpManger = new CwPropertyManager();
                drwPrpManger.SetCustomProperty((ModelDoc2)drwModel.UnsafeObject, GlobalConfig.JobNumberPropName, JobNumber);

                // Print the drawing if the user selected the option
                if (userSelection.Print == true)
                {
                    // Set print property
                    ExportDocument.PrintSelection = true;
                }

                // Export the component drawing and preview if the user selected the option
                if (userSelection.Export == true)
                {
                    ExportDocument.ExportSelection = true;
                    // Export drawing and model preview
                    ExportDocument.ExportDrawingAndPreview();
                }

                // Delete the job number from drawing custom properties
                drwPrpManger.SetCustomProperty((ModelDoc2)drwModel.UnsafeObject, GlobalConfig.JobNumberPropName, string.Empty);

                // Close the drawing
                drwModel.Close();

                // Write log entry
                asmLog.WriteLogWithDate(modelPath);
            }
        }
    }
}
