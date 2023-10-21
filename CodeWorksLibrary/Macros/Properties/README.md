# Custom property macros

A collection of methods to manage SolidWorks custom properties.

## Set author

![Set author icon](../../Resources/SetAuthor.png "Set author")

Write the name of the user connected to the PDM in the custom property specified in `GlobalConfig.AuthorPropName`. The property is set for the selected components or for the active document is the selection is empty.

If the connection with the PDM can't be established, the macro set the custom property with the username of the windows login.

### Prerequisites

* A part or assembly file is open.
