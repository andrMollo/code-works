namespace CodeWorksLibrary
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

        #region Print setup
        /// <summary>
        /// The path to the printer setup
        /// </summary>
        public const string PrinterSetupFile = GlobalConfigPrivate.MyPrinterSetupFile;

        /// <summary>
        /// The name of the property that contains the name of the user that print a drawing
        /// </summary>
        public const string PrintedBy = "Stampato da";

        /// <summary>
        /// The name of the property that contains the date when the drawing is printed
        /// </summary>
        public const string PrintedOn = "Stampato il";

        #endregion
    }
}
