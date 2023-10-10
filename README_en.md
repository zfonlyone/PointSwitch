# PointSwitch

[中文版 README](README.md)

> A point selector for use with Akebi/Korepi.

> Point format for Json_Integration [Json_Integration](https://github.com/Xcating/Json_Integration)

> Point format for Teyvat_TP_Json [Teyvat_TP_Json](https://github.com/chiqingsan/Teyvat_TP_Json)

## Introduction



![image](https://github.com/zfonlyone/PointSwitch/blob/main/IMG/1.png)

![image](https://github.com/zfonlyone/PointSwitch/blob/main/IMG/2.png)

- 1 Select the target directory.
- 2 Select the point directory.
- 3 Choose the folder to import in the folder tree.
- 4 Copy files.
   - Incremental Add: Copy files from the selected folder to the target folder.
   - One-click Replace: Delete all files in the original target folder and copy files from the selected folder to the target folder.
- 5 If there are files with the same name in the target folder, you can use batch renaming to resolve conflicts:
   - 1) Select the conflicting folder.
   - 2) Enter a renaming name in the input box.
   - 3) Click Batch Rename.
   - Tips:
     Modify the value of the "name" key in the conflicting JSON files to the name entered in the input box.
     The JSON file name will be automatically renamed after modification.

## Warning

- One-click Replace will delete all files in the target folder, please make a backup.
- Batch renaming will modify all JSON files in the selected folder.
- The replacement operation will copy subdirectory files from the selected folder to the same level directory, so be careful of naming conflicts.

## Quick Start

- You can download the latest version from the [Release Page](https://github.com/zfonlyone/PointSwitch/releases).

## Credits

- Inspired by [Original Project](https://github.com/linzhibinghan/PointSwitch)
