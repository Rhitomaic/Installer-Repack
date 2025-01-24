using System;
using Microsoft.WindowsAPICodePack.Resources;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	public class TaskDialogProgressBar : TaskDialogBar
	{
		private int _minimum;

		private int _value;

		private int _maximum = 100;

		public int Minimum
		{
			get
			{
				return _minimum;
			}
			set
			{
				CheckPropertyChangeAllowed("Minimum");
				if (value < 0)
				{
					throw new ArgumentException(LocalizedMessages.TaskDialogProgressBarMinValueGreaterThanZero, "value");
				}
				if (value >= Maximum)
				{
					throw new ArgumentException(LocalizedMessages.TaskDialogProgressBarMinValueLessThanMax, "value");
				}
				_minimum = value;
				ApplyPropertyChange("Minimum");
			}
		}

		public int Maximum
		{
			get
			{
				return _maximum;
			}
			set
			{
				CheckPropertyChangeAllowed("Maximum");
				if (value < Minimum)
				{
					throw new ArgumentException(LocalizedMessages.TaskDialogProgressBarMaxValueGreaterThanMin, "value");
				}
				_maximum = value;
				ApplyPropertyChange("Maximum");
			}
		}

		public int Value
		{
			get
			{
				return _value;
			}
			set
			{
				CheckPropertyChangeAllowed("Value");
				if (value < Minimum || value > Maximum)
				{
					throw new ArgumentException(LocalizedMessages.TaskDialogProgressBarValueInRange, "value");
				}
				_value = value;
				ApplyPropertyChange("Value");
			}
		}

		internal bool HasValidValues => _minimum <= _value && _value <= _maximum;

		public TaskDialogProgressBar()
		{
		}

		public TaskDialogProgressBar(string name)
			: base(name)
		{
		}

		public TaskDialogProgressBar(int minimum, int maximum, int value)
		{
			Minimum = minimum;
			Maximum = maximum;
			Value = value;
		}

		protected internal override void Reset()
		{
			base.Reset();
			_value = _minimum;
		}
	}
}
