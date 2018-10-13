using System;
using System.Threading;
using System.Threading.Tasks;

namespace AcquisitionManager
{
	public class AcquisitionManagerImplementation : IAcquisitionManager
	{
		protected internal const int RunDelayInMilliseconds = 5000;
		private readonly object _padLock = new object();
		private AcquisitionCompletionState _acquisitionCompletionState;
		private AcquisitionState _acquisitionState;
		private CancellationTokenSource _cancellationTokenSource;
		private string _currentSampleName;

		public AcquisitionManagerImplementation()
		{
			lock (_padLock)
			{
				SetAcquisitionState(AcquisitionState.Idle);
				SetAcquisitionCompletionState(AcquisitionCompletionState.SuccessfullyCompleted);
				SetCurrentSampleName(string.Empty);
			}
		}

		public bool Start(string sampleName)
		{
			lock (_padLock)
			{
				if (AcquisitionState == AcquisitionState.Running)
					return false;

				SetAcquisitionState(AcquisitionState.Running);
				SetAcquisitionCompletionState(AcquisitionCompletionState.Incomplete);
				SetCurrentSampleName(sampleName);
				_cancellationTokenSource = new CancellationTokenSource();
			}

			RunAsync();

			return true;
		}

		public bool Stop()
		{
			lock (_padLock)
			{
				if (AcquisitionState == AcquisitionState.Idle)
					return false;

				_cancellationTokenSource.Cancel();
			}

			return true;
		}

		public string CurrentSampleName
		{
			get
			{
				lock (_padLock)
				{
					return _currentSampleName;
				}
			}

			private set => _currentSampleName = value;
		}

		public AcquisitionState AcquisitionState
		{
			get
			{
				lock (_padLock)
				{
					return _acquisitionState;
				}
			}

			private set => _acquisitionState = value;
		}

		public AcquisitionCompletionState AcquisitionCompletionState
		{
			get
			{
				lock (_padLock)
				{
					return _acquisitionCompletionState;
				}
			}

			private set => _acquisitionCompletionState = value;
		}

		public event EventHandler<EventArgs<string>> CurrentSampleNameEvent;
		public event EventHandler<EventArgs<AcquisitionState>> AcquisitionStateEvent;
		public event EventHandler<EventArgs<AcquisitionCompletionState>> AcquisitionCompletionStateEvent;

		private void SetAcquisitionCompletionState(AcquisitionCompletionState acquisitionCompletionState)
		{
			AcquisitionCompletionState = acquisitionCompletionState;
			AcquisitionCompletionStateEvent?.Invoke(this,
				new EventArgs<AcquisitionCompletionState>(acquisitionCompletionState));
		}

		private void SetAcquisitionState(AcquisitionState acquisitionState)
		{
			AcquisitionState = acquisitionState;
			AcquisitionStateEvent?.Invoke(this, new EventArgs<AcquisitionState>(acquisitionState));
		}

		private void SetCurrentSampleName(string currentSampleName)
		{
			CurrentSampleName = currentSampleName;
			CurrentSampleNameEvent?.Invoke(this, new EventArgs<string>(currentSampleName));
		}

		private async void RunAsync()
		{
			var delayTask = Task.Delay(RunDelayInMilliseconds, _cancellationTokenSource.Token);
			try
			{
				await delayTask;
			}
			catch (TaskCanceledException)
			{
			}

			var stopped = delayTask.IsCanceled;

			lock (_padLock)
			{
				SetAcquisitionState(AcquisitionState.Idle);
				SetAcquisitionCompletionState(stopped
					? AcquisitionCompletionState.Stopped
					: AcquisitionCompletionState.SuccessfullyCompleted);
			}
		}
	}
}