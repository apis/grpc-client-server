using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using AcquisitionManager;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Ipc.Definitions;
using log4net;

namespace Ipc.Server
{
	public class IpcServerGrpcImplementation : IIpcServer
	{
		private const string HostKey = "Host";
		private const string PortKey = "Port";
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IAcquisitionManager _acquisitionManager;
		private readonly List<string> _hosts = new List<string>();
		private int _port = -1;

		private Grpc.Core.Server _server;

		public IpcServerGrpcImplementation(IAcquisitionManager acquisitionManager, int instanceIndex)
		{
			_acquisitionManager = acquisitionManager;
			ParseConfiguration();

			if (_port == -1 || _hosts.Count == 0)
				throw new Exception("Invalid configuration!");

			_port += instanceIndex;
		}

		public void Start()
		{
			_server = new Grpc.Core.Server
			{
				Services =
				{
					AcquisitionManagerService.BindService(
//						new AcquisitionManagerServiceImplementation(_acquisitionManager)).Intercept(new MyInterceptor())
						new AcquisitionManagerServiceImplementation(_acquisitionManager))
				}
			};

			foreach (var host in _hosts)
			{
				var serverPort = new ServerPort(host, _port, ServerCredentials.Insecure);
				_server.Ports.Add(serverPort);
			}

			_server.Start();
		}

		public void Stop()
		{
			_server.ShutdownAsync().Wait();
		}

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

			var hostKeys = configuration.AppSettings.Settings.AllKeys.Where(item => item.StartsWith(HostKey));

			foreach (var hostKey in hostKeys)
			{
				var hostKeyElement = configuration.AppSettings.Settings[hostKey];

				if (hostKeyElement == null)
				{
					Log.ErrorFormat("Configuration key for {0} can't be found", hostKey);
					continue;
				}

				_hosts.Add(hostKeyElement.Value);
			}
		}
	}
}