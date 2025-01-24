using System;
using System.Globalization;
using Microsoft.WindowsAPICodePack.Resources;

namespace Microsoft.WindowsAPICodePack.Shell
{
	public struct IconReference
	{
		private string moduleName;

		private string referencePath;

		private static char[] commaSeparator = new char[1] { ',' };

		public string ModuleName
		{
			get
			{
				return moduleName;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException("value");
				}
				moduleName = value;
			}
		}

		public int ResourceId { get; set; }

		public string ReferencePath
		{
			get
			{
				return referencePath;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException("value");
				}
				string[] array = value.Split(commaSeparator);
				if (array.Length != 2 || string.IsNullOrEmpty(array[0]) || string.IsNullOrEmpty(array[1]))
				{
					throw new ArgumentException(LocalizedMessages.InvalidReferencePath, "value");
				}
				ModuleName = array[0];
				ResourceId = int.Parse(array[1], CultureInfo.InvariantCulture);
				referencePath = value;
			}
		}

		public IconReference(string moduleName, int resourceId)
		{
			this = default(IconReference);
			if (string.IsNullOrEmpty(moduleName))
			{
				throw new ArgumentNullException("moduleName");
			}
			this.moduleName = moduleName;
			ResourceId = resourceId;
			referencePath = string.Format(CultureInfo.InvariantCulture, "{0},{1}", moduleName, resourceId);
		}

		public IconReference(string refPath)
		{
			this = default(IconReference);
			if (string.IsNullOrEmpty(refPath))
			{
				throw new ArgumentNullException("refPath");
			}
			string[] array = refPath.Split(commaSeparator);
			if (array.Length != 2 || string.IsNullOrEmpty(array[0]) || string.IsNullOrEmpty(array[1]))
			{
				throw new ArgumentException(LocalizedMessages.InvalidReferencePath, "refPath");
			}
			moduleName = array[0];
			ResourceId = int.Parse(array[1], CultureInfo.InvariantCulture);
			referencePath = refPath;
		}

		public static bool operator ==(IconReference icon1, IconReference icon2)
		{
			return icon1.moduleName == icon2.moduleName && icon1.referencePath == icon2.referencePath && icon1.ResourceId == icon2.ResourceId;
		}

		public static bool operator !=(IconReference icon1, IconReference icon2)
		{
			return !(icon1 == icon2);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is IconReference))
			{
				return false;
			}
			return this == (IconReference)obj;
		}

		public override int GetHashCode()
		{
			int hashCode = moduleName.GetHashCode();
			hashCode = hashCode * 31 + referencePath.GetHashCode();
			return hashCode * 31 + ResourceId.GetHashCode();
		}
	}
}
