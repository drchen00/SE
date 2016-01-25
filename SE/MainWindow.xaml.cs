using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using SolidEdgeCommunity;
using SolidEdgeCommunity.Extensions;

namespace SE
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string sourcePath = "..\\..\\SOURCE.xml";
        public static string fileFolderPath = "..\\..\\PARTFOLDER";
        public List<ProjectofParts> projectList;
        public List<Parts> partList;

        public MainWindow()
        {
            InitializeComponent();            
            loadSource();
           
        }

        /// <summary>
        /// 窗口初始化动态显示标签与表项
        /// </summary>
        private void loadSource()
        {
            try
            {
                readParts();
                initTabControl();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// 初始化tabContral控件
        /// </summary>
        /// <param name="defaultItemNum">设定初始选中标签</param>
        public void initTabControl(int defaultItemNum = 1)
        {
            ListBox partItemBox;
            for(int i = 2; i < tabControl.Items.Count;)
            {
                tabControl.Items.RemoveAt(i);
            }

            totalListBox.Items.Clear();
            
            foreach(Parts part in partList)
            {
                addPartItem(totalListBox, part);
            }         
            
            foreach(ProjectofParts project in projectList)
            {
                partItemBox = addTabItem(project);                
                foreach (Parts part in project.partList)
                {
                    addPartItem(partItemBox, part);
                }
            }
            tabControl.SelectedIndex = defaultItemNum;
        }

        /// <summary>
        /// 给指定标签页添加一个零件信息
        /// </summary>
        /// <param name="partItemBox">标签页</param>
        /// <param name="part">零件信息</param>
        private void addPartItem(ListBox partItemBox, Parts part, bool isChecked = false)
        {
            ListBoxItem partItem;
            Expander partExp;
            CheckBox partCB;
            Grid expGrid, detailGrid;
            Button openButton, settingButton, removeButtom;
            Label creatorLable, timeLabel, projectLable, typeLable, enginLable, asmLable;

            partItem = new ListBoxItem();
            partExp = new Expander();
            partCB = new CheckBox();
            openButton = new Button();
            settingButton = new Button();
            removeButtom = new Button();
            expGrid = new Grid();
            detailGrid = new Grid();
            creatorLable = new Label();
            timeLabel = new Label();
            projectLable = new Label();
            typeLable = new Label();
            enginLable = new Label();
            asmLable = new Label();

            creatorLable.Content = "作者：" + part.creator;
            timeLabel.Content = "上传时间：" + part.creationTime;
            projectLable.Content = "所属工程：" + part.project;
            typeLable.Content = "类型：" + part.type;
            if (Convert.ToBoolean(int.Parse(part.EnginDrw)))
            {
                enginLable.Content = "有配套工程图";
            }
            else
            {
                enginLable.Content = "没有配套工程图";
            }
            if (Convert.ToBoolean(int.Parse(part.AsmModel)))
            {
                asmLable.Content = "属于某个装配体模型";
            }
            else
            {
                asmLable.Content = "不属于某个装配体模型";
            }
            creatorLable.Margin = new Thickness(0, 0, 0, 0);
            timeLabel.Margin = new Thickness(0, 15, 0, 0);
            projectLable.Margin = new Thickness(0, 30, 0, 0);
            typeLable.Margin = new Thickness(0, 45, 0, 0);
            enginLable.Margin = new Thickness(0, 60, 0, 0);
            asmLable.Margin = new Thickness(0, 75, 0, 0);

            openButton.Content = "打开";
            settingButton.Content = "配置";
            removeButtom.Content = "移除";
            openButton.Margin = new Thickness(0, 0, 100, 0);
            settingButton.Margin = new Thickness(0, 0, 50, 0);
            removeButtom.Margin = new Thickness(0, 0, 0, 0);
            openButton.HorizontalAlignment = HorizontalAlignment.Right;
            settingButton.HorizontalAlignment = HorizontalAlignment.Right;
            removeButtom.HorizontalAlignment = HorizontalAlignment.Right;
            openButton.Focusable = false;
            removeButtom.Focusable = false;
            settingButton.Focusable = false;
            removeButtom.Click += new RoutedEventHandler(rmButtom_Click);
            settingButton.Click += new RoutedEventHandler(buttonAttributeEditor_Click );
            openButton.Click += new RoutedEventHandler(opButtom_Click);

            partCB.Content = part.name;
            partCB.FontSize = 15;
            partCB.Focusable = false;
            partCB.IsChecked = isChecked;
            partCB.Checked += new RoutedEventHandler(syncItemCheckBox);
            partCB.Unchecked += new RoutedEventHandler(syncItemCheckBox);

            expGrid.Children.Add(partCB);
            expGrid.Children.Add(openButton);
            expGrid.Children.Add(settingButton);
            expGrid.Children.Add(removeButtom);
            expGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            expGrid.Width = window.Width-150;

            detailGrid.Children.Add(creatorLable);
            detailGrid.Children.Add(timeLabel);
            detailGrid.Children.Add(projectLable);
            detailGrid.Children.Add(typeLable);
            detailGrid.Children.Add(enginLable);
            detailGrid.Children.Add(asmLable);
            expGrid.HorizontalAlignment = HorizontalAlignment.Stretch;

            partExp.Header = expGrid;
            partExp.Content = detailGrid;
            partExp.Focusable = false;
            partExp.HorizontalAlignment = HorizontalAlignment.Stretch;

            partItem.Content = partExp;
            partItem.IsSelected = false;
            partItem.Focusable = false;
            partItem.Tag = part;
            partItemBox.Items.Add(partItem);
            partItem.MouseEnter += new MouseEventHandler(this.item_MouseEnter);
            partItem.MouseLeave += new MouseEventHandler(item_MouseLeave);
            
        }

        /// <summary>
        /// 根据工程信息添加标签页
        /// </summary>
        /// <param name="project">工程信息</param>
        /// <returns>标签页</returns>
        private ListBox addTabItem(ProjectofParts project)
        {
            TabItem projectItem;
            ListBox partItemBox;

            projectItem = new TabItem();
            partItemBox = new ListBox();
            partItemBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            projectItem.HorizontalAlignment = HorizontalAlignment.Stretch;
            partItemBox.AllowDrop = true;
            partItemBox.Drop += new DragEventHandler(totalListBox_Drop);

            projectItem.Header = new TextBlock();
            ((TextBlock)projectItem.Header).TextWrapping = TextWrapping.Wrap;
            ((TextBlock)projectItem.Header).Text = project.name;
            projectItem.Width = 64;
            projectItem.Content = partItemBox;
            projectItem.Tag = project;
            tabControl.Items.Add(projectItem);

            return partItemBox;
        }

        /// <summary>
        /// 工程信息集排序
        /// </summary>
        private void projectListSort()
        {
            projectList.Sort(delegate (ProjectofParts x, ProjectofParts y)
            {
                if (x.name == null && y.name == null) return 0;
                else if (x.name == null) return -1;
                else if (y.name == null) return 1;
                else return x.name.CompareTo(y.name);
            });
        }

        /// <summary>
        /// 零件信息集排序
        /// </summary>
        private void partListSort()
        {
            partList.Sort(delegate (Parts x, Parts y)
            {
                if (x.name == null && y.name == null) return 0;
                else if (x.name == null) return -1;
                else if (y.name == null) return 1;
                else return x.name.CompareTo(y.name);
            });
        }

        /// <summary>
        /// 从文件中读取零件信息与工程信息
        /// </summary>
        public void readParts()
        {
            ProjectofParts projectData;
            Parts partData;
            string name;
            string creator;
            string creationTime;
            string project;
            string type;
            string EnginDrw;
            string AsmModel;

            projectList = new List<ProjectofParts>();
            partList = new List<Parts>();

            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(@sourcePath, settings);
            xmlDoc.Load(reader);

            XmlNode xmlPartData = xmlDoc.SelectSingleNode("part_data");
            XmlNodeList xmlProjectList = xmlPartData.ChildNodes;
            foreach(XmlNode xmlProject in xmlProjectList)
            {
                XmlElement xeProject = (XmlElement)xmlProject;
                projectData = new ProjectofParts(xeProject.GetAttribute("Name").ToString());
                XmlNodeList xmlPartList = xmlProject.ChildNodes;
                foreach(XmlNode xmlPart in xmlPartList)
                {
                    XmlElement xePart = (XmlElement)xmlPart;
                    name = xePart.GetAttribute("Name").ToString();
                    creator = xePart.GetAttribute("Creator").ToString();
                    project = xePart.GetAttribute("Project").ToString();
                    creationTime = xePart.GetAttribute("CreationTime").ToString();
                    type = xePart.GetAttribute("Type").ToString();
                    EnginDrw = xePart.GetAttribute("EnginDrw").ToString();
                    AsmModel = xePart.GetAttribute("AsmModel").ToString();

                    partData = new Parts(name, creator, creationTime, project, type, EnginDrw, AsmModel);
                    projectData.addPart(partData);
                    projectData.partListSort();
                    partList.Add(partData);

                }
                projectList.Add(projectData);
            }
            projectListSort();
            partListSort();
            reader.Close();
        }
      
        /// <summary>
        /// 加载按钮单击动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void load_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                launchModelPropertiesWindows();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 零件项动画效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_MouseEnter(object sender, RoutedEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)sender;
            item.IsSelected = true;
        }

        private void item_MouseLeave(object sender, RoutedEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)sender;
            item.IsSelected = false;
        }

        /// <summary>
        /// 通过加载按钮进入加载窗口
        /// </summary>
        private void launchModelPropertiesWindows()
        {
                modelPropertiesWindow ModelPropertiesWindow = new modelPropertiesWindow(this, tabControl.SelectedIndex);
                ModelPropertiesWindow.ShowDialog();
        }
        
        /// <summary>
        /// 拖拽文件进入加载窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void totalListBox_Drop(object sender, DragEventArgs e)
        {
            try
            {
                string msg;
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    msg = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
                }
                else
                {
                    return;
                }
                if (!(System.IO.Path.GetExtension(msg).Equals(".par") || System.IO.Path.GetExtension(msg).Equals(".asm"))) return;
                modelPropertiesWindow ModelPropertiesWindow = new modelPropertiesWindow(this, tabControl.SelectedIndex, msg);
                ModelPropertiesWindow.ShowDialog();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 隐藏搜索页标签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TabItem item = (TabItem)tabControl.SelectedItem;
                if (item == null || item.Equals(schTabItem)) return;
                schTabItem.Visibility = Visibility.Collapsed;
                fileListBox.Items.Clear();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 动态搜索触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void schTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {            
            initSchList();
        }

        /// <summary>
        /// 搜索结果并显示在搜索标签页
        /// </summary>
        private void initSchList()
        {
            try
            {
                fileListBox.Items.Clear();
                foreach (ListBoxItem item in ((ListBox)((TabItem)tabControl.Items[1]).Content).Items)
                {
                    Expander exp = (Expander)item.Content;
                    Grid grid = (Grid)exp.Header;
                    CheckBox checkbox = (CheckBox)grid.Children[0];
                    if (schTextBox.Text != "" && ((string)checkbox.Content).StartsWith(schTextBox.Text))
                    {
                        addPartItem(fileListBox, (Parts)item.Tag, (bool)checkbox.IsChecked);
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }            
        }

        /// <summary>
        /// 显示搜索标签页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void schTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            schTabItem.Visibility = Visibility.Visible;
            schTabItem.IsSelected = true;
            initSchList();
        }

        /// <summary>
        /// 从文件中移除某零件
        /// </summary>
        /// <param name="part"></param>
        private void rmXmlPart(Parts part)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(@sourcePath, settings);
            xmlDoc.Load(reader);
            reader.Close();

            XmlNode xmlPartData = xmlDoc.SelectSingleNode("part_data");
            XmlElement xePartData = (XmlElement)xmlPartData;
            string partPath = string.Format("/part_data/project[@Name=\"{0}\"]/part[@Name=\"{1}\"]", part.project, part.name);
            XmlElement xePart = (XmlElement)xePartData.SelectSingleNode(partPath);
            XmlNode xmlProject = xePart.ParentNode;
            xePart.ParentNode.RemoveChild(xePart);            
            if (!xmlProject.HasChildNodes) xmlProject.ParentNode.RemoveChild(xmlProject);
            xmlDoc.Save(@sourcePath);
        }

        /// <summary>
        /// 移除文件夹中具体零件文件
        /// </summary>
        /// <param name="part"></param>
        private void rmFile(Parts part)
        {
            string dirPath = @fileFolderPath + "\\" + part.project;
            string filePath = dirPath + "\\" + part.name;
            if (Directory.Exists(dirPath))
            {
                List<string> extension = new List<string>(3);
                extension.Add(".par");
                extension.Add(".asm");
                extension.Add(".cfg");
                for (int i = 0; i < 3; ++i)
                {
                    if (File.Exists(filePath + extension[i]))
                    {
                        File.Delete(filePath + extension[i]);
                        if (Directory.GetDirectories(dirPath).Length == 0 && Directory.GetFiles(dirPath).Length == 0)
                        {
                            Directory.Delete(dirPath);
                        }
                    }
                }                
            }                    
        }

        /// <summary>
        /// 单零件移除按钮单击触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rmButtom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                Parts part = getPartByWidget(button);
                if (part == null) return;
                rmXmlPart(part);
                rmFile(part);
                readParts();
                initTabControl(tabControl.SelectedIndex);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 根据当前项按钮获得当前项零件信息
        /// </summary>
        /// <param name="button">当前项按钮</param>
        /// <returns>零件信息</returns>
        private Parts getPartByWidget(object widget)
        {
            Grid grid = (Grid)((Control)widget).Parent;
            Expander exp = (Expander)grid.Parent;
            ListBoxItem item = (ListBoxItem)exp.Parent;
            return (Parts)item.Tag;
        }

        /// <summary>
        /// 全选触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkAllBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                checkItems();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 全选操作实现
        /// </summary>
        private void checkItems()
        {
            TabItem tabItem = (TabItem)tabControl.SelectedItem;
            ListBox listBox = (ListBox)tabItem.Content;
            Expander exp;
            Grid grid;
            CheckBox checkBox;

            foreach(ListBoxItem LBItem in listBox.Items)
            {
                exp = (Expander)LBItem.Content;
                grid = (Grid)exp.Header;
                checkBox = (CheckBox)grid.Children[0];
                checkBox.IsChecked = checkAllBox.IsChecked;
            }
        }

        /// <summary>
        /// 取消全选触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkAllBox_Unchecked(object sender, RoutedEventArgs e)
        {
            checkItems();
        }
               
        /// <summary>
        /// 根据勾选情况对选中标签项中零件批量删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rmAllButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TabItem tabItem = (TabItem)tabControl.SelectedItem;
                ListBox listBox = (ListBox)tabItem.Content;
                Expander exp;
                Grid expGrid;
                CheckBox checkBox;
                int a = 0, b = 0;

                foreach (ListBoxItem LBItem in listBox.Items)
                {
                    a++;
                    exp = (Expander)LBItem.Content;
                    expGrid = (Grid)exp.Header;
                    checkBox = (CheckBox)expGrid.Children[0];
                    if ((bool)checkBox.IsChecked)
                    {
                        b++;
                        //Parts part = getPartByButton((Button)expGrid.Children[1]);
                        Parts part = (Parts)LBItem.Tag;
                        if (part == null) return;
                        rmXmlPart(part);
                        rmFile(part);
                    }
                }
                readParts();
                if (a != b)
                {
                    initTabControl(tabControl.SelectedIndex);
                }
                else
                {
                    initTabControl();
                }
                checkAllBox.IsChecked = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonAttributeEditor_Click(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// 单零件打开按钮单击触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void opButtom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                Parts part = getPartByWidget(button);
                if (part == null) return;
                //rmXmlPart(part);
                //创建一个线程来打开零件文档
                Thread t1 = new Thread(new ParameterizedThreadStart(topen));
                t1.IsBackground = true;
                t1.Start(part);
                //readParts();
                //initTabControl(tabControl.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        //打开文档的线程函数
        public void topen(object data)
        {
            Parts part = data as Parts;
            opFile(part);
        }
        /// <summary>
        /// 打开文件夹中具体零件文件
        /// </summary>
        /// <param name="part"></param>
        private void opFile(Parts part)
        {
            string dirPath = @fileFolderPath + "\\" + part.project;
            string filePath = dirPath + "\\" + part.name;
            // Connect to Solid Edge.
            var application = SolidEdgeUtils.Connect(true, true);
            // Get a reference to the Documents collection.
            var documents = application.Documents;
            // Get a folder that has Solid Edge files.
            var folder = new DirectoryInfo(dirPath);
            // Get the installed version of Solid Edge.
            var solidEdgeVesion = application.GetVersion();
            // Disable prompts.
            application.DisplayAlerts = false;

            if (Directory.Exists(dirPath))
            {
                List<string> extension = new List<string>(3);
                extension.Add(".par");
                extension.Add(".asm");
                //extension.Add(".cfg");
                if (File.Exists(filePath + extension[0]))
                {
                    //string partName = System.IO.Path.GetFileName();
                    string fullPath = System.IO.Path.GetFullPath(filePath + extension[0]);
                    // Open the document.
                    var document = (SolidEdgeFramework.SolidEdgeDocument)documents.Open(fullPath);
                    // Give Solid Edge a chance to do processing.
                    application.DoIdle();
                }
                else if (File.Exists(filePath + extension[1]))
                {
                    string fullPath = Path.GetFullPath(filePath + extension[1]);
                    // Open the document.
                    var document = (SolidEdgeFramework.SolidEdgeDocument)documents.Open(fullPath);
                    // Give Solid Edge a chance to do processing.
                    application.DoIdle();
                }
            }
        }

        private void syncItemCheckBox(object sender, RoutedEventArgs e)
        {          
            Parts part = getPartByWidget(sender);

            if (tabControl.SelectedIndex == 0)
            {
                foreach(ListBoxItem item in ((ListBox)((TabItem)tabControl.Items[1]).Content).Items)
                {
                    if (part.Equals((Parts)item.Tag))
                    {
                        Expander exp = (Expander)item.Content;
                        Grid grid = (Grid)exp.Header;
                        CheckBox checkbox = (CheckBox)grid.Children[0];
                        checkbox.IsChecked = ((CheckBox)sender).IsChecked;
                        break;
                    }
                }
                foreach(TabItem project in tabControl.Items)
                {
                    if (project.Tag == null) continue;
                    if (((ProjectofParts)project.Tag).name.Equals(part.project))
                    {
                        foreach(ListBoxItem item in ((ListBox)project.Content).Items)
                        {
                            if (part.Equals((Parts)item.Tag))
                            {
                                Expander exp = (Expander)item.Content;
                                Grid grid = (Grid)exp.Header;
                                CheckBox checkbox = (CheckBox)grid.Children[0];
                                checkbox.IsChecked = ((CheckBox)sender).IsChecked;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            else if (tabControl.SelectedIndex == 1)
            {
                foreach (TabItem project in tabControl.Items)
                {
                    if (project.Tag == null) continue;
                    if (((ProjectofParts)project.Tag).name.Equals(part.project))
                    {
                        foreach (ListBoxItem item in ((ListBox)project.Content).Items)
                        {
                            if (part.Equals((Parts)item.Tag))
                            {
                                Expander exp = (Expander)item.Content;
                                Grid grid = (Grid)exp.Header;
                                CheckBox checkbox = (CheckBox)grid.Children[0];
                                checkbox.IsChecked = ((CheckBox)sender).IsChecked;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            else
            {
                foreach (ListBoxItem item in ((ListBox)((TabItem)tabControl.Items[1]).Content).Items)
                {
                    if (part.Equals((Parts)item.Tag))
                    {
                        Expander exp = (Expander)item.Content;
                        Grid grid = (Grid)exp.Header;
                        CheckBox checkbox = (CheckBox)grid.Children[0];
                        checkbox.IsChecked = ((CheckBox)sender).IsChecked;
                        break;
                    }
                }
            }
        }
    }
}
