using System;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Grpc.Core;
using log4net;

namespace Ipc.Client
{
	public class IpcClientImplementation : IIpcClient
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private const string HostKey = "Host";
		private const string PortKey = "Port";
		private string _host;
		private int _port = -1;
		private Channel _channel;
		private CancellationTokenSource _cancellationTokenSource;

		public IpcClientImplementation()
		{
			ParseConfiguration();

			if (_port == -1 || _host == null)
				throw new Exception("Invalid configuration!");
		}

		public void Start()
		{
			_cancellationTokenSource = new CancellationTokenSource();
			_channel = new Channel(_host + ":" + _port, ChannelCredentials.Insecure);
			Log.DebugFormat("Start(), Host: {0}, Port: {1}", _host, _port);
		}

		public void Stop()
		{
			_cancellationTokenSource.Cancel();
			_channel.ShutdownAsync().Wait();
			Log.Debug("Stop()");
		}

		public object Channel => _channel;

		public CancellationToken CancellationToken => _cancellationTokenSource.Token;

		private void ParseConfiguration()
		{
			var assemblyLocation = GetType().Assembly.Location;
			var configuration = ConfigurationManager.OpenExeConfiguration(assemblyLocation);

			var portKeyElement = configuration.AppSettings.Settings[PortKey];

			if (portKeyElement == null)
			{
				Log.ErrorFormat("Configuration key for {0} can't be found", PortKey);
				return;
			}

			if (!int.TryParse(portKeyElement.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var port))
			{
				Log.ErrorFormat("Configuration key for {0} can't be parsed", PortKey);
				return;
			}

			if (port <= 0)
			{
				Log.ErrorFormat("Configuration key for {0} can't be negative", PortKey);
				return;
			}

			_port = port;

			var hostKeyElement = configuration.AppSettings.Settings[HostKey];

			if (hostKeyElement == null)
			{
				Log.ErrorFormat("Configuration key for {0} can't be found", HostKey);
				return;
			}

			_host = hostKeyElement.Value;
		}
	}
}