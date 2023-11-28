# Export macros

A collection of methods to export SolidWorks files.

## Export file

![Export file icon](../../Resources/ExportFile.png "Export file")

Export the open file to different formats. The root output folder is defined in `GlobalConfig.ExportPath`, a set of sub-folder is created based on the output format (PDF, DWG...). The output file name is equal to the SolidWorks one.

If the open file is a drawing it's saved as DWG and PDF, then the referenced model is exported to STEP and it's preview to PNG.

It the open file is a model it's saved to STEP and it's preview to PNG. the method then tries to one a drawing document with the same name as the model in the same folder; if a drawing is found then it's exported as PDF and DWG.

### Prerequisites

* A SolidWorks file is open and saved.
* This method uses the SolidWorks Document Manger API. You should create a file in `CodeWorksLibrary\Private\GlobalConfigPrivate` where you define

```c#
namespace CodeWorksLibrary
{
    internal class GlobalConfigPrivate
    {
        /// <summary>
        /// The name of the Vault database
        /// </summary>
        public const string MyVaultName = "VAULT_BL";

        /// <summary>
        /// The license key for the Document Manager API
        /// </summary>
        public const string MyDmLicense = "YOUR_DOCUMENT_MANAGER_LICENSE_KEY";
    }
}
```
