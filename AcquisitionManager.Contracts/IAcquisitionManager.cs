using System;

namespace AcquisitionManager
{
	public interface IAcquisitionManager
	{
		bool Start(string sampleName);

		bool Stop();

		string CurrentSampleName { get; }

		AcquisitionState AcquisitionState { get; }

		AcquisitionCompletionState AcquisitionCompletionState { get; }

		event EventHandler<EventArgs<string>> CurrentSampleNameEvent;

		event EventHandler<EventArgs<AcquisitionState>> AcquisitionStateEvent;

		event EventHandler<EventArgs<AcquisitionCompletionState>> AcquisitionCompletionStateEvent;
	}
}