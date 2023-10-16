using CADBooster.SolidDna;
using System;
using System.ComponentModel;
using Xarial.XCad.Base.Attributes;
using Xarial.XCad.UI.Commands;

namespace CodeWorksLibrary
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public class AddIn : Xarial.XCad.SolidWorks.SwAddInEx
    {
        /// <summary>
        /// Enumeration that contains the commands to be added to SolidWorks
        /// </summary>
        [Title("CodeWorks")]
        [Description("A collection of macros for SolidWorks")]
        public enum CwCommands_e
        {
            [Title("Set author")]
            [Description("Write the component author in the custom properties")]
            SetAuthorE
        }

        /// <summary>
        /// Handle the connection to SolidWorks
        /// </summary>
        public override void OnConnect()
        {
            AddInIntegration.ConnectToActiveSolidWorks(this.Application.Sw.RevisionNumber(), this.AddInId);

            CommandManager.AddCommandGroup<CwCommands_e>().CommandClick += OnCommandClick;
        }

        private void OnCommandClick(CwCommands_e spec)
        {
            switch (spec)
            {
                case CwCommands_e.SetAuthorE:
                    break;
            }
        }
    }
}
