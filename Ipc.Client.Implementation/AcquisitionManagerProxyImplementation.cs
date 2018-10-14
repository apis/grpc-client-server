using System;
using System.Reflection;
using System.Threading;
using AcquisitionManager;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Ipc.Definitions;
using log4net;
using Enum = System.Enum;

namespace Ipc.Client
{
	public class AcquisitionManagerProxyImplementation : IAcquisitionManager
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
			Log.DebugFormat("Start({0}) => {1}", sampleName, startReply.Result);
			return startReply.Result;
		}

		public bool Stop()
		{
			var stopReply = _acquisitionManagerServiceClient.Stop(new StopRequest());
			Log.DebugFormat("Stop() => {0}", stopReply.Result);
			return stopReply.Result;
		}

		public string CurrentSampleName
		{
			get
			{
				var currentSampleNameReply =
					_acquisitionManagerServiceClient.GetCurrentSampleName(new Empty());
				Log.DebugFormat("get CurrentSampleName: {0}", currentSampleNameReply.CurrentSampleName);
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

				Log.DebugFormat("get AcquisitionState: {0}", acquisitionState);

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

				Log.DebugFormat("get AcquisitionCompletionState: {0}", acquisitionCompletionState);

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
						Log.DebugFormat("event CurrentSampleName: {0}", currentSampleNameReply.CurrentSampleName);
						CurrentSampleNameEvent?.Invoke(this,
							new EventArgs<string>(currentSampleNameReply.CurrentSampleName));
					}
				}
			}
			catch (RpcException exception)
			{
				Log.Error("event CurrentSampleName", exception);
			}
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

						Log.DebugFormat("event AcquisitionState: {0}", acquisitionState);

						AcquisitionStateEvent?.Invoke(this, new EventArgs<AcquisitionState>(acquisitionState));
					}
				}
			}
			catch (RpcException exception)
			{
				Log.Error("event AcquisitionState", exception);
			}
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

						Log.DebugFormat("event AcquisitionCompletionState: {0}", acquisitionCompletionState);

						AcquisitionCompletionStateEvent?.Invoke(this,
							new EventArgs<AcquisitionCompletionState>(acquisitionCompletionState));
					}
				}
			}
			catch (RpcException exception)
			{
				Log.Error("event AcquisitionCompletionState", exception);
			}
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