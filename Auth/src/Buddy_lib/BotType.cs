using System;
using System.Collections.Generic;

namespace Buddy_lib
{
	public static class BotType
	{
		private static readonly Dictionary<string, byte> Signatures;

		static BotType()
		{
			Signatures = new Dictionary<string, byte>(StringComparer.OrdinalIgnoreCase)
			{
				{ "HB2", 2 },
				{ "DB", 8 }
			};
		}

		public static string GetSigByNum(byte num)
		{
			foreach (KeyValuePair<string, byte> signature in Signatures)
			{
				if (signature.Value == num)
				{
					return signature.Key;
				}
			}
			return num.ToString();
		}

		public static byte GetNumBySig(string sig)
		{
			if (string.IsNullOrEmpty(sig))
				return 0;

			byte value;
			Signatures.TryGetValue(sig, out value);
			return value;
		}
	}
}
