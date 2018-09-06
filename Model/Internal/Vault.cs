using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace catamorphism.Model
{
    class Vault
    {
		private List<WebsiteData> passData;//TODO: in-memory data protection
		private string password;//TODO: in-memory data protection

		public Vault(string pass)
		{
			password = pass;
			Load();
		}
        public void Serialize()
        {
            IFormatter formatter = new BinaryFormatter();
			MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, passData);
			stream.Position = 0;
			StreamReader sr = new StreamReader(stream);
		
			string encrypted = Cryptography.Encrypt<RijndaelManaged>(sr.ReadToEnd(), password);
			sr.Dispose();
			stream.Dispose();
			File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "catamorphism/vault.bin"), encrypted);
		}
		public void Load()
		{
			if(! File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "catamorphism/vault.bin")))
			{
				passData = new List<WebsiteData>();//no data
				return;
			}

			IFormatter formatter = new BinaryFormatter();
			string encrypted = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "catamorphism/vault.bin"));
			string decrypted = Cryptography.Decrypt<RijndaelManaged>(encrypted, password);

			MemoryStream stream = new MemoryStream();
			StreamWriter sr = new StreamWriter(stream);
			sr.Write(decrypted);
			sr.Flush();
			sr.Dispose();

			passData = (List<WebsiteData>)formatter.Deserialize(stream);

			stream.Dispose();
		}
		public ViewWebsiteData getViewWebsiteData(int index)
		{
			ViewWebsiteData wd = new ViewWebsiteData();
			wd.WebName = passData.ElementAt(index).webname;
			wd.User = passData.ElementAt(index).username;
			wd.Password = passData.ElementAt(index).password;
			wd.EMail = passData.ElementAt(index).email;
			wd.Created = passData.ElementAt(index).creationDate;

			wd.Generator = passData.ElementAt(index).OTP();

			return wd;
		}
		public List<MiniList> getMiniList()
		{
			List<MiniList> list = new List<MiniList>();
			foreach (WebsiteData wd in passData)
			{
				MiniList ml = new MiniList(wd.icon, wd.webname, wd.username);
			}
			return list;
		}
    }
}
