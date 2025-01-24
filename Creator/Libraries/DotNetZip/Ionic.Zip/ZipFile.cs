using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Ionic.Zlib;
using Microsoft.CSharp;

namespace Ionic.Zip
{
	/// <summary>
	///   The ZipFile type represents a zip archive file.
	/// </summary>
	///
	/// <remarks>
	/// <para>
	///   This is the main type in the DotNetZip class library. This class reads and
	///   writes zip files, as defined in the <see href="http://www.pkware.com/documents/casestudies/APPNOTE.TXT">specification
	///   for zip files described by PKWare</see>.  The compression for this
	///   implementation is provided by a managed-code version of Zlib, included with
	///   DotNetZip in the classes in the Ionic.Zlib namespace.
	/// </para>
	///
	/// <para>
	///   This class provides a general purpose zip file capability.  Use it to read,
	///   create, or update zip files.  When you want to create zip files using a
	///   <c>Stream</c> type to write the zip file, you may want to consider the <see cref="T:Ionic.Zip.ZipOutputStream" /> class.
	/// </para>
	///
	/// <para>
	///   Both the <c>ZipOutputStream</c> class and the <c>ZipFile</c> class can
	///   be used to create zip files. Both of them support many of the common zip
	///   features, including Unicode, different compression methods and levels,
	///   and ZIP64. They provide very similar performance when creating zip
	///   files.
	/// </para>
	///
	/// <para>
	///   The <c>ZipFile</c> class is generally easier to use than
	///   <c>ZipOutputStream</c> and should be considered a higher-level interface.  For
	///   example, when creating a zip file via calls to the <c>PutNextEntry()</c> and
	///   <c>Write()</c> methods on the <c>ZipOutputStream</c> class, the caller is
	///   responsible for opening the file, reading the bytes from the file, writing
	///   those bytes into the <c>ZipOutputStream</c>, setting the attributes on the
	///   <c>ZipEntry</c>, and setting the created, last modified, and last accessed
	///   timestamps on the zip entry. All of these things are done automatically by a
	///   call to <see cref="M:Ionic.Zip.ZipFile.AddFile(System.String,System.String)">ZipFile.AddFile()</see>.
	///   For this reason, the <c>ZipOutputStream</c> is generally recommended for use
	///   only when your application emits arbitrary data, not necessarily data from a
	///   filesystem file, directly into a zip file, and does so using a <c>Stream</c>
	///   metaphor.
	/// </para>
	///
	/// <para>
	///   Aside from the differences in programming model, there are other
	///   differences in capability between the two classes.
	/// </para>
	///
	/// <list type="bullet">
	///   <item>
	///     <c>ZipFile</c> can be used to read and extract zip files, in addition to
	///     creating zip files. <c>ZipOutputStream</c> cannot read zip files. If you want
	///     to use a stream to read zip files, check out the <see cref="T:Ionic.Zip.ZipInputStream" /> class.
	///   </item>
	///
	///   <item>
	///     <c>ZipOutputStream</c> does not support the creation of segmented or spanned
	///     zip files.
	///   </item>
	///
	///   <item>
	///     <c>ZipOutputStream</c> cannot produce a self-extracting archive.
	///   </item>
	/// </list>
	///
	/// <para>
	///   Be aware that the <c>ZipFile</c> class implements the <see cref="T:System.IDisposable" /> interface.  In order for <c>ZipFile</c> to
	///   produce a valid zip file, you use use it within a using clause (<c>Using</c>
	///   in VB), or call the <c>Dispose()</c> method explicitly.  See the examples
	///   for how to employ a using clause.
	/// </para>
	///
	/// </remarks>
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00005")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class ZipFile : IEnumerable, IEnumerable<ZipEntry>, IDisposable
	{
		private class ExtractorSettings
		{
			public SelfExtractorFlavor Flavor;

			public List<string> ReferencedAssemblies;

			public List<string> CopyThroughResources;

			public List<string> ResourcesToCompile;
		}

		private static ExtractorSettings[] SettingsList;

		private TextWriter _StatusMessageTextWriter;

		private bool _CaseSensitiveRetrieval;

		private bool _IgnoreDuplicateFiles;

		private Stream _readstream;

		private Stream _writestream;

		private ushort _versionMadeBy;

		private ushort _versionNeededToExtract;

		private uint _diskNumberWithCd;

		private long _maxOutputSegmentSize;

		private uint _numberOfSegmentsForMostRecentSave;

		private ZipErrorAction _zipErrorAction;

		private bool _disposed;

		private Dictionary<string, ZipEntry> _entries;

		private Dictionary<string, ZipEntry> _entriesInsensitive;

		private List<ZipEntry> _zipEntriesAsList;

		private string _name;

		private string _readName;

		private string _Comment;

		internal string _Password;

		private bool _emitNtfsTimes = true;

		private bool _emitUnixTimes;

		private CompressionStrategy _Strategy;

		private CompressionMethod _compressionMethod = CompressionMethod.Deflate;

		private bool _fileAlreadyExists;

		private string _temporaryFileName;

		private bool _contentsChanged;

		private bool _hasBeenSaved;

		private string _TempFileFolder;

		private bool _ReadStreamIsOurs = true;

		private object LOCK = new object();

		private bool _saveOperationCanceled;

		private bool _extractOperationCanceled;

		private bool _addOperationCanceled;

		private EncryptionAlgorithm _Encryption;

		private bool _JustSaved;

		private long _locEndOfCDS = -1L;

		private uint _OffsetOfCentralDirectory;

		private long _OffsetOfCentralDirectory64;

		private bool? _OutputUsesZip64;

		internal bool _inExtractAll;

		private Encoding _alternateEncoding;

		private ZipOption _alternateEncodingUsage;

		private int _BufferSize = BufferSizeDefault;

		internal ParallelDeflateOutputStream ParallelDeflater;

		private long _ParallelDeflateThreshold;

		private int _maxBufferPairs = 16;

		internal Zip64Option _zip64;

		private bool _SavingSfx;

		/// <summary>
		///   Default size of the buffer used for IO.
		/// </summary>
		public static readonly int BufferSizeDefault;

		private long _lengthOfReadStream = -99L;

		private static Encoding _defaultEncoding;

		private static bool _defaultEncodingInitialized;

		/// <summary>
		///   Provides a human-readable string with information about the ZipFile.
		/// </summary>
		///
		/// <remarks>
		///   <para>
		///     The information string contains 10 lines or so, about each ZipEntry,
		///     describing whether encryption is in use, the compressed and uncompressed
		///     length of the entry, the offset of the entry, and so on. As a result the
		///     information string can be very long for zip files that contain many
		///     entries.
		///   </para>
		///   <para>
		///     This information is mostly useful for diagnostic purposes.
		///   </para>
		/// </remarks>
		public string Info
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append($"          ZipFile: {Name}\n");
				if (!string.IsNullOrEmpty(_Comment))
				{
					stringBuilder.Append($"          Comment: {_Comment}\n");
				}
				if (_versionMadeBy != 0)
				{
					stringBuilder.Append($"  version made by: 0x{_versionMadeBy:X4}\n");
				}
				if (_versionNeededToExtract != 0)
				{
					stringBuilder.Append($"needed to extract: 0x{_versionNeededToExtract:X4}\n");
				}
				stringBuilder.Append($"       uses ZIP64: {InputUsesZip64}\n");
				stringBuilder.Append($"     disk with CD: {_diskNumberWithCd}\n");
				if (_OffsetOfCentralDirectory == uint.MaxValue)
				{
					stringBuilder.Append($"      CD64 offset: 0x{_OffsetOfCentralDirectory64:X16}\n");
				}
				else
				{
					stringBuilder.Append($"        CD offset: 0x{_OffsetOfCentralDirectory:X8}\n");
				}
				stringBuilder.Append("\n");
				foreach (ZipEntry value in _entries.Values)
				{
					stringBuilder.Append(value.Info);
				}
				return stringBuilder.ToString();
			}
		}

		/// <summary>
		/// Indicates whether to perform a full scan of the zip file when reading it.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   You almost never want to use this property.
		/// </para>
		///
		/// <para>
		///   When reading a zip file, if this flag is <c>true</c> (<c>True</c> in
		///   VB), the entire zip archive will be scanned and searched for entries.
		///   For large archives, this can take a very, long time. The much more
		///   efficient default behavior is to read the zip directory, which is
		///   stored at the end of the zip file. But, in some cases the directory is
		///   corrupted and you need to perform a full scan of the zip file to
		///   determine the contents of the zip file. This property lets you do
		///   that, when necessary.
		/// </para>
		///
		/// <para>
		///   This flag is effective only when calling <see cref="M:Ionic.Zip.ZipFile.Initialize(System.String)" />. Normally you would read a ZipFile with the
		///   static <see cref="M:Ionic.Zip.ZipFile.Read(System.String)">ZipFile.Read</see>
		///   method. But you can't set the <c>FullScan</c> property on the
		///   <c>ZipFile</c> instance when you use a static factory method like
		///   <c>ZipFile.Read</c>.
		/// </para>
		///
		/// </remarks>
		///
		/// <example>
		///
		///   This example shows how to read a zip file using the full scan approach,
		///   and then save it, thereby producing a corrected zip file.
		///
		/// <code lang="C#">
		/// using (var zip = new ZipFile())
		/// {
		///     zip.FullScan = true;
		///     zip.Initialize(zipFileName);
		///     zip.Save(newName);
		/// }
		/// </code>
		///
		/// <code lang="VB">
		/// Using zip As New ZipFile
		///     zip.FullScan = True
		///     zip.Initialize(zipFileName)
		///     zip.Save(newName)
		/// End Using
		/// </code>
		/// </example>
		public bool FullScan { get; set; }

		/// <summary>
		///   Whether to sort the ZipEntries before saving the file.
		/// </summary>
		///
		/// <remarks>
		///   The default is false.  If you have a large number of zip entries, the sort
		///   alone can consume significant time.
		/// </remarks>
		///
		/// <example>
		/// <code lang="C#">
		/// using (var zip = new ZipFile())
		/// {
		///     zip.AddFiles(filesToAdd);
		///     zip.SortEntriesBeforeSaving = true;
		///     zip.Save(name);
		/// }
		/// </code>
		///
		/// <code lang="VB">
		/// Using zip As New ZipFile
		///     zip.AddFiles(filesToAdd)
		///     zip.SortEntriesBeforeSaving = True
		///     zip.Save(name)
		/// End Using
		/// </code>
		/// </example>
		public bool SortEntriesBeforeSaving { get; set; }

		/// <summary>
		///   Indicates whether NTFS Reparse Points, like junctions, should be
		///   traversed during calls to <c>AddDirectory()</c>.
		/// </summary>
		///
		/// <remarks>
		///   By default, calls to AddDirectory() will traverse NTFS reparse
		///   points, like mounted volumes, and directory junctions.  An example
		///   of a junction is the "My Music" directory in Windows Vista.  In some
		///   cases you may not want DotNetZip to traverse those directories.  In
		///   that case, set this property to false.
		/// </remarks>
		///
		/// <example>
		/// <code lang="C#">
		/// using (var zip = new ZipFile())
		/// {
		///     zip.AddDirectoryWillTraverseReparsePoints = false;
		///     zip.AddDirectory(dirToZip,"fodder");
		///     zip.Save(zipFileToCreate);
		/// }
		/// </code>
		/// </example>
		public bool AddDirectoryWillTraverseReparsePoints { get; set; }

		/// <summary>
		///   Size of the IO buffer used while saving.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   First, let me say that you really don't need to bother with this.  It is
		///   here to allow for optimizations that you probably won't make! It will work
		///   fine if you don't set or get this property at all. Ok?
		/// </para>
		///
		/// <para>
		///   Now that we have <em>that</em> out of the way, the fine print: This
		///   property affects the size of the buffer that is used for I/O for each
		///   entry contained in the zip file. When a file is read in to be compressed,
		///   it uses a buffer given by the size here.  When you update a zip file, the
		///   data for unmodified entries is copied from the first zip file to the
		///   other, through a buffer given by the size here.
		/// </para>
		///
		/// <para>
		///   Changing the buffer size affects a few things: first, for larger buffer
		///   sizes, the memory used by the <c>ZipFile</c>, obviously, will be larger
		///   during I/O operations.  This may make operations faster for very much
		///   larger files.  Last, for any given entry, when you use a larger buffer
		///   there will be fewer progress events during I/O operations, because there's
		///   one progress event generated for each time the buffer is filled and then
		///   emptied.
		/// </para>
		///
		/// <para>
		///   The default buffer size is 8k.  Increasing the buffer size may speed
		///   things up as you compress larger files.  But there are no hard-and-fast
		///   rules here, eh?  You won't know til you test it.  And there will be a
		///   limit where ever larger buffers actually slow things down.  So as I said
		///   in the beginning, it's probably best if you don't set or get this property
		///   at all.
		/// </para>
		///
		/// </remarks>
		///
		/// <example>
		/// This example shows how you might set a large buffer size for efficiency when
		/// dealing with zip entries that are larger than 1gb.
		/// <code lang="C#">
		/// using (ZipFile zip = new ZipFile())
		/// {
		///     zip.SaveProgress += this.zip1_SaveProgress;
		///     zip.AddDirectory(directoryToZip, "");
		///     zip.UseZip64WhenSaving = Zip64Option.Always;
		///     zip.BufferSize = 65536*8; // 65536 * 8 = 512k
		///     zip.Save(ZipFileToCreate);
		/// }
		/// </code>
		/// </example>
		public int BufferSize
		{
			get
			{
				return _BufferSize;
			}
			set
			{
				_BufferSize = value;
			}
		}

		/// <summary>
		///   Size of the work buffer to use for the ZLIB codec during compression.
		/// </summary>
		///
		/// <remarks>
		///   <para>
		///     When doing ZLIB or Deflate compression, the library fills a buffer,
		///     then passes it to the compressor for compression. Then the library
		///     reads out the compressed bytes. This happens repeatedly until there
		///     is no more uncompressed data to compress. This property sets the
		///     size of the buffer that will be used for chunk-wise compression. In
		///     order for the setting to take effect, your application needs to set
		///     this property before calling one of the <c>ZipFile.Save()</c>
		///     overloads.
		///   </para>
		///   <para>
		///     Setting this affects the performance and memory efficiency of
		///     compression and decompression. For larger files, setting this to a
		///     larger size may improve compression performance, but the exact
		///     numbers vary depending on available memory, the size of the streams
		///     you are compressing, and a bunch of other variables. I don't have
		///     good firm recommendations on how to set it.  You'll have to test it
		///     yourself. Or just leave it alone and accept the default.
		///   </para>
		/// </remarks>
		public int CodecBufferSize { get; set; }

		/// <summary>
		///   Indicates whether extracted files should keep their paths as
		///   stored in the zip archive.
		/// </summary>
		///
		/// <remarks>
		///  <para>
		///    This property affects Extraction.  It is not used when creating zip
		///    archives.
		///  </para>
		///
		///  <para>
		///    With this property set to <c>false</c>, the default, extracting entries
		///    from a zip file will create files in the filesystem that have the full
		///    path associated to the entry within the zip file.  With this property set
		///    to <c>true</c>, extracting entries from the zip file results in files
		///    with no path: the folders are "flattened."
		///  </para>
		///
		///  <para>
		///    An example: suppose the zip file contains entries /directory1/file1.txt and
		///    /directory2/file2.txt.  With <c>FlattenFoldersOnExtract</c> set to false,
		///    the files created will be \directory1\file1.txt and \directory2\file2.txt.
		///    With the property set to true, the files created are file1.txt and file2.txt.
		///  </para>
		///
		/// </remarks>
		public bool FlattenFoldersOnExtract { get; set; }

		/// <summary>
		///   The compression strategy to use for all entries.
		/// </summary>
		///
		/// <remarks>
		///   Set the Strategy used by the ZLIB-compatible compressor, when
		///   compressing entries using the DEFLATE method. Different compression
		///   strategies work better on different sorts of data. The strategy
		///   parameter can affect the compression ratio and the speed of
		///   compression but not the correctness of the compresssion.  For more
		///   information see <see cref="T:Ionic.Zlib.CompressionStrategy">Ionic.Zlib.CompressionStrategy</see>.
		/// </remarks>
		public CompressionStrategy Strategy
		{
			get
			{
				return _Strategy;
			}
			set
			{
				_Strategy = value;
			}
		}

		/// <summary>
		///   The name of the <c>ZipFile</c>, on disk.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   When the <c>ZipFile</c> instance was created by reading an archive using
		///   one of the <c>ZipFile.Read</c> methods, this property represents the name
		///   of the zip file that was read.  When the <c>ZipFile</c> instance was
		///   created by using the no-argument constructor, this value is <c>null</c>
		///   (<c>Nothing</c> in VB).
		/// </para>
		///
		/// <para>
		///   If you use the no-argument constructor, and you then explicitly set this
		///   property, when you call <see cref="M:Ionic.Zip.ZipFile.Save" />, this name will
		///   specify the name of the zip file created.  Doing so is equivalent to
		///   calling <see cref="M:Ionic.Zip.ZipFile.Save(System.String)" />.  When instantiating a
		///   <c>ZipFile</c> by reading from a stream or byte array, the <c>Name</c>
		///   property remains <c>null</c>.  When saving to a stream, the <c>Name</c>
		///   property is implicitly set to <c>null</c>.
		/// </para>
		/// </remarks>
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		/// <summary>
		///   Sets the compression level to be used for entries subsequently added to
		///   the zip archive.
		/// </summary>
		///
		/// <remarks>
		///  <para>
		///    Varying the compression level used on entries can affect the
		///    size-vs-speed tradeoff when compression and decompressing data streams
		///    or files.
		///  </para>
		///
		///  <para>
		///    As with some other properties on the <c>ZipFile</c> class, like <see cref="P:Ionic.Zip.ZipFile.Password" />, <see cref="P:Ionic.Zip.ZipFile.Encryption" />, and <see cref="P:Ionic.Zip.ZipFile.ZipErrorAction" />, setting this property on a <c>ZipFile</c>
		///    instance will cause the specified <c>CompressionLevel</c> to be used on all
		///    <see cref="T:Ionic.Zip.ZipEntry" /> items that are subsequently added to the
		///    <c>ZipFile</c> instance. If you set this property after you have added
		///    items to the <c>ZipFile</c>, but before you have called <c>Save()</c>,
		///    those items will not use the specified compression level.
		///  </para>
		///
		///  <para>
		///    If you do not set this property, the default compression level is used,
		///    which normally gives a good balance of compression efficiency and
		///    compression speed.  In some tests, using <c>BestCompression</c> can
		///    double the time it takes to compress, while delivering just a small
		///    increase in compression efficiency.  This behavior will vary with the
		///    type of data you compress.  If you are in doubt, just leave this setting
		///    alone, and accept the default.
		///  </para>
		/// </remarks>
		public CompressionLevel CompressionLevel { get; set; }

		/// <summary>
		///   The compression method for the zipfile.
		/// </summary>
		/// <remarks>
		///   <para>
		///     By default, the compression method is <c>CompressionMethod.Deflate.</c>
		///   </para>
		/// </remarks>
		/// <seealso cref="T:Ionic.Zip.CompressionMethod" />
		public CompressionMethod CompressionMethod
		{
			get
			{
				return _compressionMethod;
			}
			set
			{
				_compressionMethod = value;
			}
		}

		/// <summary>
		///   A comment attached to the zip archive.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   This property is read/write. It allows the application to specify a
		///   comment for the <c>ZipFile</c>, or read the comment for the
		///   <c>ZipFile</c>.  After setting this property, changes are only made
		///   permanent when you call a <c>Save()</c> method.
		/// </para>
		///
		/// <para>
		///   According to <see href="http://www.pkware.com/documents/casestudies/APPNOTE.TXT">PKWARE's
		///   zip specification</see>, the comment is not encrypted, even if there is a
		///   password set on the zip file.
		/// </para>
		///
		/// <para>
		///   The specification does not describe how to indicate the encoding used
		///   on a comment string. Many "compliant" zip tools and libraries use
		///   IBM437 as the code page for comments; DotNetZip, too, follows that
		///   practice.  On the other hand, there are situations where you want a
		///   Comment to be encoded with something else, for example using code page
		///   950 "Big-5 Chinese". To fill that need, DotNetZip will encode the
		///   comment following the same procedure it follows for encoding
		///   filenames: (a) if <see cref="P:Ionic.Zip.ZipFile.AlternateEncodingUsage" /> is
		///   <c>Never</c>, it uses the default encoding (IBM437). (b) if <see cref="P:Ionic.Zip.ZipFile.AlternateEncodingUsage" /> is <c>Always</c>, it always uses the
		///   alternate encoding (<see cref="P:Ionic.Zip.ZipFile.AlternateEncoding" />). (c) if <see cref="P:Ionic.Zip.ZipFile.AlternateEncodingUsage" /> is <c>AsNecessary</c>, it uses the
		///   alternate encoding only if the default encoding is not sufficient for
		///   encoding the comment - in other words if decoding the result does not
		///   produce the original string.  This decision is taken at the time of
		///   the call to <c>ZipFile.Save()</c>.
		/// </para>
		///
		/// <para>
		///   When creating a zip archive using this library, it is possible to change
		///   the value of <see cref="P:Ionic.Zip.ZipFile.AlternateEncoding" /> between each
		///   entry you add, and between adding entries and the call to
		///   <c>Save()</c>. Don't do this.  It will likely result in a zip file that is
		///   not readable by any tool or application.  For best interoperability, leave
		///   <see cref="P:Ionic.Zip.ZipFile.AlternateEncoding" /> alone, or specify it only
		///   once, before adding any entries to the <c>ZipFile</c> instance.
		/// </para>
		///
		/// </remarks>
		public string Comment
		{
			get
			{
				return _Comment;
			}
			set
			{
				_Comment = value;
				_contentsChanged = true;
			}
		}

		/// <summary>
		///   Specifies whether the Creation, Access, and Modified times for entries
		///   added to the zip file will be emitted in Windows format
		///   when the zip archive is saved.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   An application creating a zip archive can use this flag to explicitly
		///   specify that the file times for the entries should or should not be stored
		///   in the zip archive in the format used by Windows. By default this flag is
		///   <c>true</c>, meaning the Windows-format times are stored in the zip
		///   archive.
		/// </para>
		///
		/// <para>
		///   When adding an entry from a file or directory, the Creation (<see cref="P:Ionic.Zip.ZipEntry.CreationTime" />), Access (<see cref="P:Ionic.Zip.ZipEntry.AccessedTime" />), and Modified (<see cref="P:Ionic.Zip.ZipEntry.ModifiedTime" />) times for the given entry are
		///   automatically set from the filesystem values. When adding an entry from a
		///   stream or string, all three values are implicitly set to
		///   <c>DateTime.Now</c>.  Applications can also explicitly set those times by
		///   calling <see cref="M:Ionic.Zip.ZipEntry.SetEntryTimes(System.DateTime,System.DateTime,System.DateTime)" />.
		/// </para>
		///
		/// <para>
		///   <see href="http://www.pkware.com/documents/casestudies/APPNOTE.TXT">PKWARE's
		///   zip specification</see> describes multiple ways to format these times in a
		///   zip file. One is the format Windows applications normally use: 100ns ticks
		///   since January 1, 1601 UTC.  The other is a format Unix applications typically
		///   use: seconds since January 1, 1970 UTC.  Each format can be stored in an
		///   "extra field" in the zip entry when saving the zip archive. The former
		///   uses an extra field with a Header Id of 0x000A, while the latter uses a
		///   header ID of 0x5455, although you probably don't need to know that.
		/// </para>
		///
		/// <para>
		///   Not all tools and libraries can interpret these fields.  Windows
		///   compressed folders is one that can read the Windows Format timestamps,
		///   while I believe <see href="http://www.info-zip.org/">the Infozip
		///   tools</see> can read the Unix format timestamps. Some tools and libraries
		///   may be able to read only one or the other. DotNetZip can read or write
		///   times in either or both formats.
		/// </para>
		///
		/// <para>
		///   The times stored are taken from <see cref="P:Ionic.Zip.ZipEntry.ModifiedTime" />, <see cref="P:Ionic.Zip.ZipEntry.AccessedTime" />, and <see cref="P:Ionic.Zip.ZipEntry.CreationTime" />.
		/// </para>
		///
		/// <para>
		///   The value set here applies to all entries subsequently added to the
		///   <c>ZipFile</c>.
		/// </para>
		///
		/// <para>
		///   This property is not mutually exclusive of the <see cref="P:Ionic.Zip.ZipFile.EmitTimesInUnixFormatWhenSaving" /> property. It is possible and
		///   legal and valid to produce a zip file that contains timestamps encoded in
		///   the Unix format as well as in the Windows format, in addition to the <see cref="P:Ionic.Zip.ZipEntry.LastModified">LastModified</see> time attached to each
		///   entry in the archive, a time that is always stored in "DOS format". And,
		///   notwithstanding the names PKWare uses for these time formats, any of them
		///   can be read and written by any computer, on any operating system.  But,
		///   there are no guarantees that a program running on Mac or Linux will
		///   gracefully handle a zip file with "Windows" formatted times, or that an
		///   application that does not use DotNetZip but runs on Windows will be able to
		///   handle file times in Unix format.
		/// </para>
		///
		/// <para>
		///   When in doubt, test.  Sorry, I haven't got a complete list of tools and
		///   which sort of timestamps they can use and will tolerate.  If you get any
		///   good information and would like to pass it on, please do so and I will
		///   include that information in this documentation.
		/// </para>
		/// </remarks>
		///
		/// <example>
		///   This example shows how to save a zip file that contains file timestamps
		///   in a format normally used by Unix.
		/// <code lang="C#">
		/// using (var zip = new ZipFile())
		/// {
		///     // produce a zip file the Mac will like
		///     zip.EmitTimesInWindowsFormatWhenSaving = false;
		///     zip.EmitTimesInUnixFormatWhenSaving = true;
		///     zip.AddDirectory(directoryToZip, "files");
		///     zip.Save(outputFile);
		/// }
		/// </code>
		///
		/// <code lang="VB">
		/// Using zip As New ZipFile
		///     '' produce a zip file the Mac will like
		///     zip.EmitTimesInWindowsFormatWhenSaving = False
		///     zip.EmitTimesInUnixFormatWhenSaving = True
		///     zip.AddDirectory(directoryToZip, "files")
		///     zip.Save(outputFile)
		/// End Using
		/// </code>
		/// </example>
		///
		/// <seealso cref="P:Ionic.Zip.ZipEntry.EmitTimesInWindowsFormatWhenSaving" />
		/// <seealso cref="P:Ionic.Zip.ZipFile.EmitTimesInUnixFormatWhenSaving" />
		public bool EmitTimesInWindowsFormatWhenSaving
		{
			get
			{
				return _emitNtfsTimes;
			}
			set
			{
				_emitNtfsTimes = value;
			}
		}

		/// <summary>
		/// Specifies whether the Creation, Access, and Modified times
		/// for entries added to the zip file will be emitted in "Unix(tm)
		/// format" when the zip archive is saved.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   An application creating a zip archive can use this flag to explicitly
		///   specify that the file times for the entries should or should not be stored
		///   in the zip archive in the format used by Unix. By default this flag is
		///   <c>false</c>, meaning the Unix-format times are not stored in the zip
		///   archive.
		/// </para>
		///
		/// <para>
		///   When adding an entry from a file or directory, the Creation (<see cref="P:Ionic.Zip.ZipEntry.CreationTime" />), Access (<see cref="P:Ionic.Zip.ZipEntry.AccessedTime" />), and Modified (<see cref="P:Ionic.Zip.ZipEntry.ModifiedTime" />) times for the given entry are
		///   automatically set from the filesystem values. When adding an entry from a
		///   stream or string, all three values are implicitly set to DateTime.Now.
		///   Applications can also explicitly set those times by calling <see cref="M:Ionic.Zip.ZipEntry.SetEntryTimes(System.DateTime,System.DateTime,System.DateTime)" />.
		/// </para>
		///
		/// <para>
		///   <see href="http://www.pkware.com/documents/casestudies/APPNOTE.TXT">PKWARE's
		///   zip specification</see> describes multiple ways to format these times in a
		///   zip file. One is the format Windows applications normally use: 100ns ticks
		///   since January 1, 1601 UTC.  The other is a format Unix applications
		///   typically use: seconds since January 1, 1970 UTC.  Each format can be
		///   stored in an "extra field" in the zip entry when saving the zip
		///   archive. The former uses an extra field with a Header Id of 0x000A, while
		///   the latter uses a header ID of 0x5455, although you probably don't need to
		///   know that.
		/// </para>
		///
		/// <para>
		///   Not all tools and libraries can interpret these fields.  Windows
		///   compressed folders is one that can read the Windows Format timestamps,
		///   while I believe the <see href="http://www.info-zip.org/">Infozip</see>
		///   tools can read the Unix format timestamps. Some tools and libraries may be
		///   able to read only one or the other.  DotNetZip can read or write times in
		///   either or both formats.
		/// </para>
		///
		/// <para>
		///   The times stored are taken from <see cref="P:Ionic.Zip.ZipEntry.ModifiedTime" />, <see cref="P:Ionic.Zip.ZipEntry.AccessedTime" />, and <see cref="P:Ionic.Zip.ZipEntry.CreationTime" />.
		/// </para>
		///
		/// <para>
		///   This property is not mutually exclusive of the <see cref="P:Ionic.Zip.ZipFile.EmitTimesInWindowsFormatWhenSaving" /> property. It is possible and
		///   legal and valid to produce a zip file that contains timestamps encoded in
		///   the Unix format as well as in the Windows format, in addition to the <see cref="P:Ionic.Zip.ZipEntry.LastModified">LastModified</see> time attached to each
		///   entry in the zip archive, a time that is always stored in "DOS
		///   format". And, notwithstanding the names PKWare uses for these time
		///   formats, any of them can be read and written by any computer, on any
		///   operating system.  But, there are no guarantees that a program running on
		///   Mac or Linux will gracefully handle a zip file with "Windows" formatted
		///   times, or that an application that does not use DotNetZip but runs on
		///   Windows will be able to handle file times in Unix format.
		/// </para>
		///
		/// <para>
		///   When in doubt, test.  Sorry, I haven't got a complete list of tools and
		///   which sort of timestamps they can use and will tolerate.  If you get any
		///   good information and would like to pass it on, please do so and I will
		///   include that information in this documentation.
		/// </para>
		/// </remarks>
		///
		/// <seealso cref="P:Ionic.Zip.ZipEntry.EmitTimesInUnixFormatWhenSaving" />
		/// <seealso cref="P:Ionic.Zip.ZipFile.EmitTimesInWindowsFormatWhenSaving" />
		public bool EmitTimesInUnixFormatWhenSaving
		{
			get
			{
				return _emitUnixTimes;
			}
			set
			{
				_emitUnixTimes = value;
			}
		}

		/// <summary>
		///   Indicates whether verbose output is sent to the <see cref="P:Ionic.Zip.ZipFile.StatusMessageTextWriter" /> during <c>AddXxx()</c> and
		///   <c>ReadXxx()</c> operations.
		/// </summary>
		///
		/// <remarks>
		///   This is a <em>synthetic</em> property.  It returns true if the <see cref="P:Ionic.Zip.ZipFile.StatusMessageTextWriter" /> is non-null.
		/// </remarks>
		internal bool Verbose => _StatusMessageTextWriter != null;

		/// <summary>
		///   Indicates whether to perform case-sensitive matching on the filename when
		///   retrieving entries in the zipfile via the string-based indexer.
		/// </summary>
		///
		/// <remarks>
		///   The default value is <c>false</c>, which means don't do case-sensitive
		///   matching. In other words, retrieving zip["ReadMe.Txt"] is the same as
		///   zip["readme.txt"].  It really makes sense to set this to <c>true</c> only
		///   if you are not running on Windows, which has case-insensitive
		///   filenames. But since this library is not built for non-Windows platforms,
		///   in most cases you should just leave this property alone.
		/// </remarks>
		public bool CaseSensitiveRetrieval
		{
			get
			{
				return _CaseSensitiveRetrieval;
			}
			set
			{
				_CaseSensitiveRetrieval = value;
			}
		}

		private Dictionary<string, ZipEntry> RetrievalEntries
		{
			get
			{
				if (!CaseSensitiveRetrieval)
				{
					return _entriesInsensitive;
				}
				return _entries;
			}
		}

		/// <summary>
		///   Indicates whether to ignore duplicate files (report only the first entry)
		///   when loading a zipfile.
		/// </summary>
		///
		/// <remarks>
		///   The default value is <c>false</c>, which will try to make all files
		///   available (duplicates will have a "copy" suffix appended to their name).
		///   Setting this to <c>true</c> prior to using <c>Initialize</c> to read a
		///   zipfile will prevent this and instead just ignore the duplicates.
		/// </remarks>
		public bool IgnoreDuplicateFiles
		{
			get
			{
				return _IgnoreDuplicateFiles;
			}
			set
			{
				_IgnoreDuplicateFiles = value;
			}
		}

		/// <summary>
		///   Indicates whether to encode entry filenames and entry comments using Unicode
		///   (UTF-8).
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   <see href="http://www.pkware.com/documents/casestudies/APPNOTE.TXT">The
		///   PKWare zip specification</see> provides for encoding file names and file
		///   comments in either the IBM437 code page, or in UTF-8.  This flag selects
		///   the encoding according to that specification.  By default, this flag is
		///   false, and filenames and comments are encoded into the zip file in the
		///   IBM437 codepage.  Setting this flag to true will specify that filenames
		///   and comments that cannot be encoded with IBM437 will be encoded with
		///   UTF-8.
		/// </para>
		///
		/// <para>
		///   Zip files created with strict adherence to the PKWare specification with
		///   respect to UTF-8 encoding can contain entries with filenames containing
		///   any combination of Unicode characters, including the full range of
		///   characters from Chinese, Latin, Hebrew, Greek, Cyrillic, and many other
		///   alphabets.  However, because at this time, the UTF-8 portion of the PKWare
		///   specification is not broadly supported by other zip libraries and
		///   utilities, such zip files may not be readable by your favorite zip tool or
		///   archiver. In other words, interoperability will decrease if you set this
		///   flag to true.
		/// </para>
		///
		/// <para>
		///   In particular, Zip files created with strict adherence to the PKWare
		///   specification with respect to UTF-8 encoding will not work well with
		///   Explorer in Windows XP or Windows Vista, because Windows compressed
		///   folders, as far as I know, do not support UTF-8 in zip files.  Vista can
		///   read the zip files, but shows the filenames incorrectly. Unpacking from
		///   Windows Vista Explorer will result in filenames that have rubbish
		///   characters in place of the high-order UTF-8 bytes.
		/// </para>
		///
		/// <para>
		///   Also, zip files that use UTF-8 encoding will not work well with Java
		///   applications that use the java.util.zip classes, as of v5.0 of the Java
		///   runtime. The Java runtime does not correctly implement the PKWare
		///   specification in this regard.
		/// </para>
		///
		/// <para>
		///   As a result, we have the unfortunate situation that "correct" behavior by
		///   the DotNetZip library with regard to Unicode encoding of filenames during
		///   zip creation will result in zip files that are readable by strictly
		///   compliant and current tools (for example the most recent release of the
		///   commercial WinZip tool); but these zip files will not be readable by
		///   various other tools or libraries, including Windows Explorer.
		/// </para>
		///
		/// <para>
		///   The DotNetZip library can read and write zip files with UTF8-encoded
		///   entries, according to the PKware spec.  If you use DotNetZip for both
		///   creating and reading the zip file, and you use UTF-8, there will be no
		///   loss of information in the filenames. For example, using a self-extractor
		///   created by this library will allow you to unpack files correctly with no
		///   loss of information in the filenames.
		/// </para>
		///
		/// <para>
		///   If you do not set this flag, it will remain false.  If this flag is false,
		///   your <c>ZipFile</c> will encode all filenames and comments using the
		///   IBM437 codepage.  This can cause "loss of information" on some filenames,
		///   but the resulting zipfile will be more interoperable with other
		///   utilities. As an example of the loss of information, diacritics can be
		///   lost.  The o-tilde character will be down-coded to plain o.  The c with a
		///   cedilla (Unicode 0xE7) used in Portugese will be downcoded to a c.
		///   Likewise, the O-stroke character (Unicode 248), used in Danish and
		///   Norwegian, will be down-coded to plain o. Chinese characters cannot be
		///   represented in codepage IBM437; when using the default encoding, Chinese
		///   characters in filenames will be represented as ?. These are all examples
		///   of "information loss".
		/// </para>
		///
		/// <para>
		///   The loss of information associated to the use of the IBM437 encoding is
		///   inconvenient, and can also lead to runtime errors. For example, using
		///   IBM437, any sequence of 4 Chinese characters will be encoded as ????.  If
		///   your application creates a <c>ZipFile</c>, then adds two files, each with
		///   names of four Chinese characters each, this will result in a duplicate
		///   filename exception.  In the case where you add a single file with a name
		///   containing four Chinese characters, calling Extract() on the entry that
		///   has question marks in the filename will result in an exception, because
		///   the question mark is not legal for use within filenames on Windows.  These
		///   are just a few examples of the problems associated to loss of information.
		/// </para>
		///
		/// <para>
		///   This flag is independent of the encoding of the content within the entries
		///   in the zip file. Think of the zip file as a container - it supports an
		///   encoding.  Within the container are other "containers" - the file entries
		///   themselves.  The encoding within those entries is independent of the
		///   encoding of the zip archive container for those entries.
		/// </para>
		///
		/// <para>
		///   Rather than specify the encoding in a binary fashion using this flag, an
		///   application can specify an arbitrary encoding via the <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" /> property.  Setting the encoding
		///   explicitly when creating zip archives will result in non-compliant zip
		///   files that, curiously, are fairly interoperable.  The challenge is, the
		///   PKWare specification does not provide for a way to specify that an entry
		///   in a zip archive uses a code page that is neither IBM437 nor UTF-8.
		///   Therefore if you set the encoding explicitly when creating a zip archive,
		///   you must take care upon reading the zip archive to use the same code page.
		///   If you get it wrong, the behavior is undefined and may result in incorrect
		///   filenames, exceptions, stomach upset, hair loss, and acne.
		/// </para>
		/// </remarks>
		/// <seealso cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" />
		[Obsolete("Beginning with v1.9.1.6 of DotNetZip, this property is obsolete.  It will be removed in a future version of the library. Your applications should  use AlternateEncoding and AlternateEncodingUsage instead.")]
		public bool UseUnicodeAsNecessary
		{
			get
			{
				if (_alternateEncoding == Encoding.GetEncoding("UTF-8"))
				{
					return _alternateEncodingUsage == ZipOption.AsNecessary;
				}
				return false;
			}
			set
			{
				if (value)
				{
					_alternateEncoding = Encoding.GetEncoding("UTF-8");
					_alternateEncodingUsage = ZipOption.AsNecessary;
				}
				else
				{
					_alternateEncoding = DefaultEncoding;
					_alternateEncodingUsage = ZipOption.Default;
				}
			}
		}

		/// <summary>
		///   Specify whether to use ZIP64 extensions when saving a zip archive.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   When creating a zip file, the default value for the property is <see cref="F:Ionic.Zip.Zip64Option.Never" />. <see cref="F:Ionic.Zip.Zip64Option.AsNecessary" /> is
		///   safest, in the sense that you will not get an Exception if a pre-ZIP64
		///   limit is exceeded.
		/// </para>
		///
		/// <para>
		///   You may set the property at any time before calling Save().
		/// </para>
		///
		/// <para>
		///   When reading a zip file via the <c>Zipfile.Read()</c> method, DotNetZip
		///   will properly read ZIP64-endowed zip archives, regardless of the value of
		///   this property.  DotNetZip will always read ZIP64 archives.  This property
		///   governs only whether DotNetZip will write them. Therefore, when updating
		///   archives, be careful about setting this property after reading an archive
		///   that may use ZIP64 extensions.
		/// </para>
		///
		/// <para>
		///   An interesting question is, if you have set this property to
		///   <c>AsNecessary</c>, and then successfully saved, does the resulting
		///   archive use ZIP64 extensions or not?  To learn this, check the <see cref="P:Ionic.Zip.ZipFile.OutputUsedZip64" /> property, after calling <c>Save()</c>.
		/// </para>
		///
		/// <para>
		///   Have you thought about
		///   <see href="http://cheeso.members.winisp.net/DotNetZipDonate.aspx">donating</see>?
		/// </para>
		///
		/// </remarks>
		/// <seealso cref="P:Ionic.Zip.ZipFile.RequiresZip64" />
		public Zip64Option UseZip64WhenSaving
		{
			get
			{
				return _zip64;
			}
			set
			{
				_zip64 = value;
			}
		}

		/// <summary>
		///   Indicates whether the archive requires ZIP64 extensions.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   This property is <c>null</c> (or <c>Nothing</c> in VB) if the archive has
		///   not been saved, and there are fewer than 65334 <c>ZipEntry</c> items
		///   contained in the archive.
		/// </para>
		///
		/// <para>
		///   The <c>Value</c> is true if any of the following four conditions holds:
		///   the uncompressed size of any entry is larger than 0xFFFFFFFF; the
		///   compressed size of any entry is larger than 0xFFFFFFFF; the relative
		///   offset of any entry within the zip archive is larger than 0xFFFFFFFF; or
		///   there are more than 65534 entries in the archive.  (0xFFFFFFFF =
		///   4,294,967,295).  The result may not be known until a <c>Save()</c> is attempted
		///   on the zip archive.  The Value of this <see cref="T:System.Nullable" />
		///   property may be set only AFTER one of the Save() methods has been called.
		/// </para>
		///
		/// <para>
		///   If none of the four conditions holds, and the archive has been saved, then
		///   the <c>Value</c> is false.
		/// </para>
		///
		/// <para>
		///   A <c>Value</c> of false does not indicate that the zip archive, as saved,
		///   does not use ZIP64.  It merely indicates that ZIP64 is not required.  An
		///   archive may use ZIP64 even when not required if the <see cref="P:Ionic.Zip.ZipFile.UseZip64WhenSaving" /> property is set to <see cref="F:Ionic.Zip.Zip64Option.Always" />, or if the <see cref="P:Ionic.Zip.ZipFile.UseZip64WhenSaving" /> property is set to <see cref="F:Ionic.Zip.Zip64Option.AsNecessary" /> and the output stream was not
		///   seekable. Use the <see cref="P:Ionic.Zip.ZipFile.OutputUsedZip64" /> property to determine if
		///   the most recent <c>Save()</c> method resulted in an archive that utilized
		///   the ZIP64 extensions.
		/// </para>
		///
		/// </remarks>
		/// <seealso cref="P:Ionic.Zip.ZipFile.UseZip64WhenSaving" />
		/// <seealso cref="P:Ionic.Zip.ZipFile.OutputUsedZip64" />
		public bool? RequiresZip64
		{
			get
			{
				if (_entries.Count > 65534)
				{
					return true;
				}
				if (!_hasBeenSaved || _contentsChanged)
				{
					return null;
				}
				foreach (ZipEntry value in _entries.Values)
				{
					if (value.RequiresZip64.Value)
					{
						return true;
					}
				}
				return false;
			}
		}

		/// <summary>
		///   Indicates whether the most recent <c>Save()</c> operation used ZIP64 extensions.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   The use of ZIP64 extensions within an archive is not always necessary, and
		///   for interoperability concerns, it may be desired to NOT use ZIP64 if
		///   possible.  The <see cref="P:Ionic.Zip.ZipFile.UseZip64WhenSaving" /> property can be
		///   set to use ZIP64 extensions only when necessary.  In those cases,
		///   Sometimes applications want to know whether a Save() actually used ZIP64
		///   extensions.  Applications can query this read-only property to learn
		///   whether ZIP64 has been used in a just-saved <c>ZipFile</c>.
		/// </para>
		///
		/// <para>
		///   The value is <c>null</c> (or <c>Nothing</c> in VB) if the archive has not
		///   been saved.
		/// </para>
		///
		/// <para>
		///   Non-null values (<c>HasValue</c> is true) indicate whether ZIP64
		///   extensions were used during the most recent <c>Save()</c> operation.  The
		///   ZIP64 extensions may have been used as required by any particular entry
		///   because of its uncompressed or compressed size, or because the archive is
		///   larger than 4294967295 bytes, or because there are more than 65534 entries
		///   in the archive, or because the <c>UseZip64WhenSaving</c> property was set
		///   to <see cref="F:Ionic.Zip.Zip64Option.Always" />, or because the
		///   <c>UseZip64WhenSaving</c> property was set to <see cref="F:Ionic.Zip.Zip64Option.AsNecessary" /> and the output stream was not seekable.
		///   The value of this property does not indicate the reason the ZIP64
		///   extensions were used.
		/// </para>
		///
		/// </remarks>
		/// <seealso cref="P:Ionic.Zip.ZipFile.UseZip64WhenSaving" />
		/// <seealso cref="P:Ionic.Zip.ZipFile.RequiresZip64" />
		public bool? OutputUsedZip64 => _OutputUsesZip64;

		/// <summary>
		///   Indicates whether the most recent <c>Read()</c> operation read a zip file that uses
		///   ZIP64 extensions.
		/// </summary>
		///
		/// <remarks>
		///   This property will return null (Nothing in VB) if you've added an entry after reading
		///   the zip file.
		/// </remarks>
		public bool? InputUsesZip64
		{
			get
			{
				if (_entries.Count > 65534)
				{
					return true;
				}
				using (IEnumerator<ZipEntry> enumerator = GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ZipEntry current = enumerator.Current;
						if (current.Source != ZipEntrySource.ZipFile)
						{
							return null;
						}
						if (current._InputUsesZip64)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		/// <summary>
		///   The text encoding to use when writing new entries to the <c>ZipFile</c>,
		///   for those entries that cannot be encoded with the default (IBM437)
		///   encoding; or, the text encoding that was used when reading the entries
		///   from the <c>ZipFile</c>.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   In <see href="http://www.pkware.com/documents/casestudies/APPNOTE.TXT">its
		///   zip specification</see>, PKWare describes two options for encoding
		///   filenames and comments: using IBM437 or UTF-8.  But, some archiving tools
		///   or libraries do not follow the specification, and instead encode
		///   characters using the system default code page.  For example, WinRAR when
		///   run on a machine in Shanghai may encode filenames with the Big-5 Chinese
		///   (950) code page.  This behavior is contrary to the Zip specification, but
		///   it occurs anyway.
		/// </para>
		///
		/// <para>
		///   When using DotNetZip to write zip archives that will be read by one of
		///   these other archivers, set this property to specify the code page to use
		///   when encoding the <see cref="P:Ionic.Zip.ZipEntry.FileName" /> and <see cref="P:Ionic.Zip.ZipEntry.Comment" /> for each <c>ZipEntry</c> in the zip file, for
		///   values that cannot be encoded with the default codepage for zip files,
		///   IBM437.  This is why this property is "provisional".  In all cases, IBM437
		///   is used where possible, in other words, where no loss of data would
		///   result. It is possible, therefore, to have a given entry with a
		///   <c>Comment</c> encoded in IBM437 and a <c>FileName</c> encoded with the
		///   specified "provisional" codepage.
		/// </para>
		///
		/// <para>
		///   Be aware that a zip file created after you've explicitly set the <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" /> property to a value other than
		///   IBM437 may not be compliant to the PKWare specification, and may not be
		///   readable by compliant archivers.  On the other hand, many (most?)
		///   archivers are non-compliant and can read zip files created in arbitrary
		///   code pages.  The trick is to use or specify the proper codepage when
		///   reading the zip.
		/// </para>
		///
		/// <para>
		///   When creating a zip archive using this library, it is possible to change
		///   the value of <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" /> between each
		///   entry you add, and between adding entries and the call to
		///   <c>Save()</c>. Don't do this. It will likely result in a zipfile that is
		///   not readable.  For best interoperability, either leave <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" /> alone, or specify it only once,
		///   before adding any entries to the <c>ZipFile</c> instance.  There is one
		///   exception to this recommendation, described later.
		/// </para>
		///
		/// <para>
		///   When using an arbitrary, non-UTF8 code page for encoding, there is no
		///   standard way for the creator application - whether DotNetZip, WinZip,
		///   WinRar, or something else - to formally specify in the zip file which
		///   codepage has been used for the entries. As a result, readers of zip files
		///   are not able to inspect the zip file and determine the codepage that was
		///   used for the entries contained within it.  It is left to the application
		///   or user to determine the necessary codepage when reading zip files encoded
		///   this way.  In other words, if you explicitly specify the codepage when you
		///   create the zipfile, you must explicitly specify the same codepage when
		///   reading the zipfile.
		/// </para>
		///
		/// <para>
		///   The way you specify the code page to use when reading a zip file varies
		///   depending on the tool or library you use to read the zip.  In DotNetZip,
		///   you use a ZipFile.Read() method that accepts an encoding parameter.  It
		///   isn't possible with Windows Explorer, as far as I know, to specify an
		///   explicit codepage to use when reading a zip.  If you use an incorrect
		///   codepage when reading a zipfile, you will get entries with filenames that
		///   are incorrect, and the incorrect filenames may even contain characters
		///   that are not legal for use within filenames in Windows. Extracting entries
		///   with illegal characters in the filenames will lead to exceptions. It's too
		///   bad, but this is just the way things are with code pages in zip
		///   files. Caveat Emptor.
		/// </para>
		///
		/// <para>
		///   Example: Suppose you create a zipfile that contains entries with
		///   filenames that have Danish characters.  If you use <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" /> equal to "iso-8859-1" (cp 28591),
		///   the filenames will be correctly encoded in the zip.  But, to read that
		///   zipfile correctly, you have to specify the same codepage at the time you
		///   read it. If try to read that zip file with Windows Explorer or another
		///   application that is not flexible with respect to the codepage used to
		///   decode filenames in zipfiles, you will get a filename like "Inf�.txt".
		/// </para>
		///
		/// <para>
		///   When using DotNetZip to read a zip archive, and the zip archive uses an
		///   arbitrary code page, you must specify the encoding to use before or when
		///   the <c>Zipfile</c> is READ.  This means you must use a <c>ZipFile.Read()</c>
		///   method that allows you to specify a System.Text.Encoding parameter.  Setting
		///   the ProvisionalAlternateEncoding property after your application has read in
		///   the zip archive will not affect the entry names of entries that have already
		///   been read in.
		/// </para>
		///
		/// <para>
		///   And now, the exception to the rule described above.  One strategy for
		///   specifying the code page for a given zip file is to describe the code page
		///   in a human-readable form in the Zip comment. For example, the comment may
		///   read "Entries in this archive are encoded in the Big5 code page".  For
		///   maximum interoperability, the zip comment in this case should be encoded
		///   in the default, IBM437 code page.  In this case, the zip comment is
		///   encoded using a different page than the filenames.  To do this, Specify
		///   <c>ProvisionalAlternateEncoding</c> to your desired region-specific code
		///   page, once before adding any entries, and then reset
		///   <c>ProvisionalAlternateEncoding</c> to IBM437 before setting the <see cref="P:Ionic.Zip.ZipFile.Comment" /> property and calling Save().
		/// </para>
		/// </remarks>
		///
		/// <example>
		/// This example shows how to read a zip file using the Big-5 Chinese code page
		/// (950), and extract each entry in the zip file.  For this code to work as
		/// desired, the <c>Zipfile</c> must have been created using the big5 code page
		/// (CP950). This is typical, for example, when using WinRar on a machine with
		/// CP950 set as the default code page.  In that case, the names of entries
		/// within the Zip archive will be stored in that code page, and reading the zip
		/// archive must be done using that code page.  If the application did not use
		/// the correct code page in <c>ZipFile.Read()</c>, then names of entries within the
		/// zip archive would not be correctly retrieved.
		/// <code>
		/// using (var zip = ZipFile.Read(zipFileName, System.Text.Encoding.GetEncoding("big5")))
		/// {
		///     // retrieve and extract an entry using a name encoded with CP950
		///     zip[MyDesiredEntry].Extract("unpack");
		/// }
		/// </code>
		///
		/// <code lang="VB">
		/// Using zip As ZipFile = ZipFile.Read(ZipToExtract, System.Text.Encoding.GetEncoding("big5"))
		///     ' retrieve and extract an entry using a name encoded with CP950
		///     zip(MyDesiredEntry).Extract("unpack")
		/// End Using
		/// </code>
		/// </example>
		///
		/// <seealso cref="P:Ionic.Zip.ZipFile.DefaultEncoding">DefaultEncoding</seealso>
		[Obsolete("use AlternateEncoding instead.")]
		public Encoding ProvisionalAlternateEncoding
		{
			get
			{
				if (_alternateEncodingUsage == ZipOption.AsNecessary)
				{
					return _alternateEncoding;
				}
				return null;
			}
			set
			{
				_alternateEncoding = value;
				_alternateEncodingUsage = ZipOption.AsNecessary;
			}
		}

		/// <summary>
		///   A Text Encoding to use when encoding the filenames and comments for
		///   all the ZipEntry items, during a ZipFile.Save() operation.
		/// </summary>
		/// <remarks>
		///   <para>
		///     Whether the encoding specified here is used during the save depends
		///     on <see cref="P:Ionic.Zip.ZipFile.AlternateEncodingUsage" />.
		///   </para>
		/// </remarks>
		public Encoding AlternateEncoding
		{
			get
			{
				return _alternateEncoding;
			}
			set
			{
				_alternateEncoding = value;
			}
		}

		/// <summary>
		///   A flag that tells if and when this instance should apply
		///   AlternateEncoding to encode the filenames and comments associated to
		///   of ZipEntry objects contained within this instance.
		/// </summary>
		public ZipOption AlternateEncodingUsage
		{
			get
			{
				return _alternateEncodingUsage;
			}
			set
			{
				_alternateEncodingUsage = value;
			}
		}

		/// <summary>
		/// Gets or sets the <c>TextWriter</c> to which status messages are delivered
		/// for the instance.
		/// </summary>
		///
		/// <remarks>
		///   If the TextWriter is set to a non-null value, then verbose output is sent
		///   to the <c>TextWriter</c> during <c>Add</c><c>, Read</c><c>, Save</c> and
		///   <c>Extract</c> operations.  Typically, console applications might use
		///   <c>Console.Out</c> and graphical or headless applications might use a
		///   <c>System.IO.StringWriter</c>. The output of this is suitable for viewing
		///   by humans.
		/// </remarks>
		///
		/// <example>
		/// <para>
		///   In this example, a console application instantiates a <c>ZipFile</c>, then
		///   sets the <c>StatusMessageTextWriter</c> to <c>Console.Out</c>.  At that
		///   point, all verbose status messages for that <c>ZipFile</c> are sent to the
		///   console.
		/// </para>
		///
		/// <code lang="C#">
		/// using (ZipFile zip= ZipFile.Read(FilePath))
		/// {
		///   zip.StatusMessageTextWriter= System.Console.Out;
		///   // messages are sent to the console during extraction
		///   zip.ExtractAll();
		/// }
		/// </code>
		///
		/// <code lang="VB">
		/// Using zip As ZipFile = ZipFile.Read(FilePath)
		///   zip.StatusMessageTextWriter= System.Console.Out
		///   'Status Messages will be sent to the console during extraction
		///   zip.ExtractAll()
		/// End Using
		/// </code>
		///
		/// <para>
		///   In this example, a Windows Forms application instantiates a
		///   <c>ZipFile</c>, then sets the <c>StatusMessageTextWriter</c> to a
		///   <c>StringWriter</c>.  At that point, all verbose status messages for that
		///   <c>ZipFile</c> are sent to the <c>StringWriter</c>.
		/// </para>
		///
		/// <code lang="C#">
		/// var sw = new System.IO.StringWriter();
		/// using (ZipFile zip= ZipFile.Read(FilePath))
		/// {
		///   zip.StatusMessageTextWriter= sw;
		///   zip.ExtractAll();
		/// }
		/// Console.WriteLine("{0}", sw.ToString());
		/// </code>
		///
		/// <code lang="VB">
		/// Dim sw as New System.IO.StringWriter
		/// Using zip As ZipFile = ZipFile.Read(FilePath)
		///   zip.StatusMessageTextWriter= sw
		///   zip.ExtractAll()
		/// End Using
		/// 'Status Messages are now available in sw
		///
		/// </code>
		/// </example>
		public TextWriter StatusMessageTextWriter
		{
			get
			{
				return _StatusMessageTextWriter;
			}
			set
			{
				_StatusMessageTextWriter = value;
			}
		}

		/// <summary>
		///   Gets or sets the name for the folder to store the temporary file
		///   this library writes when saving a zip archive.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This library will create a temporary file when saving a Zip archive to a
		///   file.  This file is written when calling one of the <c>Save()</c> methods
		///   that does not save to a stream, or one of the <c>SaveSelfExtractor()</c>
		///   methods.
		/// </para>
		///
		/// <para>
		///   By default, the library will create the temporary file in the directory
		///   specified for the file itself, via the <see cref="P:Ionic.Zip.ZipFile.Name" /> property or via
		///   the <see cref="M:Ionic.Zip.ZipFile.Save(System.String)" /> method.
		/// </para>
		///
		/// <para>
		///   Setting this property allows applications to override this default
		///   behavior, so that the library will create the temporary file in the
		///   specified folder. For example, to have the library create the temporary
		///   file in the current working directory, regardless where the <c>ZipFile</c>
		///   is saved, specfy ".".  To revert to the default behavior, set this
		///   property to <c>null</c> (<c>Nothing</c> in VB).
		/// </para>
		///
		/// <para>
		///   When setting the property to a non-null value, the folder specified must
		///   exist; if it does not an exception is thrown.  The application should have
		///   write and delete permissions on the folder.  The permissions are not
		///   explicitly checked ahead of time; if the application does not have the
		///   appropriate rights, an exception will be thrown at the time <c>Save()</c>
		///   is called.
		/// </para>
		///
		/// <para>
		///   There is no temporary file created when reading a zip archive.  When
		///   saving to a Stream, there is no temporary file created.  For example, if
		///   the application is an ASP.NET application and calls <c>Save()</c>
		///   specifying the <c>Response.OutputStream</c> as the output stream, there is
		///   no temporary file created.
		/// </para>
		/// </remarks>
		///
		/// <exception cref="T:System.IO.FileNotFoundException">
		/// Thrown when setting the property if the directory does not exist.
		/// </exception>
		public string TempFileFolder
		{
			get
			{
				return _TempFileFolder;
			}
			set
			{
				_TempFileFolder = value;
				if (value == null || Directory.Exists(value))
				{
					return;
				}
				throw new FileNotFoundException($"That directory ({value}) does not exist.");
			}
		}

		/// <summary>
		/// Sets the password to be used on the <c>ZipFile</c> instance.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   When writing a zip archive, this password is applied to the entries, not
		///   to the zip archive itself. It applies to any <c>ZipEntry</c> subsequently
		///   added to the <c>ZipFile</c>, using one of the <c>AddFile</c>,
		///   <c>AddDirectory</c>, <c>AddEntry</c>, or <c>AddItem</c> methods, etc.
		///   When reading a zip archive, this property applies to any entry
		///   subsequently extracted from the <c>ZipFile</c> using one of the Extract
		///   methods on the <c>ZipFile</c> class.
		/// </para>
		///
		/// <para>
		///   When writing a zip archive, keep this in mind: though the password is set
		///   on the ZipFile object, according to the Zip spec, the "directory" of the
		///   archive - in other words the list of entries or files contained in the archive - is
		///   not encrypted with the password, or protected in any way.  If you set the
		///   Password property, the password actually applies to individual entries
		///   that are added to the archive, subsequent to the setting of this property.
		///   The list of filenames in the archive that is eventually created will
		///   appear in clear text, but the contents of the individual files are
		///   encrypted.  This is how Zip encryption works.
		/// </para>
		///
		/// <para>
		///   One simple way around this limitation is to simply double-wrap sensitive
		///   filenames: Store the files in a zip file, and then store that zip file
		///   within a second, "outer" zip file.  If you apply a password to the outer
		///   zip file, then readers will be able to see that the outer zip file
		///   contains an inner zip file.  But readers will not be able to read the
		///   directory or file list of the inner zip file.
		/// </para>
		///
		/// <para>
		///   If you set the password on the <c>ZipFile</c>, and then add a set of files
		///   to the archive, then each entry is encrypted with that password.  You may
		///   also want to change the password between adding different entries. If you
		///   set the password, add an entry, then set the password to <c>null</c>
		///   (<c>Nothing</c> in VB), and add another entry, the first entry is
		///   encrypted and the second is not.  If you call <c>AddFile()</c>, then set
		///   the <c>Password</c> property, then call <c>ZipFile.Save</c>, the file
		///   added will not be password-protected, and no warning will be generated.
		/// </para>
		///
		/// <para>
		///   When setting the Password, you may also want to explicitly set the <see cref="P:Ionic.Zip.ZipFile.Encryption" /> property, to specify how to encrypt the entries added
		///   to the ZipFile.  If you set the Password to a non-null value and do not
		///   set <see cref="P:Ionic.Zip.ZipFile.Encryption" />, then PKZip 2.0 ("Weak") encryption is used.
		///   This encryption is relatively weak but is very interoperable. If you set
		///   the password to a <c>null</c> value (<c>Nothing</c> in VB), Encryption is
		///   reset to None.
		/// </para>
		///
		/// <para>
		///   All of the preceding applies to writing zip archives, in other words when
		///   you use one of the Save methods.  To use this property when reading or an
		///   existing ZipFile, do the following: set the Password property on the
		///   <c>ZipFile</c>, then call one of the Extract() overloads on the <see cref="T:Ionic.Zip.ZipEntry" />. In this case, the entry is extracted using the
		///   <c>Password</c> that is specified on the <c>ZipFile</c> instance. If you
		///   have not set the <c>Password</c> property, then the password is
		///   <c>null</c>, and the entry is extracted with no password.
		/// </para>
		///
		/// <para>
		///   If you set the Password property on the <c>ZipFile</c>, then call
		///   <c>Extract()</c> an entry that has not been encrypted with a password, the
		///   password is not used for that entry, and the <c>ZipEntry</c> is extracted
		///   as normal. In other words, the password is used only if necessary.
		/// </para>
		///
		/// <para>
		///   The <see cref="T:Ionic.Zip.ZipEntry" /> class also has a <see cref="P:Ionic.Zip.ZipEntry.Password">Password</see> property.  It takes precedence
		///   over this property on the <c>ZipFile</c>.  Typically, you would use the
		///   per-entry Password when most entries in the zip archive use one password,
		///   and a few entries use a different password.  If all entries in the zip
		///   file use the same password, then it is simpler to just set this property
		///   on the <c>ZipFile</c> itself, whether creating a zip archive or extracting
		///   a zip archive.
		/// </para>
		///
		/// </remarks>
		///
		/// <example>
		/// <para>
		///   This example creates a zip file, using password protection for the
		///   entries, and then extracts the entries from the zip file.  When creating
		///   the zip file, the Readme.txt file is not protected with a password, but
		///   the other two are password-protected as they are saved. During extraction,
		///   each file is extracted with the appropriate password.
		/// </para>
		/// <code>
		/// // create a file with encryption
		/// using (ZipFile zip = new ZipFile())
		/// {
		///     zip.AddFile("ReadMe.txt");
		///     zip.Password= "!Secret1";
		///     zip.AddFile("MapToTheSite-7440-N49th.png");
		///     zip.AddFile("2008-Regional-Sales-Report.pdf");
		///     zip.Save("EncryptedArchive.zip");
		/// }
		///
		/// // extract entries that use encryption
		/// using (ZipFile zip = ZipFile.Read("EncryptedArchive.zip"))
		/// {
		///     zip.Password= "!Secret1";
		///     zip.ExtractAll("extractDir");
		/// }
		///
		/// </code>
		///
		/// <code lang="VB">
		/// Using zip As New ZipFile
		///     zip.AddFile("ReadMe.txt")
		///     zip.Password = "123456!"
		///     zip.AddFile("MapToTheSite-7440-N49th.png")
		///     zip.Password= "!Secret1";
		///     zip.AddFile("2008-Regional-Sales-Report.pdf")
		///     zip.Save("EncryptedArchive.zip")
		/// End Using
		///
		///
		/// ' extract entries that use encryption
		/// Using (zip as ZipFile = ZipFile.Read("EncryptedArchive.zip"))
		///     zip.Password= "!Secret1"
		///     zip.ExtractAll("extractDir")
		/// End Using
		///
		/// </code>
		///
		/// </example>
		///
		/// <seealso cref="P:Ionic.Zip.ZipFile.Encryption">ZipFile.Encryption</seealso>
		/// <seealso cref="P:Ionic.Zip.ZipEntry.Password">ZipEntry.Password</seealso>
		public string Password
		{
			private get
			{
				return _Password;
			}
			set
			{
				_Password = value;
				if (_Password == null)
				{
					Encryption = EncryptionAlgorithm.None;
				}
				else if (Encryption == EncryptionAlgorithm.None)
				{
					Encryption = EncryptionAlgorithm.PkzipWeak;
				}
			}
		}

		/// <summary>
		///   The action the library should take when extracting a file that already
		///   exists.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This property affects the behavior of the Extract methods (one of the
		///   <c>Extract()</c> or <c>ExtractWithPassword()</c> overloads), when
		///   extraction would would overwrite an existing filesystem file. If you do
		///   not set this property, the library throws an exception when extracting an
		///   entry would overwrite an existing file.
		/// </para>
		///
		/// <para>
		///   This property has no effect when extracting to a stream, or when the file
		///   to be extracted does not already exist.
		/// </para>
		/// </remarks>
		/// <seealso cref="P:Ionic.Zip.ZipEntry.ExtractExistingFile" />
		public ExtractExistingFileAction ExtractExistingFile { get; set; }

		/// <summary>
		///   The action the library should take when an error is encountered while
		///   opening or reading files as they are saved into a zip archive.
		/// </summary>
		///
		/// <remarks>
		///  <para>
		///    Errors can occur as a file is being saved to the zip archive.  For
		///    example, the File.Open may fail, or a File.Read may fail, because of
		///    lock conflicts or other reasons.
		///  </para>
		///
		///  <para>
		///    The first problem might occur after having called AddDirectory() on a
		///    directory that contains a Clipper .dbf file; the file is locked by
		///    Clipper and cannot be opened for read by another process. An example of
		///    the second problem might occur when trying to zip a .pst file that is in
		///    use by Microsoft Outlook. Outlook locks a range on the file, which allows
		///    other processes to open the file, but not read it in its entirety.
		///  </para>
		///
		///  <para>
		///    This property tells DotNetZip what you would like to do in the case of
		///    these errors.  The primary options are: <c>ZipErrorAction.Throw</c> to
		///    throw an exception (this is the default behavior if you don't set this
		///    property); <c>ZipErrorAction.Skip</c> to Skip the file for which there
		///    was an error and continue saving; <c>ZipErrorAction.Retry</c> to Retry
		///    the entry that caused the problem; or
		///    <c>ZipErrorAction.InvokeErrorEvent</c> to invoke an event handler.
		///  </para>
		///
		///  <para>
		///    This property is implicitly set to <c>ZipErrorAction.InvokeErrorEvent</c>
		///    if you add a handler to the <see cref="E:Ionic.Zip.ZipFile.ZipError" /> event.  If you set
		///    this property to something other than
		///    <c>ZipErrorAction.InvokeErrorEvent</c>, then the <c>ZipError</c>
		///    event is implicitly cleared.  What it means is you can set one or the
		///    other (or neither), depending on what you want, but you never need to set
		///    both.
		///  </para>
		///
		///  <para>
		///    As with some other properties on the <c>ZipFile</c> class, like <see cref="P:Ionic.Zip.ZipFile.Password" />, <see cref="P:Ionic.Zip.ZipFile.Encryption" />, and <see cref="P:Ionic.Zip.ZipFile.CompressionLevel" />, setting this property on a <c>ZipFile</c>
		///    instance will cause the specified <c>ZipErrorAction</c> to be used on all
		///    <see cref="T:Ionic.Zip.ZipEntry" /> items that are subsequently added to the
		///    <c>ZipFile</c> instance. If you set this property after you have added
		///    items to the <c>ZipFile</c>, but before you have called <c>Save()</c>,
		///    those items will not use the specified error handling action.
		///  </para>
		///
		///  <para>
		///    If you want to handle any errors that occur with any entry in the zip
		///    file in the same way, then set this property once, before adding any
		///    entries to the zip archive.
		///  </para>
		///
		///  <para>
		///    If you set this property to <c>ZipErrorAction.Skip</c> and you'd like to
		///    learn which files may have been skipped after a <c>Save()</c>, you can
		///    set the <see cref="P:Ionic.Zip.ZipFile.StatusMessageTextWriter" /> on the ZipFile before
		///    calling <c>Save()</c>. A message will be emitted into that writer for
		///    each skipped file, if any.
		///  </para>
		///
		/// </remarks>
		///
		/// <example>
		///   This example shows how to tell DotNetZip to skip any files for which an
		///   error is generated during the Save().
		/// <code lang="VB">
		/// Public Sub SaveZipFile()
		///     Dim SourceFolder As String = "fodder"
		///     Dim DestFile As String =  "eHandler.zip"
		///     Dim sw as New StringWriter
		///     Using zipArchive As ZipFile = New ZipFile
		///         ' Tell DotNetZip to skip any files for which it encounters an error
		///         zipArchive.ZipErrorAction = ZipErrorAction.Skip
		///         zipArchive.StatusMessageTextWriter = sw
		///         zipArchive.AddDirectory(SourceFolder)
		///         zipArchive.Save(DestFile)
		///     End Using
		///     ' examine sw here to see any messages
		/// End Sub
		///
		/// </code>
		/// </example>
		///
		/// <seealso cref="P:Ionic.Zip.ZipEntry.ZipErrorAction" />
		/// <seealso cref="E:Ionic.Zip.ZipFile.ZipError" />
		public ZipErrorAction ZipErrorAction
		{
			get
			{
				if (this.ZipError != null)
				{
					_zipErrorAction = ZipErrorAction.InvokeErrorEvent;
				}
				return _zipErrorAction;
			}
			set
			{
				_zipErrorAction = value;
				if (_zipErrorAction != ZipErrorAction.InvokeErrorEvent && this.ZipError != null)
				{
					this.ZipError = null;
				}
			}
		}

		/// <summary>
		///   The Encryption to use for entries added to the <c>ZipFile</c>.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   Set this when creating a zip archive, or when updating a zip archive. The
		///   specified Encryption is applied to the entries subsequently added to the
		///   <c>ZipFile</c> instance.  Applications do not need to set the
		///   <c>Encryption</c> property when reading or extracting a zip archive.
		/// </para>
		///
		/// <para>
		///   If you set this to something other than EncryptionAlgorithm.None, you
		///   will also need to set the <see cref="P:Ionic.Zip.ZipFile.Password" />.
		/// </para>
		///
		/// <para>
		///   As with some other properties on the <c>ZipFile</c> class, like <see cref="P:Ionic.Zip.ZipFile.Password" /> and <see cref="P:Ionic.Zip.ZipFile.CompressionLevel" />, setting this
		///   property on a <c>ZipFile</c> instance will cause the specified
		///   <c>EncryptionAlgorithm</c> to be used on all <see cref="T:Ionic.Zip.ZipEntry" /> items
		///   that are subsequently added to the <c>ZipFile</c> instance. In other
		///   words, if you set this property after you have added items to the
		///   <c>ZipFile</c>, but before you have called <c>Save()</c>, those items will
		///   not be encrypted or protected with a password in the resulting zip
		///   archive. To get a zip archive with encrypted entries, set this property,
		///   along with the <see cref="P:Ionic.Zip.ZipFile.Password" /> property, before calling
		///   <c>AddFile</c>, <c>AddItem</c>, or <c>AddDirectory</c> (etc.) on the
		///   <c>ZipFile</c> instance.
		/// </para>
		///
		/// <para>
		///   If you read a <c>ZipFile</c>, you can modify the <c>Encryption</c> on an
		///   encrypted entry, only by setting the <c>Encryption</c> property on the
		///   <c>ZipEntry</c> itself.  Setting the <c>Encryption</c> property on the
		///   <c>ZipFile</c>, once it has been created via a call to
		///   <c>ZipFile.Read()</c>, does not affect entries that were previously read.
		/// </para>
		///
		/// <para>
		///   For example, suppose you read a <c>ZipFile</c>, and there is an encrypted
		///   entry.  Setting the <c>Encryption</c> property on that <c>ZipFile</c> and
		///   then calling <c>Save()</c> on the <c>ZipFile</c> does not update the
		///   <c>Encryption</c> used for the entries in the archive.  Neither is an
		///   exception thrown. Instead, what happens during the <c>Save()</c> is that
		///   all previously existing entries are copied through to the new zip archive,
		///   with whatever encryption and password that was used when originally
		///   creating the zip archive. Upon re-reading that archive, to extract
		///   entries, applications should use the original password or passwords, if
		///   any.
		/// </para>
		///
		/// <para>
		///   Suppose an application reads a <c>ZipFile</c>, and there is an encrypted
		///   entry.  Setting the <c>Encryption</c> property on that <c>ZipFile</c> and
		///   then adding new entries (via <c>AddFile()</c>, <c>AddEntry()</c>, etc)
		///   and then calling <c>Save()</c> on the <c>ZipFile</c> does not update the
		///   <c>Encryption</c> on any of the entries that had previously been in the
		///   <c>ZipFile</c>.  The <c>Encryption</c> property applies only to the
		///   newly-added entries.
		/// </para>
		///
		/// </remarks>
		///
		/// <example>
		/// <para>
		///   This example creates a zip archive that uses encryption, and then extracts
		///   entries from the archive.  When creating the zip archive, the ReadMe.txt
		///   file is zipped without using a password or encryption.  The other files
		///   use encryption.
		/// </para>
		///
		/// <code>
		/// // Create a zip archive with AES Encryption.
		/// using (ZipFile zip = new ZipFile())
		/// {
		///     zip.AddFile("ReadMe.txt");
		///     zip.Encryption= EncryptionAlgorithm.WinZipAes256;
		///     zip.Password= "Top.Secret.No.Peeking!";
		///     zip.AddFile("7440-N49th.png");
		///     zip.AddFile("2008-Regional-Sales-Report.pdf");
		///     zip.Save("EncryptedArchive.zip");
		/// }
		///
		/// // Extract a zip archive that uses AES Encryption.
		/// // You do not need to specify the algorithm during extraction.
		/// using (ZipFile zip = ZipFile.Read("EncryptedArchive.zip"))
		/// {
		///     zip.Password= "Top.Secret.No.Peeking!";
		///     zip.ExtractAll("extractDirectory");
		/// }
		/// </code>
		///
		/// <code lang="VB">
		/// ' Create a zip that uses Encryption.
		/// Using zip As New ZipFile()
		///     zip.Encryption= EncryptionAlgorithm.WinZipAes256
		///     zip.Password= "Top.Secret.No.Peeking!"
		///     zip.AddFile("ReadMe.txt")
		///     zip.AddFile("7440-N49th.png")
		///     zip.AddFile("2008-Regional-Sales-Report.pdf")
		///     zip.Save("EncryptedArchive.zip")
		/// End Using
		///
		/// ' Extract a zip archive that uses AES Encryption.
		/// ' You do not need to specify the algorithm during extraction.
		/// Using (zip as ZipFile = ZipFile.Read("EncryptedArchive.zip"))
		///     zip.Password= "Top.Secret.No.Peeking!"
		///     zip.ExtractAll("extractDirectory")
		/// End Using
		/// </code>
		///
		/// </example>
		///
		/// <seealso cref="P:Ionic.Zip.ZipFile.Password">ZipFile.Password</seealso>
		/// <seealso cref="P:Ionic.Zip.ZipEntry.Encryption">ZipEntry.Encryption</seealso>
		public EncryptionAlgorithm Encryption
		{
			get
			{
				return _Encryption;
			}
			set
			{
				if (value == EncryptionAlgorithm.Unsupported)
				{
					throw new InvalidOperationException("You may not set Encryption to that value.");
				}
				_Encryption = value;
			}
		}

		/// <summary>
		///   A callback that allows the application to specify the compression level
		///   to use for entries subsequently added to the zip archive.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   With this callback, the DotNetZip library allows the application to
		///   determine whether compression will be used, at the time of the
		///   <c>Save</c>. This may be useful if the application wants to favor
		///   speed over size, and wants to defer the decision until the time of
		///   <c>Save</c>.
		/// </para>
		///
		/// <para>
		///   Typically applications set the <see cref="P:Ionic.Zip.ZipFile.CompressionLevel" /> property on
		///   the <c>ZipFile</c> or on each <c>ZipEntry</c> to determine the level of
		///   compression used. This is done at the time the entry is added to the
		///   <c>ZipFile</c>. Setting the property to
		///   <c>Ionic.Zlib.CompressionLevel.None</c> means no compression will be used.
		/// </para>
		///
		/// <para>
		///   This callback allows the application to defer the decision on the
		///   <c>CompressionLevel</c> to use, until the time of the call to
		///   <c>ZipFile.Save()</c>. The callback is invoked once per <c>ZipEntry</c>,
		///   at the time the data for the entry is being written out as part of a
		///   <c>Save()</c> operation. The application can use whatever criteria it
		///   likes in determining the level to return.  For example, an application may
		///   wish that no .mp3 files should be compressed, because they are already
		///   compressed and the extra compression is not worth the CPU time incurred,
		///   and so can return <c>None</c> for all .mp3 entries.
		/// </para>
		///
		/// <para>
		///   The library determines whether compression will be attempted for an entry
		///   this way: If the entry is a zero length file, or a directory, no
		///   compression is used.  Otherwise, if this callback is set, it is invoked
		///   and the <c>CompressionLevel</c> is set to the return value. If this
		///   callback has not been set, then the previously set value for
		///   <c>CompressionLevel</c> is used.
		/// </para>
		///
		/// </remarks>
		public SetCompressionCallback SetCompression { get; set; }

		/// <summary>
		/// The maximum size of an output segment, when saving a split Zip file.
		/// </summary>
		/// <remarks>
		///   <para>
		///     Make sure you do not read from this field if you've set the value using <see cref="P:Ionic.Zip.ZipFile.MaxOutputSegmentSize64" />
		///   </para>
		///
		///   <para>
		///     Set this to a non-zero value before calling <see cref="M:Ionic.Zip.ZipFile.Save" /> or <see cref="M:Ionic.Zip.ZipFile.Save(System.String)" /> to specify that the ZipFile should be saved as a
		///     split archive, also sometimes called a spanned archive. Some also
		///     call them multi-file archives.
		///   </para>
		///
		///   <para>
		///     A split zip archive is saved in a set of discrete filesystem files,
		///     rather than in a single file. This is handy when transmitting the
		///     archive in email or some other mechanism that has a limit to the size of
		///     each file.  The first file in a split archive will be named
		///     <c>basename.z01</c>, the second will be named <c>basename.z02</c>, and
		///     so on. The final file is named <c>basename.zip</c>. According to the zip
		///     specification from PKWare, the minimum value is 65536, for a 64k segment
		///     size. The maximum number of segments allows in a split archive is 99.
		///   </para>
		///
		///   <para>
		///     The value of this property determines the maximum size of a split
		///     segment when writing a split archive.  For example, suppose you have a
		///     <c>ZipFile</c> that would save to a single file of 200k. If you set the
		///     <c>MaxOutputSegmentSize</c> to 65536 before calling <c>Save()</c>, you
		///     will get four distinct output files. On the other hand if you set this
		///     property to 256k, then you will get a single-file archive for that
		///     <c>ZipFile</c>.
		///   </para>
		///
		///   <para>
		///     The size of each split output file will be as large as possible, up to
		///     the maximum size set here. The zip specification requires that some data
		///     fields in a zip archive may not span a split boundary, and an output
		///     segment may be smaller than the maximum if necessary to avoid that
		///     problem. Also, obviously the final segment of the archive may be smaller
		///     than the maximum segment size. Segments will never be larger than the
		///     value set with this property.
		///   </para>
		///
		///   <para>
		///     You can save a split Zip file only when saving to a regular filesystem
		///     file. It's not possible to save a split zip file as a self-extracting
		///     archive, nor is it possible to save a split zip file to a stream. When
		///     saving to a SFX or to a Stream, this property is ignored.
		///   </para>
		///
		///   <para>
		///     About interoperability: Split or spanned zip files produced by DotNetZip
		///     can be read by WinZip or PKZip, and vice-versa. Segmented zip files may
		///     not be readable by other tools, if those other tools don't support zip
		///     spanning or splitting.  When in doubt, test.  I don't believe Windows
		///     Explorer can extract a split archive.
		///   </para>
		///
		///   <para>
		///     This property has no effect when reading a split archive. You can read
		///     a split archive in the normal way with DotNetZip.
		///   </para>
		///
		///   <para>
		///     When saving a zip file, if you want a regular zip file rather than a
		///     split zip file, don't set this property, or set it to Zero.
		///   </para>
		///
		///   <para>
		///     If you read a split archive, with <see cref="M:Ionic.Zip.ZipFile.Read(System.String)" /> and
		///     then subsequently call <c>ZipFile.Save()</c>, unless you set this
		///     property before calling <c>Save()</c>, you will get a normal,
		///     single-file archive.
		///   </para>
		/// </remarks>
		///
		/// <seealso cref="P:Ionic.Zip.ZipFile.NumberOfSegmentsForMostRecentSave" />
		public int MaxOutputSegmentSize
		{
			get
			{
				if (_maxOutputSegmentSize > int.MaxValue)
				{
					throw new ZipException("MaxOutputSegmentSize is too large, use MaxOutputSegmentSize64 instead.");
				}
				return (int)_maxOutputSegmentSize;
			}
			set
			{
				if (value < 65536 && value != 0)
				{
					throw new ZipException("The minimum acceptable segment size is 65536.");
				}
				_maxOutputSegmentSize = value;
			}
		}

		/// <summary>
		/// The maximum size of an output segment, when saving a split Zip file.
		/// </summary>
		/// <remarks>
		///   <para>
		///     If you set this value, make sure you do not accidently use <see cref="P:Ionic.Zip.ZipFile.MaxOutputSegmentSize" /> in your code
		///   </para>
		///
		///   <para>
		///     Set this to a non-zero value before calling <see cref="M:Ionic.Zip.ZipFile.Save" /> or <see cref="M:Ionic.Zip.ZipFile.Save(System.String)" /> to specify that the ZipFile should be saved as a
		///     split archive, also sometimes called a spanned archive. Some also
		///     call them multi-file archives.
		///   </para>
		///
		///   <para>
		///     A split zip archive is saved in a set of discrete filesystem files,
		///     rather than in a single file. This is handy when transmitting the
		///     archive in email or some other mechanism that has a limit to the size of
		///     each file.  The first file in a split archive will be named
		///     <c>basename.z01</c>, the second will be named <c>basename.z02</c>, and
		///     so on. The final file is named <c>basename.zip</c>. According to the zip
		///     specification from PKWare, the minimum value is 65536, for a 64k segment
		///     size. The maximum number of segments allows in a split archive is 99.
		///   </para>
		///
		///   <para>
		///     The value of this property determines the maximum size of a split
		///     segment when writing a split archive.  For example, suppose you have a
		///     <c>ZipFile</c> that would save to a single file of 200k. If you set the
		///     <c>MaxOutputSegmentSize</c> to 65536 before calling <c>Save()</c>, you
		///     will get four distinct output files. On the other hand if you set this
		///     property to 256k, then you will get a single-file archive for that
		///     <c>ZipFile</c>.
		///   </para>
		///
		///   <para>
		///     The size of each split output file will be as large as possible, up to
		///     the maximum size set here. The zip specification requires that some data
		///     fields in a zip archive may not span a split boundary, and an output
		///     segment may be smaller than the maximum if necessary to avoid that
		///     problem. Also, obviously the final segment of the archive may be smaller
		///     than the maximum segment size. Segments will never be larger than the
		///     value set with this property.
		///   </para>
		///
		///   <para>
		///     You can save a split Zip file only when saving to a regular filesystem
		///     file. It's not possible to save a split zip file as a self-extracting
		///     archive, nor is it possible to save a split zip file to a stream. When
		///     saving to a SFX or to a Stream, this property is ignored.
		///   </para>
		///
		///   <para>
		///     About interoperability: Split or spanned zip files produced by DotNetZip
		///     can be read by WinZip or PKZip, and vice-versa. Segmented zip files may
		///     not be readable by other tools, if those other tools don't support zip
		///     spanning or splitting.  When in doubt, test.  I don't believe Windows
		///     Explorer can extract a split archive.
		///   </para>
		///
		///   <para>
		///     This property has no effect when reading a split archive. You can read
		///     a split archive in the normal way with DotNetZip.
		///   </para>
		///
		///   <para>
		///     When saving a zip file, if you want a regular zip file rather than a
		///     split zip file, don't set this property, or set it to Zero.
		///   </para>
		///
		///   <para>
		///     If you read a split archive, with <see cref="M:Ionic.Zip.ZipFile.Read(System.String)" /> and
		///     then subsequently call <c>ZipFile.Save()</c>, unless you set this
		///     property before calling <c>Save()</c>, you will get a normal,
		///     single-file archive.
		///   </para>
		/// </remarks>
		///
		/// <seealso cref="P:Ionic.Zip.ZipFile.NumberOfSegmentsForMostRecentSave" />
		public long MaxOutputSegmentSize64
		{
			get
			{
				return _maxOutputSegmentSize;
			}
			set
			{
				if (value < 65536 && value != 0L)
				{
					throw new ZipException("The minimum acceptable segment size is 65536.");
				}
				_maxOutputSegmentSize = value;
			}
		}

		/// <summary>
		///   Returns the number of segments used in the most recent Save() operation.
		/// </summary>
		/// <remarks>
		///   <para>
		///     This is normally zero, unless you have set the <see cref="P:Ionic.Zip.ZipFile.MaxOutputSegmentSize" /> property.  If you have set <see cref="P:Ionic.Zip.ZipFile.MaxOutputSegmentSize" />, and then you save a file, after the call to
		///     Save() completes, you can read this value to learn the number of segments that
		///     were created.
		///   </para>
		///   <para>
		///     If you call Save("Archive.zip"), and it creates 5 segments, then you
		///     will have filesystem files named Archive.z01, Archive.z02, Archive.z03,
		///     Archive.z04, and Archive.zip, and the value of this property will be 5.
		///   </para>
		/// </remarks>
		/// <seealso cref="P:Ionic.Zip.ZipFile.MaxOutputSegmentSize" />
		public int NumberOfSegmentsForMostRecentSave => (int)(_numberOfSegmentsForMostRecentSave + 1);

		/// <summary>
		///   The size threshold for an entry, above which a parallel deflate is used.
		/// </summary>
		///
		/// <remarks>
		///
		///   <para>
		///     DotNetZip will use multiple threads to compress any ZipEntry,
		///     if the entry is larger than the given size.  Zero means "always
		///     use parallel deflate", while -1 means "never use parallel
		///     deflate". The default value for this property is 512k. Aside
		///     from the special values of 0 and 1, the minimum value is 65536.
		///   </para>
		///
		///   <para>
		///     If the entry size cannot be known before compression, as with a
		///     read-forward stream, then Parallel deflate will never be
		///     performed, unless the value of this property is zero.
		///   </para>
		///
		///   <para>
		///     A parallel deflate operations will speed up the compression of
		///     large files, on computers with multiple CPUs or multiple CPU
		///     cores.  For files above 1mb, on a dual core or dual-cpu (2p)
		///     machine, the time required to compress the file can be 70% of the
		///     single-threaded deflate.  For very large files on 4p machines the
		///     compression can be done in 30% of the normal time.  The downside
		///     is that parallel deflate consumes extra memory during the deflate,
		///     and the deflation is not as effective.
		///   </para>
		///
		///   <para>
		///     Parallel deflate tends to yield slightly less compression when
		///     compared to as single-threaded deflate; this is because the original
		///     data stream is split into multiple independent buffers, each of which
		///     is compressed in parallel.  But because they are treated
		///     independently, there is no opportunity to share compression
		///     dictionaries.  For that reason, a deflated stream may be slightly
		///     larger when compressed using parallel deflate, as compared to a
		///     traditional single-threaded deflate. Sometimes the increase over the
		///     normal deflate is as much as 5% of the total compressed size. For
		///     larger files it can be as small as 0.1%.
		///   </para>
		///
		///   <para>
		///     Multi-threaded compression does not give as much an advantage when
		///     using Encryption. This is primarily because encryption tends to slow
		///     down the entire pipeline. Also, multi-threaded compression gives less
		///     of an advantage when using lower compression levels, for example <see cref="F:Ionic.Zlib.CompressionLevel.BestSpeed" />.  You may have to
		///     perform some tests to determine the best approach for your situation.
		///   </para>
		///
		/// </remarks>
		///
		/// <seealso cref="P:Ionic.Zip.ZipFile.ParallelDeflateMaxBufferPairs" />
		public long ParallelDeflateThreshold
		{
			get
			{
				return _ParallelDeflateThreshold;
			}
			set
			{
				if (value != 0L && value != -1 && value < 65536)
				{
					throw new ArgumentOutOfRangeException("ParallelDeflateThreshold should be -1, 0, or > 65536");
				}
				_ParallelDeflateThreshold = value;
			}
		}

		/// <summary>
		///   The maximum number of buffer pairs to use when performing
		///   parallel compression.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This property sets an upper limit on the number of memory
		///   buffer pairs to create when performing parallel
		///   compression.  The implementation of the parallel
		///   compression stream allocates multiple buffers to
		///   facilitate parallel compression.  As each buffer fills up,
		///   the stream uses <see cref="M:System.Threading.ThreadPool.QueueUserWorkItem(System.Threading.WaitCallback)">
		///   ThreadPool.QueueUserWorkItem()</see> to compress those
		///   buffers in a background threadpool thread. After a buffer
		///   is compressed, it is re-ordered and written to the output
		///   stream.
		/// </para>
		///
		/// <para>
		///   A higher number of buffer pairs enables a higher degree of
		///   parallelism, which tends to increase the speed of compression on
		///   multi-cpu computers.  On the other hand, a higher number of buffer
		///   pairs also implies a larger memory consumption, more active worker
		///   threads, and a higher cpu utilization for any compression. This
		///   property enables the application to limit its memory consumption and
		///   CPU utilization behavior depending on requirements.
		/// </para>
		///
		/// <para>
		///   For each compression "task" that occurs in parallel, there are 2
		///   buffers allocated: one for input and one for output.  This property
		///   sets a limit for the number of pairs.  The total amount of storage
		///   space allocated for buffering will then be (N*S*2), where N is the
		///   number of buffer pairs, S is the size of each buffer (<see cref="P:Ionic.Zip.ZipFile.BufferSize" />).  By default, DotNetZip allocates 4 buffer
		///   pairs per CPU core, so if your machine has 4 cores, and you retain
		///   the default buffer size of 128k, then the
		///   ParallelDeflateOutputStream will use 4 * 4 * 2 * 128kb of buffer
		///   memory in total, or 4mb, in blocks of 128kb.  If you then set this
		///   property to 8, then the number will be 8 * 2 * 128kb of buffer
		///   memory, or 2mb.
		/// </para>
		///
		/// <para>
		///   CPU utilization will also go up with additional buffers, because a
		///   larger number of buffer pairs allows a larger number of background
		///   threads to compress in parallel. If you find that parallel
		///   compression is consuming too much memory or CPU, you can adjust this
		///   value downward.
		/// </para>
		///
		/// <para>
		///   The default value is 16. Different values may deliver better or
		///   worse results, depending on your priorities and the dynamic
		///   performance characteristics of your storage and compute resources.
		/// </para>
		///
		/// <para>
		///   This property is not the number of buffer pairs to use; it is an
		///   upper limit. An illustration: Suppose you have an application that
		///   uses the default value of this property (which is 16), and it runs
		///   on a machine with 2 CPU cores. In that case, DotNetZip will allocate
		///   4 buffer pairs per CPU core, for a total of 8 pairs.  The upper
		///   limit specified by this property has no effect.
		/// </para>
		///
		/// <para>
		///   The application can set this value at any time
		///   before calling <c>ZipFile.Save()</c>.
		/// </para>
		/// </remarks>
		///
		/// <seealso cref="P:Ionic.Zip.ZipFile.ParallelDeflateThreshold" />
		public int ParallelDeflateMaxBufferPairs
		{
			get
			{
				return _maxBufferPairs;
			}
			set
			{
				if (value < 4)
				{
					throw new ArgumentOutOfRangeException("ParallelDeflateMaxBufferPairs", "Value must be 4 or greater.");
				}
				_maxBufferPairs = value;
			}
		}

		/// <summary>
		/// Returns the version number on the DotNetZip assembly.
		/// </summary>
		///
		/// <remarks>
		///   <para>
		///     This property is exposed as a convenience.  Callers could also get the
		///     version value by retrieving GetName().Version on the
		///     System.Reflection.Assembly object pointing to the DotNetZip
		///     assembly. But sometimes it is not clear which assembly is being loaded.
		///     This property makes it clear.
		///   </para>
		///   <para>
		///     This static property is primarily useful for diagnostic purposes.
		///   </para>
		/// </remarks>
		public static Version LibraryVersion => Assembly.GetExecutingAssembly().GetName().Version;

		private List<ZipEntry> ZipEntriesAsList
		{
			get
			{
				if (_zipEntriesAsList == null)
				{
					_zipEntriesAsList = new List<ZipEntry>(_entries.Values);
				}
				return _zipEntriesAsList;
			}
		}

		/// <summary>
		///   This is an integer indexer into the Zip archive.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This property is read-only.
		/// </para>
		///
		/// <para>
		///   Internally, the <c>ZipEntry</c> instances that belong to the
		///   <c>ZipFile</c> are stored in a Dictionary.  When you use this
		///   indexer the first time, it creates a read-only
		///   <c>List&lt;ZipEntry&gt;</c> from the Dictionary.Values Collection.
		///   If at any time you modify the set of entries in the <c>ZipFile</c>,
		///   either by adding an entry, removing an entry, or renaming an
		///   entry, a new List will be created, and the numeric indexes for the
		///   remaining entries may be different.
		/// </para>
		///
		/// <para>
		///   This means you cannot rename any ZipEntry from
		///   inside an enumeration of the zip file.
		/// </para>
		///
		/// <param name="ix">
		///   The index value.
		/// </param>
		///
		/// </remarks>
		///
		/// <returns>
		///   The <c>ZipEntry</c> within the Zip archive at the specified index. If the
		///   entry does not exist in the archive, this indexer throws.
		/// </returns>
		public ZipEntry this[int ix] => ZipEntriesAsList[ix];

		/// <summary>
		///   This is a name-based indexer into the Zip archive.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This property is read-only.
		/// </para>
		///
		/// <para>
		///   The <see cref="P:Ionic.Zip.ZipFile.CaseSensitiveRetrieval" /> property on the <c>ZipFile</c>
		///   determines whether retrieval via this indexer is done via case-sensitive
		///   comparisons. By default, retrieval is not case sensitive.  This makes
		///   sense on Windows, in which filesystems are not case sensitive.
		/// </para>
		///
		/// <para>
		///   Regardless of case-sensitivity, it is not always the case that
		///   <c>this[value].FileName == value</c>. In other words, the <c>FileName</c>
		///   property of the <c>ZipEntry</c> retrieved with this indexer, may or may
		///   not be equal to the index value.
		/// </para>
		///
		/// <para>
		///   This is because DotNetZip performs a normalization of filenames passed to
		///   this indexer, before attempting to retrieve the item.  That normalization
		///   includes: removal of a volume letter and colon, swapping backward slashes
		///   for forward slashes.  So, <c>zip["dir1\\entry1.txt"].FileName ==
		///   "dir1/entry.txt"</c>.
		/// </para>
		///
		/// <para>
		///   Directory entries in the zip file may be retrieved via this indexer only
		///   with names that have a trailing slash. DotNetZip automatically appends a
		///   trailing slash to the names of any directory entries added to a zip.
		/// </para>
		///
		/// </remarks>
		///
		/// <example>
		/// This example extracts only the entries in a zip file that are .txt files.
		/// <code>
		/// using (ZipFile zip = ZipFile.Read("PackedDocuments.zip"))
		/// {
		///   foreach (string s1 in zip.EntryFilenames)
		///   {
		///     if (s1.EndsWith(".txt"))
		///       zip[s1].Extract("textfiles");
		///   }
		/// }
		/// </code>
		/// <code lang="VB">
		///   Using zip As ZipFile = ZipFile.Read("PackedDocuments.zip")
		///       Dim s1 As String
		///       For Each s1 In zip.EntryFilenames
		///           If s1.EndsWith(".txt") Then
		///               zip(s1).Extract("textfiles")
		///           End If
		///       Next
		///   End Using
		/// </code>
		/// </example>
		/// <seealso cref="M:Ionic.Zip.ZipFile.RemoveEntry(System.String)" />
		///
		/// <exception cref="T:System.ArgumentException">
		///   Thrown if the caller attempts to assign a non-null value to the indexer.
		/// </exception>
		///
		/// <param name="fileName">
		///   The name of the file, including any directory path, to retrieve from the
		///   zip.  The filename match is not case-sensitive by default; you can use the
		///   <see cref="P:Ionic.Zip.ZipFile.CaseSensitiveRetrieval" /> property to change this behavior. The
		///   pathname can use forward-slashes or backward slashes.
		/// </param>
		///
		/// <returns>
		///   The <c>ZipEntry</c> within the Zip archive, given by the specified
		///   filename. If the named entry does not exist in the archive, this indexer
		///   returns <c>null</c> (<c>Nothing</c> in VB).
		/// </returns>
		public ZipEntry this[string fileName]
		{
			get
			{
				Dictionary<string, ZipEntry> retrievalEntries = RetrievalEntries;
				string text = SharedUtilities.NormalizePathForUseInZipFile(fileName);
				if (retrievalEntries.ContainsKey(text))
				{
					return retrievalEntries[text];
				}
				text = text.Replace("/", "\\");
				if (retrievalEntries.ContainsKey(text))
				{
					return retrievalEntries[text];
				}
				return null;
			}
		}

		/// <summary>
		///   The list of filenames for the entries contained within the zip archive.
		/// </summary>
		///
		/// <remarks>
		///   According to the ZIP specification, the names of the entries use forward
		///   slashes in pathnames.  If you are scanning through the list, you may have
		///   to swap forward slashes for backslashes.
		/// </remarks>
		///
		/// <seealso cref="P:Ionic.Zip.ZipFile.Item(System.String)" />
		///
		/// <example>
		///   This example shows one way to test if a filename is already contained
		///   within a zip archive.
		/// <code>
		/// String zipFileToRead= "PackedDocuments.zip";
		/// string candidate = "DatedMaterial.xps";
		/// using (ZipFile zip = new ZipFile(zipFileToRead))
		/// {
		///   if (zip.EntryFilenames.Contains(candidate))
		///     Console.WriteLine("The file '{0}' exists in the zip archive '{1}'",
		///                       candidate,
		///                       zipFileName);
		///   else
		///     Console.WriteLine("The file, '{0}', does not exist in the zip archive '{1}'",
		///                       candidate,
		///                       zipFileName);
		///   Console.WriteLine();
		/// }
		/// </code>
		/// <code lang="VB">
		///   Dim zipFileToRead As String = "PackedDocuments.zip"
		///   Dim candidate As String = "DatedMaterial.xps"
		///   Using zip As ZipFile.Read(ZipFileToRead)
		///       If zip.EntryFilenames.Contains(candidate) Then
		///           Console.WriteLine("The file '{0}' exists in the zip archive '{1}'", _
		///                       candidate, _
		///                       zipFileName)
		///       Else
		///         Console.WriteLine("The file, '{0}', does not exist in the zip archive '{1}'", _
		///                       candidate, _
		///                       zipFileName)
		///       End If
		///       Console.WriteLine
		///   End Using
		/// </code>
		/// </example>
		///
		/// <returns>
		///   The list of strings for the filenames contained within the Zip archive.
		/// </returns>
		public ICollection<string> EntryFileNames => _entries.Keys;

		/// <summary>
		///   Returns the readonly collection of entries in the Zip archive.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   If there are no entries in the current <c>ZipFile</c>, the value returned is a
		///   non-null zero-element collection.  If there are entries in the zip file,
		///   the elements are returned in no particular order.
		/// </para>
		/// <para>
		///   This is the implied enumerator on the <c>ZipFile</c> class.  If you use a
		///   <c>ZipFile</c> instance in a context that expects an enumerator, you will
		///   get this collection.
		/// </para>
		/// </remarks>
		/// <seealso cref="P:Ionic.Zip.ZipFile.EntriesSorted" />
		public ICollection<ZipEntry> Entries => _entries.Values;

		/// <summary>
		///   Returns a readonly collection of entries in the Zip archive, sorted by FileName.
		/// </summary>
		///
		/// <remarks>
		///   If there are no entries in the current <c>ZipFile</c>, the value returned
		///   is a non-null zero-element collection.  If there are entries in the zip
		///   file, the elements are returned sorted by the name of the entry.
		/// </remarks>
		///
		/// <example>
		///
		///   This example fills a Windows Forms ListView with the entries in a zip file.
		///
		/// <code lang="C#">
		/// using (ZipFile zip = ZipFile.Read(zipFile))
		/// {
		///     foreach (ZipEntry entry in zip.EntriesSorted)
		///     {
		///         ListViewItem item = new ListViewItem(n.ToString());
		///         n++;
		///         string[] subitems = new string[] {
		///             entry.FileName.Replace("/","\\"),
		///             entry.LastModified.ToString("yyyy-MM-dd HH:mm:ss"),
		///             entry.UncompressedSize.ToString(),
		///             String.Format("{0,5:F0}%", entry.CompressionRatio),
		///             entry.CompressedSize.ToString(),
		///             (entry.UsesEncryption) ? "Y" : "N",
		///             String.Format("{0:X8}", entry.Crc)};
		///
		///         foreach (String s in subitems)
		///         {
		///             ListViewItem.ListViewSubItem subitem = new ListViewItem.ListViewSubItem();
		///             subitem.Text = s;
		///             item.SubItems.Add(subitem);
		///         }
		///
		///         this.listView1.Items.Add(item);
		///     }
		/// }
		/// </code>
		/// </example>
		///
		/// <seealso cref="P:Ionic.Zip.ZipFile.Entries" />
		public ICollection<ZipEntry> EntriesSorted
		{
			get
			{
				List<ZipEntry> list = new List<ZipEntry>();
				foreach (ZipEntry entry in Entries)
				{
					list.Add(entry);
				}
				StringComparison sc = (CaseSensitiveRetrieval ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
				list.Sort((ZipEntry x, ZipEntry y) => string.Compare(x.FileName, y.FileName, sc));
				return list.AsReadOnly();
			}
		}

		/// <summary>
		/// Returns the number of entries in the Zip archive.
		/// </summary>
		public int Count => _entries.Count;

		internal Stream ReadStream
		{
			get
			{
				if (_readstream == null && (_readName != null || _name != null))
				{
					_readstream = File.Open(_readName ?? _name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
					_ReadStreamIsOurs = true;
				}
				return _readstream;
			}
		}

		private Stream WriteStream
		{
			get
			{
				if (_writestream != null)
				{
					return _writestream;
				}
				if (_name == null)
				{
					return _writestream;
				}
				if (_maxOutputSegmentSize != 0L)
				{
					_writestream = ZipSegmentedStream.ForWriting(_name, _maxOutputSegmentSize);
					return _writestream;
				}
				SharedUtilities.CreateAndOpenUniqueTempFile(TempFileFolder ?? Path.GetDirectoryName(_name), out _writestream, out _temporaryFileName);
				return _writestream;
			}
			set
			{
				if (value != null)
				{
					throw new ZipException("Cannot set the stream to a non-null value.");
				}
				_writestream = null;
			}
		}

		private string ArchiveNameForEvent
		{
			get
			{
				if (_name == null)
				{
					return "(stream)";
				}
				return _name;
			}
		}

		private long LengthOfReadStream
		{
			get
			{
				if (_lengthOfReadStream == -99)
				{
					_lengthOfReadStream = (_ReadStreamIsOurs ? SharedUtilities.GetFileLength(_name) : (-1));
				}
				return _lengthOfReadStream;
			}
		}

		/// <summary>
		/// The default text encoding used in zip archives.  It is numeric 437, also
		/// known as IBM437.
		/// </summary>
		/// <seealso cref="P:Ionic.Zip.ZipFile.AlternateEncoding" />
		public static Encoding DefaultEncoding
		{
			get
			{
				return _defaultEncoding;
			}
			set
			{
				if (!_defaultEncodingInitialized)
				{
					_defaultEncoding = value;
					_defaultEncodingInitialized = true;
				}
			}
		}

		/// <summary>
		///   An event handler invoked when a Save() starts, before and after each
		///   entry has been written to the archive, when a Save() completes, and
		///   during other Save events.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   Depending on the particular event, different properties on the <see cref="T:Ionic.Zip.SaveProgressEventArgs" /> parameter are set.  The following
		///   table summarizes the available EventTypes and the conditions under
		///   which this event handler is invoked with a
		///   <c>SaveProgressEventArgs</c> with the given EventType.
		/// </para>
		///
		/// <list type="table">
		/// <listheader>
		/// <term>value of EntryType</term>
		/// <description>Meaning and conditions</description>
		/// </listheader>
		///
		/// <item>
		/// <term>ZipProgressEventType.Saving_Started</term>
		/// <description>Fired when ZipFile.Save() begins.
		/// </description>
		/// </item>
		///
		/// <item>
		/// <term>ZipProgressEventType.Saving_BeforeSaveEntry</term>
		/// <description>
		///   Fired within ZipFile.Save(), just before writing data for each
		///   particular entry.
		/// </description>
		/// </item>
		///
		/// <item>
		/// <term>ZipProgressEventType.Saving_AfterSaveEntry</term>
		/// <description>
		///   Fired within ZipFile.Save(), just after having finished writing data
		///   for each particular entry.
		/// </description>
		/// </item>
		///
		/// <item>
		/// <term>ZipProgressEventType.Saving_Completed</term>
		/// <description>Fired when ZipFile.Save() has completed.
		/// </description>
		/// </item>
		///
		/// <item>
		/// <term>ZipProgressEventType.Saving_AfterSaveTempArchive</term>
		/// <description>
		///   Fired after the temporary file has been created.  This happens only
		///   when saving to a disk file.  This event will not be invoked when
		///   saving to a stream.
		/// </description>
		/// </item>
		///
		/// <item>
		/// <term>ZipProgressEventType.Saving_BeforeRenameTempArchive</term>
		/// <description>
		///   Fired just before renaming the temporary file to the permanent
		///   location.  This happens only when saving to a disk file.  This event
		///   will not be invoked when saving to a stream.
		/// </description>
		/// </item>
		///
		/// <item>
		/// <term>ZipProgressEventType.Saving_AfterRenameTempArchive</term>
		/// <description>
		///   Fired just after renaming the temporary file to the permanent
		///   location.  This happens only when saving to a disk file.  This event
		///   will not be invoked when saving to a stream.
		/// </description>
		/// </item>
		///
		/// <item>
		/// <term>ZipProgressEventType.Saving_AfterCompileSelfExtractor</term>
		/// <description>
		///   Fired after a self-extracting archive has finished compiling.  This
		///   EventType is used only within SaveSelfExtractor().
		/// </description>
		/// </item>
		///
		/// <item>
		/// <term>ZipProgressEventType.Saving_BytesRead</term>
		/// <description>
		///   Set during the save of a particular entry, to update progress of the
		///   Save().  When this EventType is set, the BytesTransferred is the
		///   number of bytes that have been read from the source stream.  The
		///   TotalBytesToTransfer is the number of bytes in the uncompressed
		///   file.
		/// </description>
		/// </item>
		///
		/// </list>
		/// </remarks>
		///
		/// <example>
		///
		///    This example uses an anonymous method to handle the
		///    SaveProgress event, by updating a progress bar.
		///
		/// <code lang="C#">
		/// progressBar1.Value = 0;
		/// progressBar1.Max = listbox1.Items.Count;
		/// using (ZipFile zip = new ZipFile())
		/// {
		///    // listbox1 contains a list of filenames
		///    zip.AddFiles(listbox1.Items);
		///
		///    // do the progress bar:
		///    zip.SaveProgress += (sender, e) =&gt; {
		///       if (e.EventType == ZipProgressEventType.Saving_BeforeWriteEntry) {
		///          progressBar1.PerformStep();
		///       }
		///    };
		///
		///    zip.Save(fs);
		/// }
		/// </code>
		/// </example>
		///
		/// <example>
		///   This example uses a named method as the
		///   <c>SaveProgress</c> event handler, to update the user, in a
		///   console-based application.
		///
		/// <code lang="C#">
		/// static bool justHadByteUpdate= false;
		/// public static void SaveProgress(object sender, SaveProgressEventArgs e)
		/// {
		///     if (e.EventType == ZipProgressEventType.Saving_Started)
		///         Console.WriteLine("Saving: {0}", e.ArchiveName);
		///
		///     else if (e.EventType == ZipProgressEventType.Saving_Completed)
		///     {
		///         justHadByteUpdate= false;
		///         Console.WriteLine();
		///         Console.WriteLine("Done: {0}", e.ArchiveName);
		///     }
		///
		///     else if (e.EventType == ZipProgressEventType.Saving_BeforeWriteEntry)
		///     {
		///         if (justHadByteUpdate)
		///             Console.WriteLine();
		///         Console.WriteLine("  Writing: {0} ({1}/{2})",
		///                           e.CurrentEntry.FileName, e.EntriesSaved, e.EntriesTotal);
		///         justHadByteUpdate= false;
		///     }
		///
		///     else if (e.EventType == ZipProgressEventType.Saving_EntryBytesRead)
		///     {
		///         if (justHadByteUpdate)
		///             Console.SetCursorPosition(0, Console.CursorTop);
		///          Console.Write("     {0}/{1} ({2:N0}%)", e.BytesTransferred, e.TotalBytesToTransfer,
		///                       e.BytesTransferred / (0.01 * e.TotalBytesToTransfer ));
		///         justHadByteUpdate= true;
		///     }
		/// }
		///
		/// public static ZipUp(string targetZip, string directory)
		/// {
		///   using (var zip = new ZipFile()) {
		///     zip.SaveProgress += SaveProgress;
		///     zip.AddDirectory(directory);
		///     zip.Save(targetZip);
		///   }
		/// }
		///
		/// </code>
		///
		/// <code lang="VB">
		/// Public Sub ZipUp(ByVal targetZip As String, ByVal directory As String)
		///     Using zip As ZipFile = New ZipFile
		///         AddHandler zip.SaveProgress, AddressOf MySaveProgress
		///         zip.AddDirectory(directory)
		///         zip.Save(targetZip)
		///     End Using
		/// End Sub
		///
		/// Private Shared justHadByteUpdate As Boolean = False
		///
		/// Public Shared Sub MySaveProgress(ByVal sender As Object, ByVal e As SaveProgressEventArgs)
		///     If (e.EventType Is ZipProgressEventType.Saving_Started) Then
		///         Console.WriteLine("Saving: {0}", e.ArchiveName)
		///
		///     ElseIf (e.EventType Is ZipProgressEventType.Saving_Completed) Then
		///         justHadByteUpdate = False
		///         Console.WriteLine
		///         Console.WriteLine("Done: {0}", e.ArchiveName)
		///
		///     ElseIf (e.EventType Is ZipProgressEventType.Saving_BeforeWriteEntry) Then
		///         If justHadByteUpdate Then
		///             Console.WriteLine
		///         End If
		///         Console.WriteLine("  Writing: {0} ({1}/{2})", e.CurrentEntry.FileName, e.EntriesSaved, e.EntriesTotal)
		///         justHadByteUpdate = False
		///
		///     ElseIf (e.EventType Is ZipProgressEventType.Saving_EntryBytesRead) Then
		///         If justHadByteUpdate Then
		///             Console.SetCursorPosition(0, Console.CursorTop)
		///         End If
		///         Console.Write("     {0}/{1} ({2:N0}%)", e.BytesTransferred, _
		///                       e.TotalBytesToTransfer, _
		///                       (CDbl(e.BytesTransferred) / (0.01 * e.TotalBytesToTransfer)))
		///         justHadByteUpdate = True
		///     End If
		/// End Sub
		/// </code>
		/// </example>
		///
		/// <example>
		///
		/// This is a more complete example of using the SaveProgress
		/// events in a Windows Forms application, with a
		/// Thread object.
		///
		/// <code lang="C#">
		/// delegate void SaveEntryProgress(SaveProgressEventArgs e);
		/// delegate void ButtonClick(object sender, EventArgs e);
		///
		/// public class WorkerOptions
		/// {
		///     public string ZipName;
		///     public string Folder;
		///     public string Encoding;
		///     public string Comment;
		///     public int ZipFlavor;
		///     public Zip64Option Zip64;
		/// }
		///
		/// private int _progress2MaxFactor;
		/// private bool _saveCanceled;
		/// private long _totalBytesBeforeCompress;
		/// private long _totalBytesAfterCompress;
		/// private Thread _workerThread;
		///
		///
		/// private void btnZipup_Click(object sender, EventArgs e)
		/// {
		///     KickoffZipup();
		/// }
		///
		/// private void btnCancel_Click(object sender, EventArgs e)
		/// {
		///     if (this.lblStatus.InvokeRequired)
		///     {
		///         this.lblStatus.Invoke(new ButtonClick(this.btnCancel_Click), new object[] { sender, e });
		///     }
		///     else
		///     {
		///         _saveCanceled = true;
		///         lblStatus.Text = "Canceled...";
		///         ResetState();
		///     }
		/// }
		///
		/// private void KickoffZipup()
		/// {
		///     _folderName = tbDirName.Text;
		///
		///     if (_folderName == null || _folderName == "") return;
		///     if (this.tbZipName.Text == null || this.tbZipName.Text == "") return;
		///
		///     // check for existence of the zip file:
		///     if (System.IO.File.Exists(this.tbZipName.Text))
		///     {
		///         var dlgResult = MessageBox.Show(String.Format("The file you have specified ({0}) already exists." +
		///                                                       "  Do you want to overwrite this file?", this.tbZipName.Text),
		///                                         "Confirmation is Required", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
		///         if (dlgResult != DialogResult.Yes) return;
		///         System.IO.File.Delete(this.tbZipName.Text);
		///     }
		///
		///      _saveCanceled = false;
		///     _nFilesCompleted = 0;
		///     _totalBytesAfterCompress = 0;
		///     _totalBytesBeforeCompress = 0;
		///     this.btnOk.Enabled = false;
		///     this.btnOk.Text = "Zipping...";
		///     this.btnCancel.Enabled = true;
		///     lblStatus.Text = "Zipping...";
		///
		///     var options = new WorkerOptions
		///     {
		///         ZipName = this.tbZipName.Text,
		///         Folder = _folderName,
		///         Encoding = "ibm437"
		///     };
		///
		///     if (this.comboBox1.SelectedIndex != 0)
		///     {
		///         options.Encoding = this.comboBox1.SelectedItem.ToString();
		///     }
		///
		///     if (this.radioFlavorSfxCmd.Checked)
		///         options.ZipFlavor = 2;
		///     else if (this.radioFlavorSfxGui.Checked)
		///         options.ZipFlavor = 1;
		///     else options.ZipFlavor = 0;
		///
		///     if (this.radioZip64AsNecessary.Checked)
		///         options.Zip64 = Zip64Option.AsNecessary;
		///     else if (this.radioZip64Always.Checked)
		///         options.Zip64 = Zip64Option.Always;
		///     else options.Zip64 = Zip64Option.Never;
		///
		///     options.Comment = String.Format("Encoding:{0} || Flavor:{1} || ZIP64:{2}\r\nCreated at {3} || {4}\r\n",
		///                 options.Encoding,
		///                 FlavorToString(options.ZipFlavor),
		///                 options.Zip64.ToString(),
		///                 System.DateTime.Now.ToString("yyyy-MMM-dd HH:mm:ss"),
		///                 this.Text);
		///
		///     if (this.tbComment.Text != TB_COMMENT_NOTE)
		///         options.Comment += this.tbComment.Text;
		///
		///     _workerThread = new Thread(this.DoSave);
		///     _workerThread.Name = "Zip Saver thread";
		///     _workerThread.Start(options);
		///     this.Cursor = Cursors.WaitCursor;
		///  }
		///
		///
		/// private void DoSave(Object p)
		/// {
		///     WorkerOptions options = p as WorkerOptions;
		///     try
		///     {
		///         using (var zip1 = new ZipFile())
		///         {
		///             zip1.ProvisionalAlternateEncoding = System.Text.Encoding.GetEncoding(options.Encoding);
		///             zip1.Comment = options.Comment;
		///             zip1.AddDirectory(options.Folder);
		///             _entriesToZip = zip1.EntryFileNames.Count;
		///             SetProgressBars();
		///             zip1.SaveProgress += this.zip1_SaveProgress;
		///
		///             zip1.UseZip64WhenSaving = options.Zip64;
		///
		///             if (options.ZipFlavor == 1)
		///                 zip1.SaveSelfExtractor(options.ZipName, SelfExtractorFlavor.WinFormsApplication);
		///             else if (options.ZipFlavor == 2)
		///                 zip1.SaveSelfExtractor(options.ZipName, SelfExtractorFlavor.ConsoleApplication);
		///             else
		///                 zip1.Save(options.ZipName);
		///         }
		///     }
		///     catch (System.Exception exc1)
		///     {
		///         MessageBox.Show(String.Format("Exception while zipping: {0}", exc1.Message));
		///         btnCancel_Click(null, null);
		///     }
		/// }
		///
		///
		///
		/// void zip1_SaveProgress(object sender, SaveProgressEventArgs e)
		/// {
		///     switch (e.EventType)
		///     {
		///         case ZipProgressEventType.Saving_AfterWriteEntry:
		///             StepArchiveProgress(e);
		///             break;
		///         case ZipProgressEventType.Saving_EntryBytesRead:
		///             StepEntryProgress(e);
		///             break;
		///         case ZipProgressEventType.Saving_Completed:
		///             SaveCompleted();
		///             break;
		///         case ZipProgressEventType.Saving_AfterSaveTempArchive:
		///             // this event only occurs when saving an SFX file
		///             TempArchiveSaved();
		///             break;
		///     }
		///     if (_saveCanceled)
		///         e.Cancel = true;
		/// }
		///
		///
		///
		/// private void StepArchiveProgress(SaveProgressEventArgs e)
		/// {
		///     if (this.progressBar1.InvokeRequired)
		///     {
		///         this.progressBar1.Invoke(new SaveEntryProgress(this.StepArchiveProgress), new object[] { e });
		///     }
		///     else
		///     {
		///         if (!_saveCanceled)
		///         {
		///             _nFilesCompleted++;
		///             this.progressBar1.PerformStep();
		///             _totalBytesAfterCompress += e.CurrentEntry.CompressedSize;
		///             _totalBytesBeforeCompress += e.CurrentEntry.UncompressedSize;
		///
		///             // reset the progress bar for the entry:
		///             this.progressBar2.Value = this.progressBar2.Maximum = 1;
		///
		///             this.Update();
		///         }
		///     }
		/// }
		///
		///
		/// private void StepEntryProgress(SaveProgressEventArgs e)
		/// {
		///     if (this.progressBar2.InvokeRequired)
		///     {
		///         this.progressBar2.Invoke(new SaveEntryProgress(this.StepEntryProgress), new object[] { e });
		///     }
		///     else
		///     {
		///         if (!_saveCanceled)
		///         {
		///             if (this.progressBar2.Maximum == 1)
		///             {
		///                 // reset
		///                 Int64 max = e.TotalBytesToTransfer;
		///                 _progress2MaxFactor = 0;
		///                 while (max &gt; System.Int32.MaxValue)
		///                 {
		///                     max /= 2;
		///                     _progress2MaxFactor++;
		///                 }
		///                 this.progressBar2.Maximum = (int)max;
		///                 lblStatus.Text = String.Format("{0} of {1} files...({2})",
		///                     _nFilesCompleted + 1, _entriesToZip, e.CurrentEntry.FileName);
		///             }
		///
		///              int xferred = e.BytesTransferred &gt;&gt; _progress2MaxFactor;
		///
		///              this.progressBar2.Value = (xferred &gt;= this.progressBar2.Maximum)
		///                 ? this.progressBar2.Maximum
		///                 : xferred;
		///
		///              this.Update();
		///         }
		///     }
		/// }
		///
		/// private void SaveCompleted()
		/// {
		///     if (this.lblStatus.InvokeRequired)
		///     {
		///         this.lblStatus.Invoke(new MethodInvoker(this.SaveCompleted));
		///     }
		///     else
		///     {
		///         lblStatus.Text = String.Format("Done, Compressed {0} files, {1:N0}% of original.",
		///             _nFilesCompleted, (100.00 * _totalBytesAfterCompress) / _totalBytesBeforeCompress);
		///          ResetState();
		///     }
		/// }
		///
		/// private void ResetState()
		/// {
		///     this.btnCancel.Enabled = false;
		///     this.btnOk.Enabled = true;
		///     this.btnOk.Text = "Zip it!";
		///     this.progressBar1.Value = 0;
		///     this.progressBar2.Value = 0;
		///     this.Cursor = Cursors.Default;
		///     if (!_workerThread.IsAlive)
		///         _workerThread.Join();
		/// }
		/// </code>
		///
		/// </example>
		///
		/// <seealso cref="E:Ionic.Zip.ZipFile.ReadProgress" />
		/// <seealso cref="E:Ionic.Zip.ZipFile.AddProgress" />
		/// <seealso cref="E:Ionic.Zip.ZipFile.ExtractProgress" />
		public event EventHandler<SaveProgressEventArgs> SaveProgress;

		/// <summary>
		/// An event handler invoked before, during, and after the reading of a zip archive.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		/// Depending on the particular event being signaled, different properties on the
		/// <see cref="T:Ionic.Zip.ReadProgressEventArgs" /> parameter are set.  The following table
		/// summarizes the available EventTypes and the conditions under which this
		/// event handler is invoked with a <c>ReadProgressEventArgs</c> with the given EventType.
		/// </para>
		///
		/// <list type="table">
		/// <listheader>
		/// <term>value of EntryType</term>
		/// <description>Meaning and conditions</description>
		/// </listheader>
		///
		/// <item>
		/// <term>ZipProgressEventType.Reading_Started</term>
		/// <description>Fired just as ZipFile.Read() begins. Meaningful properties: ArchiveName.
		/// </description>
		/// </item>
		///
		/// <item>
		/// <term>ZipProgressEventType.Reading_Completed</term>
		/// <description>Fired when ZipFile.Read() has completed. Meaningful properties: ArchiveName.
		/// </description>
		/// </item>
		///
		/// <item>
		/// <term>ZipProgressEventType.Reading_ArchiveBytesRead</term>
		/// <description>Fired while reading, updates the number of bytes read for the entire archive.
		/// Meaningful properties: ArchiveName, CurrentEntry, BytesTransferred, TotalBytesToTransfer.
		/// </description>
		/// </item>
		///
		/// <item>
		/// <term>ZipProgressEventType.Reading_BeforeReadEntry</term>
		/// <description>Indicates an entry is about to be read from the archive.
		/// Meaningful properties: ArchiveName, EntriesTotal.
		/// </description>
		/// </item>
		///
		/// <item>
		/// <term>ZipProgressEventType.Reading_AfterReadEntry</term>
		/// <description>Indicates an entry has just been read from the archive.
		/// Meaningful properties: ArchiveName, EntriesTotal, CurrentEntry.
		/// </description>
		/// </item>
		///
		/// </list>
		/// </remarks>
		///
		/// <seealso cref="E:Ionic.Zip.ZipFile.SaveProgress" />
		/// <seealso cref="E:Ionic.Zip.ZipFile.AddProgress" />
		/// <seealso cref="E:Ionic.Zip.ZipFile.ExtractProgress" />
		public event EventHandler<ReadProgressEventArgs> ReadProgress;

		/// <summary>
		///   An event handler invoked before, during, and after extraction of
		///   entries in the zip archive.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   Depending on the particular event, different properties on the <see cref="T:Ionic.Zip.ExtractProgressEventArgs" /> parameter are set.  The following
		///   table summarizes the available EventTypes and the conditions under
		///   which this event handler is invoked with a
		///   <c>ExtractProgressEventArgs</c> with the given EventType.
		/// </para>
		///
		/// <list type="table">
		/// <listheader>
		/// <term>value of EntryType</term>
		/// <description>Meaning and conditions</description>
		/// </listheader>
		///
		/// <item>
		/// <term>ZipProgressEventType.Extracting_BeforeExtractAll</term>
		/// <description>
		///   Set when ExtractAll() begins. The ArchiveName, Overwrite, and
		///   ExtractLocation properties are meaningful.</description>
		/// </item>
		///
		/// <item>
		/// <term>ZipProgressEventType.Extracting_AfterExtractAll</term>
		/// <description>
		///   Set when ExtractAll() has completed.  The ArchiveName, Overwrite,
		///   and ExtractLocation properties are meaningful.
		/// </description>
		/// </item>
		///
		/// <item>
		/// <term>ZipProgressEventType.Extracting_BeforeExtractEntry</term>
		/// <description>
		///   Set when an Extract() on an entry in the ZipFile has begun.
		///   Properties that are meaningful: ArchiveName, EntriesTotal,
		///   CurrentEntry, Overwrite, ExtractLocation, EntriesExtracted.
		/// </description>
		/// </item>
		///
		/// <item>
		/// <term>ZipProgressEventType.Extracting_AfterExtractEntry</term>
		/// <description>
		///   Set when an Extract() on an entry in the ZipFile has completed.
		///   Properties that are meaningful: ArchiveName, EntriesTotal,
		///   CurrentEntry, Overwrite, ExtractLocation, EntriesExtracted.
		/// </description>
		/// </item>
		///
		/// <item>
		/// <term>ZipProgressEventType.Extracting_EntryBytesWritten</term>
		/// <description>
		///   Set within a call to Extract() on an entry in the ZipFile, as data
		///   is extracted for the entry.  Properties that are meaningful:
		///   ArchiveName, CurrentEntry, BytesTransferred, TotalBytesToTransfer.
		/// </description>
		/// </item>
		///
		/// <item>
		/// <term>ZipProgressEventType.Extracting_ExtractEntryWouldOverwrite</term>
		/// <description>
		///   Set within a call to Extract() on an entry in the ZipFile, when the
		///   extraction would overwrite an existing file. This event type is used
		///   only when <c>ExtractExistingFileAction</c> on the <c>ZipFile</c> or
		///   <c>ZipEntry</c> is set to <c>InvokeExtractProgressEvent</c>.
		/// </description>
		/// </item>
		///
		/// </list>
		///
		/// </remarks>
		///
		/// <example>
		/// <code>
		/// private static bool justHadByteUpdate = false;
		/// public static void ExtractProgress(object sender, ExtractProgressEventArgs e)
		/// {
		///   if(e.EventType == ZipProgressEventType.Extracting_EntryBytesWritten)
		///   {
		///     if (justHadByteUpdate)
		///       Console.SetCursorPosition(0, Console.CursorTop);
		///
		///     Console.Write("   {0}/{1} ({2:N0}%)", e.BytesTransferred, e.TotalBytesToTransfer,
		///                   e.BytesTransferred / (0.01 * e.TotalBytesToTransfer ));
		///     justHadByteUpdate = true;
		///   }
		///   else if(e.EventType == ZipProgressEventType.Extracting_BeforeExtractEntry)
		///   {
		///     if (justHadByteUpdate)
		///       Console.WriteLine();
		///     Console.WriteLine("Extracting: {0}", e.CurrentEntry.FileName);
		///     justHadByteUpdate= false;
		///   }
		/// }
		///
		/// public static ExtractZip(string zipToExtract, string directory)
		/// {
		///   string TargetDirectory= "extract";
		///   using (var zip = ZipFile.Read(zipToExtract)) {
		///     zip.ExtractProgress += ExtractProgress;
		///     foreach (var e in zip1)
		///     {
		///       e.Extract(TargetDirectory, true);
		///     }
		///   }
		/// }
		///
		/// </code>
		/// <code lang="VB">
		/// Public Shared Sub Main(ByVal args As String())
		///     Dim ZipToUnpack As String = "C1P3SML.zip"
		///     Dim TargetDir As String = "ExtractTest_Extract"
		///     Console.WriteLine("Extracting file {0} to {1}", ZipToUnpack, TargetDir)
		///     Using zip1 As ZipFile = ZipFile.Read(ZipToUnpack)
		///         AddHandler zip1.ExtractProgress, AddressOf MyExtractProgress
		///         Dim e As ZipEntry
		///         For Each e In zip1
		///             e.Extract(TargetDir, True)
		///         Next
		///     End Using
		/// End Sub
		///
		/// Private Shared justHadByteUpdate As Boolean = False
		///
		/// Public Shared Sub MyExtractProgress(ByVal sender As Object, ByVal e As ExtractProgressEventArgs)
		///     If (e.EventType = ZipProgressEventType.Extracting_EntryBytesWritten) Then
		///         If ExtractTest.justHadByteUpdate Then
		///             Console.SetCursorPosition(0, Console.CursorTop)
		///         End If
		///         Console.Write("   {0}/{1} ({2:N0}%)", e.BytesTransferred, e.TotalBytesToTransfer, (CDbl(e.BytesTransferred) / (0.01 * e.TotalBytesToTransfer)))
		///         ExtractTest.justHadByteUpdate = True
		///     ElseIf (e.EventType = ZipProgressEventType.Extracting_BeforeExtractEntry) Then
		///         If ExtractTest.justHadByteUpdate Then
		///             Console.WriteLine
		///         End If
		///         Console.WriteLine("Extracting: {0}", e.CurrentEntry.FileName)
		///         ExtractTest.justHadByteUpdate = False
		///     End If
		/// End Sub
		/// </code>
		/// </example>
		///
		/// <seealso cref="E:Ionic.Zip.ZipFile.SaveProgress" />
		/// <seealso cref="E:Ionic.Zip.ZipFile.ReadProgress" />
		/// <seealso cref="E:Ionic.Zip.ZipFile.AddProgress" />
		public event EventHandler<ExtractProgressEventArgs> ExtractProgress;

		/// <summary>
		/// An event handler invoked before, during, and after Adding entries to a zip archive.
		/// </summary>
		///
		/// <remarks>
		///     Adding a large number of entries to a zip file can take a long
		///     time.  For example, when calling <see cref="M:Ionic.Zip.ZipFile.AddDirectory(System.String)" /> on a
		///     directory that contains 50,000 files, it could take 3 minutes or so.
		///     This event handler allws an application to track the progress of the Add
		///     operation, and to optionally cancel a lengthy Add operation.
		/// </remarks>
		///
		/// <example>
		/// <code lang="C#">
		///
		/// int _numEntriesToAdd= 0;
		/// int _numEntriesAdded= 0;
		/// void AddProgressHandler(object sender, AddProgressEventArgs e)
		/// {
		///     switch (e.EventType)
		///     {
		///         case ZipProgressEventType.Adding_Started:
		///             Console.WriteLine("Adding files to the zip...");
		///             break;
		///         case ZipProgressEventType.Adding_AfterAddEntry:
		///             _numEntriesAdded++;
		///             Console.WriteLine(String.Format("Adding file {0}/{1} :: {2}",
		///                                      _numEntriesAdded, _numEntriesToAdd, e.CurrentEntry.FileName));
		///             break;
		///         case ZipProgressEventType.Adding_Completed:
		///             Console.WriteLine("Added all files");
		///             break;
		///     }
		/// }
		///
		/// void CreateTheZip()
		/// {
		///     using (ZipFile zip = new ZipFile())
		///     {
		///         zip.AddProgress += AddProgressHandler;
		///         zip.AddDirectory(System.IO.Path.GetFileName(DirToZip));
		///         zip.Save(ZipFileToCreate);
		///     }
		/// }
		///
		/// </code>
		///
		/// <code lang="VB">
		///
		/// Private Sub AddProgressHandler(ByVal sender As Object, ByVal e As AddProgressEventArgs)
		///     Select Case e.EventType
		///         Case ZipProgressEventType.Adding_Started
		///             Console.WriteLine("Adding files to the zip...")
		///             Exit Select
		///         Case ZipProgressEventType.Adding_AfterAddEntry
		///             Console.WriteLine(String.Format("Adding file {0}", e.CurrentEntry.FileName))
		///             Exit Select
		///         Case ZipProgressEventType.Adding_Completed
		///             Console.WriteLine("Added all files")
		///             Exit Select
		///     End Select
		/// End Sub
		///
		/// Sub CreateTheZip()
		///     Using zip as ZipFile = New ZipFile
		///         AddHandler zip.AddProgress, AddressOf AddProgressHandler
		///         zip.AddDirectory(System.IO.Path.GetFileName(DirToZip))
		///         zip.Save(ZipFileToCreate);
		///     End Using
		/// End Sub
		///
		/// </code>
		///
		/// </example>
		///
		/// <seealso cref="E:Ionic.Zip.ZipFile.SaveProgress" />
		/// <seealso cref="E:Ionic.Zip.ZipFile.ReadProgress" />
		/// <seealso cref="E:Ionic.Zip.ZipFile.ExtractProgress" />
		public event EventHandler<AddProgressEventArgs> AddProgress;

		/// <summary>
		/// An event that is raised when an error occurs during open or read of files
		/// while saving a zip archive.
		/// </summary>
		///
		/// <remarks>
		///  <para>
		///     Errors can occur as a file is being saved to the zip archive.  For
		///     example, the File.Open may fail, or a File.Read may fail, because of
		///     lock conflicts or other reasons.  If you add a handler to this event,
		///     you can handle such errors in your own code.  If you don't add a
		///     handler, the library will throw an exception if it encounters an I/O
		///     error during a call to <c>Save()</c>.
		///  </para>
		///
		///  <para>
		///    Setting a handler implicitly sets <see cref="P:Ionic.Zip.ZipFile.ZipErrorAction" /> to
		///    <c>ZipErrorAction.InvokeErrorEvent</c>.
		///  </para>
		///
		///  <para>
		///    The handler you add applies to all <see cref="T:Ionic.Zip.ZipEntry" /> items that are
		///    subsequently added to the <c>ZipFile</c> instance. If you set this
		///    property after you have added items to the <c>ZipFile</c>, but before you
		///    have called <c>Save()</c>, errors that occur while saving those items
		///    will not cause the error handler to be invoked.
		///  </para>
		///
		///  <para>
		///    If you want to handle any errors that occur with any entry in the zip
		///    file using the same error handler, then add your error handler once,
		///    before adding any entries to the zip archive.
		///  </para>
		///
		///  <para>
		///    In the error handler method, you need to set the <see cref="P:Ionic.Zip.ZipEntry.ZipErrorAction" /> property on the
		///    <c>ZipErrorEventArgs.CurrentEntry</c>.  This communicates back to
		///    DotNetZip what you would like to do with this particular error.  Within
		///    an error handler, if you set the <c>ZipEntry.ZipErrorAction</c> property
		///    on the <c>ZipEntry</c> to <c>ZipErrorAction.InvokeErrorEvent</c> or if
		///    you don't set it at all, the library will throw the exception. (It is the
		///    same as if you had set the <c>ZipEntry.ZipErrorAction</c> property on the
		///    <c>ZipEntry</c> to <c>ZipErrorAction.Throw</c>.) If you set the
		///    <c>ZipErrorEventArgs.Cancel</c> to true, the entire <c>Save()</c> will be
		///    canceled.
		///  </para>
		///
		///  <para>
		///    In the case that you use <c>ZipErrorAction.Skip</c>, implying that
		///    you want to skip the entry for which there's been an error, DotNetZip
		///    tries to seek backwards in the output stream, and truncate all bytes
		///    written on behalf of that particular entry. This works only if the
		///    output stream is seekable.  It will not work, for example, when using
		///    ASPNET's Response.OutputStream.
		///  </para>
		///
		/// </remarks>
		///
		/// <example>
		///
		/// This example shows how to use an event handler to handle
		/// errors during save of the zip file.
		/// <code lang="C#">
		///
		/// public static void MyZipError(object sender, ZipErrorEventArgs e)
		/// {
		///     Console.WriteLine("Error saving {0}...", e.FileName);
		///     Console.WriteLine("   Exception: {0}", e.exception);
		///     ZipEntry entry = e.CurrentEntry;
		///     string response = null;
		///     // Ask the user whether he wants to skip this error or not
		///     do
		///     {
		///         Console.Write("Retry, Skip, Throw, or Cancel ? (R/S/T/C) ");
		///         response = Console.ReadLine();
		///         Console.WriteLine();
		///
		///     } while (response != null &amp;&amp;
		///              response[0]!='S' &amp;&amp; response[0]!='s' &amp;&amp;
		///              response[0]!='R' &amp;&amp; response[0]!='r' &amp;&amp;
		///              response[0]!='T' &amp;&amp; response[0]!='t' &amp;&amp;
		///              response[0]!='C' &amp;&amp; response[0]!='c');
		///
		///     e.Cancel = (response[0]=='C' || response[0]=='c');
		///
		///     if (response[0]=='S' || response[0]=='s')
		///         entry.ZipErrorAction = ZipErrorAction.Skip;
		///     else if (response[0]=='R' || response[0]=='r')
		///         entry.ZipErrorAction = ZipErrorAction.Retry;
		///     else if (response[0]=='T' || response[0]=='t')
		///         entry.ZipErrorAction = ZipErrorAction.Throw;
		/// }
		///
		/// public void SaveTheFile()
		/// {
		///   string directoryToZip = "fodder";
		///   string directoryInArchive = "files";
		///   string zipFileToCreate = "Archive.zip";
		///   using (var zip = new ZipFile())
		///   {
		///     // set the event handler before adding any entries
		///     zip.ZipError += MyZipError;
		///     zip.AddDirectory(directoryToZip, directoryInArchive);
		///     zip.Save(zipFileToCreate);
		///   }
		/// }
		/// </code>
		///
		/// <code lang="VB">
		/// Private Sub MyZipError(ByVal sender As Object, ByVal e As Ionic.Zip.ZipErrorEventArgs)
		///     ' At this point, the application could prompt the user for an action to take.
		///     ' But in this case, this application will simply automatically skip the file, in case of error.
		///     Console.WriteLine("Zip Error,  entry {0}", e.CurrentEntry.FileName)
		///     Console.WriteLine("   Exception: {0}", e.exception)
		///     ' set the desired ZipErrorAction on the CurrentEntry to communicate that to DotNetZip
		///     e.CurrentEntry.ZipErrorAction = Zip.ZipErrorAction.Skip
		/// End Sub
		///
		/// Public Sub SaveTheFile()
		///     Dim directoryToZip As String = "fodder"
		///     Dim directoryInArchive As String = "files"
		///     Dim zipFileToCreate as String = "Archive.zip"
		///     Using zipArchive As ZipFile = New ZipFile
		///         ' set the event handler before adding any entries
		///         AddHandler zipArchive.ZipError, AddressOf MyZipError
		///         zipArchive.AddDirectory(directoryToZip, directoryInArchive)
		///         zipArchive.Save(zipFileToCreate)
		///     End Using
		/// End Sub
		///
		/// </code>
		/// </example>
		///
		/// <seealso cref="P:Ionic.Zip.ZipFile.ZipErrorAction" />
		public event EventHandler<ZipErrorEventArgs> ZipError;

		/// <summary>
		/// Saves the ZipFile instance to a self-extracting zip archive.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		/// The generated exe image will execute on any machine that has the .NET
		/// Framework 4.0 installed on it.  The generated exe image is also a
		/// valid ZIP file, readable with DotNetZip or another Zip library or tool
		/// such as WinZip.
		/// </para>
		///
		/// <para>
		/// There are two "flavors" of self-extracting archive.  The
		/// <c>WinFormsApplication</c> version will pop up a GUI and allow the
		/// user to select a target directory into which to extract. There's also
		/// a checkbox allowing the user to specify to overwrite existing files,
		/// and another checkbox to allow the user to request that Explorer be
		/// opened to see the extracted files after extraction.  The other flavor
		/// is <c>ConsoleApplication</c>.  A self-extractor generated with that
		/// flavor setting will run from the command line. It accepts command-line
		/// options to set the overwrite behavior, and to specify the target
		/// extraction directory.
		/// </para>
		///
		/// <para>
		/// There are a few temporary files created during the saving to a
		/// self-extracting zip.  These files are created in the directory pointed
		/// to by <see cref="P:Ionic.Zip.ZipFile.TempFileFolder" />, which defaults to <see cref="M:System.IO.Path.GetTempPath" />.  These temporary files are
		/// removed upon successful completion of this method.
		/// </para>
		///
		/// <para>
		/// When a user runs the WinForms SFX, the user's personal directory (<see cref="F:System.Environment.SpecialFolder.Personal">Environment.SpecialFolder.Personal</see>)
		/// will be used as the default extract location.  If you want to set the
		/// default extract location, you should use the other overload of
		/// <c>SaveSelfExtractor()</c>/ The user who runs the SFX will have the
		/// opportunity to change the extract directory before extracting. When
		/// the user runs the Command-Line SFX, the user must explicitly specify
		/// the directory to which to extract.  The .NET Framework 4.0 is required
		/// on the computer when the self-extracting archive is run.
		/// </para>
		///
		/// <para>
		/// NB: This method is not available in the "Reduced" DotNetZip library.
		/// </para>
		///
		/// </remarks>
		///
		/// <example>
		/// <code>
		/// string DirectoryPath = "c:\\Documents\\Project7";
		/// using (ZipFile zip = new ZipFile())
		/// {
		///     zip.AddDirectory(DirectoryPath, System.IO.Path.GetFileName(DirectoryPath));
		///     zip.Comment = "This will be embedded into a self-extracting console-based exe";
		///     zip.SaveSelfExtractor("archive.exe", SelfExtractorFlavor.ConsoleApplication);
		/// }
		/// </code>
		/// <code lang="VB">
		/// Dim DirectoryPath As String = "c:\Documents\Project7"
		/// Using zip As New ZipFile()
		///     zip.AddDirectory(DirectoryPath, System.IO.Path.GetFileName(DirectoryPath))
		///     zip.Comment = "This will be embedded into a self-extracting console-based exe"
		///     zip.SaveSelfExtractor("archive.exe", SelfExtractorFlavor.ConsoleApplication)
		/// End Using
		/// </code>
		/// </example>
		///
		/// <param name="exeToGenerate">
		///   a pathname, possibly fully qualified, to be created. Typically it
		///   will end in an .exe extension.</param>
		/// <param name="flavor">
		///   Indicates whether a Winforms or Console self-extractor is
		///   desired. </param>
		public void SaveSelfExtractor(string exeToGenerate, SelfExtractorFlavor flavor)
		{
			SelfExtractorSaveOptions selfExtractorSaveOptions = new SelfExtractorSaveOptions();
			selfExtractorSaveOptions.Flavor = flavor;
			SaveSelfExtractor(exeToGenerate, selfExtractorSaveOptions);
		}

		/// <summary>
		///   Saves the ZipFile instance to a self-extracting zip archive, using
		///   the specified save options.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This method saves a self extracting archive, using the specified save
		///   options. These options include the flavor of the SFX, the default extract
		///   directory, the icon file, and so on.  See the documentation
		///   for <see cref="M:Ionic.Zip.ZipFile.SaveSelfExtractor(System.String,Ionic.Zip.SelfExtractorFlavor)" /> for more
		///   details.
		/// </para>
		///
		/// <para>
		///   The user who runs the SFX will have the opportunity to change the extract
		///   directory before extracting. If at the time of extraction, the specified
		///   directory does not exist, the SFX will create the directory before
		///   extracting the files.
		/// </para>
		///
		/// </remarks>
		///
		/// <example>
		///   This example saves a WinForms-based self-extracting archive EXE that
		///   will use c:\ExtractHere as the default extract location. The C# code
		///   shows syntax for .NET 3.0, which uses an object initializer for
		///   the SelfExtractorOptions object.
		/// <code>
		/// string DirectoryPath = "c:\\Documents\\Project7";
		/// using (ZipFile zip = new ZipFile())
		/// {
		///     zip.AddDirectory(DirectoryPath, System.IO.Path.GetFileName(DirectoryPath));
		///     zip.Comment = "This will be embedded into a self-extracting WinForms-based exe";
		///     var options = new SelfExtractorOptions
		///     {
		///       Flavor = SelfExtractorFlavor.WinFormsApplication,
		///       DefaultExtractDirectory = "%USERPROFILE%\\ExtractHere",
		///       PostExtractCommandLine = ExeToRunAfterExtract,
		///       SfxExeWindowTitle = "My Custom Window Title",
		///       RemoveUnpackedFilesAfterExecute = true
		///     };
		///     zip.SaveSelfExtractor("archive.exe", options);
		/// }
		/// </code>
		/// <code lang="VB">
		/// Dim DirectoryPath As String = "c:\Documents\Project7"
		/// Using zip As New ZipFile()
		///     zip.AddDirectory(DirectoryPath, System.IO.Path.GetFileName(DirectoryPath))
		///     zip.Comment = "This will be embedded into a self-extracting console-based exe"
		///     Dim options As New SelfExtractorOptions()
		///     options.Flavor = SelfExtractorFlavor.WinFormsApplication
		///     options.DefaultExtractDirectory = "%USERPROFILE%\\ExtractHere"
		///     options.PostExtractCommandLine = ExeToRunAfterExtract
		///     options.SfxExeWindowTitle = "My Custom Window Title"
		///     options.RemoveUnpackedFilesAfterExecute = True
		///     zip.SaveSelfExtractor("archive.exe", options)
		/// End Using
		/// </code>
		/// </example>
		///
		/// <param name="exeToGenerate">The name of the EXE to generate.</param>
		/// <param name="options">provides the options for creating the
		/// Self-extracting archive.</param>
		public void SaveSelfExtractor(string exeToGenerate, SelfExtractorSaveOptions options)
		{
			if (_name == null)
			{
				_writestream = null;
			}
			_SavingSfx = true;
			_name = exeToGenerate;
			if (Directory.Exists(_name))
			{
				throw new ZipException("Bad Directory", new ArgumentException("That name specifies an existing directory. Please specify a filename.", "exeToGenerate"));
			}
			_contentsChanged = true;
			_fileAlreadyExists = File.Exists(_name);
			_SaveSfxStub(exeToGenerate, options);
			Save();
			_SavingSfx = false;
		}

		private static void ExtractResourceToFile(Assembly a, string resourceName, string filename)
		{
			int num = 0;
			byte[] array = new byte[1024];
			using (Stream stream = a.GetManifestResourceStream(resourceName))
			{
				if (stream == null)
				{
					throw new ZipException($"missing resource '{resourceName}'");
				}
				using (FileStream fileStream = File.OpenWrite(filename))
				{
					do
					{
						num = stream.Read(array, 0, array.Length);
						fileStream.Write(array, 0, num);
					}
					while (num > 0);
				}
			}
		}

		private void _SaveSfxStub(string exeToGenerate, SelfExtractorSaveOptions options)
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			try
			{
				if (File.Exists(exeToGenerate) && Verbose)
				{
					StatusMessageTextWriter.WriteLine("The existing file ({0}) will be overwritten.", exeToGenerate);
				}
				if (!exeToGenerate.EndsWith(".exe") && Verbose)
				{
					StatusMessageTextWriter.WriteLine("Warning: The generated self-extracting file will not have an .exe extension.");
				}
				text4 = TempFileFolder ?? Path.GetDirectoryName(exeToGenerate);
				text2 = GenerateTempPathname(text4, "exe");
				Assembly assembly = typeof(ZipFile).Assembly;
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				if (!string.IsNullOrEmpty(options.CompilerVersion))
				{
					dictionary.Add("CompilerVersion", options.CompilerVersion);
				}
				using (CSharpCodeProvider cSharpCodeProvider = new CSharpCodeProvider(dictionary))
				{
					ExtractorSettings extractorSettings = SettingsList.Where((ExtractorSettings x) => x.Flavor == options.Flavor).FirstOrDefault();
					if (extractorSettings == null)
					{
						throw new BadStateException($"While saving a Self-Extracting Zip, Cannot find that flavor ({options.Flavor})?");
					}
					CompilerParameters compilerParameters = new CompilerParameters();
					compilerParameters.ReferencedAssemblies.Add(assembly.Location);
					if (extractorSettings.ReferencedAssemblies != null)
					{
						foreach (string referencedAssembly in extractorSettings.ReferencedAssemblies)
						{
							compilerParameters.ReferencedAssemblies.Add(referencedAssembly);
						}
					}
					compilerParameters.GenerateInMemory = false;
					compilerParameters.GenerateExecutable = true;
					compilerParameters.IncludeDebugInformation = false;
					compilerParameters.CompilerOptions = "";
					Assembly executingAssembly = Assembly.GetExecutingAssembly();
					StringBuilder stringBuilder = new StringBuilder();
					string text5 = GenerateTempPathname(text4, "cs");
					using (ZipFile zipFile = Read(executingAssembly.GetManifestResourceStream("Ionic.Zip.Resources.ZippedResources.zip")))
					{
						text3 = GenerateTempPathname(text4, "tmp");
						if (string.IsNullOrEmpty(options.IconFile))
						{
							Directory.CreateDirectory(text3);
							ZipEntry zipEntry = zipFile["zippedFile.ico"];
							if ((zipEntry.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
							{
								zipEntry.Attributes ^= FileAttributes.ReadOnly;
							}
							zipEntry.Extract(text3);
							text = Path.Combine(text3, "zippedFile.ico");
							compilerParameters.CompilerOptions += $"/win32icon:\"{text}\"";
						}
						else
						{
							compilerParameters.CompilerOptions += $"/win32icon:\"{options.IconFile}\"";
						}
						compilerParameters.OutputAssembly = text2;
						if (options.Flavor == SelfExtractorFlavor.WinFormsApplication)
						{
							compilerParameters.CompilerOptions += " /target:winexe";
						}
						if (!string.IsNullOrEmpty(options.AdditionalCompilerSwitches))
						{
							compilerParameters.CompilerOptions = compilerParameters.CompilerOptions + " " + options.AdditionalCompilerSwitches;
						}
						if (string.IsNullOrEmpty(compilerParameters.CompilerOptions))
						{
							compilerParameters.CompilerOptions = null;
						}
						if (extractorSettings.CopyThroughResources != null && extractorSettings.CopyThroughResources.Count != 0)
						{
							if (!Directory.Exists(text3))
							{
								Directory.CreateDirectory(text3);
							}
							foreach (string copyThroughResource in extractorSettings.CopyThroughResources)
							{
								string text6 = Path.Combine(text3, copyThroughResource);
								ExtractResourceToFile(executingAssembly, copyThroughResource, text6);
								compilerParameters.EmbeddedResources.Add(text6);
							}
						}
						compilerParameters.EmbeddedResources.Add(assembly.Location);
						stringBuilder.Append("// " + Path.GetFileName(text5) + "\n").Append("// --------------------------------------------\n//\n").Append("// This SFX source file was generated by DotNetZip ")
							.Append(LibraryVersion.ToString())
							.Append("\n//         at ")
							.Append(DateTime.Now.ToString("yyyy MMMM dd  HH:mm:ss"))
							.Append("\n//\n// --------------------------------------------\n\n\n");
						if (!string.IsNullOrEmpty(options.Description))
						{
							stringBuilder.Append("[assembly: System.Reflection.AssemblyTitle(\"" + options.Description.Replace("\"", "") + "\")]\n");
						}
						else
						{
							stringBuilder.Append("[assembly: System.Reflection.AssemblyTitle(\"DotNetZip SFX Archive\")]\n");
						}
						if (!string.IsNullOrEmpty(options.ProductVersion))
						{
							stringBuilder.Append("[assembly: System.Reflection.AssemblyInformationalVersion(\"" + options.ProductVersion.Replace("\"", "") + "\")]\n");
						}
						string text7 = (string.IsNullOrEmpty(options.Copyright) ? "Extractor: Copyright \ufffd Dino Chiesa 2008-2011" : options.Copyright.Replace("\"", ""));
						if (!string.IsNullOrEmpty(options.ProductName))
						{
							stringBuilder.Append("[assembly: System.Reflection.AssemblyProduct(\"").Append(options.ProductName.Replace("\"", "")).Append("\")]\n");
						}
						else
						{
							stringBuilder.Append("[assembly: System.Reflection.AssemblyProduct(\"DotNetZip\")]\n");
						}
						stringBuilder.Append("[assembly: System.Reflection.AssemblyCopyright(\"" + text7 + "\")]\n").Append($"[assembly: System.Reflection.AssemblyVersion(\"{LibraryVersion.ToString()}\")]\n");
						if (options.FileVersion != null)
						{
							stringBuilder.Append($"[assembly: System.Reflection.AssemblyFileVersion(\"{options.FileVersion.ToString()}\")]\n");
						}
						stringBuilder.Append("\n\n\n");
						string text8 = options.DefaultExtractDirectory;
						if (text8 != null)
						{
							text8 = text8.Replace("\"", "").Replace("\\", "\\\\");
						}
						string text9 = options.PostExtractCommandLine;
						if (text9 != null)
						{
							text9 = text9.Replace("\\", "\\\\");
							text9 = text9.Replace("\"", "\\\"");
						}
						foreach (string item in extractorSettings.ResourcesToCompile)
						{
							using (Stream stream = zipFile[item].OpenReader())
							{
								if (stream == null)
								{
									throw new ZipException($"missing resource '{item}'");
								}
								using (StreamReader streamReader = new StreamReader(stream))
								{
									while (streamReader.Peek() >= 0)
									{
										string text10 = streamReader.ReadLine();
										if (text8 != null)
										{
											text10 = text10.Replace("@@EXTRACTLOCATION", text8);
										}
										text10 = text10.Replace("@@REMOVE_AFTER_EXECUTE", options.RemoveUnpackedFilesAfterExecute.ToString());
										text10 = text10.Replace("@@QUIET", options.Quiet.ToString());
										if (!string.IsNullOrEmpty(options.SfxExeWindowTitle))
										{
											text10 = text10.Replace("@@SFX_EXE_WINDOW_TITLE", options.SfxExeWindowTitle);
										}
										text10 = text10.Replace("@@EXTRACT_EXISTING_FILE", ((int)options.ExtractExistingFile).ToString());
										if (text9 != null)
										{
											text10 = text10.Replace("@@POST_UNPACK_CMD_LINE", text9);
										}
										stringBuilder.Append(text10).Append("\n");
									}
								}
								stringBuilder.Append("\n\n");
							}
						}
					}
					string text11 = stringBuilder.ToString();
					CompilerResults compilerResults = cSharpCodeProvider.CompileAssemblyFromSource(compilerParameters, text11);
					if (compilerResults == null)
					{
						throw new SfxGenerationException("Cannot compile the extraction logic!");
					}
					if (Verbose)
					{
						StringEnumerator enumerator2 = compilerResults.Output.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								string current4 = enumerator2.Current;
								StatusMessageTextWriter.WriteLine(current4);
							}
						}
						finally
						{
							if (enumerator2 is IDisposable disposable)
							{
								disposable.Dispose();
							}
						}
					}
					if (compilerResults.Errors.Count != 0)
					{
						using (TextWriter textWriter = new StreamWriter(text5))
						{
							textWriter.Write(text11);
							textWriter.Write("\n\n\n// ------------------------------------------------------------------\n");
							textWriter.Write("// Errors during compilation: \n//\n");
							string fileName = Path.GetFileName(text5);
							foreach (CompilerError error in compilerResults.Errors)
							{
								textWriter.Write(string.Format("//   {0}({1},{2}): {3} {4}: {5}\n//\n", fileName, error.Line, error.Column, error.IsWarning ? "Warning" : "error", error.ErrorNumber, error.ErrorText));
							}
						}
						throw new SfxGenerationException($"Errors compiling the extraction logic!  {text5}");
					}
					OnSaveEvent(ZipProgressEventType.Saving_AfterCompileSelfExtractor);
					using (Stream stream2 = File.OpenRead(text2))
					{
						byte[] array = new byte[4000];
						int num = 1;
						while (num != 0)
						{
							num = stream2.Read(array, 0, array.Length);
							if (num != 0)
							{
								WriteStream.Write(array, 0, num);
							}
						}
					}
				}
				OnSaveEvent(ZipProgressEventType.Saving_AfterSaveTempArchive);
			}
			finally
			{
				try
				{
					if (Directory.Exists(text3))
					{
						try
						{
							Directory.Delete(text3, true);
						}
						catch (IOException arg)
						{
							StatusMessageTextWriter.WriteLine("Warning: Exception: {0}", arg);
						}
					}
					if (File.Exists(text2))
					{
						try
						{
							File.Delete(text2);
						}
						catch (IOException arg2)
						{
							StatusMessageTextWriter.WriteLine("Warning: Exception: {0}", arg2);
						}
					}
				}
				catch (IOException)
				{
				}
			}
		}

		internal static string GenerateTempPathname(string dir, string extension)
		{
			string text = null;
			string name = Assembly.GetExecutingAssembly().GetName().Name;
			do
			{
				string text2 = Guid.NewGuid().ToString();
				string path = string.Format("{0}-{1}-{2}.{3}", name, DateTime.Now.ToString("yyyyMMMdd-HHmmss"), text2, extension);
				text = Path.Combine(dir, path);
			}
			while (File.Exists(text) || Directory.Exists(text));
			return text;
		}

		/// <summary>
		///   Adds an item, either a file or a directory, to a zip file archive.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This method is handy if you are adding things to zip archive and don't
		///   want to bother distinguishing between directories or files.  Any files are
		///   added as single entries.  A directory added through this method is added
		///   recursively: all files and subdirectories contained within the directory
		///   are added to the <c>ZipFile</c>.
		/// </para>
		///
		/// <para>
		///   The name of the item may be a relative path or a fully-qualified
		///   path. Remember, the items contained in <c>ZipFile</c> instance get written
		///   to the disk only when you call <see cref="M:Ionic.Zip.ZipFile.Save" /> or a similar
		///   save method.
		/// </para>
		///
		/// <para>
		///   The directory name used for the file within the archive is the same
		///   as the directory name (potentially a relative path) specified in the
		///   <paramref name="fileOrDirectoryName" />.
		/// </para>
		///
		/// <para>
		///   For <c>ZipFile</c> properties including <see cref="P:Ionic.Zip.ZipFile.Encryption" />, <see cref="P:Ionic.Zip.ZipFile.Password" />, <see cref="P:Ionic.Zip.ZipFile.SetCompression" />, <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" />, <see cref="P:Ionic.Zip.ZipFile.ExtractExistingFile" />,
		///   <see cref="P:Ionic.Zip.ZipFile.ZipErrorAction" />, and <see cref="P:Ionic.Zip.ZipFile.CompressionLevel" />, their
		///   respective values at the time of this call will be applied to the
		///   <c>ZipEntry</c> added.
		/// </para>
		///
		/// </remarks>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddFile(System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddDirectory(System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.UpdateItem(System.String)" />
		///
		/// <overloads>This method has two overloads.</overloads>
		/// <param name="fileOrDirectoryName">
		/// the name of the file or directory to add.</param>
		///
		/// <returns>The <c>ZipEntry</c> added.</returns>
		public ZipEntry AddItem(string fileOrDirectoryName)
		{
			return AddItem(fileOrDirectoryName, null);
		}

		/// <summary>
		///   Adds an item, either a file or a directory, to a zip file archive,
		///   explicitly specifying the directory path to be used in the archive.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   If adding a directory, the add is recursive on all files and
		///   subdirectories contained within it.
		/// </para>
		/// <para>
		///   The name of the item may be a relative path or a fully-qualified path.
		///   The item added by this call to the <c>ZipFile</c> is not read from the
		///   disk nor written to the zip file archive until the application calls
		///   Save() on the <c>ZipFile</c>.
		/// </para>
		///
		/// <para>
		///   This version of the method allows the caller to explicitly specify the
		///   directory path to be used in the archive, which would override the
		///   "natural" path of the filesystem file.
		/// </para>
		///
		/// <para>
		///   Encryption will be used on the file data if the <c>Password</c> has
		///   been set on the <c>ZipFile</c> object, prior to calling this method.
		/// </para>
		///
		/// <para>
		///   For <c>ZipFile</c> properties including <see cref="P:Ionic.Zip.ZipFile.Encryption" />, <see cref="P:Ionic.Zip.ZipFile.Password" />, <see cref="P:Ionic.Zip.ZipFile.SetCompression" />, <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" />, <see cref="P:Ionic.Zip.ZipFile.ExtractExistingFile" />,
		///   <see cref="P:Ionic.Zip.ZipFile.ZipErrorAction" />, and <see cref="P:Ionic.Zip.ZipFile.CompressionLevel" />, their
		///   respective values at the time of this call will be applied to the
		///   <c>ZipEntry</c> added.
		/// </para>
		///
		/// </remarks>
		///
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   Thrown if the file or directory passed in does not exist.
		/// </exception>
		///
		/// <param name="fileOrDirectoryName">the name of the file or directory to add.
		/// </param>
		///
		/// <param name="directoryPathInArchive">
		///   The name of the directory path to use within the zip archive.  This path
		///   need not refer to an extant directory in the current filesystem.  If the
		///   files within the zip are later extracted, this is the path used for the
		///   extracted file.  Passing <c>null</c> (<c>Nothing</c> in VB) will use the
		///   path on the fileOrDirectoryName.  Passing the empty string ("") will
		///   insert the item at the root path within the archive.
		/// </param>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddFile(System.String,System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddDirectory(System.String,System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.UpdateItem(System.String,System.String)" />
		///
		/// <example>
		///   This example shows how to zip up a set of files into a flat hierarchy,
		///   regardless of where in the filesystem the files originated. The resulting
		///   zip archive will contain a toplevel directory named "flat", which itself
		///   will contain files Readme.txt, MyProposal.docx, and Image1.jpg.  A
		///   subdirectory under "flat" called SupportFiles will contain all the files
		///   in the "c:\SupportFiles" directory on disk.
		///
		/// <code>
		/// String[] itemnames= {
		///   "c:\\fixedContent\\Readme.txt",
		///   "MyProposal.docx",
		///   "c:\\SupportFiles",  // a directory
		///   "images\\Image1.jpg"
		/// };
		///
		/// try
		/// {
		///   using (ZipFile zip = new ZipFile())
		///   {
		///     for (int i = 1; i &lt; itemnames.Length; i++)
		///     {
		///       // will add Files or Dirs, recurses and flattens subdirectories
		///       zip.AddItem(itemnames[i],"flat");
		///     }
		///     zip.Save(ZipToCreate);
		///   }
		/// }
		/// catch (System.Exception ex1)
		/// {
		///   System.Console.Error.WriteLine("exception: {0}", ex1);
		/// }
		/// </code>
		///
		/// <code lang="VB">
		///   Dim itemnames As String() = _
		///     New String() { "c:\fixedContent\Readme.txt", _
		///                    "MyProposal.docx", _
		///                    "SupportFiles", _
		///                    "images\Image1.jpg" }
		///   Try
		///       Using zip As New ZipFile
		///           Dim i As Integer
		///           For i = 1 To itemnames.Length - 1
		///               ' will add Files or Dirs, recursing and flattening subdirectories.
		///               zip.AddItem(itemnames(i), "flat")
		///           Next i
		///           zip.Save(ZipToCreate)
		///       End Using
		///   Catch ex1 As Exception
		///       Console.Error.WriteLine("exception: {0}", ex1.ToString())
		///   End Try
		/// </code>
		/// </example>
		/// <returns>The <c>ZipEntry</c> added.</returns>
		public ZipEntry AddItem(string fileOrDirectoryName, string directoryPathInArchive)
		{
			if (File.Exists(fileOrDirectoryName))
			{
				return AddFile(fileOrDirectoryName, directoryPathInArchive);
			}
			if (Directory.Exists(fileOrDirectoryName))
			{
				return AddDirectory(fileOrDirectoryName, directoryPathInArchive);
			}
			throw new FileNotFoundException($"That file or directory ({fileOrDirectoryName}) does not exist!");
		}

		/// <summary>
		///   Adds a File to a Zip file archive.
		/// </summary>
		/// <remarks>
		///
		/// <para>
		///   This call collects metadata for the named file in the filesystem,
		///   including the file attributes and the timestamp, and inserts that metadata
		///   into the resulting ZipEntry.  Only when the application calls Save() on
		///   the <c>ZipFile</c>, does DotNetZip read the file from the filesystem and
		///   then write the content to the zip file archive.
		/// </para>
		///
		/// <para>
		///   This method will throw an exception if an entry with the same name already
		///   exists in the <c>ZipFile</c>.
		/// </para>
		///
		/// <para>
		///   For <c>ZipFile</c> properties including <see cref="P:Ionic.Zip.ZipFile.Encryption" />, <see cref="P:Ionic.Zip.ZipFile.Password" />, <see cref="P:Ionic.Zip.ZipFile.SetCompression" />, <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" />, <see cref="P:Ionic.Zip.ZipFile.ExtractExistingFile" />,
		///   <see cref="P:Ionic.Zip.ZipFile.ZipErrorAction" />, and <see cref="P:Ionic.Zip.ZipFile.CompressionLevel" />, their
		///   respective values at the time of this call will be applied to the
		///   <c>ZipEntry</c> added.
		/// </para>
		///
		/// </remarks>
		///
		/// <example>
		/// <para>
		///   In this example, three files are added to a Zip archive. The ReadMe.txt
		///   file will be placed in the root of the archive. The .png file will be
		///   placed in a folder within the zip called photos\personal.  The pdf file
		///   will be included into a folder within the zip called Desktop.
		/// </para>
		/// <code>
		///    try
		///    {
		///      using (ZipFile zip = new ZipFile())
		///      {
		///        zip.AddFile("c:\\photos\\personal\\7440-N49th.png");
		///        zip.AddFile("c:\\Desktop\\2008-Regional-Sales-Report.pdf");
		///        zip.AddFile("ReadMe.txt");
		///
		///        zip.Save("Package.zip");
		///      }
		///    }
		///    catch (System.Exception ex1)
		///    {
		///      System.Console.Error.WriteLine("exception: " + ex1);
		///    }
		/// </code>
		///
		/// <code lang="VB">
		///  Try
		///       Using zip As ZipFile = New ZipFile
		///           zip.AddFile("c:\photos\personal\7440-N49th.png")
		///           zip.AddFile("c:\Desktop\2008-Regional-Sales-Report.pdf")
		///           zip.AddFile("ReadMe.txt")
		///           zip.Save("Package.zip")
		///       End Using
		///   Catch ex1 As Exception
		///       Console.Error.WriteLine("exception: {0}", ex1.ToString)
		///   End Try
		/// </code>
		/// </example>
		///
		/// <overloads>This method has two overloads.</overloads>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddItem(System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddDirectory(System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.UpdateFile(System.String)" />
		///
		/// <param name="fileName">
		///   The name of the file to add. It should refer to a file in the filesystem.
		///   The name of the file may be a relative path or a fully-qualified path.
		/// </param>
		/// <returns>The <c>ZipEntry</c> corresponding to the File added.</returns>
		public ZipEntry AddFile(string fileName)
		{
			return AddFile(fileName, null);
		}

		/// <summary>
		///   Adds a File to a Zip file archive, potentially overriding the path to be
		///   used within the zip archive.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   The file added by this call to the <c>ZipFile</c> is not written to the
		///   zip file archive until the application calls Save() on the <c>ZipFile</c>.
		/// </para>
		///
		/// <para>
		///   This method will throw an exception if an entry with the same name already
		///   exists in the <c>ZipFile</c>.
		/// </para>
		///
		/// <para>
		///   This version of the method allows the caller to explicitly specify the
		///   directory path to be used in the archive.
		/// </para>
		///
		/// <para>
		///   For <c>ZipFile</c> properties including <see cref="P:Ionic.Zip.ZipFile.Encryption" />, <see cref="P:Ionic.Zip.ZipFile.Password" />, <see cref="P:Ionic.Zip.ZipFile.SetCompression" />, <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" />, <see cref="P:Ionic.Zip.ZipFile.ExtractExistingFile" />,
		///   <see cref="P:Ionic.Zip.ZipFile.ZipErrorAction" />, and <see cref="P:Ionic.Zip.ZipFile.CompressionLevel" />, their
		///   respective values at the time of this call will be applied to the
		///   <c>ZipEntry</c> added.
		/// </para>
		///
		/// </remarks>
		///
		/// <example>
		/// <para>
		///   In this example, three files are added to a Zip archive. The ReadMe.txt
		///   file will be placed in the root of the archive. The .png file will be
		///   placed in a folder within the zip called images.  The pdf file will be
		///   included into a folder within the zip called files\docs, and will be
		///   encrypted with the given password.
		/// </para>
		/// <code>
		/// try
		/// {
		///   using (ZipFile zip = new ZipFile())
		///   {
		///     // the following entry will be inserted at the root in the archive.
		///     zip.AddFile("c:\\datafiles\\ReadMe.txt", "");
		///     // this image file will be inserted into the "images" directory in the archive.
		///     zip.AddFile("c:\\photos\\personal\\7440-N49th.png", "images");
		///     // the following will result in a password-protected file called
		///     // files\\docs\\2008-Regional-Sales-Report.pdf  in the archive.
		///     zip.Password = "EncryptMe!";
		///     zip.AddFile("c:\\Desktop\\2008-Regional-Sales-Report.pdf", "files\\docs");
		///     zip.Save("Archive.zip");
		///   }
		/// }
		/// catch (System.Exception ex1)
		/// {
		///   System.Console.Error.WriteLine("exception: {0}", ex1);
		/// }
		/// </code>
		///
		/// <code lang="VB">
		///   Try
		///       Using zip As ZipFile = New ZipFile
		///           ' the following entry will be inserted at the root in the archive.
		///           zip.AddFile("c:\datafiles\ReadMe.txt", "")
		///           ' this image file will be inserted into the "images" directory in the archive.
		///           zip.AddFile("c:\photos\personal\7440-N49th.png", "images")
		///           ' the following will result in a password-protected file called
		///           ' files\\docs\\2008-Regional-Sales-Report.pdf  in the archive.
		///           zip.Password = "EncryptMe!"
		///           zip.AddFile("c:\Desktop\2008-Regional-Sales-Report.pdf", "files\documents")
		///           zip.Save("Archive.zip")
		///       End Using
		///   Catch ex1 As Exception
		///       Console.Error.WriteLine("exception: {0}", ex1)
		///   End Try
		/// </code>
		/// </example>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddItem(System.String,System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddDirectory(System.String,System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.UpdateFile(System.String,System.String)" />
		///
		/// <param name="fileName">
		///   The name of the file to add.  The name of the file may be a relative path
		///   or a fully-qualified path.
		/// </param>
		///
		/// <param name="directoryPathInArchive">
		///   Specifies a directory path to use to override any path in the fileName.
		///   This path may, or may not, correspond to a real directory in the current
		///   filesystem.  If the files within the zip are later extracted, this is the
		///   path used for the extracted file.  Passing <c>null</c> (<c>Nothing</c> in
		///   VB) will use the path on the fileName, if any.  Passing the empty string
		///   ("") will insert the item at the root path within the archive.
		/// </param>
		///
		/// <returns>The <c>ZipEntry</c> corresponding to the file added.</returns>
		public ZipEntry AddFile(string fileName, string directoryPathInArchive)
		{
			string nameInArchive = ZipEntry.NameInArchive(fileName, directoryPathInArchive);
			ZipEntry ze = ZipEntry.CreateFromFile(fileName, nameInArchive);
			if (Verbose)
			{
				StatusMessageTextWriter.WriteLine("adding {0}...", fileName);
			}
			return _InternalAddEntry(ze);
		}

		/// <summary>
		///   This method removes a collection of entries from the <c>ZipFile</c>.
		/// </summary>
		///
		/// <param name="entriesToRemove">
		///   A collection of ZipEntry instances from this zip file to be removed. For
		///   example, you can pass in an array of ZipEntry instances; or you can call
		///   SelectEntries(), and then add or remove entries from that
		///   ICollection&lt;ZipEntry&gt; (ICollection(Of ZipEntry) in VB), and pass
		///   that ICollection to this method.
		/// </param>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.SelectEntries(System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.RemoveSelectedEntries(System.String)" />
		public void RemoveEntries(ICollection<ZipEntry> entriesToRemove)
		{
			if (entriesToRemove == null)
			{
				throw new ArgumentNullException("entriesToRemove");
			}
			foreach (ZipEntry item in entriesToRemove)
			{
				RemoveEntry(item);
			}
		}

		/// <summary>
		///   This method removes a collection of entries from the <c>ZipFile</c>, by name.
		/// </summary>
		///
		/// <param name="entriesToRemove">
		///   A collection of strings that refer to names of entries to be removed
		///   from the <c>ZipFile</c>.  For example, you can pass in an array or a
		///   List of Strings that provide the names of entries to be removed.
		/// </param>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.SelectEntries(System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.RemoveSelectedEntries(System.String)" />
		public void RemoveEntries(ICollection<string> entriesToRemove)
		{
			if (entriesToRemove == null)
			{
				throw new ArgumentNullException("entriesToRemove");
			}
			foreach (string item in entriesToRemove)
			{
				RemoveEntry(item);
			}
		}

		/// <summary>
		///   This method adds a set of files to the <c>ZipFile</c>.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   Use this method to add a set of files to the zip archive, in one call.
		///   For example, a list of files received from
		///   <c>System.IO.Directory.GetFiles()</c> can be added to a zip archive in one
		///   call.
		/// </para>
		///
		/// <para>
		///   For <c>ZipFile</c> properties including <see cref="P:Ionic.Zip.ZipFile.Encryption" />, <see cref="P:Ionic.Zip.ZipFile.Password" />, <see cref="P:Ionic.Zip.ZipFile.SetCompression" />, <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" />, <see cref="P:Ionic.Zip.ZipFile.ExtractExistingFile" />,
		///   <see cref="P:Ionic.Zip.ZipFile.ZipErrorAction" />, and <see cref="P:Ionic.Zip.ZipFile.CompressionLevel" />, their
		///   respective values at the time of this call will be applied to each
		///   ZipEntry added.
		/// </para>
		/// </remarks>
		///
		/// <param name="fileNames">
		///   The collection of names of the files to add. Each string should refer to a
		///   file in the filesystem. The name of the file may be a relative path or a
		///   fully-qualified path.
		/// </param>
		///
		/// <example>
		///   This example shows how to create a zip file, and add a few files into it.
		/// <code>
		/// String ZipFileToCreate = "archive1.zip";
		/// String DirectoryToZip = "c:\\reports";
		/// using (ZipFile zip = new ZipFile())
		/// {
		///   // Store all files found in the top level directory, into the zip archive.
		///   String[] filenames = System.IO.Directory.GetFiles(DirectoryToZip);
		///   zip.AddFiles(filenames);
		///   zip.Save(ZipFileToCreate);
		/// }
		/// </code>
		///
		/// <code lang="VB">
		/// Dim ZipFileToCreate As String = "archive1.zip"
		/// Dim DirectoryToZip As String = "c:\reports"
		/// Using zip As ZipFile = New ZipFile
		///     ' Store all files found in the top level directory, into the zip archive.
		///     Dim filenames As String() = System.IO.Directory.GetFiles(DirectoryToZip)
		///     zip.AddFiles(filenames)
		///     zip.Save(ZipFileToCreate)
		/// End Using
		/// </code>
		/// </example>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddSelectedFiles(System.String,System.String)" />
		public void AddFiles(IEnumerable<string> fileNames)
		{
			AddFiles(fileNames, null);
		}

		/// <summary>
		///   Adds or updates a set of files in the <c>ZipFile</c>.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   Any files that already exist in the archive are updated. Any files that
		///   don't yet exist in the archive are added.
		/// </para>
		///
		/// <para>
		///   For <c>ZipFile</c> properties including <see cref="P:Ionic.Zip.ZipFile.Encryption" />, <see cref="P:Ionic.Zip.ZipFile.Password" />, <see cref="P:Ionic.Zip.ZipFile.SetCompression" />, <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" />, <see cref="P:Ionic.Zip.ZipFile.ExtractExistingFile" />,
		///   <see cref="P:Ionic.Zip.ZipFile.ZipErrorAction" />, and <see cref="P:Ionic.Zip.ZipFile.CompressionLevel" />, their
		///   respective values at the time of this call will be applied to each
		///   ZipEntry added.
		/// </para>
		/// </remarks>
		///
		/// <param name="fileNames">
		///   The collection of names of the files to update. Each string should refer to a file in
		///   the filesystem. The name of the file may be a relative path or a fully-qualified path.
		/// </param>
		public void UpdateFiles(IEnumerable<string> fileNames)
		{
			UpdateFiles(fileNames, null);
		}

		/// <summary>
		///   Adds a set of files to the <c>ZipFile</c>, using the
		///   specified directory path in the archive.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   Any directory structure that may be present in the
		///   filenames contained in the list is "flattened" in the
		///   archive.  Each file in the list is added to the archive in
		///   the specified top-level directory.
		/// </para>
		///
		/// <para>
		///   For <c>ZipFile</c> properties including <see cref="P:Ionic.Zip.ZipFile.Encryption" />, <see cref="P:Ionic.Zip.ZipFile.Password" />, <see cref="P:Ionic.Zip.ZipFile.SetCompression" />, <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" />, <see cref="P:Ionic.Zip.ZipFile.ExtractExistingFile" />, <see cref="P:Ionic.Zip.ZipFile.ZipErrorAction" />, and <see cref="P:Ionic.Zip.ZipFile.CompressionLevel" />, their respective values at the
		///   time of this call will be applied to each ZipEntry added.
		/// </para>
		/// </remarks>
		///
		/// <param name="fileNames">
		///   The names of the files to add. Each string should refer to
		///   a file in the filesystem.  The name of the file may be a
		///   relative path or a fully-qualified path.
		/// </param>
		///
		/// <param name="directoryPathInArchive">
		///   Specifies a directory path to use to override any path in the file name.
		///   Th is path may, or may not, correspond to a real directory in the current
		///   filesystem.  If the files within the zip are later extracted, this is the
		///   path used for the extracted file.  Passing <c>null</c> (<c>Nothing</c> in
		///   VB) will use the path on each of the <c>fileNames</c>, if any.  Passing
		///   the empty string ("") will insert the item at the root path within the
		///   archive.
		/// </param>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddSelectedFiles(System.String,System.String)" />
		public void AddFiles(IEnumerable<string> fileNames, string directoryPathInArchive)
		{
			AddFiles(fileNames, false, directoryPathInArchive);
		}

		/// <summary>
		///   Adds a set of files to the <c>ZipFile</c>, using the specified directory
		///   path in the archive, and preserving the full directory structure in the
		///   filenames.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   Think of the <paramref name="directoryPathInArchive" /> as a "root" or
		///   base directory used in the archive for the files that get added.  when
		///   <paramref name="preserveDirHierarchy" /> is true, the hierarchy of files
		///   found in the filesystem will be placed, with the hierarchy intact,
		///   starting at that root in the archive. When <c>preserveDirHierarchy</c>
		///   is false, the path hierarchy of files is flattned, and the flattened
		///   set of files gets placed in the root within the archive as specified in
		///   <c>directoryPathInArchive</c>.
		/// </para>
		///
		/// <para>
		///   For <c>ZipFile</c> properties including <see cref="P:Ionic.Zip.ZipFile.Encryption" />, <see cref="P:Ionic.Zip.ZipFile.Password" />, <see cref="P:Ionic.Zip.ZipFile.SetCompression" />, <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" />, <see cref="P:Ionic.Zip.ZipFile.ExtractExistingFile" />,
		///   <see cref="P:Ionic.Zip.ZipFile.ZipErrorAction" />, and <see cref="P:Ionic.Zip.ZipFile.CompressionLevel" />, their
		///   respective values at the time of this call will be applied to each
		///   ZipEntry added.
		/// </para>
		///
		/// </remarks>
		///
		/// <param name="fileNames">
		///   The names of the files to add. Each string should refer to a file in the
		///   filesystem.  The name of the file may be a relative path or a
		///   fully-qualified path.
		/// </param>
		///
		/// <param name="directoryPathInArchive">
		///   Specifies a directory path to use as a prefix for each entry name.
		///   This path may, or may not, correspond to a real directory in the current
		///   filesystem.  If the files within the zip are later extracted, this is the
		///   path used for the extracted file.  Passing <c>null</c> (<c>Nothing</c> in
		///   VB) will use the path on each of the <c>fileNames</c>, if any.  Passing
		///   the empty string ("") will insert the item at the root path within the
		///   archive.
		/// </param>
		///
		/// <param name="preserveDirHierarchy">
		///   whether the entries in the zip archive will reflect the directory
		///   hierarchy that is present in the various filenames.  For example, if
		///   <paramref name="fileNames" /> includes two paths,
		///   \Animalia\Chordata\Mammalia\Info.txt and
		///   \Plantae\Magnoliophyta\Dicotyledon\Info.txt, then calling this method
		///   with <paramref name="preserveDirHierarchy" /> = <c>false</c> will
		///   result in an exception because of a duplicate entry name, while
		///   calling this method with <paramref name="preserveDirHierarchy" /> =
		///   <c>true</c> will result in the full direcory paths being included in
		///   the entries added to the ZipFile.
		/// </param>
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddSelectedFiles(System.String,System.String)" />
		public void AddFiles(IEnumerable<string> fileNames, bool preserveDirHierarchy, string directoryPathInArchive)
		{
			if (fileNames == null)
			{
				throw new ArgumentNullException("fileNames");
			}
			_addOperationCanceled = false;
			OnAddStarted();
			if (preserveDirHierarchy)
			{
				foreach (string fileName in fileNames)
				{
					if (_addOperationCanceled)
					{
						break;
					}
					if (directoryPathInArchive != null)
					{
						string fullPath = Path.GetFullPath(Path.Combine(directoryPathInArchive, Path.GetDirectoryName(fileName)));
						AddFile(fileName, fullPath);
					}
					else
					{
						AddFile(fileName, null);
					}
				}
			}
			else
			{
				foreach (string fileName2 in fileNames)
				{
					if (_addOperationCanceled)
					{
						break;
					}
					AddFile(fileName2, directoryPathInArchive);
				}
			}
			if (!_addOperationCanceled)
			{
				OnAddCompleted();
			}
		}

		/// <summary>
		///   Adds or updates a set of files to the <c>ZipFile</c>, using the specified
		///   directory path in the archive.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   Any files that already exist in the archive are updated. Any files that
		///   don't yet exist in the archive are added.
		/// </para>
		///
		/// <para>
		///   For <c>ZipFile</c> properties including <see cref="P:Ionic.Zip.ZipFile.Encryption" />, <see cref="P:Ionic.Zip.ZipFile.Password" />, <see cref="P:Ionic.Zip.ZipFile.SetCompression" />, <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" />, <see cref="P:Ionic.Zip.ZipFile.ExtractExistingFile" />,
		///   <see cref="P:Ionic.Zip.ZipFile.ZipErrorAction" />, and <see cref="P:Ionic.Zip.ZipFile.CompressionLevel" />, their
		///   respective values at the time of this call will be applied to each
		///   ZipEntry added.
		/// </para>
		/// </remarks>
		///
		/// <param name="fileNames">
		///   The names of the files to add or update. Each string should refer to a
		///   file in the filesystem.  The name of the file may be a relative path or a
		///   fully-qualified path.
		/// </param>
		///
		/// <param name="directoryPathInArchive">
		///   Specifies a directory path to use to override any path in the file name.
		///   This path may, or may not, correspond to a real directory in the current
		///   filesystem.  If the files within the zip are later extracted, this is the
		///   path used for the extracted file.  Passing <c>null</c> (<c>Nothing</c> in
		///   VB) will use the path on each of the <c>fileNames</c>, if any.  Passing
		///   the empty string ("") will insert the item at the root path within the
		///   archive.
		/// </param>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddSelectedFiles(System.String,System.String)" />
		public void UpdateFiles(IEnumerable<string> fileNames, string directoryPathInArchive)
		{
			if (fileNames == null)
			{
				throw new ArgumentNullException("fileNames");
			}
			OnAddStarted();
			foreach (string fileName in fileNames)
			{
				UpdateFile(fileName, directoryPathInArchive);
			}
			OnAddCompleted();
		}

		/// <summary>
		///   Adds or Updates a File in a Zip file archive.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This method adds a file to a zip archive, or, if the file already exists
		///   in the zip archive, this method Updates the content of that given filename
		///   in the zip archive.  The <c>UpdateFile</c> method might more accurately be
		///   called "AddOrUpdateFile".
		/// </para>
		///
		/// <para>
		///   Upon success, there is no way for the application to learn whether the file
		///   was added versus updated.
		/// </para>
		///
		/// <para>
		///   For <c>ZipFile</c> properties including <see cref="P:Ionic.Zip.ZipFile.Encryption" />, <see cref="P:Ionic.Zip.ZipFile.Password" />, <see cref="P:Ionic.Zip.ZipFile.SetCompression" />, <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" />, <see cref="P:Ionic.Zip.ZipFile.ExtractExistingFile" />,
		///   <see cref="P:Ionic.Zip.ZipFile.ZipErrorAction" />, and <see cref="P:Ionic.Zip.ZipFile.CompressionLevel" />, their
		///   respective values at the time of this call will be applied to the
		///   <c>ZipEntry</c> added.
		/// </para>
		/// </remarks>
		///
		/// <example>
		///
		///   This example shows how to Update an existing entry in a zipfile. The first
		///   call to UpdateFile adds the file to the newly-created zip archive.  The
		///   second call to UpdateFile updates the content for that file in the zip
		///   archive.
		///
		/// <code>
		/// using (ZipFile zip1 = new ZipFile())
		/// {
		///   // UpdateFile might more accurately be called "AddOrUpdateFile"
		///   zip1.UpdateFile("MyDocuments\\Readme.txt");
		///   zip1.UpdateFile("CustomerList.csv");
		///   zip1.Comment = "This zip archive has been created.";
		///   zip1.Save("Content.zip");
		/// }
		///
		/// using (ZipFile zip2 = ZipFile.Read("Content.zip"))
		/// {
		///   zip2.UpdateFile("Updates\\Readme.txt");
		///   zip2.Comment = "This zip archive has been updated: The Readme.txt file has been changed.";
		///   zip2.Save();
		/// }
		///
		/// </code>
		/// <code lang="VB">
		///   Using zip1 As New ZipFile
		///       ' UpdateFile might more accurately be called "AddOrUpdateFile"
		///       zip1.UpdateFile("MyDocuments\Readme.txt")
		///       zip1.UpdateFile("CustomerList.csv")
		///       zip1.Comment = "This zip archive has been created."
		///       zip1.Save("Content.zip")
		///   End Using
		///
		///   Using zip2 As ZipFile = ZipFile.Read("Content.zip")
		///       zip2.UpdateFile("Updates\Readme.txt")
		///       zip2.Comment = "This zip archive has been updated: The Readme.txt file has been changed."
		///       zip2.Save
		///   End Using
		/// </code>
		/// </example>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddFile(System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.UpdateDirectory(System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.UpdateItem(System.String)" />
		///
		/// <param name="fileName">
		///   The name of the file to add or update. It should refer to a file in the
		///   filesystem.  The name of the file may be a relative path or a
		///   fully-qualified path.
		/// </param>
		///
		/// <returns>
		///   The <c>ZipEntry</c> corresponding to the File that was added or updated.
		/// </returns>
		public ZipEntry UpdateFile(string fileName)
		{
			return UpdateFile(fileName, null);
		}

		/// <summary>
		///   Adds or Updates a File in a Zip file archive.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This method adds a file to a zip archive, or, if the file already exists
		///   in the zip archive, this method Updates the content of that given filename
		///   in the zip archive.
		/// </para>
		///
		/// <para>
		///   This version of the method allows the caller to explicitly specify the
		///   directory path to be used in the archive.  The entry to be added or
		///   updated is found by using the specified directory path, combined with the
		///   basename of the specified filename.
		/// </para>
		///
		/// <para>
		///   Upon success, there is no way for the application to learn if the file was
		///   added versus updated.
		/// </para>
		///
		/// <para>
		///   For <c>ZipFile</c> properties including <see cref="P:Ionic.Zip.ZipFile.Encryption" />, <see cref="P:Ionic.Zip.ZipFile.Password" />, <see cref="P:Ionic.Zip.ZipFile.SetCompression" />, <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" />, <see cref="P:Ionic.Zip.ZipFile.ExtractExistingFile" />,
		///   <see cref="P:Ionic.Zip.ZipFile.ZipErrorAction" />, and <see cref="P:Ionic.Zip.ZipFile.CompressionLevel" />, their
		///   respective values at the time of this call will be applied to the
		///   <c>ZipEntry</c> added.
		/// </para>
		/// </remarks>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddFile(System.String,System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.UpdateDirectory(System.String,System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.UpdateItem(System.String,System.String)" />
		///
		/// <param name="fileName">
		///   The name of the file to add or update. It should refer to a file in the
		///   filesystem.  The name of the file may be a relative path or a
		///   fully-qualified path.
		/// </param>
		///
		/// <param name="directoryPathInArchive">
		///   Specifies a directory path to use to override any path in the
		///   <c>fileName</c>.  This path may, or may not, correspond to a real
		///   directory in the current filesystem.  If the files within the zip are
		///   later extracted, this is the path used for the extracted file.  Passing
		///   <c>null</c> (<c>Nothing</c> in VB) will use the path on the
		///   <c>fileName</c>, if any.  Passing the empty string ("") will insert the
		///   item at the root path within the archive.
		/// </param>
		///
		/// <returns>
		///   The <c>ZipEntry</c> corresponding to the File that was added or updated.
		/// </returns>
		public ZipEntry UpdateFile(string fileName, string directoryPathInArchive)
		{
			string fileName2 = ZipEntry.NameInArchive(fileName, directoryPathInArchive);
			if (this[fileName2] != null)
			{
				RemoveEntry(fileName2);
			}
			return AddFile(fileName, directoryPathInArchive);
		}

		/// <summary>
		///   Add or update a directory in a zip archive.
		/// </summary>
		///
		/// <remarks>
		///   If the specified directory does not exist in the archive, then this method
		///   is equivalent to calling <c>AddDirectory()</c>.  If the specified
		///   directory already exists in the archive, then this method updates any
		///   existing entries, and adds any new entries. Any entries that are in the
		///   zip archive but not in the specified directory, are left alone.  In other
		///   words, the contents of the zip file will be a union of the previous
		///   contents and the new files.
		/// </remarks>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.UpdateFile(System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddDirectory(System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.UpdateItem(System.String)" />
		///
		/// <param name="directoryName">
		///   The path to the directory to be added to the zip archive, or updated in
		///   the zip archive.
		/// </param>
		///
		/// <returns>
		/// The <c>ZipEntry</c> corresponding to the Directory that was added or updated.
		/// </returns>
		public ZipEntry UpdateDirectory(string directoryName)
		{
			return UpdateDirectory(directoryName, null);
		}

		/// <summary>
		///   Add or update a directory in the zip archive at the specified root
		///   directory in the archive.
		/// </summary>
		///
		/// <remarks>
		///   If the specified directory does not exist in the archive, then this method
		///   is equivalent to calling <c>AddDirectory()</c>.  If the specified
		///   directory already exists in the archive, then this method updates any
		///   existing entries, and adds any new entries. Any entries that are in the
		///   zip archive but not in the specified directory, are left alone.  In other
		///   words, the contents of the zip file will be a union of the previous
		///   contents and the new files.
		/// </remarks>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.UpdateFile(System.String,System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddDirectory(System.String,System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.UpdateItem(System.String,System.String)" />
		///
		/// <param name="directoryName">
		///   The path to the directory to be added to the zip archive, or updated
		///   in the zip archive.
		/// </param>
		///
		/// <param name="directoryPathInArchive">
		///   Specifies a directory path to use to override any path in the
		///   <c>directoryName</c>.  This path may, or may not, correspond to a real
		///   directory in the current filesystem.  If the files within the zip are
		///   later extracted, this is the path used for the extracted file.  Passing
		///   <c>null</c> (<c>Nothing</c> in VB) will use the path on the
		///   <c>directoryName</c>, if any.  Passing the empty string ("") will insert
		///   the item at the root path within the archive.
		/// </param>
		///
		/// <returns>
		///   The <c>ZipEntry</c> corresponding to the Directory that was added or updated.
		/// </returns>
		public ZipEntry UpdateDirectory(string directoryName, string directoryPathInArchive)
		{
			return AddOrUpdateDirectoryImpl(directoryName, directoryPathInArchive, AddOrUpdateAction.AddOrUpdate);
		}

		/// <summary>
		///   Add or update a file or directory in the zip archive.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This is useful when the application is not sure or does not care if the
		///   item to be added is a file or directory, and does not know or does not
		///   care if the item already exists in the <c>ZipFile</c>. Calling this method
		///   is equivalent to calling <c>RemoveEntry()</c> if an entry by the same name
		///   already exists, followed calling by <c>AddItem()</c>.
		/// </para>
		///
		/// <para>
		///   For <c>ZipFile</c> properties including <see cref="P:Ionic.Zip.ZipFile.Encryption" />, <see cref="P:Ionic.Zip.ZipFile.Password" />, <see cref="P:Ionic.Zip.ZipFile.SetCompression" />, <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" />, <see cref="P:Ionic.Zip.ZipFile.ExtractExistingFile" />,
		///   <see cref="P:Ionic.Zip.ZipFile.ZipErrorAction" />, and <see cref="P:Ionic.Zip.ZipFile.CompressionLevel" />, their
		///   respective values at the time of this call will be applied to the
		///   <c>ZipEntry</c> added.
		/// </para>
		/// </remarks>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddItem(System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.UpdateFile(System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.UpdateDirectory(System.String)" />
		///
		/// <param name="itemName">
		///  the path to the file or directory to be added or updated.
		/// </param>
		public void UpdateItem(string itemName)
		{
			UpdateItem(itemName, null);
		}

		/// <summary>
		///   Add or update a file or directory.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This method is useful when the application is not sure or does not care if
		///   the item to be added is a file or directory, and does not know or does not
		///   care if the item already exists in the <c>ZipFile</c>. Calling this method
		///   is equivalent to calling <c>RemoveEntry()</c>, if an entry by that name
		///   exists, and then calling <c>AddItem()</c>.
		/// </para>
		///
		/// <para>
		///   This version of the method allows the caller to explicitly specify the
		///   directory path to be used for the item being added to the archive.  The
		///   entry or entries that are added or updated will use the specified
		///   <c>DirectoryPathInArchive</c>. Extracting the entry from the archive will
		///   result in a file stored in that directory path.
		/// </para>
		///
		/// <para>
		///   For <c>ZipFile</c> properties including <see cref="P:Ionic.Zip.ZipFile.Encryption" />, <see cref="P:Ionic.Zip.ZipFile.Password" />, <see cref="P:Ionic.Zip.ZipFile.SetCompression" />, <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" />, <see cref="P:Ionic.Zip.ZipFile.ExtractExistingFile" />,
		///   <see cref="P:Ionic.Zip.ZipFile.ZipErrorAction" />, and <see cref="P:Ionic.Zip.ZipFile.CompressionLevel" />, their
		///   respective values at the time of this call will be applied to the
		///   <c>ZipEntry</c> added.
		/// </para>
		/// </remarks>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddItem(System.String,System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.UpdateFile(System.String,System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.UpdateDirectory(System.String,System.String)" />
		///
		/// <param name="itemName">
		///   The path for the File or Directory to be added or updated.
		/// </param>
		/// <param name="directoryPathInArchive">
		///   Specifies a directory path to use to override any path in the
		///   <c>itemName</c>.  This path may, or may not, correspond to a real
		///   directory in the current filesystem.  If the files within the zip are
		///   later extracted, this is the path used for the extracted file.  Passing
		///   <c>null</c> (<c>Nothing</c> in VB) will use the path on the
		///   <c>itemName</c>, if any.  Passing the empty string ("") will insert the
		///   item at the root path within the archive.
		/// </param>
		public void UpdateItem(string itemName, string directoryPathInArchive)
		{
			if (File.Exists(itemName))
			{
				UpdateFile(itemName, directoryPathInArchive);
				return;
			}
			if (Directory.Exists(itemName))
			{
				UpdateDirectory(itemName, directoryPathInArchive);
				return;
			}
			throw new FileNotFoundException($"That file or directory ({itemName}) does not exist!");
		}

		/// <summary>
		///   Adds a named entry into the zip archive, taking content for the entry
		///   from a string.
		/// </summary>
		///
		/// <remarks>
		///   Calling this method creates an entry using the given fileName and
		///   directory path within the archive.  There is no need for a file by the
		///   given name to exist in the filesystem; the name is used within the zip
		///   archive only. The content for the entry is encoded using the default text
		///   encoding for the machine.
		/// </remarks>
		///
		/// <param name="content">
		///   The content of the file, should it be extracted from the zip.
		/// </param>
		///
		/// <param name="entryName">
		///   The name, including any path, to use for the entry within the archive.
		/// </param>
		///
		/// <returns>The <c>ZipEntry</c> added.</returns>
		///
		/// <example>
		///
		/// This example shows how to add an entry to the zipfile, using a string as
		/// content for that entry.
		///
		/// <code lang="C#">
		/// string Content = "This string will be the content of the Readme.txt file in the zip archive.";
		/// using (ZipFile zip1 = new ZipFile())
		/// {
		///   zip1.AddFile("MyDocuments\\Resume.doc", "files");
		///   zip1.AddEntry("Readme.txt", Content);
		///   zip1.Comment = "This zip file was created at " + System.DateTime.Now.ToString("G");
		///   zip1.Save("Content.zip");
		/// }
		///
		/// </code>
		/// <code lang="VB">
		/// Public Sub Run()
		///   Dim Content As String = "This string will be the content of the Readme.txt file in the zip archive."
		///   Using zip1 As ZipFile = New ZipFile
		///     zip1.AddEntry("Readme.txt", Content)
		///     zip1.AddFile("MyDocuments\Resume.doc", "files")
		///     zip1.Comment = ("This zip file was created at " &amp; DateTime.Now.ToString("G"))
		///     zip1.Save("Content.zip")
		///   End Using
		/// End Sub
		/// </code>
		/// </example>
		public ZipEntry AddEntry(string entryName, string content)
		{
			return AddEntry(entryName, content, Encoding.Default);
		}

		/// <summary>
		///   Adds a named entry into the zip archive, taking content for the entry
		///   from a string, and using the specified text encoding.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   Calling this method creates an entry using the given fileName and
		///   directory path within the archive.  There is no need for a file by the
		///   given name to exist in the filesystem; the name is used within the zip
		///   archive only.
		/// </para>
		///
		/// <para>
		///   The content for the entry, a string value, is encoded using the given
		///   text encoding. A BOM (byte-order-mark) is emitted into the file, if the
		///   Encoding parameter is set for that.
		/// </para>
		///
		/// <para>
		///   Most Encoding classes support a constructor that accepts a boolean,
		///   indicating whether to emit a BOM or not. For example see <see cref="M:System.Text.UTF8Encoding.#ctor(System.Boolean)" />.
		/// </para>
		///
		/// </remarks>
		///
		/// <param name="entryName">
		///   The name, including any path, to use within the archive for the entry.
		/// </param>
		///
		/// <param name="content">
		///   The content of the file, should it be extracted from the zip.
		/// </param>
		///
		/// <param name="encoding">
		///   The text encoding to use when encoding the string. Be aware: This is
		///   distinct from the text encoding used to encode the fileName, as specified
		///   in <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" />.
		/// </param>
		///
		/// <returns>The <c>ZipEntry</c> added.</returns>
		public ZipEntry AddEntry(string entryName, string content, Encoding encoding)
		{
			MemoryStream memoryStream = new MemoryStream();
			StreamWriter streamWriter = new StreamWriter(memoryStream, encoding);
			streamWriter.Write(content);
			streamWriter.Flush();
			memoryStream.Seek(0L, SeekOrigin.Begin);
			return AddEntry(entryName, memoryStream);
		}

		/// <summary>
		///   Create an entry in the <c>ZipFile</c> using the given <c>Stream</c>
		///   as input.  The entry will have the given filename.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   The application should provide an open, readable stream; in this case it
		///   will be read during the call to <see cref="M:Ionic.Zip.ZipFile.Save" /> or one of
		///   its overloads.
		/// </para>
		///
		/// <para>
		///   The passed stream will be read from its current position. If
		///   necessary, callers should set the position in the stream before
		///   calling AddEntry(). This might be appropriate when using this method
		///   with a MemoryStream, for example.
		/// </para>
		///
		/// <para>
		///   In cases where a large number of streams will be added to the
		///   <c>ZipFile</c>, the application may wish to avoid maintaining all of the
		///   streams open simultaneously.  To handle this situation, the application
		///   should use the <see cref="M:Ionic.Zip.ZipFile.AddEntry(System.String,Ionic.Zip.OpenDelegate,Ionic.Zip.CloseDelegate)" />
		///   overload.
		/// </para>
		///
		/// <para>
		///   For <c>ZipFile</c> properties including <see cref="P:Ionic.Zip.ZipFile.Encryption" />, <see cref="P:Ionic.Zip.ZipFile.Password" />, <see cref="P:Ionic.Zip.ZipFile.SetCompression" />, <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" />, <see cref="P:Ionic.Zip.ZipFile.ExtractExistingFile" />,
		///   <see cref="P:Ionic.Zip.ZipFile.ZipErrorAction" />, and <see cref="P:Ionic.Zip.ZipFile.CompressionLevel" />, their
		///   respective values at the time of this call will be applied to the
		///   <c>ZipEntry</c> added.
		/// </para>
		///
		/// </remarks>
		///
		/// <example>
		/// <para>
		///   This example adds a single entry to a <c>ZipFile</c> via a <c>Stream</c>.
		/// </para>
		///
		/// <code lang="C#">
		/// String zipToCreate = "Content.zip";
		/// String fileNameInArchive = "Content-From-Stream.bin";
		/// using (System.IO.Stream streamToRead = MyStreamOpener())
		/// {
		///   using (ZipFile zip = new ZipFile())
		///   {
		///     ZipEntry entry= zip.AddEntry(fileNameInArchive, streamToRead);
		///     zip.AddFile("Readme.txt");
		///     zip.Save(zipToCreate);  // the stream is read implicitly here
		///   }
		/// }
		/// </code>
		///
		/// <code lang="VB">
		/// Dim zipToCreate As String = "Content.zip"
		/// Dim fileNameInArchive As String = "Content-From-Stream.bin"
		/// Using streamToRead as System.IO.Stream = MyStreamOpener()
		///   Using zip As ZipFile = New ZipFile()
		///     Dim entry as ZipEntry = zip.AddEntry(fileNameInArchive, streamToRead)
		///     zip.AddFile("Readme.txt")
		///     zip.Save(zipToCreate)  '' the stream is read implicitly, here
		///   End Using
		/// End Using
		/// </code>
		/// </example>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.UpdateEntry(System.String,System.IO.Stream)" />
		///
		/// <param name="entryName">
		///   The name, including any path, which is shown in the zip file for the added
		///   entry.
		/// </param>
		/// <param name="stream">
		///   The input stream from which to grab content for the file
		/// </param>
		/// <returns>The <c>ZipEntry</c> added.</returns>
		public ZipEntry AddEntry(string entryName, Stream stream)
		{
			ZipEntry zipEntry = ZipEntry.CreateForStream(entryName, stream);
			zipEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
			if (Verbose)
			{
				StatusMessageTextWriter.WriteLine("adding {0}...", entryName);
			}
			return _InternalAddEntry(zipEntry);
		}

		/// <summary>
		///   Add a ZipEntry for which content is written directly by the application.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   When the application needs to write the zip entry data, use this
		///   method to add the ZipEntry. For example, in the case that the
		///   application wishes to write the XML representation of a DataSet into
		///   a ZipEntry, the application can use this method to do so.
		/// </para>
		///
		/// <para>
		///   For <c>ZipFile</c> properties including <see cref="P:Ionic.Zip.ZipFile.Encryption" />, <see cref="P:Ionic.Zip.ZipFile.Password" />, <see cref="P:Ionic.Zip.ZipFile.SetCompression" />, <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" />, <see cref="P:Ionic.Zip.ZipFile.ExtractExistingFile" />,
		///   <see cref="P:Ionic.Zip.ZipFile.ZipErrorAction" />, and <see cref="P:Ionic.Zip.ZipFile.CompressionLevel" />, their
		///   respective values at the time of this call will be applied to the
		///   <c>ZipEntry</c> added.
		/// </para>
		///
		/// <para>
		///   About progress events: When using the WriteDelegate, DotNetZip does
		///   not issue any SaveProgress events with <c>EventType</c> = <see cref="F:Ionic.Zip.ZipProgressEventType.Saving_EntryBytesRead">
		///   Saving_EntryBytesRead</see>. (This is because it is the
		///   application's code that runs in WriteDelegate - there's no way for
		///   DotNetZip to know when to issue a EntryBytesRead event.)
		///   Applications that want to update a progress bar or similar status
		///   indicator should do so from within the WriteDelegate
		///   itself. DotNetZip will issue the other SaveProgress events,
		///   including <see cref="F:Ionic.Zip.ZipProgressEventType.Saving_Started">
		///   Saving_Started</see>,
		///   <see cref="F:Ionic.Zip.ZipProgressEventType.Saving_BeforeWriteEntry">
		///   Saving_BeforeWriteEntry</see>, and <see cref="F:Ionic.Zip.ZipProgressEventType.Saving_AfterWriteEntry">
		///   Saving_AfterWriteEntry</see>.
		/// </para>
		///
		/// <para>
		///   Note: When you use PKZip encryption, it's normally necessary to
		///   compute the CRC of the content to be encrypted, before compressing or
		///   encrypting it. Therefore, when using PKZip encryption with a
		///   WriteDelegate, the WriteDelegate CAN BE called twice: once to compute
		///   the CRC, and the second time to potentially compress and
		///   encrypt. Surprising, but true. This is because PKWARE specified that
		///   the encryption initialization data depends on the CRC.
		///   If this happens, for each call of the delegate, your
		///   application must stream the same entry data in its entirety. If your
		///   application writes different data during the second call, it will
		///   result in a corrupt zip file.
		/// </para>
		///
		/// <para>
		///   The double-read behavior happens with all types of entries, not only
		///   those that use WriteDelegate. It happens if you add an entry from a
		///   filesystem file, or using a string, or a stream, or an opener/closer
		///   pair. But in those cases, DotNetZip takes care of reading twice; in
		///   the case of the WriteDelegate, the application code gets invoked
		///   twice. Be aware.
		/// </para>
		///
		/// <para>
		///   As you can imagine, this can cause performance problems for large
		///   streams, and it can lead to correctness problems when you use a
		///   <c>WriteDelegate</c>. This is a pretty big pitfall.  There are two
		///   ways to avoid it.  First, and most preferred: don't use PKZIP
		///   encryption.  If you use the WinZip AES encryption, this problem
		///   doesn't occur, because the encryption protocol doesn't require the CRC
		///   up front. Second: if you do choose to use PKZIP encryption, write out
		///   to a non-seekable stream (like standard output, or the
		///   Response.OutputStream in an ASP.NET application).  In this case,
		///   DotNetZip will use an alternative encryption protocol that does not
		///   rely on the CRC of the content.  This also implies setting bit 3 in
		///   the zip entry, which still presents problems for some zip tools.
		/// </para>
		///
		/// <para>
		///   In the future I may modify DotNetZip to *always* use bit 3 when PKZIP
		///   encryption is in use.  This seems like a win overall, but there will
		///   be some work involved.  If you feel strongly about it, visit the
		///   DotNetZip forums and vote up <see href="http://dotnetzip.codeplex.com/workitem/13686">the Workitem
		///   tracking this issue</see>.
		/// </para>
		///
		/// </remarks>
		///
		/// <param name="entryName">the name of the entry to add</param>
		/// <param name="writer">the delegate which will write the entry content</param>
		/// <returns>the ZipEntry added</returns>
		///
		/// <example>
		///
		///   This example shows an application filling a DataSet, then saving the
		///   contents of that DataSet as XML, into a ZipEntry in a ZipFile, using an
		///   anonymous delegate in C#. The DataSet XML is never saved to a disk file.
		///
		/// <code lang="C#">
		/// var c1= new System.Data.SqlClient.SqlConnection(connstring1);
		/// var da = new System.Data.SqlClient.SqlDataAdapter()
		///     {
		///         SelectCommand=  new System.Data.SqlClient.SqlCommand(strSelect, c1)
		///     };
		///
		/// DataSet ds1 = new DataSet();
		/// da.Fill(ds1, "Invoices");
		///
		/// using(Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
		/// {
		///     zip.AddEntry(zipEntryName, (name,stream) =&gt; ds1.WriteXml(stream) );
		///     zip.Save(zipFileName);
		/// }
		/// </code>
		/// </example>
		///
		/// <example>
		///
		/// This example uses an anonymous method in C# as the WriteDelegate to provide
		/// the data for the ZipEntry. The example is a bit contrived - the
		/// <c>AddFile()</c> method is a simpler way to insert the contents of a file
		/// into an entry in a zip file. On the other hand, if there is some sort of
		/// processing or transformation of the file contents required before writing,
		/// the application could use the <c>WriteDelegate</c> to do it, in this way.
		///
		/// <code lang="C#">
		/// using (var input = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite ))
		/// {
		///     using(Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
		///     {
		///         zip.AddEntry(zipEntryName, (name,output) =&gt;
		///             {
		///                 byte[] buffer = new byte[BufferSize];
		///                 int n;
		///                 while ((n = input.Read(buffer, 0, buffer.Length)) != 0)
		///                 {
		///                     // could transform the data here...
		///                     output.Write(buffer, 0, n);
		///                     // could update a progress bar here
		///                 }
		///             });
		///
		///         zip.Save(zipFileName);
		///     }
		/// }
		/// </code>
		/// </example>
		///
		/// <example>
		///
		/// This example uses a named delegate in VB to write data for the given
		/// ZipEntry (VB9 does not have anonymous delegates). The example here is a bit
		/// contrived - a simpler way to add the contents of a file to a ZipEntry is to
		/// simply use the appropriate <c>AddFile()</c> method.  The key scenario for
		/// which the <c>WriteDelegate</c> makes sense is saving a DataSet, in XML
		/// format, to the zip file. The DataSet can write XML to a stream, and the
		/// WriteDelegate is the perfect place to write into the zip file.  There may be
		/// other data structures that can write to a stream, but cannot be read as a
		/// stream.  The <c>WriteDelegate</c> would be appropriate for those cases as
		/// well.
		///
		/// <code lang="VB">
		/// Private Sub WriteEntry (ByVal name As String, ByVal output As Stream)
		///     Using input As FileStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
		///         Dim n As Integer = -1
		///         Dim buffer As Byte() = New Byte(BufferSize){}
		///         Do While n &lt;&gt; 0
		///             n = input.Read(buffer, 0, buffer.Length)
		///             output.Write(buffer, 0, n)
		///         Loop
		///     End Using
		/// End Sub
		///
		/// Public Sub Run()
		///     Using zip = New ZipFile
		///         zip.AddEntry(zipEntryName, New WriteDelegate(AddressOf WriteEntry))
		///         zip.Save(zipFileName)
		///     End Using
		/// End Sub
		/// </code>
		/// </example>
		public ZipEntry AddEntry(string entryName, WriteDelegate writer)
		{
			ZipEntry ze = ZipEntry.CreateForWriter(entryName, writer);
			if (Verbose)
			{
				StatusMessageTextWriter.WriteLine("adding {0}...", entryName);
			}
			return _InternalAddEntry(ze);
		}

		/// <summary>
		///   Add an entry, for which the application will provide a stream
		///   containing the entry data, on a just-in-time basis.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   In cases where the application wishes to open the stream that
		///   holds the content for the ZipEntry, on a just-in-time basis, the
		///   application can use this method.  The application provides an
		///   opener delegate that will be called by the DotNetZip library to
		///   obtain a readable stream that can be read to get the bytes for
		///   the given entry.  Typically, this delegate opens a stream.
		///   Optionally, the application can provide a closer delegate as
		///   well, which will be called by DotNetZip when all bytes have been
		///   read from the entry.
		/// </para>
		///
		/// <para>
		///   These delegates are called from within the scope of the call to
		///   ZipFile.Save().
		/// </para>
		///
		/// <para>
		///   For <c>ZipFile</c> properties including <see cref="P:Ionic.Zip.ZipFile.Encryption" />, <see cref="P:Ionic.Zip.ZipFile.Password" />, <see cref="P:Ionic.Zip.ZipFile.SetCompression" />, <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" />, <see cref="P:Ionic.Zip.ZipFile.ExtractExistingFile" />,
		///   <see cref="P:Ionic.Zip.ZipFile.ZipErrorAction" />, and <see cref="P:Ionic.Zip.ZipFile.CompressionLevel" />, their
		///   respective values at the time of this call will be applied to the
		///   <c>ZipEntry</c> added.
		/// </para>
		///
		/// </remarks>
		///
		/// <example>
		///
		///   This example uses anonymous methods in C# to open and close the
		///   source stream for the content for a zip entry.
		///
		/// <code lang="C#">
		/// using(Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
		/// {
		///     zip.AddEntry(zipEntryName,
		///                  (name) =&gt;  File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite ),
		///                  (name, stream) =&gt;  stream.Close()
		///                  );
		///
		///     zip.Save(zipFileName);
		/// }
		/// </code>
		///
		/// </example>
		///
		/// <example>
		///
		///   This example uses delegates in VB.NET to open and close the
		///   the source stream for the content for a zip entry.  VB 9.0 lacks
		///   support for "Sub" lambda expressions, and so the CloseDelegate must
		///   be an actual, named Sub.
		///
		/// <code lang="VB">
		///
		/// Function MyStreamOpener(ByVal entryName As String) As Stream
		///     '' This simply opens a file.  You probably want to do somethinig
		///     '' more involved here: open a stream to read from a database,
		///     '' open a stream on an HTTP connection, and so on.
		///     Return File.OpenRead(entryName)
		/// End Function
		///
		/// Sub MyStreamCloser(entryName As String, stream As Stream)
		///     stream.Close()
		/// End Sub
		///
		/// Public Sub Run()
		///     Dim dirToZip As String = "fodder"
		///     Dim zipFileToCreate As String = "Archive.zip"
		///     Dim opener As OpenDelegate = AddressOf MyStreamOpener
		///     Dim closer As CloseDelegate = AddressOf MyStreamCloser
		///     Dim numFilestoAdd As Int32 = 4
		///     Using zip As ZipFile = New ZipFile
		///         Dim i As Integer
		///         For i = 0 To numFilesToAdd - 1
		///             zip.AddEntry(String.Format("content-{0:000}.txt"), opener, closer)
		///         Next i
		///         zip.Save(zipFileToCreate)
		///     End Using
		/// End Sub
		///
		/// </code>
		/// </example>
		///
		/// <param name="entryName">the name of the entry to add</param>
		/// <param name="opener">
		///  the delegate that will be invoked by ZipFile.Save() to get the
		///  readable stream for the given entry. ZipFile.Save() will call
		///  read on this stream to obtain the data for the entry. This data
		///  will then be compressed and written to the newly created zip
		///  file.
		/// </param>
		/// <param name="closer">
		///  the delegate that will be invoked to close the stream. This may
		///  be null (Nothing in VB), in which case no call is makde to close
		///  the stream.
		/// </param>
		/// <returns>the ZipEntry added</returns>
		public ZipEntry AddEntry(string entryName, OpenDelegate opener, CloseDelegate closer)
		{
			ZipEntry zipEntry = ZipEntry.CreateForJitStreamProvider(entryName, opener, closer);
			zipEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
			if (Verbose)
			{
				StatusMessageTextWriter.WriteLine("adding {0}...", entryName);
			}
			return _InternalAddEntry(zipEntry);
		}

		public void AddEntry(ZipEntry ze)
		{
			if (ze._container != null)
			{
				throw new InvalidOperationException("Entry already belongs to a zip file");
			}
			ze._container = new ZipContainer(this);
			InternalAddEntry(ze.FileName, ze);
			AfterAddEntry(ze);
		}

		private ZipEntry _InternalAddEntry(ZipEntry ze)
		{
			ze._container = new ZipContainer(this);
			ze.CompressionMethod = CompressionMethod;
			ze.CompressionLevel = CompressionLevel;
			ze.ExtractExistingFile = ExtractExistingFile;
			ze.ZipErrorAction = ZipErrorAction;
			ze.SetCompression = SetCompression;
			ze.AlternateEncoding = AlternateEncoding;
			ze.AlternateEncodingUsage = AlternateEncodingUsage;
			ze.Password = _Password;
			ze.Encryption = Encryption;
			ze.EmitTimesInWindowsFormatWhenSaving = _emitNtfsTimes;
			ze.EmitTimesInUnixFormatWhenSaving = _emitUnixTimes;
			InternalAddEntry(ze.FileName, ze);
			AfterAddEntry(ze);
			return ze;
		}

		/// <summary>
		///   Updates the given entry in the <c>ZipFile</c>, using the given
		///   string as content for the <c>ZipEntry</c>.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   Calling this method is equivalent to removing the <c>ZipEntry</c> for
		///   the given file name and directory path, if it exists, and then calling
		///   <see cref="M:Ionic.Zip.ZipFile.AddEntry(System.String,System.String)" />.  See the documentation for
		///   that method for further explanation. The string content is encoded
		///   using the default encoding for the machine. This encoding is distinct
		///   from the encoding used for the filename itself.  See
		///   <see cref="P:Ionic.Zip.ZipFile.AlternateEncoding" />.
		/// </para>
		///
		/// </remarks>
		///
		/// <param name="entryName">
		///   The name, including any path, to use within the archive for the entry.
		/// </param>
		///
		/// <param name="content">
		///   The content of the file, should it be extracted from the zip.
		/// </param>
		///
		/// <returns>The <c>ZipEntry</c> added.</returns>
		public ZipEntry UpdateEntry(string entryName, string content)
		{
			return UpdateEntry(entryName, content, Encoding.Default);
		}

		/// <summary>
		///   Updates the given entry in the <c>ZipFile</c>, using the given string as
		///   content for the <c>ZipEntry</c>.
		/// </summary>
		///
		/// <remarks>
		///   Calling this method is equivalent to removing the <c>ZipEntry</c> for the
		///   given file name and directory path, if it exists, and then calling <see cref="M:Ionic.Zip.ZipFile.AddEntry(System.String,System.String,System.Text.Encoding)" />.  See the
		///   documentation for that method for further explanation.
		/// </remarks>
		///
		/// <param name="entryName">
		///   The name, including any path, to use within the archive for the entry.
		/// </param>
		///
		/// <param name="content">
		///   The content of the file, should it be extracted from the zip.
		/// </param>
		///
		/// <param name="encoding">
		///   The text encoding to use when encoding the string. Be aware: This is
		///   distinct from the text encoding used to encode the filename. See <see cref="P:Ionic.Zip.ZipFile.AlternateEncoding" />.
		/// </param>
		///
		/// <returns>The <c>ZipEntry</c> added.</returns>
		public ZipEntry UpdateEntry(string entryName, string content, Encoding encoding)
		{
			RemoveEntryForUpdate(entryName);
			return AddEntry(entryName, content, encoding);
		}

		/// <summary>
		///   Updates the given entry in the <c>ZipFile</c>, using the given delegate
		///   as the source for content for the <c>ZipEntry</c>.
		/// </summary>
		///
		/// <remarks>
		///   Calling this method is equivalent to removing the <c>ZipEntry</c> for the
		///   given file name and directory path, if it exists, and then calling <see cref="M:Ionic.Zip.ZipFile.AddEntry(System.String,Ionic.Zip.WriteDelegate)" />.  See the
		///   documentation for that method for further explanation.
		/// </remarks>
		///
		/// <param name="entryName">
		///   The name, including any path, to use within the archive for the entry.
		/// </param>
		///
		/// <param name="writer">the delegate which will write the entry content.</param>
		///
		/// <returns>The <c>ZipEntry</c> added.</returns>
		public ZipEntry UpdateEntry(string entryName, WriteDelegate writer)
		{
			RemoveEntryForUpdate(entryName);
			return AddEntry(entryName, writer);
		}

		/// <summary>
		///   Updates the given entry in the <c>ZipFile</c>, using the given delegates
		///   to open and close the stream that provides the content for the <c>ZipEntry</c>.
		/// </summary>
		///
		/// <remarks>
		///   Calling this method is equivalent to removing the <c>ZipEntry</c> for the
		///   given file name and directory path, if it exists, and then calling <see cref="M:Ionic.Zip.ZipFile.AddEntry(System.String,Ionic.Zip.OpenDelegate,Ionic.Zip.CloseDelegate)" />.  See the
		///   documentation for that method for further explanation.
		/// </remarks>
		///
		/// <param name="entryName">
		///   The name, including any path, to use within the archive for the entry.
		/// </param>
		///
		/// <param name="opener">
		///  the delegate that will be invoked to open the stream
		/// </param>
		/// <param name="closer">
		///  the delegate that will be invoked to close the stream
		/// </param>
		///
		/// <returns>The <c>ZipEntry</c> added or updated.</returns>
		public ZipEntry UpdateEntry(string entryName, OpenDelegate opener, CloseDelegate closer)
		{
			RemoveEntryForUpdate(entryName);
			return AddEntry(entryName, opener, closer);
		}

		/// <summary>
		///   Updates the given entry in the <c>ZipFile</c>, using the given stream as
		///   input, and the given filename and given directory Path.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   Calling the method is equivalent to calling <c>RemoveEntry()</c> if an
		///   entry by the same name already exists, and then calling <c>AddEntry()</c>
		///   with the given <c>fileName</c> and stream.
		/// </para>
		///
		/// <para>
		///   The stream must be open and readable during the call to
		///   <c>ZipFile.Save</c>.  You can dispense the stream on a just-in-time basis
		///   using the <see cref="P:Ionic.Zip.ZipEntry.InputStream" /> property. Check the
		///   documentation of that property for more information.
		/// </para>
		///
		/// <para>
		///   For <c>ZipFile</c> properties including <see cref="P:Ionic.Zip.ZipFile.Encryption" />, <see cref="P:Ionic.Zip.ZipFile.Password" />, <see cref="P:Ionic.Zip.ZipFile.SetCompression" />, <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" />, <see cref="P:Ionic.Zip.ZipFile.ExtractExistingFile" />,
		///   <see cref="P:Ionic.Zip.ZipFile.ZipErrorAction" />, and <see cref="P:Ionic.Zip.ZipFile.CompressionLevel" />, their
		///   respective values at the time of this call will be applied to the
		///   <c>ZipEntry</c> added.
		/// </para>
		///
		/// </remarks>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddEntry(System.String,System.IO.Stream)" />
		/// <seealso cref="P:Ionic.Zip.ZipEntry.InputStream" />
		///
		/// <param name="entryName">
		///   The name, including any path, to use within the archive for the entry.
		/// </param>
		///
		/// <param name="stream">The input stream from which to read file data.</param>
		/// <returns>The <c>ZipEntry</c> added.</returns>
		public ZipEntry UpdateEntry(string entryName, Stream stream)
		{
			RemoveEntryForUpdate(entryName);
			return AddEntry(entryName, stream);
		}

		private void RemoveEntryForUpdate(string entryName)
		{
			if (string.IsNullOrEmpty(entryName))
			{
				throw new ArgumentNullException("entryName");
			}
			string directoryPathInArchive = null;
			if (entryName.IndexOf('\\') != -1)
			{
				directoryPathInArchive = Path.GetDirectoryName(entryName);
				entryName = Path.GetFileName(entryName);
			}
			string fileName = ZipEntry.NameInArchive(entryName, directoryPathInArchive);
			if (this[fileName] != null)
			{
				RemoveEntry(fileName);
			}
		}

		/// <summary>
		///   Add an entry into the zip archive using the given filename and
		///   directory path within the archive, and the given content for the
		///   file. No file is created in the filesystem.
		/// </summary>
		///
		/// <param name="byteContent">The data to use for the entry.</param>
		///
		/// <param name="entryName">
		///   The name, including any path, to use within the archive for the entry.
		/// </param>
		///
		/// <returns>The <c>ZipEntry</c> added.</returns>
		public ZipEntry AddEntry(string entryName, byte[] byteContent)
		{
			if (byteContent == null)
			{
				throw new ArgumentException("bad argument", "byteContent");
			}
			MemoryStream stream = new MemoryStream(byteContent);
			return AddEntry(entryName, stream);
		}

		/// <summary>
		///   Updates the given entry in the <c>ZipFile</c>, using the given byte
		///   array as content for the entry.
		/// </summary>
		///
		/// <remarks>
		///   Calling this method is equivalent to removing the <c>ZipEntry</c>
		///   for the given filename and directory path, if it exists, and then
		///   calling <see cref="M:Ionic.Zip.ZipFile.AddEntry(System.String,System.Byte[])" />.  See the
		///   documentation for that method for further explanation.
		/// </remarks>
		///
		/// <param name="entryName">
		///   The name, including any path, to use within the archive for the entry.
		/// </param>
		///
		/// <param name="byteContent">The content to use for the <c>ZipEntry</c>.</param>
		///
		/// <returns>The <c>ZipEntry</c> added.</returns>
		public ZipEntry UpdateEntry(string entryName, byte[] byteContent)
		{
			RemoveEntryForUpdate(entryName);
			return AddEntry(entryName, byteContent);
		}

		/// <summary>
		///   Adds the contents of a filesystem directory to a Zip file archive.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   The name of the directory may be a relative path or a fully-qualified
		///   path. Any files within the named directory are added to the archive.  Any
		///   subdirectories within the named directory are also added to the archive,
		///   recursively.
		/// </para>
		///
		/// <para>
		///   Top-level entries in the named directory will appear as top-level entries
		///   in the zip archive.  Entries in subdirectories in the named directory will
		///   result in entries in subdirectories in the zip archive.
		/// </para>
		///
		/// <para>
		///   If you want the entries to appear in a containing directory in the zip
		///   archive itself, then you should call the AddDirectory() overload that
		///   allows you to explicitly specify a directory path for use in the archive.
		/// </para>
		///
		/// <para>
		///   For <c>ZipFile</c> properties including <see cref="P:Ionic.Zip.ZipFile.Encryption" />, <see cref="P:Ionic.Zip.ZipFile.Password" />, <see cref="P:Ionic.Zip.ZipFile.SetCompression" />, <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" />, <see cref="P:Ionic.Zip.ZipFile.ExtractExistingFile" />,
		///   <see cref="P:Ionic.Zip.ZipFile.ZipErrorAction" />, and <see cref="P:Ionic.Zip.ZipFile.CompressionLevel" />, their
		///   respective values at the time of this call will be applied to each
		///   ZipEntry added.
		/// </para>
		///
		/// </remarks>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddItem(System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddFile(System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.UpdateDirectory(System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddDirectory(System.String,System.String)" />
		///
		/// <overloads>This method has 2 overloads.</overloads>
		///
		/// <param name="directoryName">The name of the directory to add.</param>
		/// <returns>The <c>ZipEntry</c> added.</returns>
		public ZipEntry AddDirectory(string directoryName)
		{
			return AddDirectory(directoryName, null);
		}

		/// <summary>
		///   Adds the contents of a filesystem directory to a Zip file archive,
		///   overriding the path to be used for entries in the archive.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   The name of the directory may be a relative path or a fully-qualified
		///   path. The add operation is recursive, so that any files or subdirectories
		///   within the name directory are also added to the archive.
		/// </para>
		///
		/// <para>
		///   Top-level entries in the named directory will appear as top-level entries
		///   in the zip archive.  Entries in subdirectories in the named directory will
		///   result in entries in subdirectories in the zip archive.
		/// </para>
		///
		/// <para>
		///   For <c>ZipFile</c> properties including <see cref="P:Ionic.Zip.ZipFile.Encryption" />, <see cref="P:Ionic.Zip.ZipFile.Password" />, <see cref="P:Ionic.Zip.ZipFile.SetCompression" />, <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" />, <see cref="P:Ionic.Zip.ZipFile.ExtractExistingFile" />,
		///   <see cref="P:Ionic.Zip.ZipFile.ZipErrorAction" />, and <see cref="P:Ionic.Zip.ZipFile.CompressionLevel" />, their
		///   respective values at the time of this call will be applied to each
		///   ZipEntry added.
		/// </para>
		///
		/// </remarks>
		///
		/// <example>
		/// <para>
		///   In this code, calling the ZipUp() method with a value of "c:\reports" for
		///   the directory parameter will result in a zip file structure in which all
		///   entries are contained in a toplevel "reports" directory.
		/// </para>
		///
		/// <code lang="C#">
		/// public void ZipUp(string targetZip, string directory)
		/// {
		///   using (var zip = new ZipFile())
		///   {
		///     zip.AddDirectory(directory, System.IO.Path.GetFileName(directory));
		///     zip.Save(targetZip);
		///   }
		/// }
		/// </code>
		/// </example>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddItem(System.String,System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddFile(System.String,System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.UpdateDirectory(System.String,System.String)" />
		///
		/// <param name="directoryName">The name of the directory to add.</param>
		///
		/// <param name="directoryPathInArchive">
		///   Specifies a directory path to use to override any path in the
		///   DirectoryName.  This path may, or may not, correspond to a real directory
		///   in the current filesystem.  If the zip is later extracted, this is the
		///   path used for the extracted file or directory.  Passing <c>null</c>
		///   (<c>Nothing</c> in VB) or the empty string ("") will insert the items at
		///   the root path within the archive.
		/// </param>
		///
		/// <returns>The <c>ZipEntry</c> added.</returns>
		public ZipEntry AddDirectory(string directoryName, string directoryPathInArchive)
		{
			return AddOrUpdateDirectoryImpl(directoryName, directoryPathInArchive, AddOrUpdateAction.AddOnly);
		}

		/// <summary>
		///   Creates a directory in the zip archive.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   Use this when you want to create a directory in the archive but there is
		///   no corresponding filesystem representation for that directory.
		/// </para>
		///
		/// <para>
		///   You will probably not need to do this in your code. One of the only times
		///   you will want to do this is if you want an empty directory in the zip
		///   archive.  The reason: if you add a file to a zip archive that is stored
		///   within a multi-level directory, all of the directory tree is implicitly
		///   created in the zip archive.
		/// </para>
		///
		/// </remarks>
		///
		/// <param name="directoryNameInArchive">
		///   The name of the directory to create in the archive.
		/// </param>
		/// <returns>The <c>ZipEntry</c> added.</returns>
		public ZipEntry AddDirectoryByName(string directoryNameInArchive)
		{
			ZipEntry zipEntry = ZipEntry.CreateFromNothing(directoryNameInArchive);
			zipEntry._container = new ZipContainer(this);
			zipEntry.MarkAsDirectory();
			zipEntry.AlternateEncoding = AlternateEncoding;
			zipEntry.AlternateEncodingUsage = AlternateEncodingUsage;
			zipEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
			zipEntry.EmitTimesInWindowsFormatWhenSaving = _emitNtfsTimes;
			zipEntry.EmitTimesInUnixFormatWhenSaving = _emitUnixTimes;
			zipEntry._Source = ZipEntrySource.Stream;
			InternalAddEntry(zipEntry.FileName, zipEntry);
			AfterAddEntry(zipEntry);
			return zipEntry;
		}

		private ZipEntry AddOrUpdateDirectoryImpl(string directoryName, string rootDirectoryPathInArchive, AddOrUpdateAction action)
		{
			if (rootDirectoryPathInArchive == null)
			{
				rootDirectoryPathInArchive = "";
			}
			return AddOrUpdateDirectoryImpl(directoryName, rootDirectoryPathInArchive, action, true, 0);
		}

		internal void InternalAddEntry(string name, ZipEntry entry)
		{
			_entries.Add(name, entry);
			if (!_entriesInsensitive.ContainsKey(name))
			{
				_entriesInsensitive.Add(name, entry);
			}
			_zipEntriesAsList = null;
			_contentsChanged = true;
		}

		private ZipEntry AddOrUpdateDirectoryImpl(string directoryName, string rootDirectoryPathInArchive, AddOrUpdateAction action, bool recurse, int level)
		{
			if (Verbose)
			{
				StatusMessageTextWriter.WriteLine("{0} {1}...", (action == AddOrUpdateAction.AddOnly) ? "adding" : "Adding or updating", directoryName);
			}
			if (level == 0)
			{
				_addOperationCanceled = false;
				OnAddStarted();
			}
			if (_addOperationCanceled)
			{
				return null;
			}
			string text = rootDirectoryPathInArchive;
			ZipEntry zipEntry = null;
			if (level > 0)
			{
				int num = directoryName.Length;
				for (int num2 = level; num2 > 0; num2--)
				{
					num = directoryName.LastIndexOfAny("/\\".ToCharArray(), num - 1, num - 1);
				}
				text = directoryName.Substring(num + 1);
				text = Path.Combine(rootDirectoryPathInArchive, text);
			}
			if (level > 0 || rootDirectoryPathInArchive != "")
			{
				zipEntry = ZipEntry.CreateFromFile(directoryName, text);
				zipEntry._container = new ZipContainer(this);
				zipEntry.AlternateEncoding = AlternateEncoding;
				zipEntry.AlternateEncodingUsage = AlternateEncodingUsage;
				zipEntry.MarkAsDirectory();
				zipEntry.EmitTimesInWindowsFormatWhenSaving = _emitNtfsTimes;
				zipEntry.EmitTimesInUnixFormatWhenSaving = _emitUnixTimes;
				if (!_entries.ContainsKey(zipEntry.FileName))
				{
					InternalAddEntry(zipEntry.FileName, zipEntry);
					AfterAddEntry(zipEntry);
				}
				text = zipEntry.FileName;
			}
			if (!_addOperationCanceled)
			{
				string[] files = Directory.GetFiles(directoryName);
				if (recurse)
				{
					string[] array = files;
					foreach (string fileName in array)
					{
						if (_addOperationCanceled)
						{
							break;
						}
						if (action == AddOrUpdateAction.AddOnly)
						{
							AddFile(fileName, text);
						}
						else
						{
							UpdateFile(fileName, text);
						}
					}
					if (!_addOperationCanceled)
					{
						array = Directory.GetDirectories(directoryName);
						foreach (string text2 in array)
						{
							FileAttributes attributes = File.GetAttributes(text2);
							if (AddDirectoryWillTraverseReparsePoints || (attributes & FileAttributes.ReparsePoint) == 0)
							{
								AddOrUpdateDirectoryImpl(text2, rootDirectoryPathInArchive, action, recurse, level + 1);
							}
						}
					}
				}
			}
			if (level == 0)
			{
				OnAddCompleted();
			}
			return zipEntry;
		}

		/// <summary>
		///   Checks a zip file to see if its directory is consistent.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   In cases of data error, the directory within a zip file can get out
		///   of synch with the entries in the zip file.  This method checks the
		///   given zip file and returns true if this has occurred.
		/// </para>
		///
		/// <para> This method may take a long time to run for large zip files.  </para>
		///
		/// <para>
		///   This method is not supported in the Reduced version of DotNetZip.
		/// </para>
		///
		/// <para>
		///   Developers using COM can use the <see cref="M:Ionic.Zip.ComHelper.CheckZip(System.String)">ComHelper.CheckZip(String)</see>
		///   method.
		/// </para>
		///
		/// </remarks>
		///
		/// <param name="zipFileName">The filename to of the zip file to check.</param>
		///
		/// <returns>true if the named zip file checks OK. Otherwise, false. </returns>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.FixZipDirectory(System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.CheckZip(System.String,System.Boolean,System.IO.TextWriter)" />
		public static bool CheckZip(string zipFileName)
		{
			return CheckZip(zipFileName, false, null);
		}

		/// <summary>
		///   Checks a zip file to see if its directory is consistent,
		///   and optionally fixes the directory if necessary.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   In cases of data error, the directory within a zip file can get out of
		///   synch with the entries in the zip file.  This method checks the given
		///   zip file, and returns true if this has occurred. It also optionally
		///   fixes the zipfile, saving the fixed copy in <em>Name</em>_Fixed.zip.
		/// </para>
		///
		/// <para>
		///   This method may take a long time to run for large zip files.  It
		///   will take even longer if the file actually needs to be fixed, and if
		///   <c>fixIfNecessary</c> is true.
		/// </para>
		///
		/// <para>
		///   This method is not supported in the Reduced version of DotNetZip.
		/// </para>
		///
		/// </remarks>
		///
		/// <param name="zipFileName">The filename to of the zip file to check.</param>
		///
		/// <param name="fixIfNecessary">If true, the method will fix the zip file if
		///     necessary.</param>
		///
		/// <param name="writer">
		/// a TextWriter in which messages generated while checking will be written.
		/// </param>
		///
		/// <returns>true if the named zip is OK; false if the file needs to be fixed.</returns>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.CheckZip(System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.FixZipDirectory(System.String)" />
		public static bool CheckZip(string zipFileName, bool fixIfNecessary, TextWriter writer)
		{
			ZipFile zipFile = null;
			ZipFile zipFile2 = null;
			bool flag = true;
			try
			{
				zipFile = new ZipFile();
				zipFile.FullScan = true;
				zipFile.Initialize(zipFileName);
				zipFile2 = Read(zipFileName);
				foreach (ZipEntry item in zipFile)
				{
					foreach (ZipEntry item2 in zipFile2)
					{
						if (item.FileName == item2.FileName)
						{
							if (item._RelativeOffsetOfLocalHeader != item2._RelativeOffsetOfLocalHeader)
							{
								flag = false;
								writer?.WriteLine("{0}: mismatch in RelativeOffsetOfLocalHeader  (0x{1:X16} != 0x{2:X16})", item.FileName, item._RelativeOffsetOfLocalHeader, item2._RelativeOffsetOfLocalHeader);
							}
							if (item._CompressedSize != item2._CompressedSize)
							{
								flag = false;
								writer?.WriteLine("{0}: mismatch in CompressedSize  (0x{1:X16} != 0x{2:X16})", item.FileName, item._CompressedSize, item2._CompressedSize);
							}
							if (item._UncompressedSize != item2._UncompressedSize)
							{
								flag = false;
								writer?.WriteLine("{0}: mismatch in UncompressedSize  (0x{1:X16} != 0x{2:X16})", item.FileName, item._UncompressedSize, item2._UncompressedSize);
							}
							if (item.CompressionMethod != item2.CompressionMethod)
							{
								flag = false;
								writer?.WriteLine("{0}: mismatch in CompressionMethod  (0x{1:X4} != 0x{2:X4})", item.FileName, item.CompressionMethod, item2.CompressionMethod);
							}
							if (item.Crc != item2.Crc)
							{
								flag = false;
								writer?.WriteLine("{0}: mismatch in Crc32  (0x{1:X4} != 0x{2:X4})", item.FileName, item.Crc, item2.Crc);
							}
							break;
						}
					}
				}
				zipFile2.Dispose();
				zipFile2 = null;
				if (!flag && fixIfNecessary)
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(zipFileName);
					fileNameWithoutExtension = $"{fileNameWithoutExtension}_fixed.zip";
					zipFile.Save(fileNameWithoutExtension);
					return flag;
				}
				return flag;
			}
			finally
			{
				zipFile?.Dispose();
				zipFile2?.Dispose();
			}
		}

		/// <summary>
		///   Rewrite the directory within a zipfile.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   In cases of data error, the directory in a zip file can get out of
		///   synch with the entries in the zip file.  This method attempts to fix
		///   the zip file if this has occurred.
		/// </para>
		///
		/// <para> This can take a long time for large zip files. </para>
		///
		/// <para> This won't work if the zip file uses a non-standard
		/// code page - neither IBM437 nor UTF-8. </para>
		///
		/// <para>
		///   This method is not supported in the Reduced or Compact Framework
		///   versions of DotNetZip.
		/// </para>
		///
		/// <para>
		///   Developers using COM can use the <see cref="M:Ionic.Zip.ComHelper.FixZipDirectory(System.String)">ComHelper.FixZipDirectory(String)</see>
		///   method.
		/// </para>
		///
		/// </remarks>
		///
		/// <param name="zipFileName">The filename to of the zip file to fix.</param>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.CheckZip(System.String)" />
		/// <seealso cref="M:Ionic.Zip.ZipFile.CheckZip(System.String,System.Boolean,System.IO.TextWriter)" />
		public static void FixZipDirectory(string zipFileName)
		{
			using (ZipFile zipFile = new ZipFile())
			{
				zipFile.FullScan = true;
				zipFile.Initialize(zipFileName);
				zipFile.Save(zipFileName);
			}
		}

		/// <summary>
		///   Verify the password on a zip file.
		/// </summary>
		///
		/// <remarks>
		///   <para>
		///     Keep in mind that passwords in zipfiles are applied to
		///     zip entries, not to the entire zip file. So testing a
		///     zipfile for a particular password doesn't work in the
		///     general case. On the other hand, it's often the case
		///     that a single password will be used on all entries in a
		///     zip file. This method works for that case.
		///   </para>
		///   <para>
		///     There is no way to check a password without doing the
		///     decryption. So this code decrypts and extracts the given
		///     zipfile into <see cref="F:System.IO.Stream.Null" />
		///   </para>
		/// </remarks>
		///
		/// <param name="zipFileName">The filename to of the zip file to fix.</param>
		///
		/// <param name="password">The password to check.</param>
		///
		/// <returns>a bool indicating whether the password matches.</returns>
		public static bool CheckZipPassword(string zipFileName, string password)
		{
			bool result = false;
			try
			{
				using (ZipFile zipFile = Read(zipFileName))
				{
					foreach (ZipEntry item in zipFile)
					{
						if (!item.IsDirectory && item.UsesEncryption)
						{
							item.ExtractWithPassword(Stream.Null, password);
						}
					}
				}
				result = true;
				return result;
			}
			catch (BadPasswordException)
			{
				return result;
			}
		}

		/// <summary>
		///   Returns true if an entry by the given name exists in the ZipFile.
		/// </summary>
		///
		/// <param name="name">the name of the entry to find</param>
		/// <returns>true if an entry with the given name exists; otherwise false.
		/// </returns>
		public bool ContainsEntry(string name)
		{
			return RetrievalEntries.ContainsKey(SharedUtilities.NormalizePathForUseInZipFile(name));
		}

		/// <summary>Provides a string representation of the instance.</summary>
		/// <returns>a string representation of the instance.</returns>
		public override string ToString()
		{
			return $"ZipFile::{Name}";
		}

		internal void NotifyEntryChanged()
		{
			_contentsChanged = true;
		}

		internal Stream StreamForDiskNumber(uint diskNumber)
		{
			if (diskNumber + 1 == _diskNumberWithCd || (diskNumber == 0 && _diskNumberWithCd == 0))
			{
				return ReadStream;
			}
			return ZipSegmentedStream.ForReading(_readName ?? _name, diskNumber, _diskNumberWithCd);
		}

		internal void Reset(bool whileSaving)
		{
			if (!_JustSaved)
			{
				return;
			}
			using (ZipFile zipFile = new ZipFile())
			{
				if (File.Exists(_readName ?? _name))
				{
					zipFile._readName = (zipFile._name = (whileSaving ? (_readName ?? _name) : _name));
				}
				else
				{
					if (_readstream.CanSeek)
					{
						_readstream.Seek(0L, SeekOrigin.Begin);
					}
					zipFile._readstream = _readstream;
				}
				zipFile.AlternateEncoding = AlternateEncoding;
				zipFile.AlternateEncodingUsage = AlternateEncodingUsage;
				ReadIntoInstance(zipFile);
				foreach (ZipEntry item in zipFile)
				{
					ZipEntry zipEntry = this[item.FileName];
					if (zipEntry != null && !zipEntry.IsChanged)
					{
						zipEntry.CopyMetaData(item);
					}
				}
			}
			_JustSaved = false;
		}

		/// <summary>
		///   Creates a new <c>ZipFile</c> instance, using the specified filename.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   Applications can use this constructor to create a new ZipFile for writing,
		///   or to slurp in an existing zip archive for read and update purposes.
		/// </para>
		///
		/// <para>
		///   To create a new zip archive, an application can call this constructor,
		///   passing the name of a file that does not exist.  The name may be a fully
		///   qualified path. Then the application can add directories or files to the
		///   <c>ZipFile</c> via <c>AddDirectory()</c>, <c>AddFile()</c>, <c>AddItem()</c>
		///   and then write the zip archive to the disk by calling <c>Save()</c>. The
		///   zip file is not actually opened and written to the disk until the
		///   application calls <c>ZipFile.Save()</c>.  At that point the new zip file
		///   with the given name is created.
		/// </para>
		///
		/// <para>
		///   If you won't know the name of the <c>Zipfile</c> until the time you call
		///   <c>ZipFile.Save()</c>, or if you plan to save to a stream (which has no
		///   name), then you should use the no-argument constructor.
		/// </para>
		///
		/// <para>
		///   The application can also call this constructor to read an existing zip
		///   archive.  passing the name of a valid zip file that does exist. But, it's
		///   better form to use the static <see cref="M:Ionic.Zip.ZipFile.Read(System.String)" /> method,
		///   passing the name of the zip file, because using <c>ZipFile.Read()</c> in
		///   your code communicates very clearly what you are doing.  In either case,
		///   the file is then read into the <c>ZipFile</c> instance.  The app can then
		///   enumerate the entries or can modify the zip file, for example adding
		///   entries, removing entries, changing comments, and so on.
		/// </para>
		///
		/// <para>
		///   One advantage to this parameterized constructor: it allows applications to
		///   use the same code to add items to a zip archive, regardless of whether the
		///   zip file exists.
		/// </para>
		///
		/// <para>
		///   Instances of the <c>ZipFile</c> class are not multi-thread safe.  You may
		///   not party on a single instance with multiple threads.  You may have
		///   multiple threads that each use a distinct <c>ZipFile</c> instance, or you
		///   can synchronize multi-thread access to a single instance.
		/// </para>
		///
		/// <para>
		///   By the way, since DotNetZip is so easy to use, don't you think <see href="http://cheeso.members.winisp.net/DotNetZipDonate.aspx">you should
		///   donate $5 or $10</see>?
		/// </para>
		///
		/// </remarks>
		///
		/// <exception cref="T:Ionic.Zip.ZipException">
		/// Thrown if name refers to an existing file that is not a valid zip file.
		/// </exception>
		///
		/// <example>
		/// This example shows how to create a zipfile, and add a few files into it.
		/// <code>
		/// String ZipFileToCreate = "archive1.zip";
		/// String DirectoryToZip  = "c:\\reports";
		/// using (ZipFile zip = new ZipFile())
		/// {
		///   // Store all files found in the top level directory, into the zip archive.
		///   String[] filenames = System.IO.Directory.GetFiles(DirectoryToZip);
		///   zip.AddFiles(filenames, "files");
		///   zip.Save(ZipFileToCreate);
		/// }
		/// </code>
		///
		/// <code lang="VB">
		/// Dim ZipFileToCreate As String = "archive1.zip"
		/// Dim DirectoryToZip As String = "c:\reports"
		/// Using zip As ZipFile = New ZipFile()
		///     Dim filenames As String() = System.IO.Directory.GetFiles(DirectoryToZip)
		///     zip.AddFiles(filenames, "files")
		///     zip.Save(ZipFileToCreate)
		/// End Using
		/// </code>
		/// </example>
		///
		/// <param name="fileName">The filename to use for the new zip archive.</param>
		public ZipFile(string fileName)
		{
			if (DefaultEncoding == null)
			{
				_alternateEncoding = Encoding.UTF8;
				AlternateEncodingUsage = ZipOption.Always;
			}
			else
			{
				_alternateEncoding = DefaultEncoding;
			}
			try
			{
				_InitInstance(fileName, null);
			}
			catch (Exception innerException)
			{
				throw new ZipException($"Could not read {fileName} as a zip file", innerException);
			}
		}

		/// <summary>
		///   Creates a new <c>ZipFile</c> instance, using the specified name for the
		///   filename, and the specified Encoding.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   See the documentation on the <see cref="M:Ionic.Zip.ZipFile.#ctor(System.String)">ZipFile
		///   constructor that accepts a single string argument</see> for basic
		///   information on all the <c>ZipFile</c> constructors.
		/// </para>
		///
		/// <para>
		///   The Encoding is used as the default alternate encoding for entries with
		///   filenames or comments that cannot be encoded with the IBM437 code page.
		///   This is equivalent to setting the <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" /> property on the <c>ZipFile</c>
		///   instance after construction.
		/// </para>
		///
		/// <para>
		///   Instances of the <c>ZipFile</c> class are not multi-thread safe.  You may
		///   not party on a single instance with multiple threads.  You may have
		///   multiple threads that each use a distinct <c>ZipFile</c> instance, or you
		///   can synchronize multi-thread access to a single instance.
		/// </para>
		///
		/// </remarks>
		///
		/// <exception cref="T:Ionic.Zip.ZipException">
		/// Thrown if name refers to an existing file that is not a valid zip file.
		/// </exception>
		///
		/// <param name="fileName">The filename to use for the new zip archive.</param>
		/// <param name="encoding">The Encoding is used as the default alternate
		/// encoding for entries with filenames or comments that cannot be encoded
		/// with the IBM437 code page. </param>
		public ZipFile(string fileName, Encoding encoding)
		{
			try
			{
				AlternateEncoding = encoding;
				AlternateEncodingUsage = ZipOption.Always;
				_InitInstance(fileName, null);
			}
			catch (Exception innerException)
			{
				throw new ZipException($"{fileName} is not a valid zip file", innerException);
			}
		}

		/// <summary>
		///   Create a zip file, without specifying a target filename or stream to save to.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   See the documentation on the <see cref="M:Ionic.Zip.ZipFile.#ctor(System.String)">ZipFile
		///   constructor that accepts a single string argument</see> for basic
		///   information on all the <c>ZipFile</c> constructors.
		/// </para>
		///
		/// <para>
		///   After instantiating with this constructor and adding entries to the
		///   archive, the application should call <see cref="M:Ionic.Zip.ZipFile.Save(System.String)" /> or
		///   <see cref="M:Ionic.Zip.ZipFile.Save(System.IO.Stream)" /> to save to a file or a
		///   stream, respectively.  The application can also set the <see cref="P:Ionic.Zip.ZipFile.Name" />
		///   property and then call the no-argument <see cref="M:Ionic.Zip.ZipFile.Save" /> method.  (This
		///   is the preferred approach for applications that use the library through
		///   COM interop.)  If you call the no-argument <see cref="M:Ionic.Zip.ZipFile.Save" /> method
		///   without having set the <c>Name</c> of the <c>ZipFile</c>, either through
		///   the parameterized constructor or through the explicit property , the
		///   Save() will throw, because there is no place to save the file.  </para>
		///
		/// <para>
		///   Instances of the <c>ZipFile</c> class are not multi-thread safe.  You may
		///   have multiple threads that each use a distinct <c>ZipFile</c> instance, or
		///   you can synchronize multi-thread access to a single instance.  </para>
		///
		/// </remarks>
		///
		/// <example>
		/// This example creates a Zip archive called Backup.zip, containing all the files
		/// in the directory DirectoryToZip. Files within subdirectories are not zipped up.
		/// <code>
		/// using (ZipFile zip = new ZipFile())
		/// {
		///   // Store all files found in the top level directory, into the zip archive.
		///   // note: this code does not recurse subdirectories!
		///   String[] filenames = System.IO.Directory.GetFiles(DirectoryToZip);
		///   zip.AddFiles(filenames, "files");
		///   zip.Save("Backup.zip");
		/// }
		/// </code>
		///
		/// <code lang="VB">
		/// Using zip As New ZipFile
		///     ' Store all files found in the top level directory, into the zip archive.
		///     ' note: this code does not recurse subdirectories!
		///     Dim filenames As String() = System.IO.Directory.GetFiles(DirectoryToZip)
		///     zip.AddFiles(filenames, "files")
		///     zip.Save("Backup.zip")
		/// End Using
		/// </code>
		/// </example>
		public ZipFile()
		{
			if (DefaultEncoding == null)
			{
				_alternateEncoding = Encoding.UTF8;
				AlternateEncodingUsage = ZipOption.Always;
			}
			else
			{
				_alternateEncoding = DefaultEncoding;
			}
			_InitInstance(null, null);
		}

		/// <summary>
		///   Create a zip file, specifying a text Encoding, but without specifying a
		///   target filename or stream to save to.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   See the documentation on the <see cref="M:Ionic.Zip.ZipFile.#ctor(System.String)">ZipFile
		///   constructor that accepts a single string argument</see> for basic
		///   information on all the <c>ZipFile</c> constructors.
		/// </para>
		///
		/// </remarks>
		///
		/// <param name="encoding">
		/// The Encoding is used as the default alternate encoding for entries with
		/// filenames or comments that cannot be encoded with the IBM437 code page.
		/// </param>
		public ZipFile(Encoding encoding)
		{
			AlternateEncoding = encoding;
			AlternateEncodingUsage = ZipOption.Always;
			_InitInstance(null, null);
		}

		/// <summary>
		///   Creates a new <c>ZipFile</c> instance, using the specified name for the
		///   filename, and the specified status message writer.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   See the documentation on the <see cref="M:Ionic.Zip.ZipFile.#ctor(System.String)">ZipFile
		///   constructor that accepts a single string argument</see> for basic
		///   information on all the <c>ZipFile</c> constructors.
		/// </para>
		///
		/// <para>
		///   This version of the constructor allows the caller to pass in a TextWriter,
		///   to which verbose messages will be written during extraction or creation of
		///   the zip archive.  A console application may wish to pass
		///   System.Console.Out to get messages on the Console. A graphical or headless
		///   application may wish to capture the messages in a different
		///   <c>TextWriter</c>, for example, a <c>StringWriter</c>, and then display
		///   the messages in a TextBox, or generate an audit log of ZipFile operations.
		/// </para>
		///
		/// <para>
		///   To encrypt the data for the files added to the <c>ZipFile</c> instance,
		///   set the Password property after creating the <c>ZipFile</c> instance.
		/// </para>
		///
		/// <para>
		///   Instances of the <c>ZipFile</c> class are not multi-thread safe.  You may
		///   not party on a single instance with multiple threads.  You may have
		///   multiple threads that each use a distinct <c>ZipFile</c> instance, or you
		///   can synchronize multi-thread access to a single instance.
		/// </para>
		///
		/// </remarks>
		///
		/// <exception cref="T:Ionic.Zip.ZipException">
		/// Thrown if name refers to an existing file that is not a valid zip file.
		/// </exception>
		///
		/// <example>
		/// <code>
		/// using (ZipFile zip = new ZipFile("Backup.zip", Console.Out))
		/// {
		///   // Store all files found in the top level directory, into the zip archive.
		///   // note: this code does not recurse subdirectories!
		///   // Status messages will be written to Console.Out
		///   String[] filenames = System.IO.Directory.GetFiles(DirectoryToZip);
		///   zip.AddFiles(filenames);
		///   zip.Save();
		/// }
		/// </code>
		///
		/// <code lang="VB">
		/// Using zip As New ZipFile("Backup.zip", Console.Out)
		///     ' Store all files found in the top level directory, into the zip archive.
		///     ' note: this code does not recurse subdirectories!
		///     ' Status messages will be written to Console.Out
		///     Dim filenames As String() = System.IO.Directory.GetFiles(DirectoryToZip)
		///     zip.AddFiles(filenames)
		///     zip.Save()
		/// End Using
		/// </code>
		/// </example>
		///
		/// <param name="fileName">The filename to use for the new zip archive.</param>
		/// <param name="statusMessageWriter">A TextWriter to use for writing
		/// verbose status messages.</param>
		public ZipFile(string fileName, TextWriter statusMessageWriter)
		{
			if (DefaultEncoding == null)
			{
				_alternateEncoding = Encoding.UTF8;
				AlternateEncodingUsage = ZipOption.Always;
			}
			else
			{
				_alternateEncoding = DefaultEncoding;
			}
			try
			{
				_InitInstance(fileName, statusMessageWriter);
			}
			catch (Exception innerException)
			{
				throw new ZipException($"{fileName} is not a valid zip file", innerException);
			}
		}

		/// <summary>
		///   Creates a new <c>ZipFile</c> instance, using the specified name for the
		///   filename, the specified status message writer, and the specified Encoding.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This constructor works like the <see cref="M:Ionic.Zip.ZipFile.#ctor(System.String)">ZipFile
		///   constructor that accepts a single string argument.</see> See that
		///   reference for detail on what this constructor does.
		/// </para>
		///
		/// <para>
		///   This version of the constructor allows the caller to pass in a
		///   <c>TextWriter</c>, and an Encoding.  The <c>TextWriter</c> will collect
		///   verbose messages that are generated by the library during extraction or
		///   creation of the zip archive.  A console application may wish to pass
		///   <c>System.Console.Out</c> to get messages on the Console. A graphical or
		///   headless application may wish to capture the messages in a different
		///   <c>TextWriter</c>, for example, a <c>StringWriter</c>, and then display
		///   the messages in a <c>TextBox</c>, or generate an audit log of
		///   <c>ZipFile</c> operations.
		/// </para>
		///
		/// <para>
		///   The <c>Encoding</c> is used as the default alternate encoding for entries
		///   with filenames or comments that cannot be encoded with the IBM437 code
		///   page.  This is a equivalent to setting the <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" /> property on the <c>ZipFile</c>
		///   instance after construction.
		/// </para>
		///
		/// <para>
		///   To encrypt the data for the files added to the <c>ZipFile</c> instance,
		///   set the <c>Password</c> property after creating the <c>ZipFile</c>
		///   instance.
		/// </para>
		///
		/// <para>
		///   Instances of the <c>ZipFile</c> class are not multi-thread safe.  You may
		///   not party on a single instance with multiple threads.  You may have
		///   multiple threads that each use a distinct <c>ZipFile</c> instance, or you
		///   can synchronize multi-thread access to a single instance.
		/// </para>
		///
		/// </remarks>
		///
		/// <exception cref="T:Ionic.Zip.ZipException">
		/// Thrown if <c>fileName</c> refers to an existing file that is not a valid zip file.
		/// </exception>
		///
		/// <param name="fileName">The filename to use for the new zip archive.</param>
		/// <param name="statusMessageWriter">A TextWriter to use for writing verbose
		/// status messages.</param>
		/// <param name="encoding">
		/// The Encoding is used as the default alternate encoding for entries with
		/// filenames or comments that cannot be encoded with the IBM437 code page.
		/// </param>
		public ZipFile(string fileName, TextWriter statusMessageWriter, Encoding encoding)
		{
			try
			{
				AlternateEncoding = encoding;
				AlternateEncodingUsage = ZipOption.Always;
				_InitInstance(fileName, statusMessageWriter);
			}
			catch (Exception innerException)
			{
				throw new ZipException($"{fileName} is not a valid zip file", innerException);
			}
		}

		/// <summary>
		///   Initialize a <c>ZipFile</c> instance by reading in a zip file.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   This method is primarily useful from COM Automation environments, when
		///   reading or extracting zip files. In COM, it is not possible to invoke
		///   parameterized constructors for a class. A COM Automation application can
		///   update a zip file by using the <see cref="M:Ionic.Zip.ZipFile.#ctor">default (no argument)
		///   constructor</see>, then calling <c>Initialize()</c> to read the contents
		///   of an on-disk zip archive into the <c>ZipFile</c> instance.
		/// </para>
		///
		/// <para>
		///   .NET applications are encouraged to use the <c>ZipFile.Read()</c> methods
		///   for better clarity.
		/// </para>
		///
		/// </remarks>
		/// <param name="fileName">the name of the existing zip file to read in.</param>
		public void Initialize(string fileName)
		{
			try
			{
				_InitInstance(fileName, null);
			}
			catch (Exception innerException)
			{
				throw new ZipException($"{fileName} is not a valid zip file", innerException);
			}
		}

		private void _InitInstance(string zipFileName, TextWriter statusMessageWriter)
		{
			_name = zipFileName;
			_StatusMessageTextWriter = statusMessageWriter;
			_contentsChanged = true;
			AddDirectoryWillTraverseReparsePoints = true;
			CompressionLevel = CompressionLevel.Default;
			ParallelDeflateThreshold = 524288L;
			_entries = new Dictionary<string, ZipEntry>(StringComparer.Ordinal);
			_entriesInsensitive = new Dictionary<string, ZipEntry>(StringComparer.OrdinalIgnoreCase);
			if (File.Exists(_name))
			{
				if (FullScan)
				{
					ReadIntoInstance_Orig(this);
				}
				else
				{
					ReadIntoInstance(this);
				}
				_fileAlreadyExists = true;
			}
		}

		/// <summary>
		///   Removes the given <c>ZipEntry</c> from the zip archive.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   After calling <c>RemoveEntry</c>, the application must call <c>Save</c> to
		///   make the changes permanent.
		/// </para>
		/// </remarks>
		///
		/// <exception cref="T:System.ArgumentException">
		///   Thrown if the specified <c>ZipEntry</c> does not exist in the <c>ZipFile</c>.
		/// </exception>
		///
		/// <example>
		///   In this example, all entries in the zip archive dating from before
		///   December 31st, 2007, are removed from the archive.  This is actually much
		///   easier if you use the RemoveSelectedEntries method.  But I needed an
		///   example for RemoveEntry, so here it is.
		/// <code>
		/// String ZipFileToRead = "ArchiveToModify.zip";
		/// System.DateTime Threshold = new System.DateTime(2007,12,31);
		/// using (ZipFile zip = ZipFile.Read(ZipFileToRead))
		/// {
		///   var EntriesToRemove = new System.Collections.Generic.List&lt;ZipEntry&gt;();
		///   foreach (ZipEntry e in zip)
		///   {
		///     if (e.LastModified &lt; Threshold)
		///     {
		///       // We cannot remove the entry from the list, within the context of
		///       // an enumeration of said list.
		///       // So we add the doomed entry to a list to be removed later.
		///       EntriesToRemove.Add(e);
		///     }
		///   }
		///
		///   // actually remove the doomed entries.
		///   foreach (ZipEntry zombie in EntriesToRemove)
		///     zip.RemoveEntry(zombie);
		///
		///   zip.Comment= String.Format("This zip archive was updated at {0}.",
		///                              System.DateTime.Now.ToString("G"));
		///
		///   // save with a different name
		///   zip.Save("Archive-Updated.zip");
		/// }
		/// </code>
		///
		/// <code lang="VB">
		///   Dim ZipFileToRead As String = "ArchiveToModify.zip"
		///   Dim Threshold As New DateTime(2007, 12, 31)
		///   Using zip As ZipFile = ZipFile.Read(ZipFileToRead)
		///       Dim EntriesToRemove As New System.Collections.Generic.List(Of ZipEntry)
		///       Dim e As ZipEntry
		///       For Each e In zip
		///           If (e.LastModified &lt; Threshold) Then
		///               ' We cannot remove the entry from the list, within the context of
		///               ' an enumeration of said list.
		///               ' So we add the doomed entry to a list to be removed later.
		///               EntriesToRemove.Add(e)
		///           End If
		///       Next
		///
		///       ' actually remove the doomed entries.
		///       Dim zombie As ZipEntry
		///       For Each zombie In EntriesToRemove
		///           zip.RemoveEntry(zombie)
		///       Next
		///       zip.Comment = String.Format("This zip archive was updated at {0}.", DateTime.Now.ToString("G"))
		///       'save as a different name
		///       zip.Save("Archive-Updated.zip")
		///   End Using
		/// </code>
		/// </example>
		///
		/// <param name="entry">
		/// The <c>ZipEntry</c> to remove from the zip.
		/// </param>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.RemoveSelectedEntries(System.String)" />
		public void RemoveEntry(ZipEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			string text = SharedUtilities.NormalizePathForUseInZipFile(entry.FileName);
			_entries.Remove(text);
			if (!AnyCaseInsensitiveMatches(text))
			{
				_entriesInsensitive.Remove(text);
			}
			_zipEntriesAsList = null;
			_contentsChanged = true;
		}

		private bool AnyCaseInsensitiveMatches(string path)
		{
			foreach (ZipEntry value in _entries.Values)
			{
				if (string.Equals(value.FileName, path, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Removes the <c>ZipEntry</c> with the given filename from the zip archive.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   After calling <c>RemoveEntry</c>, the application must call <c>Save</c> to
		///   make the changes permanent.
		/// </para>
		///
		/// </remarks>
		///
		/// <exception cref="T:System.InvalidOperationException">
		///   Thrown if the <c>ZipFile</c> is not updatable.
		/// </exception>
		///
		/// <exception cref="T:System.ArgumentException">
		///   Thrown if a <c>ZipEntry</c> with the specified filename does not exist in
		///   the <c>ZipFile</c>.
		/// </exception>
		///
		/// <example>
		///
		///   This example shows one way to remove an entry with a given filename from
		///   an existing zip archive.
		///
		/// <code>
		/// String zipFileToRead= "PackedDocuments.zip";
		/// string candidate = "DatedMaterial.xps";
		/// using (ZipFile zip = ZipFile.Read(zipFileToRead))
		/// {
		///   if (zip.EntryFilenames.Contains(candidate))
		///   {
		///     zip.RemoveEntry(candidate);
		///     zip.Comment= String.Format("The file '{0}' has been removed from this archive.",
		///                                Candidate);
		///     zip.Save();
		///   }
		/// }
		/// </code>
		/// <code lang="VB">
		///   Dim zipFileToRead As String = "PackedDocuments.zip"
		///   Dim candidate As String = "DatedMaterial.xps"
		///   Using zip As ZipFile = ZipFile.Read(zipFileToRead)
		///       If zip.EntryFilenames.Contains(candidate) Then
		///           zip.RemoveEntry(candidate)
		///           zip.Comment = String.Format("The file '{0}' has been removed from this archive.", Candidate)
		///           zip.Save
		///       End If
		///   End Using
		/// </code>
		/// </example>
		///
		/// <param name="fileName">
		/// The name of the file, including any directory path, to remove from the zip.
		/// The filename match is not case-sensitive by default; you can use the
		/// <c>CaseSensitiveRetrieval</c> property to change this behavior. The
		/// pathname can use forward-slashes or backward slashes.
		/// </param>
		public void RemoveEntry(string fileName)
		{
			string fileName2 = ZipEntry.NameInArchive(fileName, null);
			ZipEntry zipEntry = this[fileName2];
			if (zipEntry == null)
			{
				throw new ArgumentException("The entry you specified was not found in the zip archive.");
			}
			RemoveEntry(zipEntry);
		}

		/// <summary>
		///   Closes the read and write streams associated
		///   to the <c>ZipFile</c>, if necessary.
		/// </summary>
		///
		/// <remarks>
		///   The Dispose() method is generally employed implicitly, via a <c>using(..) {..}</c>
		///   statement. (<c>Using...End Using</c> in VB) If you do not employ a using
		///   statement, insure that your application calls Dispose() explicitly.  For
		///   example, in a Powershell application, or an application that uses the COM
		///   interop interface, you must call Dispose() explicitly.
		/// </remarks>
		///
		/// <example>
		/// This example extracts an entry selected by name, from the Zip file to the
		/// Console.
		/// <code>
		/// using (ZipFile zip = ZipFile.Read(zipfile))
		/// {
		///   foreach (ZipEntry e in zip)
		///   {
		///     if (WantThisEntry(e.FileName))
		///       zip.Extract(e.FileName, Console.OpenStandardOutput());
		///   }
		/// } // Dispose() is called implicitly here.
		/// </code>
		///
		/// <code lang="VB">
		/// Using zip As ZipFile = ZipFile.Read(zipfile)
		///     Dim e As ZipEntry
		///     For Each e In zip
		///       If WantThisEntry(e.FileName) Then
		///           zip.Extract(e.FileName, Console.OpenStandardOutput())
		///       End If
		///     Next
		/// End Using ' Dispose is implicity called here
		/// </code>
		/// </example>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		///   Disposes any managed resources, if the flag is set, then marks the
		///   instance disposed.  This method is typically not called explicitly from
		///   application code.
		/// </summary>
		///
		/// <remarks>
		///   Applications should call <see cref="M:Ionic.Zip.ZipFile.Dispose">the no-arg Dispose method</see>.
		/// </remarks>
		///
		/// <param name="disposeManagedResources">
		///   indicates whether the method should dispose streams or not.
		/// </param>
		protected virtual void Dispose(bool disposeManagedResources)
		{
			if (_disposed)
			{
				return;
			}
			if (disposeManagedResources)
			{
				if (_ReadStreamIsOurs && _readstream != null)
				{
					_readstream.Dispose();
					_readstream = null;
				}
				if (_temporaryFileName != null && _name != null && _writestream != null)
				{
					_writestream.Dispose();
					_writestream = null;
				}
				if (ParallelDeflater != null)
				{
					ParallelDeflater.Dispose();
					ParallelDeflater = null;
				}
			}
			_disposed = true;
		}

		internal bool OnSaveBlock(ZipEntry entry, long bytesXferred, long totalBytesToXfer)
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = SaveProgressEventArgs.ByteUpdate(ArchiveNameForEvent, entry, bytesXferred, totalBytesToXfer);
				saveProgress(this, saveProgressEventArgs);
				if (saveProgressEventArgs.Cancel)
				{
					_saveOperationCanceled = true;
				}
			}
			return _saveOperationCanceled;
		}

		private void OnSaveEntry(int current, ZipEntry entry, bool before)
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = new SaveProgressEventArgs(ArchiveNameForEvent, before, _entries.Count, current, entry);
				saveProgress(this, saveProgressEventArgs);
				if (saveProgressEventArgs.Cancel)
				{
					_saveOperationCanceled = true;
				}
			}
		}

		private void OnSaveEvent(ZipProgressEventType eventFlavor)
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = new SaveProgressEventArgs(ArchiveNameForEvent, eventFlavor);
				saveProgress(this, saveProgressEventArgs);
				if (saveProgressEventArgs.Cancel)
				{
					_saveOperationCanceled = true;
				}
			}
		}

		private void OnSaveStarted()
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = SaveProgressEventArgs.Started(ArchiveNameForEvent);
				saveProgress(this, saveProgressEventArgs);
				if (saveProgressEventArgs.Cancel)
				{
					_saveOperationCanceled = true;
				}
			}
		}

		private void OnSaveCompleted()
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs e = SaveProgressEventArgs.Completed(ArchiveNameForEvent);
				saveProgress(this, e);
			}
		}

		private void OnReadStarted()
		{
			EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
			if (readProgress != null)
			{
				ReadProgressEventArgs e = ReadProgressEventArgs.Started(ArchiveNameForEvent);
				readProgress(this, e);
			}
		}

		private void OnReadCompleted()
		{
			EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
			if (readProgress != null)
			{
				ReadProgressEventArgs e = ReadProgressEventArgs.Completed(ArchiveNameForEvent);
				readProgress(this, e);
			}
		}

		internal void OnReadBytes(ZipEntry entry)
		{
			EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
			if (readProgress != null)
			{
				ReadProgressEventArgs e = ReadProgressEventArgs.ByteUpdate(ArchiveNameForEvent, entry, ReadStream.Position, LengthOfReadStream);
				readProgress(this, e);
			}
		}

		internal void OnReadEntry(bool before, ZipEntry entry)
		{
			EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
			if (readProgress != null)
			{
				ReadProgressEventArgs e = (before ? ReadProgressEventArgs.Before(ArchiveNameForEvent, _entries.Count) : ReadProgressEventArgs.After(ArchiveNameForEvent, entry, _entries.Count));
				readProgress(this, e);
			}
		}

		private void OnExtractEntry(int current, bool before, ZipEntry currentEntry, string path)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = new ExtractProgressEventArgs(ArchiveNameForEvent, before, _entries.Count, current, currentEntry, path);
				extractProgress(this, extractProgressEventArgs);
				if (extractProgressEventArgs.Cancel)
				{
					_extractOperationCanceled = true;
				}
			}
		}

		internal bool OnExtractBlock(ZipEntry entry, long bytesWritten, long totalBytesToWrite)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = ExtractProgressEventArgs.ByteUpdate(ArchiveNameForEvent, entry, bytesWritten, totalBytesToWrite);
				extractProgress(this, extractProgressEventArgs);
				if (extractProgressEventArgs.Cancel)
				{
					_extractOperationCanceled = true;
				}
			}
			return _extractOperationCanceled;
		}

		internal bool OnSingleEntryExtract(ZipEntry entry, string path, bool before)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = (before ? ExtractProgressEventArgs.BeforeExtractEntry(ArchiveNameForEvent, entry, path) : ExtractProgressEventArgs.AfterExtractEntry(ArchiveNameForEvent, entry, path));
				extractProgress(this, extractProgressEventArgs);
				if (extractProgressEventArgs.Cancel)
				{
					_extractOperationCanceled = true;
				}
			}
			return _extractOperationCanceled;
		}

		internal bool OnExtractExisting(ZipEntry entry, string path)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = ExtractProgressEventArgs.ExtractExisting(ArchiveNameForEvent, entry, path);
				extractProgress(this, extractProgressEventArgs);
				if (extractProgressEventArgs.Cancel)
				{
					_extractOperationCanceled = true;
				}
			}
			return _extractOperationCanceled;
		}

		private void OnExtractAllCompleted(string path)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs e = ExtractProgressEventArgs.ExtractAllCompleted(ArchiveNameForEvent, path);
				extractProgress(this, e);
			}
		}

		private void OnExtractAllStarted(string path)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs e = ExtractProgressEventArgs.ExtractAllStarted(ArchiveNameForEvent, path);
				extractProgress(this, e);
			}
		}

		private void OnAddStarted()
		{
			EventHandler<AddProgressEventArgs> addProgress = this.AddProgress;
			if (addProgress != null)
			{
				AddProgressEventArgs addProgressEventArgs = AddProgressEventArgs.Started(ArchiveNameForEvent);
				addProgress(this, addProgressEventArgs);
				if (addProgressEventArgs.Cancel)
				{
					_addOperationCanceled = true;
				}
			}
		}

		private void OnAddCompleted()
		{
			EventHandler<AddProgressEventArgs> addProgress = this.AddProgress;
			if (addProgress != null)
			{
				AddProgressEventArgs e = AddProgressEventArgs.Completed(ArchiveNameForEvent);
				addProgress(this, e);
			}
		}

		internal void AfterAddEntry(ZipEntry entry)
		{
			EventHandler<AddProgressEventArgs> addProgress = this.AddProgress;
			if (addProgress != null)
			{
				AddProgressEventArgs addProgressEventArgs = AddProgressEventArgs.AfterEntry(ArchiveNameForEvent, entry, _entries.Count);
				addProgress(this, addProgressEventArgs);
				if (addProgressEventArgs.Cancel)
				{
					_addOperationCanceled = true;
				}
			}
		}

		internal bool OnZipErrorSaving(ZipEntry entry, Exception exc)
		{
			if (this.ZipError != null)
			{
				lock (LOCK)
				{
					ZipErrorEventArgs zipErrorEventArgs = ZipErrorEventArgs.Saving(Name, entry, exc);
					this.ZipError(this, zipErrorEventArgs);
					if (zipErrorEventArgs.Cancel)
					{
						_saveOperationCanceled = true;
					}
				}
			}
			return _saveOperationCanceled;
		}

		/// <summary>
		/// Extracts all of the items in the zip archive, to the specified path in the
		/// filesystem.  The path can be relative or fully-qualified.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This method will extract all entries in the <c>ZipFile</c> to the
		///   specified path.
		/// </para>
		///
		/// <para>
		///   If an extraction of a file from the zip archive would overwrite an
		///   existing file in the filesystem, the action taken is dictated by the
		///   ExtractExistingFile property, which overrides any setting you may have
		///   made on individual ZipEntry instances.  By default, if you have not
		///   set that property on the <c>ZipFile</c> instance, the entry will not
		///   be extracted, the existing file will not be overwritten and an
		///   exception will be thrown. To change this, set the property, or use the
		///   <see cref="M:Ionic.Zip.ZipFile.ExtractAll(System.String,Ionic.Zip.ExtractExistingFileAction)" /> overload that allows you to
		///   specify an ExtractExistingFileAction parameter.
		/// </para>
		///
		/// <para>
		///   The action to take when an extract would overwrite an existing file
		///   applies to all entries.  If you want to set this on a per-entry basis,
		///   then you must use one of the <see cref="M:Ionic.Zip.ZipEntry.Extract">ZipEntry.Extract</see> methods.
		/// </para>
		///
		/// <para>
		///   This method will send verbose output messages to the <see cref="P:Ionic.Zip.ZipFile.StatusMessageTextWriter" />, if it is set on the <c>ZipFile</c>
		///   instance.
		/// </para>
		///
		/// <para>
		/// You may wish to take advantage of the <c>ExtractProgress</c> event.
		/// </para>
		///
		/// <para>
		///   About timestamps: When extracting a file entry from a zip archive, the
		///   extracted file gets the last modified time of the entry as stored in
		///   the archive. The archive may also store extended file timestamp
		///   information, including last accessed and created times. If these are
		///   present in the <c>ZipEntry</c>, then the extracted file will also get
		///   these times.
		/// </para>
		///
		/// <para>
		///   A Directory entry is somewhat different. It will get the times as
		///   described for a file entry, but, if there are file entries in the zip
		///   archive that, when extracted, appear in the just-created directory,
		///   then when those file entries are extracted, the last modified and last
		///   accessed times of the directory will change, as a side effect.  The
		///   result is that after an extraction of a directory and a number of
		///   files within the directory, the last modified and last accessed
		///   timestamps on the directory will reflect the time that the last file
		///   was extracted into the directory, rather than the time stored in the
		///   zip archive for the directory.
		/// </para>
		///
		/// <para>
		///   To compensate, when extracting an archive with <c>ExtractAll</c>,
		///   DotNetZip will extract all the file and directory entries as described
		///   above, but it will then make a second pass on the directories, and
		///   reset the times on the directories to reflect what is stored in the
		///   zip archive.
		/// </para>
		///
		/// <para>
		///   This compensation is performed only within the context of an
		///   <c>ExtractAll</c>. If you call <c>ZipEntry.Extract</c> on a directory
		///   entry, the timestamps on directory in the filesystem will reflect the
		///   times stored in the zip.  If you then call <c>ZipEntry.Extract</c> on
		///   a file entry, which is extracted into the directory, the timestamps on
		///   the directory will be updated to the current time.
		/// </para>
		/// </remarks>
		///
		/// <example>
		///   This example extracts all the entries in a zip archive file, to the
		///   specified target directory.  The extraction will overwrite any
		///   existing files silently.
		///
		/// <code>
		/// String TargetDirectory= "unpack";
		/// using(ZipFile zip= ZipFile.Read(ZipFileToExtract))
		/// {
		///     zip.ExtractExistingFile= ExtractExistingFileAction.OverwriteSilently;
		///     zip.ExtractAll(TargetDirectory);
		/// }
		/// </code>
		///
		/// <code lang="VB">
		/// Dim TargetDirectory As String = "unpack"
		/// Using zip As ZipFile = ZipFile.Read(ZipFileToExtract)
		///     zip.ExtractExistingFile= ExtractExistingFileAction.OverwriteSilently
		///     zip.ExtractAll(TargetDirectory)
		/// End Using
		/// </code>
		/// </example>
		///
		/// <seealso cref="E:Ionic.Zip.ZipFile.ExtractProgress" />
		/// <seealso cref="P:Ionic.Zip.ZipFile.ExtractExistingFile" />
		///
		/// <param name="path">
		///   The path to which the contents of the zipfile will be extracted.
		///   The path can be relative or fully-qualified.
		/// </param>
		public void ExtractAll(string path)
		{
			_InternalExtractAll(path, true);
		}

		/// <summary>
		/// Extracts all of the items in the zip archive, to the specified path in the
		/// filesystem, using the specified behavior when extraction would overwrite an
		/// existing file.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		/// This method will extract all entries in the <c>ZipFile</c> to the specified
		/// path.  For an extraction that would overwrite an existing file, the behavior
		/// is dictated by <paramref name="extractExistingFile" />, which overrides any
		/// setting you may have made on individual ZipEntry instances.
		/// </para>
		///
		/// <para>
		/// The action to take when an extract would overwrite an existing file
		/// applies to all entries.  If you want to set this on a per-entry basis,
		/// then you must use <see cref="M:Ionic.Zip.ZipEntry.Extract(System.String,Ionic.Zip.ExtractExistingFileAction)" /> or one of the similar methods.
		/// </para>
		///
		/// <para>
		/// Calling this method is equivalent to setting the <see cref="P:Ionic.Zip.ZipFile.ExtractExistingFile" /> property and then calling <see cref="M:Ionic.Zip.ZipFile.ExtractAll(System.String)" />.
		/// </para>
		///
		/// <para>
		/// This method will send verbose output messages to the
		/// <see cref="P:Ionic.Zip.ZipFile.StatusMessageTextWriter" />, if it is set on the <c>ZipFile</c> instance.
		/// </para>
		/// </remarks>
		///
		/// <example>
		/// This example extracts all the entries in a zip archive file, to the
		/// specified target directory.  It does not overwrite any existing files.
		/// <code>
		/// String TargetDirectory= "c:\\unpack";
		/// using(ZipFile zip= ZipFile.Read(ZipFileToExtract))
		/// {
		///   zip.ExtractAll(TargetDirectory, ExtractExistingFileAction.DontOverwrite);
		/// }
		/// </code>
		///
		/// <code lang="VB">
		/// Dim TargetDirectory As String = "c:\unpack"
		/// Using zip As ZipFile = ZipFile.Read(ZipFileToExtract)
		///     zip.ExtractAll(TargetDirectory, ExtractExistingFileAction.DontOverwrite)
		/// End Using
		/// </code>
		/// </example>
		///
		/// <param name="path">
		/// The path to which the contents of the zipfile will be extracted.
		/// The path can be relative or fully-qualified.
		/// </param>
		///
		/// <param name="extractExistingFile">
		/// The action to take if extraction would overwrite an existing file.
		/// </param>
		/// <seealso cref="M:Ionic.Zip.ZipFile.ExtractSelectedEntries(System.String,Ionic.Zip.ExtractExistingFileAction)" />
		public void ExtractAll(string path, ExtractExistingFileAction extractExistingFile)
		{
			ExtractExistingFile = extractExistingFile;
			_InternalExtractAll(path, true);
		}

		private void _InternalExtractAll(string path, bool overrideExtractExistingProperty)
		{
			bool flag = Verbose;
			_inExtractAll = true;
			try
			{
				OnExtractAllStarted(path);
				int num = 0;
				foreach (ZipEntry value in _entries.Values)
				{
					if (flag)
					{
						StatusMessageTextWriter.WriteLine("\n{1,-22} {2,-8} {3,4}   {4,-8}  {0}", "Name", "Modified", "Size", "Ratio", "Packed");
						StatusMessageTextWriter.WriteLine(new string('-', 72));
						flag = false;
					}
					if (Verbose)
					{
						StatusMessageTextWriter.WriteLine("{1,-22} {2,-8} {3,4:F0}%   {4,-8} {0}", value.FileName, value.LastModified.ToString("yyyy-MM-dd HH:mm:ss"), value.UncompressedSize, value.CompressionRatio, value.CompressedSize);
						if (!string.IsNullOrEmpty(value.Comment))
						{
							StatusMessageTextWriter.WriteLine("  Comment: {0}", value.Comment);
						}
					}
					value.Password = _Password;
					OnExtractEntry(num, true, value, path);
					if (overrideExtractExistingProperty)
					{
						value.ExtractExistingFile = ExtractExistingFile;
					}
					value.Extract(path);
					num++;
					OnExtractEntry(num, false, value, path);
					if (_extractOperationCanceled)
					{
						break;
					}
				}
				if (_extractOperationCanceled)
				{
					return;
				}
				foreach (ZipEntry value2 in _entries.Values)
				{
					if (value2.IsDirectory || value2.FileName.EndsWith("/"))
					{
						string fileOrDirectory = (value2.FileName.StartsWith("/") ? Path.Combine(path, value2.FileName.Substring(1)) : Path.Combine(path, value2.FileName));
						value2._SetTimes(fileOrDirectory, false);
					}
				}
				OnExtractAllCompleted(path);
			}
			finally
			{
				_inExtractAll = false;
			}
		}

		/// <summary>
		/// Reads a zip file archive and returns the instance.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		/// The stream is read using the default <c>System.Text.Encoding</c>, which is the
		/// <c>IBM437</c> codepage.
		/// </para>
		/// </remarks>
		///
		/// <exception cref="T:System.Exception">
		/// Thrown if the <c>ZipFile</c> cannot be read. The implementation of this method
		/// relies on <c>System.IO.File.OpenRead</c>, which can throw a variety of exceptions,
		/// including specific exceptions if a file is not found, an unauthorized access
		/// exception, exceptions for poorly formatted filenames, and so on.
		/// </exception>
		///
		/// <param name="fileName">
		/// The name of the zip archive to open.  This can be a fully-qualified or relative
		/// pathname.
		/// </param>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.Read(System.String,Ionic.Zip.ReadOptions)" />.
		///
		/// <returns>The instance read from the zip archive.</returns>
		public static ZipFile Read(string fileName)
		{
			return Read(fileName, null, null, null);
		}

		/// <summary>
		///   Reads a zip file archive from the named filesystem file using the
		///   specified options.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This version of the <c>Read()</c> method allows the caller to pass
		///   in a <c>TextWriter</c> an <c>Encoding</c>, via an instance of the
		///   <c>ReadOptions</c> class.  The <c>ZipFile</c> is read in using the
		///   specified encoding for entries where UTF-8 encoding is not
		///   explicitly specified.
		/// </para>
		/// </remarks>
		///
		/// <example>
		///
		/// <para>
		///   This example shows how to read a zip file using the Big-5 Chinese
		///   code page (950), and extract each entry in the zip file, while
		///   sending status messages out to the Console.
		/// </para>
		///
		/// <para>
		///   For this code to work as intended, the zipfile must have been
		///   created using the big5 code page (CP950). This is typical, for
		///   example, when using WinRar on a machine with CP950 set as the
		///   default code page.  In that case, the names of entries within the
		///   Zip archive will be stored in that code page, and reading the zip
		///   archive must be done using that code page.  If the application did
		///   not use the correct code page in ZipFile.Read(), then names of
		///   entries within the zip archive would not be correctly retrieved.
		/// </para>
		///
		/// <code lang="C#">
		/// string zipToExtract = "MyArchive.zip";
		/// string extractDirectory = "extract";
		/// var options = new ReadOptions
		/// {
		///   StatusMessageWriter = System.Console.Out,
		///   Encoding = System.Text.Encoding.GetEncoding(950)
		/// };
		/// using (ZipFile zip = ZipFile.Read(zipToExtract, options))
		/// {
		///   foreach (ZipEntry e in zip)
		///   {
		///      e.Extract(extractDirectory);
		///   }
		/// }
		/// </code>
		///
		///
		/// <code lang="VB">
		/// Dim zipToExtract as String = "MyArchive.zip"
		/// Dim extractDirectory as String = "extract"
		/// Dim options as New ReadOptions
		/// options.Encoding = System.Text.Encoding.GetEncoding(950)
		/// options.StatusMessageWriter = System.Console.Out
		/// Using zip As ZipFile = ZipFile.Read(zipToExtract, options)
		///     Dim e As ZipEntry
		///     For Each e In zip
		///      e.Extract(extractDirectory)
		///     Next
		/// End Using
		/// </code>
		/// </example>
		///
		///
		/// <example>
		///
		/// <para>
		///   This example shows how to read a zip file using the default
		///   code page, to remove entries that have a modified date before a given threshold,
		///   sending status messages out to a <c>StringWriter</c>.
		/// </para>
		///
		/// <code lang="C#">
		/// var options = new ReadOptions
		/// {
		///   StatusMessageWriter = new System.IO.StringWriter()
		/// };
		/// using (ZipFile zip =  ZipFile.Read("PackedDocuments.zip", options))
		/// {
		///   var Threshold = new DateTime(2007,7,4);
		///   // We cannot remove the entry from the list, within the context of
		///   // an enumeration of said list.
		///   // So we add the doomed entry to a list to be removed later.
		///   // pass 1: mark the entries for removal
		///   var MarkedEntries = new System.Collections.Generic.List&lt;ZipEntry&gt;();
		///   foreach (ZipEntry e in zip)
		///   {
		///     if (e.LastModified &lt; Threshold)
		///       MarkedEntries.Add(e);
		///   }
		///   // pass 2: actually remove the entry.
		///   foreach (ZipEntry zombie in MarkedEntries)
		///      zip.RemoveEntry(zombie);
		///   zip.Comment = "This archive has been updated.";
		///   zip.Save();
		/// }
		/// // can now use contents of sw, eg store in an audit log
		/// </code>
		///
		/// <code lang="VB">
		/// Dim options as New ReadOptions
		/// options.StatusMessageWriter = New System.IO.StringWriter
		/// Using zip As ZipFile = ZipFile.Read("PackedDocuments.zip", options)
		///     Dim Threshold As New DateTime(2007, 7, 4)
		///     ' We cannot remove the entry from the list, within the context of
		///     ' an enumeration of said list.
		///     ' So we add the doomed entry to a list to be removed later.
		///     ' pass 1: mark the entries for removal
		///     Dim MarkedEntries As New System.Collections.Generic.List(Of ZipEntry)
		///     Dim e As ZipEntry
		///     For Each e In zip
		///         If (e.LastModified &lt; Threshold) Then
		///             MarkedEntries.Add(e)
		///         End If
		///     Next
		///     ' pass 2: actually remove the entry.
		///     Dim zombie As ZipEntry
		///     For Each zombie In MarkedEntries
		///         zip.RemoveEntry(zombie)
		///     Next
		///     zip.Comment = "This archive has been updated."
		///     zip.Save
		/// End Using
		/// ' can now use contents of sw, eg store in an audit log
		/// </code>
		/// </example>
		///
		/// <exception cref="T:System.Exception">
		///   Thrown if the zipfile cannot be read. The implementation of
		///   this method relies on <c>System.IO.File.OpenRead</c>, which
		///   can throw a variety of exceptions, including specific
		///   exceptions if a file is not found, an unauthorized access
		///   exception, exceptions for poorly formatted filenames, and so
		///   on.
		/// </exception>
		///
		/// <param name="fileName">
		/// The name of the zip archive to open.
		/// This can be a fully-qualified or relative pathname.
		/// </param>
		///
		/// <param name="options">
		/// The set of options to use when reading the zip file.
		/// </param>
		///
		/// <returns>The ZipFile instance read from the zip archive.</returns>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.Read(System.IO.Stream,Ionic.Zip.ReadOptions)" />
		public static ZipFile Read(string fileName, ReadOptions options)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			return Read(fileName, options.StatusMessageWriter, options.Encoding, options.ReadProgress);
		}

		/// <summary>
		/// Reads a zip file archive using the specified text encoding,  the specified
		/// TextWriter for status messages, and the specified ReadProgress event handler,
		/// and returns the instance.
		/// </summary>
		///
		/// <param name="fileName">
		/// The name of the zip archive to open.
		/// This can be a fully-qualified or relative pathname.
		/// </param>
		///
		/// <param name="readProgress">
		/// An event handler for Read operations.
		/// </param>
		///
		/// <param name="statusMessageWriter">
		/// The <c>System.IO.TextWriter</c> to use for writing verbose status messages
		/// during operations on the zip archive.  A console application may wish to
		/// pass <c>System.Console.Out</c> to get messages on the Console. A graphical
		/// or headless application may wish to capture the messages in a different
		/// <c>TextWriter</c>, such as a <c>System.IO.StringWriter</c>.
		/// </param>
		///
		/// <param name="encoding">
		/// The <c>System.Text.Encoding</c> to use when reading in the zip archive. Be
		/// careful specifying the encoding.  If the value you use here is not the same
		/// as the Encoding used when the zip archive was created (possibly by a
		/// different archiver) you will get unexpected results and possibly exceptions.
		/// </param>
		///
		/// <returns>The instance read from the zip archive.</returns>
		private static ZipFile Read(string fileName, TextWriter statusMessageWriter, Encoding encoding, EventHandler<ReadProgressEventArgs> readProgress)
		{
			ZipFile zipFile = new ZipFile();
			zipFile.AlternateEncoding = encoding ?? DefaultEncoding;
			zipFile.AlternateEncodingUsage = ZipOption.Always;
			zipFile._StatusMessageTextWriter = statusMessageWriter;
			zipFile._name = fileName;
			if (readProgress != null)
			{
				zipFile.ReadProgress = readProgress;
			}
			if (zipFile.Verbose)
			{
				zipFile._StatusMessageTextWriter.WriteLine("reading from {0}...", fileName);
			}
			ReadIntoInstance(zipFile);
			zipFile._fileAlreadyExists = true;
			return zipFile;
		}

		/// <summary>
		///   Reads a zip archive from a stream.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   When reading from a file, it's probably easier to just use
		///   <see cref="M:Ionic.Zip.ZipFile.Read(System.String,Ionic.Zip.ReadOptions)">ZipFile.Read(String, ReadOptions)</see>.  This
		///   overload is useful when when the zip archive content is
		///   available from an already-open stream. The stream must be
		///   open and readable and seekable when calling this method.  The
		///   stream is left open when the reading is completed.
		/// </para>
		///
		/// <para>
		///   Using this overload, the stream is read using the default
		///   <c>System.Text.Encoding</c>, which is the <c>IBM437</c>
		///   codepage. If you want to specify the encoding to use when
		///   reading the zipfile content, see
		///   <see cref="M:Ionic.Zip.ZipFile.Read(System.IO.Stream,Ionic.Zip.ReadOptions)">ZipFile.Read(Stream, ReadOptions)</see>.  This
		/// </para>
		///
		/// <para>
		///   Reading of zip content begins at the current position in the
		///   stream.  This means if you have a stream that concatenates
		///   regular data and zip data, if you position the open, readable
		///   stream at the start of the zip data, you will be able to read
		///   the zip archive using this constructor, or any of the ZipFile
		///   constructors that accept a <see cref="T:System.IO.Stream" /> as
		///   input. Some examples of where this might be useful: the zip
		///   content is concatenated at the end of a regular EXE file, as
		///   some self-extracting archives do.  (Note: SFX files produced
		///   by DotNetZip do not work this way; they can be read as normal
		///   ZIP files). Another example might be a stream being read from
		///   a database, where the zip content is embedded within an
		///   aggregate stream of data.
		/// </para>
		///
		/// </remarks>
		///
		/// <example>
		/// <para>
		///   This example shows how to Read zip content from a stream, and
		///   extract one entry into a different stream. In this example,
		///   the filename "NameOfEntryInArchive.doc", refers only to the
		///   name of the entry within the zip archive.  A file by that
		///   name is not created in the filesystem.  The I/O is done
		///   strictly with the given streams.
		/// </para>
		///
		/// <code>
		/// using (ZipFile zip = ZipFile.Read(InputStream))
		/// {
		///    zip.Extract("NameOfEntryInArchive.doc", OutputStream);
		/// }
		/// </code>
		///
		/// <code lang="VB">
		/// Using zip as ZipFile = ZipFile.Read(InputStream)
		///    zip.Extract("NameOfEntryInArchive.doc", OutputStream)
		/// End Using
		/// </code>
		/// </example>
		///
		/// <param name="zipStream">the stream containing the zip data.</param>
		///
		/// <returns>The ZipFile instance read from the stream</returns>
		public static ZipFile Read(Stream zipStream)
		{
			return Read(zipStream, null, null, null);
		}

		/// <summary>
		///   Reads a zip file archive from the given stream using the
		///   specified options.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   When reading from a file, it's probably easier to just use
		///   <see cref="M:Ionic.Zip.ZipFile.Read(System.String,Ionic.Zip.ReadOptions)">ZipFile.Read(String, ReadOptions)</see>.  This
		///   overload is useful when when the zip archive content is
		///   available from an already-open stream. The stream must be
		///   open and readable and seekable when calling this method.  The
		///   stream is left open when the reading is completed.
		/// </para>
		///
		/// <para>
		///   Reading of zip content begins at the current position in the
		///   stream.  This means if you have a stream that concatenates
		///   regular data and zip data, if you position the open, readable
		///   stream at the start of the zip data, you will be able to read
		///   the zip archive using this constructor, or any of the ZipFile
		///   constructors that accept a <see cref="T:System.IO.Stream" /> as
		///   input. Some examples of where this might be useful: the zip
		///   content is concatenated at the end of a regular EXE file, as
		///   some self-extracting archives do.  (Note: SFX files produced
		///   by DotNetZip do not work this way; they can be read as normal
		///   ZIP files). Another example might be a stream being read from
		///   a database, where the zip content is embedded within an
		///   aggregate stream of data.
		/// </para>
		/// </remarks>
		///
		/// <param name="zipStream">the stream containing the zip data.</param>
		///
		/// <param name="options">
		///   The set of options to use when reading the zip file.
		/// </param>
		///
		/// <exception cref="T:System.Exception">
		///   Thrown if the zip archive cannot be read.
		/// </exception>
		///
		/// <returns>The ZipFile instance read from the stream.</returns>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.Read(System.String,Ionic.Zip.ReadOptions)" />
		public static ZipFile Read(Stream zipStream, ReadOptions options)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			return Read(zipStream, options.StatusMessageWriter, options.Encoding, options.ReadProgress);
		}

		/// <summary>
		/// Reads a zip archive from a stream, using the specified text Encoding, the
		/// specified TextWriter for status messages,
		/// and the specified ReadProgress event handler.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		/// Reading of zip content begins at the current position in the stream.  This
		/// means if you have a stream that concatenates regular data and zip data, if
		/// you position the open, readable stream at the start of the zip data, you
		/// will be able to read the zip archive using this constructor, or any of the
		/// ZipFile constructors that accept a <see cref="T:System.IO.Stream" /> as
		/// input. Some examples of where this might be useful: the zip content is
		/// concatenated at the end of a regular EXE file, as some self-extracting
		/// archives do.  (Note: SFX files produced by DotNetZip do not work this
		/// way). Another example might be a stream being read from a database, where
		/// the zip content is embedded within an aggregate stream of data.
		/// </para>
		/// </remarks>
		///
		/// <param name="zipStream">the stream containing the zip data.</param>
		///
		/// <param name="statusMessageWriter">
		/// The <c>System.IO.TextWriter</c> to which verbose status messages are written
		/// during operations on the <c>ZipFile</c>.  For example, in a console
		/// application, System.Console.Out works, and will get a message for each entry
		/// added to the ZipFile.  If the TextWriter is <c>null</c>, no verbose messages
		/// are written.
		/// </param>
		///
		/// <param name="encoding">
		/// The text encoding to use when reading entries that do not have the UTF-8
		/// encoding bit set.  Be careful specifying the encoding.  If the value you use
		/// here is not the same as the Encoding used when the zip archive was created
		/// (possibly by a different archiver) you will get unexpected results and
		/// possibly exceptions.  See the <see cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" />
		/// property for more information.
		/// </param>
		///
		/// <param name="readProgress">
		/// An event handler for Read operations.
		/// </param>
		///
		/// <returns>an instance of ZipFile</returns>
		private static ZipFile Read(Stream zipStream, TextWriter statusMessageWriter, Encoding encoding, EventHandler<ReadProgressEventArgs> readProgress)
		{
			if (zipStream == null)
			{
				throw new ArgumentNullException("zipStream");
			}
			ZipFile zipFile = new ZipFile();
			zipFile._StatusMessageTextWriter = statusMessageWriter;
			zipFile._alternateEncoding = encoding ?? DefaultEncoding;
			zipFile._alternateEncodingUsage = ZipOption.Always;
			if (readProgress != null)
			{
				zipFile.ReadProgress += readProgress;
			}
			zipFile._readstream = ((zipStream.Position == 0L) ? zipStream : new OffsetStream(zipStream));
			zipFile._ReadStreamIsOurs = false;
			if (zipFile.Verbose)
			{
				zipFile._StatusMessageTextWriter.WriteLine("reading from stream...");
			}
			ReadIntoInstance(zipFile);
			return zipFile;
		}

		private static void ReadIntoInstance(ZipFile zf)
		{
			Stream readStream = zf.ReadStream;
			try
			{
				zf._readName = zf._name;
				if (!readStream.CanSeek)
				{
					ReadIntoInstance_Orig(zf);
					return;
				}
				zf.OnReadStarted();
				if (ReadFirstFourBytes(readStream) == 101010256)
				{
					return;
				}
				int num = 0;
				bool flag = false;
				long num2 = readStream.Length - 64;
				long num3 = Math.Max(readStream.Length - 16384, 10L);
				do
				{
					if (num2 < 0)
					{
						num2 = 0L;
					}
					readStream.Seek(num2, SeekOrigin.Begin);
					if (SharedUtilities.FindSignature(readStream, 101010256) != -1)
					{
						flag = true;
						continue;
					}
					if (num2 == 0L)
					{
						break;
					}
					num++;
					num2 -= 32 * (num + 1) * num;
				}
				while (!flag && num2 > num3);
				if (flag)
				{
					zf._locEndOfCDS = readStream.Position - 4;
					byte[] array = new byte[16];
					readStream.Read(array, 0, array.Length);
					zf._diskNumberWithCd = BitConverter.ToUInt16(array, 2);
					if (zf._diskNumberWithCd == 65535)
					{
						throw new ZipException("Spanned archives with more than 65534 segments are not supported at this time.");
					}
					zf._diskNumberWithCd++;
					int startIndex = 12;
					uint num4 = BitConverter.ToUInt32(array, startIndex);
					if (num4 == uint.MaxValue)
					{
						Zip64SeekToCentralDirectory(zf);
					}
					else
					{
						zf._OffsetOfCentralDirectory = num4;
						readStream.Seek(num4, SeekOrigin.Begin);
					}
					ReadCentralDirectory(zf);
				}
				else
				{
					readStream.Seek(0L, SeekOrigin.Begin);
					ReadIntoInstance_Orig(zf);
				}
			}
			catch (Exception innerException)
			{
				if (zf._ReadStreamIsOurs && zf._readstream != null)
				{
					zf._readstream.Dispose();
					zf._readstream = null;
				}
				throw new ZipException("Cannot read that as a ZipFile", innerException);
			}
			zf._contentsChanged = false;
		}

		private static void Zip64SeekToCentralDirectory(ZipFile zf)
		{
			Stream readStream = zf.ReadStream;
			byte[] array = new byte[16];
			readStream.Seek(-40L, SeekOrigin.Current);
			readStream.Read(array, 0, 16);
			long num = BitConverter.ToInt64(array, 8);
			zf._OffsetOfCentralDirectory = uint.MaxValue;
			zf._OffsetOfCentralDirectory64 = num;
			readStream.Seek(num, SeekOrigin.Begin);
			uint num2 = (uint)SharedUtilities.ReadInt(readStream);
			if (num2 != 101075792)
			{
				throw new BadReadException($"  Bad signature (0x{num2:X8}) looking for ZIP64 EoCD Record at position 0x{readStream.Position:X8}");
			}
			readStream.Read(array, 0, 8);
			array = new byte[BitConverter.ToInt64(array, 0)];
			readStream.Read(array, 0, array.Length);
			num = BitConverter.ToInt64(array, 36);
			readStream.Seek(num, SeekOrigin.Begin);
		}

		private static uint ReadFirstFourBytes(Stream s)
		{
			return (uint)SharedUtilities.ReadInt(s);
		}

		private static void ReadCentralDirectory(ZipFile zf)
		{
			bool flag = false;
			Dictionary<string, object> dictionary = new Dictionary<string, object>(StringComparer.Ordinal);
			ZipEntry zipEntry;
			while ((zipEntry = ZipEntry.ReadDirEntry(zf, dictionary)) != null)
			{
				zipEntry.ResetDirEntry();
				zf.OnReadEntry(true, null);
				if (zf.Verbose)
				{
					zf.StatusMessageTextWriter.WriteLine("entry {0}", zipEntry.FileName);
				}
				zf._entries.Add(zipEntry.FileName, zipEntry);
				if (!zf._entriesInsensitive.ContainsKey(zipEntry.FileName))
				{
					zf._entriesInsensitive.Add(zipEntry.FileName, zipEntry);
				}
				if (zipEntry._InputUsesZip64)
				{
					flag = true;
				}
				dictionary.Add(zipEntry.FileName, null);
			}
			if (flag)
			{
				zf.UseZip64WhenSaving = Zip64Option.Always;
			}
			if (zf._locEndOfCDS > 0)
			{
				zf.ReadStream.Seek(zf._locEndOfCDS, SeekOrigin.Begin);
			}
			ReadCentralDirectoryFooter(zf);
			if (zf.Verbose && !string.IsNullOrEmpty(zf.Comment))
			{
				zf.StatusMessageTextWriter.WriteLine("Zip file Comment: {0}", zf.Comment);
			}
			if (zf.Verbose)
			{
				zf.StatusMessageTextWriter.WriteLine("read in {0} entries.", zf._entries.Count);
			}
			zf.OnReadCompleted();
		}

		private static void ReadIntoInstance_Orig(ZipFile zf)
		{
			zf.OnReadStarted();
			zf._entries.Clear();
			zf._entriesInsensitive.Clear();
			if (zf.Verbose)
			{
				if (zf.Name == null)
				{
					zf.StatusMessageTextWriter.WriteLine("Reading zip from stream...");
				}
				else
				{
					zf.StatusMessageTextWriter.WriteLine("Reading zip {0}...", zf.Name);
				}
			}
			bool first = true;
			ZipContainer zc = new ZipContainer(zf);
			ZipEntry zipEntry;
			while ((zipEntry = ZipEntry.ReadEntry(zc, first)) != null)
			{
				if (zf.Verbose)
				{
					zf.StatusMessageTextWriter.WriteLine("  {0}", zipEntry.FileName);
				}
				zf._entries.Add(zipEntry.FileName, zipEntry);
				if (!zf._entriesInsensitive.ContainsKey(zipEntry.FileName))
				{
					zf._entriesInsensitive.Add(zipEntry.FileName, zipEntry);
				}
				first = false;
			}
			try
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>(StringComparer.Ordinal);
				ZipEntry zipEntry2;
				while ((zipEntry2 = ZipEntry.ReadDirEntry(zf, dictionary)) != null)
				{
					ZipEntry zipEntry3 = zf._entries[zipEntry2.FileName];
					if (zipEntry3 != null)
					{
						zipEntry3._Comment = zipEntry2.Comment;
						if (zipEntry2.IsDirectory)
						{
							zipEntry3.MarkAsDirectory();
						}
					}
					dictionary.Add(zipEntry2.FileName, null);
				}
				if (zf._locEndOfCDS > 0)
				{
					zf.ReadStream.Seek(zf._locEndOfCDS, SeekOrigin.Begin);
				}
				ReadCentralDirectoryFooter(zf);
				if (zf.Verbose && !string.IsNullOrEmpty(zf.Comment))
				{
					zf.StatusMessageTextWriter.WriteLine("Zip file Comment: {0}", zf.Comment);
				}
			}
			catch (ZipException)
			{
			}
			catch (IOException)
			{
			}
			zf.OnReadCompleted();
		}

		private static void ReadCentralDirectoryFooter(ZipFile zf)
		{
			Stream readStream = zf.ReadStream;
			int num = SharedUtilities.ReadSignature(readStream);
			byte[] array = null;
			int num2 = 0;
			if ((long)num == 101075792)
			{
				array = new byte[52];
				readStream.Read(array, 0, array.Length);
				long num3 = BitConverter.ToInt64(array, 0);
				if (num3 < 44)
				{
					throw new ZipException("Bad size in the ZIP64 Central Directory.");
				}
				zf._versionMadeBy = BitConverter.ToUInt16(array, num2);
				num2 += 2;
				zf._versionNeededToExtract = BitConverter.ToUInt16(array, num2);
				num2 += 2;
				zf._diskNumberWithCd = BitConverter.ToUInt32(array, num2);
				num2 += 2;
				array = new byte[num3 - 44];
				readStream.Read(array, 0, array.Length);
				num = SharedUtilities.ReadSignature(readStream);
				if ((long)num != 117853008)
				{
					throw new ZipException("Inconsistent metadata in the ZIP64 Central Directory.");
				}
				array = new byte[16];
				readStream.Read(array, 0, array.Length);
				num = SharedUtilities.ReadSignature(readStream);
			}
			if ((long)num != 101010256)
			{
				readStream.Seek(-4L, SeekOrigin.Current);
				throw new BadReadException($"Bad signature ({num:X8}) at position 0x{readStream.Position:X8}");
			}
			array = new byte[16];
			zf.ReadStream.Read(array, 0, array.Length);
			if (zf._diskNumberWithCd == 0)
			{
				zf._diskNumberWithCd = BitConverter.ToUInt16(array, 2);
			}
			ReadZipFileComment(zf);
		}

		private static void ReadZipFileComment(ZipFile zf)
		{
			byte[] array = new byte[2];
			zf.ReadStream.Read(array, 0, array.Length);
			short num = (short)(array[0] + array[1] * 256);
			if (num > 0)
			{
				array = new byte[num];
				zf.ReadStream.Read(array, 0, array.Length);
				string @string = zf.AlternateEncoding.GetString(array, 0, array.Length);
				zf.Comment = @string;
			}
		}

		/// <summary>
		/// Checks the given file to see if it appears to be a valid zip file.
		/// </summary>
		/// <remarks>
		///
		/// <para>
		///   Calling this method is equivalent to calling <see cref="M:Ionic.Zip.ZipFile.IsZipFile(System.String,System.Boolean)" /> with the testExtract parameter set to false.
		/// </para>
		/// </remarks>
		///
		/// <param name="fileName">The file to check.</param>
		/// <returns>true if the file appears to be a zip file.</returns>
		public static bool IsZipFile(string fileName)
		{
			return IsZipFile(fileName, false);
		}

		/// <summary>
		/// Checks a file to see if it is a valid zip file.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This method opens the specified zip file, reads in the zip archive,
		///   verifying the ZIP metadata as it reads.
		/// </para>
		///
		/// <para>
		///   If everything succeeds, then the method returns true.  If anything fails -
		///   for example if an incorrect signature or CRC is found, indicating a
		///   corrupt file, the the method returns false.  This method also returns
		///   false for a file that does not exist.
		/// </para>
		///
		/// <para>
		///   If <paramref name="testExtract" /> is true, as part of its check, this
		///   method reads in the content for each entry, expands it, and checks CRCs.
		///   This provides an additional check beyond verifying the zip header and
		///   directory data.
		/// </para>
		///
		/// <para>
		///   If <paramref name="testExtract" /> is true, and if any of the zip entries
		///   are protected with a password, this method will return false.  If you want
		///   to verify a <c>ZipFile</c> that has entries which are protected with a
		///   password, you will need to do that manually.
		/// </para>
		///
		/// </remarks>
		///
		/// <param name="fileName">The zip file to check.</param>
		/// <param name="testExtract">true if the caller wants to extract each entry.</param>
		/// <returns>true if the file contains a valid zip file.</returns>
		public static bool IsZipFile(string fileName, bool testExtract)
		{
			bool result = false;
			try
			{
				if (!File.Exists(fileName))
				{
					return false;
				}
				using (FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					result = IsZipFile(stream, testExtract);
					return result;
				}
			}
			catch (IOException)
			{
				return result;
			}
			catch (ZipException)
			{
				return result;
			}
		}

		/// <summary>
		/// Checks a stream to see if it contains a valid zip archive.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		/// This method reads the zip archive contained in the specified stream, verifying
		/// the ZIP metadata as it reads.  If testExtract is true, this method also extracts
		/// each entry in the archive, dumping all the bits into <see cref="F:System.IO.Stream.Null" />.
		/// </para>
		///
		/// <para>
		/// If everything succeeds, then the method returns true.  If anything fails -
		/// for example if an incorrect signature or CRC is found, indicating a corrupt
		/// file, the the method returns false.  This method also returns false for a
		/// file that does not exist.
		/// </para>
		///
		/// <para>
		/// If <c>testExtract</c> is true, this method reads in the content for each
		/// entry, expands it, and checks CRCs.  This provides an additional check
		/// beyond verifying the zip header data.
		/// </para>
		///
		/// <para>
		/// If <c>testExtract</c> is true, and if any of the zip entries are protected
		/// with a password, this method will return false.  If you want to verify a
		/// ZipFile that has entries which are protected with a password, you will need
		/// to do that manually.
		/// </para>
		/// </remarks>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.IsZipFile(System.String,System.Boolean)" />
		///
		/// <param name="stream">The stream to check.</param>
		/// <param name="testExtract">true if the caller wants to extract each entry.</param>
		/// <returns>true if the stream contains a valid zip archive.</returns>
		public static bool IsZipFile(Stream stream, bool testExtract)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			bool result = false;
			try
			{
				if (!stream.CanRead)
				{
					return false;
				}
				Stream @null = Stream.Null;
				using (ZipFile zipFile = Read(stream, null, null, null))
				{
					if (testExtract)
					{
						foreach (ZipEntry item in zipFile)
						{
							if (!item.IsDirectory)
							{
								item.Extract(@null);
							}
						}
					}
				}
				result = true;
				return result;
			}
			catch (IOException)
			{
				return result;
			}
			catch (ZipException)
			{
				return result;
			}
		}

		/// <summary>
		///   Delete file with retry on UnauthorizedAccessException.
		/// </summary>
		///
		/// <remarks>
		///   <para>
		///     When calling File.Delete() on a file that has been "recently"
		///     created, the call sometimes fails with
		///     UnauthorizedAccessException. This method simply retries the Delete 3
		///     times with a sleep between tries.
		///   </para>
		/// </remarks>
		///
		/// <param name="filename">the name of the file to be deleted</param>
		private void DeleteFileWithRetry(string filename)
		{
			bool flag = false;
			int num = 3;
			for (int i = 0; i < num; i++)
			{
				if (flag)
				{
					break;
				}
				try
				{
					File.Delete(filename);
					flag = true;
				}
				catch (UnauthorizedAccessException)
				{
					Console.WriteLine("************************************************** Retry delete.");
					Thread.Sleep(200 + i * 200);
				}
			}
		}

		/// <summary>
		///   Saves the Zip archive to a file, specified by the Name property of the
		///   <c>ZipFile</c>.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   The <c>ZipFile</c> instance is written to storage, typically a zip file
		///   in a filesystem, only when the caller calls <c>Save</c>.  In the typical
		///   case, the Save operation writes the zip content to a temporary file, and
		///   then renames the temporary file to the desired name. If necessary, this
		///   method will delete a pre-existing file before the rename.
		/// </para>
		///
		/// <para>
		///   The <see cref="P:Ionic.Zip.ZipFile.Name" /> property is specified either explicitly,
		///   or implicitly using one of the parameterized ZipFile constructors.  For
		///   COM Automation clients, the <c>Name</c> property must be set explicitly,
		///   because COM Automation clients cannot call parameterized constructors.
		/// </para>
		///
		/// <para>
		///   When using a filesystem file for the Zip output, it is possible to call
		///   <c>Save</c> multiple times on the <c>ZipFile</c> instance. With each
		///   call the zip content is re-written to the same output file.
		/// </para>
		///
		/// <para>
		///   Data for entries that have been added to the <c>ZipFile</c> instance is
		///   written to the output when the <c>Save</c> method is called. This means
		///   that the input streams for those entries must be available at the time
		///   the application calls <c>Save</c>.  If, for example, the application
		///   adds entries with <c>AddEntry</c> using a dynamically-allocated
		///   <c>MemoryStream</c>, the memory stream must not have been disposed
		///   before the call to <c>Save</c>. See the <see cref="P:Ionic.Zip.ZipEntry.InputStream" /> property for more discussion of the
		///   availability requirements of the input stream for an entry, and an
		///   approach for providing just-in-time stream lifecycle management.
		/// </para>
		///
		/// </remarks>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddEntry(System.String,System.IO.Stream)" />
		///
		/// <exception cref="T:Ionic.Zip.BadStateException">
		///   Thrown if you haven't specified a location or stream for saving the zip,
		///   either in the constructor or by setting the Name property, or if you try
		///   to save a regular zip archive to a filename with a .exe extension.
		/// </exception>
		///
		/// <exception cref="T:System.OverflowException">
		///   Thrown if <see cref="P:Ionic.Zip.ZipFile.MaxOutputSegmentSize" /> or <see cref="P:Ionic.Zip.ZipFile.MaxOutputSegmentSize64" /> is non-zero, and the number
		///   of segments that would be generated for the spanned zip file during the
		///   save operation exceeds 99.  If this happens, you need to increase the
		///   segment size.
		/// </exception>
		public void Save()
		{
			try
			{
				bool flag = false;
				_saveOperationCanceled = false;
				_numberOfSegmentsForMostRecentSave = 0u;
				OnSaveStarted();
				if (WriteStream == null)
				{
					throw new BadStateException("You haven't specified where to save the zip.");
				}
				if (_name != null && _name.EndsWith(".exe") && !_SavingSfx)
				{
					throw new BadStateException("You specified an EXE for a plain zip file.");
				}
				if (!_contentsChanged)
				{
					OnSaveCompleted();
					if (Verbose)
					{
						StatusMessageTextWriter.WriteLine("No save is necessary....");
					}
					return;
				}
				Reset(true);
				if (Verbose)
				{
					StatusMessageTextWriter.WriteLine("saving....");
				}
				if (_entries.Count >= 65535 && _zip64 == Zip64Option.Default)
				{
					throw new ZipException("The number of entries is 65535 or greater. Consider setting the UseZip64WhenSaving property on the ZipFile instance.");
				}
				int num = 0;
				ICollection<ZipEntry> collection = (SortEntriesBeforeSaving ? EntriesSorted : Entries);
				foreach (ZipEntry item in collection)
				{
					OnSaveEntry(num, item, true);
					item.Write(WriteStream);
					if (_saveOperationCanceled)
					{
						break;
					}
					num++;
					OnSaveEntry(num, item, false);
					if (_saveOperationCanceled)
					{
						break;
					}
					if (item.IncludedInMostRecentSave)
					{
						flag |= item.OutputUsedZip64.Value;
					}
				}
				if (_saveOperationCanceled)
				{
					return;
				}
				ZipSegmentedStream zipSegmentedStream = WriteStream as ZipSegmentedStream;
				_numberOfSegmentsForMostRecentSave = zipSegmentedStream?.CurrentSegment ?? 1;
				bool flag2 = ZipOutput.WriteCentralDirectoryStructure(WriteStream, collection, _numberOfSegmentsForMostRecentSave, _zip64, Comment, new ZipContainer(this));
				OnSaveEvent(ZipProgressEventType.Saving_AfterSaveTempArchive);
				_hasBeenSaved = true;
				_contentsChanged = false;
				flag = flag || flag2;
				_OutputUsesZip64 = flag;
				if (_fileAlreadyExists && _readstream != null)
				{
					_readstream.Close();
					_readstream = null;
				}
				foreach (ZipEntry item2 in collection)
				{
					if (item2._archiveStream is ZipSegmentedStream zipSegmentedStream2)
					{
						zipSegmentedStream2.Dispose();
					}
					item2._archiveStream = null;
				}
				if (_name != null && (_temporaryFileName != null || zipSegmentedStream != null))
				{
					WriteStream.Dispose();
					if (_saveOperationCanceled)
					{
						return;
					}
					string text = null;
					if (File.Exists(_name))
					{
						text = _name + "." + Path.GetRandomFileName();
						if (File.Exists(text))
						{
							DeleteFileWithRetry(text);
						}
						File.Move(_name, text);
					}
					OnSaveEvent(ZipProgressEventType.Saving_BeforeRenameTempArchive);
					File.Move((zipSegmentedStream != null) ? zipSegmentedStream.CurrentTempName : _temporaryFileName, _name);
					OnSaveEvent(ZipProgressEventType.Saving_AfterRenameTempArchive);
					if (text != null)
					{
						try
						{
							if (File.Exists(text))
							{
								File.Delete(text);
							}
						}
						catch
						{
						}
					}
					_fileAlreadyExists = true;
				}
				_readName = _name;
				NotifyEntriesSaveComplete(collection);
				OnSaveCompleted();
				_JustSaved = true;
			}
			finally
			{
				CleanupAfterSaveOperation();
			}
		}

		private static void NotifyEntriesSaveComplete(ICollection<ZipEntry> c)
		{
			foreach (ZipEntry item in c)
			{
				item.NotifySaveComplete();
			}
		}

		private void RemoveTempFile()
		{
			try
			{
				if (File.Exists(_temporaryFileName))
				{
					File.Delete(_temporaryFileName);
				}
			}
			catch (IOException ex)
			{
				if (Verbose)
				{
					StatusMessageTextWriter.WriteLine("ZipFile::Save: could not delete temp file: {0}.", ex.Message);
				}
			}
		}

		private void CleanupAfterSaveOperation()
		{
			if (_name == null)
			{
				return;
			}
			if (_writestream != null)
			{
				try
				{
					_writestream.Dispose();
				}
				catch (IOException)
				{
				}
			}
			_writestream = null;
			if (_temporaryFileName != null)
			{
				RemoveTempFile();
				_temporaryFileName = null;
			}
		}

		/// <summary>
		/// Save the file to a new zipfile, with the given name.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		/// This method allows the application to explicitly specify the name of the zip
		/// file when saving. Use this when creating a new zip file, or when
		/// updating a zip archive.
		/// </para>
		///
		/// <para>
		/// An application can also save a zip archive in several places by calling this
		/// method multiple times in succession, with different filenames.
		/// </para>
		///
		/// <para>
		/// The <c>ZipFile</c> instance is written to storage, typically a zip file in a
		/// filesystem, only when the caller calls <c>Save</c>.  The Save operation writes
		/// the zip content to a temporary file, and then renames the temporary file
		/// to the desired name. If necessary, this method will delete a pre-existing file
		/// before the rename.
		/// </para>
		///
		/// </remarks>
		///
		/// <exception cref="T:System.ArgumentException">
		/// Thrown if you specify a directory for the filename.
		/// </exception>
		///
		/// <param name="fileName">
		/// The name of the zip archive to save to. Existing files will
		/// be overwritten with great prejudice.
		/// </param>
		///
		/// <example>
		/// This example shows how to create and Save a zip file.
		/// <code>
		/// using (ZipFile zip = new ZipFile())
		/// {
		///   zip.AddDirectory(@"c:\reports\January");
		///   zip.Save("January.zip");
		/// }
		/// </code>
		///
		/// <code lang="VB">
		/// Using zip As New ZipFile()
		///   zip.AddDirectory("c:\reports\January")
		///   zip.Save("January.zip")
		/// End Using
		/// </code>
		///
		/// </example>
		///
		/// <example>
		/// This example shows how to update a zip file.
		/// <code>
		/// using (ZipFile zip = ZipFile.Read("ExistingArchive.zip"))
		/// {
		///   zip.AddFile("NewData.csv");
		///   zip.Save("UpdatedArchive.zip");
		/// }
		/// </code>
		///
		/// <code lang="VB">
		/// Using zip As ZipFile = ZipFile.Read("ExistingArchive.zip")
		///   zip.AddFile("NewData.csv")
		///   zip.Save("UpdatedArchive.zip")
		/// End Using
		/// </code>
		///
		/// </example>
		public void Save(string fileName)
		{
			if (_name == null)
			{
				_writestream = null;
			}
			else
			{
				_readName = _name;
			}
			_name = fileName;
			if (Directory.Exists(_name))
			{
				throw new ZipException("Bad Directory", new ArgumentException("That name specifies an existing directory. Please specify a filename.", "fileName"));
			}
			_contentsChanged = true;
			_fileAlreadyExists = File.Exists(_readName);
			Save();
		}

		/// <summary>
		///   Save the zip archive to the specified stream.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   The <c>ZipFile</c> instance is written to storage - typically a zip file
		///   in a filesystem, but using this overload, the storage can be anything
		///   accessible via a writable stream - only when the caller calls <c>Save</c>.
		/// </para>
		///
		/// <para>
		///   Use this method to save the zip content to a stream directly.  A common
		///   scenario is an ASP.NET application that dynamically generates a zip file
		///   and allows the browser to download it. The application can call
		///   <c>Save(Response.OutputStream)</c> to write a zipfile directly to the
		///   output stream, without creating a zip file on the disk on the ASP.NET
		///   server.
		/// </para>
		///
		/// <para>
		///   Be careful when saving a file to a non-seekable stream, including
		///   <c>Response.OutputStream</c>. When DotNetZip writes to a non-seekable
		///   stream, the zip archive is formatted in such a way that may not be
		///   compatible with all zip tools on all platforms.  It's a perfectly legal
		///   and compliant zip file, but some people have reported problems opening
		///   files produced this way using the Mac OS archive utility.
		/// </para>
		///
		/// </remarks>
		///
		/// <example>
		///
		///   This example saves the zipfile content into a MemoryStream, and
		///   then gets the array of bytes from that MemoryStream.
		///
		/// <code lang="C#">
		/// using (var zip = new Ionic.Zip.ZipFile())
		/// {
		///     zip.CompressionLevel= Ionic.Zlib.CompressionLevel.BestCompression;
		///     zip.Password = "VerySecret.";
		///     zip.Encryption = EncryptionAlgorithm.WinZipAes128;
		///     zip.AddFile(sourceFileName);
		///     MemoryStream output = new MemoryStream();
		///     zip.Save(output);
		///
		///     byte[] zipbytes = output.ToArray();
		/// }
		/// </code>
		/// </example>
		///
		/// <example>
		/// <para>
		///   This example shows a pitfall you should avoid. DO NOT read
		///   from a stream, then try to save to the same stream.  DO
		///   NOT DO THIS:
		/// </para>
		///
		/// <code lang="C#">
		/// using (var fs = new FileStream(filename, FileMode.Open))
		/// {
		///   using (var zip = Ionic.Zip.ZipFile.Read(inputStream))
		///   {
		///     zip.AddEntry("Name1.txt", "this is the content");
		///     zip.Save(inputStream);  // NO NO NO!!
		///   }
		/// }
		/// </code>
		///
		/// <para>
		///   Better like this:
		/// </para>
		///
		/// <code lang="C#">
		/// using (var zip = Ionic.Zip.ZipFile.Read(filename))
		/// {
		///     zip.AddEntry("Name1.txt", "this is the content");
		///     zip.Save();  // YES!
		/// }
		/// </code>
		///
		/// </example>
		///
		/// <param name="outputStream">
		///   The <c>System.IO.Stream</c> to write to. It must be
		///   writable. If you created the ZipFile instance by calling
		///   ZipFile.Read(), this stream must not be the same stream
		///   you passed to ZipFile.Read().
		/// </param>
		public void Save(Stream outputStream)
		{
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			if (!outputStream.CanWrite)
			{
				throw new ArgumentException("Must be a writable stream.", "outputStream");
			}
			_name = null;
			if (_writestream != null)
			{
				_readstream = _writestream;
			}
			_writestream = new CountingStream(outputStream);
			_contentsChanged = true;
			_fileAlreadyExists = File.Exists(_readName);
			Save();
			_fileAlreadyExists = false;
			_readName = null;
		}

		/// <summary>
		///   Adds to the ZipFile a set of files from the current working directory on
		///   disk, that conform to the specified criteria.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This method selects files from the the current working directory matching
		///   the specified criteria, and adds them to the ZipFile.
		/// </para>
		///
		/// <para>
		///   Specify the criteria in statements of 3 elements: a noun, an operator, and
		///   a value.  Consider the string "name != *.doc" .  The noun is "name".  The
		///   operator is "!=", implying "Not Equal".  The value is "*.doc".  That
		///   criterion, in English, says "all files with a name that does not end in
		///   the .doc extension."
		/// </para>
		///
		/// <para>
		///   Supported nouns include "name" (or "filename") for the filename; "atime",
		///   "mtime", and "ctime" for last access time, last modfied time, and created
		///   time of the file, respectively; "attributes" (or "attrs") for the file
		///   attributes; "size" (or "length") for the file length (uncompressed), and
		///   "type" for the type of object, either a file or a directory.  The
		///   "attributes", "name" and "type" nouns both support = and != as operators.
		///   The "size", "atime", "mtime", and "ctime" nouns support = and !=, and
		///   &gt;, &gt;=, &lt;, &lt;= as well. The times are taken to be expressed in
		///   local time.
		/// </para>
		///
		/// <para>
		/// Specify values for the file attributes as a string with one or more of the
		/// characters H,R,S,A,I,L in any order, implying file attributes of Hidden,
		/// ReadOnly, System, Archive, NotContextIndexed, and ReparsePoint (symbolic
		/// link) respectively.
		/// </para>
		///
		/// <para>
		/// To specify a time, use YYYY-MM-DD-HH:mm:ss or YYYY/MM/DD-HH:mm:ss as the
		/// format.  If you omit the HH:mm:ss portion, it is assumed to be 00:00:00
		/// (midnight).
		/// </para>
		///
		/// <para>
		/// The value for a size criterion is expressed in integer quantities of bytes,
		/// kilobytes (use k or kb after the number), megabytes (m or mb), or gigabytes
		/// (g or gb).
		/// </para>
		///
		/// <para>
		/// The value for a name is a pattern to match against the filename, potentially
		/// including wildcards.  The pattern follows CMD.exe glob rules: * implies one
		/// or more of any character, while ?  implies one character.  If the name
		/// pattern contains any slashes, it is matched to the entire filename,
		/// including the path; otherwise, it is matched against only the filename
		/// without the path.  This means a pattern of "*\*.*" matches all files one
		/// directory level deep, while a pattern of "*.*" matches all files in all
		/// directories.
		/// </para>
		///
		/// <para>
		/// To specify a name pattern that includes spaces, use single quotes around the
		/// pattern.  A pattern of "'* *.*'" will match all files that have spaces in
		/// the filename.  The full criteria string for that would be "name = '* *.*'" .
		/// </para>
		///
		/// <para>
		/// The value for a type criterion is either F (implying a file) or D (implying
		/// a directory).
		/// </para>
		///
		/// <para>
		/// Some examples:
		/// </para>
		///
		/// <list type="table">
		///   <listheader>
		///     <term>criteria</term>
		///     <description>Files retrieved</description>
		///   </listheader>
		///
		///   <item>
		///     <term>name != *.xls </term>
		///     <description>any file with an extension that is not .xls
		///     </description>
		///   </item>
		///
		///   <item>
		///     <term>name = *.mp3 </term>
		///     <description>any file with a .mp3 extension.
		///     </description>
		///   </item>
		///
		///   <item>
		///     <term>*.mp3</term>
		///     <description>(same as above) any file with a .mp3 extension.
		///     </description>
		///   </item>
		///
		///   <item>
		///     <term>attributes = A </term>
		///     <description>all files whose attributes include the Archive bit.
		///     </description>
		///   </item>
		///
		///   <item>
		///     <term>attributes != H </term>
		///     <description>all files whose attributes do not include the Hidden bit.
		///     </description>
		///   </item>
		///
		///   <item>
		///     <term>mtime &gt; 2009-01-01</term>
		///     <description>all files with a last modified time after January 1st, 2009.
		///     </description>
		///   </item>
		///
		///   <item>
		///     <term>size &gt; 2gb</term>
		///     <description>all files whose uncompressed size is greater than 2gb.
		///     </description>
		///   </item>
		///
		///   <item>
		///     <term>type = D</term>
		///     <description>all directories in the filesystem. </description>
		///   </item>
		///
		/// </list>
		///
		/// <para>
		/// You can combine criteria with the conjunctions AND or OR. Using a string
		/// like "name = *.txt AND size &gt;= 100k" for the selectionCriteria retrieves
		/// entries whose names end in .txt, and whose uncompressed size is greater than
		/// or equal to 100 kilobytes.
		/// </para>
		///
		/// <para>
		/// For more complex combinations of criteria, you can use parenthesis to group
		/// clauses in the boolean logic.  Without parenthesis, the precedence of the
		/// criterion atoms is determined by order of appearance.  Unlike the C#
		/// language, the AND conjunction does not take precendence over the logical OR.
		/// This is important only in strings that contain 3 or more criterion atoms.
		/// In other words, "name = *.txt and size &gt; 1000 or attributes = H" implies
		/// "((name = *.txt AND size &gt; 1000) OR attributes = H)" while "attributes =
		/// H OR name = *.txt and size &gt; 1000" evaluates to "((attributes = H OR name
		/// = *.txt) AND size &gt; 1000)".  When in doubt, use parenthesis.
		/// </para>
		///
		/// <para>
		/// Using time properties requires some extra care. If you want to retrieve all
		/// entries that were last updated on 2009 February 14, specify a time range
		/// like so:"mtime &gt;= 2009-02-14 AND mtime &lt; 2009-02-15".  Read this to
		/// say: all files updated after 12:00am on February 14th, until 12:00am on
		/// February 15th.  You can use the same bracketing approach to specify any time
		/// period - a year, a month, a week, and so on.
		/// </para>
		///
		/// <para>
		/// The syntax allows one special case: if you provide a string with no spaces, it is
		/// treated as a pattern to match for the filename.  Therefore a string like "*.xls"
		/// will be equivalent to specifying "name = *.xls".
		/// </para>
		///
		/// <para>
		/// There is no logic in this method that insures that the file inclusion
		/// criteria are internally consistent.  For example, it's possible to specify
		/// criteria that says the file must have a size of less than 100 bytes, as well
		/// as a size that is greater than 1000 bytes. Obviously no file will ever
		/// satisfy such criteria, but this method does not detect such logical
		/// inconsistencies. The caller is responsible for insuring the criteria are
		/// sensible.
		/// </para>
		///
		/// <para>
		///   Using this method, the file selection does not recurse into
		///   subdirectories, and the full path of the selected files is included in the
		///   entries added into the zip archive.  If you don't like these behaviors,
		///   see the other overloads of this method.
		/// </para>
		/// </remarks>
		///
		/// <example>
		/// This example zips up all *.csv files in the current working directory.
		/// <code>
		/// using (ZipFile zip = new ZipFile())
		/// {
		///     // To just match on filename wildcards,
		///     // use the shorthand form of the selectionCriteria string.
		///     zip.AddSelectedFiles("*.csv");
		///     zip.Save(PathToZipArchive);
		/// }
		/// </code>
		/// <code lang="VB">
		/// Using zip As ZipFile = New ZipFile()
		///     zip.AddSelectedFiles("*.csv")
		///     zip.Save(PathToZipArchive)
		/// End Using
		/// </code>
		/// </example>
		///
		/// <param name="selectionCriteria">The criteria for file selection</param>
		public void AddSelectedFiles(string selectionCriteria)
		{
			AddSelectedFiles(selectionCriteria, ".", null, false);
		}

		/// <summary>
		///   Adds to the ZipFile a set of files from the disk that conform to the
		///   specified criteria, optionally recursing into subdirectories.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This method selects files from the the current working directory matching
		///   the specified criteria, and adds them to the ZipFile.  If
		///   <c>recurseDirectories</c> is true, files are also selected from
		///   subdirectories, and the directory structure in the filesystem is
		///   reproduced in the zip archive, rooted at the current working directory.
		/// </para>
		///
		/// <para>
		///   Using this method, the full path of the selected files is included in the
		///   entries added into the zip archive.  If you don't want this behavior, use
		///   one of the overloads of this method that allows the specification of a
		///   <c>directoryInArchive</c>.
		/// </para>
		///
		/// <para>
		///   For details on the syntax for the selectionCriteria parameter, see <see cref="M:Ionic.Zip.ZipFile.AddSelectedFiles(System.String)" />.
		/// </para>
		///
		/// </remarks>
		///
		/// <example>
		///
		///   This example zips up all *.xml files in the current working directory, or any
		///   subdirectory, that are larger than 1mb.
		///
		/// <code>
		/// using (ZipFile zip = new ZipFile())
		/// {
		///     // Use a compound expression in the selectionCriteria string.
		///     zip.AddSelectedFiles("name = *.xml  and  size &gt; 1024kb", true);
		///     zip.Save(PathToZipArchive);
		/// }
		/// </code>
		/// <code lang="VB">
		/// Using zip As ZipFile = New ZipFile()
		///     ' Use a compound expression in the selectionCriteria string.
		///     zip.AddSelectedFiles("name = *.xml  and  size &gt; 1024kb", true)
		///     zip.Save(PathToZipArchive)
		/// End Using
		/// </code>
		/// </example>
		///
		/// <param name="selectionCriteria">The criteria for file selection</param>
		///
		/// <param name="recurseDirectories">
		///   If true, the file selection will recurse into subdirectories.
		/// </param>
		public void AddSelectedFiles(string selectionCriteria, bool recurseDirectories)
		{
			AddSelectedFiles(selectionCriteria, ".", null, recurseDirectories);
		}

		/// <summary>
		///   Adds to the ZipFile a set of files from a specified directory in the
		///   filesystem, that conform to the specified criteria.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This method selects files that conform to the specified criteria, from the
		///   the specified directory on disk, and adds them to the ZipFile.  The search
		///   does not recurse into subdirectores.
		/// </para>
		///
		/// <para>
		///   Using this method, the full filesystem path of the files on disk is
		///   reproduced on the entries added to the zip file.  If you don't want this
		///   behavior, use one of the other overloads of this method.
		/// </para>
		///
		/// <para>
		///   For details on the syntax for the selectionCriteria parameter, see <see cref="M:Ionic.Zip.ZipFile.AddSelectedFiles(System.String)" />.
		/// </para>
		///
		/// </remarks>
		///
		/// <example>
		///
		///   This example zips up all *.xml files larger than 1mb in the directory
		///   given by "d:\rawdata".
		///
		/// <code>
		/// using (ZipFile zip = new ZipFile())
		/// {
		///     // Use a compound expression in the selectionCriteria string.
		///     zip.AddSelectedFiles("name = *.xml  and  size &gt; 1024kb", "d:\\rawdata");
		///     zip.Save(PathToZipArchive);
		/// }
		/// </code>
		///
		/// <code lang="VB">
		/// Using zip As ZipFile = New ZipFile()
		///     ' Use a compound expression in the selectionCriteria string.
		///     zip.AddSelectedFiles("name = *.xml  and  size &gt; 1024kb", "d:\rawdata)
		///     zip.Save(PathToZipArchive)
		/// End Using
		/// </code>
		/// </example>
		///
		/// <param name="selectionCriteria">The criteria for file selection</param>
		///
		/// <param name="directoryOnDisk">
		/// The name of the directory on the disk from which to select files.
		/// </param>
		public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk)
		{
			AddSelectedFiles(selectionCriteria, directoryOnDisk, null, false);
		}

		/// <summary>
		///   Adds to the ZipFile a set of files from the specified directory on disk,
		///   that conform to the specified criteria.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   This method selects files from the the specified disk directory matching
		///   the specified selection criteria, and adds them to the ZipFile.  If
		///   <c>recurseDirectories</c> is true, files are also selected from
		///   subdirectories.
		/// </para>
		///
		/// <para>
		///   The full directory structure in the filesystem is reproduced on the
		///   entries added to the zip archive.  If you don't want this behavior, use
		///   one of the overloads of this method that allows the specification of a
		///   <c>directoryInArchive</c>.
		/// </para>
		///
		/// <para>
		///   For details on the syntax for the selectionCriteria parameter, see <see cref="M:Ionic.Zip.ZipFile.AddSelectedFiles(System.String)" />.
		/// </para>
		/// </remarks>
		///
		/// <example>
		///
		///   This example zips up all *.csv files in the "files" directory, or any
		///   subdirectory, that have been saved since 2009 February 14th.
		///
		/// <code>
		/// using (ZipFile zip = new ZipFile())
		/// {
		///     // Use a compound expression in the selectionCriteria string.
		///     zip.AddSelectedFiles("name = *.csv  and  mtime &gt; 2009-02-14", "files", true);
		///     zip.Save(PathToZipArchive);
		/// }
		/// </code>
		/// <code lang="VB">
		/// Using zip As ZipFile = New ZipFile()
		///     ' Use a compound expression in the selectionCriteria string.
		///     zip.AddSelectedFiles("name = *.csv  and  mtime &gt; 2009-02-14", "files", true)
		///     zip.Save(PathToZipArchive)
		/// End Using
		/// </code>
		/// </example>
		///
		/// <example>
		///   This example zips up all files in the current working
		///   directory, and all its child directories, except those in
		///   the <c>excludethis</c> subdirectory.
		/// <code lang="VB">
		/// Using Zip As ZipFile = New ZipFile(zipfile)
		///   Zip.AddSelectedFfiles("name != 'excludethis\*.*'", datapath, True)
		///   Zip.Save()
		/// End Using
		/// </code>
		/// </example>
		///
		/// <param name="selectionCriteria">The criteria for file selection</param>
		///
		/// <param name="directoryOnDisk">
		///   The filesystem path from which to select files.
		/// </param>
		///
		/// <param name="recurseDirectories">
		///   If true, the file selection will recurse into subdirectories.
		/// </param>
		public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk, bool recurseDirectories)
		{
			AddSelectedFiles(selectionCriteria, directoryOnDisk, null, recurseDirectories);
		}

		/// <summary>
		///   Adds to the ZipFile a selection of files from the specified directory on
		///   disk, that conform to the specified criteria, and using a specified root
		///   path for entries added to the zip archive.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This method selects files from the specified disk directory matching the
		///   specified selection criteria, and adds those files to the ZipFile, using
		///   the specified directory path in the archive.  The search does not recurse
		///   into subdirectories.  For details on the syntax for the selectionCriteria
		///   parameter, see <see cref="M:Ionic.Zip.ZipFile.AddSelectedFiles(System.String)" />.
		/// </para>
		///
		/// </remarks>
		///
		/// <example>
		///
		///   This example zips up all *.psd files in the "photos" directory that have
		///   been saved since 2009 February 14th, and puts them all in a zip file,
		///   using the directory name of "content" in the zip archive itself. When the
		///   zip archive is unzipped, the folder containing the .psd files will be
		///   named "content".
		///
		/// <code>
		/// using (ZipFile zip = new ZipFile())
		/// {
		///     // Use a compound expression in the selectionCriteria string.
		///     zip.AddSelectedFiles("name = *.psd  and  mtime &gt; 2009-02-14", "photos", "content");
		///     zip.Save(PathToZipArchive);
		/// }
		/// </code>
		/// <code lang="VB">
		/// Using zip As ZipFile = New ZipFile
		///     zip.AddSelectedFiles("name = *.psd  and  mtime &gt; 2009-02-14", "photos", "content")
		///     zip.Save(PathToZipArchive)
		/// End Using
		/// </code>
		/// </example>
		///
		/// <param name="selectionCriteria">
		///   The criteria for selection of files to add to the <c>ZipFile</c>.
		/// </param>
		///
		/// <param name="directoryOnDisk">
		///   The path to the directory in the filesystem from which to select files.
		/// </param>
		///
		/// <param name="directoryPathInArchive">
		///   Specifies a directory path to use to in place of the
		///   <c>directoryOnDisk</c>.  This path may, or may not, correspond to a real
		///   directory in the current filesystem.  If the files within the zip are
		///   later extracted, this is the path used for the extracted file.  Passing
		///   null (nothing in VB) will use the path on the file name, if any; in other
		///   words it would use <c>directoryOnDisk</c>, plus any subdirectory.  Passing
		///   the empty string ("") will insert the item at the root path within the
		///   archive.
		/// </param>
		public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk, string directoryPathInArchive)
		{
			AddSelectedFiles(selectionCriteria, directoryOnDisk, directoryPathInArchive, false);
		}

		/// <summary>
		///   Adds to the ZipFile a selection of files from the specified directory on
		///   disk, that conform to the specified criteria, optionally recursing through
		///   subdirectories, and using a specified root path for entries added to the
		///   zip archive.
		/// </summary>
		///
		/// <remarks>
		///   This method selects files from the specified disk directory that match the
		///   specified selection criteria, and adds those files to the ZipFile, using
		///   the specified directory path in the archive. If <c>recurseDirectories</c>
		///   is true, files are also selected from subdirectories, and the directory
		///   structure in the filesystem is reproduced in the zip archive, rooted at
		///   the directory specified by <c>directoryOnDisk</c>.  For details on the
		///   syntax for the selectionCriteria parameter, see <see cref="M:Ionic.Zip.ZipFile.AddSelectedFiles(System.String)" />.
		/// </remarks>
		///
		/// <example>
		///
		///   This example zips up all files that are NOT *.pst files, in the current
		///   working directory and any subdirectories.
		///
		/// <code>
		/// using (ZipFile zip = new ZipFile())
		/// {
		///     zip.AddSelectedFiles("name != *.pst", SourceDirectory, "backup", true);
		///     zip.Save(PathToZipArchive);
		/// }
		/// </code>
		/// <code lang="VB">
		/// Using zip As ZipFile = New ZipFile
		///     zip.AddSelectedFiles("name != *.pst", SourceDirectory, "backup", true)
		///     zip.Save(PathToZipArchive)
		/// End Using
		/// </code>
		/// </example>
		///
		/// <param name="selectionCriteria">
		///   The criteria for selection of files to add to the <c>ZipFile</c>.
		/// </param>
		///
		/// <param name="directoryOnDisk">
		///   The path to the directory in the filesystem from which to select files.
		/// </param>
		///
		/// <param name="directoryPathInArchive">
		///   Specifies a directory path to use to in place of the
		///   <c>directoryOnDisk</c>.  This path may, or may not, correspond to a real
		///   directory in the current filesystem.  If the files within the zip are
		///   later extracted, this is the path used for the extracted file.  Passing
		///   null (nothing in VB) will use the path on the file name, if any; in other
		///   words it would use <c>directoryOnDisk</c>, plus any subdirectory.  Passing
		///   the empty string ("") will insert the item at the root path within the
		///   archive.
		/// </param>
		///
		/// <param name="recurseDirectories">
		///   If true, the method also scans subdirectories for files matching the
		///   criteria.
		/// </param>
		public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk, string directoryPathInArchive, bool recurseDirectories)
		{
			_AddOrUpdateSelectedFiles(selectionCriteria, directoryOnDisk, directoryPathInArchive, recurseDirectories, false);
		}

		/// <summary>
		///   Updates the ZipFile with a selection of files from the disk that conform
		///   to the specified criteria.
		/// </summary>
		///
		/// <remarks>
		///   This method selects files from the specified disk directory that match the
		///   specified selection criteria, and Updates the <c>ZipFile</c> with those
		///   files, using the specified directory path in the archive. If
		///   <c>recurseDirectories</c> is true, files are also selected from
		///   subdirectories, and the directory structure in the filesystem is
		///   reproduced in the zip archive, rooted at the directory specified by
		///   <c>directoryOnDisk</c>.  For details on the syntax for the
		///   selectionCriteria parameter, see <see cref="M:Ionic.Zip.ZipFile.AddSelectedFiles(System.String)" />.
		/// </remarks>
		///
		/// <param name="selectionCriteria">
		///   The criteria for selection of files to add to the <c>ZipFile</c>.
		/// </param>
		///
		/// <param name="directoryOnDisk">
		///   The path to the directory in the filesystem from which to select files.
		/// </param>
		///
		/// <param name="directoryPathInArchive">
		///   Specifies a directory path to use to in place of the
		///   <c>directoryOnDisk</c>. This path may, or may not, correspond to a
		///   real directory in the current filesystem. If the files within the zip
		///   are later extracted, this is the path used for the extracted file.
		///   Passing null (nothing in VB) will use the path on the file name, if
		///   any; in other words it would use <c>directoryOnDisk</c>, plus any
		///   subdirectory.  Passing the empty string ("") will insert the item at
		///   the root path within the archive.
		/// </param>
		///
		/// <param name="recurseDirectories">
		///   If true, the method also scans subdirectories for files matching the criteria.
		/// </param>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.AddSelectedFiles(System.String,System.String,System.String,System.Boolean)" />
		public void UpdateSelectedFiles(string selectionCriteria, string directoryOnDisk, string directoryPathInArchive, bool recurseDirectories)
		{
			_AddOrUpdateSelectedFiles(selectionCriteria, directoryOnDisk, directoryPathInArchive, recurseDirectories, true);
		}

		private string EnsureendInSlash(string s)
		{
			if (s.EndsWith("\\"))
			{
				return s;
			}
			return s + "\\";
		}

		private void _AddOrUpdateSelectedFiles(string selectionCriteria, string directoryOnDisk, string directoryPathInArchive, bool recurseDirectories, bool wantUpdate)
		{
			if (directoryOnDisk == null && Directory.Exists(selectionCriteria))
			{
				directoryOnDisk = selectionCriteria;
				selectionCriteria = "*.*";
			}
			else if (string.IsNullOrEmpty(directoryOnDisk))
			{
				directoryOnDisk = ".";
			}
			while (directoryOnDisk.EndsWith("\\"))
			{
				directoryOnDisk = directoryOnDisk.Substring(0, directoryOnDisk.Length - 1);
			}
			if (Verbose)
			{
				StatusMessageTextWriter.WriteLine("adding selection '{0}' from dir '{1}'...", selectionCriteria, directoryOnDisk);
			}
			ReadOnlyCollection<string> readOnlyCollection = new FileSelector(selectionCriteria, AddDirectoryWillTraverseReparsePoints).SelectFiles(directoryOnDisk, recurseDirectories);
			if (Verbose)
			{
				StatusMessageTextWriter.WriteLine("found {0} files...", readOnlyCollection.Count);
			}
			OnAddStarted();
			AddOrUpdateAction action = (wantUpdate ? AddOrUpdateAction.AddOrUpdate : AddOrUpdateAction.AddOnly);
			foreach (string item in readOnlyCollection)
			{
				string text = ((directoryPathInArchive == null) ? null : ReplaceLeadingDirectory(Path.GetDirectoryName(item), directoryOnDisk, directoryPathInArchive));
				if (File.Exists(item))
				{
					if (wantUpdate)
					{
						UpdateFile(item, text);
					}
					else
					{
						AddFile(item, text);
					}
				}
				else
				{
					AddOrUpdateDirectoryImpl(item, text, action, false, 0);
				}
			}
			OnAddCompleted();
		}

		private static string ReplaceLeadingDirectory(string original, string pattern, string replacement)
		{
			string text = original.ToUpper();
			string text2 = pattern.ToUpper();
			if (text.IndexOf(text2) != 0)
			{
				return original;
			}
			return replacement + original.Substring(text2.Length);
		}

		/// <summary>
		/// Retrieve entries from the zipfile by specified criteria.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		/// This method allows callers to retrieve the collection of entries from the zipfile
		/// that fit the specified criteria.  The criteria are described in a string format, and
		/// can include patterns for the filename; constraints on the size of the entry;
		/// constraints on the last modified, created, or last accessed time for the file
		/// described by the entry; or the attributes of the entry.
		/// </para>
		///
		/// <para>
		/// For details on the syntax for the selectionCriteria parameter, see <see cref="M:Ionic.Zip.ZipFile.AddSelectedFiles(System.String)" />.
		/// </para>
		///
		/// <para>
		/// This method is intended for use with a ZipFile that has been read from storage.
		/// When creating a new ZipFile, this method will work only after the ZipArchive has
		/// been Saved to the disk (the ZipFile class subsequently and implicitly reads the Zip
		/// archive from storage.)  Calling SelectEntries on a ZipFile that has not yet been
		/// saved will deliver undefined results.
		/// </para>
		/// </remarks>
		///
		/// <exception cref="T:System.Exception">
		/// Thrown if selectionCriteria has an invalid syntax.
		/// </exception>
		///
		/// <example>
		/// This example selects all the PhotoShop files from within an archive, and extracts them
		/// to the current working directory.
		/// <code>
		/// using (ZipFile zip1 = ZipFile.Read(ZipFileName))
		/// {
		///     var PhotoShopFiles = zip1.SelectEntries("*.psd");
		///     foreach (ZipEntry psd in PhotoShopFiles)
		///     {
		///         psd.Extract();
		///     }
		/// }
		/// </code>
		/// <code lang="VB">
		/// Using zip1 As ZipFile = ZipFile.Read(ZipFileName)
		///     Dim PhotoShopFiles as ICollection(Of ZipEntry)
		///     PhotoShopFiles = zip1.SelectEntries("*.psd")
		///     Dim psd As ZipEntry
		///     For Each psd In PhotoShopFiles
		///         psd.Extract
		///     Next
		/// End Using
		/// </code>
		/// </example>
		/// <param name="selectionCriteria">the string that specifies which entries to select</param>
		/// <returns>a collection of ZipEntry objects that conform to the inclusion spec</returns>
		public ICollection<ZipEntry> SelectEntries(string selectionCriteria)
		{
			return new FileSelector(selectionCriteria, AddDirectoryWillTraverseReparsePoints).SelectEntries(this);
		}

		/// <summary>
		/// Retrieve entries from the zipfile by specified criteria.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		/// This method allows callers to retrieve the collection of entries from the zipfile
		/// that fit the specified criteria.  The criteria are described in a string format, and
		/// can include patterns for the filename; constraints on the size of the entry;
		/// constraints on the last modified, created, or last accessed time for the file
		/// described by the entry; or the attributes of the entry.
		/// </para>
		///
		/// <para>
		/// For details on the syntax for the selectionCriteria parameter, see <see cref="M:Ionic.Zip.ZipFile.AddSelectedFiles(System.String)" />.
		/// </para>
		///
		/// <para>
		/// This method is intended for use with a ZipFile that has been read from storage.
		/// When creating a new ZipFile, this method will work only after the ZipArchive has
		/// been Saved to the disk (the ZipFile class subsequently and implicitly reads the Zip
		/// archive from storage.)  Calling SelectEntries on a ZipFile that has not yet been
		/// saved will deliver undefined results.
		/// </para>
		/// </remarks>
		///
		/// <exception cref="T:System.Exception">
		/// Thrown if selectionCriteria has an invalid syntax.
		/// </exception>
		///
		/// <example>
		/// <code>
		/// using (ZipFile zip1 = ZipFile.Read(ZipFileName))
		/// {
		///     var UpdatedPhotoShopFiles = zip1.SelectEntries("*.psd", "UpdatedFiles");
		///     foreach (ZipEntry e in UpdatedPhotoShopFiles)
		///     {
		///         // prompt for extract here
		///         if (WantExtract(e.FileName))
		///             e.Extract();
		///     }
		/// }
		/// </code>
		/// <code lang="VB">
		/// Using zip1 As ZipFile = ZipFile.Read(ZipFileName)
		///     Dim UpdatedPhotoShopFiles As ICollection(Of ZipEntry) = zip1.SelectEntries("*.psd", "UpdatedFiles")
		///     Dim e As ZipEntry
		///     For Each e In UpdatedPhotoShopFiles
		///         ' prompt for extract here
		///         If Me.WantExtract(e.FileName) Then
		///             e.Extract
		///         End If
		///     Next
		/// End Using
		/// </code>
		/// </example>
		/// <param name="selectionCriteria">the string that specifies which entries to select</param>
		///
		/// <param name="directoryPathInArchive">
		/// the directory in the archive from which to select entries. If null, then
		/// all directories in the archive are used.
		/// </param>
		///
		/// <returns>a collection of ZipEntry objects that conform to the inclusion spec</returns>
		public ICollection<ZipEntry> SelectEntries(string selectionCriteria, string directoryPathInArchive)
		{
			return new FileSelector(selectionCriteria, AddDirectoryWillTraverseReparsePoints).SelectEntries(this, directoryPathInArchive);
		}

		/// <summary>
		/// Remove entries from the zipfile by specified criteria.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		/// This method allows callers to remove the collection of entries from the zipfile
		/// that fit the specified criteria.  The criteria are described in a string format, and
		/// can include patterns for the filename; constraints on the size of the entry;
		/// constraints on the last modified, created, or last accessed time for the file
		/// described by the entry; or the attributes of the entry.
		/// </para>
		///
		/// <para>
		/// For details on the syntax for the selectionCriteria parameter, see <see cref="M:Ionic.Zip.ZipFile.AddSelectedFiles(System.String)" />.
		/// </para>
		///
		/// <para>
		/// This method is intended for use with a ZipFile that has been read from storage.
		/// When creating a new ZipFile, this method will work only after the ZipArchive has
		/// been Saved to the disk (the ZipFile class subsequently and implicitly reads the Zip
		/// archive from storage.)  Calling SelectEntries on a ZipFile that has not yet been
		/// saved will deliver undefined results.
		/// </para>
		/// </remarks>
		///
		/// <exception cref="T:System.Exception">
		/// Thrown if selectionCriteria has an invalid syntax.
		/// </exception>
		///
		/// <example>
		/// This example removes all entries in a zip file that were modified prior to January 1st, 2008.
		/// <code>
		/// using (ZipFile zip1 = ZipFile.Read(ZipFileName))
		/// {
		///     // remove all entries from prior to Jan 1, 2008
		///     zip1.RemoveEntries("mtime &lt; 2008-01-01");
		///     // don't forget to save the archive!
		///     zip1.Save();
		/// }
		/// </code>
		/// <code lang="VB">
		/// Using zip As ZipFile = ZipFile.Read(ZipFileName)
		///     ' remove all entries from prior to Jan 1, 2008
		///     zip1.RemoveEntries("mtime &lt; 2008-01-01")
		///     ' do not forget to save the archive!
		///     zip1.Save
		/// End Using
		/// </code>
		/// </example>
		/// <param name="selectionCriteria">the string that specifies which entries to select</param>
		/// <returns>the number of entries removed</returns>
		public int RemoveSelectedEntries(string selectionCriteria)
		{
			ICollection<ZipEntry> collection = SelectEntries(selectionCriteria);
			RemoveEntries(collection);
			return collection.Count;
		}

		/// <summary>
		/// Remove entries from the zipfile by specified criteria, and within the specified
		/// path in the archive.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		/// This method allows callers to remove the collection of entries from the zipfile
		/// that fit the specified criteria.  The criteria are described in a string format, and
		/// can include patterns for the filename; constraints on the size of the entry;
		/// constraints on the last modified, created, or last accessed time for the file
		/// described by the entry; or the attributes of the entry.
		/// </para>
		///
		/// <para>
		/// For details on the syntax for the selectionCriteria parameter, see <see cref="M:Ionic.Zip.ZipFile.AddSelectedFiles(System.String)" />.
		/// </para>
		///
		/// <para>
		/// This method is intended for use with a ZipFile that has been read from storage.
		/// When creating a new ZipFile, this method will work only after the ZipArchive has
		/// been Saved to the disk (the ZipFile class subsequently and implicitly reads the Zip
		/// archive from storage.)  Calling SelectEntries on a ZipFile that has not yet been
		/// saved will deliver undefined results.
		/// </para>
		/// </remarks>
		///
		/// <exception cref="T:System.Exception">
		/// Thrown if selectionCriteria has an invalid syntax.
		/// </exception>
		///
		/// <example>
		/// <code>
		/// using (ZipFile zip1 = ZipFile.Read(ZipFileName))
		/// {
		///     // remove all entries from prior to Jan 1, 2008
		///     zip1.RemoveEntries("mtime &lt; 2008-01-01", "documents");
		///     // a call to ZipFile.Save will make the modifications permanent
		///     zip1.Save();
		/// }
		/// </code>
		/// <code lang="VB">
		/// Using zip As ZipFile = ZipFile.Read(ZipFileName)
		///     ' remove all entries from prior to Jan 1, 2008
		///     zip1.RemoveEntries("mtime &lt; 2008-01-01", "documents")
		///     ' a call to ZipFile.Save will make the modifications permanent
		///     zip1.Save
		/// End Using
		/// </code>
		/// </example>
		///
		/// <param name="selectionCriteria">the string that specifies which entries to select</param>
		/// <param name="directoryPathInArchive">
		/// the directory in the archive from which to select entries. If null, then
		/// all directories in the archive are used.
		/// </param>
		/// <returns>the number of entries removed</returns>
		public int RemoveSelectedEntries(string selectionCriteria, string directoryPathInArchive)
		{
			ICollection<ZipEntry> collection = SelectEntries(selectionCriteria, directoryPathInArchive);
			RemoveEntries(collection);
			return collection.Count;
		}

		/// <summary>
		/// Selects and Extracts a set of Entries from the ZipFile.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		/// The entries are extracted into the current working directory.
		/// </para>
		///
		/// <para>
		/// If any of the files to be extracted already exist, then the action taken is as
		/// specified in the <see cref="P:Ionic.Zip.ZipEntry.ExtractExistingFile" /> property on the
		/// corresponding ZipEntry instance.  By default, the action taken in this case is to
		/// throw an exception.
		/// </para>
		///
		/// <para>
		/// For information on the syntax of the selectionCriteria string,
		/// see <see cref="M:Ionic.Zip.ZipFile.AddSelectedFiles(System.String)" />.
		/// </para>
		/// </remarks>
		///
		/// <example>
		/// This example shows how extract all XML files modified after 15 January 2009.
		/// <code>
		/// using (ZipFile zip = ZipFile.Read(zipArchiveName))
		/// {
		///   zip.ExtractSelectedEntries("name = *.xml  and  mtime &gt; 2009-01-15");
		/// }
		/// </code>
		/// </example>
		/// <param name="selectionCriteria">the selection criteria for entries to extract.</param>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.ExtractSelectedEntries(System.String,Ionic.Zip.ExtractExistingFileAction)" />
		public void ExtractSelectedEntries(string selectionCriteria)
		{
			foreach (ZipEntry item in SelectEntries(selectionCriteria))
			{
				item.Password = _Password;
				item.Extract();
			}
		}

		/// <summary>
		/// Selects and Extracts a set of Entries from the ZipFile.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		/// The entries are extracted into the current working directory. When extraction would would
		/// overwrite an existing filesystem file, the action taken is as specified in the
		/// <paramref name="extractExistingFile" /> parameter.
		/// </para>
		///
		/// <para>
		/// For information on the syntax of the string describing the entry selection criteria,
		/// see <see cref="M:Ionic.Zip.ZipFile.AddSelectedFiles(System.String)" />.
		/// </para>
		/// </remarks>
		///
		/// <example>
		/// This example shows how extract all XML files modified after 15 January 2009,
		/// overwriting any existing files.
		/// <code>
		/// using (ZipFile zip = ZipFile.Read(zipArchiveName))
		/// {
		///   zip.ExtractSelectedEntries("name = *.xml  and  mtime &gt; 2009-01-15",
		///                              ExtractExistingFileAction.OverwriteSilently);
		/// }
		/// </code>
		/// </example>
		///
		/// <param name="selectionCriteria">the selection criteria for entries to extract.</param>
		///
		/// <param name="extractExistingFile">
		/// The action to take if extraction would overwrite an existing file.
		/// </param>
		public void ExtractSelectedEntries(string selectionCriteria, ExtractExistingFileAction extractExistingFile)
		{
			foreach (ZipEntry item in SelectEntries(selectionCriteria))
			{
				item.Password = _Password;
				item.Extract(extractExistingFile);
			}
		}

		/// <summary>
		/// Selects and Extracts a set of Entries from the ZipFile.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		/// The entries are selected from the specified directory within the archive, and then
		/// extracted into the current working directory.
		/// </para>
		///
		/// <para>
		/// If any of the files to be extracted already exist, then the action taken is as
		/// specified in the <see cref="P:Ionic.Zip.ZipEntry.ExtractExistingFile" /> property on the
		/// corresponding ZipEntry instance.  By default, the action taken in this case is to
		/// throw an exception.
		/// </para>
		///
		/// <para>
		/// For information on the syntax of the string describing the entry selection criteria,
		/// see <see cref="M:Ionic.Zip.ZipFile.AddSelectedFiles(System.String)" />.
		/// </para>
		/// </remarks>
		///
		/// <example>
		/// This example shows how extract all XML files modified after 15 January 2009,
		/// and writes them to the "unpack" directory.
		/// <code>
		/// using (ZipFile zip = ZipFile.Read(zipArchiveName))
		/// {
		///   zip.ExtractSelectedEntries("name = *.xml  and  mtime &gt; 2009-01-15","unpack");
		/// }
		/// </code>
		/// </example>
		///
		/// <param name="selectionCriteria">the selection criteria for entries to extract.</param>
		///
		/// <param name="directoryPathInArchive">
		/// the directory in the archive from which to select entries. If null, then
		/// all directories in the archive are used.
		/// </param>
		///
		/// <seealso cref="M:Ionic.Zip.ZipFile.ExtractSelectedEntries(System.String,System.String,System.String,Ionic.Zip.ExtractExistingFileAction)" />
		public void ExtractSelectedEntries(string selectionCriteria, string directoryPathInArchive)
		{
			foreach (ZipEntry item in SelectEntries(selectionCriteria, directoryPathInArchive))
			{
				item.Password = _Password;
				item.Extract();
			}
		}

		/// <summary>
		/// Selects and Extracts a set of Entries from the ZipFile.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		/// The entries are extracted into the specified directory. If any of the files to be
		/// extracted already exist, an exception will be thrown.
		/// </para>
		/// <para>
		/// For information on the syntax of the string describing the entry selection criteria,
		/// see <see cref="M:Ionic.Zip.ZipFile.AddSelectedFiles(System.String)" />.
		/// </para>
		/// </remarks>
		///
		/// <param name="selectionCriteria">the selection criteria for entries to extract.</param>
		///
		/// <param name="directoryInArchive">
		/// the directory in the archive from which to select entries. If null, then
		/// all directories in the archive are used.
		/// </param>
		///
		/// <param name="extractDirectory">
		/// the directory on the disk into which to extract. It will be created
		/// if it does not exist.
		/// </param>
		public void ExtractSelectedEntries(string selectionCriteria, string directoryInArchive, string extractDirectory)
		{
			foreach (ZipEntry item in SelectEntries(selectionCriteria, directoryInArchive))
			{
				item.Password = _Password;
				item.Extract(extractDirectory);
			}
		}

		/// <summary>
		/// Selects and Extracts a set of Entries from the ZipFile.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		/// The entries are extracted into the specified directory. When extraction would would
		/// overwrite an existing filesystem file, the action taken is as specified in the
		/// <paramref name="extractExistingFile" /> parameter.
		/// </para>
		///
		/// <para>
		/// For information on the syntax of the string describing the entry selection criteria,
		/// see <see cref="M:Ionic.Zip.ZipFile.AddSelectedFiles(System.String)" />.
		/// </para>
		/// </remarks>
		///
		/// <example>
		/// This example shows how extract all files  with an XML extension or with  a size larger than 100,000 bytes,
		/// and puts them in the unpack directory.  For any files that already exist in
		/// that destination directory, they will not be overwritten.
		/// <code>
		/// using (ZipFile zip = ZipFile.Read(zipArchiveName))
		/// {
		///   zip.ExtractSelectedEntries("name = *.xml  or  size &gt; 100000",
		///                              null,
		///                              "unpack",
		///                              ExtractExistingFileAction.DontOverwrite);
		/// }
		/// </code>
		/// </example>
		///
		/// <param name="selectionCriteria">the selection criteria for entries to extract.</param>
		///
		/// <param name="extractDirectory">
		/// The directory on the disk into which to extract. It will be created if it does not exist.
		/// </param>
		///
		/// <param name="directoryPathInArchive">
		/// The directory in the archive from which to select entries. If null, then
		/// all directories in the archive are used.
		/// </param>
		///
		/// <param name="extractExistingFile">
		/// The action to take if extraction would overwrite an existing file.
		/// </param>
		public void ExtractSelectedEntries(string selectionCriteria, string directoryPathInArchive, string extractDirectory, ExtractExistingFileAction extractExistingFile)
		{
			foreach (ZipEntry item in SelectEntries(selectionCriteria, directoryPathInArchive))
			{
				item.Password = _Password;
				item.Extract(extractDirectory, extractExistingFile);
			}
		}

		/// <summary>
		///
		///
		/// Static constructor for ZipFile
		/// </summary>
		/// <remarks>
		/// Code Pages 437 and 1252 for English are same
		/// Code Page 1252 Windows Latin 1 (ANSI) - <see href="https://msdn.microsoft.com/en-us/library/cc195054.aspx" />
		/// Code Page 437 MS-DOS Latin US - <see href="https://msdn.microsoft.com/en-us/library/cc195060.aspx" />
		/// </remarks>
		static ZipFile()
		{
			SettingsList = new ExtractorSettings[2]
			{
				new ExtractorSettings
				{
					Flavor = SelfExtractorFlavor.WinFormsApplication,
					ReferencedAssemblies = new List<string> { "System.dll", "System.Windows.Forms.dll", "System.Drawing.dll" },
					CopyThroughResources = new List<string> { "Ionic.Zip.WinFormsSelfExtractorStub.resources", "Ionic.Zip.Forms.PasswordDialog.resources", "Ionic.Zip.Forms.ZipContentsDialog.resources" },
					ResourcesToCompile = new List<string> { "WinFormsSelfExtractorStub.cs", "WinFormsSelfExtractorStub.Designer.cs", "PasswordDialog.cs", "PasswordDialog.Designer.cs", "ZipContentsDialog.cs", "ZipContentsDialog.Designer.cs", "FolderBrowserDialogEx.cs" }
				},
				new ExtractorSettings
				{
					Flavor = SelfExtractorFlavor.ConsoleApplication,
					ReferencedAssemblies = new List<string> { "System.dll" },
					CopyThroughResources = null,
					ResourcesToCompile = new List<string> { "CommandLineSelfExtractorStub.cs" }
				}
			};
			BufferSizeDefault = 32768;
			_defaultEncoding = null;
			_defaultEncodingInitialized = false;
			Encoding encoding = null;
			try
			{
				encoding = Encoding.GetEncoding("IBM437");
			}
			catch (Exception)
			{
			}
			if (encoding == null)
			{
				try
				{
					encoding = Encoding.GetEncoding(1252);
				}
				catch (Exception)
				{
				}
			}
			_defaultEncoding = encoding;
		}

		/// <summary>
		/// Generic IEnumerator support, for use of a ZipFile in an enumeration.
		/// </summary>
		///
		/// <remarks>
		/// You probably do not want to call <c>GetEnumerator</c> explicitly. Instead
		/// it is implicitly called when you use a <see langword="foreach" /> loop in C#, or a
		/// <c>For Each</c> loop in VB.NET.
		/// </remarks>
		///
		/// <example>
		/// This example reads a zipfile of a given name, then enumerates the
		/// entries in that zip file, and displays the information about each
		/// entry on the Console.
		/// <code>
		/// using (ZipFile zip = ZipFile.Read(zipfile))
		/// {
		///   bool header = true;
		///   foreach (ZipEntry e in zip)
		///   {
		///     if (header)
		///     {
		///        System.Console.WriteLine("Zipfile: {0}", zip.Name);
		///        System.Console.WriteLine("Version Needed: 0x{0:X2}", e.VersionNeeded);
		///        System.Console.WriteLine("BitField: 0x{0:X2}", e.BitField);
		///        System.Console.WriteLine("Compression Method: 0x{0:X2}", e.CompressionMethod);
		///        System.Console.WriteLine("\n{1,-22} {2,-6} {3,4}   {4,-8}  {0}",
		///                     "Filename", "Modified", "Size", "Ratio", "Packed");
		///        System.Console.WriteLine(new System.String('-', 72));
		///        header = false;
		///     }
		///
		///     System.Console.WriteLine("{1,-22} {2,-6} {3,4:F0}%   {4,-8}  {0}",
		///                 e.FileName,
		///                 e.LastModified.ToString("yyyy-MM-dd HH:mm:ss"),
		///                 e.UncompressedSize,
		///                 e.CompressionRatio,
		///                 e.CompressedSize);
		///
		///     e.Extract();
		///   }
		/// }
		/// </code>
		///
		/// <code lang="VB">
		///   Dim ZipFileToExtract As String = "c:\foo.zip"
		///   Using zip As ZipFile = ZipFile.Read(ZipFileToExtract)
		///       Dim header As Boolean = True
		///       Dim e As ZipEntry
		///       For Each e In zip
		///           If header Then
		///               Console.WriteLine("Zipfile: {0}", zip.Name)
		///               Console.WriteLine("Version Needed: 0x{0:X2}", e.VersionNeeded)
		///               Console.WriteLine("BitField: 0x{0:X2}", e.BitField)
		///               Console.WriteLine("Compression Method: 0x{0:X2}", e.CompressionMethod)
		///               Console.WriteLine(ChrW(10) &amp; "{1,-22} {2,-6} {3,4}   {4,-8}  {0}", _
		///                 "Filename", "Modified", "Size", "Ratio", "Packed" )
		///               Console.WriteLine(New String("-"c, 72))
		///               header = False
		///           End If
		///           Console.WriteLine("{1,-22} {2,-6} {3,4:F0}%   {4,-8}  {0}", _
		///             e.FileName, _
		///             e.LastModified.ToString("yyyy-MM-dd HH:mm:ss"), _
		///             e.UncompressedSize, _
		///             e.CompressionRatio, _
		///             e.CompressedSize )
		///           e.Extract
		///       Next
		///   End Using
		/// </code>
		/// </example>
		///
		/// <returns>A generic enumerator suitable for use  within a foreach loop.</returns>
		public IEnumerator<ZipEntry> GetEnumerator()
		{
			foreach (ZipEntry value in _entries.Values)
			{
				yield return value;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// An IEnumerator, for use of a ZipFile in a foreach construct.
		/// </summary>
		///
		/// <remarks>
		/// This method is included for COM support.  An application generally does not call
		/// this method directly.  It is called implicitly by COM clients when enumerating
		/// the entries in the ZipFile instance.  In VBScript, this is done with a <c>For Each</c>
		/// statement.  In Javascript, this is done with <c>new Enumerator(zipfile)</c>.
		/// </remarks>
		///
		/// <returns>
		/// The IEnumerator over the entries in the ZipFile.
		/// </returns>
		[DispId(-4)]
		public IEnumerator GetNewEnum()
		{
			return GetEnumerator();
		}
	}
}
