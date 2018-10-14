using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using AcquisitionManager;

namespace WpfClientApplication.ViewModel
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		private readonly IAcquisitionManager _acquisitionManager;
		private AcquisitionCompletionState _acquisitionCompletionState;
		private AcquisitionState _acquisitionState;

		private string _currentSampleName;

		public MainWindowViewModel()
		{
			StartCommand = new RelayCommand(OnStartCommand, OnCanStartCommand);
			StopCommand = new RelayCommand(OnStopCommand, OnCanStopCommand);

			_acquisitionManager = ServiceLocator.Instance.GetService<IAcquisitionManager>();

			_acquisitionManager.CurrentSampleNameEvent += OnAcquisitionManagerCurrentSampleNameEvent;
			_acquisitionManager.AcquisitionStateEvent += OnAcquisitionManagerAcquisitionStateEvent;
			_acquisitionManager.AcquisitionCompletionStateEvent += OnAcquisitionManagerAcquisitionCompletionStateEvent;

			CurrentSampleName = _acquisitionManager.CurrentSampleName;
			AcquisitionState = _acquisitionManager.AcquisitionState;
			AcquisitionCompletionState = _acquisitionManager.AcquisitionCompletionState;
		}

		public RelayCommand StartCommand { get; }

		public RelayCommand StopCommand { get; }

		public string CurrentSampleName
		{
			get => _currentSampleName;
			set
			{
				_currentSampleName = value;
				OnPropertyChanged();
			}
		}

		public AcquisitionState AcquisitionState
		{
			get => _acquisitionState;
			set
			{
				_acquisitionState = value;
				StartCommand.RaiseCanExecuteChanged();
				StopCommand.RaiseCanExecuteChanged();
				OnPropertyChanged();
			}
		}

		public AcquisitionCompletionState AcquisitionCompletionState
		{
			get => _acquisitionCompletionState;
			set
			{
				_acquisitionCompletionState = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnAcquisitionManagerAcquisitionCompletionStateEvent(object sender,
			EventArgs<AcquisitionCompletionState> eventArgs)
		{
			AcquisitionCompletionState = eventArgs.Parameter;
		}

		private void OnAcquisitionManagerAcquisitionStateEvent(object sender, EventArgs<AcquisitionState> eventArgs)
		{
			AcquisitionState = eventArgs.Parameter;
		}

		private void OnAcquisitionManagerCurrentSampleNameEvent(object sender, EventArgs<string> eventArgs)
		{
			CurrentSampleName = eventArgs.Parameter;
		}

		private bool OnCanStopCommand()
		{
			return AcquisitionState == AcquisitionState.Running;
		}

		private void OnStopCommand(object parameter)
		{
			if (_acquisitionManager.Stop())
				return;

			MessageBox.Show("Can't stop sample!\nPossibly sample is not yet running.",
				"Confirmation",
				MessageBoxButton.OK,
				MessageBoxImage.Exclamation);
		}

		private bool OnCanStartCommand()
		{
			return AcquisitionState == AcquisitionState.Idle;
		}

		private void OnStartCommand(object parameter)
		{
			if (_acquisitionManager.Start("ZZZZ"))
				return;

			MessageBox.Show("Can't start sample!\nPossibly sample is running already.",
				"Confirmation",
				MessageBoxButton.OK,
				MessageBoxImage.Exclamation);
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}