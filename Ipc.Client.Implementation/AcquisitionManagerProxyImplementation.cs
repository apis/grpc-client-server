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

		public AcquisitionManagerProxyImplementation(object channel, CancellationToken cancellationToken)
		{
			_acquisitionManagerServiceClient = new AcquisitionManagerService.AcquisitionManagerServiceClient((Channel)channel);

			GetCurrentSampleNameAsync(_acquisitionManagerServiceClient, cancellationToken);
			GetAcquisitionStateAsync(_acquisitionManagerServiceClient, cancellationToken);
			AcquisitionCompletionStateAsync(_acquisitionManagerServiceClient, cancellationToken);
		}

		public bool Start(string sampleName)
		{
			Log.DebugFormat("Begin Start({0})", sampleName);
			var startReply = _acquisitionManagerServiceClient.Start(new StartRequest {SampleName = sampleName});
			Log.DebugFormat("End Start() => {0}", startReply.Result);
			return startReply.Result;
		}

		public bool Stop()
		{
			Log.DebugFormat("Begin Stop()");
			var stopReply = _acquisitionManagerServiceClient.Stop(new StopRequest());
			Log.DebugFormat("End Stop() => {0}", stopReply.Result);
			return stopReply.Result;
		}

		public string CurrentSampleName
		{
			get
			{
				Log.DebugFormat("Begin get CurrentSampleName()");

				var currentSampleNameReply = _acquisitionManagerServiceClient.GetCurrentSampleName(new Empty());

				Log.DebugFormat("End get CurrentSampleName() => {0}", currentSampleNameReply.CurrentSampleName);

				return currentSampleNameReply.CurrentSampleName;
			}
		}

		public AcquisitionState AcquisitionState
		{
			get
			{
				Log.DebugFormat("Begin get AcquisitionState()");

				var acquisitionStateReply =
					_acquisitionManagerServiceClient.GetAcquisitionState(new Empty());

				var acquisitionState =
					ConvertToAcquisitionState(acquisitionStateReply.AcquisitionStateEnum);

				Log.DebugFormat("End get AcquisitionState() => {0}", acquisitionState);

				return acquisitionState;
			}
		}

		public AcquisitionCompletionState AcquisitionCompletionState
		{
			get
			{
				Log.DebugFormat("Begin get AcquisitionCompletionState()");

				var acquisitionCompletionStateReply =
					_acquisitionManagerServiceClient.GetAcquisitionCompletionState(
						new Empty());

				var acquisitionCompletionState =
					ConvertToAcquisitionCompletionState(acquisitionCompletionStateReply.AcquisitionCompletionStateEnum);

				Log.DebugFormat("End get AcquisitionCompletionState() => {0}", acquisitionCompletionState);

				return acquisitionCompletionState;
			}
		}

		public event EventHandler<EventArgs<string>> CurrentSampleNameEvent;
		public event EventHandler<EventArgs<AcquisitionState>> AcquisitionStateEvent;
		public event EventHandler<EventArgs<AcquisitionCompletionState>> AcquisitionCompletionStateEvent;

		public async void GetCurrentSampleNameAsync(
			AcquisitionManagerService.AcquisitionManagerServiceClient client, CancellationToken cancellationToken)
		{
			try
			{
				Log.DebugFormat("Begin stream event CurrentSampleName()");

				using (var call = client.GetCurrentSampleNameStream(new Empty()))
				{
					var responseStream = call.ResponseStream;

					while (await responseStream.MoveNext(cancellationToken))
					{
						var currentSampleNameReply = responseStream.Current;
						Log.DebugFormat("event CurrentSampleName() => {0}", currentSampleNameReply.CurrentSampleName);
						CurrentSampleNameEvent?.Invoke(this,
							new EventArgs<string>(currentSampleNameReply.CurrentSampleName));
					}
				}

				Log.DebugFormat("End stream event CurrentSampleName()");
			}
			catch (RpcException exception)
			{
				Log.Error("Error in stream event CurrentSampleName()", exception);
			}
			catch (OperationCanceledException)
			{
				Log.DebugFormat("End stream event CurrentSampleName() by cancelling it");
			}
		}

		public async void GetAcquisitionStateAsync(
			AcquisitionManagerService.AcquisitionManagerServiceClient client, CancellationToken cancellationToken)
		{
			try
			{
				Log.DebugFormat("Begin stream event AcquisitionState()");

				using (var call = client.GetAcquisitionStateStream(new Empty()))
				{
					var responseStream = call.ResponseStream;

					while (await responseStream.MoveNext(cancellationToken))
					{
						var acquisitionStateReply = responseStream.Current;
						var acquisitionState =
							ConvertToAcquisitionState(acquisitionStateReply.AcquisitionStateEnum);

						Log.DebugFormat("event AcquisitionState() => {0}", acquisitionState);

						AcquisitionStateEvent?.Invoke(this, new EventArgs<AcquisitionState>(acquisitionState));
					}
				}

				Log.DebugFormat("End stream event AcquisitionState()");
			}
			catch (RpcException exception)
			{
				Log.Error("Error in stream event AcquisitionState()", exception);
			}
			catch (OperationCanceledException)
			{
				Log.DebugFormat("End stream event AcquisitionState() by cancelling it");
			}
		}

		public async void AcquisitionCompletionStateAsync(
			AcquisitionManagerService.AcquisitionManagerServiceClient client, CancellationToken cancellationToken)
		{
			try
			{
				Log.DebugFormat("Begin stream event AcquisitionCompletionState()");

				using (var call =
					client.GetAcquisitionCompletionStateStream(new Empty()))
				{
					var responseStream = call.ResponseStream;

					while (await responseStream.MoveNext(cancellationToken))
					{
						var acquisitionCompletionStateReply = responseStream.Current;

						var acquisitionCompletionState =
							ConvertToAcquisitionCompletionState(acquisitionCompletionStateReply
								.AcquisitionCompletionStateEnum);

						Log.DebugFormat("event AcquisitionCompletionState() => {0}", acquisitionCompletionState);

						AcquisitionCompletionStateEvent?.Invoke(this,
							new EventArgs<AcquisitionCompletionState>(acquisitionCompletionState));
					}
				}

				Log.DebugFormat("End stream event AcquisitionCompletionState()");
			}
			catch (RpcException exception)
			{
				Log.Error("Error in stream event AcquisitionCompletionState()", exception);
			}
			catch (OperationCanceledException)
			{
				Log.DebugFormat("End stream event AcquisitionCompletionState() by cancelling it");
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