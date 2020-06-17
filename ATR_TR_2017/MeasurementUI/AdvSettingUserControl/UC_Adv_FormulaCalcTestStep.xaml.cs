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

namespace MeasurementUI
{
    /// <summary>
    /// Interaction logic for UC_Adv_FormulaCalcTestStep.xaml
    /// </summary>
    public partial class UC_Adv_FormulaCalcTestStep : UserControl
    {
        public UC_Adv_FormulaCalcTestStep()
        {
            InitializeComponent();
        }

        private void listFunName_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(listFunName.SelectedItem !=null)
            {
                string funName = listFunName.SelectedItem.ToString() + "()";
                this.txtFormul.Text = this.txtFormul.Text.Insert(this.txtFormul.SelectionStart,funName);
            }
        }

        private void listVarName_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(listVarName.SelectedItem !=null)
            {
                string varName = listVarName.SelectedItem.ToString();
                this.txtFormul.Text = this.txtFormul.Text.Insert(this.txtFormul.SelectionStart,varName);
                this.txtFormul.Focus();
                this.txtFormul.SelectionStart = this.txtFormul.Text.Length;
            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}
