// Ignore Spelling: App

using CADBooster.SolidDna;
using CodeWorksLibrary.Macros.Drawings;
using CodeWorksLibrary.Macros.Export;
using CodeWorksLibrary.Macros.Files;
using CodeWorksLibrary.Macros.Properties;
using CodeWorksLibrary.Properties;
using SolidWorks.Interop.sldworks;
using System.ComponentModel;
using Xarial.XCad.Base.Attributes;
using Xarial.XCad.UI.Commands;

namespace CodeWorksLibrary
{
    [System.Runtime.InteropServices.ComVisible(true)]
    [Title("CodeWorks")]
    [Description("A collection of macros for SolidWorks")]
    public class AddIn : Xarial.XCad.SolidWorks.SwAddInEx
    {
        #region Enumeration
        /// <summary>
        /// Enumeration that contains the commands to be added to SolidWorks
        /// </summary>
        [Title("CodeWorks")]
        [Description("A collection of macros for SolidWorks")]
        public enum CwCommands_e
        {
            [Title("Set author")]
            [Description("Write the component author in the custom properties")]
            [Icon(typeof(Resources), nameof(Resources.SetAuthor))]
            SetAuthorE,
            [Title("Export file")]
            [Description("Export the current file in different formats")]
            [Icon(typeof(Resources), nameof(Resources.ExportFile))]
            ExportFileE,
            [Title("Export assembly")]
            [Description("Export the current assembly and its components")]
            ExportAssemblyE,
            [Title("Print drawing")]
            [Description("Print all the sheet of the active drawing")]
            [Icon(typeof(Resources), nameof(Resources.FastPrint))]
            FastPrintE,
            [Title("Print sheet")]
            [Description("Print the current sheet")]
            [Icon(typeof(Resources), nameof(Resources.FastPrintSheet))]
            FastPrintSheetE,
            [Title("Print to PDF")]
            [Description("Print the current drawing to PDF")]
            PrintToPdfE,
            [Title("Update sheet format")]
            [Description("Update sheet format for all the sheet of the active document")]
            [Icon(typeof(Resources), nameof(Resources.ChangeFormat))]
            UpdateFormatE,
            [Title("Write quantity")]
            [Description("Write the quantity custom property in all components of the open assembly")]
            WriteQuantityE
        }

        #endregion

        #region Public properties
        public static SldWorks swApp {  get; set; }

        #endregion

        /// <summary>
        /// Handle the connection to SolidWorks
        /// </summary>
        public override void OnConnect()
        {
            AddInIntegration.ConnectToActiveSolidWorks(this.Application.Sw.RevisionNumber(), this.AddInId);

            CommandManager.AddCommandGroup<CwCommands_e>().CommandClick += OnCommandClick;

            swApp = (SldWorks)this.Application.Sw;
        }

        /// <summary>
        /// Handle the disconnection from SolidWorks
        /// </summary>
        public override void OnDisconnect()
        {
            AddInIntegration.TearDown();
        }

        private void OnCommandClick(CwCommands_e spec)
        {
            switch (spec)
            {
                case CwCommands_e.SetAuthorE:
                    SetAuthorMacro.SetAuthor();
                    break;
                case CwCommands_e.ExportFileE:
                    ExportFileMacro.ExportFile();
                    break;
                case CwCommands_e.ExportAssemblyE:
                    ExportAssyMacro.ExportAssembly();
                    break;
                case CwCommands_e.UpdateFormatE:
                    UpdateFormatMacro.UpdateFormatAllSheets();
                    break;
                case CwCommands_e.FastPrintE:
                    FastPrintMacro.FastPrint();
                    break;
                case CwCommands_e.FastPrintSheetE:
                    FastPrintMacro.FastPrintSheet();
                    break;
                case CwCommands_e.WriteQuantityE:
                    WriteQuantityMacro.WriteComponentsQuantity();
                    break;
                case CwCommands_e.PrintToPdfE:
                    FastPrintMacro.PrintToPdf();
                    break;
            }
        }
    }
}
