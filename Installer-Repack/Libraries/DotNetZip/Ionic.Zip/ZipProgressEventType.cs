namespace Ionic.Zip
{
	/// <summary>
	///   In an EventArgs type, indicates which sort of progress event is being
	///   reported.
	/// </summary>
	/// <remarks>
	///   There are events for reading, events for saving, and events for
	///   extracting. This enumeration allows a single EventArgs type to be sued to
	///   describe one of multiple subevents. For example, a SaveProgress event is
	///   invoked before, after, and during the saving of a single entry.  The value
	///   of an enum with this type, specifies which event is being triggered.  The
	///   same applies to Extraction, Reading and Adding events.
	/// </remarks>
	public enum ZipProgressEventType
	{
		/// <summary>
		/// Indicates that a Add() operation has started.
		/// </summary>
		Adding_Started,
		/// <summary>
		/// Indicates that an individual entry in the archive has been added.
		/// </summary>
		Adding_AfterAddEntry,
		/// <summary>
		/// Indicates that a Add() operation has completed.
		/// </summary>
		Adding_Completed,
		/// <summary>
		/// Indicates that a Read() operation has started.
		/// </summary>
		Reading_Started,
		/// <summary>
		/// Indicates that an individual entry in the archive is about to be read.
		/// </summary>
		Reading_BeforeReadEntry,
		/// <summary>
		/// Indicates that an individual entry in the archive has just been read.
		/// </summary>
		Reading_AfterReadEntry,
		/// <summary>
		/// Indicates that a Read() operation has completed.
		/// </summary>
		Reading_Completed,
		/// <summary>
		/// The given event reports the number of bytes read so far
		/// during a Read() operation.
		/// </summary>
		Reading_ArchiveBytesRead,
		/// <summary>
		/// Indicates that a Save() operation has started.
		/// </summary>
		Saving_Started,
		/// <summary>
		/// Indicates that an individual entry in the archive is about to be written.
		/// </summary>
		Saving_BeforeWriteEntry,
		/// <summary>
		/// Indicates that an individual entry in the archive has just been saved.
		/// </summary>
		Saving_AfterWriteEntry,
		/// <summary>
		/// Indicates that a Save() operation has completed.
		/// </summary>
		Saving_Completed,
		/// <summary>
		/// Indicates that the zip archive has been created in a
		/// temporary location during a Save() operation.
		/// </summary>
		Saving_AfterSaveTempArchive,
		/// <summary>
		/// Indicates that the temporary file is about to be renamed to the final archive
		/// name during a Save() operation.
		/// </summary>
		Saving_BeforeRenameTempArchive,
		/// <summary>
		/// Indicates that the temporary file is has just been renamed to the final archive
		/// name during a Save() operation.
		/// </summary>
		Saving_AfterRenameTempArchive,
		/// <summary>
		/// Indicates that the self-extracting archive has been compiled
		/// during a Save() operation.
		/// </summary>
		Saving_AfterCompileSelfExtractor,
		/// <summary>
		/// The given event is reporting the number of source bytes that have run through the compressor so far
		/// during a Save() operation.
		/// </summary>
		Saving_EntryBytesRead,
		/// <summary>
		/// Indicates that an entry is about to be extracted.
		/// </summary>
		Extracting_BeforeExtractEntry,
		/// <summary>
		/// Indicates that an entry has just been extracted.
		/// </summary>
		Extracting_AfterExtractEntry,
		/// <summary>
		///   Indicates that extraction of an entry would overwrite an existing
		///   filesystem file. You must use
		///   <see cref="F:Ionic.Zip.ExtractExistingFileAction.InvokeExtractProgressEvent">
		///   ExtractExistingFileAction.InvokeExtractProgressEvent</see> in the call
		///   to <c>ZipEntry.Extract()</c> in order to receive this event.
		/// </summary>
		Extracting_ExtractEntryWouldOverwrite,
		/// <summary>
		///   The given event is reporting the number of bytes written so far for
		///   the current entry during an Extract() operation.
		/// </summary>
		Extracting_EntryBytesWritten,
		/// <summary>
		/// Indicates that an ExtractAll operation is about to begin.
		/// </summary>
		Extracting_BeforeExtractAll,
		/// <summary>
		/// Indicates that an ExtractAll operation has completed.
		/// </summary>
		Extracting_AfterExtractAll,
		/// <summary>
		/// Indicates that an error has occurred while saving a zip file.
		/// This generally means the file cannot be opened, because it has been
		/// removed, or because it is locked by another process.  It can also
		/// mean that the file cannot be Read, because of a range lock conflict.
		/// </summary>
		Error_Saving
	}
}
