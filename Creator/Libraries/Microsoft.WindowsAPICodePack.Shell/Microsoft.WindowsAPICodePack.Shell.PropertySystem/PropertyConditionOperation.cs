namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	public enum PropertyConditionOperation
	{
		Implicit,
		Equal,
		NotEqual,
		LessThan,
		GreaterThan,
		LessThanOrEqual,
		GreaterThanOrEqual,
		ValueStartsWith,
		ValueEndsWith,
		ValueContains,
		ValueNotContains,
		DOSWildCards,
		WordEqual,
		WordStartsWith,
		ApplicationSpecific
	}
}
