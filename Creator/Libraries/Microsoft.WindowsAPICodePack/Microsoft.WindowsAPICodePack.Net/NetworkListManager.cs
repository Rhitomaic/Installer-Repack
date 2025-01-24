using System;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Net
{
	public static class NetworkListManager
	{
		private static NetworkListManagerClass manager = new NetworkListManagerClass();

		public static bool IsConnectedToInternet
		{
			get
			{
				CoreHelpers.ThrowIfNotVista();
				return manager.IsConnectedToInternet;
			}
		}

		public static bool IsConnected
		{
			get
			{
				CoreHelpers.ThrowIfNotVista();
				return manager.IsConnected;
			}
		}

		public static ConnectivityStates Connectivity
		{
			get
			{
				CoreHelpers.ThrowIfNotVista();
				return manager.GetConnectivity();
			}
		}

		public static NetworkCollection GetNetworks(NetworkConnectivityLevels level)
		{
			CoreHelpers.ThrowIfNotVista();
			return new NetworkCollection(manager.GetNetworks(level));
		}

		public static Network GetNetwork(Guid networkId)
		{
			CoreHelpers.ThrowIfNotVista();
			return new Network(manager.GetNetwork(networkId));
		}

		public static NetworkConnectionCollection GetNetworkConnections()
		{
			CoreHelpers.ThrowIfNotVista();
			return new NetworkConnectionCollection(manager.GetNetworkConnections());
		}

		public static NetworkConnection GetNetworkConnection(Guid networkConnectionId)
		{
			CoreHelpers.ThrowIfNotVista();
			return new NetworkConnection(manager.GetNetworkConnection(networkConnectionId));
		}
	}
}
