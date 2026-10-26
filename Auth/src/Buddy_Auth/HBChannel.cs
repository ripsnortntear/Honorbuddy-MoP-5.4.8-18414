using System;
using System.Text;
using Buddy_lib;

namespace HB_Auth
{
	internal class HBChannel : IA
	{
		private static string GetOpcodeName(byte opcode)
		{
			switch (opcode)
			{
				case 1: return "Login";
				case 2: return "Logout";
				case 4: return "SendOffsets";
				case 8: return "SendMeshFile";
				case 16: return "SendTileFile";
				case 18: return "CheckTime";
				case 52: return "CheckVersion";
				case 56: return "FlyTo";
				default: return "Opcode_" + opcode;
			}
		}

		public d0 Do(byte b, object[] args)
		{
			try
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendFormat("{0}({1}", GetOpcodeName(b), b);
				if (args != null && args.Length > 0)
				{
					for (int i = 0; i < args.Length; i++)
					{
						object obj = args[i];
						if (obj == null)
						{
							sb.Append(", null");
						}
						else if (obj is byte[] bytes)
						{
							sb.AppendFormat(", [byte[{0}]]", bytes.Length);
						}
						else
						{
							sb.AppendFormat(", \"{0}\"", obj);
						}
					}
				}
				sb.Append(")");
				Logging.Req(sb.ToString());
			}
			catch (Exception ex)
			{
				Logging.Ex("Error formatting request {0}: {1}", b, ex.Message);
			}

			return AuthImpl.Instance.Request(b, args);
		}
	}
}
