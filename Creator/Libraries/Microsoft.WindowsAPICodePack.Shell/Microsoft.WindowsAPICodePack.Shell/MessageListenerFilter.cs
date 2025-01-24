using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAPICodePack.Shell.Resources;

namespace Microsoft.WindowsAPICodePack.Shell
{
	internal static class MessageListenerFilter
	{
		private class RegisteredListener
		{
			private uint _lastMessage = 1029u;

			public Dictionary<uint, Action<WindowMessageEventArgs>> Callbacks { get; private set; }

			public MessageListener Listener { get; private set; }

			public RegisteredListener()
			{
				Callbacks = new Dictionary<uint, Action<WindowMessageEventArgs>>();
				Listener = new MessageListener();
				Listener.MessageReceived += MessageReceived;
			}

			private void MessageReceived(object sender, WindowMessageEventArgs e)
			{
				if (Callbacks.TryGetValue(e.Message.Msg, out var value))
				{
					value(e);
				}
			}

			public bool TryRegister(Action<WindowMessageEventArgs> callback, out uint message)
			{
				message = 0u;
				if ((long)Callbacks.Count < 64506L)
				{
					for (uint num = _lastMessage + 1; num != _lastMessage; num++)
					{
						if (num > 65535)
						{
							num = 1029u;
						}
						if (!Callbacks.ContainsKey(num))
						{
							_lastMessage = (message = num);
							Callbacks.Add(num, callback);
							return true;
						}
					}
				}
				return false;
			}
		}

		private static readonly object _registerLock = new object();

		private static List<RegisteredListener> _packages = new List<RegisteredListener>();

		public static MessageListenerFilterRegistrationResult Register(Action<WindowMessageEventArgs> callback)
		{
			lock (_registerLock)
			{
				uint message = 0u;
				RegisteredListener registeredListener = _packages.FirstOrDefault((RegisteredListener x) => x.TryRegister(callback, out message));
				if (registeredListener == null)
				{
					registeredListener = new RegisteredListener();
					if (!registeredListener.TryRegister(callback, out message))
					{
						throw new ShellException(LocalizedMessages.MessageListenerFilterUnableToRegister);
					}
					_packages.Add(registeredListener);
				}
				return new MessageListenerFilterRegistrationResult(registeredListener.Listener.WindowHandle, message);
			}
		}

		public static void Unregister(IntPtr listenerHandle, uint message)
		{
			lock (_registerLock)
			{
				RegisteredListener registeredListener = _packages.FirstOrDefault((RegisteredListener x) => x.Listener.WindowHandle == listenerHandle);
				if (registeredListener == null || !registeredListener.Callbacks.Remove(message))
				{
					throw new ArgumentException(LocalizedMessages.MessageListenerFilterUnknownListenerHandle);
				}
				if (registeredListener.Callbacks.Count == 0)
				{
					registeredListener.Listener.Dispose();
					_packages.Remove(registeredListener);
				}
			}
		}
	}
}
