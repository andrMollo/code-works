# CodeWorks

An add-in for SolidWorks with a set of tools to automate your workflow.

CodeWorks uses the [xCAD.NET](https://xcad.xarial.com/) framework for the add-in registration and connection to SolidWorks. It also uses [SolidDNA](https://github.com/CAD-Booster/solidworks-api) as a wrapper around the core of SolidWorks API.

CodeWorks can help you in different areas:

* [Manage SolidWorks custom properties](/CodeWorksLibrary/Macros/Properties/README.md)
* [Export files](/CodeWorksLibrary/Macros/Export/README.md)
* [Manage SolidWorks drawings formats](/CodeWorksLibrary/Macros/Drawings/README.md)

## Installation

At the moment CodeWorks comes without an installer. To use CodeWorks you will have to clone this repository and compile the solution on your PC. xCAD.NET will automatically register the add-in for you.

You also create a file `Private\GlobalConfigPrivate.cs`:

```cs
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
        public const string MyDmLicense = "YOUR-DOCUMENT-MANAGER-LICENSE-KEY";

        /// <summary>
        /// The path to my map for the new sheet format
        /// </summary>
        public const string MyReplaceMapPath = @"FULL_PATH_TO_REPLACE_MAP.txt";
    }
}
```

See also [update sheet format](/CodeWorksLibrary/Macros/Drawings/README.md) for more details on the format for the replace map text file.

## References

* [Implementing SOLIDWORKS® Top Ten List Ideas 2020 from scratch using xCAD.NET](https://www.youtube.com/watch?v=BuiFfv7-Qig): how to implement a SolidWorks add-in with [xCAD.NET](https://xcad.xarial.com/) framework.
* [SolidDNA YouYbe playlist by AngelSix](https://www.youtube.com/playlist?list=PLrW43fNmjaQVMN1-lsB29ECnHRlA4ebYn): a series of videos providing a great introduction to programming in C# with the SOlidWorks API
