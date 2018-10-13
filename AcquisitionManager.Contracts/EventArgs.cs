using System;

namespace AcquisitionManager
{
	public class EventArgs<T> : EventArgs
	{
		public EventArgs(T parameter)
		{
			Parameter = parameter;
		}

		public T Parameter { get; set; }
	}

	public class EventArgs<T1, T2> : EventArgs
	{
		public EventArgs(T1 parameter1, T2 parameter2)
		{
			Parameter1 = parameter1;
			Parameter2 = parameter2;
		}

		public T1 Parameter1 { get; set; }

		public T2 Parameter2 { get; set; }
	}
}