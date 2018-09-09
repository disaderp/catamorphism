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
		private List<WebsiteData> passes = new List<WebsiteData>();

        public void Serialize()
        {
			WebsiteData d = new Model.WebsiteData();
			d.initOTP();
			Console.WriteLine(d.getOTP);
			passes.Add(d);

            IFormatter formatter = new BinaryFormatter();
            var stream = new MemoryStream();
            //Stream stream = new FileStream("MyFile.bin", FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, passes);
			stream.Position = 0;
			StreamReader sr = new StreamReader(stream);
		
			string encrypted = Cryptography.Encrypt<RijndaelManaged>(sr.ReadToEnd(), "testing");
			string decrypted = Cryptography.Decrypt<RijndaelManaged>(encrypted, "testing");

			sr.Dispose();
			stream.Dispose();
		}
    }
}
