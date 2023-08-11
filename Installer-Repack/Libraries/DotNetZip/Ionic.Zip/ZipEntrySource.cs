namespace Ionic.Zip
{
	/// <summary>
	/// An enum that specifies the source of the ZipEntry. 
	/// </summary>
	public enum ZipEntrySource
	{
		/// <summary>
		/// Default value.  Invalid on a bonafide ZipEntry.
		/// </summary>
		None,
		/// <summary>
		/// The entry was instantiated by calling AddFile() or another method that 
		/// added an entry from the filesystem.
		/// </summary>
		FileSystem,
		/// <summary>
		/// The entry was instantiated via <see cref="M:Ionic.Zip.ZipFile.AddEntry(System.String,System.String)" /> or
		/// <see cref="M:Ionic.Zip.ZipFile.AddEntry(System.String,System.IO.Stream)" /> .
		/// </summary>
		Stream,
		/// <summary>
		/// The ZipEntry was instantiated by reading a zipfile.
		/// </summary>
		ZipFile,
		/// <summary>
		/// The content for the ZipEntry will be or was provided by the WriteDelegate.
		/// </summary>
		WriteDelegate,
		/// <summary>
		/// The content for the ZipEntry will be obtained from the stream dispensed by the <c>OpenDelegate</c>.
		/// The entry was instantiated via <see cref="M:Ionic.Zip.ZipFile.AddEntry(System.String,Ionic.Zip.OpenDelegate,Ionic.Zip.CloseDelegate)" />.
		/// </summary>
		JitStream,
		/// <summary>
		/// The content for the ZipEntry will be or was obtained from a <c>ZipOutputStream</c>.
		/// </summary>
		ZipOutputStream
	}
}
