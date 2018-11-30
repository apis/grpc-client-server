using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using AcquisitionManager;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Ipc.Definitions;
using Enum = System.Enum;

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
			var acquisitionCompletionStateEnum = ConvertToAcquisitionCompletionStateEnum(eventArgs.Parameter);

			await ResponseStreamEventHandler(_acquisitionCompletionStateDictionary,
				new AcquisitionCompletionStateReply {AcquisitionCompletionStateEnum = acquisitionCompletionStateEnum});
		}

		private static AcquisitionCompletionStateReply.Types.AcquisitionCompletionStateEnum
			ConvertToAcquisitionCompletionStateEnum(AcquisitionCompletionState acquisitionCompletionState)
		{
			var acquisitionCompletionStateEnum =
				(AcquisitionCompletionStateReply.Types.AcquisitionCompletionStateEnum) Enum.Parse(
					typeof(AcquisitionCompletionStateReply.Types.AcquisitionCompletionStateEnum),
					acquisitionCompletionState.ToString());

			return acquisitionCompletionStateEnum;
		}

		private async void OnAcquisitionStateEventAsync(object sender, EventArgs<AcquisitionState> eventArgs)
		{
			var acquisitionStateEnum = ConvertToAcquisitionStateEnum(eventArgs.Parameter);

			await ResponseStreamEventHandler(_acquisitionStateDictionary,
				new AcquisitionStateReply {AcquisitionStateEnum = acquisitionStateEnum});
		}

		private static AcquisitionStateReply.Types.AcquisitionStateEnum ConvertToAcquisitionStateEnum(
			AcquisitionState acquisitionState)
		{
			var acquisitionStateEnum = (AcquisitionStateReply.Types.AcquisitionStateEnum) Enum.Parse(
				typeof(AcquisitionStateReply.Types.AcquisitionStateEnum),
				acquisitionState.ToString());

			return acquisitionStateEnum;
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

		public override async Task GetCurrentSampleNameStream(Empty request,
			IServerStreamWriter<CurrentSampleNameReply> responseStream, ServerCallContext context)
		{
			await ResponseStreamHandler(responseStream, context.CancellationToken,
				_currentSampleNameDictionary);
		}

		public override async Task GetAcquisitionStateStream(Empty request,
			IServerStreamWriter<AcquisitionStateReply> responseStream, ServerCallContext context)
		{
			await ResponseStreamHandler(responseStream, context.CancellationToken, _acquisitionStateDictionary);
		}

		public override async Task GetAcquisitionCompletionStateStream(Empty request,
			IServerStreamWriter<AcquisitionCompletionStateReply> responseStream, ServerCallContext context)
		{
			await ResponseStreamHandler(responseStream, context.CancellationToken,
				_acquisitionCompletionStateDictionary);
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
			ConcurrentDictionary<Guid, Tuple<IServerStreamWriter<T>, SemaphoreSlim>> dictionary)
		{
			using (var semaphoreSlim = new SemaphoreSlim(0, 1))
			{
				var newGuid = Guid.NewGuid();
				if (!dictionary.TryAdd(newGuid,
					new Tuple<IServerStreamWriter<T>, SemaphoreSlim>(serverStream, semaphoreSlim)))
					throw new Exception("Something really wrong!");

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

		public override Task<CurrentSampleNameReply> GetCurrentSampleName(Empty request, ServerCallContext context)
		{
			return Task.FromResult(new CurrentSampleNameReply
				{CurrentSampleName = _acquisitionManager.CurrentSampleName});
		}

		public override Task<AcquisitionStateReply> GetAcquisitionState(Empty request, ServerCallContext context)
		{
			var acquisitionStateEnum = ConvertToAcquisitionStateEnum(_acquisitionManager.AcquisitionState);

			return Task.FromResult(new AcquisitionStateReply {AcquisitionStateEnum = acquisitionStateEnum});
		}

		public override Task<AcquisitionCompletionStateReply> GetAcquisitionCompletionState(Empty request,
			ServerCallContext context)
		{
			var acquisitionCompletionStateEnum =
				ConvertToAcquisitionCompletionStateEnum(_acquisitionManager.AcquisitionCompletionState);

			return Task.FromResult(new AcquisitionCompletionStateReply
				{AcquisitionCompletionStateEnum = acquisitionCompletionStateEnum});
		}
	}
}