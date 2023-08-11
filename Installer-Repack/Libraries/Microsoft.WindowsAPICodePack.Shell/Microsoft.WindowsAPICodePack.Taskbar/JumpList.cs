using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	public class JumpList
	{
		private readonly object syncLock = new object();

		private ICustomDestinationList customDestinationList;

		private JumpListCustomCategoryCollection customCategoriesCollection;

		private JumpListItemCollection<JumpListTask> userTasks;

		private int knownCategoryOrdinalPosition;

		public uint MaxSlotsInList
		{
			get
			{
				uint cMaxSlots = 10u;
				object ppvObject;
				HResult result = customDestinationList.BeginList(out cMaxSlots, ref TaskbarNativeMethods.TaskbarGuids.IObjectArray, out ppvObject);
				if (CoreErrorHelper.Succeeded(result))
				{
					customDestinationList.AbortList();
				}
				return cMaxSlots;
			}
		}

		public JumpListKnownCategoryType KnownCategoryToDisplay { get; set; }

		public int KnownCategoryOrdinalPosition
		{
			get
			{
				return knownCategoryOrdinalPosition;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value", LocalizedMessages.JumpListNegativeOrdinalPosition);
				}
				knownCategoryOrdinalPosition = value;
			}
		}

		public string ApplicationId { get; private set; }

		public IEnumerable RemovedDestinations
		{
			get
			{
				customDestinationList.GetRemovedDestinations(ref TaskbarNativeMethods.TaskbarGuids.IObjectArray, out var ppvObject);
				return ProcessDeletedItems((IObjectArray)ppvObject);
			}
		}

		public event EventHandler<UserRemovedJumpListItemsEventArgs> JumpListItemsRemoved = delegate
		{
		};

		public static JumpList CreateJumpList()
		{
			return new JumpList(TaskbarManager.Instance.ApplicationId);
		}

		public static JumpList CreateJumpListForIndividualWindow(string appId, IntPtr windowHandle)
		{
			return new JumpList(appId, windowHandle);
		}

		public static JumpList CreateJumpListForIndividualWindow(string appId, Window window)
		{
			return new JumpList(appId, window);
		}

		public void AddCustomCategories(params JumpListCustomCategory[] customCategories)
		{
			lock (syncLock)
			{
				if (customCategoriesCollection == null)
				{
					customCategoriesCollection = new JumpListCustomCategoryCollection();
				}
			}
			if (customCategories != null)
			{
				foreach (JumpListCustomCategory category in customCategories)
				{
					customCategoriesCollection.Add(category);
				}
			}
		}

		public void AddUserTasks(params JumpListTask[] tasks)
		{
			if (userTasks == null)
			{
				lock (syncLock)
				{
					if (userTasks == null)
					{
						userTasks = new JumpListItemCollection<JumpListTask>();
					}
				}
			}
			if (tasks != null)
			{
				foreach (JumpListTask item in tasks)
				{
					userTasks.Add(item);
				}
			}
		}

		public void ClearAllUserTasks()
		{
			if (userTasks != null)
			{
				userTasks.Clear();
			}
		}

		internal JumpList(string appID)
			: this(appID, TaskbarManager.Instance.OwnerHandle)
		{
		}

		internal JumpList(string appID, Window window)
			: this(appID, new WindowInteropHelper(window).Handle)
		{
		}

		private JumpList(string appID, IntPtr windowHandle)
		{
			CoreHelpers.ThrowIfNotWin7();
			customDestinationList = (ICustomDestinationList)new CDestinationList();
			if (!string.IsNullOrEmpty(appID))
			{
				ApplicationId = appID;
				if (!TaskbarManager.Instance.ApplicationIdSetProcessWide)
				{
					TaskbarManager.Instance.ApplicationId = appID;
				}
				TaskbarManager.Instance.SetApplicationIdForSpecificWindow(windowHandle, appID);
			}
		}

		public static void AddToRecent(string destination)
		{
			TaskbarNativeMethods.SHAddToRecentDocs(destination);
		}

		public void Refresh()
		{
			if (!string.IsNullOrEmpty(ApplicationId))
			{
				customDestinationList.SetAppID(ApplicationId);
			}
			BeginList();
			Exception ex = null;
			try
			{
				AppendTaskList();
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			try
			{
				AppendCustomCategories();
			}
			finally
			{
				customDestinationList.CommitList();
			}
			if (ex != null)
			{
				throw ex;
			}
		}

		private void BeginList()
		{
			uint cMaxSlots = 10u;
			object ppvObject;
			HResult result = customDestinationList.BeginList(out cMaxSlots, ref TaskbarNativeMethods.TaskbarGuids.IObjectArray, out ppvObject);
			if (!CoreErrorHelper.Succeeded(result))
			{
				throw new ShellException(result);
			}
			IEnumerable enumerable = ProcessDeletedItems((IObjectArray)ppvObject);
			if (this.JumpListItemsRemoved != null && enumerable != null && enumerable.GetEnumerator().MoveNext())
			{
				this.JumpListItemsRemoved(this, new UserRemovedJumpListItemsEventArgs(enumerable));
			}
		}

		private IEnumerable<string> ProcessDeletedItems(IObjectArray removedItems)
		{
			List<string> list = new List<string>();
			removedItems.GetCount(out var cObjects);
			for (uint num = 0u; num < cObjects; num++)
			{
				removedItems.GetAt(num, ref TaskbarNativeMethods.TaskbarGuids.IUnknown, out var ppvObject);
				if (ppvObject is IShellItem item)
				{
					list.Add(RemoveCustomCategoryItem(item));
				}
				else if (ppvObject is IShellLinkW link)
				{
					list.Add(RemoveCustomCategoryLink(link));
				}
			}
			return list;
		}

		private string RemoveCustomCategoryItem(IShellItem item)
		{
			string text = null;
			if (customCategoriesCollection != null)
			{
				IntPtr ppszName = IntPtr.Zero;
				if (item.GetDisplayName(ShellNativeMethods.ShellItemDesignNameOptions.FileSystemPath, out ppszName) == HResult.Ok && ppszName != IntPtr.Zero)
				{
					text = Marshal.PtrToStringAuto(ppszName);
					Marshal.FreeCoTaskMem(ppszName);
				}
				foreach (JumpListCustomCategory item2 in (IEnumerable<JumpListCustomCategory>)customCategoriesCollection)
				{
					item2.RemoveJumpListItem(text);
				}
			}
			return text;
		}

		private string RemoveCustomCategoryLink(IShellLinkW link)
		{
			string text = null;
			if (customCategoriesCollection != null)
			{
				StringBuilder stringBuilder = new StringBuilder(256);
				link.GetPath(stringBuilder, stringBuilder.Capacity, IntPtr.Zero, 2u);
				text = stringBuilder.ToString();
				foreach (JumpListCustomCategory item in (IEnumerable<JumpListCustomCategory>)customCategoriesCollection)
				{
					item.RemoveJumpListItem(text);
				}
			}
			return text;
		}

		private void AppendCustomCategories()
		{
			int num = 0;
			bool flag = false;
			if (customCategoriesCollection != null)
			{
				foreach (JumpListCustomCategory item in (IEnumerable<JumpListCustomCategory>)customCategoriesCollection)
				{
					if (!flag && num == KnownCategoryOrdinalPosition)
					{
						AppendKnownCategories();
						flag = true;
					}
					if (item.JumpListItems.Count == 0)
					{
						continue;
					}
					IObjectCollection objectCollection = (IObjectCollection)new CEnumerableObjectCollection();
					foreach (IJumpListItem item2 in (IEnumerable<IJumpListItem>)item.JumpListItems)
					{
						JumpListItem jumpListItem = item2 as JumpListItem;
						JumpListLink jumpListLink = item2 as JumpListLink;
						if (jumpListItem != null)
						{
							objectCollection.AddObject(jumpListItem.NativeShellItem);
						}
						else if (jumpListLink != null)
						{
							objectCollection.AddObject(jumpListLink.NativeShellLink);
						}
					}
					HResult hResult = customDestinationList.AppendCategory(item.Name, (IObjectArray)objectCollection);
					if (!CoreErrorHelper.Succeeded(hResult))
					{
						switch (hResult)
						{
						case (HResult)(-2147217661):
							throw new InvalidOperationException(LocalizedMessages.JumpListFileTypeNotRegistered);
						case (HResult)(-2147024891):
							throw new UnauthorizedAccessException(LocalizedMessages.JumpListCustomCategoriesDisabled);
						default:
							throw new ShellException(hResult);
						}
					}
					num++;
				}
			}
			if (!flag)
			{
				AppendKnownCategories();
			}
		}

		private void AppendTaskList()
		{
			if (userTasks == null || userTasks.Count == 0)
			{
				return;
			}
			IObjectCollection objectCollection = (IObjectCollection)new CEnumerableObjectCollection();
			foreach (JumpListTask item in (IEnumerable<JumpListTask>)userTasks)
			{
				if (item is JumpListLink jumpListLink)
				{
					objectCollection.AddObject(jumpListLink.NativeShellLink);
				}
				else if (item is JumpListSeparator jumpListSeparator)
				{
					objectCollection.AddObject(jumpListSeparator.NativeShellLink);
				}
			}
			HResult hResult = customDestinationList.AddUserTasks((IObjectArray)objectCollection);
			if (CoreErrorHelper.Succeeded(hResult))
			{
				return;
			}
			if (hResult == (HResult)(-2147217661))
			{
				throw new InvalidOperationException(LocalizedMessages.JumpListFileTypeNotRegistered);
			}
			throw new ShellException(hResult);
		}

		private void AppendKnownCategories()
		{
			if (KnownCategoryToDisplay == JumpListKnownCategoryType.Recent)
			{
				customDestinationList.AppendKnownCategory(KnownDestinationCategory.Recent);
			}
			else if (KnownCategoryToDisplay == JumpListKnownCategoryType.Frequent)
			{
				customDestinationList.AppendKnownCategory(KnownDestinationCategory.Frequent);
			}
		}
	}
}
