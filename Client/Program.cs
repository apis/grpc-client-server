using System;
using System.Threading;
using Grpc.Core;
using Ipc.Definitions;

namespace Client
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			var channel = new Channel("127.0.0.1:11972", ChannelCredentials.Insecure);

			var client = new AcquisitionManagerService.AcquisitionManagerServiceClient(channel);

			GetCurrentSampleNameAsync(client);
			GetAcquisitionStateAsync(client);
			AcquisitionCompletionStateAsync(client);

			var currentSampleNameReply = client.GetCurrentSampleName(new Google.Protobuf.WellKnownTypes.Empty());
			PrintCurrentSampleName(currentSampleNameReply.CurrentSampleName);

			var acquisitionStateReply = client.GetAcquisitionState(new Google.Protobuf.WellKnownTypes.Empty());
			PrintAcquisitionState(acquisitionStateReply.AcquisitionStateEnum);

			var acquisitionCompletionStateReply = client.GetAcquisitionCompletionState(new Google.Protobuf.WellKnownTypes.Empty());
			PrintAcquisitionCompletionState(acquisitionCompletionStateReply.AcquisitionCompletionStateEnum);

			var sampleName = "remote_sample_007";
			var reply = client.Start(new StartRequest { SampleName = sampleName });
			Console.WriteLine("Start() => " + reply.Result);

			Console.ReadKey();

			channel.ShutdownAsync().Wait();
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}

		public static async void GetCurrentSampleNameAsync(AcquisitionManagerService.AcquisitionManagerServiceClient client)
		{
			try
			{
				using (var call = client.GetCurrentSampleNameStream(new Google.Protobuf.WellKnownTypes.Empty()))
				{
					var responseStream = call.ResponseStream;

					while (await responseStream.MoveNext(default(CancellationToken)))
					{
						var currentSampleNameReply = responseStream.Current;
						PrintCurrentSampleName(currentSampleNameReply.CurrentSampleName);
					}
				}
			}
			catch (RpcException exception)
			{
				Console.WriteLine("Exception => " + exception.Message);
			}
		}

		private static void PrintCurrentSampleName(string currentSampleName)
		{
			Console.WriteLine("CurrentSampleName => " + currentSampleName);
		}

		public static async void GetAcquisitionStateAsync(AcquisitionManagerService.AcquisitionManagerServiceClient client)
		{
			try
			{
				using (var call = client.GetAcquisitionStateStream(new Google.Protobuf.WellKnownTypes.Empty()))
				{
					var responseStream = call.ResponseStream;

					while (await responseStream.MoveNext(default(CancellationToken)))
					{
						var acquisitionStateReply = responseStream.Current;
						PrintAcquisitionState(acquisitionStateReply.AcquisitionStateEnum);
					}
				}
			}
			catch (RpcException exception)
			{
				Console.WriteLine("Exception => " + exception.Message);
			}
		}

		private static void PrintAcquisitionState(AcquisitionStateReply.Types.AcquisitionStateEnum acquisitionStateEnum)
		{
			Console.WriteLine("AcquisitionState => " + acquisitionStateEnum);
		}

		public static async void AcquisitionCompletionStateAsync(AcquisitionManagerService.AcquisitionManagerServiceClient client)
		{
			try
			{
				using (var call = client.GetAcquisitionCompletionStateStream(new Google.Protobuf.WellKnownTypes.Empty()))
				{
					var responseStream = call.ResponseStream;

					while (await responseStream.MoveNext(default(CancellationToken)))
					{
						var acquisitionCompletionStateReply = responseStream.Current;
						PrintAcquisitionCompletionState(acquisitionCompletionStateReply.AcquisitionCompletionStateEnum);
					}
				}
			}
			catch (RpcException exception)
			{
				Console.WriteLine("Exception => " + exception.Message);
			}
		}

		private static void PrintAcquisitionCompletionState(AcquisitionCompletionStateReply.Types.AcquisitionCompletionStateEnum acquisitionCompletionStateEnum)
		{
			Console.WriteLine("AcquisitionCompletionState => " + acquisitionCompletionStateEnum);
		}
	}
}