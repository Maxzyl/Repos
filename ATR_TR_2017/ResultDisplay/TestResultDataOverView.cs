using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace TestResultMarkerDip
{
    public class TestResultDataOverView
    {
        public TestResultDataOverView()
        {
            TestResultTable = new DataTable("ResultTable");
            DetailTestResultTable = new DataTable("DetailTable");
            TRDetailTestResultTable = new DataTable("TRTable");
            TestResultTable.Columns.Add(new DataColumn() { ColumnName = "全选", DataType = typeof(bool)});
            TestResultTable.Columns.Add(new DataColumn() { ColumnName = "IndexStr", DataType = typeof(string)});
            TestResultTable.Columns.Add(new DataColumn() { ColumnName = "SN", DataType = typeof(string) });
            TestResultTable.Columns.Add(new DataColumn() { ColumnName = "类型名",DataType=typeof(string) });
            TestResultTable.Columns.Add(new DataColumn() { ColumnName = "门限类型", DataType = typeof(string) });
            TestResultTable.Columns.Add(new DataColumn() { ColumnName = "测试项名称", DataType = typeof(string) });
            TestResultTable.Columns.Add(new DataColumn() { ColumnName = "端口", DataType = typeof(string) });
            TestResultTable.Columns.Add(new DataColumn() { ColumnName = "测试条件(XDesc)", DataType = typeof(string) });
            TestResultTable.Columns.Add(new DataColumn() { ColumnName = "门限描述", DataType = typeof(string) });
            TestResultTable.Columns.Add(new DataColumn() { ColumnName = "X", DataType =  typeof(string)});
            TestResultTable.Columns.Add(new DataColumn() { ColumnName = "测试数据", DataType = typeof(string) });
            TestResultTable.Columns.Add(new DataColumn() { ColumnName = "PassFail", DataType = typeof(string) });

            DetailTestResultTable.Columns.Add(new DataColumn() { ColumnName = "类型名", DataType = typeof(string) });
            DetailTestResultTable.Columns.Add(new DataColumn() { ColumnName = "门限类型", DataType = typeof(string) });
            DetailTestResultTable.Columns.Add(new DataColumn() { ColumnName = "测试项名称", DataType = typeof(string) });
            DetailTestResultTable.Columns.Add(new DataColumn() { ColumnName = "端口", DataType = typeof(string) });
            DetailTestResultTable.Columns.Add(new DataColumn() { ColumnName = "测试条件(XDesc)", DataType = typeof(string) });
            DetailTestResultTable.Columns.Add(new DataColumn() { ColumnName = "门限描述", DataType = typeof(string) });
            DetailTestResultTable.Columns.Add(new DataColumn() { ColumnName = "X", DataType = typeof(string) });
            DetailTestResultTable.Columns.Add(new DataColumn() { ColumnName = "Y", DataType = typeof(string) });
            DetailTestResultTable.Columns.Add(new DataColumn() { ColumnName = "PassFail", DataType = typeof(string) });

            TRDetailTestResultTable.Columns.Add(new DataColumn() { ColumnName = "类型名", DataType = typeof(string) });
            TRDetailTestResultTable.Columns.Add(new DataColumn() { ColumnName = "门限类型", DataType = typeof(string) });
            TRDetailTestResultTable.Columns.Add(new DataColumn() { ColumnName = "测试项名称", DataType = typeof(string) });
            TRDetailTestResultTable.Columns.Add(new DataColumn() { ColumnName = "端口", DataType = typeof(string) });
            TRDetailTestResultTable.Columns.Add(new DataColumn() { ColumnName = "测试条件(XDesc)", DataType = typeof(string) });
            TRDetailTestResultTable.Columns.Add(new DataColumn() { ColumnName = "门限描述", DataType = typeof(string) });
            TRDetailTestResultTable.Columns.Add(new DataColumn() {ColumnName = "Att", DataType = typeof(int)});
            TRDetailTestResultTable.Columns.Add(new DataColumn() {ColumnName = "Phase", DataType = typeof(int)});
            TRDetailTestResultTable.Columns.Add(new DataColumn() {ColumnName = "Freq", DataType = typeof(string)});
            TRDetailTestResultTable.Columns.Add(new DataColumn() {ColumnName = "测试数据", DataType = typeof(string)});
            TRDetailTestResultTable.Columns.Add(new DataColumn() {ColumnName = "PassFail", DataType = typeof(string)});

        }
        public DataTable TestResultTable { get; set; }
        public DataTable DetailTestResultTable { get; set; }
        public DataTable TRDetailTestResultTable { get; set; }
    }
}
