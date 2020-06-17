using SymtantPropertyGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace MeasurementUI.View
{
    /// <summary>
    /// Interaction logic for PropertyGridUC.xaml
    /// </summary>
    public partial class PropertyGridUC : UserControl
    {
        public PropertyGridUC(PropertyGridVM view)
        {
            InitializeComponent();
            Binding bind = new Binding();
            bind.Source = view;
            bind.Mode = BindingMode.TwoWay;
            bind.Path = new PropertyPath("Property");
            SymPro.SetBinding(SymtantProperty.SelectObjectProperty, bind);
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }

    public class PropertyGridVM : INotifyPropertyChanged
    {
        private object _Property;
        public object Property
        {
            get
            {
                return _Property;
            }
            set
            {
                _Property = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Property"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
