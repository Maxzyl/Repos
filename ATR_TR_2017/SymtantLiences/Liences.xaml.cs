using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SymtantLiences
{
    /// <summary>
    /// Interaction logic for Liences.xaml
    /// </summary>
    public partial class Liences : DXWindow
    {
        public Liences()
        {
            InitializeComponent();
            Refresh();
        }
        public string sql = @"
                        SELECT A.ID,CLIENTHOST 主机名,A.CLIENTIP IP地址簇,A.CLIENTINFO 相关信息,A.PERMISSION 是否可用,A.ClientKind 授权类型,'N' 是否有效,
                                A.CLIENTSTRING 原始码,A.LICENSESTRING 授权码, ISNULL(B.USERNAME, A.LASTLOGINUSERID) 最后登录人员,CONVERT(VARCHAR,A.LASTLOGINTIME,120) 最后登录时间 
                                FROM SYS_CLIENTHOST A LEFT JOIN SYS_USER B ON A.LASTLOGINUSERID=B.USERID
                                ORDER BY A.ID;
                                SELECT CONVERT(VARCHAR,GETDATE(),120) NOW";
        private void Refresh()
        {
            DataSet ds1 = DataUtils.DB.GetDataSetFromSQL(string.Format(sql));
            DateTime Now = DateTime.ParseExact(Convert.ToString(ds1.Tables[1].Rows[0]["NOW"]), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
            int CheckedCount = 0;
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                string[] strLicense = Convert.ToString(dr["授权码"]).Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                if (strLicense.Count() >= 2)
                {
                    string[] datetimes = strLicense[1].Split(new string[] { " -- " }, StringSplitOptions.None);
                    if (datetimes.Count() >= 2)
                    {
                        DateTime ValiedBegin = DateTime.ParseExact(datetimes[0], "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                        DateTime ValiedEnd = DateTime.ParseExact(datetimes[1], "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                        if ((ValiedBegin <= Now) && (ValiedEnd >= Now))
                        {
                            dr["是否有效"] = "Y";
                            CheckedCount++;
                        }
                    }
                }
            }
            gridControl1.ItemsSource = ds1.Tables[0];
            
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            int i = gridControl1.GetSelectedRowHandles()[0];
            Refresh();
            tableView.FocusedRowHandle = i;
            gridControl1.SelectedItem = gridControl1.GetRow(i);
            
        }

        private void btnExport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = ((DataTable)gridControl1.ItemsSource).Clone();
            dsResult.Tables.Add(dtResult);

            foreach (int handle in gridControl1.GetSelectedRowHandles())
            {
                DataRow dr = ((DataRowView)gridControl1.GetRow(handle)).Row;
                dtResult.ImportRow(dr);
            }
            if (DataUtils.UIPackage.ExportExcelByNPOI(dsResult, new string[] { "License" }) == 0)
            {
                DXMessageBox.Show("导出Excel文件成功！");
            }
        }

        private void btnImport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "ExcelFile(*.xlsx;*.xls)|*.xlsx;*.xls";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataTable dtImport = DataUtils.UIPackage.XlSToDataTable(ofd.FileName, "License", "License");
                DataTable dtAll = ((DataTable)gridControl1.ItemsSource);
                foreach (DataRow dr in dtImport.Rows)
                {
                    string clientstr = Convert.ToString(dr["原始码"]);
                    string licensestr = Convert.ToString(dr["授权码"]);
                    string canbeuse = Convert.ToString(dr["是否可用"]);
                    if (!(string.IsNullOrEmpty(clientstr) || string.IsNullOrEmpty(licensestr)))
                    {
                        DataRow[] drs = dtAll.Select("原始码='" + clientstr + "'");
                        if (drs.Count() > 0)
                        {
                            int index = licensestr.IndexOf(Environment.NewLine);
                            string Sqlstr = string.Format("update SYS_CLIENTHOST set LICENSESTRING='{1}',Permission='{2}' where CLIENTSTRING='{0}'", clientstr, licensestr, canbeuse);
                            DataUtils.DB.GetDataSetFromSQL(Sqlstr);
                        }
                        else
                        {
                            string Sqlstr = "insert into SYS_CLIENTHOST (ClientHost,ClientIP,ClientString,ClientInfo,LicenseString,Permission) values ('{0}','{1}','{2}','{3}','{4}','{5}')";
                            DataUtils.DB.GetDataSetFromSQL(string.Format(Sqlstr, dr["主机名"], dr["IP地址簇"], dr["原始码"], dr["相关信息"], dr["授权码"], dr["是否可用"]));
                        }
                    }
                }
            }
            Refresh();
        }

        private void DXWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = DXMessageBox.Show("是否退出？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void btnLiencse_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.symtant.com/cust_Login.aspx");
        }
    }
}
