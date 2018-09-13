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
using MahApps.Metro.Controls.Dialogs;

namespace catamorphism
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
		ViewModel vm;
        public MainWindow()
        {
            InitializeComponent();

			showPasswordDialogAsync();

			progressbar1.Visibility = Visibility.Hidden;
            progressbar2.Visibility = Visibility.Hidden;
            checkBox1.Visibility = Visibility.Hidden;
            checkBox2.Visibility = Visibility.Hidden;
		}

        private void listBox1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (listBox1.SelectedItems.Count == 1) {
                progressbar1.Visibility = Visibility.Visible;
                progressbar2.Visibility = Visibility.Visible;
                checkBox1.Visibility = Visibility.Visible;
                checkBox2.Visibility = Visibility.Visible;

				vm.Load(listBox1.SelectedIndex);
            }
            else
            {
                progressbar1.Visibility = Visibility.Hidden;
                progressbar2.Visibility = Visibility.Hidden;
                checkBox1.Visibility = Visibility.Hidden;
                checkBox2.Visibility = Visibility.Hidden;

				vm.Load(-1); //-1 = unload
            }
		}
		public async void showDialog(string title, string text, bool critical)
		{
			await this.ShowMessageAsync(title, text);
			if (critical)
			{
				Environment.Exit(1);
			}
		}
		public async void showPasswordDialogAsync()
		{
			LoginDialogData t = await this.ShowLoginAsync("Authentication", "Enter your credentials", new LoginDialogSettings { ColorScheme = MetroDialogOptions.ColorScheme, InitialUsername = "catamorphism" });
			if (t == null)
			{
				showPasswordDialogAsync();
			}
			else
			{
				vm = new ViewModel(t.Password);
				DataContext = vm;
			}
		}

		private void MetroWindow_Closing(object sender, CancelEventArgs e)
		{
			vm.Save();
		}
	}
}
