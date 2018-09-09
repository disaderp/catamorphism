using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.ComponentModel;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using MahApps.Metro.Controls;
using System.Windows.Data;

namespace catamorphism
{
    class ViewModel : INotifyPropertyChanged
    {
		private ViewWebsiteData pd;
		private List<MiniList> mini;
		private Model.Vault vault;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
		public void Load(int index)
		{
			if ( index == -1 )
			{//unload
				pd = null;
				OnPropertyChanged(new PropertyChangedEventArgs(string.Empty));
				return;
			}
			pd = vault.getViewWebsiteData(index);
			pd.Refresh(); //TODO: check if parent property changed is enough
			OnPropertyChanged(new PropertyChangedEventArgs(string.Empty));
		}
        public ViewModel(string pass)
        {
			vault = new Model.Vault(pass);
			mini = vault.getMiniList();
			//CollectionView view = CollectionViewSource.GetDefaultView(SavedList);
			//view.Refresh();
			//((MainWindow)Application.Current.MainWindow).listBox1.ItemsSource = "{ Binding SavedList }";
			OnPropertyChanged(new PropertyChangedEventArgs(string.Empty));
		}

		public ViewWebsiteData Page
        {
            get { return pd; }
        }
        public List<MiniList> SavedList
        {
            get { return mini; }
        }
		public void Save()
		{
			vault.Serialize();
		}
    }
}
