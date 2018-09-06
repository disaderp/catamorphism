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

namespace catamorphism
{
    class ViewModel : INotifyPropertyChanged
    {
		private ViewWebsiteData pd;
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
        public ViewModel()
        {
			//TODO: ask for password
			vault = new Model.Vault("");
        }

		public ViewWebsiteData Page
        {
            get { return pd; }
        }
        public List<MiniList> SavedList
        {
            get { return vault.getMiniList(); }
        }
    }
}
