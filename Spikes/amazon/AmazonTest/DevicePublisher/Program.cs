using System;
using System.IO;
using System.Reflection;
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

			var caCertBuffer = LoadFromResource("root.pem.crt");
			var caCert = new X509Certificate();
			caCert.Import(caCertBuffer);

			var clientCertBuffer = LoadFromResource("ff52451764-certificate.pfx.crt");
			var clientCert = new X509Certificate2();
			clientCert.Import(clientCertBuffer);

			var testMessage = "Test message";
			var clientId = Guid.NewGuid().ToString();

			var iotClient = new MqttClient(iotEndPointHost, iotEndPointPort, true, caCert, clientCert,
				MqttSslProtocols.TLSv1_2);


			iotClient.Connect(clientId);
			Console.WriteLine("Connected");

			while (true)
			{
				iotClient.Publish(topic + "/" + clientId, Encoding.UTF8.GetBytes(testMessage));
				Console.WriteLine("published" + testMessage);
				Thread.Sleep(3000);
			}
		}

		private static byte[] LoadFromResource(string resourceName)
		{
			byte[] buffer;
			using (var resourceStream =
				Assembly.GetExecutingAssembly().GetManifestResourceStream("DevicePublisher." + resourceName))
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