using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;

namespace EventHubTest
{
	internal class ReadDeviceToCloudMessages
	{
		protected internal const string ConnectionString =
			"Endpoint=sb://iothub-ns-odessa-1015867-2100a2dabd.servicebus.windows.net/;SharedAccessKeyName=iothubowner;SharedAccessKey=RhmTqYzz7TYmIJ85MeGNMCBZ2w2CNIfE0JJJGGS51qs=;EntityPath=odessa";

		private static EventHubClient _eventHubClient;

		private static async Task ReceiveMessagesFromDeviceAsync(string partition, CancellationToken ct)
		{
			var eventHubReceiver =
				_eventHubClient.CreateReceiver("$Default", partition, EventPosition.FromEnqueuedTime(DateTime.Now));
			//			Console.WriteLine("Create receiver on partition: " + partition);

			while (true)
			{
				if (ct.IsCancellationRequested) break;
//				Console.WriteLine("Listening for messages on: " + partition);
				var events = await eventHubReceiver.ReceiveAsync(30, TimeSpan.FromSeconds(15));

				if (events == null)
					continue;

				foreach (var eventData in events)
				{
					var data = Encoding.UTF8.GetString(eventData.Body.Array);
					Console.WriteLine(">>> Message received on partition {0}:", partition);
					Console.WriteLine("  {0}:", data);
					Console.WriteLine("Application properties (set by device):");
					foreach (var prop in eventData.Properties) Console.WriteLine("  {0}: {1}", prop.Key, prop.Value);
					Console.WriteLine("System properties (set by IoT Hub):");
					foreach (var prop in eventData.SystemProperties)
						Console.WriteLine("  {0}: {1}", prop.Key, prop.Value);
				}
			}
		}

		private static void Main(string[] args)
		{
			Console.WriteLine("Read IoT Hub device to cloud messages. Ctrl-C to exit.\n");

			_eventHubClient = EventHubClient.CreateFromConnectionString(ConnectionString);

			var runtimeInfo = _eventHubClient.GetRuntimeInformationAsync().GetAwaiter().GetResult();
			var partitions = runtimeInfo.PartitionIds;

			var cts = new CancellationTokenSource();

			Console.CancelKeyPress += (s, e) =>
			{
				e.Cancel = true;
				cts.Cancel();
				Console.WriteLine("Exiting...");
			};

			var tasks = new List<Task>();
			foreach (var partition in partitions)
				tasks.Add(ReceiveMessagesFromDeviceAsync(partition, cts.Token));

			Task.WaitAll(tasks.ToArray());
		}
	}
}