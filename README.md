# PointSwitch

[English README](README_en.md)

> 适用于Akebi/Korepi使用的点位选择器 

> Json_Integration点位格式[Json_Integration](https://github.com/Xcating/Json_Integration) 

> Teyvat_TP_Json点位格式[Teyvat_TP_Json](https://github.com/chiqingsan/Teyvat_TP_Json)

## Introduction





![image](https://github.com/zfonlyone/PointSwitch/blob/main/IMG/1.png)

![image](https://github.com/zfonlyone/PointSwitch/blob/main/IMG/2.png)





- 1选择目标目录(Korepi默认读取文件夹)

- 2选择点位目录(保存有.json文件的文件夹)

- 3在文件夹列表树点击选中要导入的.json文件夹

- 4复制文件
  - 增量添加：将选择的文件夹里的文件复制到目标文件夹,不影响原有文件。
  - 一键替换：删除原目标文件夹所有文件，复制选择的文件夹文件到目标文件夹，会删除目录下的所有文件。
	
- 5如果复制到目标文件夹有同名文件可使用批量重命名修改冲突文件，使用方法为：
  - 1)选择冲突文件夹
  - 2)在输入框输入重命名名称
  - 3)点击批量重命名
  - Tips:
		冲突json文件中key键“name”的值修改为输入框输入的名称
		修改后将自动重命名json文件名



## Warning
- 一键替换将删除目标文件夹里所有文件，请做好备份。
- 批量重命名将修改选择文件夹里所有json文件。
- 替换操作会将选择文件夹子目录文件复制到同一级目录中，注意避免同名文件冲突。


## Quick start
- 可以在[Release Page](https://github.com/zfonlyone/PointSwitch/releases)下载最新版本。





## Credits

- （原项目）Inspired by https://github.com/linzhibinghan/PointSwitch

