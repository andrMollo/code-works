# Class: ExportAssembly

Pseudo code to export all components in assembly

## Naming Conventions

- Assembly abbreviation: asm
- Export abbreviation: exp

## Public properties

- [ ] public static string JobNumber { get; set; }
- [ ] public static Logger AssExpLog { get; set; }

## Private fields

## public static void ExportAssyMacro

- [ ] Validation
  - [ ] File is open
  - [ ] File is saved
  - [ ] File is assembly
- [ ] [Export assembly](#private-static-void-exportassembly)

## private static void ExportAssembly

- [ ] Resolve ResolveLightweight to get assembly quantity?
- [ ] Get assembly quantity --> mew method in property manager (validate quantity) string return?
- [ ] New logger
- [ ] Compose log path
- [ ] New WinForm
  - [ ] WinForm setup: quantity and logPath
- [ ] If WinForm ok
  - [ ] Start time
  - [ ] ResolveLightweight of all components?
  - [ ] Get value from UI
    - [ ] user selection (user selection as property?) --> new method
    - [ ] quantity and write back to property --> new method
    - [ ] job number
    - [ ] log path
  - [ ] Get root component
  - [ ] Get bom to export (based on log and user selection) + assembly
  - [ ] [Export all components](#private-static-void-exportallcomponents)
  - [ ] Stop timer

## private static void ExportAllComponents

- [ ] Check if log exists
- [ ] If log don't exists: write log first line
- [ ] Loop through bom
  - [ ] [Export assembly component](#private-static-void-exportassemblycomponent)

## private static void ExportAssemblyComponent

- [ ] Write component quantity
- [ ] Get drawing path
- [ ] Try to open drawing
- [ ] Set export model
- [ ] Set export job number
- [ ] Set print \ export selection
- [ ] Export\print
- [ ] Delete job number
