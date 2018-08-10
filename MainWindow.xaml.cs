using MahApps.Metro.Controls;
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

namespace catamorphism
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            ViewModel vm = new ViewModel();
            DataContext = vm;

            InitializeComponent();

            listBox1.ItemsSource = vm.savedpasswords;
            
        }

        private void listBox1_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
