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

		private void OnaAppStartup(object sender, StartupEventArgs eventArgs)
		{
			_ipcClient = new IpcClientImplementation();
			_ipcClient.Start();

			ServiceLocator.Instance.RegisterService<IAcquisitionManager>(new AcquisitionManagerProxyImplementation(_ipcClient.Channel, _ipcClient.CancellationToken));
		}

		private void OnAppExit(object sender, ExitEventArgs eventArgs)
		{
			_ipcClient.Stop();
		}
	}
}