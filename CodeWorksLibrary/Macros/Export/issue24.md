# CL: Export Document class

Pseudo code to export a SolidWorks model.

## Properties

## Private

- [x] ModelNameNoExt
- [x] ExportPath string

## Public

- [x] JobNumber string
- [x] PrintSelection bool

## M: Export document macro

- [ ] Validation
  - [ ] file is open
  - [ ] file is saved
- [ ] Set job folder as empty string

### M: Export Document

- [ ] get file name without extension
- [ ] M: validate job number
- [ ] set export path: GlobalConfig + jobNumber
- [ ] if open file is drawing
  - [ ] **M: export drawing and preview**
    - [ ] sheet loop
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
    - [ ] M: export preview
  - [ ] M: get parent model
  - [ ] M: export parent model
- [ ] if is model
  - [ ] M: export model
  - [ ] open drawing
  - [ ] M: export drawing and preview

## M: Export and print document macro

- [ ] Validation
  - [ ] file is open
  - [ ] file is saved
- [ ] Set job folder as empty string
- [ ] set print as true
- [ ] export document
