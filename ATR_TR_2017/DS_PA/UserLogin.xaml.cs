using DevExpress.Xpf.Core;
using ModelBaseLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
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

namespace ATS_TR
{
    /// <summary>
    /// Interaction logic for UserLogin.xaml
    /// </summary>
    public partial class UserLogin : Window
    {
        public bool IsAdminUser { get; set; }
        public string Terminal { get; set; }
        public string Process { get; set; }
        public string Token { get; set; }
        private DataTable dt = new DataTable();
        public UserLogin()
        {
            InitializeComponent();
            InitialData();
        }
        private void InitialData()
        {
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            tbRevision.Text = "Revision " + Convert.ToString(fvi.ProductMinorPart);
            tbProduct.Text = Convert.ToString(fvi.ProductName);
            Footer_Text.Text = Convert.ToString(fvi.LegalCopyright);

            string currentFilePath = Environment.CurrentDirectory;
            string fileName = currentFilePath + "/configfiles/UserLoginInfo.xml";
            if (System.IO.File.Exists(fileName))
            {
                DataUtils.LastUserInfo info = (DataUtils.LastUserInfo)DataUtils.CommUtils.DeserializerData(typeof(DataUtils.LastUserInfo), fileName);
                txtUserID.Text = info.LastUserName;
                pxtPwd.Text = info.LastUserPassword;
                Process = info.Process;
                Terminal = info.Terminal;
                processID.ToolTip = Process;
                (new ViewModelLocator()).MainWindow.StatusInfo.IsLocal = info.IsLocal;
                GeneTestSetup.Instance.IsLocal = info.IsLocal;
                if (!(new ViewModelLocator()).MainWindow.StatusInfo.IsLocal)
                {
                    try
                    {
                        updateTerminal();
                    }
                    catch (Exception ex)
                    {
                        DXMessageBox.Show(ex.ToString());
                        (new ViewModelLocator()).MainWindow.StatusInfo.IsLocal = true;
                        GeneTestSetup.Instance.IsLocal = true;
                        DataUtils.StaticInfo.LocalModel = "LOCAL";
                    }
                }
                else
                {
                    DataUtils.StaticInfo.LocalModel = "LOCAL";
                }
                foreach (DataRow dr in dt.Rows)
                {
                    
                    string str = Convert.ToString(dr["TerminalID"]);
                    if (str == Terminal)
                    {
                        cmbTerminal.Text = info.TerminalName; ;
                        processID.ToolTip = Process;
                        break;
                    }
                    else
                    {
                        cmbTerminal.SelectedIndex = -1;
                        processID.ToolTip = "";
                    }
                }
                if (info.IsSave == true)
                {
                    IsSave.IsChecked = true;
                }
                else
                {
                    IsSave.IsChecked = false;
                }
                button1.Focus();
            }
            else
            {
                (new ViewModelLocator()).MainWindow.StatusInfo.IsLocal = true;
                GeneTestSetup.Instance.IsLocal = true;
                DataUtils.StaticInfo.LocalModel = "LOCAL";
                txtUserID.Focus();
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DXSplashScreen.IsActive) DXSplashScreen.Close();
        }

        private void grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void txtUserID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                button1_Click(null, null);
            }
        }

        private void pxtPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                button1_Click(null, null);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if ((new ViewModelLocator()).MainWindow.StatusInfo.IsLocal == true)
            {
                string localClientResult = Symtant.SymtantSecurityClient.CheckPermissionLocal("TaFang");
                if (!localClientResult.ToUpper().StartsWith("OK"))
                {
                    string str = Symtant.SymtantSecurityClient.GetENVStr();
                    SymtantLiences.LoadLiences loadLiences = new SymtantLiences.LoadLiences();
                    loadLiences.lblMsg.Content = localClientResult;
                    loadLiences.txtLiences.Text = str;
                    loadLiences.ShowDialog();
                    return;
                }
                this.DialogResult = true;
                SaveUserLoginInfo();
                return;
            }
            if (DataUtils.StaticInfo.MesMode.ToLower() == "true")
            {
                if (!string.IsNullOrWhiteSpace(cmbTerminal.Text))
                {
                    string clientResult = Symtant.SymtantSecurityClient.CheckPermission("TaFang", txtUserID.Text.Trim());
                    if (!clientResult.ToUpper().StartsWith("OK"))
                    {
                        MessageBoxResult response = DXMessageBox.Show(clientResult + "\r\n 是否需要授权？", "提示", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (response == MessageBoxResult.Yes)
                        {
                            SymtantLiences.Liences liences = new SymtantLiences.Liences();
                            liences.ShowDialog();
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }
                    ServiceInterface.UserInfo userInfo = ServiceInterface.DataService.CheckUser(txtUserID.Text.Trim(), pxtPwd.Password, "ATEPA", Convert.ToString(cmbTerminal.SelectedValue));
                    if (userInfo.TOKEN != null)
                    {
                        this.DialogResult = true;
                        //给token赋值
                        Token = userInfo.TOKEN;

                        DataUtils.StaticInfo.LoginUser = txtUserID.Text.Trim();
                        //DataUtils.StaticInfo.Process=
                        string TRES = "";
                        DataSet ds = DataUtils.DB.QueryProc("USER_CHECKUSERGROUP", ref TRES, "@UserID=" + DataUtils.StaticInfo.LoginUser);
                        if (TRES.Substring(0, 2) == "OK")
                        {
                            if (ds.Tables[0].Select("UserGroupName='ADMIN'").Length > 0)
                            {
                                IsAdminUser = true;
                            }
                            else
                            {
                                IsAdminUser = false;
                            }
                        }
                    }
                    else
                    {
                        DXMessageBox.Show("请检查用户名或密码是否正确,是否有该工位操作权限！", "认证");
                        pxtPwd.SelectAll();
                        pxtPwd.Focus();
                    }
                }
                else
                {
                    DXMessageBox.Show("请选择工序！", "提示");
                }
            }
            SaveUserLoginInfo();
            if (DataUtils.StaticInfo.MesMode.ToLower() != "true")
            {
                this.DialogResult = true;
            }
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btn_Min_Click(object sender, RoutedEventArgs e)
        {
            (this.Owner as Window).WindowState = WindowState.Minimized;
        }

        private void SaveUserLoginInfo()
        {
            string currentFilePath = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo directory = new DirectoryInfo(currentFilePath + "/configfiles");
            if (!directory.Exists)
            directory.Create();
            string fileName = currentFilePath + "/configfiles/UserLoginInfo.xml";
            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }
            DataUtils.LastUserInfo info = new DataUtils.LastUserInfo();
            info.LastUserName = txtUserID.Text;
            if (IsSave.IsChecked == true)
            {
                info.IsSave = true;
                info.LastUserPassword = pxtPwd.Text;
                info.Process = Process;
                info.Terminal = Terminal;
                info.TerminalName = cmbTerminal.Text;
                info.IsLocal = (new ViewModelLocator()).MainWindow.StatusInfo.IsLocal;
            }
            else
            {
                info.IsSave = false;
                info.LastUserPassword = "";
                info.Process = "";
                info.Terminal = "";
                info.IsLocal = true;
            }
            string result = DataUtils.CommUtils.SerializeData(typeof(DataUtils.LastUserInfo), info, fileName);
        }

        private void cmbTerminal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string str = cmbTerminal.SelectedValue.ToString();
            DataTable dt = DataUtils.Interface.GetProcessFromTerminal(str);
            if (dt.Rows.Count > 0)
            {
                Terminal = str;
                Process = dt.Rows[0][0].ToString();
                processID.ToolTip = Process.ToString();
            }
        }

        private void btnLocalSetUp_Click(object sender, RoutedEventArgs e)
        {
            UC_LocalSet us = new UC_LocalSet((new ViewModelLocator()).MainWindow.StatusInfo.IsLocal);
            us.Update += updateTerminal;
            us.ShowDialog();
        }

        private void updateTerminal()
        {
            dt = DataUtils.Interface.GetTerminalInfo();
            cmbTerminal.ItemsSource = dt.AsDataView();
            cmbTerminal.DisplayMemberPath = "TerminalName";
            cmbTerminal.SelectedValuePath = "TerminalID";
        }
    }
}
