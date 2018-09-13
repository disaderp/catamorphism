using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace catamorphism.Model
{
    [Serializable]
    class WebsiteData
    {
        public string webname;
        public string username;
        public string email;
        public string password;
        public string creationDate;
		public string icon;

		public string otpSecret;
        public OtpNet.OtpHashMode mode = OtpNet.OtpHashMode.Sha1;
        public int otpStep = 30;
        public int otpSize = 6;

		public WebsiteData(string _webn, string _usern, string _email, string _password, string _date, string _icon, string _otpSecret, OtpNet.OtpHashMode _mode, int _step, int _size)
		{
			webname = _webn;
			username = _usern;
			email = _email;
			password = _password;
			creationDate = _date;
			icon = _icon;

			otpSecret = _otpSecret;
			mode = _mode;
			otpStep = _step;
			otpSize = _size;
		}
		public OtpNet.Totp OTP()
		{
			if (otpSecret == "") {
				return null;
			}
			return new OtpNet.Totp(Base32.FromBase32String(otpSecret), otpStep, mode, otpSize);
		}
	}
    
}
