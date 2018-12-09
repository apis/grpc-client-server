using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;

namespace DevicePublisher
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

			var testMessage = "Test message";
			var clientId = Guid.NewGuid().ToString();

			var iotClient = new MqttClient(iotEndPointHost, iotEndPointPort, true, caCert, clientCert,
				MqttSslProtocols.TLSv1_2);


			iotClient.Connect(clientId);
			Console.WriteLine("Connected");

			while (true)
			{
				iotClient.Publish(topic, Encoding.UTF8.GetBytes(testMessage));
				Console.WriteLine("published" + testMessage);
				Thread.Sleep(3000);
			}
		}
	}
}