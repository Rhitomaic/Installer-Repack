namespace Ionic.Zip
{
	/// <summary>
	/// Provides information about the progress of the extract operation.
	/// </summary>
	public class ExtractProgressEventArgs : ZipProgressEventArgs
	{
		private int _entriesExtracted;

		private string _target;

		/// <summary>
		/// Number of entries extracted so far.  This is set only if the
		/// EventType is Extracting_BeforeExtractEntry or Extracting_AfterExtractEntry, and
		/// the Extract() is occurring witin the scope of a call to ExtractAll().
		/// </summary>
		public int EntriesExtracted => _entriesExtracted;

		/// <summary>
		/// Returns the extraction target location, a filesystem path.
		/// </summary>
		public string ExtractLocation => _target;

		/// <summary>
		/// Constructor for the ExtractProgressEventArgs.
		/// </summary>
		/// <param name="archiveName">the name of the zip archive.</param>
		/// <param name="before">whether this is before saving the entry, or after</param>
		/// <param name="entriesTotal">The total number of entries in the zip archive.</param>
		/// <param name="entriesExtracted">Number of entries that have been extracted.</param>
		/// <param name="entry">The entry involved in the event.</param>
		/// <param name="extractLocation">The location to which entries are extracted.</param>
		internal ExtractProgressEventArgs(string archiveName, bool before, int entriesTotal, int entriesExtracted, ZipEntry entry, string extractLocation)
			: base(archiveName, before ? ZipProgressEventType.Extracting_BeforeExtractEntry : ZipProgressEventType.Extracting_AfterExtractEntry)
		{
			base.EntriesTotal = entriesTotal;
			base.CurrentEntry = entry;
			_entriesExtracted = entriesExtracted;
			_target = extractLocation;
		}

		internal ExtractProgressEventArgs(string archiveName, ZipProgressEventType flavor)
			: base(archiveName, flavor)
		{
		}

		internal ExtractProgressEventArgs()
		{
		}

		internal static ExtractProgressEventArgs BeforeExtractEntry(string archiveName, ZipEntry entry, string extractLocation)
		{
			return new ExtractProgressEventArgs
			{
				ArchiveName = archiveName,
				EventType = ZipProgressEventType.Extracting_BeforeExtractEntry,
				CurrentEntry = entry,
				_target = extractLocation
			};
		}

		internal static ExtractProgressEventArgs ExtractExisting(string archiveName, ZipEntry entry, string extractLocation)
		{
			return new ExtractProgressEventArgs
			{
				ArchiveName = archiveName,
				EventType = ZipProgressEventType.Extracting_ExtractEntryWouldOverwrite,
				CurrentEntry = entry,
				_target = extractLocation
			};
		}

		internal static ExtractProgressEventArgs AfterExtractEntry(string archiveName, ZipEntry entry, string extractLocation)
		{
			return new ExtractProgressEventArgs
			{
				ArchiveName = archiveName,
				EventType = ZipProgressEventType.Extracting_AfterExtractEntry,
				CurrentEntry = entry,
				_target = extractLocation
			};
		}

		internal static ExtractProgressEventArgs ExtractAllStarted(string archiveName, string extractLocation)
		{
			return new ExtractProgressEventArgs(archiveName, ZipProgressEventType.Extracting_BeforeExtractAll)
			{
				_target = extractLocation
			};
		}

		internal static ExtractProgressEventArgs ExtractAllCompleted(string archiveName, string extractLocation)
		{
			return new ExtractProgressEventArgs(archiveName, ZipProgressEventType.Extracting_AfterExtractAll)
			{
				_target = extractLocation
			};
		}

		internal static ExtractProgressEventArgs ByteUpdate(string archiveName, ZipEntry entry, long bytesWritten, long totalBytes)
		{
			return new ExtractProgressEventArgs(archiveName, ZipProgressEventType.Extracting_EntryBytesWritten)
			{
				ArchiveName = archiveName,
				CurrentEntry = entry,
				BytesTransferred = bytesWritten,
				TotalBytesToTransfer = totalBytes
			};
		}
	}
}
