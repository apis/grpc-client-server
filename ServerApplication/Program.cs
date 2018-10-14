using System;
using AcquisitionManager;
using Ipc.Server;

namespace ServerApplication
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			IAcquisitionManager acquisitionManager = new AcquisitionManagerImplementation();
			acquisitionManager.AcquisitionStateEvent += OnAcquisitionStateEvent;
			acquisitionManager.AcquisitionCompletionStateEvent += OnAcquisitionCompletionStateEvent;
			acquisitionManager.CurrentSampleNameEvent += OnCurrentSampleNameEvent;
			PrintAcquisitionState(acquisitionManager.AcquisitionState);
			PrintAcquisitionCompletionState(acquisitionManager.AcquisitionCompletionState);
			PrintCurrentSampleName(acquisitionManager.CurrentSampleName);

			IIpcServer server = new IpcServerImplementation(acquisitionManager);
			server.Start();

			Console.WriteLine("Press any key to stop the server...");
			Console.ReadKey();

			server.Stop();
		}

		private static void OnCurrentSampleNameEvent(object sender, EventArgs<string> eventArgs)
		{
			PrintCurrentSampleName(eventArgs.Parameter);
		}

		private static void PrintCurrentSampleName(string currentSampleName)
		{
			Console.WriteLine("CurrentSampleName => {0}", currentSampleName);
		}

		private static void OnAcquisitionStateEvent(object sender, EventArgs<AcquisitionState> eventArgs)
		{
			PrintAcquisitionState(eventArgs.Parameter);
		}

		private static void OnAcquisitionCompletionStateEvent(object sender,
			EventArgs<AcquisitionCompletionState> eventArgs)
		{
			PrintAcquisitionCompletionState(eventArgs.Parameter);
		}

		private static void PrintAcquisitionState(AcquisitionState acquisitionState)
		{
			Console.WriteLine("AcquisitionState => {0}", acquisitionState);
		}

		private static void PrintAcquisitionCompletionState(AcquisitionCompletionState acquisitionCompletionState)
		{
			Console.WriteLine("AcquisitionCompletionState => {0}", acquisitionCompletionState);
		}
	}
}