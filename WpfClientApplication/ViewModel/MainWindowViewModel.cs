using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using AcquisitionManager;

namespace WpfClientApplication.ViewModel
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		private readonly IAcquisitionManager _acquisitionManager;
		private AcquisitionCompletionState _acquisitionCompletionState;
		private AcquisitionState _acquisitionState;

		private string _currentSampleName;
		private string _sampleName;
		private const string RegexPattern = @"^\s*(\S.*)(\d+)\s*$";

		public MainWindowViewModel()
		{
			StartCommand = new RelayCommand(OnStartCommand, OnCanStartCommand);
			StopCommand = new RelayCommand(OnStopCommand, OnCanStopCommand);
			SampleName = "Remote Sample 1";

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

		public string SampleName
		{
			get => _sampleName;
			set
			{
				_sampleName = value;
				OnPropertyChanged();
			}
		}

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

			if (eventArgs.Parameter == AcquisitionState.Idle)
			{
				var regex = new Regex(RegexPattern, RegexOptions.RightToLeft);
				var match = regex.Match(SampleName);

				if (match.Groups.Count != 3)
					return;

				SampleName = match.Groups[1].Value + (Convert.ToInt32(match.Groups[2].Value) + 1);
			}
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
			if (_acquisitionManager.Start(SampleName))
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