using System.Collections;
using System.Collections.Generic;

namespace Microsoft.WindowsAPICodePack.Net
{
	public class NetworkConnectionCollection : IEnumerable<NetworkConnection>, IEnumerable
	{
		private IEnumerable networkConnectionEnumerable;

		internal NetworkConnectionCollection(IEnumerable networkConnectionEnumerable)
		{
			this.networkConnectionEnumerable = networkConnectionEnumerable;
		}

		public IEnumerator<NetworkConnection> GetEnumerator()
		{
			foreach (INetworkConnection networkConnection in networkConnectionEnumerable)
			{
				yield return new NetworkConnection(networkConnection);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach (INetworkConnection networkConnection in networkConnectionEnumerable)
			{
				yield return new NetworkConnection(networkConnection);
			}
		}
	}
}
