using System.Threading;

namespace Ipc.Client
{
	public interface IIpcClient
	{
		void Start();

		void Stop();
	}
}
