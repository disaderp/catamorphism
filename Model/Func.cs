using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Security.Cryptography;

namespace catamorphism
{
    static class Func
    {
        public static int CheckStrength(string password)//https://stackoverflow.com/a/10966297
        {
            int score = 0;

            // using three requirements here:  min length and two types of characters (numbers and letters)
            bool blnMinLengthRequirementMet = false;
            bool blnRequirement1Met = false;
            bool blnRequirement2Met = false;

            // check for chars in password
            if (password.Length < 1)
                return 0;

            // if less than 6 chars, return as too short, else, plus one
            if (password.Length < 6)
            {
                return 1;
            }
            else
            {
                score++;
                blnMinLengthRequirementMet = true;
            }

            // if 8 or more chars, plus one
            if (password.Length >= 8)
                score++;

            // if 10 or more chars, plus one
            if (password.Length >= 10)
                score++;

            // if password has a number, plus one
            if (Regex.IsMatch(password, @"[\d]", RegexOptions.ECMAScript))
            {
                score++;
                blnRequirement1Met = true;
            }

            // if password has lower case letter, plus one
            if (Regex.IsMatch(password, @"[a-z]", RegexOptions.ECMAScript))
            {
                score++;
                blnRequirement2Met = true;
            }

            // if password has upper case letter, plus one
            if (Regex.IsMatch(password, @"[A-Z]", RegexOptions.ECMAScript))
            {
                score++;
                blnRequirement2Met = true;
            }

            // if password has a special character, plus one
            if (Regex.IsMatch(password, @"[~`!@#$%\^\&\*\(\)\-_\+=\[\{\]\}\|\\;:'\""<\,>\.\?\/£]", RegexOptions.ECMAScript))
                score++;

            // if password is longer than 2 characters and has 3 repeating characters, minus one (to minimum of score of 3)
            List<char> lstPass = password.ToList();
            if (lstPass.Count >= 3)
            {
                for (int i = 2; i < lstPass.Count; i++)
                {
                    char charCurrent = lstPass[i];
                    if (charCurrent == lstPass[i - 1] && charCurrent == lstPass[i - 2] && score >= 4)
                    {
                        score++;
                    }
                }
            }
            if (!blnMinLengthRequirementMet || !blnRequirement1Met || !blnRequirement2Met)
            {
                return 2;
            }
            return score;
        }

		public static List<Dictionary<string, object>> BreachTest(string account)
		{
			//wait for 1,5 sec
			System.Threading.Thread.Sleep(1600);
			string json;

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://haveibeenpwned.com/api/v2/breachedaccount/" + account);
			request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			request.UserAgent = "catamorphism-password-manager";
			//apply patch.reg

			HttpWebResponse response = null;
			try
			{
				response = (HttpWebResponse)request.GetResponse();
			} catch (WebException ex) {
				Dictionary<string, object> err = new Dictionary<string, object>();
				if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
				{
					var resp = (HttpWebResponse)ex.Response;
					if (resp.StatusCode == HttpStatusCode.NotFound)
					{
						err.Add("Leaked", false);
					}
					else
					{
						err.Add("HTTPError", true);//rate exceeded??
					}
				}
				else
				{
					err.Add("Error", true);
				}
				List<Dictionary<string, object>> temp = new List<Dictionary<string, object>>();
				temp.Add(err);
				return temp;
			}
			Stream stream = response.GetResponseStream();
			StreamReader reader = new StreamReader(stream);
			json = reader.ReadToEnd();
			
			return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
		}
		public static bool passLeak(string pass)
		{
			//wait for 1,5 sec
			System.Threading.Thread.Sleep(1600);

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.pwnedpasswords.com/range/" + GetSHA1Hash(pass).Substring(0, 5));
			request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			request.UserAgent = "catamorphism-password-manager";
			//apply patch.reg

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			Stream stream = response.GetResponseStream();
			StreamReader reader = new StreamReader(stream);

			string[] lines = reader.ReadToEnd().Split(new[] { "\r\n", "\r", "\n" },StringSplitOptions.None);
			
			foreach ( string line in lines)
			{
				if(line.Split(':')[0] == GetSHA1Hash(pass).Substring(5))// format HASH(from 6th char to end):leakcount
				{
					return true;
				}
			}
			return false;
		}
		public static string GetSHA1Hash(string input)
		{
			return string.Join("", (new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input))).Select(x => x.ToString("X2")).ToArray());
		}

	}

}
