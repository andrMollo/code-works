using CADBooster.SolidDna;
using CodeWorksLibrary.Macros.Drawings;
using CodeWorksLibrary.Macros.Export;
using CodeWorksLibrary.Macros.Files;
using CodeWorksLibrary.Macros.Properties;
using CodeWorksLibrary.Properties;
using SolidWorks.Interop.sldworks;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Xarial.XCad.Base.Attributes;
using Xarial.XCad.SolidWorks;
using Xarial.XCad.UI.Commands;
using Xarial.XCad.UI.Commands.Attributes;
using Xarial.XCad.UI.Commands.Enums;

namespace CodeWorksLibrary
{
    [ComVisible(true)]
    [Guid("B611522B-5141-41D9-A918-2B50BB885BAA")]
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
            [Title("Save component")]
            [Description("Make a copy of the selected file and its drawing")]
            MakeIndepPdmE,
            [Title("Set author")]
            [Description("Write the component author in the custom properties")]
            [Icon(typeof(Resources), nameof(Resources.SetAuthor))]
            SetAuthorE,
            [Title("Export file")]
            [Description("Export the current file in different formats")]
            [Icon(typeof(Resources), nameof(Resources.ExportFile))]
            ExportFileE,
            [Title("Export print file")]
            [Description("Export and print the current file in different formats")]
            [Icon(typeof(Resources), nameof(Resources.ExportFile))]
            ExportFilePrintE,
            [Title("Export assembly")]
            [Description("Export the current assembly and its components")]
            [Icon(typeof(Resources), nameof(Resources.ExportAssy))]
            ExportAssemblyE,
            [Title("Print drawing")]
            [Description("Print all the sheet of the active drawing")]
            [Icon(typeof(Resources), nameof(Resources.FastPrint))]
            FastPrintE,
            [Title("Print sheet")]
            [Description("Print the current sheet")]
            [Icon(typeof(Resources), nameof(Resources.FastPrintSheet))]
            FastPrintSheetE,
            [Title("Update sheet format")]
            [Description("Update sheet format for all the sheet of the active document")]
            [Icon(typeof(Resources), nameof(Resources.ChangeFormat))]
            UpdateFormatE,
            [Title("Write quantity")]
            [Description("Write the quantity custom property in all components of the open assembly")]
            [Icon(typeof(Resources), nameof(Resources.WriteQuantity))]
            WriteQuantityE,
        }

        #endregion

        #region Public properties
        
        /// <summary>
        /// The SolidWorks application
        /// </summary>
        public static SldWorks SwApp {  get; set; }

        /// <summary>
        /// The application for xCAD
        /// </summary>
        public static ISwApplication App { get; set; }

        #endregion

        /// <summary>
        /// Handle the connection to SolidWorks
        /// </summary>
        public override void OnConnect()
        {
            AddInIntegration.ConnectToActiveSolidWorks(this.Application.Sw.RevisionNumber(), this.AddInId);

            CommandManager.AddCommandGroup<CwCommands_e>().CommandClick += OnCommandClick;

            SwApp = (SldWorks)this.Application.Sw;

            App = this.Application;
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
                case CwCommands_e.MakeIndepPdmE:
                    SaveFile.MakeIndependentWithDrawingMacro();
                    break;
                case CwCommands_e.SetAuthorE:
                    SetAuthorMacro.SetAuthor();
                    break;
                case CwCommands_e.ExportFileE:
                    ExportDocument.ExportDocumentMacro();
                    break;
                case CwCommands_e.ExportFilePrintE:
                    ExportDocument.ExportPrintDocumentMacro();
                    break;
                case CwCommands_e.ExportAssemblyE:
                    ExportAssembly.ExportAssemblyMacro();
                    break;
                case CwCommands_e.UpdateFormatE:
                    UpdateSheetFormat.UpdateSheetsFormatMacro();
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
            }
        }
    }
}
