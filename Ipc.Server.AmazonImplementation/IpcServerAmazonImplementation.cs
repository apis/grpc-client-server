using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using AcquisitionManager;
using log4net;
using Newtonsoft.Json;
using uPLibrary.Networking.M2Mqtt;

namespace Ipc.Server
{
	public class IpcServerAmazonImplementation : IIpcServer
	{
		private const string IotEndPointHostKey = "IotEndPointHost";
		private const string IotEndPointPortKey = "IotEndPointPort";
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IAcquisitionManager _acquisitionManager;
		private readonly X509Certificate _caCertificate;
		private readonly X509Certificate2 _deviceCertificate;
		private readonly object _padLock = new object();
		private MqttClient _deviceClient;
		private string _iotEndPointHost;
		private int _iotEndPointPort = -1;
		private readonly string _clientId;
		private const string MainTopic = "AcquisitionManager";

		public IpcServerAmazonImplementation(string clientId, IAcquisitionManager acquisitionManager)
		{
			_clientId = clientId;
			_acquisitionManager = acquisitionManager;

			ParseConfiguration();

			if (_iotEndPointHost == null || _iotEndPointPort == -1)
				throw new Exception("Invalid configuration!");

			var caCertificateBuffer = LoadFromResource("root.pem.crt");
			_caCertificate = new X509Certificate();
			_caCertificate.Import(caCertificateBuffer);

			var deviceCertificateBuffer = LoadFromResource("device.pfx.crt");
			_deviceCertificate = new X509Certificate2();
			_deviceCertificate.Import(deviceCertificateBuffer);
		}

		public void Start()
		{
			lock (_padLock)
			{
				if (_deviceClient != null)
					return;

				_deviceClient = new MqttClient(_iotEndPointHost, _iotEndPointPort, true, _caCertificate,
					_deviceCertificate,
					MqttSslProtocols.TLSv1_2);

				_deviceClient.Connect(_clientId);

				_acquisitionManager.AcquisitionStateEvent += OnAcquisitionStateEvent;
				_acquisitionManager.AcquisitionCompletionStateEvent += OnAcquisitionCompletionStateEvent;
				_acquisitionManager.CurrentSampleNameEvent += OnCurrentSampleNameEvent;

				SendCurrentSampleNameToCloud(_acquisitionManager.CurrentSampleName);
				SendAcquisitionCompletionStateToCloud(_acquisitionManager.AcquisitionCompletionState);
				SendAcquisitionStateToCloud(_acquisitionManager.AcquisitionState);
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

				_deviceClient.Disconnect();
				_deviceClient = null;
			}
		}

		private void SendCurrentSampleNameToCloud(string currentSampleName)
		{
			SendValueToCloud("CurrentSampleName", currentSampleName);
		}

		private void SendAcquisitionCompletionStateToCloud(AcquisitionCompletionState acquisitionCompletionState)
		{
			SendValueToCloud("AcquisitionCompletionState", acquisitionCompletionState.ToString());
		}

		private void SendAcquisitionStateToCloud(AcquisitionState acquisitionState)
		{
			SendValueToCloud("AcquisitionState", acquisitionState.ToString());
		}

		private void OnCurrentSampleNameEvent(object sender, EventArgs<string> eventArgs)
		{
			SendCurrentSampleNameToCloud(eventArgs.Parameter);
		}

		private void OnAcquisitionCompletionStateEvent(object sender, EventArgs<AcquisitionCompletionState> eventArgs)
		{
			SendAcquisitionCompletionStateToCloud(eventArgs.Parameter);
		}

		private void OnAcquisitionStateEvent(object sender, EventArgs<AcquisitionState> eventArgs)
		{
			SendAcquisitionStateToCloud(eventArgs.Parameter);
		}

		private void ParseConfiguration()
		{
			var assemblyLocation = GetType().Assembly.Location;
			var configuration = ConfigurationManager.OpenExeConfiguration(assemblyLocation);

			var iotEndPointHostElement = configuration.AppSettings.Settings[IotEndPointHostKey];

			if (iotEndPointHostElement == null)
			{
				Log.ErrorFormat("Configuration key for {0} can't be found", IotEndPointHostKey);
				return;
			}

			_iotEndPointHost = iotEndPointHostElement.Value;

			var iotEndPointPortElement = configuration.AppSettings.Settings[IotEndPointPortKey];

			if (iotEndPointPortElement == null)
			{
				Log.ErrorFormat("Configuration key for {0} can't be found", IotEndPointPortKey);
				return;
			}

			if (!int.TryParse(iotEndPointPortElement.Value, out var result))
			{
				Log.ErrorFormat("Configuration key value for {0} can't be parsed", IotEndPointPortKey);
				return;
			}

			_iotEndPointPort = result;
		}

		private void SendValueToCloud<T>(string key, T value)
		{
			var valueJson = JsonConvert.SerializeObject(value);
			var topic = MainTopic + "/" + key + "/" + _clientId;
			_deviceClient.Publish(topic, Encoding.UTF8.GetBytes(valueJson));
		}

		private static byte[] LoadFromResource(string resourceName)
		{
			byte[] buffer;
			using (var resourceStream =
				Assembly.GetExecutingAssembly()
					.GetManifestResourceStream("Ipc.Server." + resourceName))
			{
				if (resourceStream == null)
					throw new Exception("Something really wrong here!");

				using (var binaryReader = new BinaryReader(resourceStream))
				{
					buffer = binaryReader.ReadBytes((int) resourceStream.Length);
				}
			}

			return buffer;
		}
	}
}