using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Resources;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PropertyKey : IEquatable<PropertyKey>
	{
		private Guid formatId;

		private int propertyId;

		public Guid FormatId => formatId;

		public int PropertyId => propertyId;

		public PropertyKey(Guid formatId, int propertyId)
		{
			this.formatId = formatId;
			this.propertyId = propertyId;
		}

		public PropertyKey(string formatId, int propertyId)
		{
			this.formatId = new Guid(formatId);
			this.propertyId = propertyId;
		}

		public bool Equals(PropertyKey other)
		{
			return other.Equals((object)this);
		}

		public override int GetHashCode()
		{
			return formatId.GetHashCode() ^ propertyId;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!(obj is PropertyKey propertyKey))
			{
				return false;
			}
			return propertyKey.formatId.Equals(formatId) && propertyKey.propertyId == propertyId;
		}

		public static bool operator ==(PropertyKey propKey1, PropertyKey propKey2)
		{
			return propKey1.Equals(propKey2);
		}

		public static bool operator !=(PropertyKey propKey1, PropertyKey propKey2)
		{
			return !propKey1.Equals(propKey2);
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, LocalizedMessages.PropertyKeyFormatString, formatId.ToString("B"), propertyId);
		}
	}
}
