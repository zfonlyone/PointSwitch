using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using static System.Net.WebRequestMethods;
using System.Globalization;
using System.Threading;
using System.Resources;
using PointSwitch.Properties; // 确保使用你的资源文件的正确命名空间

namespace MyPointSwitchApp
{
    public partial class Form1 : Form
    {
        private string iniFilePath;

        private ResourceManager rm;

        private string info_IO_error = PointSwitch.Properties.Resources.info_IO_error;
        private string info_Dont_permission = PointSwitch.Properties.Resources.info_Dont_permission;
        private string info_Dont_exist = PointSwitch.Properties.Resources.info_Dont_exist;
        private string info_OK_copy = PointSwitch.Properties.Resources.info_OK_copy;
        private string info_select_folder = PointSwitch.Properties.Resources.info_select_folder;
        private string info_OK_replaced = PointSwitch.Properties.Resources.info_OK_replaced;
        private string info_INI_error = PointSwitch.Properties.Resources.info_INI_error;
        private string info_valid_folder = PointSwitch.Properties.Resources.info_valid_folder;
        private string info_IO_samefile = PointSwitch.Properties.Resources.info_IO_samefile;
        private string info_rename = PointSwitch.Properties.Resources.info_rename;


        public Form1()
        {
            rm = new ResourceManager("PointSwitch.Properties.Resources", typeof(Form1).Assembly);
            
            InitializeComponent();

            // 获取程序根目录下的INI文件路径
            iniFilePath = Path.Combine(Application.StartupPath, "config.ini");

            // 在窗体加载时从INI文件中加载配置信息
            LoadConfigFromIni();
            
    }

        private void LoadConfigFromIni()
        {
            try
            {
                // 检查INI文件是否存在
                if (System.IO.File.Exists(iniFilePath))
                {
                    // 创建一个INI文件读取器
                    using (StreamReader reader = new StreamReader(iniFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            // 解析INI文件中的配置项
                            if (line.StartsWith("TextBox1Value="))
                            {
                                textBox1.Text = line.Substring("TextBox1Value=".Length);
                            }
                            else if (line.StartsWith("TextBox3Value="))
                            {
                                textBox3.Text = line.Substring("TextBox3Value=".Length);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(info_INI_error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var folderBrowser = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowser.ShowDialog();
                if (result == DialogResult.OK)
                {
                    textBox1.Text = folderBrowser.SelectedPath;
                }
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            using (var folderBrowser = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowser.ShowDialog();
                if (result == DialogResult.OK)
                {
                    textBox3.Text = folderBrowser.SelectedPath;
                    ShowSubDirectoriesInTreeView(textBox3.Text, treeView1);
                }
            }
        }

        private void button01_Click(object sender, EventArgs e)
        {
            string directoryPath = textBox1.Text;

            if (!string.IsNullOrWhiteSpace(directoryPath))
            {
                // 使用默认的文件资源管理器打开目录
                Process.Start("explorer.exe", directoryPath);
            }
            else
            {

                MessageBox.Show(info_valid_folder);
            }
        }

        private void button03_Click(object sender, EventArgs e)
        {
            string directoryPath = textBox3.Text;

            if (!string.IsNullOrWhiteSpace(directoryPath))
            {
                // 使用默认的文件资源管理器打开目录
                Process.Start("explorer.exe", directoryPath);
            }
            else
            {
                MessageBox.Show(info_valid_folder);

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string sourceFolderPath = textBox1.Text;
            ClearFolder(sourceFolderPath);
        }


        private void button5_Click(object sender, EventArgs e)
        {
            
            ShowSubDirectoriesInTreeView(textBox3.Text, treeView1);
        }



        //全选复制到目标文件夹
        private void button6_Click(object sender, EventArgs e)
        {
            string targetFolderPath = textBox1.Text;
            TreeNode selectedNode = treeView1.SelectedNode;

            if (selectedNode == null)
            {

                MessageBox.Show(info_select_folder);
                return;
            }
            ClearFolder(targetFolderPath);
            string sourceFolderPath = Path.Combine(textBox3.Text, selectedNode.FullPath);
            
            
            int replacedFilesCount = CopyFiles(sourceFolderPath, targetFolderPath);


            MessageBox.Show(info_OK_replaced);
        }




        //复制到目标文件夹
        private void button9_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = treeView1.SelectedNode;
            if (selectedNode == null)
            {

                MessageBox.Show(info_select_folder);
                return;
            }

            string selectedFolderName = selectedNode.FullPath;


            string sourceFolderPath = Path.Combine(textBox3.Text, selectedFolderName);
            string targetFolderPath = textBox1.Text;

            int copiedFilesCount = CopyFiles(sourceFolderPath, targetFolderPath);
            int targetFilesCount = Directory.GetFiles(targetFolderPath, "*", SearchOption.AllDirectories).Length;


            MessageBox.Show(info_OK_copy);
        }




        private void ShowSubDirectoriesInTreeView(string folderPath, System.Windows.Forms.TreeView treeView)
        {
            treeView.Nodes.Clear();
            try
            {
                string[] subDirectories = Directory.GetDirectories(folderPath);
                foreach (string subDirectory in subDirectories)
                {
                    string folderName = new DirectoryInfo(subDirectory).Name;
                    TreeNode node = new TreeNode(folderName);
                    treeView.Nodes.Add(node);
                    ShowSubDirectoriesRecursive(subDirectory, node);
                }
            }
            catch (UnauthorizedAccessException)
            {

                MessageBox.Show(info_Dont_permission);
            }
            catch (DirectoryNotFoundException)
            {

                MessageBox.Show(info_Dont_exist);
            }
            catch (IOException)
            {

                MessageBox.Show(info_IO_error);
            }
        }

        private void ShowSubDirectoriesRecursive(string path, TreeNode parentNode)
        {
            try
            {
                string[] subDirectories = Directory.GetDirectories(path);
                foreach (string subDirectory in subDirectories)
                {
                    string folderName = new DirectoryInfo(subDirectory).Name;
                    TreeNode node = new TreeNode(folderName);
                    parentNode.Nodes.Add(node);
                    ShowSubDirectoriesRecursive(subDirectory, node);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Handle unauthorized access exception, skip this folder
            }
            catch (DirectoryNotFoundException)
            {
                // Handle directory not found exception, skip this folder
            }
            catch (IOException)
            {
                // Handle other IO errors, skip this folder
            }
        }

        private void ClearFolder(string folderPath)
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(folderPath);
                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo subDirectory in directory.GetDirectories())
                {
                    subDirectory.Delete(true);
                }
            }
            catch (UnauthorizedAccessException)
            {

                MessageBox.Show(info_Dont_permission);
            }
            catch (DirectoryNotFoundException)
            {

                MessageBox.Show(info_Dont_exist);
            }
            catch (IOException)
            {

                MessageBox.Show(info_IO_error);
            }
        }

        private int CopyFiles(string sourceFolderPath, string targetFolderPath)
        {
            int replacedFilesCount = 0;
            try
            {
                Directory.CreateDirectory(targetFolderPath);
                foreach (string sourceFilePath in Directory.GetFiles(sourceFolderPath))
                {
                    string fileName = Path.GetFileName(sourceFilePath);
                    string targetFilePath = Path.Combine(targetFolderPath, fileName);
                    replacedFilesCount++;
                    System.IO.File.Copy(sourceFilePath, targetFilePath, false);//是否覆盖
                }
                foreach (string sourceSubFolderPath in Directory.GetDirectories(sourceFolderPath))
                {
                    string subFolderName = new DirectoryInfo(sourceSubFolderPath).Name;
                    //string targetSubFolderPath = Path.Combine(targetFolderPath, subFolderName);
                    replacedFilesCount += CopyFiles(sourceSubFolderPath, targetFolderPath);
                }
            }
            catch (UnauthorizedAccessException)
            {

                MessageBox.Show(info_Dont_permission);
            }
            catch (DirectoryNotFoundException)
            {

                MessageBox.Show(info_Dont_exist);
            }
            catch (IOException)
            {

                MessageBox.Show(info_IO_samefile);
            }
            return replacedFilesCount;
        }

        private void SaveConfigToIni()
        {
            try
            {
                // 创建一个INI文件写入器
                using (StreamWriter writer = new StreamWriter(iniFilePath))
                {
                    // 写入textBox1、textBox2、textBox3和textBox4的内容到INI文件
                    writer.WriteLine("[Config]");
                    writer.WriteLine($"TextBox1Value={textBox1.Text}");
                    writer.WriteLine($"TextBox3Value={textBox3.Text}");


                    // 确保数据被立即写入到文件中
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                // 错误处理代码，您可以选择输出错误日志或记录日志文件等，而不是弹出消息框


                Console.WriteLine(info_INI_error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // 当 textBox1 文本内容发生变化时，保存配置信息到INI文件
            SaveConfigToIni();
        }


        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // 当 textBox3 文本内容发生变化时，保存配置信息到INI文件
            SaveConfigToIni();
        }


        //批量重命名
        private void button7_Click(object sender, EventArgs e)
        {
            
            //初始化读取目录
            TreeNode selectedNode = treeView1.SelectedNode;

            if (selectedNode == null)
            {

                MessageBox.Show(info_select_folder);
                return;
            }
            string sourceFolderPath = Path.Combine(textBox3.Text, selectedNode.FullPath);
            //设置重命名文件名
            string jsonName = textBox2.Text;
            //读取json
            //文件名+文件数量  优化子文件夹

            int count=Changejsonname(sourceFolderPath, jsonName);

            MessageBox.Show(info_rename);
            //批量修改json标签

            //重命名文件json
        }


        private int Changejsonname(string sourceFolderPath, string newname)
        {
            int replacedFilesCount = 0;
            try
            {
                foreach (string sourceFilePath in Directory.GetFiles(sourceFolderPath))
                {
                    string fileName = Path.GetFileName(sourceFilePath);
                    //string targetFilePath = Path.Combine(targetFolderPath, fileName);
                    replacedFilesCount++;

                    ChangejsonValue(sourceFilePath, "name", (newname + replacedFilesCount));

                    if (System.IO.File.Exists(sourceFilePath))
                    {
                        System.IO.File.Move(sourceFilePath, sourceFolderPath + "\\" + newname + replacedFilesCount + ".json");
                    }  
                }
                foreach (string sourceSubFolderPath in Directory.GetDirectories(sourceFolderPath))
                {
                    string subFolderName = new DirectoryInfo(sourceSubFolderPath).Name;

                    replacedFilesCount += Changejsonname(sourceSubFolderPath,(newname+ (replacedFilesCount+1)));
                }

            }
            catch (IOException)
            {

                MessageBox.Show(info_IO_error);
            }
            return replacedFilesCount;
        }


        private void ChangejsonValue(string jsonfile, string key, string Value)
        {
            try
            {
                string jsonString = System.IO.File.ReadAllText(jsonfile);//读取文件
                JObject jobject = JObject.Parse(jsonString);//解析成json
                jobject[key] = Value;//替换需要的文件
                string convertString = Convert.ToString(jobject);//将json装换为string
                System.IO.File.WriteAllText(jsonfile, convertString);//将内容写进jon文件中

            }
            catch (IOException)
            {


                MessageBox.Show(info_IO_error);


            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
            {
                // 切换到中文区域性
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-CN");
                button2.Text = PointSwitch.Properties.Resources.Language_English; // 更新按钮文本
                button1.Text = PointSwitch.Properties.Resources.teleports_Folder;
                button3.Text = PointSwitch.Properties.Resources.point_Folder;
                button5.Text = PointSwitch.Properties.Resources.Reload;
                button6.Text = PointSwitch.Properties.Resources.Replace;
                button01.Text = PointSwitch.Properties.Resources.open_folder;
                button03.Text = PointSwitch.Properties.Resources.open_folder;
                button9.Text = PointSwitch.Properties.Resources.incremental_addition;
                button4.Text = PointSwitch.Properties.Resources.Clear_directory;
                button7.Text = PointSwitch.Properties.Resources.Batch_Rename;
                Text = PointSwitch.Properties.Resources.pointswitch;

                info_IO_error = PointSwitch.Properties.Resources.info_IO_error;
                info_Dont_permission = PointSwitch.Properties.Resources.info_Dont_permission;
                info_Dont_exist = PointSwitch.Properties.Resources.info_Dont_exist;
                info_OK_copy = PointSwitch.Properties.Resources.info_OK_copy;
                info_select_folder = PointSwitch.Properties.Resources.info_select_folder;
                info_OK_replaced = PointSwitch.Properties.Resources.info_OK_replaced;
                info_INI_error = PointSwitch.Properties.Resources.info_INI_error;
                info_valid_folder = PointSwitch.Properties.Resources.info_valid_folder;
                info_IO_samefile = PointSwitch.Properties.Resources.info_IO_samefile;
                info_rename = PointSwitch.Properties.Resources.info_rename;



    }
            else
            {
                // 切换到英文区域性
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                button2.Text = PointSwitch.Properties.Resources.Language_Chinese; // 更新按钮文本
                button1.Text = PointSwitch.Properties.Resources.teleports_Folder_en;
                button3.Text = PointSwitch.Properties.Resources.point_Folder_en;
                button5.Text = PointSwitch.Properties.Resources.Reload_en;
                button6.Text = PointSwitch.Properties.Resources.Replace_en;
                button01.Text = PointSwitch.Properties.Resources.open_folder_en;
                button03.Text = PointSwitch.Properties.Resources.open_folder_en;
                button9.Text = PointSwitch.Properties.Resources.incremental_addition_en;
                button4.Text = PointSwitch.Properties.Resources.Clear_directory_en;
                button7.Text = PointSwitch.Properties.Resources.Batch_Rename_en;
                Text = PointSwitch.Properties.Resources.pointswitch_en;


                info_IO_error = PointSwitch.Properties.Resources.info_IO_error_en;
                info_Dont_permission = PointSwitch.Properties.Resources.info_Dont_permission_en;
                info_Dont_exist = PointSwitch.Properties.Resources.info_Dont_exist_en;
                info_OK_copy = PointSwitch.Properties.Resources.info_OK_copy_en;
                info_select_folder = PointSwitch.Properties.Resources.info_select_folder_en;
                info_OK_replaced = PointSwitch.Properties.Resources.info_OK_replaced_en;
                info_INI_error = PointSwitch.Properties.Resources.info_INI_error_en;
                info_valid_folder = PointSwitch.Properties.Resources.info_valid_folder_en;
                info_IO_samefile = PointSwitch.Properties.Resources.info_IO_samefile_en;
                info_rename = PointSwitch.Properties.Resources.info_rename_en;


            }

            // 更新界面文本和资源
            UpdateUIForCurrentLanguage();

            // 重新加载窗体或更新文本
            this.Invalidate(true);
        }

        private void UpdateUIForCurrentLanguage()
        {
            // 创建资源管理器，用于获取资源文件中的文本
            //this.rm = new ResourceManager("PointSwitch.Properties.Resources", typeof(Form1).Assembly);

            // 遍历窗体上的所有控件
            foreach (Control control in this.Controls)
            {
                // 检查控件是否具有Name属性，并且Name属性对应于资源文件中的键
                if (!string.IsNullOrEmpty(control.Name))
                {
                    string resourceName = control.Name;

                    // 尝试从资源文件中获取与控件名称匹配的文本
                    string localizedText = rm.GetString(resourceName, Thread.CurrentThread.CurrentUICulture);

                    if (!string.IsNullOrEmpty(localizedText))
                    {
                        // 更新控件的文本属性
                        control.Text = localizedText;
                    }
                }

                // 如果控件有子控件（例如Panel内的控件），递归调用以更新所有控件
                if (control.HasChildren)
                {
                    UpdateUIForCurrentLanguage(control);
                }
            }
        }

        // 递归函数，用于更新子控件
        private void UpdateUIForCurrentLanguage(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                if (!string.IsNullOrEmpty(control.Name))
                {
                    string resourceName = control.Name;
                    string localizedText = this.rm.GetString(resourceName, Thread.CurrentThread.CurrentUICulture);

                    if (!string.IsNullOrEmpty(localizedText))
                    {
                        control.Text = localizedText;
                    }
                }

                if (control.HasChildren)
                {
                    UpdateUIForCurrentLanguage(control);
                }
            }
        }


    }
}