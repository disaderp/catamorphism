using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace catamorphism.Model
{
    [Serializable]
    class WebsiteData
    {
        private string webname;
        private string username;
        private string email;
        private string password;
        private string creationDate;

        private string otpSecret;
        private OtpNet.OtpHashMode mode = OtpNet.OtpHashMode.Sha1;
        private int otpStep = 30;
        private int otpSize = 6;
        [NonSerialized] private OtpNet.Totp otpGenerator;

		public WebsiteData()
		{
			webname = "Website";
			username = "User";
			email = "Email";
			password = "Password";
			creationDate = "Date";
			otpSecret = "JBSWY3DPEHPK3PXP";
		}
        public void initOTP()
        {
            otpGenerator = new OtpNet.Totp(Base32.FromBase32String(otpSecret), otpStep, mode, otpSize);
        }
        public string getOTP
        {
            get { return otpGenerator.ComputeTotp(); }
        }
        public int OTPRemainingTime
        {
            get { return otpGenerator.RemainingSeconds(); }
        }
    }
    
}
