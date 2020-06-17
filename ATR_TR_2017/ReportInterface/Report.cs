using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ReportInterface
{
    public class ReportPackage : MarshalByRefObject
    {
        public string TitleName { get; set; }//表头名称
        public string ClientName { get; set; }//客户名称
        public string Result { get; set; }//測試結果

        public DataTable dt { get; set; }//封面内容

        public List<ReportItems> ReportItems { get; set; }

        public ReportPackage()
        {
            dt = new DataTable();
            ReportItems = new List<ReportItems>();
        }

        public void Dispose()
        {
            ReportItems.Clear();
            GC.ReRegisterForFinalize(this);
        }
    }

    public class ReportItems : MarshalByRefObject
    {
        public List<ImageItem> image { get; set; }
        public List<DataTableItem> dt { get; set; }
        public ReportItems(List<ImageItem> image, List<DataTableItem> dt)
        {
            this.dt = dt;
            this.image = image;
        }
    }

    public class DataTableItem : MarshalByRefObject
    {
        public DataTable dt { get; set; }
        public string name { get; set; }
        public DataTableItem(DataTable dt,string name)
        {
            this.name = name;
            this.dt = dt;
        }

    }

    public class ImageItem : MarshalByRefObject
    {
        public Image image { get; set; }
        public string name { get; set; }
        public ImageItem(Image image, string name)
        {
            this.image = image;
            this.name = name;
        }
    }
}
