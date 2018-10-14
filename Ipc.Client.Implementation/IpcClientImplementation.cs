using Grpc.Core;

namespace Ipc.Client
{
	public class IpcClientImplementation : IIpcClient
	{
		protected internal const string HostName = "127.0.0.1";
		protected internal const string PortName = "11972";
		private Channel _channel;

		public void Start()
		{
			_channel = new Channel(HostName + ":" + PortName, ChannelCredentials.Insecure);
		}

		public void Stop()
		{
			_channel.ShutdownAsync().Wait();
		}

		public object Channel => _channel;
	}
}