using System;
using System.Threading;
using AcquisitionManager;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Ipc.Definitions;
using Enum = System.Enum;

namespace Ipc.Client
{
	public class AcquisitionManagerProxyImplementation : IAcquisitionManager
	{
		private readonly AcquisitionManagerService.AcquisitionManagerServiceClient _acquisitionManagerServiceClient;

		public AcquisitionManagerProxyImplementation(object channel)
		{
			_acquisitionManagerServiceClient = new AcquisitionManagerService.AcquisitionManagerServiceClient((Channel)channel);

			GetCurrentSampleNameAsync(_acquisitionManagerServiceClient);
			GetAcquisitionStateAsync(_acquisitionManagerServiceClient);
			AcquisitionCompletionStateAsync(_acquisitionManagerServiceClient);
		}

		public bool Start(string sampleName)
		{
			var startReply = _acquisitionManagerServiceClient.Start(new StartRequest {SampleName = sampleName});
			Console.WriteLine("Start() => " + startReply.Result);
			return startReply.Result;
		}

		public bool Stop()
		{
			var stopReply = _acquisitionManagerServiceClient.Stop(new StopRequest());
			Console.WriteLine("Stop() => " + stopReply.Result);
			return stopReply.Result;
		}

		public string CurrentSampleName
		{
			get
			{
				var currentSampleNameReply =
					_acquisitionManagerServiceClient.GetCurrentSampleName(new Empty());
				PrintCurrentSampleName(currentSampleNameReply.CurrentSampleName);
				return currentSampleNameReply.CurrentSampleName;
			}
		}

		public AcquisitionState AcquisitionState
		{
			get
			{
				var acquisitionStateReply =
					_acquisitionManagerServiceClient.GetAcquisitionState(new Empty());
				var acquisitionState =
					ConvertToAcquisitionState(acquisitionStateReply.AcquisitionStateEnum);

				PrintAcquisitionState(acquisitionState);

				return acquisitionState;
			}
		}

		public AcquisitionCompletionState AcquisitionCompletionState
		{
			get
			{
				var acquisitionCompletionStateReply =
					_acquisitionManagerServiceClient.GetAcquisitionCompletionState(
						new Empty());
				var acquisitionCompletionState =
					ConvertToAcquisitionCompletionState(acquisitionCompletionStateReply.AcquisitionCompletionStateEnum);
				PrintAcquisitionCompletionState(acquisitionCompletionState);
				return acquisitionCompletionState;
			}
		}

		public event EventHandler<EventArgs<string>> CurrentSampleNameEvent;
		public event EventHandler<EventArgs<AcquisitionState>> AcquisitionStateEvent;
		public event EventHandler<EventArgs<AcquisitionCompletionState>> AcquisitionCompletionStateEvent;


		public async void GetCurrentSampleNameAsync(
			AcquisitionManagerService.AcquisitionManagerServiceClient client)
		{
			try
			{
				using (var call = client.GetCurrentSampleNameStream(new Empty()))
				{
					var responseStream = call.ResponseStream;

					while (await responseStream.MoveNext(default(CancellationToken)))
					{
						var currentSampleNameReply = responseStream.Current;
						PrintCurrentSampleName(currentSampleNameReply.CurrentSampleName);
						CurrentSampleNameEvent?.Invoke(this,
							new EventArgs<string>(currentSampleNameReply.CurrentSampleName));
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

		public async void GetAcquisitionStateAsync(
			AcquisitionManagerService.AcquisitionManagerServiceClient client)
		{
			try
			{
				using (var call = client.GetAcquisitionStateStream(new Empty()))
				{
					var responseStream = call.ResponseStream;

					while (await responseStream.MoveNext(default(CancellationToken)))
					{
						var acquisitionStateReply = responseStream.Current;
						var acquisitionState =
							ConvertToAcquisitionState(acquisitionStateReply.AcquisitionStateEnum);
						PrintAcquisitionState(acquisitionState);
						AcquisitionStateEvent?.Invoke(this, new EventArgs<AcquisitionState>(acquisitionState));
					}
				}
			}
			catch (RpcException exception)
			{
				Console.WriteLine("Exception => " + exception.Message);
			}
		}

		private static void PrintAcquisitionState(AcquisitionState acquisitionState)
		{
			Console.WriteLine("AcquisitionState => " + acquisitionState);
		}

		public async void AcquisitionCompletionStateAsync(
			AcquisitionManagerService.AcquisitionManagerServiceClient client)
		{
			try
			{
				using (var call =
					client.GetAcquisitionCompletionStateStream(new Empty()))
				{
					var responseStream = call.ResponseStream;

					while (await responseStream.MoveNext(default(CancellationToken)))
					{
						var acquisitionCompletionStateReply = responseStream.Current;

						var acquisitionCompletionState =
							ConvertToAcquisitionCompletionState(acquisitionCompletionStateReply
								.AcquisitionCompletionStateEnum);

						PrintAcquisitionCompletionState(acquisitionCompletionState);

						AcquisitionCompletionStateEvent?.Invoke(this,
							new EventArgs<AcquisitionCompletionState>(acquisitionCompletionState));
					}
				}
			}
			catch (RpcException exception)
			{
				Console.WriteLine("Exception => " + exception.Message);
			}
		}

		private static void PrintAcquisitionCompletionState(
			AcquisitionCompletionState acquisitionCompletionState)
		{
			Console.WriteLine("AcquisitionCompletionState => " + acquisitionCompletionState);
		}

		private static AcquisitionCompletionState
			ConvertToAcquisitionCompletionState(
				AcquisitionCompletionStateReply.Types.AcquisitionCompletionStateEnum acquisitionCompletionStateEnum)
		{
			var acquisitionCompletionState =
				(AcquisitionCompletionState) Enum.Parse(
					typeof(AcquisitionCompletionState),
					acquisitionCompletionStateEnum.ToString());

			return acquisitionCompletionState;
		}

		private static AcquisitionState
			ConvertToAcquisitionState(
				AcquisitionStateReply.Types.AcquisitionStateEnum acquisitionStateEnum)
		{
			var acquisitionState =
				(AcquisitionState) Enum.Parse(
					typeof(AcquisitionState),
					acquisitionStateEnum.ToString());

			return acquisitionState;
		}
	}
}