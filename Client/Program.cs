using System;
using AcquisitionManager;
using Ipc.Client;

namespace Client
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			IIpcClient ipcClient = new IpcClientImplementation();
			ipcClient.Start();

			IAcquisitionManager acquisitionManagerProxy = new AcquisitionManagerProxyImplementation(ipcClient.Channel);
			var sampleName = "remote_sample_007";
			acquisitionManagerProxy.Start(sampleName);

			Console.ReadKey();

			ipcClient.Stop();
		}
	}
}