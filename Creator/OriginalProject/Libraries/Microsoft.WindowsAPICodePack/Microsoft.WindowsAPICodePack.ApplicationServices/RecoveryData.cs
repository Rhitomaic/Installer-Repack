namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	public class RecoveryData
	{
		public RecoveryCallback Callback { get; set; }

		public object State { get; set; }

		public RecoveryData(RecoveryCallback callback, object state)
		{
			Callback = callback;
			State = state;
		}

		public void Invoke()
		{
			if (Callback != null)
			{
				Callback(State);
			}
		}
	}
}
