using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using AcquisitionManager;
using Ipc.Server;

namespace ServerApplication
{
	internal class Program
	{
		private static readonly string FileNameFormatter = "{0}_{1}.instance";
		private static Mutex _instanceMutex;

		public static string AssemblyDirectory
		{
			get
			{
				var codeBase = Assembly.GetExecutingAssembly().CodeBase;
				var uri = new UriBuilder(codeBase);
				var path = Uri.UnescapeDataString(uri.Path);
				return Path.GetDirectoryName(path);
			}
		}

		public static void Main(string[] args)
		{
			var instanceId = GetInstanceId(out var instanceIndex);
			Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "ID: {0} Index: {1}", instanceId,
				instanceIndex));

			IAcquisitionManager acquisitionManager = new AcquisitionManagerImplementation();
			acquisitionManager.AcquisitionStateEvent += OnAcquisitionStateEvent;
			acquisitionManager.AcquisitionCompletionStateEvent += OnAcquisitionCompletionStateEvent;
			acquisitionManager.CurrentSampleNameEvent += OnCurrentSampleNameEvent;
			PrintAcquisitionState(acquisitionManager.AcquisitionState);
			PrintAcquisitionCompletionState(acquisitionManager.AcquisitionCompletionState);
			PrintCurrentSampleName(acquisitionManager.CurrentSampleName);

			var grpcServer = CreateGrpcServer(acquisitionManager, instanceIndex);
			grpcServer.Start();

			//			var azureServer = CreateAzureServer(acquisitionManager);
			//			azureServer.Start();

			var amazonServer = CreateAmazonServer(instanceId, acquisitionManager);
			amazonServer.Start();


			Console.WriteLine("Press any key to stop the server...");
			Console.ReadKey();

			grpcServer.Stop();
//			azureServer.Stop();
			amazonServer.Stop();

			_instanceMutex.Dispose();
			_instanceMutex = null;
		}

		private static IIpcServer CreateGrpcServer(IAcquisitionManager acquisitionManager, int instanceIndex)
		{
			IIpcServer server = new IpcServerGrpcImplementation(acquisitionManager, instanceIndex);
			return server;
		}

		private static IIpcServer CreateAzureServer(IAcquisitionManager acquisitionManager)
		{
			IIpcServer server = new IpcServerAzureImplementation(acquisitionManager);
			return server;
		}

		private static IIpcServer CreateAmazonServer(string clientId, IAcquisitionManager acquisitionManager)
		{
			IIpcServer server = new IpcServerAmazonImplementation(clientId, acquisitionManager);
			return server;
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

		private static string GetInstanceId(out int instanceIndex)
		{
			var tuples = GetInstanceGuids();

			for (var index = 0; index < tuples.Length; index++)
			{
				var instanceGuid = tuples[index].Item2;
				_instanceMutex = new Mutex(false, instanceGuid);

				var mutexAcquired = AcquireMutex(_instanceMutex);

				if (mutexAcquired)
				{
					instanceIndex = tuples[index].Item1;
					return instanceGuid;
				}

				_instanceMutex.Dispose();
				_instanceMutex = null;
			}

			instanceIndex = tuples.Length;
			return CreateNewInstanceId(instanceIndex);
		}

		private static string CreateNewInstanceId(int instanceIndex)
		{
			var instanceGuid = Guid.NewGuid().ToString().ToLower(CultureInfo.InvariantCulture);

			_instanceMutex = new Mutex(false, instanceGuid);

			var mutexAcquired = AcquireMutex(_instanceMutex);

			if (!mutexAcquired)
				throw new Exception("Something really wrong here!!!");

			var fileName = string.Format(CultureInfo.InvariantCulture, FileNameFormatter, instanceIndex.ToString("D3"), instanceGuid);

			using (File.Create(Path.Combine(AssemblyDirectory, fileName)))
			{
			}

			return instanceGuid;
		}

		private static bool AcquireMutex(Mutex mutex)
		{
			bool mutexAcquired;
			try
			{
				mutexAcquired = mutex.WaitOne(0);
			}
			catch (AbandonedMutexException)
			{
				mutexAcquired = true;
			}

			return mutexAcquired;
		}

		private static Tuple<int, string>[] GetInstanceGuids()
		{
			return Directory.GetFiles(AssemblyDirectory, string.Format(CultureInfo.InvariantCulture, FileNameFormatter, "???", "*"))
				.Select(Path.GetFileNameWithoutExtension)
				.Select(item => item.ToLower(CultureInfo.InvariantCulture))
				.Select(item => new Tuple<int, string>(Convert.ToInt32(item.Substring(0, 3)), item.Substring(4)))
				.ToArray();
		}
	}
}