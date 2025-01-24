namespace Microsoft.WindowsAPICodePack.Taskbar
{
	internal static class TaskbarList
	{
		private static object _syncLock = new object();

		private static ITaskbarList4 _taskbarList;

		internal static ITaskbarList4 Instance
		{
			get
			{
				if (_taskbarList == null)
				{
					lock (_syncLock)
					{
						if (_taskbarList == null)
						{
							_taskbarList = (ITaskbarList4)new CTaskbarList();
							_taskbarList.HrInit();
						}
					}
				}
				return _taskbarList;
			}
		}
	}
}
