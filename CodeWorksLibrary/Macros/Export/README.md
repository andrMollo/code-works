# Export macros

A collection of methods to export SolidWorks files.

## Export assembly

![Export assembly](../../Resources/ExportAssy.png)

Export the open assembly and all ist components to different format.

A form ask the user if components need to be exported, printed or both. The user can also decide to print all the drawing and to update the quantity custom properties.

The user can also specify a `Job Number` which will be used to create a sub-folder in the root output folder is defined in `GlobalConfig.ExportPath`.

For more information see the notes on:

* [Export file](#export-file)
* [Write componente quantities](../Properties/README.md/#write-quantities)
* [Print file](#print-drawing)
* [Upgrade sheet format](..//Drawings/README.md/#upgrade-format)
* [Set the file author](../Properties/README.md/#set-author)

### Prerequisites

* A SolidWorks file is opened and saved.

## Export file

![Export file icon](../../Resources/ExportFile.png "Export file")

Export the open file to different formats. The root output folder is defined in `GlobalConfig.ExportPath`, a set of sub-folder is created based on the output format (PDF, DWG...). The output file name is equal to the SolidWorks one.

If the open file is a drawing it's saved as DWG and PDF, then the referenced model is exported to STEP and it's preview to PNG.

It the open file is a model it's saved to STEP and it's preview to PNG. the method then tries to one a drawing document with the same name as the model in the same folder; if a drawing is found then it's exported as PDF and DWG.

### Prerequisites

* A SolidWorks file is opened and saved.
* This method uses the SolidWorks Document Manger API. You should create a file in `CodeWorksLibrary\Private\GlobalConfigPrivate` where you define

```c#
namespace CodeWorksLibrary
{
    internal class GlobalConfigPrivate
    {
        /// <summary>
        /// The name of the Vault database
        /// </summary>
        public const string MyVaultName = "YOUR_VAULT_NAME";

        /// <summary>
        /// The license key for the Document Manager API
        /// </summary>
        public const string MyDmLicense = "YOUR_DOCUMENT_MANAGER_LICENSE_KEY";
    }
}
```

## Print drawing

![Print file icon](../../Resources/FastPrint.png)

Print all the sheet the active drawing. Sheets are printed one at a time simulating simplex setup no matter how the printer is set. The sheet dimension will determine the paper size: A4 sheet are printed on A4 paper, sheets with size of A3 and above are printed on A3 paper.

TThe macro do not print sheets that contain only one view in the configuration `DefaultSviluppo` (flat pattern).

The printer name and the names for the A4 and A3 paper format are defined in [GlobalConfig](../../GlobalConfig.cs):

```cs
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
```

Before printing, the macro makes visible the layer specified in `GlobalConfig.PrintNoteLayer`, then the layer is hidden again. The macro also set the custom property defined in `GlobalConfig.PrintedBy` with the vault username who printed the file and the custom property set in `GlobalConfig.PrintedOn` with the date the file is printed. See [SetAuthorMacro](../Properties/SetAuthorMacro.cs) for more information on how the macro get the PDM username.

The macro also upgrades the sheet format. For more information on the format replace see [UpdateFormatMacro](../Drawings/UpdateFormatMacro.cs).

### Prerequisites

* a drawing is open and active

### References

* [Original macro from CodeStack](https://www.codestack.net/solidworks-api/document/print/)

## Print sheet

![Print file icon](../../Resources/FastPrintSheet.png)

Simular to the previous one but print the active sheet.
