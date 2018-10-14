using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipc.Client
{
	public interface IIpcClient
	{
		void Start();

		void Stop();

		object Channel { get; }
	}
}
