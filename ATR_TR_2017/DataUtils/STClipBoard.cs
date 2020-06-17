using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Xpf.Grid;
using System.Windows;
using System.Data;

namespace DataUtils
{
    public class STClipBoard
    {
        public static void CopyFromClipBoard(GridControl gc)
        {
            if (gc.GetSelectedRowHandles().Count() == 0) return;
            int idx = gc.GetSelectedRowHandles()[0];
            string str = Clipboard.GetText();
            string[] strlist = str.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in strlist)
            {
                string[] cols = item.Split(new char[] { (char)09 });
                int colindex = 0;
                DataRowView drv = (DataRowView)gc.GetRow(idx);
                if (drv == null)
                {
                    DataRow dr = ((DataTable)gc.ItemsSource).NewRow();
                    foreach (GridColumn gcol in gc.Columns)
                    {
                        if ((gcol.Visible == true) && (gcol.AllowEditing != DevExpress.Utils.DefaultBoolean.False))
                        { dr[gcol.FieldName] = (colindex < (cols.Length - 1)) ? cols[colindex++] : ""; }
                    }
                    ((DataTable)gc.ItemsSource).Rows.Add(dr);
                }
                else
                {
                    foreach (GridColumn gcol in gc.Columns)
                    {
                        if ((gcol.Visible == true) && (gcol.AllowEditing != DevExpress.Utils.DefaultBoolean.False))
                        { drv[gcol.FieldName] = (colindex < (cols.Length - 1)) ? cols[colindex++] : ""; }
                    }
                }
                idx++;
            }
        }
    }
}
