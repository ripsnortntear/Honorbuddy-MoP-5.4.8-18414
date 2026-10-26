using System.ServiceModel;
using System.ServiceModel.Channels;
using Buddy_lib;

namespace Buddy_dump
{
	public class AClient : ClientBase<IA>, IA
	{
		private static Binding CreateDefaultBinding()
		{
			return new NetTcpBinding(SecurityMode.None)
			{
				MaxReceivedMessageSize = 67108864,
				MaxBufferSize = 67108864,
				ReaderQuotas =
				{
					MaxArrayLength = 67108864,
					MaxBytesPerRead = 67108864,
					MaxStringContentLength = 67108864
				}
			};
		}

		public AClient()
			: base(CreateDefaultBinding(), new EndpointAddress("net.tcp://localhost:5031/AuthService.svc"))
		{
		}

		public AClient(string endpointConfigurationName)
			: base(endpointConfigurationName)
		{
		}

		public AClient(string endpointConfigurationName, string remoteAddress)
			: base(endpointConfigurationName, remoteAddress)
		{
		}

		public AClient(string endpointConfigurationName, EndpointAddress remoteAddress)
			: base(endpointConfigurationName, remoteAddress)
		{
		}

		public AClient(Binding binding, EndpointAddress remoteAddress)
			: base(binding, remoteAddress)
		{
		}

		public d0 Do(byte b, object[] args)
		{
			return Channel.Do(b, args);
		}
	}
}
