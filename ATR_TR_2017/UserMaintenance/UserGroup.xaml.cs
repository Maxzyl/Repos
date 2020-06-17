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

namespace UserMaintenance
{
    /// <summary>
    /// 用户群组管理.xaml 的交互逻辑
    /// </summary>
    public partial class UserGroup : UserControl
    {

        public UserGroup()
        {
            InitializeComponent();
        }

        private void UserGroupControl_Loaded(object sender, RoutedEventArgs e)
        {
            gridControl1.ClipboardCopyMode = ClipboardCopyMode.ExcludeHeader;
            GetDataFromDB();
            LoadActivityFormDB();
        }

        private void GetDataFromDB()
        {
            string sqlselect = @"
SELECT [ID],[UserGroupName] 用户群组,[LASTMODIFYUSER] 修改人员,[LASTMODIFYTIME] 修改时间, 
Activitys=STUFF((SELECT ';'+CONVERT(VARCHAR, SYS_Activity_ID)+','+ISNULL( SYS_Activity_ITEM,'')+','+SecurityLevel FROM 
(
SELECT A.*,B.SYS_Activity_ID,B.SYS_Activity_ITEM,B.SecurityLevel
FROM SYS_USERGROUP A 
LEFT JOIN SYS_ASSIGN_USERGROUP_ACTIVITY B ON A.ID=B.SYS_USERGROUP_ID
)A WHERE A.ID=B.ID FOR XML PATH('')), 1, 1, '') 
FROM 
(
SELECT A.*,B.SYS_Activity_ID,B.SYS_Activity_ITEM,B.SecurityLevel
FROM SYS_USERGROUP A 
LEFT JOIN SYS_ASSIGN_USERGROUP_ACTIVITY B ON A.ID=B.SYS_USERGROUP_ID
) B
GROUP BY [ID],[UserGroupName],[LASTMODIFYUSER] ,[LASTMODIFYTIME] ORDER BY ID 

";
            DataSet ds = DataUtils.DB.GetDataSetFromSQL(sqlselect);
            gridControl1.ItemsSource = ds.Tables[0];
            gridControl1.Columns["ID"].Visible = false;
            gridControl1.Columns["Activitys"].Visible = false;
            gridControl1.Columns["ID"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
            gridControl1.Columns["Activitys"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
            gridControl1.Columns["修改人员"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
            gridControl1.Columns["修改时间"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
            LoadLayoutFromDB();
        }

        private void LoadLayoutFromDB()
        {
           // string result = DataUtils.Interface.LoadUserLayoutByActivity(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.Name, this.GetType().FullName, DataUtils.StaticInfo.LoginUser, new List<DockLayoutManager>() { dockLayoutManager1, dockLayoutManager2 });
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
                    result = DataUtils.DB.ExecProc("USERGROUP_UPDATE",
                                                    "@ACTION=" + "UPDATE",
                                                    "@ID=" + Convert.ToString(dr["ID"]),
                                                    "@USERGROUPNAME=" + Convert.ToString(dr["用户群组"]),
                                                    "@UPDATEUSER=" + DataUtils.StaticInfo.LoginUser,
                                                    "@ACTIVITYLIST=" + Convert.ToString(dr["Activitys"])
                                                    );

                }
                if (dr.RowState == DataRowState.Deleted)
                {
                    result = DataUtils.DB.ExecProc("USERGROUP_UPDATE",
                                                    "@ACTION=" + "DELETE",
                                                    "@ID=" + Convert.ToString(dr["ID", DataRowVersion.Original]),
                                                    "@USERGROUPNAME=" + Convert.ToString(dr["用户群组", DataRowVersion.Original]),
                                                    "@UPDATEUSER=" + DataUtils.StaticInfo.LoginUser,
                                                    "@ACTIVITYLIST=" + Convert.ToString(dr["Activitys", DataRowVersion.Original])
                                                    );

                }
                if (!string.IsNullOrWhiteSpace(result))
                {
                    if (result.StartsWith("OK"))
                    {
                        if (string.IsNullOrWhiteSpace(strSucess)) strSucess = "成功:";
                        strSucess += Convert.ToString(dr["用户群组", (dr.RowState == DataRowState.Deleted) ? DataRowVersion.Original : DataRowVersion.Current]) + ";";
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(strFail)) strFail = "失败:";
                        strFail += Convert.ToString(dr["用户群组"]) + ";";
                    }
                }
            }
            return string.Format(strMessage, strSucess, strFail);
        }

        private void ClearInputData()
        {
            ebUserGroupName.Text = "";
            ebUserGroupName.Focus();
        }

        private void UserGroupControl_PreviewKeyDown(object sender, KeyEventArgs e)
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
            int idx = gridControl1.GetSelectedRowHandles()[0];
            if (idx < 0)
            {
                DataRow dr = ((DataTable)gridControl1.ItemsSource).NewRow();
                dr["用户群组"] = ebUserGroupName.Text;
                dr["Activitys"] = GetSelectListValue(lbeActivity);
                ((DataTable)gridControl1.ItemsSource).Rows.Add(dr);
            }
            else
            {
                ((DataRowView)gridControl1.GetRow(idx))["用户群组"] = ebUserGroupName.Text;
                ((DataRowView)gridControl1.GetRow(idx))["Activitys"] = GetSelectListValue(lbeActivity);
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
            string itemlist = "";
            ebUserGroupName.Text = "";
            if (e.NewItem != null)
            {
                itemlist = Convert.ToString(((DataRowView)e.NewItem)["Activitys"]);
                ebUserGroupName.Text = Convert.ToString(((DataRowView)e.NewItem)["用户群组"]);
            }
            List<object> selected = new List<object>();
            foreach (string item in itemlist.Split(new char[] { ';' }))
            {
                selected.Add(item);
            }
            lbeActivity.EditValue = selected;
        }

        private void lbeActivity_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            //
        }

        private void Grid9085_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            lbeActivity.Height = ((System.Windows.Controls.Grid)(sender)).RowDefinitions[0].ActualHeight;
        }

        private void LoadActivityFormDB()
        {
            string SQLStr = @"
SELECT A.ID,A.ACTIVITYNAME,A.PATH,A.ACTIVITYGROUP,A.ISACTIVITY,A.FILESTREAM,A.FILETIMETAG ,A.LASTMODIFYTIME,A.LASTMODIFYUSER
FROM SYS_ACTIVITY A LEFT JOIN SYS_ACTIVITYGROUP B ON A.ACTIVITYGROUP=B.ACTIVITYGROUP
WHERE A.ISACTIVITY='Y'
ORDER BY B.ACTIVITYGROUPSEQ,A.ACTIVITYSEQ
";
            DataSet dsActivity = DataUtils.DB.GetDataSetFromSQL(string.Format(SQLStr));
            string fileinfo = "{0};{1}.Main:{2}";
            List<ACTIVITY> ActivityList = new List<ACTIVITY>();
            foreach (DataRow dr in dsActivity.Tables[0].Rows)
            {
                string fileName = DataUtils.StaticInfo.ExecutePath + string.Format(@"\{0}\{1}", dr["PATH"], dr["ACTIVITYNAME"]);
                string fileNamespace = System.IO.Path.GetFileNameWithoutExtension(fileName);
                string urisitems = string.Format(fileinfo, fileName, fileNamespace, "ItemNames");
                //string sitems = (string)DataUtils.CommUtils.GetPropertyFromUrl(urisitems);
                //string[] itemList = sitems.Split(new char[] { ';' });
                //foreach (string item in itemList)
                //{
                //    ActivityList.Add(new ACTIVITY(Convert.ToString(dr["ID"]), item, "R"));
                //    ActivityList.Add(new ACTIVITY(Convert.ToString(dr["ID"]), item, "W"));
                //}
            }
            lbeActivity.ItemsSource = ActivityList;
            lbeActivity.DisplayMember = "DisplayItem";
            lbeActivity.ValueMember = "ValueItem";
        }

    }

    class ACTIVITY
    {
        public string ID { get; set; }
        public string ITEM { get; set; }
        public string SECURITYLEVEL { get; set; }
        public string DisplayItem
        {
            get
            {
                return ITEM + "(" + SECURITYLEVEL + ")";
            }
        }
        public string ValueItem
        {
            get
            {
                return ID + "," + ITEM + "," + SECURITYLEVEL;
            }
        }

        public ACTIVITY(string id, string item, string securitylevel)
        {
            this.ID = id;
            this.ITEM = item;
            this.SECURITYLEVEL = securitylevel;
        }
    }

}
