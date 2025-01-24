namespace Ionic.Zip
{
	/// <summary>
	/// An enum that provides the different self-extractor flavors
	/// </summary>
	public enum SelfExtractorFlavor
	{
		/// <summary>
		/// A self-extracting zip archive that runs from the console or
		/// command line.
		/// </summary>
		ConsoleApplication,
		/// <summary>
		/// A self-extracting zip archive that presents a graphical user
		/// interface when it is executed.
		/// </summary>
		WinFormsApplication
	}
}
