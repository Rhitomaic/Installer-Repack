using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.WindowsAPICodePack.Shell.Interop;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	internal class MessageListener : IDisposable
	{
		public const uint CreateWindowMessage = 1025u;

		public const uint DestroyWindowMessage = 1026u;

		public const uint BaseUserMessage = 1029u;

		private const string MessageWindowClassName = "MessageListenerClass";

		private static readonly object _threadlock = new object();

		private static uint _atom;

		private static Thread _windowThread = null;

		private static volatile bool _running = false;

		private static ShellObjectWatcherNativeMethods.WndProcDelegate wndProc = WndProc;

		private static Dictionary<IntPtr, MessageListener> _listeners = new Dictionary<IntPtr, MessageListener>();

		private static IntPtr _firstWindowHandle = IntPtr.Zero;

		private static readonly object _crossThreadWindowLock = new object();

		private static IntPtr _tempHandle = IntPtr.Zero;

		public IntPtr WindowHandle { get; private set; }

		public static bool Running => _running;

		public event EventHandler<WindowMessageEventArgs> MessageReceived;

		public MessageListener()
		{
			lock (_threadlock)
			{
				if (_windowThread == null)
				{
					_windowThread = new Thread(ThreadMethod);
					_windowThread.SetApartmentState(ApartmentState.STA);
					_windowThread.Name = "ShellObjectWatcherMessageListenerHelperThread";
					lock (_crossThreadWindowLock)
					{
						_windowThread.Start();
						Monitor.Wait(_crossThreadWindowLock);
					}
					_firstWindowHandle = WindowHandle;
				}
				else
				{
					CrossThreadCreateWindow();
				}
				if (WindowHandle == IntPtr.Zero)
				{
					throw new ShellException(LocalizedMessages.MessageListenerCannotCreateWindow, Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error()));
				}
				_listeners.Add(WindowHandle, this);
			}
		}

		private void CrossThreadCreateWindow()
		{
			if (_firstWindowHandle == IntPtr.Zero)
			{
				throw new InvalidOperationException(LocalizedMessages.MessageListenerNoWindowHandle);
			}
			lock (_crossThreadWindowLock)
			{
				CoreNativeMethods.PostMessage(_firstWindowHandle, (WindowMessage)1025, IntPtr.Zero, IntPtr.Zero);
				Monitor.Wait(_crossThreadWindowLock);
			}
			WindowHandle = _tempHandle;
		}

		private static void RegisterWindowClass()
		{
			WindowClassEx windowClass = default(WindowClassEx);
			windowClass.ClassName = "MessageListenerClass";
			windowClass.WndProc = wndProc;
			windowClass.Size = (uint)Marshal.SizeOf(typeof(WindowClassEx));
			uint num = ShellObjectWatcherNativeMethods.RegisterClassEx(ref windowClass);
			if (num == 0)
			{
				throw new ShellException(LocalizedMessages.MessageListenerClassNotRegistered, Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error()));
			}
			_atom = num;
		}

		private static IntPtr CreateWindow()
		{
			return ShellObjectWatcherNativeMethods.CreateWindowEx(0, "MessageListenerClass", "MessageListenerWindow", 0, 0, 0, 0, 0, new IntPtr(-3), IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
		}

		private void ThreadMethod()
		{
			lock (_crossThreadWindowLock)
			{
				_running = true;
				if (_atom == 0)
				{
					RegisterWindowClass();
				}
				WindowHandle = CreateWindow();
				Monitor.Pulse(_crossThreadWindowLock);
			}
			while (_running)
			{
				if (ShellObjectWatcherNativeMethods.GetMessage(out var message, IntPtr.Zero, 0u, 0u))
				{
					ShellObjectWatcherNativeMethods.DispatchMessage(ref message);
				}
			}
		}

		private static int WndProc(IntPtr hwnd, uint msg, IntPtr wparam, IntPtr lparam)
		{
			switch (msg)
			{
			case 1025u:
				lock (_crossThreadWindowLock)
				{
					_tempHandle = CreateWindow();
					Monitor.Pulse(_crossThreadWindowLock);
				}
				break;
			default:
			{
				if (_listeners.TryGetValue(hwnd, out var value))
				{
					Message msg2 = new Message(hwnd, msg, wparam, lparam, 0, default(NativePoint));
					value.MessageReceived.SafeRaise(value, new WindowMessageEventArgs(msg2));
				}
				break;
			}
			case 2u:
				break;
			}
			return ShellObjectWatcherNativeMethods.DefWindowProc(hwnd, msg, wparam, lparam);
		}

		~MessageListener()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			lock (_threadlock)
			{
				_listeners.Remove(WindowHandle);
				if (_listeners.Count == 0)
				{
					CoreNativeMethods.PostMessage(WindowHandle, WindowMessage.Destroy, IntPtr.Zero, IntPtr.Zero);
				}
			}
		}
	}
}
