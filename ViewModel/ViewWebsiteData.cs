using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace catamorphism
{
    class ViewWebsiteData : INotifyPropertyChanged
    {
        public string _website;
        public string _user;
        private string _password;
        public string _email;
        public string _created;
        private int? _strength;
		private OtpNet.Totp otpGenerator;


		public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public ViewWebsiteData()
        {
			Refresh();
        }

		public void Refresh()
		{
			OnPropertyChanged(new PropertyChangedEventArgs(string.Empty));
		}

        public string WebName
        {
            get { return _website; }
			set { _website = value; }
        }
        public string User
        {
            get { return _user; }
			set { _user = value; }
        }
        public string Password
        {
            get { return "●●●●●●●●●●"; }
            set
            {
                this._password = value;
                this._strength = (int)(Func.CheckStrength(value) * 12.5d);
            }
        }
        public string EMail
		{
			get { return _email; }
			set { _email = value; }
		}
		public string Created
		{
			get { return _created; }
			set { _created = value; }
		}

		public int Strength
        {
            get
            {
                if (_strength == null) return 0;
                else return (int)_strength;
            }
        }
        public bool StrDeterminable
        {
            get
            {
                if (_strength == null) return true;
                else return false;
            }
        }
        public SolidColorBrush SColor
        {
            get
            {
                SolidColorBrush br = new SolidColorBrush();
                if (_strength > 80)
                {
                    br.Color = System.Windows.Media.Color.FromArgb(255, 0, 255, 0);
                }
                else if (_strength > 50)
                {
                    br.Color = System.Windows.Media.Color.FromArgb(255, 255, 255, 0);
                }
                else if (_strength == null)
                {
                    br.Color = System.Windows.Media.Color.FromArgb(255, 0, 255, 0);
                }
                else
                {
                    br.Color = System.Windows.Media.Color.FromArgb(255, 255, 0, 0);
                }
                return br;
            }
        }
		public bool? Blacklisted { get; set; }
		public OtpNet.Totp Generator
		{
			set { this.otpGenerator = value; }
		}
		public string OTP
        {
            get { return otpGenerator.ComputeTotp(); }
        }

        public int OTPTime
        {
			get { return Math.Min(100, (int)(otpGenerator.RemainingSeconds() * 3.4)); } //30 = 100% 
        }
    }
}
