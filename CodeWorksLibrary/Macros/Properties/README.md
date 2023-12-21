# Custom property macros

A collection of methods to manage SolidWorks custom properties.

## Set author

![Set author icon](../../Resources/SetAuthor.png "Set author")

Write the name of the user connected to the PDM in the custom property specified in `GlobalConfig.AuthorPropName`. The property is set for the selected components or for the active document is the selection is empty.

If the connection with the PDM can't be established, the macro set the custom property with the username of the windows login.

### Prerequisites

* A part or assembly file is open.

## Write quantities

![Write quantities](../../Resources/WriteQuantity.png "Write quantities")

Update the quantity properties in all the components of the assembly with the actual count of instances, multiplied by the assembly quantity.

Component that are suppressed or excluded from the Bill of Material are not counted for. Different configuration of the same component are not counted as separate but added to each other.

The name of the quantity custom property is set buy `GlobalConfig.QuantityProperty`.
