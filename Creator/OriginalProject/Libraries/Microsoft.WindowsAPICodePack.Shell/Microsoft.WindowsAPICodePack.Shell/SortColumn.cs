using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace Microsoft.WindowsAPICodePack.Shell
{
	public struct SortColumn
	{
		private PropertyKey propertyKey;

		private SortDirection direction;

		public PropertyKey PropertyKey
		{
			get
			{
				return propertyKey;
			}
			set
			{
				propertyKey = value;
			}
		}

		public SortDirection Direction
		{
			get
			{
				return direction;
			}
			set
			{
				direction = value;
			}
		}

		public SortColumn(PropertyKey propertyKey, SortDirection direction)
		{
			this = default(SortColumn);
			this.propertyKey = propertyKey;
			this.direction = direction;
		}

		public static bool operator ==(SortColumn col1, SortColumn col2)
		{
			return col1.direction == col2.direction && col1.propertyKey == col2.propertyKey;
		}

		public static bool operator !=(SortColumn col1, SortColumn col2)
		{
			return !(col1 == col2);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != typeof(SortColumn))
			{
				return false;
			}
			return this == (SortColumn)obj;
		}

		public override int GetHashCode()
		{
			int hashCode = direction.GetHashCode();
			return hashCode * 31 + propertyKey.GetHashCode();
		}
	}
}
