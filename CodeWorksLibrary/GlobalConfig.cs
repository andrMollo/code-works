// Ignore Spelling: Dm

namespace CodeWorksLibrary
{
    public class GlobalConfig
    {
        #region Path
        /// <summary>
        /// The root folder for the export path
        /// </summary>
        public const string ExportPath = @"C:\_Export";

        /// <summary>
        /// The folder for the log file
        /// </summary>
        public const string LogPath = @"C:\_Export\.log\";
        #endregion

        #region Custom properties

        /// <summary>
        /// The name of the custom property that contains the author of the file
        /// </summary>
        public const string AuthorPropName = "Disegnatore";

        /// <summary>
        /// The name of the configuration with the flat pattern
        /// </summary>
        public const string FlatPatternConfigurationName = "DefaultSviluppo";

        /// <summary>
        /// The name of the custom property that contains the component quantity
        /// </summary>
        public const string QuantityProperty = "Quantità";

        /// <summary>
        /// The name of the property for the job number
        /// </summary>
        public const string JobNumberPropName = "Commessa";

        /// <summary>
        /// The name of the project for standard components
        /// </summary>
        public const string StandardProjectNumber = "A000";

        #endregion

        #region Private path constants

        /// <summary>
        /// The name of the Vault database
        /// </summary>
        public const string VaultName = GlobalConfigPrivate.MyVaultName;

        /// <summary>
        /// The name of the root folder for the PDM
        /// </summary>
        public const string VaultRootFolder = GlobalConfigPrivate.MyVaultRootFolder;

        /// <summary>
        /// The path to the root folder for build components
        /// </summary>
        public const string ComponentRootFolder = GlobalConfigPrivate.MyComponentRootFolder;

        /// <summary>
        /// The path to the root folder for trade \ buy components
        /// </summary>
        public const string LibraryRootFolder = GlobalConfigPrivate.MyLibraryRootFolder;

        /// <summary>
        /// The path to the root folder for drat components
        /// </summary>
        public const string DraftRootFolder = GlobalConfigPrivate.MyDraftRootFolder;

        /// <summary>
        /// The path to the PDM folder with the standard components
        /// </summary>
        public const string StandardComponentsFolder = GlobalConfigPrivate.MyStandardComponentsFolder;

        /// <summary>
        /// The path to my map for the new sheet format
        /// </summary>
        public const string MyReplaceMapPath = GlobalConfigPrivate.MyReplaceMapPath;

        /// <summary>
        /// The license key for the Document Manager API
        /// </summary>
        public const string DmKey = GlobalConfigPrivate.MyDmLicense;

        /// <summary>
        /// The path to the text file containing the map for the replacing of the sheet format
        /// </summary>
        public const string SheetFormatMapPath = GlobalConfigPrivate.MyReplaceMapPath;

        #endregion

        #region Private PDM Variables and serials names

        /// <summary>
        /// The name of the variabile with the project starting letter
        /// </summary>
        public const string ProjectLetterVariable = GlobalConfigPrivate.MyProjectLetterVariable;

        /// <summary>
        /// The name of the variable with the project number
        /// </summary>
        public const string ProjectNumberVariable = GlobalConfigPrivate.MyProjectNumberVariable;

        /// <summary>
        /// The name of the serial for part files
        /// </summary>
        public const string PartSerialNumberName = GlobalConfigPrivate.MyPartSerialNumberName;

        /// <summary>
        /// The name of the serial for assembly files
        /// </summary>
        public const string AssemblySerialNumberName = GlobalConfigPrivate.MyAssemblySerialNumberName;

        /// <summary>
        /// The name of the serial for draft part files
        /// </summary>
        public const string DraftPartSerialNumberName = GlobalConfigPrivate.MyDraftPartSerialNumberName;

        /// <summary>
        /// The name of the serial for draft assembly files
        /// </summary>
        public const string DraftAssemblySerialNumberName = GlobalConfigPrivate.MyDraftAssemblySerialNumberName;

        /// <summary>
        /// The name of the serial number for library files
        /// </summary>
        public const string LibrarySerialNumberName = GlobalConfigPrivate.MyLibrarySerialNumberName;

        /// <summary>
        /// The name of the serial for non coded files
        /// </summary>
        public const string NonCodedSerialNumberName = GlobalConfigPrivate.MyNonCodedSerialNumberName;

        #endregion

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

        /// <summary>
        /// The name of the layer that contains the job number
        /// </summary>
        public const string PrintJobLayer = "COMMESSA STAMPA";

        #endregion

        /// <summary>
        /// The name of the logger for the Add-In
        /// </summary>
        public const string LoggerName = "CodeWorksLog";
    }
}
