using System;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	public interface IShellProperty
	{
		PropertyKey PropertyKey { get; }

		ShellPropertyDescription Description { get; }

		string CanonicalName { get; }

		object ValueAsObject { get; }

		Type ValueType { get; }

		IconReference IconReference { get; }

		string FormatForDisplay(PropertyDescriptionFormatOptions format);
	}
}
