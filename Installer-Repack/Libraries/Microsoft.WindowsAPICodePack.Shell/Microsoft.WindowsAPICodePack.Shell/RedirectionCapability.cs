namespace Microsoft.WindowsAPICodePack.Shell
{
	public enum RedirectionCapability
	{
		None = 0,
		AllowAll = 255,
		Redirectable = 1,
		DenyAll = 1048320,
		DenyPolicyRedirected = 256,
		DenyPolicy = 512,
		DenyPermissions = 1024
	}
}
