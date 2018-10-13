using AcquisitionManager;
using Grpc.Core;
using Ipc.Definitions;

namespace Ipc.Server
{
	public class IpcServerImplementation : IIpcServer
	{
		private const int Port = 11972;
		private Grpc.Core.Server _server;
		private readonly IAcquisitionManager _acquisitionManager;

		public IpcServerImplementation(IAcquisitionManager acquisitionManager)
		{
			_acquisitionManager = acquisitionManager;
		}

		public void Start()
		{
			_server = new Grpc.Core.Server
			{
				Services = {AcquisitionManagerService.BindService(new AcquisitionManagerServiceImplementation(_acquisitionManager))},
				Ports = {new ServerPort("localhost", Port, ServerCredentials.Insecure)}
			};
			_server.Start();
		}

		public void Stop()
		{
			_server.ShutdownAsync().Wait();
		}
	}
}