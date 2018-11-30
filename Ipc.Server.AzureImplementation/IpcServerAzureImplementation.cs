using System;
using System.Configuration;
using System.Reflection;
using System.Text;
using AcquisitionManager;
using log4net;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace Ipc.Server
{
	public class IpcServerAzureImplementation : IIpcServer
	{
		private const string DeviceConnectionStringKey = "DeviceConnectionString";
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IAcquisitionManager _acquisitionManager;
		private readonly object _padLock = new object();
		private DeviceClient _deviceClient;
		private string _deviceConnectionString;

		public IpcServerAzureImplementation(IAcquisitionManager acquisitionManager)
		{
			_acquisitionManager = acquisitionManager;

			ParseConfiguration();

			if (_deviceConnectionString == null)
				throw new Exception("Invalid configuration!");
		}

		public void Start()
		{
			lock (_padLock)
			{
				if (_deviceClient != null)
					return;

				_deviceClient = DeviceClient.CreateFromConnectionString(_deviceConnectionString, TransportType.Amqp);

				_acquisitionManager.AcquisitionStateEvent += OnAcquisitionStateEvent;
				_acquisitionManager.AcquisitionCompletionStateEvent += OnAcquisitionCompletionStateEvent;
				_acquisitionManager.CurrentSampleNameEvent += OnCurrentSampleNameEvent;

				SendCurrentSampleNameToHubAsync(_acquisitionManager.CurrentSampleName);
				SendAcquisitionCompletionStateToHubAsync(_acquisitionManager.AcquisitionCompletionState);
				SendAcquisitionStateToHubAsync(_acquisitionManager.AcquisitionState);
			}
		}

		public void Stop()
		{
			lock (_padLock)
			{
				if (_deviceClient == null)
					return;

				_acquisitionManager.AcquisitionStateEvent -= OnAcquisitionStateEvent;
				_acquisitionManager.AcquisitionCompletionStateEvent -= OnAcquisitionCompletionStateEvent;
				_acquisitionManager.CurrentSampleNameEvent -= OnCurrentSampleNameEvent;

				_deviceClient.CloseAsync().GetAwaiter().GetResult();
				_deviceClient = null;
			}
		}

		private void SendCurrentSampleNameToHubAsync(string currentSampleName)
		{
			SendToHubAsync("CurrentSampleName", currentSampleName);
		}

		private void SendAcquisitionCompletionStateToHubAsync(AcquisitionCompletionState acquisitionCompletionState)
		{
			SendToHubAsync("AcquisitionCompletionState", acquisitionCompletionState.ToString());
		}

		private void SendAcquisitionStateToHubAsync(AcquisitionState acquisitionState)
		{
			SendToHubAsync("AcquisitionState", acquisitionState.ToString());
		}

		private void OnCurrentSampleNameEvent(object sender, EventArgs<string> eventArgs)
		{
			SendCurrentSampleNameToHubAsync(eventArgs.Parameter);
		}

		private void OnAcquisitionCompletionStateEvent(object sender, EventArgs<AcquisitionCompletionState> eventArgs)
		{
			SendAcquisitionCompletionStateToHubAsync(eventArgs.Parameter);
		}

		private void OnAcquisitionStateEvent(object sender, EventArgs<AcquisitionState> eventArgs)
		{
			SendAcquisitionStateToHubAsync(eventArgs.Parameter);
		}

		private void ParseConfiguration()
		{
			var assemblyLocation = GetType().Assembly.Location;
			var configuration = ConfigurationManager.OpenExeConfiguration(assemblyLocation);

			var deviceConnectionStringElement = configuration.AppSettings.Settings[DeviceConnectionStringKey];

			if (deviceConnectionStringElement == null)
			{
				Log.ErrorFormat("Configuration key for {0} can't be found", DeviceConnectionStringKey);
				return;
			}

			_deviceConnectionString = deviceConnectionStringElement.Value;
		}

		private async void SendToHubAsync<T>(string key, T value)
		{
			var valueJson = JsonConvert.SerializeObject(value);
			var message = new Message(Encoding.ASCII.GetBytes(valueJson));

			message.Properties.Add("Type", key);

			await _deviceClient.SendEventAsync(message);
		}
	}
}