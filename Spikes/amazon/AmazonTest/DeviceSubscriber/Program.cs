using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace DeviceSubscriber
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			var iotEndPointHost = "a1glj8i4w7qgnc-ats.iot.us-west-2.amazonaws.com";
			var iotEndPointPort = 8883;
			var topic = "AcquisitionServer/MyState";

			var caCert = X509Certificate.CreateFromCertFile(@"C:\sandbox\Spikes\amazon\root.pem.crt");
			var clientCert = new X509Certificate2(@"C:\sandbox\Spikes\amazon\ff52451764-certificate.pfx.crt");

			var clientId = Guid.NewGuid().ToString();

			var iotClient = new MqttClient(iotEndPointHost, iotEndPointPort, true, caCert, clientCert,
				MqttSslProtocols.TLSv1_2);
			iotClient.MqttMsgPublishReceived += OnMqttMsgPublishReceived;
			iotClient.MqttMsgSubscribed += OnMqttMsgSubscribed;

			iotClient.Connect(clientId);
			Console.WriteLine("Connected");
			iotClient.Subscribe(new[] {topic}, new[] {MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE});

			while (true)
			{
			}
		}

		private static void OnMqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs eventArgs)
		{
			Console.WriteLine("Message subscribed");
		}

		private static void OnMqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs eventArgs)
		{
			Console.WriteLine("Message Received is      " + Encoding.UTF8.GetString(eventArgs.Message));
		}
	}
}