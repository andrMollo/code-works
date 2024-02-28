# File commands

## Close No Active

Macro to close all SolidWorks documents except the active one.

### References

- [Original macro from CodeStack](https://www.codestack.net/solidworks-api/application/frame/close-all-documents-except-active/)

## Open component folder

 Open the explorer folder for the selected component.

### References

- [Original macro from CodeStack](https://www.codestack.net/solidworks-api/document/assembly/components/show-selected-assembly-component-window-folder/)


## Save file

Save as a copy the target model with its drawing. Drawing must have same name and path of target model. Target model is the selected component(s) or the active component if nothing is selected. Multiple selected components are allowed but must be identical.

The macro can replace the new model in all the selected instances.

The macro can also name the new file and its drawing according to a PDM part number.

The macro also performs some custom properties clean-up.

### Prerequisites

- A part or an assembly is open and active
- Drawing must have same name and path of target model

### References

- [Make independent with drawing from CodeStack](https://www.codestack.net/solidworks-api/document/assembly/components/make-independent-drawing/)
- [Replace components from CodeStack](https://www.codestack.net/solidworks-api/document/assembly/components/replace/)
- [Read custom properties from file, configuration and cut-list elements using SOLIDWORKS API](https://www.codestack.net/solidworks-api/data-storage/custom-properties/read-all-properties/)


