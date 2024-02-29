using CADBooster.SolidDna;
using CodeWorksLibrary.Macros.Drawings;
using CodeWorksLibrary.Macros.Export;
using CodeWorksLibrary.Macros.Files;
using CodeWorksLibrary.Macros.Properties;
using SolidWorks.Interop.sldworks;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Xarial.XCad.Base.Attributes;
using Xarial.XCad.SolidWorks;
using Xarial.XCad.UI.Commands;
using static CodeWorksLibrary.Enums.AddInEnum;

namespace CodeWorksLibrary
{
    [ComVisible(true)]
    [Guid("B611522B-5141-41D9-A918-2B50BB885BAA")]
    [Title("CodeWorks")]
    [Description("A collection of macros for SolidWorks")]
    public class AddIn : SwAddInEx
    {
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
                case CwCommands_e.CloseAllNoActiveE:
                    CloseNoActive.CloseNoActiveMacro();
                    break;
                case CwCommands_e.OpenFIleFolderE:
                    FileFolder.OpenFolderMacro();
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
