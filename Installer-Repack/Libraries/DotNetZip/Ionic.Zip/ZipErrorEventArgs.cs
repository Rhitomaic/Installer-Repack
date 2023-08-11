using System;

namespace Ionic.Zip
{
	/// <summary>
	/// Provides information about the an error that occurred while zipping.
	/// </summary>
	public class ZipErrorEventArgs : ZipProgressEventArgs
	{
		private Exception _exc;

		/// <summary>
		/// Returns the exception that occurred, if any.
		/// </summary>
		public Exception Exception => _exc;

		/// <summary>
		/// Returns the name of the file that caused the exception, if any.
		/// </summary>
		public string FileName => base.CurrentEntry.LocalFileName;

		private ZipErrorEventArgs()
		{
		}

		internal static ZipErrorEventArgs Saving(string archiveName, ZipEntry entry, Exception exception)
		{
			return new ZipErrorEventArgs
			{
				EventType = ZipProgressEventType.Error_Saving,
				ArchiveName = archiveName,
				CurrentEntry = entry,
				_exc = exception
			};
		}
	}
}
