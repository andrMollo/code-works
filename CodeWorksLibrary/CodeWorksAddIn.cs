﻿using CADBooster.SolidDna;
using CodeWorksLibrary.Properties;
using SolidWorks.Interop.sldworks;
using System.ComponentModel;
using Xarial.XCad.Base.Attributes;
using Xarial.XCad.UI.Commands;
using Xarial.XCad.Extensions.Attributes;
using CodeWorksLibrary.Macros.Files;

namespace CodeWorksLibrary
{
    [System.Runtime.InteropServices.ComVisible(true)]
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
            ExportFileE
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
            }
        }
    }
}
