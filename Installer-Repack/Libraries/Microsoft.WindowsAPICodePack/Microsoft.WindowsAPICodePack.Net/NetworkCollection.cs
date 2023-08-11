using System.Collections;
using System.Collections.Generic;

namespace Microsoft.WindowsAPICodePack.Net
{
	public class NetworkCollection : IEnumerable<Network>, IEnumerable
	{
		private IEnumerable networkEnumerable;

		internal NetworkCollection(IEnumerable networkEnumerable)
		{
			this.networkEnumerable = networkEnumerable;
		}

		public IEnumerator<Network> GetEnumerator()
		{
			foreach (INetwork network in networkEnumerable)
			{
				yield return new Network(network);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach (INetwork network in networkEnumerable)
			{
				yield return new Network(network);
			}
		}
	}
}
