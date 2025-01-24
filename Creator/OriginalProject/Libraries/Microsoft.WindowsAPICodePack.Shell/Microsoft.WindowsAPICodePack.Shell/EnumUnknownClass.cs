using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	internal class EnumUnknownClass : IEnumUnknown
	{
		private List<ICondition> conditionList = new List<ICondition>();

		private int current = -1;

		internal EnumUnknownClass(ICondition[] conditions)
		{
			conditionList.AddRange(conditions);
		}

		public HResult Next(uint requestedNumber, ref IntPtr buffer, ref uint fetchedNumber)
		{
			current++;
			if (current < conditionList.Count)
			{
				buffer = Marshal.GetIUnknownForObject(conditionList[current]);
				fetchedNumber = 1u;
				return HResult.Ok;
			}
			return HResult.False;
		}

		public HResult Skip(uint number)
		{
			int num = current + (int)number;
			if (num > conditionList.Count - 1)
			{
				return HResult.False;
			}
			current = num;
			return HResult.Ok;
		}

		public HResult Reset()
		{
			current = -1;
			return HResult.Ok;
		}

		public HResult Clone(out IEnumUnknown result)
		{
			result = new EnumUnknownClass(conditionList.ToArray());
			return HResult.Ok;
		}
	}
}
