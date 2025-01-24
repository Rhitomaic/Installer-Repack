using System.Globalization;
using Microsoft.WindowsAPICodePack.Resources;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	public class RestartSettings
	{
		private string command;

		private RestartRestrictions restrictions;

		public string Command => command;

		public RestartRestrictions Restrictions => restrictions;

		public RestartSettings(string command, RestartRestrictions restrictions)
		{
			this.command = command;
			this.restrictions = restrictions;
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, LocalizedMessages.RestartSettingsFormatString, command, restrictions.ToString());
		}
	}
}
