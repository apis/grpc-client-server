using System;
using System.Threading;
using System.Threading.Tasks;
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
			var sampleName = "remote_sample_007";

			GetCurrentSampleNameAsync(client);
			AcquisitionStateAsync(client);
			AcquisitionCompletionStateAsync(client);

			Thread.Sleep(1000);

			var reply = client.Start(new StartRequest { SampleName = sampleName });
			Console.WriteLine("Start() => " + reply.Result);

			Console.ReadKey();

			channel.ShutdownAsync().Wait();
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}

		public static async Task GetCurrentSampleNameAsync(AcquisitionManagerService.AcquisitionManagerServiceClient client)
		{
			try
			{
				using (var call = client.CurrentSampleName(new CurrentSampleNameRequest()))
				{
					var responseStream = call.ResponseStream;

					while (await responseStream.MoveNext(default(CancellationToken)))
					{
						var currentSampleNameReply = responseStream.Current;
						Console.WriteLine("CurrentSampleName => " + currentSampleNameReply.CurrentSampleName);
					}
				}
			}
			catch (RpcException exception)
			{
				Console.WriteLine("Exception => " + exception.Message);
			}
		}

		public static async Task AcquisitionStateAsync(AcquisitionManagerService.AcquisitionManagerServiceClient client)
		{
			try
			{
				using (var call = client.AcquisitionState(new AcquisitionStateRequest()))
				{
					var responseStream = call.ResponseStream;

					while (await responseStream.MoveNext(default(CancellationToken)))
					{
						var acquisitionStateReply = responseStream.Current;
						Console.WriteLine("AcquisitionState => " + acquisitionStateReply.AcquisitionStateEnum);
					}
				}
			}
			catch (RpcException exception)
			{
				Console.WriteLine("Exception => " + exception.Message);
			}
		}

		public static async Task AcquisitionCompletionStateAsync(AcquisitionManagerService.AcquisitionManagerServiceClient client)
		{
			try
			{
				using (var call = client.AcquisitionCompletionState(new AcquisitionCompletionStateRequest()))
				{
					var responseStream = call.ResponseStream;

					while (await responseStream.MoveNext(default(CancellationToken)))
					{
						var acquisitionCompletionStateReply = responseStream.Current;
						Console.WriteLine("AcquisitionCompletionState => " + acquisitionCompletionStateReply.AcquisitionCompletionStateEnum);
					}
				}
			}
			catch (RpcException exception)
			{
				Console.WriteLine("Exception => " + exception.Message);
			}
		}
	}
}