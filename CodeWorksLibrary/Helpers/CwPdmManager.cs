﻿using CADBooster.SolidDna;
using EPDM.Interop.epdm;
using System;

namespace CodeWorksLibrary.Helpers
{
    public class CwPdmManager
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
                CwMessage.NoPDMConnection();

                // Set the username ad the windows login
                userName = Environment.UserName;

                return userName;
            }
        }

        /// <summary>
        /// Get the next PDM serial number
        /// </summary>
        /// <param name="folderPath">The path of the file for which the serial number is needed</param>
        /// <param name="type">The type of SolidWorks file for which the serial number is needed</param>
        /// <returns></returns>
        public static string GetPdmSerialNumber(string folderPath, ModelType type)
        {
            string output = string.Empty;

            return output;
        }
    }
}
