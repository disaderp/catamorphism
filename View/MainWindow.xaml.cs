using MahApps.Metro.Controls;
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

namespace catamorphism
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        ViewModel vm = new ViewModel();
        public MainWindow()
        {
            DataContext = vm;

            InitializeComponent();
            progressbar1.Visibility = Visibility.Hidden;
            progressbar2.Visibility = Visibility.Hidden;
            checkBox1.Visibility = Visibility.Hidden;
            checkBox2.Visibility = Visibility.Hidden;

			Model.Vault v = new Model.Vault();
			v.Serialize();
        }

        private void listBox1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (listBox1.SelectedItems.Count == 1) {
                progressbar1.Visibility = Visibility.Visible;
                progressbar2.Visibility = Visibility.Visible;
                checkBox1.Visibility = Visibility.Visible;
                checkBox2.Visibility = Visibility.Visible;

                vm.pd = new catamorphism.PageData();
                vm.pd._user = "USER!!";
                vm.pd.Password = "abc";
                vm.pd.Blacklisted = true;
                vm.pd.OTPTime = 5;
                vm.OnPropertyChanged(new PropertyChangedEventArgs(string.Empty));
            }
            else
            {
                progressbar1.Visibility = Visibility.Hidden;
                progressbar2.Visibility = Visibility.Hidden;
                checkBox1.Visibility = Visibility.Hidden;
                checkBox2.Visibility = Visibility.Hidden;
                vm.pd = null;
                vm.OnPropertyChanged(new PropertyChangedEventArgs(string.Empty));
            }
        }
    }
}
