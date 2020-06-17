using System;
using System.Collections.Generic;
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
using DevExpress.Xpf.Grid;
using System.Data;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;
using System.ComponentModel;

namespace UserMaintenance
{
    /// <summary>
    /// 用户管理.xaml 的交互逻辑
    /// </summary>
    public partial class User : UserControl
    {

        public User()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                gridControl1.ClipboardCopyMode = ClipboardCopyMode.ExcludeHeader;
                GetDataFromDB();
            }
        }

        private void GetDataFromDB()
        {
            string sqlselect = @"
SELECT [ID],[USERID] 用户编号,[USERNAME] 用户名,'******' 密码,[EMAIL] 邮件,[PHONE] 电话,[DEPAT] 部门,[ENABLE] 是否有效,[LASTMODIFYUSER] 修改人员,[LASTMODIFYTIME] 修改时间, UGID=STUFF((SELECT ';'+CONVERT(VARCHAR, UGID) FROM 
(
SELECT A.*,C.ID UGID
FROM SYS_USER A 
LEFT JOIN SYS_ASSIGN_USER_USERGROUP B ON A.ID=B.SYS_USER_ID
LEFT JOIN SYS_USERGROUP C ON B.SYS_USERGROUP_ID=C.ID
)A WHERE A.ID=B.ID FOR XML PATH('')), 1, 1, '') 
FROM 
(
SELECT A.*,C.ID UGID
FROM SYS_USER A 
LEFT JOIN SYS_ASSIGN_USER_USERGROUP B ON A.ID=B.SYS_USER_ID
LEFT JOIN SYS_USERGROUP C ON B.SYS_USERGROUP_ID=C.ID
) B
GROUP BY [ID],[USERID] ,[USERNAME] ,[EMAIL] ,[PHONE] ,[DEPAT] ,[ENABLE] ,[LASTMODIFYUSER] ,[LASTMODIFYTIME] ORDER BY ID 
";
            DataSet ds = DataUtils.DB.GetDataSetFromSQL(sqlselect);
            gridControl1.ItemsSource = ds.Tables[0];
            gridControl1.Columns["ID"].Visible = false;
            gridControl1.Columns["密码"].Visible = false;
            gridControl1.Columns["UGID"].Visible = false;
            //gridControl1.Columns["参数"].CellStyle = (Style)gridControl1.Resources["customCellStyle"];
            //gridControl1.Columns["值"].CellStyle = (Style)gridControl1.Resources["customCellStyle"];
            gridControl1.Columns["ID"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
            gridControl1.Columns["密码"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
            gridControl1.Columns["UGID"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
            gridControl1.Columns["修改人员"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
            gridControl1.Columns["修改时间"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
            gridControl1.RefreshData();

            //string filterstr = Convert.ToString(this.Tag);
            //string[] filters = filterstr.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
            //if (filters.Count() == 2)
            //{
            //    if (gridControl1.Columns.GetColumnByFieldName(filters[0]) != null)
            //    {
            //        gridControl1.Columns[filters[0]].AutoFilterCondition = AutoFilterCondition.Like;
            //        gridControl1.Columns[filters[0]].AutoFilterValue = filters[1];
            //    }
            //}
            //LoadLayoutFromDB();
        }

        private void LoadLayoutFromDB()
        {
          //  string result = DataUtils.Interface.LoadUserLayoutByActivity(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.Name, this.GetType().FullName, DataUtils.StaticInfo.LoginUser, new List<DockLayoutManager>() { dockLayoutManager1, dockLayoutManager2 });
        }

        private void SaveLayoutToDB()
        {
           // string result = DataUtils.Interface.UpdataUserLayoutByActivity(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.Name, this.GetType().FullName, DataUtils.StaticInfo.LoginUser, new List<DockLayoutManager>() { dockLayoutManager1, dockLayoutManager2 });
        }

        private string PostDataToDB()
        {
            DataTable dt = (DataTable)gridControl1.ItemsSource;
            string strSucess = "";
            string strFail = "";
            string strMessage = "{0}" + Environment.NewLine + "{1}";
            foreach (DataRow dr in dt.Rows)
            {
                string result = "";
                if ((dr.RowState == DataRowState.Modified) || (dr.RowState == DataRowState.Added))
                {
                    result = DataUtils.DB.ExecProc("USER_UPDATE",
                                                    "@ACTION=" + "UPDATE",
                                                    "@ID=" + Convert.ToString(dr["ID"]),
                                                    "@USERID=" + Convert.ToString(dr["用户编号"]),
                                                    "@USERNAME=" + Convert.ToString(dr["用户名"]),
                                                    "@PWD=" + Convert.ToString(dr["密码"]),
                                                    "@EMAIL=" + Convert.ToString(dr["邮件"]),
                                                    "@PHONE=" + Convert.ToString(dr["电话"]),
                                                    "@DEPAT=" + Convert.ToString(dr["部门"]),
                                                    "@ENABLE=" + Convert.ToString(dr["是否有效"]),
                                                    "@UPDATEUSER=" + DataUtils.StaticInfo.LoginUser,
                                                    "@GROUPLIST="+Convert.ToString(dr["UGID"])
                                                    );

                }
                if (dr.RowState == DataRowState.Deleted)
                {
                    result = DataUtils.DB.ExecProc("USER_UPDATE",
                                                    "@ACTION=" + "DELETE",
                                                    "@ID=" + Convert.ToString(dr["ID", DataRowVersion.Original]),
                                                    "@USERID=" + Convert.ToString(dr["用户编号", DataRowVersion.Original]),
                                                    "@USERNAME=" + Convert.ToString(dr["用户名", DataRowVersion.Original]),
                                                    "@PWD=" + Convert.ToString(dr["密码", DataRowVersion.Original]),
                                                    "@EMAIL=" + Convert.ToString(dr["邮件", DataRowVersion.Original]),
                                                    "@PHONE=" + Convert.ToString(dr["电话", DataRowVersion.Original]),
                                                    "@DEPAT=" + Convert.ToString(dr["部门", DataRowVersion.Original]),
                                                    "@ENABLE=" + Convert.ToString(dr["是否有效", DataRowVersion.Original]),
                                                    "@UPDATEUSER=" + DataUtils.StaticInfo.LoginUser,
                                                    "@GROUPLIST=" + Convert.ToString(dr["UGID", DataRowVersion.Original])
                                                    );

                }
                if (!string.IsNullOrWhiteSpace(result))
                {
                    if (result.StartsWith("OK"))
                    {
                        if (string.IsNullOrWhiteSpace(strSucess)) strSucess = "成功:";
                        strSucess += Convert.ToString(dr["用户编号", (dr.RowState == DataRowState.Deleted) ? DataRowVersion.Original : DataRowVersion.Current]) + ";";
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(strFail)) strFail = "失败:";
                        strFail += Convert.ToString(dr["用户编号"]) + ";";
                    }
                }
            }
            return string.Format(strMessage, strSucess, strFail);
        }

        private void ClearInputData()
        {
            ebUserID.Text = "";
            ebUserName.Text = "";
            ebPwd.Password = "";
            ebPwdcfm.Password = "";
            ebEmail.Text = "";
            ebPhone.Text = "";
            ebDepat.Text = "";
            cbxEnable.IsChecked = false;
            ebUserID.Focus();
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                btnDelete_ItemClick(null, null);
            }
            if (e.Key == Key.F5)
            {
                btnRefresh_ItemClick(null, null);
            }
            if (e.Key == Key.F7)
            {
                btnClear_ItemClick(null, null);
            }
            if (e.Key == Key.F12)
            {
                btnSave_ItemClick(null, null);
            }
            if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.V))
            {
                DataUtils.STClipBoard.CopyFromClipBoard(gridControl1);
                //CopyFromClipBoard();
                e.Handled = true;
            }
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            GetDataFromDB();
        }

        private void btnSave_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            SaveLayoutToDB();
            string result = PostDataToDB();
            if (!string.IsNullOrWhiteSpace(result))
            {
                DXMessageBox.Show(result, "数据提交结果");
            }
            GetDataFromDB();
        }

        private void btnClear_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ClearInputData();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            foreach (int handle in gridControl1.GetSelectedRowHandles())
            {
                ((DataRowView)gridControl1.GetRow(handle)).Delete();
            }
        }

        private void btUpdate_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (gridControl1.GetSelectedRowHandles().Count() == 0) return;
            if (ebPwd.Password != ebPwdcfm.Password)
            {
                DXMessageBox.Show("密码不一致!");
                return;
            }
            int idx = gridControl1.GetSelectedRowHandles()[0];
            if (idx <= 0)
            {
                DataRow dr = ((DataTable)gridControl1.ItemsSource).NewRow();
                dr["用户编号"] = ebUserID.Text;
                dr["用户名"] = ebUserName.Text;
                dr["邮件"] = ebEmail.Text;
                dr["电话"] = ebPhone.Text;
                dr["部门"] = ebDepat.Text;
                dr["是否有效"] = cbxEnable.IsChecked.Value ? "Y" : "N";
                dr["密码"]= ebPwd.Password;
                dr["UGID"] = GetSelectListValue(lbeUserGroup);
                ((DataTable)gridControl1.ItemsSource).Rows.Add(dr);
            }
            else
            {
                ((DataRowView)gridControl1.GetRow(idx))["用户编号"] = ebUserID.Text;
                ((DataRowView)gridControl1.GetRow(idx))["用户名"] = ebUserName.Text;
                ((DataRowView)gridControl1.GetRow(idx))["邮件"] = ebEmail.Text;
                ((DataRowView)gridControl1.GetRow(idx))["电话"] = ebPhone.Text;
                ((DataRowView)gridControl1.GetRow(idx))["部门"] = ebDepat.Text;
                ((DataRowView)gridControl1.GetRow(idx))["是否有效"] = cbxEnable.IsChecked.Value ? "Y" : "N";
                ((DataRowView)gridControl1.GetRow(idx))["密码"] = ebPwd.Password;
                ((DataRowView)gridControl1.GetRow(idx))["UGID"] = GetSelectListValue(lbeUserGroup);
            }
        }

        private string GetSelectListValue(DevExpress.Xpf.Editors.ListBoxEdit lbe)
        {
            string result = "";
            foreach (object obj in ((List<object>)lbe.EditValue))
            {
                result += Convert.ToString(obj) + ";";
            }
            return result;
        }

        private void gridControl1_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            string SqlSelect = @"
SELECT A.*,ISNULL(CONVERT(VARCHAR, B.SYS_USER_ID),'N') CHECKED FROM SYS_USERGROUP A 
LEFT JOIN SYS_ASSIGN_USER_USERGROUP B ON A.ID =B.SYS_USERGROUP_ID AND B.SYS_USER_ID='{0}'
ORDER BY A.USERGROUPNAME
";
            string uid = "";
            if (e.NewItem != null)
            {
                uid = Convert.ToString(((DataRowView)e.NewItem)["ID"]);
                ebUserID.Text = Convert.ToString(((DataRowView)e.NewItem)["用户编号"]);
                ebUserName.Text = Convert.ToString(((DataRowView)e.NewItem)["用户名"]);
                ebPwd.Password = Convert.ToString(((DataRowView)e.NewItem)["密码"]);
                ebPwdcfm.Password = Convert.ToString(((DataRowView)e.NewItem)["密码"]);
                ebEmail.Text = Convert.ToString(((DataRowView)e.NewItem)["邮件"]);
                ebPhone.Text = Convert.ToString(((DataRowView)e.NewItem)["电话"]);
                ebDepat.Text = Convert.ToString(((DataRowView)e.NewItem)["部门"]);
                cbxEnable.IsChecked = Convert.ToString(((DataRowView)e.NewItem)["是否有效"]) == "Y";
            }
            else
            {
                uid = "";
                ebUserID.Text = "";
                ebUserName.Text = "";
                ebPwd.Password = "";
                ebPwdcfm.Password = "";
                ebEmail.Text = "";
                ebPhone.Text = "";
                ebDepat.Text = "";
                cbxEnable.IsChecked = false;
            }
            DataSet ds = DataUtils.DB.GetDataSetFromSQL(string.Format(SqlSelect, uid));
            List<UserGroupTemp> usergroupList = new List<UserGroupTemp>();
            List<object> selected = new List<object>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                usergroupList.Add(new UserGroupTemp(Convert.ToString(dr["USERGROUPNAME"]), Convert.ToInt32(dr["ID"])));
                if (Convert.ToString(dr["CHECKED"]).ToUpper() != "N")
                {
                    selected.Add(Convert.ToInt32(dr["ID"]));
                }
            }
            lbeUserGroup.ItemsSource = usergroupList;
            lbeUserGroup.DisplayMember = "USERGROUPNAME";
            lbeUserGroup.ValueMember = "ID";
            lbeUserGroup.EditValue = selected;
        }

        private void Grid9085_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            lbeUserGroup.Height = ((System.Windows.Controls.Grid)(sender)).RowDefinitions[0].ActualHeight;
        }
    }

    class UserGroupTemp
    {
        public string USERGROUPNAME { get; set; }
        public int ID { get; set; }
        public UserGroupTemp(string ugname, int id)
        {
            this.USERGROUPNAME = ugname;
            this.ID = id;
        }
    }

}
