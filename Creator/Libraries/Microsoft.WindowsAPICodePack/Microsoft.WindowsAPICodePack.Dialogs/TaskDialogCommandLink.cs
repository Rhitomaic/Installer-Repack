using System;
using System.Globalization;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	public class TaskDialogCommandLink : TaskDialogButton
	{
		private string instruction;

		public string Instruction
		{
			get
			{
				return instruction;
			}
			set
			{
				instruction = value;
			}
		}

		public TaskDialogCommandLink()
		{
		}

		public TaskDialogCommandLink(string name, string text)
			: base(name, text)
		{
		}

		public TaskDialogCommandLink(string name, string text, string instruction)
			: base(name, text)
		{
			this.instruction = instruction;
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}", base.Text ?? string.Empty, (!string.IsNullOrEmpty(base.Text) && !string.IsNullOrEmpty(instruction)) ? Environment.NewLine : string.Empty, instruction ?? string.Empty);
		}
	}
}
