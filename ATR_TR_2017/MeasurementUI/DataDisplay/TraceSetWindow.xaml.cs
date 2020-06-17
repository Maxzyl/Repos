using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Popups;
using DevExpress.Xpf.Editors.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace MeasurementUI
{
    /// <summary>
    /// Interaction logic for TraceSetWindow.xaml
    /// </summary>
    public partial class TraceSetWindow : Window
    {
        public UpdateTraceData updateTraceData;
        public DataTable dt;
        public List<TraceEditValue> TraceEditValuelist = new List<TraceEditValue>();
        private List<TraceEditValue> EditTraceValuelist = new List<TraceEditValue>();
        private string traceName;
        public TraceSetWindow(DataTable dt,List<TraceEditValue> TraceEditValuelist,string str)
        {
            InitializeComponent();
            this.WindowStyle = System.Windows.WindowStyle.ToolWindow;
            this.dt = dt;
            this.TraceEditValuelist = TraceEditValuelist;
            this.traceName = str;
            InitialData();
        }
        private void InitialData()
        {
            List<string> Axislist = new List<string>();
             foreach(DataColumn column in dt.Columns)
             {   
                 if(column.ColumnName.ToUpper() !="Y")
                 {
                     Axislist.Add(column.ColumnName);
                 }
             }
             AxisComboBox.ItemsSource = Axislist;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
             if(updateTraceData !=null)
             {   
                 if(AxisComboBox.SelectedItem !=null)
                 {
                     updateTraceData.Invoke(EditTraceValuelist, AxisComboBox.SelectedItem.ToString(),traceName);
                 }
             }
        }

        private void AxisComboBox_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            Allpanel.Children.Clear();
            foreach(DataColumn column in dt.Columns)
            {
                if(column.ColumnName.ToUpper() !="Y" && column.ColumnName.ToString() !=AxisComboBox.SelectedItem.ToString())
                {
                    TraceEditValue traceEditValue = new TraceEditValue() { DisplayName=column.ColumnName};
                    StackPanel panel = new StackPanel() { Orientation=Orientation.Vertical};
                    Label lable = new Label() {Content=column.ColumnName };
                    ComboBoxEdit comboBoxEdit = new ComboBoxEdit() {Width=150,Height=25,HorizontalAlignment=HorizontalAlignment.Left,
                    VerticalAlignment=VerticalAlignment.Center,PopupFooterButtons=PopupFooterButtons.None,ImmediatePopup=true,Tag=column.ColumnName.ToString()};
                    comboBoxEdit.StyleSettings = new CheckedComboBoxStyleSettings();
                    comboBoxEdit.PopupClosing += comb_PopupClosing;
                    comboBoxEdit.PopupOpening += comb_PopupOpening;
                    comboBoxEdit.EditValueChanged += comb_EditValueChanged;
                    foreach(DataRow row in dt.Rows)
                    {
                        if (!traceEditValue.EditValuelist.Contains((row[column.ColumnName])))
                        {
                            traceEditValue.EditValuelist.Add((row[column.ColumnName]));
                        }
                    }
                    comboBoxEdit.ItemsSource=traceEditValue.EditValuelist;
                    panel.Children.Add(lable);
                    panel.Children.Add(comboBoxEdit);
                    Allpanel.Children.Add(panel);
                }
            }
        }
        private void comb_PopupClosing(object sender, DevExpress.Xpf.Editors.ClosePopupEventArgs e)
        {
            ComboBoxEdit comboBoxEdit = sender as ComboBoxEdit;
            var popup = (sender as ComboBoxEdit).GetPopup();
            if (popup != null)
            {
                var list = (PopupListBox)LayoutHelper.FindElementByType(popup.PopupContent as FrameworkElement, typeof(PopupListBox));
                (sender as ComboBoxEdit).EditValue = list.SelectedItems.Cast<object>().ToList();            
            }
        }
        private void comb_PopupOpening(object sender, DevExpress.Xpf.Editors.OpenPopupEventArgs e)
        {
            ComboBoxEdit comboBoxEdit = sender as ComboBoxEdit;
            List<double> listitem = comboBoxEdit.EditValue as List<double>;
        }
        private void comb_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            List<object> listitem = (sender as ComboBoxEdit).EditValue as List<object>;
            if (listitem == null) return;
            for (int i = 0; i < listitem.Count(); i++)
            {
                if (listitem[i].GetType() == typeof(DevExpress.Xpf.Editors.SelectAllItem))
                {
                    listitem.RemoveAt(i);
                }
            }
            string str=(sender as ComboBoxEdit).Tag.ToString();
            var TraceValue = EditTraceValuelist.Find(x=>x.DisplayName==str);
            if (TraceValue != null)
            {
                TraceValue.EditValuelist.Clear();
                foreach (var item in listitem)
                {
                    TraceValue.EditValuelist.Add(item);
                }
            }
            else
            {
                TraceEditValue editValue = new TraceEditValue() { DisplayName = str };
                EditTraceValuelist.Add(editValue);
                foreach (var item in listitem)
                {
                   editValue.EditValuelist.Add(item);
                }
            }
        }
    }
    public delegate void UpdateTraceData(List<TraceEditValue> TraceEditValue,string str,string traceName);
}
 