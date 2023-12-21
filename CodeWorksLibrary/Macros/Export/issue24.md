# CL: Export Document class

Pseudo code to export a SolidWorks model.

## Properties

## Private

- [x] ModelNameNoExt
- [x] ExportPath string

## Public

- [x] JobNumber string
- [x] PrintSelection bool

## PM: Export document macro

- [x] Validation
  - [x] file is open
  - [x] file is saved
- [x] Set job folder as empty string

### M: Export Document

- [x] get file name without extension
- [x] M: validate job number
- [x] set export path: GlobalConfig + jobNumber
- [x] if open file is drawing
  - [x] **MP: export drawing and preview**
    - [x] Get all the sheet
    - [ ] sheet loop from the active sheet
      - [ ] **M: ExportSheet**
      - [ ] ? is sheet NOT flat config ?
        - [ ] M: update format
        - [ ] get sheet name
        - [ ] M: export sheet to PDF
        - [ ] M: export sheet to DWG
        - [ ] if print == true
          - [ ] M: set printed on and printed by
          - [ ] M: change layer visibility
          - [ ] M: print sheet
          - [ ] M: change layer visibility
    - [ ] Return to active sheet
    - [ ] M: export preview
  - [ ] M: get parent model
  - [ ] M: export parent model
- [ ] if is model
  - [ ] M: export model
  - [ ] open drawing
  - [ ] M: export drawing and preview

## PM: Export and print document macro

- [ ] Validation
  - [ ] file is open
  - [ ] file is saved
- [ ] Set job folder as empty string
- [ ] set print as true
- [ ] export document
