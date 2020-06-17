using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using ModelBaseLib;
using Symtant.InstruDriver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for InstruMgrNewView.xaml
    /// </summary>
    public partial class InstruMgrNewView : UserControl
    {
        InstruMgrManagerVM vm = new InstruMgrManagerVM();
        InstruInfoVM _selectedInstruCmdVM = new InstruInfoVM();
        public InstruMgrNewView()
        {
            InitializeComponent();
            InitialFile();
            grid.DataContext = vm;
            _selectedInstruCmdVM.InstruInfoList = vm.InstruInfo;
        }

        private void treeView_Selected(object sender, RoutedEventArgs e)
        {
            object obj = treeView.SelectedItem;
            GetSelectedItem(obj);
            if (listBox2.Visibility == Visibility.Visible)
            {
                UpdateLayout();
                ScrollViewer scrollViewer = DataUtils.CommUtils.FindVisualChild<ScrollViewer>(listBox2 as DependencyObject);
                int i = listBox2.Items.IndexOf(_selectedInstruCmdVM);
                scrollViewer.ScrollToVerticalOffset(listBox2.Items.IndexOf(_selectedInstruCmdVM) * 100);
            }
            else if (listBox1.Visibility == Visibility.Visible)
            {
                UpdateLayout();
                ScrollViewer scrollViewer = DataUtils.CommUtils.FindVisualChild<ScrollViewer>(listBox1 as DependencyObject);
                scrollViewer.ScrollToVerticalOffset(listBox1.Items.IndexOf(_selectedInstruCmdVM) * 100);
            }

        }

        private void barButtonConnectAll_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            vm.ConnectAll();
        }

        private void barButtonOverView_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {   
            BarButtonItem bi=sender as BarButtonItem;
            string str = bi.Content.ToString();
            if (str == "设备总览")
            {
                bi.Content = "退出总览";
                listBox1.Visibility = Visibility.Collapsed;
                listBox2.Visibility = Visibility.Visible;
                treeView.Visibility = Visibility.Collapsed;
                lv.Visibility = Visibility.Visible;
                vm.InstruList = vm.OverViewInstruList;
                var v = vm.InstruMgrList;
                vm.UpdateInstruMgrList();
            }
            else
            {
                bi.Content = "设备总览";
                listBox1.Visibility = Visibility.Visible;
                listBox2.Visibility = Visibility.Collapsed;
                lv.Visibility = Visibility.Collapsed;
                treeView.Visibility = Visibility.Visible;
                vm.InstruList = vm.TempInstruList;
                vm.UpdateInstruMgrList();
            }
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object obj = listBox1.SelectedItem;
            GetSelectedItem(obj);
        }

        private void listBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object obj = listBox2.SelectedItem;
            GetSelectedItem2(obj);
        }

        private void GetSelectedItem(object obj)
        {
            if (obj as ViewModelBaseLib.InstruMgrVM != null)
            {
                vm.SelectedInstruMgr = obj as ViewModelBaseLib.InstruMgrVM;
            }
            else if (obj as ViewModelBaseLib.InstruInfoVM != null)
            {
                InstruInfoVM instruComVM = obj as ViewModelBaseLib.InstruInfoVM;
                foreach (InstruMgrVM instruVM in vm.InstruList)
                {
                    foreach (InstruInfoVM instrCVM in instruVM.InstruInfoList)
                    {
                        if (instrCVM.Equals(instruComVM))
                        {
                            vm.SelectedInstruMgr = instruVM;
                            instruComVM.IsSelectedInstruMgr = true;
                            //lv.SelectedItem = instruVM;
                            _selectedInstruCmdVM = instruComVM;
                        }
                        else
                        {
                            instrCVM.IsSelectedInstruMgr = false;
                        }
                    }
                }
            }
        }

        private void GetSelectedItem2(object obj)
        {
            if (obj as ViewModelBaseLib.InstruMgrVM != null)
            {
                vm.SelectedInstruMgr = obj as ViewModelBaseLib.InstruMgrVM;
            }
            else if (obj as ViewModelBaseLib.InstruInfoVM != null)
            {
                InstruInfoVM instruComVM = obj as ViewModelBaseLib.InstruInfoVM;
                foreach (InstruMgrVM instruVM in vm.InstruList)
                {
                    foreach (InstruInfoVM instrCVM in instruVM.InstruInfoList)
                    {
                        if (instrCVM.Equals(instruComVM))
                        {
                            vm.SelectedInstruMgr = instruVM;
                            instruComVM.IsSelectedInstruMgr = true;
                            lv.SelectedItem = instruVM;
                            _selectedInstruCmdVM = instruComVM;
                        }
                        else
                        {
                            instrCVM.IsSelectedInstruMgr = false;
                        }
                    }
                }
            }
        }

        private void setUpButton_Click(object sender, RoutedEventArgs e)
        {
            var v = (sender as FrameworkElement).DataContext;
            InstruMgrSetUp instruMgrSetup = new InstruMgrSetUp();
            instruMgrSetup.DataContext = v;
            instruMgrSetup.Owner = Window.GetWindow(this);
            instruMgrSetup.Show();
            
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
             SaveFile();
        }

        private void InitialFile()
        {   
        }

        private void SaveFile()
        {
            string currentFilePath = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = currentFilePath + "/configfiles/DutDescSet.xml";
            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }
            //string result = DataUtils.CommUtils.SerializeData(typeof(TestStepInfo[]),TestStepInfoMgr.Instance.TestStepInfoList,fileName);
            //if(!result.Equals("OK"))
            //{
            //    DXMessageBox.Show(result.ToString());
            //}
            ObservableCollection<InstruInfo> instruInfolist = new ObservableCollection<InstruInfo>();
            vm.InstruList = vm.TempInstruList;
            foreach(InstruInfoVM instruInfoVM in vm.InstruMgrList)
            {
                instruInfolist.Add(instruInfoVM.InstruInfo);
            }
            string result = DataUtils.CommUtils.SerializeData(typeof(ObservableCollection<InstruInfo>),instruInfolist,fileName);
            if(!result.Equals("OK"))
            {
                DXMessageBox.Show(result.ToString());
            }
        }

        private void lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object obj = lv.SelectedItem;
            if (obj as ViewModelBaseLib.InstruMgrVM != null)
            {
                foreach (var item in (obj as ViewModelBaseLib.InstruMgrVM).InstruInfoList)
                {
                    GetSelectedItem(item);
                    break;
                }
            }
            if (listBox2.Visibility == Visibility.Visible)
            {
                ScrollViewer scrollViewer = DataUtils.CommUtils.FindVisualChild<ScrollViewer>(listBox2 as DependencyObject);
                int i = listBox2.Items.IndexOf(_selectedInstruCmdVM);
                scrollViewer.ScrollToVerticalOffset(listBox2.Items.IndexOf(_selectedInstruCmdVM) * 100);
            }
            else if (listBox1.Visibility == Visibility.Visible)
            {
                ScrollViewer scrollViewer = DataUtils.CommUtils.FindVisualChild<ScrollViewer>(listBox1 as DependencyObject);
                scrollViewer.ScrollToVerticalOffset(listBox1.Items.IndexOf(_selectedInstruCmdVM) * 100);
            }
        }
    }
}
