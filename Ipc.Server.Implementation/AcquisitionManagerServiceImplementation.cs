using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using AcquisitionManager;
using Grpc.Core;
using Ipc.Definitions;

namespace Ipc.Server
{
	internal class AcquisitionManagerServiceImplementation : AcquisitionManagerService.AcquisitionManagerServiceBase
	{
		private readonly ConcurrentDictionary<Guid,
				Tuple<IServerStreamWriter<AcquisitionCompletionStateReply>, SemaphoreSlim>>
			_acquisitionCompletionStateDictionary =
				new ConcurrentDictionary<Guid,
					Tuple<IServerStreamWriter<AcquisitionCompletionStateReply>, SemaphoreSlim>>();

		private readonly IAcquisitionManager _acquisitionManager;

		private readonly ConcurrentDictionary<Guid, Tuple<IServerStreamWriter<AcquisitionStateReply>, SemaphoreSlim>>
			_acquisitionStateDictionary =
				new ConcurrentDictionary<Guid, Tuple<IServerStreamWriter<AcquisitionStateReply>, SemaphoreSlim>>();

		private readonly ConcurrentDictionary<Guid, Tuple<IServerStreamWriter<CurrentSampleNameReply>, SemaphoreSlim>>
			_currentSampleNameDictionary =
				new ConcurrentDictionary<Guid, Tuple<IServerStreamWriter<CurrentSampleNameReply>, SemaphoreSlim>>();

		public AcquisitionManagerServiceImplementation(IAcquisitionManager acquisitionManager)
		{
			_acquisitionManager = acquisitionManager;
			_acquisitionManager.CurrentSampleNameEvent += OnCurrentSampleNameEventAsync;
			_acquisitionManager.AcquisitionStateEvent += OnAcquisitionStateEventAsync;
			_acquisitionManager.AcquisitionCompletionStateEvent += OnAcquisitionCompletionStateAsync;
		}

		private async void OnAcquisitionCompletionStateAsync(object sender,
			EventArgs<AcquisitionCompletionState> eventArgs)
		{
			var acquisitionCompletionStateEnum =
				(AcquisitionCompletionStateReply.Types.AcquisitionCompletionStateEnum) Enum.Parse(
					typeof(AcquisitionCompletionStateReply.Types.AcquisitionCompletionStateEnum),
					eventArgs.Parameter.ToString());

			await ResponseStreamEventHandler(_acquisitionCompletionStateDictionary,
				new AcquisitionCompletionStateReply {AcquisitionCompletionStateEnum = acquisitionCompletionStateEnum});
		}

		private async void OnAcquisitionStateEventAsync(object sender, EventArgs<AcquisitionState> eventArgs)
		{
			var acquisitionStateEnum = (AcquisitionStateReply.Types.AcquisitionStateEnum) Enum.Parse(
				typeof(AcquisitionStateReply.Types.AcquisitionStateEnum),
				eventArgs.Parameter.ToString());

			await ResponseStreamEventHandler(_acquisitionStateDictionary,
				new AcquisitionStateReply {AcquisitionStateEnum = acquisitionStateEnum});
		}

		private async void OnCurrentSampleNameEventAsync(object sender, EventArgs<string> eventArgs)
		{
			await ResponseStreamEventHandler(_currentSampleNameDictionary,
				new CurrentSampleNameReply {CurrentSampleName = eventArgs.Parameter});
		}

		public override Task<StartReply> Start(StartRequest request, ServerCallContext context)
		{
			return Task.FromResult(new StartReply {Result = _acquisitionManager.Start(request.SampleName)});
		}

		public override Task<StopReply> Stop(StopRequest request, ServerCallContext context)
		{
			return Task.FromResult(new StopReply {Result = _acquisitionManager.Stop()});
		}

		public override async Task CurrentSampleName(CurrentSampleNameRequest request,
			IServerStreamWriter<CurrentSampleNameReply> responseStream, ServerCallContext context)
		{
			await ResponseStreamHandler(responseStream, context.CancellationToken,
				_currentSampleNameDictionary,
				new CurrentSampleNameReply {CurrentSampleName = _acquisitionManager.CurrentSampleName});
		}

		public override async Task AcquisitionState(AcquisitionStateRequest request,
			IServerStreamWriter<AcquisitionStateReply> responseStream, ServerCallContext context)
		{
			var acquisitionStateEnum = (AcquisitionStateReply.Types.AcquisitionStateEnum) Enum.Parse(
				typeof(AcquisitionStateReply.Types.AcquisitionStateEnum),
				_acquisitionManager.AcquisitionState.ToString());

			await ResponseStreamHandler(responseStream, context.CancellationToken, _acquisitionStateDictionary,
				new AcquisitionStateReply {AcquisitionStateEnum = acquisitionStateEnum});
		}

		public override async Task AcquisitionCompletionState(AcquisitionCompletionStateRequest request,
			IServerStreamWriter<AcquisitionCompletionStateReply> responseStream, ServerCallContext context)
		{
			var acquisitionCompletionStateEnum =
				(AcquisitionCompletionStateReply.Types.AcquisitionCompletionStateEnum) Enum.Parse(
					typeof(AcquisitionCompletionStateReply.Types.AcquisitionCompletionStateEnum),
					_acquisitionManager.AcquisitionCompletionState.ToString());

			await ResponseStreamHandler(responseStream, context.CancellationToken,
				_acquisitionCompletionStateDictionary,
				new AcquisitionCompletionStateReply {AcquisitionCompletionStateEnum = acquisitionCompletionStateEnum});
		}

		private static async Task ServerStreamWriteAsync<T>(IServerStreamWriter<T> serverStream,
			SemaphoreSlim semaphoreSlim, T serverStreamItem)
		{
			try
			{
				await serverStream.WriteAsync(serverStreamItem);
			}
			catch (RpcException exception)
			{
				Console.WriteLine(exception.Message);
				semaphoreSlim.Release();
			}
		}

		private static async Task ResponseStreamHandler<T>(IServerStreamWriter<T> serverStream,
			CancellationToken cancellationToken,
			ConcurrentDictionary<Guid, Tuple<IServerStreamWriter<T>, SemaphoreSlim>> dictionary, T serverStreamItem)
		{
			using (var semaphoreSlim = new SemaphoreSlim(0, 1))
			{
				var newGuid = Guid.NewGuid();
				if (!dictionary.TryAdd(newGuid,
					new Tuple<IServerStreamWriter<T>, SemaphoreSlim>(serverStream, semaphoreSlim)))
					throw new Exception("Something really wrong!");

				// Send initial state
				await ServerStreamWriteAsync(serverStream, semaphoreSlim, serverStreamItem);

				try
				{
					await semaphoreSlim.WaitAsync(cancellationToken);
				}
				catch (OperationCanceledException operationCanceledException)
				{
					Console.WriteLine(operationCanceledException.Message);
				}

				if (!dictionary.TryRemove(newGuid, out _))
					throw new Exception("Something really wrong!");

				Console.WriteLine("Done");
			}
		}

		private static async Task ResponseStreamEventHandler<T>(
			ConcurrentDictionary<Guid, Tuple<IServerStreamWriter<T>, SemaphoreSlim>> dictionary,
			T serverStreamItem)
		{
			foreach (var tuple in dictionary)
				await ServerStreamWriteAsync(tuple.Value.Item1, tuple.Value.Item2, serverStreamItem);
		}
	}
}