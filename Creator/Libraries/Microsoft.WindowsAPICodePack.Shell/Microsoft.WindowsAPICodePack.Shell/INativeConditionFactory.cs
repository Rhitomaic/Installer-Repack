using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[ComImport]
	[Guid("A5EFE073-B16F-474f-9F3E-9F8B497A3E08")]
	[CoClass(typeof(ConditionFactoryCoClass))]
	internal interface INativeConditionFactory : IConditionFactory
	{
	}
}
