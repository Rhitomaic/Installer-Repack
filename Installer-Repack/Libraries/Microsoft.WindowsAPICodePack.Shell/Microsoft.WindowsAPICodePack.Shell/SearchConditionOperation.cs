namespace Microsoft.WindowsAPICodePack.Shell
{
	public enum SearchConditionOperation
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
		DosWildcards,
		WordEqual,
		WordStartsWith,
		ApplicationSpecific
	}
}
