using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.ComponentModel;

namespace catamorphism
{
    class ViewModel : INotifyPropertyChanged
    {
        public List<MiniList> savedpasswords;
        public PageData pd;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        public ViewModel()
        {
            pd = new PageData();
            pd._website = "http://webtesT.com";
            pd._user = "useeer";
            //pd.Password = "aaa123Daeg*^#";
            pd._email = "use@mail";
            pd._created = "01:01:2000";
            pd.OTP = "000 000";
            pd.OTPTime = 10;//10sec left

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
        public List<MiniList> SavedList
        {
            get { return savedpasswords; }
        }
    }
}
