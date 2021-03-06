﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

//https://stackoverflow.com/a/14286740
namespace catamorphism.Model
{
	public static class Cryptography
	{
		#region Settings

		private static int _iterations = 20;
		private static int _keySize = 256;

		private static string _hash = "SHA1";
		private static string _salt = "yYYuK0pCEmphZuibTgDT"; // Random
		private static string _vector = "5sWirEcAJfqvx0IX"; // Randomx16

		#endregion

		public static string Encrypt(string value, string password)
		{
			return Encrypt<AesManaged>(value, password);
		}
		public static string Encrypt<T>(string value, string password)
				where T : SymmetricAlgorithm, new()
		{
			byte[] vectorBytes = UTF8Encoding.UTF8.GetBytes(_vector);
			byte[] saltBytes = UTF8Encoding.UTF8.GetBytes(_salt);
			byte[] valueBytes = UTF8Encoding.UTF8.GetBytes(value);

			byte[] encrypted;
			using (T cipher = new T())
			{
				PasswordDeriveBytes _passwordBytes =
					new PasswordDeriveBytes(password, saltBytes, _hash, _iterations);
				byte[] keyBytes = _passwordBytes.GetBytes(_keySize / 8);

				cipher.Mode = CipherMode.CBC;

				using (ICryptoTransform encryptor = cipher.CreateEncryptor(keyBytes, vectorBytes))
				{
					using (MemoryStream to = new MemoryStream())
					{
						using (CryptoStream writer = new CryptoStream(to, encryptor, CryptoStreamMode.Write))
						{
							writer.Write(valueBytes, 0, valueBytes.Length);
							writer.FlushFinalBlock();
							encrypted = to.ToArray();
						}
					}
				}
				cipher.Clear();
			}
			return Convert.ToBase64String(encrypted);
		}

		public static string Decrypt(string value, string password)
		{
			return Decrypt<AesManaged>(value, password);
		}
		public static string Decrypt<T>(string value, string password) where T : SymmetricAlgorithm, new()
		{
			byte[] vectorBytes = ASCIIEncoding.ASCII.GetBytes(_vector);
			byte[] saltBytes = ASCIIEncoding.ASCII.GetBytes(_salt);
			byte[] valueBytes = Convert.FromBase64String(value);

			byte[] decrypted;
			int decryptedByteCount = 0;

			using (T cipher = new T())
			{
				PasswordDeriveBytes _passwordBytes = new PasswordDeriveBytes(password, saltBytes, _hash, _iterations);
				byte[] keyBytes = _passwordBytes.GetBytes(_keySize / 8);

				cipher.Mode = CipherMode.CBC;

				try
				{
					using (ICryptoTransform decryptor = cipher.CreateDecryptor(keyBytes, vectorBytes))
					{
						using (MemoryStream from = new MemoryStream(valueBytes))
						{
							using (CryptoStream reader = new CryptoStream(from, decryptor, CryptoStreamMode.Read))
							{
								decrypted = new byte[valueBytes.Length];
								decryptedByteCount = reader.Read(decrypted, 0, decrypted.Length);
							}
						}
					}
				}
				catch (Exception)
				{
					return String.Empty;
				}

				cipher.Clear();
			}
			return Encoding.UTF8.GetString(decrypted, 0, decryptedByteCount);
		}

	}
}