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
using CwLogger = CodeWorksLibrary.Helpers.CwLogger;

namespace CodeWorksLibrary.Macros.Export
{
    internal class ExportAssembly
    {
        #region Public properties

        /// <summary>
        /// The pointer to the SolidDNA Model object
        /// </summary>
        public static BomElement AssemblyBomElement {  get; set; }

        /// <summary>
        /// The job number
        /// </summary>
        public static string JobNumber { get; set; }

        /// <summary>
        /// The log object for the assembly export
        /// </summary>
        public static CwLogger AssExpLog { get; set; }
        #endregion

        /// <summary>
        /// Export the assembly and all its components
        /// </summary>
        public static void ExportAssemblyMacro()
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

            AssemblyBomElement = new BomElement();
            AssemblyBomElement.Model = (ModelDoc2)model.UnsafeObject;

            ExportAssemblyDocument();          
        }

        /// <summary>
        /// Export the assembly and all its components
        /// </summary>
        private static void ExportAssemblyDocument()
        {
            // Get the SolidWoks assembly doc object
            var swAssy = (AssemblyDoc)AssemblyBomElement.Model;

            // Resolve lightweight components
            var resResolve = swAssy.ResolveAllLightWeightComponents(false);

            // Get the assembly quantity
            var amsPrpManager = new CwPropertyManager();
            AssemblyBomElement.Quantity = amsPrpManager.GetModelQuantity(AssemblyBomElement.Model);

            // Compose the full path to the logger
            AssExpLog = new CwLogger();
            AssExpLog.LogPath = CwLogger.ComposeLogPath(string.Empty);

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
                ProcessAssembly(expAsmForm);
                
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
        /// <param name="expAsmForm">The pointer to the instances of the export form</param>
        private static void ProcessAssembly(ExportAssemblyForm expAsmForm)
        {
            // Rebuild assembly an all components
            var asmRebuildRet = AssemblyBomElement.Model.ForceRebuild3(false);

            // Get values back from WinForm
            UserSelectionModel userSel = GetUserSelection(expAsmForm);            

            // Compose set the export path
            ExportDocument.ExportFolderPath = Path.Combine(GlobalConfig.ExportPath, JobNumber);

            // Get the BoM to be processed
            List<BomElement> bom = GetBomToProcess(userSel);            

            // Export all component in the BOM
            if (bom != null)
            {
                ExportAllComponent(bom, userSel);
            }

        }

        /// <summary>
        /// Get the Bill of Material to be exported
        /// </summary>
        /// <param name="userSel">The user selection model</param>
        /// <returns></returns>
        private static List<BomElement> GetBomToProcess(UserSelectionModel userSel)
        {
            // Get the active configuration
            var swConf = AssemblyBomElement.Model.ConfigurationManager.ActiveConfiguration;

            // Get the root component
            var rootComp = swConf.GetRootComponent3(true);

            // Create the Bill of Material
            List<BomElement> bom = new List<BomElement>();

            // Add the assembly to the BoM
            bom.Add(new BomElement()
            {
                Model = AssemblyBomElement.Model,
                Configuration = swConf.Name,
                Quantity = Convert.ToDouble(AssemblyBomElement.Quantity),
                Path = AssemblyBomElement.Model.GetPathName()
            });

            // Get the Bill of Material to be exported comparing with the log file
            bom = GetBomToExport(rootComp, bom, userSel.ExportAgain);

            return bom;
        }

        /// <summary>
        /// Get the user selection from the UI
        /// </summary>
        /// <param name="expAsmForm">The pointer to the instances of the export form</param>
        /// <returns></returns>
        private static UserSelectionModel GetUserSelection(ExportAssemblyForm expAsmForm)
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
            AssemblyBomElement.Quantity = formDoubleQty;

            // Write the assembly quantity back to the SolidWorks file
            // to update it in case it have been changed by the user
            Model model = new Model(AssemblyBomElement.Model);

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
            Model asmModel = new Model(AssemblyBomElement.Model);
            string asmQtyString = asmModel.GetCustomProperty(GlobalConfig.QuantityProperty);

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
                List<string> pathList = CwLogger.ReadLogFile(AssExpLog.LogPath);               

                // Filter the bom with the list from the log
                bom.RemoveAll(bomList => pathList.Contains(bomList.Path));
            }

            return bom;
        }

        /// <summary>
        /// Export all components in the BOM
        /// </summary>
        /// <param name="bom">The instance of the Bill of Material</param>
        /// <param name="userSelection">The model with the option the user selected</param>
        private static void ExportAllComponent(List<BomElement> bom, UserSelectionModel userSelection)
        {
            if (bom != null)
            {
                // Initiate a log model
                var asmLog = new CwLogger();

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
                    ExportComponent(comp, userSelection, asmLog);                    
                }
            }
        }

        /// <summary>
        /// Export the assembly component
        /// </summary>
        /// <param name="comp">The pointer to th BoMElement to be exported</param>
        /// <param name="userSelection">The pointer to user selection object</param>
        /// <param name="asmLog">The pointer to the logger</param>
        private static void ExportComponent(BomElement comp, UserSelectionModel userSelection, CwLogger asmLog)
        {
            // Update the component quantity is the user selected the option
            // and if the component is not the assembly
            if (userSelection.QtyUpdate == true && comp.Path != AssemblyBomElement.Model.GetPathName())
            {
                // Write quantity
                WriteQuantityMacro.WriteQuantity(comp.Model, comp.Quantity, AssemblyBomElement.Quantity);
            }

            // If one between print and export option is selected open the drawing
            if (userSelection.Export == true || userSelection.Print == true)
            {
                // Get drawing model
                ExportDocument.ExportModel = new Model(comp.Model);
                ExportDocument.ExportModel = ExportDocument.GetDrawingModel();

                if (ExportDocument.ExportModel != null)
                {
                    Model drwModel = ExportDocument.ExportModel;

                    // Set the export job number
                    ExportDocument.JobNumber = JobNumber;

                    // Set the file name
                    ExportDocument.ModelNameNoExt = Path.GetFileNameWithoutExtension(drwModel.FilePath);

                    // Set the job number in the drawing model
                    drwModel.SetCustomProperty(GlobalConfig.JobNumberPropName, JobNumber);

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
                    drwModel.SetCustomProperty(GlobalConfig.JobNumberPropName, string.Empty);

                    // Close the drawing
                    drwModel.Close();

                }
            }
            // Write log entry
            asmLog.WriteLogWithDate(comp.Model.GetPathName()); 
        }
    }
}
