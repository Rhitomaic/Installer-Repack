using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Resources;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	internal static class MessageManager
	{
		internal class PowerRegWindow : Form
		{
			private Hashtable eventList = new Hashtable();

			private ReaderWriterLock readerWriterLock = new ReaderWriterLock();

			internal PowerRegWindow()
			{
			}

			internal void RegisterPowerEvent(Guid eventId, EventHandler eventToRegister)
			{
				readerWriterLock.AcquireWriterLock(-1);
				if (!eventList.Contains(eventId))
				{
					Power.RegisterPowerSettingNotification(base.Handle, eventId);
					ArrayList arrayList = new ArrayList();
					arrayList.Add(eventToRegister);
					eventList.Add(eventId, arrayList);
				}
				else
				{
					ArrayList arrayList2 = (ArrayList)eventList[eventId];
					arrayList2.Add(eventToRegister);
				}
				readerWriterLock.ReleaseWriterLock();
			}

			internal void UnregisterPowerEvent(Guid eventId, EventHandler eventToUnregister)
			{
				readerWriterLock.AcquireWriterLock(-1);
				if (eventList.Contains(eventId))
				{
					ArrayList arrayList = (ArrayList)eventList[eventId];
					arrayList.Remove(eventToUnregister);
					readerWriterLock.ReleaseWriterLock();
					return;
				}
				throw new InvalidOperationException(LocalizedMessages.MessageManagerHandlerNotRegistered);
			}

			private static void ExecuteEvents(ArrayList eventHandlerList)
			{
				foreach (EventHandler eventHandler in eventHandlerList)
				{
					eventHandler(null, new EventArgs());
				}
			}

			protected override void WndProc(ref Message m)
			{
				if ((long)m.Msg == 536 && (long)(int)m.WParam == 32787)
				{
					PowerManagementNativeMethods.PowerBroadcastSetting powerBroadcastSetting = (PowerManagementNativeMethods.PowerBroadcastSetting)Marshal.PtrToStructure(m.LParam, typeof(PowerManagementNativeMethods.PowerBroadcastSetting));
					IntPtr ptr = new IntPtr(m.LParam.ToInt64() + Marshal.SizeOf(powerBroadcastSetting));
					Guid powerSetting = powerBroadcastSetting.PowerSetting;
					if (powerBroadcastSetting.PowerSetting == EventManager.MonitorPowerStatus && powerBroadcastSetting.DataLength == Marshal.SizeOf(typeof(int)))
					{
						int num = (int)Marshal.PtrToStructure(ptr, typeof(int));
						PowerManager.IsMonitorOn = num != 0;
						EventManager.monitorOnReset.Set();
					}
					if (!EventManager.IsMessageCaught(powerSetting))
					{
						ExecuteEvents((ArrayList)eventList[powerSetting]);
					}
				}
				else
				{
					base.WndProc(ref m);
				}
			}
		}

		private static object lockObject = new object();

		private static PowerRegWindow window;

		internal static void RegisterPowerEvent(Guid eventId, EventHandler eventToRegister)
		{
			EnsureInitialized();
			window.RegisterPowerEvent(eventId, eventToRegister);
		}

		internal static void UnregisterPowerEvent(Guid eventId, EventHandler eventToUnregister)
		{
			EnsureInitialized();
			window.UnregisterPowerEvent(eventId, eventToUnregister);
		}

		private static void EnsureInitialized()
		{
			lock (lockObject)
			{
				if (window == null)
				{
					window = new PowerRegWindow();
				}
			}
		}
	}
}
