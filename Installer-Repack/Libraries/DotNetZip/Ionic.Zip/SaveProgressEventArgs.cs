namespace Ionic.Zip
{
	/// <summary>
	/// Provides information about the progress of a save operation.
	/// </summary>
	public class SaveProgressEventArgs : ZipProgressEventArgs
	{
		private int _entriesSaved;

		/// <summary>
		/// Number of entries saved so far.
		/// </summary>
		public int EntriesSaved => _entriesSaved;

		/// <summary>
		/// Constructor for the SaveProgressEventArgs.
		/// </summary>
		/// <param name="archiveName">the name of the zip archive.</param>
		/// <param name="before">whether this is before saving the entry, or after</param>
		/// <param name="entriesTotal">The total number of entries in the zip archive.</param>
		/// <param name="entriesSaved">Number of entries that have been saved.</param>
		/// <param name="entry">The entry involved in the event.</param>
		internal SaveProgressEventArgs(string archiveName, bool before, int entriesTotal, int entriesSaved, ZipEntry entry)
			: base(archiveName, before ? ZipProgressEventType.Saving_BeforeWriteEntry : ZipProgressEventType.Saving_AfterWriteEntry)
		{
			base.EntriesTotal = entriesTotal;
			base.CurrentEntry = entry;
			_entriesSaved = entriesSaved;
		}

		internal SaveProgressEventArgs()
		{
		}

		internal SaveProgressEventArgs(string archiveName, ZipProgressEventType flavor)
			: base(archiveName, flavor)
		{
		}

		internal static SaveProgressEventArgs ByteUpdate(string archiveName, ZipEntry entry, long bytesXferred, long totalBytes)
		{
			return new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_EntryBytesRead)
			{
				ArchiveName = archiveName,
				CurrentEntry = entry,
				BytesTransferred = bytesXferred,
				TotalBytesToTransfer = totalBytes
			};
		}

		internal static SaveProgressEventArgs Started(string archiveName)
		{
			return new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_Started);
		}

		internal static SaveProgressEventArgs Completed(string archiveName)
		{
			return new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_Completed);
		}
	}
}
