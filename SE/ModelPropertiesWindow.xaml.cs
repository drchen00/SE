using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Configuration;

namespace SE
{
    /// <summary>
    /// ModelProperties.xaml 的交互逻辑
    /// </summary>
    public partial class modelPropertiesWindow : Window
    {
        private MainWindow mainWindow;
        private ProjectofParts defaultProject;
        private int selectedItemNum;
        public string selectPath;
        public modelPropertiesWindow()
        {
            InitializeComponent();
        }

        public modelPropertiesWindow(MainWindow mainWindow, int selectedItem)
        {
            InitializeComponent();         
            init(mainWindow, selectedItem);            
        }

        public modelPropertiesWindow(MainWindow mainWindow, int selectedItem, string filePath)
        {
            InitializeComponent();            
            init(mainWindow, selectedItem, filePath);
        }

        /// <summary>
        /// 对各项数据进行初始化
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <param name="selectedItem"></param>
        /// <param name="filePath"></param>
        private void init(MainWindow mainWindow = null, int selectedItem = 1, string filePath = null)
        {
            this.mainWindow = mainWindow;
            selectedItemNum = selectedItem;
            if (selectedItem > 1)
            {
                defaultProject = mainWindow.projectList[selectedItem - 2];
            }
            this.selectPath = filePath;           
            getProjectCBItems();
            setDefaultDate();
            setDefaultPath();
            setDefaultCreator();
        }

        /// <summary>
        /// 初始化项选择表
        /// </summary>
        private void getProjectCBItems()
        {
            foreach(ProjectofParts project in mainWindow.projectList)
            {                
                projectComboBox.Items.Add(project.name);
                if (defaultProject != null && project.name.Equals(defaultProject.name))
                {
                    projectComboBox.Text = project.name;
                }
            }
        }

        /// <summary>
        /// 初始化时间选择
        /// </summary>
        private void setDefaultDate()
        {
            datePicker.Text = DateTime.Now.ToShortDateString();
        }

        /// <summary>
        /// 文本框初始化
        /// </summary>
        private void setDefaultCreator()
        {
            creatorTextBox.Text = ConfigurationManager.AppSettings["Creator"];
        }

        private void setDefaultName()
        {
            if (selectPath == null) return;
            nameTextBox.Text = System.IO.Path.GetFileNameWithoutExtension(selectPath);
        }

        private void setDefaultPath()
        {
            if (selectPath == null) return;
            filePathTextBox.Text = selectPath;
        }
               
        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 文件路径选择按钮触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = ConfigurationManager.AppSettings["Path"];
                openFileDialog.Filter = "(*.par)|*.par|(*.asm)|*.asm|All Files(*.par,*.asm)|*.par;*.asm";
                openFileDialog.FilterIndex = 3;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        filePathTextBox.Text = openFileDialog.FileName;
                        Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        cfa.AppSettings.Settings["Path"].Value = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                        cfa.Save(ConfigurationSaveMode.Modified);
                        ConfigurationManager.RefreshSection("appSettings");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                        throw (ex);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        /// <summary>
        /// 保存动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (nameTextBox.Text == "" || creatorTextBox.Text == "" || datePicker.Text == "" || projectComboBox.Text == "" || typeComboBox.Text == "" || filePathTextBox.Text == "")
                {
                    MessageBox.Show("请确保全部信息输入完成");
                    return;
                }
                if (addPartFile() && addPartXml())
                {
                    mainWindow.readParts();
                    mainWindow.initTabControl(selectedItemNum);
                    Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    cfa.AppSettings.Settings["Creator"].Value = creatorTextBox.Text;
                    cfa.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                    this.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 将新零件写入文件
        /// </summary>
        /// <returns></returns>
        private bool addPartXml()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(@MainWindow.sourcePath, settings);
            xmlDoc.Load(reader);
            XmlNode xmlPartData = xmlDoc.SelectSingleNode("part_data");
            XmlNode xmlProject;
            reader.Close();

            XmlElement newXmlPart = xmlDoc.CreateElement("part");
            newXmlPart.SetAttribute("Name", nameTextBox.Text);
            newXmlPart.SetAttribute("Creator", creatorTextBox.Text);
            newXmlPart.SetAttribute("CreationTime", datePicker.Text);
            newXmlPart.SetAttribute("Project", projectComboBox.Text);
            newXmlPart.SetAttribute("Type", typeComboBox.Text);
            if ((bool)enginDrwCheckBox.IsChecked)
            {
                newXmlPart.SetAttribute("EnginDrw", "1");
            }
            else
            {
                newXmlPart.SetAttribute("EnginDrw", "0");
            }
            if ((bool)AsmModelCheckBox.IsChecked)
            {
                newXmlPart.SetAttribute("AsmModel", "1");
            }
            else
            {
                newXmlPart.SetAttribute("AsmModel", "0");
            }

            string projectPath = string.Format("/part_data/project[@Name=\"{0}\"]", newXmlPart.GetAttribute("Project").ToString());
            xmlProject = xmlPartData.SelectSingleNode(projectPath);
            if (xmlProject == null)
            {
                xmlProject = xmlDoc.CreateElement("project");
                ((XmlElement)xmlProject).SetAttribute("Name", projectComboBox.Text);
                xmlPartData.AppendChild(xmlProject);
            }
            string partPath = string.Format("/part_data/project[@Name=\"{0}\"]/part[@Name=\"{1}\"]", newXmlPart.GetAttribute("Project").ToString(), newXmlPart.GetAttribute("Name").ToString());
            if (xmlProject.SelectSingleNode(partPath) != null)
            {
                MessageBox.Show("该项目已有同名文件");
                return false;
            }
            xmlProject.AppendChild(newXmlPart);           
            xmlDoc.Save(@MainWindow.sourcePath);
            return true;
        }

        /// <summary>
        /// 将零件文件移到指定文件夹
        /// </summary>
        /// <returns></returns>
        private bool addPartFile()
        {
            string dirPath = @MainWindow.fileFolderPath + "\\" + projectComboBox.Text;
            string filePath = dirPath + "\\" + System.IO.Path.GetFileName(filePathTextBox.Text);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            if (!File.Exists(filePath))
            {
                File.Copy(@filePathTextBox.Text, filePath);
            }
            if (System.IO.Path.GetExtension(filePathTextBox.Text).Equals(".asm"))
            {
                filePath = dirPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(filePathTextBox.Text) + ".cfg";
                if (!File.Exists(filePath) && File.Exists(System.IO.Path.ChangeExtension(filePathTextBox.Text, "cfg")))
                {
                    File.Copy(System.IO.Path.ChangeExtension(filePathTextBox.Text, "cfg"), filePath);
                }
            }            
            return true;
        }

        /// <summary>
        /// 自动显示零件名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filePathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            selectPath = filePathTextBox.Text;
            setDefaultName();
        }
    }
}
