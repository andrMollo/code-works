using CADBooster.SolidDna;
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
        /// <returns>The PDM serial number</returns>
        /// <remarks>This method get the next available serial number from the PDM
        /// then the serial number is rolled-back</remarks>
        public static string GetPdmSerialNumber(string folderPath, ModelType type)
        {
            string output = string.Empty;

            // Check if path is inside PDM
            if (folderPath.StartsWith(GlobalConfig.VaultRootFolder.ToLower()) == false)
            {
                throw new ArgumentException("Select path inside PDM.");
            }

            if (folderPath.StartsWith(GlobalConfig.LibraryRootFolder.ToLower()))
            {
                output = GlobalConfig.LibrarySerialNumberName;
            }
            else if (folderPath.StartsWith (GlobalConfig.ComponentRootFolder.ToLower()) 
                && type == ModelType.Part)
            {
                output = GlobalConfig.PartSerialNumberName;
            }
            else if (folderPath.StartsWith(GlobalConfig.ComponentRootFolder.ToLower())
                && type == ModelType.Assembly)
            {
                output = GlobalConfig.AssemblySerialNumberName;
            }
            else if (folderPath.StartsWith(GlobalConfig.DraftRootFolder.ToLower()) 
                && type == ModelType.Part)
            {
                output = GlobalConfig.DraftPartSerialNumberName;
            }
            else if (folderPath.StartsWith(GlobalConfig.DraftRootFolder.ToLower())
                && type == ModelType.Assembly)
            {
                output = GlobalConfig.DraftAssemblySerialNumberName;
            }
            
            return output;
        }
    }
}
