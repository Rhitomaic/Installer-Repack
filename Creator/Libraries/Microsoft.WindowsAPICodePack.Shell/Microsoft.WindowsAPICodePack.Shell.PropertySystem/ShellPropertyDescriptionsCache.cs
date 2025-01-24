using System.Collections.Generic;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	internal class ShellPropertyDescriptionsCache
	{
		private IDictionary<PropertyKey, ShellPropertyDescription> propsDictionary;

		private static ShellPropertyDescriptionsCache cacheInstance;

		public static ShellPropertyDescriptionsCache Cache
		{
			get
			{
				if (cacheInstance == null)
				{
					cacheInstance = new ShellPropertyDescriptionsCache();
				}
				return cacheInstance;
			}
		}

		private ShellPropertyDescriptionsCache()
		{
			propsDictionary = new Dictionary<PropertyKey, ShellPropertyDescription>();
		}

		public ShellPropertyDescription GetPropertyDescription(PropertyKey key)
		{
			if (!propsDictionary.ContainsKey(key))
			{
				propsDictionary.Add(key, new ShellPropertyDescription(key));
			}
			return propsDictionary[key];
		}
	}
}
