# CL: UpdateSheetFormat class

Pseudo code to update sheet format

## Public properties

- [ ] DrawingModel drwModel
- [ ] Sheet swSheet
- [ ] bool AlwaysReplace

## Private fields

## PD: UpdateSheetFormatMacro

- [ ] Validation
  - [ ] File is open
  - [ ] File is save
  - [ ] file is drawing
- [ ] Loop sheet starting from active
  - [ ] M: Update sheet format

## M: Update sheet format

- [ ] If not flat
  - [ ] M: Get the name of the current format
  - [ ] M: Get the path to the current format
  - [ ] M: Get the path to the new format
  - [ ] if AlwaysReplace == true
    - [ ] M: Replace format
  - [ ] if AlwaysReplace == false
    - [ ] If currentFormatPath != newFormatPath
      - [ ] M: ReplaceFormat

## PM: Check flat pattern
