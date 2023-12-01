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
        /// The name of the default printer as shown in device manager
        /// </summary>
        public const string DefaultPrinterName = "TECNICOMONO";

        /// <summary>
        /// The name of the A4 size for the default printer
        /// </summary>
        public const string A4FormatName = "A4 210 x 297 mm";

        /// <summary>
        /// The name of the A4 size for the default printer
        /// </summary>
        public const string A3FormatName = "A3 297 x 420 mm";

        /// <summary>
        /// The name of the property that contains the name of the user that print a drawing
        /// </summary>
        public const string PrintedBy = "Stampato da";

        /// <summary>
        /// The name of the property that contains the date when the drawing is printed
        /// </summary>
        public const string PrintedOn = "Stampato il";

        /// <summary>
        /// The name of the layer that contains the notes to be shown in the prints
        /// </summary>
        public const string PrintNoteLayer = "NOTE STAMPA";

        #endregion
    }
}
