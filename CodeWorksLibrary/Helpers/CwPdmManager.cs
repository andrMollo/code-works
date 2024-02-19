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

                if (pdmVault.IsLoggedIn)
                {
                    IEdmUserMgr5 userMgr = (IEdmUserMgr5) pdmVault;

                    IEdmUser6 user = (IEdmUser6)userMgr.GetLoggedInUser();

                    userName = (string)user.UserData;

                    return userName;
                }
                else
                {
                    throw new Exception("Unable to connect to PDM.");
                }
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

            string serialName = string.Empty;

            if (folderPath.StartsWith(GlobalConfig.LibraryRootFolder.ToLower()))
            {
                serialName = GlobalConfig.LibrarySerialNumberName;
            }
            else if (folderPath.StartsWith (GlobalConfig.ComponentRootFolder.ToLower()) 
                && type == ModelType.Part)
            {
                serialName = GlobalConfig.PartSerialNumberName;
            }
            else if (folderPath.StartsWith(GlobalConfig.ComponentRootFolder.ToLower())
                && type == ModelType.Assembly)
            {
                serialName = GlobalConfig.AssemblySerialNumberName;
            }
            else if (folderPath.StartsWith(GlobalConfig.DraftRootFolder.ToLower()) 
                && type == ModelType.Part)
            {
                serialName = GlobalConfig.DraftPartSerialNumberName;
            }
            else if (folderPath.StartsWith(GlobalConfig.DraftRootFolder.ToLower())
                && type == ModelType.Assembly)
            {
                serialName = GlobalConfig.DraftAssemblySerialNumberName;
            }
            else
            {
                throw new Exception("Unable get a name for a serial number schema.");
            }

            // Get the serial number from the Vault
            try
            {
                EdmVault5 pdmVault = new EdmVault5();

                pdmVault.LoginAuto(GlobalConfig.VaultName, 0);

                IEdmSerNoGen7 serialNumber = (IEdmSerNoGen7)pdmVault;

                IEdmSerNoValue serialNumberValue = serialNumber.AllocSerNoValue(serialName);

                output = serialNumberValue.Value;

                serialNumberValue.Rollback();
            }
            catch
            {
                throw new Exception("Unable to get a serial number value.");
            }

            return output;
        }

        /// <summary>
        /// Get the full project number by combining project letter and project number
        /// </summary>
        /// <param name="folderPath">The path to a PDM folder</param>
        /// <returns>A string with the combination of project letter and project number</returns>
        /// <remarks>It return the A000 project number if the path starts with StandardComponentsFolder</remarks>
        public static string GetProjectNumber(string folderPath)
        {
            string output = string.Empty;

            try
            {
                EdmVault5 pdmVault = new EdmVault5();

                pdmVault.LoginAuto(GlobalConfig.VaultName, 0);

                if (pdmVault.IsLoggedIn)
                {
                    // Return the project for standard components
                    if (folderPath.StartsWith(GlobalConfig.StandardComponentsFolder.ToLower()))
                    {
                        output = GlobalConfig.StandardProjectNumber;

                        return output;
                    }

                    IEdmVariableMgr7 pdmVariableMgr = (IEdmVariableMgr7)pdmVault;

                    IEdmVariable5 pdmLetterVariable = pdmVariableMgr.GetVariable(GlobalConfig.ProjectLetterVariable);

                    IEdmVariable5 pdmNumberVariable = pdmVariableMgr.GetVariable(GlobalConfig.ProjectNumberVariable);

                    IEdmFolder5 pdmFolder = pdmVault.GetFolderFromPath(folderPath);

                    int folderId = pdmFolder.ID;

                    IEdmEnumeratorVariable5 variableEnum = (IEdmEnumeratorVariable5)pdmFolder;

                    var projectLetter = variableEnum.GetVar(GlobalConfig.ProjectLetterVariable, "", out var poRetValueLet);

                    var projectNumber = variableEnum.GetVar(GlobalConfig.ProjectNumberVariable, "", out var poRetValueNbr);

                    output = projectLetter.ToString() + " " + projectNumber.ToString();
                }
                else
                {
                    throw new Exception("Unable to connect to PDM.");
                }
            }
            catch
            {
                throw;
            }

            return output;
        }
    }
}
