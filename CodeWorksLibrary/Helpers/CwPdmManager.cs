// Ignore Spelling: Pdm

using EPDM.Interop.epdm;
using System;
using static CADBooster.SolidDna.SolidWorksEnvironment;

namespace CodeWorksLibrary
{
    internal class CwPdmManager
    {
        /// <summary>
        /// Get the username from SolidWorks PDM
        /// </summary>
        /// <returns>The string with the UserData of the user connected to the Vault, or the Window username if PDM can't be reached</returns>
        public static string GetPdmUserName()
        {
            string userName = string.Empty;

            // Try to auto-login to PDM
            try
            {
                IEdmVault5 pdmVault = new EdmVault5();

                pdmVault.LoginAuto(GlobalConfig.VaultName, 0);

                IEdmUserMgr5 userMgr = (IEdmUserMgr5) pdmVault;

                IEdmUser6 user = (IEdmUser6)userMgr.GetLoggedInUser();

                userName = (string)user.UserData;

                return userName;
            }
            catch
            {
                Application.ShowMessageBox("Unable to connect to the Vault, the user is set to the Windows login username", CADBooster.SolidDna.SolidWorksMessageBoxIcon.Warning);
                userName = Environment.UserName;

                return userName;
            }
        }
    }
}
