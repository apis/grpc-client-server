using System.Reflection;
using System.Threading;
using Grpc.Core;
using log4net;

namespace Ipc.Client
{
	public class IpcClientImplementation : IIpcClient
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected internal const string HostName = "127.0.0.1";
		protected internal const string PortName = "11972";
		private Channel _channel;
		private CancellationTokenSource _cancellationTokenSource;

		public void Start()
		{
			_cancellationTokenSource = new CancellationTokenSource();
			_channel = new Channel(HostName + ":" + PortName, ChannelCredentials.Insecure);
			Log.DebugFormat("Start(), Host: {0}, Port: {1}", HostName, PortName);
		}

		public void Stop()
		{
			_cancellationTokenSource.Cancel();
			_channel.ShutdownAsync().Wait();
			Log.Debug("Stop()");
		}

		public object Channel => _channel;

		public CancellationToken CancellationToken => _cancellationTokenSource.Token;
	}
}