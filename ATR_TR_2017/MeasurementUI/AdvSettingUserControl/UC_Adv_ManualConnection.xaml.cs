using System;
using System.Collections.Generic;
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

using System.IO;
using ViewModelBaseLib;

namespace MeasurementUI
{
    /// <summary>
    /// Interaction logic for UC_Adv_ManualConnection.xaml
    /// </summary>
    public partial class UC_Adv_ManualConnection : UserControl
    {
        public UC_Adv_ManualConnection()
        {
            InitializeComponent();
        }

        private void btnExportIMG_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.SaveFileDialog SFD = new System.Windows.Forms.SaveFileDialog();
                SFD.Filter = "PNG files (*.png)|*.png|JPG files (*.jpg)|*.jpg|BMP files (*.bmp)|*.bmp|GIF files (*.gif)|*.gif|All files (*.*)|*.*";
                if (SFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    BitmapSource BS = (BitmapSource)img.Source;
                    PngBitmapEncoder PBE = new PngBitmapEncoder();
                    PBE.Frames.Add(BitmapFrame.Create(BS));
                    using (Stream stream = File.Create(SFD.FileName))
                    {
                        PBE.Save(stream);
                    }
                    MessageBox.Show("导出成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnImportIMG_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.OpenFileDialog OFD = new System.Windows.Forms.OpenFileDialog();
                OFD.Filter = "PNG files (*.png)|*.png|JPG files (*.jpg)|*.jpg|BMP files (*.bmp)|*.bmp|GIF files (*.gif)|*.gif|All files (*.*)|*.*";
                if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ManualConnectionVM MC = this.DataContext as ManualConnectionVM;
                    MC.ImageFileName = OFD.FileName;
                    //this.img.Source = new BitmapImage(new Uri("\\bin\\Debug\\Sources\\" + OFD.FileName, UriKind.Relative));
                    this.img.Source = new BitmapImage(new Uri(OFD.FileName, UriKind.Absolute));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnDeleteIMG_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ManualConnectionVM MC = this.DataContext as ManualConnectionVM;
                MC.ImageFileName = "";
                this.img.Source = new BitmapImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                 if (e.ClickCount == 2)
                 {
                     UC_Adv_ShowImage AA = new UC_Adv_ShowImage();
                     AA.imageShow.Source = ((System.Windows.Controls.Image)(e.Source)).Source;
                     AA.ShowDialog();
                 }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
