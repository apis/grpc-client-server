using System.Windows;
using AcquisitionManager;
using Ipc.Client;
using log4net.Config;

namespace WpfClientApplication
{
	public partial class App
	{
		private IIpcClient _ipcClient;

		public App()
		{
			XmlConfigurator.Configure();
		}

		private void OnStartup(object sender, StartupEventArgs eventArgs)
		{
			_ipcClient = CreateGrpcClient();
			_ipcClient.Start();

			var acquisitionManagerProxy = CreateGrpcAcquisitionManagerProxy(_ipcClient);

			ServiceLocator.Instance.RegisterService<IAcquisitionManager>(acquisitionManagerProxy);
		}

		private IIpcClient CreateGrpcClient()
		{
			var grpcClientImplementation = new IpcClientGrpcImplementation();
			return grpcClientImplementation;
		}

		private IAcquisitionManager CreateGrpcAcquisitionManagerProxy(IIpcClient ipcClient)
		{
			var grpcClientImplementation = (IpcClientGrpcImplementation) ipcClient;

			var grpcAcquisitionManagerProxyImplementation = new AcquisitionManagerProxyGrpcImplementation(
				grpcClientImplementation.Channel,
				grpcClientImplementation.CancellationToken);

			return grpcAcquisitionManagerProxyImplementation;
		}

		private void OnExit(object sender, ExitEventArgs eventArgs)
		{
			_ipcClient.Stop();
		}
	}
}