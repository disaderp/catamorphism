using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace catamorphism
{
    class MiniList
    {
        public BitmapImage _icon;
        public string _shortwebpagename;
        public string _login;
        public BitmapImage Icon
        {
            get { return _icon;  }
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
    class PageData
    {
        public string _website;
        public string _user;
        private string _password;
        public string _email;
        public string _created;
        private int _strength;
        public string WebName
        {
            get { return _website; }
        }
        public string User
        {
            get { return _user; }
        }
        public string Password
        {
            get { return "●●●●●●●●●●"; }
            set { this._password = value;
                this._strength = (int)(UIHelper.CheckStrength(value) * 12.5d);
            }
        }
        public string EMail
        {
            get { return _email; }
        }
        public string Created
        {
            get { return _created; }
        }
        public int Strength
        {
           get { return _strength ; }
        }
        public SolidColorBrush SColor
        {
            get {
                SolidColorBrush br = new SolidColorBrush();
                if (_strength > 80)
                {
                    br.Color = System.Windows.Media.Color.FromArgb(255, 0, 255, 0);
                } else if(_strength > 50)
                {
                    br.Color = System.Windows.Media.Color.FromArgb(255, 255, 255, 0);
                } else
                {
                    br.Color = System.Windows.Media.Color.FromArgb(255,255, 0, 0);
                }
                return br;
           }
        }
    }
    class ViewModel
    {
        public List<MiniList> savedpasswords;
        public PageData pd;
        public ViewModel()
        {
            pd = new PageData();
            pd._website = "http://webtesT.com";
            pd._user = "useeer";
            pd.Password = "aaa123Daeg*^#";
            pd._email = "use@mail";
            pd._created = "01:01:2000";

            savedpasswords = new List<MiniList>();
            MiniList item = new MiniList();
            item._login = "Test";
            item._shortwebpagename = "Short";
            item._icon = new BitmapImage(new Uri("C:\\Users\\Karol\\Documents\\Visual Studio 2013\\Projects\\DTun\\_0.ico"));
            savedpasswords.Add(item);
        }
        public PageData Page
        {
            get { return pd; }
        }
    }
}
