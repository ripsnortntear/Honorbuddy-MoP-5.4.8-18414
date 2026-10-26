using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using SevenZip;
using SevenZip.Compression.LZMA;

namespace Buddy_lib
{
	public class DataLib
	{
		private static byte[] ArrayPadding(byte[] arr, int toSize)
		{
			if (arr.Length != toSize)
			{
				byte[] array = new byte[toSize];
				for (int i = 0; i < toSize; i++)
				{
					if (i < arr.Length)
					{
						array[i] = arr[i];
					}
					else
					{
						array[i] = (byte)i;
					}
				}
				return array;
			}
			return arr;
		}

		public static string MD5(byte[] input)
		{
			if (input == null)
				return string.Empty;

			try
			{
				using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
				{
					byte[] hash = md5.ComputeHash(input);
					StringBuilder sb = new StringBuilder(hash.Length * 2);
					for (int i = 0; i < hash.Length; i++)
					{
						sb.Append(hash[i].ToString("x2"));
					}
					return sb.ToString();
				}
			}
			catch (Exception ex)
			{
				throw new ApplicationException(string.Format("\"{0}\" in `MD5`, data [{1}]", ex.Message, input != null), ex);
			}
		}

		public static byte[] AES_Crypt(byte[] data, byte[] key, byte[] iv)
		{
			try
			{
				key = ArrayPadding(key, 32);
				iv = ArrayPadding(iv, 16);
				using (RijndaelManaged rijndael = new RijndaelManaged())
				{
					rijndael.Key = key;
					rijndael.IV = iv;
					rijndael.Mode = CipherMode.ECB;
					rijndael.Padding = PaddingMode.ISO10126;
					using (MemoryStream ms = new MemoryStream())
					{
						using (CryptoStream cs = new CryptoStream(ms, rijndael.CreateEncryptor(), CryptoStreamMode.Write))
						{
							cs.Write(data, 0, data.Length);
						}
						return ms.ToArray();
					}
				}
			}
			catch (Exception ex)
			{
				throw new ApplicationException(string.Format("\"{0}\" in `AES_Crypt`, data [{1}], keyLen [{2}], ivLen [{3}]",
					ex.Message, data != null, key != null ? key.Length : 0, iv != null ? iv.Length : 0), ex);
			}
		}

		public static byte[] AES_Decrypt(byte[] data, byte[] key, byte[] iv)
		{
			try
			{
				key = ArrayPadding(key, 32);
				iv = ArrayPadding(iv, 16);
				using (RijndaelManaged rijndael = new RijndaelManaged())
				{
					rijndael.IV = iv;
					rijndael.Key = key;
					rijndael.Mode = CipherMode.ECB;
					rijndael.Padding = PaddingMode.ISO10126;
					using (MemoryStream ms = new MemoryStream(data))
					using (CryptoStream cs = new CryptoStream(ms, rijndael.CreateDecryptor(), CryptoStreamMode.Read))
					{
						byte[] buffer = new byte[data.Length];
						int read = cs.Read(buffer, 0, data.Length);
						byte[] result = new byte[read];
						Buffer.BlockCopy(buffer, 0, result, 0, read);
						return result;
					}
				}
			}
			catch (Exception ex)
			{
				throw new ApplicationException(string.Format("\"{0}\" in `AES_Decrypt`, data [{1}], keyLen [{2}], ivLen [{3}]",
					ex.Message, data != null, key != null ? key.Length : 0, iv != null ? iv.Length : 0), ex);
			}
		}

		public static byte[] Compress(byte[] data)
		{
			using (MemoryStream inputStream = new MemoryStream(data))
			{
				return Compress(inputStream);
			}
		}

		public static byte[] Compress(MemoryStream inputStream)
		{
			int dictionarySize = 8388608;
			int posStateBits = 2;
			int litContextBits = 3;
			int litPosBits = 0;
			int algorithm = 2;
			int numFastBytes = 128;
			string matchFinder = "bt4";
			bool endMarker = false;

			CoderPropID[] propIDs = new CoderPropID[8]
			{
				CoderPropID.DictionarySize,
				CoderPropID.PosStateBits,
				CoderPropID.LitContextBits,
				CoderPropID.LitPosBits,
				CoderPropID.Algorithm,
				CoderPropID.NumFastBytes,
				CoderPropID.MatchFinder,
				CoderPropID.EndMarker
			};

			object[] properties = new object[8]
			{
				dictionarySize,
				posStateBits,
				litContextBits,
				litPosBits,
				algorithm,
				numFastBytes,
				matchFinder,
				endMarker
			};

			using (MemoryStream memoryStream = new MemoryStream())
			{
				SevenZip.Compression.LZMA.Encoder encoder = new SevenZip.Compression.LZMA.Encoder();
				encoder.SetCoderProperties(propIDs, properties);
				encoder.WriteCoderProperties(memoryStream);
				memoryStream.Write(BitConverter.GetBytes(inputStream.Length), 0, 8);
				encoder.Code(inputStream, memoryStream, -1L, -1L, null);
				return memoryStream.ToArray();
			}
		}

		public static byte[] Decompress(byte[] data)
		{
			using (MemoryStream inputStream = new MemoryStream(data))
			{
				return Decompress(inputStream);
			}
		}

		public static byte[] Decompress(Stream inputStream)
		{
			SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();
			inputStream.Seek(0L, SeekOrigin.Begin);
			byte[] properties = new byte[5];
			if (inputStream.Read(properties, 0, 5) != 5)
			{
				throw new Exception("Can't read LZMA.properties");
			}
			decoder.SetDecoderProperties(properties);

			byte[] outSizeBytes = new byte[8];
			if (inputStream.Read(outSizeBytes, 0, 8) != 8)
			{
				throw new Exception("Can't read LZMA.outsize");
			}
			long outSize = BitConverter.ToInt64(outSizeBytes, 0);
			long inSize = inputStream.Length - inputStream.Position;

			using (MemoryStream memoryStream = new MemoryStream())
			{
				decoder.Code(inputStream, memoryStream, inSize, outSize, null);
				return memoryStream.ToArray();
			}
		}
	}
}
