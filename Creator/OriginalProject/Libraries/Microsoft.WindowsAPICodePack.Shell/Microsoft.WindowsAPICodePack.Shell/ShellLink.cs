using System;

namespace Microsoft.WindowsAPICodePack.Shell
{
	public class ShellLink : ShellObject
	{
		private string _internalPath;

		private string internalTargetLocation;

		private string internalArguments;

		private string internalComments;

		public virtual string Path
		{
			get
			{
				if (_internalPath == null && NativeShellItem != null)
				{
					_internalPath = base.ParsingName;
				}
				return _internalPath;
			}
			protected set
			{
				_internalPath = value;
			}
		}

		public string TargetLocation
		{
			get
			{
				if (string.IsNullOrEmpty(internalTargetLocation) && NativeShellItem2 != null)
				{
					internalTargetLocation = base.Properties.System.Link.TargetParsingPath.Value;
				}
				return internalTargetLocation;
			}
			set
			{
				if (value != null)
				{
					internalTargetLocation = value;
					if (NativeShellItem2 != null)
					{
						base.Properties.System.Link.TargetParsingPath.Value = internalTargetLocation;
					}
				}
			}
		}

		public ShellObject TargetShellObject => ShellObjectFactory.Create(TargetLocation);

		public string Title
		{
			get
			{
				if (NativeShellItem2 != null)
				{
					return base.Properties.System.Title.Value;
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (NativeShellItem2 != null)
				{
					base.Properties.System.Title.Value = value;
				}
			}
		}

		public string Arguments
		{
			get
			{
				if (string.IsNullOrEmpty(internalArguments) && NativeShellItem2 != null)
				{
					internalArguments = base.Properties.System.Link.Arguments.Value;
				}
				return internalArguments;
			}
		}

		public string Comments
		{
			get
			{
				if (string.IsNullOrEmpty(internalComments) && NativeShellItem2 != null)
				{
					internalComments = base.Properties.System.Comment.Value;
				}
				return internalComments;
			}
		}

		internal ShellLink(IShellItem2 shellItem)
		{
			nativeShellItem = shellItem;
		}
	}
}
