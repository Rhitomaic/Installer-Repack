using System;

namespace Ionic.Zip
{
	/// <summary>
	/// Provides information about the progress of a save, read, or extract operation.
	/// This is a base class; you will probably use one of the classes derived from this one.
	/// </summary>
	public class ZipProgressEventArgs : EventArgs
	{
		private int _entriesTotal;

		private bool _cancel;

		private ZipEntry _latestEntry;

		private ZipProgressEventType _flavor;

		private string _archiveName;

		private long _bytesTransferred;

		private long _totalBytesToTransfer;

		/// <summary>
		/// The total number of entries to be saved or extracted.
		/// </summary>
		public int EntriesTotal
		{
			get
			{
				return _entriesTotal;
			}
			set
			{
				_entriesTotal = value;
			}
		}

		/// <summary>
		/// The name of the last entry saved or extracted.
		/// </summary>
		public ZipEntry CurrentEntry
		{
			get
			{
				return _latestEntry;
			}
			set
			{
				_latestEntry = value;
			}
		}

		/// <summary>
		/// In an event handler, set this to cancel the save or extract
		/// operation that is in progress.
		/// </summary>
		public bool Cancel
		{
			get
			{
				return _cancel;
			}
			set
			{
				_cancel |= value;
			}
		}

		/// <summary>
		/// The type of event being reported.
		/// </summary>
		public ZipProgressEventType EventType
		{
			get
			{
				return _flavor;
			}
			set
			{
				_flavor = value;
			}
		}

		/// <summary>
		/// Returns the archive name associated to this event.
		/// </summary>
		public string ArchiveName
		{
			get
			{
				return _archiveName;
			}
			set
			{
				_archiveName = value;
			}
		}

		/// <summary>
		/// The number of bytes read or written so far for this entry.
		/// </summary>
		public long BytesTransferred
		{
			get
			{
				return _bytesTransferred;
			}
			set
			{
				_bytesTransferred = value;
			}
		}

		/// <summary>
		/// Total number of bytes that will be read or written for this entry.
		/// This number will be -1 if the value cannot be determined.
		/// </summary>
		public long TotalBytesToTransfer
		{
			get
			{
				return _totalBytesToTransfer;
			}
			set
			{
				_totalBytesToTransfer = value;
			}
		}

		internal ZipProgressEventArgs()
		{
		}

		internal ZipProgressEventArgs(string archiveName, ZipProgressEventType flavor)
		{
			_archiveName = archiveName;
			_flavor = flavor;
		}
	}
}
