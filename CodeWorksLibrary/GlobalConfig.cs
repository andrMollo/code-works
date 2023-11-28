﻿namespace CodeWorksLibrary
{
    internal class GlobalConfig
    {
        /// <summary>
        /// The name of the Vault database
        /// </summary>
        public const string VaultName = GlobalConfigPrivate.MyVaultName;

        /// <summary>
        /// The name of the custom property that contains the author of the file
        /// </summary>
        public const string AuthorPropName = "Disegnatore";

        /// <summary>
        /// The root folder for the export path
        /// </summary>
        public const string ExportPath = @"C:\_Export";

        /// <summary>
        /// The license key for the Document Manager API
        /// </summary>
        public const string DmKey = GlobalConfigPrivate.MyDmLicense;

        /// <summary>
        /// The path to the text file containing the map for the replacing of the sheet format
        /// </summary>
        public const string SheetFormatMapPath = GlobalConfigPrivate.MyReplaceMapPath;

        /// <summary>
        /// The name of the configuration with the flat pattern
        /// </summary>
        public const string FlatPatternConfigurationName = "DefaultSviluppo";
    }
}
