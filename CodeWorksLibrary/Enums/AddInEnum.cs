using CodeWorksLibrary.Properties;
using System.ComponentModel;
using Xarial.XCad.Base.Attributes;
using Xarial.XCad.UI.Commands.Attributes;
using Xarial.XCad.UI.Commands.Enums;

namespace CodeWorksLibrary.Enums
{
    internal class AddInEnum
    {
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
            [Description("Make a copy of the selected component and its drawing")]
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
    }
}
