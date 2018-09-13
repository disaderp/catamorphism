using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace catamorphism.Model
{
    class Vault
    {
		private List<WebsiteData> passData = new List<WebsiteData>();//TODO: in-memory data protection //TODO remove new
		private string password;//TODO: in-memory data protection

		public Vault(string pass)
		{
			password = pass;
			Load();
		}
        public void Serialize()
        {
			WebsiteData tmp = new WebsiteData("www.", "disa", "@mail", "pass", "today", null, "JBSWY3DPEHPK3PXP", OtpNet.OtpHashMode.Sha1, 30, 6);
			//passData.Add(tmp);

            IFormatter formatter = new BinaryFormatter();
			MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, passData);
			stream.Position = 0;
		
			string encrypted = Cryptography.Encrypt<RijndaelManaged>(Convert.ToBase64String(stream.ToArray()), password);
			stream.Dispose();

			Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/catamorphism"));
			File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/catamorphism/vault.bin"), encrypted);
		}
		public void Load()
		{
			if(! File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/catamorphism/vault.bin")))
			{
				passData = new List<WebsiteData>();//no data
				return;
			}

			IFormatter formatter = new BinaryFormatter();
			string encrypted = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/catamorphism/vault.bin"));
			string decrypted = Cryptography.Decrypt<RijndaelManaged>(encrypted, password);
			
			if (decrypted == string.Empty)
			{
				((MainWindow)Application.Current.MainWindow).showDialog("Error", "Wrong password", true);
				return;
			}

			byte[] bytes = Convert.FromBase64String(decrypted);
			MemoryStream stream = new MemoryStream(bytes, 0, bytes.Length);
			stream.Write(bytes, 0, bytes.Length);
			stream.Position = 0;

			passData = (List<WebsiteData>)formatter.Deserialize(stream);
			//passData.ElementAt(0).email = "test@example.com"; //debug for breachtest
			stream.Dispose();
		}
		public ViewWebsiteData getViewWebsiteData(int index)
		{
			ViewWebsiteData wd = new ViewWebsiteData
			{
				WebName = passData.ElementAt(index).webname,
				User = passData.ElementAt(index).username,
				Password = passData.ElementAt(index).password,
				EMail = passData.ElementAt(index).email,
				Created = passData.ElementAt(index).creationDate,

				Generator = passData.ElementAt(index).OTP()
			};

			return wd;
		}
		public string getPass(int index)
		{
			return passData.ElementAt(index).password;
		}
		public List<MiniList> getMiniList()
		{
			List<MiniList> list = new List<MiniList>();
			foreach (WebsiteData wd in passData)
			{
				MiniList ml = new MiniList(wd.icon, wd.webname, wd.username);
				list.Add(ml);
			}
			return list;
		}
    }
}
