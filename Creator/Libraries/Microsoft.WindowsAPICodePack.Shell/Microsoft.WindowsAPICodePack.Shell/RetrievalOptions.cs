namespace Microsoft.WindowsAPICodePack.Shell
{
	internal enum RetrievalOptions
	{
		None = 0,
		Create = 32768,
		DontVerify = 16384,
		DontUnexpand = 8192,
		NoAlias = 4096,
		Init = 2048,
		DefaultPath = 1024,
		NotParentRelative = 512
	}
}
