using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace catamorphism
{
    class MiniList : INotifyPropertyChanged
    {
        public BitmapImage _icon;
        public string _shortwebpagename;
        public string _login;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

		public MiniList(BitmapImage icon, string shortPage, string login)
		{
			_icon = icon;
			_shortwebpagename = shortPage;
			_login = login;
		}

        public BitmapImage Icon
        {
            get { return _icon; }
        }
        public string ShortName
        {
            get { return _shortwebpagename; }
        }
        public string Login
        {
            get { return _login; }
        }
    }
}
