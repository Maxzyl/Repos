using DevExpress.Xpf.Core;
using MeasurementUI.View;
using ModelBaseLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModelBaseLib;

namespace MeasurementUI
{
    /// <summary>
    /// Interaction logic for ProjectMgr_Old.xaml
    /// </summary>
    public partial class ProjectMgr_Old : UserControl
    {
        StateFileManagerVM_Old fileVM = new StateFileManagerVM_Old();
        ListViewItem item;
        string id;
        public ProjectMgr_Old()
        {
            InitializeComponent();
            ChangeView("GridView");
            this.DataContext = fileVM;
            listView.View = listView.FindResource("iconView") as ViewBase;
            listView.ItemsSource = fileVM.FileterFileList;
            if(DataUtils.StaticInfo.MesMode.ToLower()=="true")
            {
                this.barButtonNew.IsVisible = false;
                this.barButtonDelete.IsVisible = false;
            }
        }
        void ChangeView(string str)
        {
            if (str == "详细视图")
            {
                listView.View = listView.FindResource("gridView") as ViewBase;
            }
            else if (str == "小图标")
            {
                listView.View = listView.FindResource("iconView") as ViewBase;
            }
            else if (str == "大图标")
            {
                listView.View = listView.FindResource("tileView") as ViewBase;
            }
        }

        private void barButtonView_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (barButtonView.Content.ToString() == "详细视图")
            {
                barButtonView.Content = "大视图";
                listView.View = listView.FindResource("tileView") as ViewBase;
                barButtonView.Glyph = new BitmapImage(new Uri(@"/MeasurementUi;component/Images/菜单.png", UriKind.Relative));
            }
            else if (barButtonView.Content.ToString() == "大视图")
            {
                barButtonView.Content = "详细视图";
                listView.View = listView.FindResource("gridView") as ViewBase;
                barButtonView.Glyph = new BitmapImage(new Uri(@"/MeasurementUi;component/Images/菜单 (2).png", UriKind.Relative));
            }
        }

        private void barButtonLoad_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            LoadFile();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            ChangeView(mi.Header.ToString());
        }

        private void loadMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LoadFile();
        }

        private void listView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenFile();
        }
        private void OpenFile()
        {
            if(DataUtils.StaticInfo.MesMode.ToLower()=="true")
            {
                if ((new ViewModelLocator()).MainWindow.StatusInfo.IsAdmin)
                {
                    object obj = listView.SelectedItem as StateFileModel;
                    if (obj != null)
                    {
                        StateFileModel stateFileModel = listView.SelectedItem as StateFileModel;
                        if (DataUtils.StaticInfo.ATEStatusFile.ToUpper() == "TRUE")
                        {
                            string material = stateFileModel.Name;
                            string process = (new ViewModelLocator()).MainWindow.StatusInfo.Process;
                            DataTable dtFile = DataUtils.Interface.GetFile("", process, material);
                            if (dtFile.Rows.Count >= 1)
                            {
                                DXMessageBox.Show("文件已审核不能编辑！", "提示");
                                return;
                            }
                        }
                        Window mainWindow = DevExpress.Xpf.Core.Native.LayoutHelper.FindLayoutOrVisualParentObject(this, (el) => { return el is Window; }) as Window;
                        System.Reflection.MethodInfo mif = mainWindow.GetType().GetMethod("ActiveItem", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        if (mif != null)
                        {
                            mif.Invoke(
                                    mainWindow, new object[] { string.Format(@"\MeasurementUI.dll;component/View/ConfigTestStep.xaml?PlanID={0}={1}", stateFileModel.ID, stateFileModel.Name), stateFileModel.Name, "文件" }
                                    );
                        }
                    }
                }
                else
                {
                    DXMessageBox.Show("没有修改配置文件权限！", "提示");
                }   
            }
            else
            {
                  object obj = listView.SelectedItem as StateFileModel;
                  if (obj != null)
                  {
                      StateFileModel stateFileModel = listView.SelectedItem as StateFileModel;
                      Window mainWindow = DevExpress.Xpf.Core.Native.LayoutHelper.FindLayoutOrVisualParentObject(this, (el) => { return el is Window; }) as Window;
                      System.Reflection.MethodInfo mif = mainWindow.GetType().GetMethod("ActiveItem", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                      if (mif != null)
                      {
                          mif.Invoke(
                                  mainWindow, new object[] { string.Format(@"\MeasurementUI.dll;component/View/ConfigTestStep.xaml?PlanID={0}={1}", stateFileModel.ID, stateFileModel.Name), stateFileModel.Name, "文件" }
                                  );
                      }
                  }
            }
        }

        private void LoadFile()
        {
            if (listView.SelectedItems.Count != 1)
            {
                DXMessageBox.Show("请选择一个方案，执行加载程序！", "加载程序", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            id = (listView.SelectedItem as StateFileModel).ID.ToString();           
            LoadFileFromID(id);
            CloseTestResultPanel();
        }

        //连接数据库
        private void LoadFileFromID(string id)
        {
            DataSet ds = null;
            if(DataUtils.StaticInfo.MesMode.ToLower()=="true")
            {
                ds = DataUtils.DB.GetDataSetFromSQL(string.Format("SELECT StateData,Material FROM SYS_TEST_PLAN WHERE FILEID='{0}'", id));
            }
            else
            {
                ds = DataUtils.DB.GetDataSetFromSQL(string.Format("SELECT StateData, FileName as Material FROM ATE_TEST_FILE WHERE FILEID='{0}'", id));
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                TestPlanVM MTSM = new ViewModelLocator().CurrentTestPlanVm;
                if (ds.Tables[0].Rows[0]["StateData"] != System.DBNull.Value)
                {
                    byte[] data = (byte[])ds.Tables[0].Rows[0]["StateData"];
                  //  TestPlanVM testPlan = new TestPlanVM();
                    TestPlan testPlan = new TestPlan();
                    object obj = Interface.DeSerializerStateModel(data, testPlan);
                    testPlan = obj as TestPlan;
                    if (testPlan != null)
                    {
                        MTSM.TestPlan = testPlan;
                    }
                }
                else
                {
                    MTSM.TestPlan = new TestPlan();
                }
                MTSM.DisplayName = ds.Tables[0].Rows[0]["Material"].ToString();
                MTSM.ApplyTestPlan();
                (new ViewModelLocator()).MainWindow.StatusInfo.OpenProject = ds.Tables[0].Rows[0]["Material"].ToString();
                (new ViewModelLocator()).MainWindow.StatusInfo.Material = MTSM.DisplayName;

                SaveStateFileToXml(id);
                foreach (StateFileModel sfModel in fileVM.CurrentFileList)
                {
                    if (sfModel.Name != MTSM.DisplayName)
                    {
                        sfModel.IsLoad = false;
                    }
                    else
                    {
                        sfModel.IsLoad = true;
                    }
                }
           }
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void txtBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                Search();
            }
        }
        private void Search()
        {
            string str = txtBoxSearch.Text.Trim();
            fileVM.Search(str); 
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TestPlanVM MTSM = new ViewModelLocator().CurrentTestPlanVm;
            if (fileVM.CurrentFileList!=null)
            {
                foreach (StateFileModel sfModel in fileVM.CurrentFileList)
                {
                    if (sfModel.Name != MTSM.DisplayName)
                    {
                        sfModel.IsLoad = false;
                    }
                    else
                    {
                        sfModel.IsLoad = true;
                    }
                }
            }
            //if ((new ViewModelLocator()).MainWindow.StatusInfo.IsLoad == false) return;
            //if (DXSplashScreen.IsActive) DXSplashScreen.Close();
            //DXSplashScreen.Show<LoadFile_SplashScreen>();
            //string currFilePath = AppDomain.CurrentDomain.BaseDirectory;
            //string fileName = currFilePath + "configfiles/StateFileID.txt";
            //if(System.IO.File.Exists(fileName))
            //{
            //    string str = File.ReadAllText(fileName);
            //    if(!string.IsNullOrWhiteSpace(str))
            //    {
            //        string id = str.Substring(0,str.Length-2);
            //        LoadFileFromID(id);
            //    }
            //}
            //if (DXSplashScreen.IsActive) DXSplashScreen.Close();
            //(new ViewModelLocator()).MainWindow.StatusInfo.IsLoad = false;
        }
        private void SaveStateFileToXml(string id)
        {
            string currFilePath = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = currFilePath + "configfiles/StateFileID.txt";
            if(System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }
            using( FileStream fs = new FileStream(fileName,FileMode.Create,FileAccess.Write))
            {
                 StreamWriter sw=new StreamWriter(fs);
                 sw.WriteLine(id);
                 sw.Close();
                 fs.Close();
            }
        }

        //连接数据库
        private void barButtondownload_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {   
            if(listView.SelectedItem ==null)
            {
                DXMessageBox.Show("请选择一个方案！", "导出文件", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DataSet ds =null;
            id = (listView.SelectedItem as StateFileModel).ID.ToString();
            if(DataUtils.StaticInfo.MesMode.ToLower()=="true")
            {
                ds = DataUtils.DB.GetDataSetFromSQL(string.Format("SELECT StateData,Material FROM SYS_TEST_PLAN WHERE FILEID='{0}'", id));
            }
            else
            {
                ds = DataUtils.DB.GetDataSetFromSQL(string.Format("SELECT StateData FROM ATE_TEST_FILE WHERE FILEID='{0}'", id));
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["StateData"] != System.DBNull.Value)
                {
                    byte[] data = (byte[])ds.Tables[0].Rows[0]["StateData"];
                    System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
                    saveFileDialog.Filter = "STA 文件|*.sta";
                    saveFileDialog.RestoreDirectory = true;
                    if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        System.IO.File.WriteAllBytes(saveFileDialog.FileName,data);
                    }
                }    
            }
        }

        private void barButtoUpload_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (listView.SelectedItem == null)
            {
                DXMessageBox.Show("请选择一个方案！", "导入文件", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            id = (listView.SelectedItem as StateFileModel).ID.ToString();
            string material = (listView.SelectedItem as StateFileModel).Name.ToString();
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "STA 文件|*.sta";
            if(openFileDialog.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {
                 if(!string.IsNullOrWhiteSpace(openFileDialog.FileName))
                 {
                     if(DataUtils.StaticInfo.MesMode.ToLower()=="true")
                     {
                         string processid = (new ViewModelLocator()).MainWindow.StatusInfo.Process;
                         byte[] data = System.IO.File.ReadAllBytes(openFileDialog.FileName);
                         //TestPlanVM testPlan = new TestPlanVM();
                         TestPlan testPlan = new TestPlan();
                         object obj = Interface.DeSerializerStateModel(data, testPlan);
                         if (obj == null)
                         {
                             return;
                         }
                         else
                         {
                             TestPlan tp = obj as TestPlan;
                             if (tp != null)
                             {
                                 tp.DisplayName = material;
                                 data = ViewModelBaseLib.Interface.SerializerStateModel(tp);
                             }
                         }
                         if (DataUtils.StaticInfo.ATEStatusFile.ToUpper() == "TRUE")
                         {
                             string str = DataUtils.Interface.UpLoadActivity(data, DataUtils.StaticInfo.ATEKind, material, processid, DataUtils.StaticInfo.LoginUser, material);
                             if (!str.ToUpper().Equals("OK"))
                             {
                                 DXMessageBox.Show(str, "提示");
                                 return;
                             }
                         }
                         string result = DataUtils.Interface.SaveStateModel(data, id);
                         if (!result.StartsWith("OK"))
                         {
                             DXMessageBox.Show(result.ToString());
                         }
                     }
                     else
                     {
                         byte[] data = System.IO.File.ReadAllBytes(openFileDialog.FileName);
                         TestPlan testPlan = new TestPlan();
                         testPlan.DisplayName = material;
                         object obj = Interface.DeSerializerStateModel(data, testPlan);
                         if (obj == null)
                         {
                             return;
                         }
                         else
                         {
                             TestPlan tp = obj as TestPlan;
                             if(tp!=null)
                             {
                                 tp.DisplayName = material;
                                 data = ViewModelBaseLib.Interface.SerializerStateModel(tp);
                             }
                         }
                         DataUtils.Interface.UpdateFileDB(data, id);
                     }
                 }
            }
        }

        private void txtBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            //fileVM.getCurrentFileData();
            //listView.ItemsSource = fileVM.FileterFileList;
        }

        private void barButtonNew_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            AddFileName add = new AddFileName();
            if (add.ShowDialog() == true)
            {
                if (!string.IsNullOrEmpty(add.txtName.Text))
                {
                    foreach (StateFileModel item in fileVM.CurrentFileList)
                    {
                        if (item.Name == add.txtName.Text)
                        {
                            DXMessageBox.Show("文件名 '" + add.txtName.Text + "' 已存在,请重新输入!", "系统提示");
                            return;
                        }
                    }
                    DataUtils.Interface.InsertFileDB(null, "N", add.txtName.Text, null, null, null, DataUtils.StaticInfo.LoginUser, DateTime.Now, "Y");
                    fileVM.getCurrentFileData();
                    listView.ItemsSource = fileVM.FileterFileList;
                }
            }
            add.Close();
        }

        private void barButtonDelete_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                int id = (listView.SelectedItem as StateFileModel).ID;
                string name = (listView.SelectedItem as StateFileModel).Name;
                MessageBoxResult response = DXMessageBox.Show("是否要删除 '" + name + "' 文件", "系统提示", System.Windows.MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (response == MessageBoxResult.OK)
                {
                    DataUtils.Interface.DeleteFileDB(Convert.ToString(id));
                    fileVM.getCurrentFileData();
                    listView.ItemsSource = fileVM.FileterFileList;
                }
            }
        }
        private void CloseTestResultPanel()
        {
            try
            {
                Window mainWindow = DevExpress.Xpf.Core.Native.LayoutHelper.FindLayoutOrVisualParentObject(this, (el) => { return el is Window; }) as Window;
                System.Reflection.MethodInfo mif = mainWindow.GetType().GetMethod("CloseDockItem", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (mif != null)
                {
                    mif.Invoke(
                            mainWindow, new object[] { new string[] { "测试结果" } }
                            );
                    mif.Invoke(
                            mainWindow, new object[] { new string[] { "本地设置和校准" } }
                            );
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error:" + ex.Message);
            }
        }
    }
}
