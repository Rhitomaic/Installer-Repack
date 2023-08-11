using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	public class SearchCondition : IDisposable
	{
		private string canonicalName;

		private PropertyKey propertyKey;

		private PropertyKey emptyPropertyKey = default(PropertyKey);

		private SearchConditionOperation conditionOperation = SearchConditionOperation.Implicit;

		private SearchConditionType conditionType = SearchConditionType.Leaf;

		internal ICondition NativeSearchCondition { get; set; }

		public string PropertyCanonicalName => canonicalName;

		public PropertyKey PropertyKey
		{
			get
			{
				if (propertyKey == emptyPropertyKey)
				{
					int num = PropertySystemNativeMethods.PSGetPropertyKeyFromName(PropertyCanonicalName, out propertyKey);
					if (!CoreErrorHelper.Succeeded(num))
					{
						throw new ShellException(num);
					}
				}
				return propertyKey;
			}
		}

		public string PropertyValue { get; internal set; }

		public SearchConditionOperation ConditionOperation => conditionOperation;

		public SearchConditionType ConditionType => conditionType;

		internal SearchCondition(ICondition nativeSearchCondition)
		{
			if (nativeSearchCondition == null)
			{
				throw new ArgumentNullException("nativeSearchCondition");
			}
			NativeSearchCondition = nativeSearchCondition;
			HResult result = NativeSearchCondition.GetConditionType(out conditionType);
			if (!CoreErrorHelper.Succeeded(result))
			{
				throw new ShellException(result);
			}
			if (ConditionType != SearchConditionType.Leaf)
			{
				return;
			}
			using (PropVariant propVariant = new PropVariant())
			{
				result = NativeSearchCondition.GetComparisonInfo(out canonicalName, out conditionOperation, propVariant);
				if (!CoreErrorHelper.Succeeded(result))
				{
					throw new ShellException(result);
				}
				PropertyValue = propVariant.Value.ToString();
			}
		}

		public IEnumerable<SearchCondition> GetSubConditions()
		{
			List<SearchCondition> list = new List<SearchCondition>();
			Guid riid = new Guid("00000100-0000-0000-C000-000000000046");
			object ppv;
			HResult hResult = NativeSearchCondition.GetSubConditions(ref riid, out ppv);
			if (!CoreErrorHelper.Succeeded(hResult))
			{
				throw new ShellException(hResult);
			}
			if (ppv != null)
			{
				IEnumUnknown enumUnknown = ppv as IEnumUnknown;
				IntPtr buffer = IntPtr.Zero;
				uint fetchedNumber = 0u;
				while (hResult == HResult.Ok)
				{
					hResult = enumUnknown.Next(1u, ref buffer, ref fetchedNumber);
					if (hResult == HResult.Ok && fetchedNumber == 1)
					{
						list.Add(new SearchCondition((ICondition)Marshal.GetObjectForIUnknown(buffer)));
					}
				}
			}
			return list;
		}

		~SearchCondition()
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
			if (NativeSearchCondition != null)
			{
				Marshal.ReleaseComObject(NativeSearchCondition);
				NativeSearchCondition = null;
			}
		}
	}
}
