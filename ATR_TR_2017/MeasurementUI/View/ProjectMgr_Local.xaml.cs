using DevExpress.Xpf.Core;
using ModelBaseLib;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ProjectMgr_Local.xaml
    /// </summary>
    public partial class ProjectMgr_Local : UserControl
    {
        string strFile = "";
        public ProjectMgr_Local()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openDialog = new System.Windows.Forms.OpenFileDialog();
            openDialog.Filter = "STA 文件|*.sta";
            openDialog.RestoreDirectory = true;
            if (openDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = openDialog.FileName;
                strFile = openDialog.SafeFileName.Substring(0, openDialog.SafeFileName.Length - 4);
                txtFileName.Text = fileName;
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveDialog = new System.Windows.Forms.SaveFileDialog();
            saveDialog.Filter = "STA 文件|*.sta";
            saveDialog.RestoreDirectory = true;
            if(saveDialog.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {
                   System.IO.File.WriteAllText(saveDialog.FileName,"");
                   string fileName = saveDialog.FileName;
                   txtFileName.Text = fileName;
                   strFile = saveDialog.FileName.Substring(saveDialog.FileName.LastIndexOf("\\") + 1);
                   strFile = strFile.Substring(0,strFile.Length-4);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            OpenFile();
            if (string.IsNullOrWhiteSpace(txtFileName.Text))
            {
                DXMessageBox.Show("请选择一个方案，进行编辑！", "编辑程序", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Window mainWindow = DevExpress.Xpf.Core.Native.LayoutHelper.FindLayoutOrVisualParentObject(this, (el) => { return el is Window; }) as Window;
            System.Reflection.MethodInfo mif = mainWindow.GetType().GetMethod("ActiveItem", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (mif != null)
            {
                mif.Invoke(
                        mainWindow, new object[] { string.Format(@"\MeasurementUI.dll;component/View/ConfigTestStep.xaml?PlanID={0}={1}", txtFileName.Text, strFile), strFile, "文件" }
                        );
            }
        }

        private void LoadFile()
        {
            if (!string.IsNullOrWhiteSpace(txtFileName.Text))
            {
                TestPlanVM MTSM = new ViewModelLocator().CurrentTestPlanVm;
                MTSM.DisplayName = strFile;
                byte[] statefile = System.IO.File.ReadAllBytes(txtFileName.Text);
                
                if (statefile != null && statefile.Length > 0)
                {
                    TestPlan testPlan = new TestPlan();
                    object obj = Interface.DeSerializerStateModel(statefile, testPlan);
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
                MTSM.DisplayName = strFile;
                MTSM.ApplyTestPlan();
                (new ViewModelLocator()).MainWindow.StatusInfo.OpenProject = strFile;
                MTSM.DisplayName = strFile;
                CloseTestResultPanel();
            }
            else
            {
                DXMessageBox.Show("请选择一个方案，执行加载程序！", "加载程序", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFile();
            LoadFile();
        }

        private void OpenFile()
        {
            if (string.IsNullOrWhiteSpace(txtFileName.Text))
            {
                System.Windows.Forms.OpenFileDialog openDialog = new System.Windows.Forms.OpenFileDialog();
                openDialog.Filter = "STA 文件|*.sta";
                openDialog.RestoreDirectory = true;
                if (openDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string fileName = openDialog.FileName;
                    strFile = openDialog.SafeFileName.Substring(0, openDialog.SafeFileName.Length - 4);
                    txtFileName.Text = fileName;
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
                            mainWindow, new object[] {new string[]{"测试结果"}}
                            );
                    mif.Invoke(
                            mainWindow, new object[] { new string[] { "本地设置和校准" } }
                            );
                }
            }
            catch(Exception ex)
            {
                 throw new Exception("error:" + ex.Message);
            }
        }

    }
}
