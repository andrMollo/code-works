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
    public class AddIn : SwAddInEx
    {
        #region Enumerations

        /// <summary>
        /// Enumeration that contains the commands to be added to SolidWorks
        /// </summary>
        [Title("CodeWorks")]
        [Description("A collection of macros for SolidWorks")]
        public enum CwCommands_e
        {
            [Title("Copy PDM")]
            [Description("Make a copy of the selected component and its drawing using PDM part number")]
            [Icon(typeof(Resources), nameof(Resources.SaveFile))]
            [CommandItemInfo(true, true, WorkspaceTypes_e.Part | WorkspaceTypes_e.Assembly, true, RibbonTabTextDisplay_e.TextBelow)]
            MakeIndepPdmE,
            [Title("Save PDM")]
            [Description("Save the selected component and its drawing using PDM part number")]
            [Icon(typeof(Resources), nameof(Resources.SaveFile))]
            [CommandItemInfo(true, false, WorkspaceTypes_e.Part | WorkspaceTypes_e.Assembly, true, RibbonTabTextDisplay_e.TextBelow)]
            SavePdmE,
            [Title("Copy")]
            [Description("Make a copy of the selected file and its drawing")]
            [Icon(typeof(Resources), nameof(Resources.SaveFile))]
            [CommandItemInfo(true, false, WorkspaceTypes_e.Part | WorkspaceTypes_e.Assembly, true, RibbonTabTextDisplay_e.TextBelow)]
            MakeIndepE,
            [Title("Save")]
            [Description("Save the selected component and its drawing")]
            [Icon(typeof(Resources), nameof(Resources.SaveFile))]
            [CommandItemInfo(true, false, WorkspaceTypes_e.Part | WorkspaceTypes_e.Assembly, true, RibbonTabTextDisplay_e.TextBelow)]
            SaveE,
            [Title("Set author")]
            [Description("Write the component author in the custom properties")]
            [Icon(typeof(Resources), nameof(Resources.SetAuthor))]
            [CommandItemInfo(true, false, WorkspaceTypes_e.Part | WorkspaceTypes_e.Assembly, true, RibbonTabTextDisplay_e.TextBelow)]
            SetAuthorE,
            [Title("Export file")]
            [Description("Export the current file in different formats")]
            [Icon(typeof(Resources), nameof(Resources.ExportFile))]
            [CommandItemInfo(true, false, WorkspaceTypes_e.All, true, RibbonTabTextDisplay_e.TextBelow)]
            ExportFileE,
            [Title("Export print file")]
            [Description("Export and print the current file in different formats")]
            [Icon(typeof(Resources), nameof(Resources.ExportFile))]
            [CommandItemInfo(true, false, WorkspaceTypes_e.All, true, RibbonTabTextDisplay_e.TextBelow)]
            ExportFilePrintE,
            [Title("Export assembly")]
            [Description("Export the current assembly and its components")]
            [Icon(typeof(Resources), nameof(Resources.ExportAssy))]
            [CommandItemInfo(true, false, WorkspaceTypes_e.Assembly, true, RibbonTabTextDisplay_e.TextBelow)]
            ExportAssemblyE,
            [Title("Print drawing")]
            [Description("Print all the sheet of the active drawing")]
            [Icon(typeof(Resources), nameof(Resources.FastPrint))]
            [CommandItemInfo(true, false, WorkspaceTypes_e.Drawing, true, RibbonTabTextDisplay_e.TextBelow)]
            FastPrintE,
            [Title("Print sheet")]
            [Description("Print the current sheet")]
            [Icon(typeof(Resources), nameof(Resources.FastPrintSheet))]
            [CommandItemInfo(true, false, WorkspaceTypes_e.Drawing, true, RibbonTabTextDisplay_e.TextBelow)]
            FastPrintSheetE,
            [Title("Update sheet format")]
            [Description("Update sheet format for all the sheet of the active document")]
            [Icon(typeof(Resources), nameof(Resources.ChangeFormat))]
            [CommandItemInfo(true, false, WorkspaceTypes_e.Drawing, true, RibbonTabTextDisplay_e.TextBelow)]
            UpdateFormatE,
            [Title("Write quantity")]
            [Description("Write the quantity custom property in all components of the open assembly")]
            [Icon(typeof(Resources), nameof(Resources.WriteQuantity))]
            [CommandItemInfo(true, false, WorkspaceTypes_e.Assembly, true, RibbonTabTextDisplay_e.TextBelow)]
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

        /// <summary>
        /// Event handler to execute commands on click in Command Tab
        /// </summary>
        private void OnCommandClick(CwCommands_e spec)
        {
            switch (spec)
            {
                case CwCommands_e.MakeIndepPdmE:
                    SaveFile.SaveWithDrawing(true, false);
                    break;
                case CwCommands_e.SavePdmE:
                    SaveFile.SaveWithDrawing(true, true);
                    break;
                case CwCommands_e.MakeIndepE:
                    SaveFile.SaveWithDrawing(false, false);
                    break;
                case CwCommands_e.SaveE:
                    SaveFile.SaveWithDrawing(false, true);
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
