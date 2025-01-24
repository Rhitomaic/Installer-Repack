using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Ionic.BZip2;
using Ionic.Crc;
using Ionic.Zip.Deflate64;
using Ionic.Zlib;

namespace Ionic.Zip
{
	/// <summary>
	/// Represents a single entry in a ZipFile. Typically, applications get a ZipEntry
	/// by enumerating the entries within a ZipFile, or by adding an entry to a ZipFile.
	/// </summary>
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00004")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class ZipEntry
	{
		private class CopyHelper
		{
			private static Regex re = new Regex(" \\(copy (\\d+)\\)$");

			private static int callCount = 0;

			internal static void Reset()
			{
				callCount = 0;
			}

			internal static string AppendCopyToFileName(string f)
			{
				callCount++;
				if (callCount > 25)
				{
					throw new OverflowException("overflow while creating filename");
				}
				int num = 1;
				int num2 = f.LastIndexOf(".");
				if (num2 == -1)
				{
					Match match = re.Match(f);
					if (match.Success)
					{
						num = int.Parse(match.Groups[1].Value) + 1;
						string text = $" (copy {num})";
						f = f.Substring(0, match.Index) + text;
					}
					else
					{
						string text2 = $" (copy {num})";
						f += text2;
					}
				}
				else
				{
					Match match2 = re.Match(f.Substring(0, num2));
					if (match2.Success)
					{
						num = int.Parse(match2.Groups[1].Value) + 1;
						string text3 = $" (copy {num})";
						f = f.Substring(0, match2.Index) + text3 + f.Substring(num2);
					}
					else
					{
						string text4 = $" (copy {num})";
						f = f.Substring(0, num2) + text4 + f.Substring(num2);
					}
				}
				return f;
			}
		}

		private delegate T Func<T>();

		private short _VersionMadeBy;

		private short _InternalFileAttrs;

		private int _ExternalFileAttrs;

		private short _filenameLength;

		private short _extraFieldLength;

		private short _commentLength;

		private ZipCrypto _zipCrypto_forExtract;

		private ZipCrypto _zipCrypto_forWrite;

		private WinZipAesCrypto _aesCrypto_forExtract;

		private WinZipAesCrypto _aesCrypto_forWrite;

		private short _WinZipAesMethod;

		internal DateTime _LastModified;

		private bool _dontEmitLastModified;

		private DateTime _Mtime;

		private DateTime _Atime;

		private DateTime _Ctime;

		private bool _ntfsTimesAreSet;

		private bool _emitNtfsTimes = true;

		private bool _emitUnixTimes;

		private bool _TrimVolumeFromFullyQualifiedPaths = true;

		internal string _LocalFileName;

		private string _FileNameInArchive;

		internal short _VersionNeeded;

		internal short _BitField;

		internal short _CompressionMethod;

		private short _CompressionMethod_FromZipFile;

		private CompressionLevel _CompressionLevel;

		internal string _Comment;

		private bool _IsDirectory;

		private byte[] _CommentBytes;

		internal long _CompressedSize;

		internal long _CompressedFileDataSize;

		internal long _UncompressedSize;

		internal int _TimeBlob;

		private bool _crcCalculated;

		internal int _Crc32;

		internal byte[] _Extra;

		private bool _metadataChanged;

		private bool _restreamRequiredOnSave;

		private bool _sourceIsEncrypted;

		private bool _skippedDuringSave;

		private uint _diskNumber;

		private static Encoding ibm437 = Encoding.GetEncoding("IBM437");

		private Encoding _actualEncoding;

		internal ZipContainer _container;

		private long __FileDataPosition = -1L;

		private byte[] _EntryHeader;

		internal long _RelativeOffsetOfLocalHeader;

		private long _future_ROLH;

		private long _TotalEntrySize;

		private int _LengthOfHeader;

		private int _LengthOfTrailer;

		internal bool _InputUsesZip64;

		private uint _UnsupportedAlgorithmId;

		internal string _Password;

		internal ZipEntrySource _Source;

		internal EncryptionAlgorithm _Encryption;

		internal EncryptionAlgorithm _Encryption_FromZipFile;

		internal byte[] _WeakEncryptionHeader;

		internal Stream _archiveStream;

		private Stream _sourceStream;

		private long? _sourceStreamOriginalPosition;

		private bool _sourceWasJitProvided;

		private bool _ioOperationCanceled;

		private bool _presumeZip64;

		private bool? _entryRequiresZip64;

		private bool? _OutputUsesZip64;

		private bool _IsText;

		private ZipEntryTimestamp _timestamp;

		private static DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		private static DateTime _win32Epoch = DateTime.FromFileTimeUtc(0L);

		private static DateTime _zeroHour = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		private WriteDelegate _WriteDelegate;

		private OpenDelegate _OpenDelegate;

		private CloseDelegate _CloseDelegate;

		private Stream _inputDecryptorStream;

		private int _readExtraDepth;

		private object _outputLock = new object();

		/// <summary>
		/// True if the referenced entry is a directory.
		/// </summary>
		internal bool AttributesIndicateDirectory
		{
			get
			{
				if (_InternalFileAttrs == 0)
				{
					return (_ExternalFileAttrs & 0x10) == 16;
				}
				return false;
			}
		}

		/// <summary>
		/// Provides a human-readable string with information about the ZipEntry.
		/// </summary>
		public string Info
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append($"          ZipEntry: {FileName}\n").Append($"   Version Made By: {_VersionMadeBy}\n").Append($" Needed to extract: {VersionNeeded}\n");
				if (_IsDirectory)
				{
					stringBuilder.Append("        Entry type: directory\n");
				}
				else
				{
					stringBuilder.Append(string.Format("         File type: {0}\n", _IsText ? "text" : "binary")).Append($"       Compression: {CompressionMethod}\n").Append($"        Compressed: 0x{CompressedSize:X}\n")
						.Append($"      Uncompressed: 0x{UncompressedSize:X}\n")
						.Append($"             CRC32: 0x{_Crc32:X8}\n");
				}
				stringBuilder.Append($"       Disk Number: {_diskNumber}\n");
				if (_RelativeOffsetOfLocalHeader > uint.MaxValue)
				{
					stringBuilder.Append($"   Relative Offset: 0x{_RelativeOffsetOfLocalHeader:X16}\n");
				}
				else
				{
					stringBuilder.Append($"   Relative Offset: 0x{_RelativeOffsetOfLocalHeader:X8}\n");
				}
				stringBuilder.Append($"         Bit Field: 0x{_BitField:X4}\n").Append($"        Encrypted?: {_sourceIsEncrypted}\n").Append($"          Timeblob: 0x{_TimeBlob:X8}\n")
					.Append($"              Time: {SharedUtilities.PackedToDateTime(_TimeBlob)}\n");
				stringBuilder.Append($"         Is Zip64?: {_InputUsesZip64}\n");
				if (!string.IsNullOrEmpty(_Comment))
				{
					stringBuilder.Append($"           Comment: {_Comment}\n");
				}
				stringBuilder.Append("\n");
				return stringBuilder.ToString();
			}
		}

		/// <summary>
		///   The time and date at which the file indicated by the <c>ZipEntry</c> was
		///   last modified.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   The DotNetZip library sets the LastModified value for an entry, equal to
		///   the Last Modified time of the file in the filesystem.  If an entry is
		///   added from a stream, the library uses <c>System.DateTime.Now</c> for this
		///   value, for the given entry.
		/// </para>
		///
		/// <para>
		///   This property allows the application to retrieve and possibly set the
		///   LastModified value on an entry, to an arbitrary value.  <see cref="T:System.DateTime" /> values with a <see cref="T:System.DateTimeKind" />
		///   setting of <c>DateTimeKind.Unspecified</c> are taken to be expressed as
		///   <c>DateTimeKind.Local</c>.
		/// </para>
		///
		/// <para>
		///   Be aware that because of the way <see href="http://www.pkware.com/documents/casestudies/APPNOTE.TXT">PKWare's
		///   Zip specification</see> describes how times are stored in the zip file,
		///   the full precision of the <c>System.DateTime</c> datatype is not stored
		///   for the last modified time when saving zip files.  For more information on
		///   how times are formatted, see the PKZip specification.
		/// </para>
		///
		/// <para>
		///   The actual last modified time of a file can be stored in multiple ways in
		///   the zip file, and they are not mutually exclusive:
		/// </para>
		///
		/// <list type="bullet">
		///   <item>
		///     In the so-called "DOS" format, which has a 2-second precision. Values
		///     are rounded to the nearest even second. For example, if the time on the
		///     file is 12:34:43, then it will be stored as 12:34:44. This first value
		///     is accessible via the <c>LastModified</c> property. This value is always
		///     present in the metadata for each zip entry.  In some cases the value is
		///     invalid, or zero.
		///   </item>
		///
		///   <item>
		///     In the so-called "Windows" or "NTFS" format, as an 8-byte integer
		///     quantity expressed as the number of 1/10 milliseconds (in other words
		///     the number of 100 nanosecond units) since January 1, 1601 (UTC).  This
		///     format is how Windows represents file times.  This time is accessible
		///     via the <c>ModifiedTime</c> property.
		///   </item>
		///
		///   <item>
		///     In the "Unix" format, a 4-byte quantity specifying the number of seconds since
		///     January 1, 1970 UTC.
		///   </item>
		///
		///   <item>
		///     In an older format, now deprecated but still used by some current
		///     tools. This format is also a 4-byte quantity specifying the number of
		///     seconds since January 1, 1970 UTC.
		///   </item>
		///
		/// </list>
		///
		/// <para>
		///   Zip tools and libraries will always at least handle (read or write) the
		///   DOS time, and may also handle the other time formats.  Keep in mind that
		///   while the names refer to particular operating systems, there is nothing in
		///   the time formats themselves that prevents their use on other operating
		///   systems.
		/// </para>
		///
		/// <para>
		///   When reading ZIP files, the DotNetZip library reads the Windows-formatted
		///   time, if it is stored in the entry, and sets both <c>LastModified</c> and
		///   <c>ModifiedTime</c> to that value. When writing ZIP files, the DotNetZip
		///   library by default will write both time quantities. It can also emit the
		///   Unix-formatted time if desired (See <see cref="P:Ionic.Zip.ZipEntry.EmitTimesInUnixFormatWhenSaving" />.)
		/// </para>
		///
		/// <para>
		///   The last modified time of the file created upon a call to
		///   <c>ZipEntry.Extract()</c> may be adjusted during extraction to compensate
		///   for differences in how the .NET Base Class Library deals with daylight
		///   saving time (DST) versus how the Windows filesystem deals with daylight
		///   saving time.  Raymond Chen <see href="http://blogs.msdn.com/oldnewthing/archive/2003/10/24/55413.aspx">provides
		///   some good context</see>.
		/// </para>
		///
		/// <para>
		///   In a nutshell: Daylight savings time rules change regularly.  In 2007, for
		///   example, the inception week of DST changed.  In 1977, DST was in place all
		///   year round. In 1945, likewise.  And so on.  Win32 does not attempt to
		///   guess which time zone rules were in effect at the time in question.  It
		///   will render a time as "standard time" and allow the app to change to DST
		///   as necessary.  .NET makes a different choice.
		/// </para>
		///
		/// <para>
		///   Compare the output of FileInfo.LastWriteTime.ToString("f") with what you
		///   see in the Windows Explorer property sheet for a file that was last
		///   written to on the other side of the DST transition. For example, suppose
		///   the file was last modified on October 17, 2003, during DST but DST is not
		///   currently in effect. Explorer's file properties reports Thursday, October
		///   17, 2003, 8:45:38 AM, but .NETs FileInfo reports Thursday, October 17,
		///   2003, 9:45 AM.
		/// </para>
		///
		/// <para>
		///   Win32 says, "Thursday, October 17, 2002 8:45:38 AM PST". Note: Pacific
		///   STANDARD Time. Even though October 17 of that year occurred during Pacific
		///   Daylight Time, Win32 displays the time as standard time because that's
		///   what time it is NOW.
		/// </para>
		///
		/// <para>
		///   .NET BCL assumes that the current DST rules were in place at the time in
		///   question.  So, .NET says, "Well, if the rules in effect now were also in
		///   effect on October 17, 2003, then that would be daylight time" so it
		///   displays "Thursday, October 17, 2003, 9:45 AM PDT" - daylight time.
		/// </para>
		///
		/// <para>
		///   So .NET gives a value which is more intuitively correct, but is also
		///   potentially incorrect, and which is not invertible. Win32 gives a value
		///   which is intuitively incorrect, but is strictly correct.
		/// </para>
		///
		/// <para>
		///   Because of this funkiness, this library adds one hour to the LastModified
		///   time on the extracted file, if necessary.  That is to say, if the time in
		///   question had occurred in what the .NET Base Class Library assumed to be
		///   DST. This assumption may be wrong given the constantly changing DST rules,
		///   but it is the best we can do.
		/// </para>
		///
		/// </remarks>
		public DateTime LastModified
		{
			get
			{
				return _LastModified.ToLocalTime();
			}
			set
			{
				_LastModified = ((value.Kind == DateTimeKind.Unspecified) ? DateTime.SpecifyKind(value, DateTimeKind.Local) : value.ToLocalTime());
				_Mtime = SharedUtilities.AdjustTime_Reverse(_LastModified).ToUniversalTime();
				_metadataChanged = true;
			}
		}

		/// <summary>
		/// Ability to set Last Modified DOS time to zero
		/// (for using with EmitTimesInWindowsFormatWhenSaving+EmitTimesInUnixFormatWhenSaving setted to false)
		/// some flasher hardware use as marker of first binary
		/// </summary>
		public bool DontEmitLastModified
		{
			get
			{
				return _dontEmitLastModified;
			}
			set
			{
				_dontEmitLastModified = value;
			}
		}

		private int BufferSize => _container.BufferSize;

		/// <summary>
		/// Last Modified time for the file represented by the entry.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   This value corresponds to the "last modified" time in the NTFS file times
		///   as described in <see href="http://www.pkware.com/documents/casestudies/APPNOTE.TXT">the Zip
		///   specification</see>.  When getting this property, the value may be
		///   different from <see cref="P:Ionic.Zip.ZipEntry.LastModified" />.  When setting the property,
		///   the <see cref="P:Ionic.Zip.ZipEntry.LastModified" /> property also gets set, but with a lower
		///   precision.
		/// </para>
		///
		/// <para>
		///   Let me explain. It's going to take a while, so get
		///   comfortable. Originally, waaaaay back in 1989 when the ZIP specification
		///   was originally described by the esteemed Mr. Phil Katz, the dominant
		///   operating system of the time was MS-DOS. MSDOS stored file times with a
		///   2-second precision, because, c'mon, <em>who is ever going to need better
		///   resolution than THAT?</em> And so ZIP files, regardless of the platform on
		///   which the zip file was created, store file times in exactly <see href="http://www.vsft.com/hal/dostime.htm">the same format that DOS used
		///   in 1989</see>.
		/// </para>
		///
		/// <para>
		///   Since then, the ZIP spec has evolved, but the internal format for file
		///   timestamps remains the same.  Despite the fact that the way times are
		///   stored in a zip file is rooted in DOS heritage, any program on any
		///   operating system can format a time in this way, and most zip tools and
		///   libraries DO - they round file times to the nearest even second and store
		///   it just like DOS did 25+ years ago.
		/// </para>
		///
		/// <para>
		///   PKWare extended the ZIP specification to allow a zip file to store what
		///   are called "NTFS Times" and "Unix(tm) times" for a file.  These are the
		///   <em>last write</em>, <em>last access</em>, and <em>file creation</em>
		///   times of a particular file. These metadata are not actually specific
		///   to NTFS or Unix. They are tracked for each file by NTFS and by various
		///   Unix filesystems, but they are also tracked by other filesystems, too.
		///   The key point is that the times are <em>formatted in the zip file</em>
		///   in the same way that NTFS formats the time (ticks since win32 epoch),
		///   or in the same way that Unix formats the time (seconds since Unix
		///   epoch). As with the DOS time, any tool or library running on any
		///   operating system is capable of formatting a time in one of these ways
		///   and embedding it into the zip file.
		/// </para>
		///
		/// <para>
		///   These extended times are higher precision quantities than the DOS time.
		///   As described above, the (DOS) LastModified has a precision of 2 seconds.
		///   The Unix time is stored with a precision of 1 second. The NTFS time is
		///   stored with a precision of 0.0000001 seconds. The quantities are easily
		///   convertible, except for the loss of precision you may incur.
		/// </para>
		///
		/// <para>
		///   A zip archive can store the {C,A,M} times in NTFS format, in Unix format,
		///   or not at all.  Often a tool running on Unix or Mac will embed the times
		///   in Unix format (1 second precision), while WinZip running on Windows might
		///   embed the times in NTFS format (precision of of 0.0000001 seconds).  When
		///   reading a zip file with these "extended" times, in either format,
		///   DotNetZip represents the values with the
		///   <c>ModifiedTime</c>, <c>AccessedTime</c> and <c>CreationTime</c>
		///   properties on the <c>ZipEntry</c>.
		/// </para>
		///
		/// <para>
		///   While any zip application or library, regardless of the platform it
		///   runs on, could use any of the time formats allowed by the ZIP
		///   specification, not all zip tools or libraries do support all these
		///   formats.  Storing the higher-precision times for each entry is
		///   optional for zip files, and many tools and libraries don't use the
		///   higher precision quantities at all. The old DOS time, represented by
		///   <see cref="P:Ionic.Zip.ZipEntry.LastModified" />, is guaranteed to be present, though it
		///   sometimes unset.
		/// </para>
		///
		/// <para>
		///   Ok, getting back to the question about how the <c>LastModified</c>
		///   property relates to this <c>ModifiedTime</c>
		///   property... <c>LastModified</c> is always set, while
		///   <c>ModifiedTime</c> is not. (The other times stored in the <em>NTFS
		///   times extension</em>, <c>CreationTime</c> and <c>AccessedTime</c> also
		///   may not be set on an entry that is read from an existing zip file.)
		///   When reading a zip file, then <c>LastModified</c> takes the DOS time
		///   that is stored with the file. If the DOS time has been stored as zero
		///   in the zipfile, then this library will use <c>DateTime.Now</c> for the
		///   <c>LastModified</c> value.  If the ZIP file was created by an evolved
		///   tool, then there will also be higher precision NTFS or Unix times in
		///   the zip file.  In that case, this library will read those times, and
		///   set <c>LastModified</c> and <c>ModifiedTime</c> to the same value, the
		///   one corresponding to the last write time of the file.  If there are no
		///   higher precision times stored for the entry, then <c>ModifiedTime</c>
		///   remains unset (likewise <c>AccessedTime</c> and <c>CreationTime</c>),
		///   and <c>LastModified</c> keeps its DOS time.
		/// </para>
		///
		/// <para>
		///   When creating zip files with this library, by default the extended time
		///   properties (<c>ModifiedTime</c>, <c>AccessedTime</c>, and
		///   <c>CreationTime</c>) are set on the ZipEntry instance, and these data are
		///   stored in the zip archive for each entry, in NTFS format. If you add an
		///   entry from an actual filesystem file, then the entry gets the actual file
		///   times for that file, to NTFS-level precision.  If you add an entry from a
		///   stream, or a string, then the times get the value <c>DateTime.Now</c>.  In
		///   this case <c>LastModified</c> and <c>ModifiedTime</c> will be identical,
		///   to 2 seconds of precision.  You can explicitly set the
		///   <c>CreationTime</c>, <c>AccessedTime</c>, and <c>ModifiedTime</c> of an
		///   entry using the property setters.  If you want to set all of those
		///   quantities, it's more efficient to use the <see cref="M:Ionic.Zip.ZipEntry.SetEntryTimes(System.DateTime,System.DateTime,System.DateTime)" /> method.  Those
		///   changes are not made permanent in the zip file until you call <see cref="M:Ionic.Zip.ZipFile.Save" /> or one of its cousins.
		/// </para>
		///
		/// <para>
		///   When creating a zip file, you can override the default behavior of
		///   this library for formatting times in the zip file, disabling the
		///   embedding of file times in NTFS format or enabling the storage of file
		///   times in Unix format, or both.  You may want to do this, for example,
		///   when creating a zip file on Windows, that will be consumed on a Mac,
		///   by an application that is not hip to the "NTFS times" format. To do
		///   this, use the <see cref="P:Ionic.Zip.ZipEntry.EmitTimesInWindowsFormatWhenSaving" /> and
		///   <see cref="P:Ionic.Zip.ZipEntry.EmitTimesInUnixFormatWhenSaving" /> properties.  A valid zip
		///   file may store the file times in both formats.  But, there are no
		///   guarantees that a program running on Mac or Linux will gracefully
		///   handle the NTFS-formatted times when Unix times are present, or that a
		///   non-DotNetZip-powered application running on Windows will be able to
		///   handle file times in Unix format. DotNetZip will always do something
		///   reasonable; other libraries or tools may not. When in doubt, test.
		/// </para>
		///
		/// <para>
		///   I'll bet you didn't think one person could type so much about time, eh?
		///   And reading it was so enjoyable, too!  Well, in appreciation, <see href="http://cheeso.members.winisp.net/DotNetZipDonate.aspx">maybe you
		///   should donate</see>?
		/// </para>
		/// </remarks>
		///
		/// <seealso cref="P:Ionic.Zip.ZipEntry.AccessedTime" />
		/// <seealso cref="P:Ionic.Zip.ZipEntry.CreationTime" />
		/// <seealso cref="P:Ionic.Zip.ZipEntry.LastModified" />
		/// <seealso cref="M:Ionic.Zip.ZipEntry.SetEntryTimes(System.DateTime,System.DateTime,System.DateTime)" />
		public DateTime ModifiedTime
		{
			get
			{
				return _Mtime;
			}
			set
			{
				SetEntryTimes(_Ctime, _Atime, value);
			}
		}

		/// <summary>
		/// Last Access time for the file represented by the entry.
		/// </summary>
		/// <remarks>
		/// This value may or may not be meaningful.  If the <c>ZipEntry</c> was read from an existing
		/// Zip archive, this information may not be available. For an explanation of why, see
		/// <see cref="P:Ionic.Zip.ZipEntry.ModifiedTime" />.
		/// </remarks>
		/// <seealso cref="P:Ionic.Zip.ZipEntry.ModifiedTime" />
		/// <seealso cref="P:Ionic.Zip.ZipEntry.CreationTime" />
		/// <seealso cref="M:Ionic.Zip.ZipEntry.SetEntryTimes(System.DateTime,System.DateTime,System.DateTime)" />
		public DateTime AccessedTime
		{
			get
			{
				return _Atime;
			}
			set
			{
				SetEntryTimes(_Ctime, value, _Mtime);
			}
		}

		/// <summary>
		/// The file creation time for the file represented by the entry.
		/// </summary>
		///
		/// <remarks>
		/// This value may or may not be meaningful.  If the <c>ZipEntry</c> was read
		/// from an existing zip archive, and the creation time was not set on the entry
		/// when the zip file was created, then this property may be meaningless. For an
		/// explanation of why, see <see cref="P:Ionic.Zip.ZipEntry.ModifiedTime" />.
		/// </remarks>
		/// <seealso cref="P:Ionic.Zip.ZipEntry.ModifiedTime" />
		/// <seealso cref="P:Ionic.Zip.ZipEntry.AccessedTime" />
		/// <seealso cref="M:Ionic.Zip.ZipEntry.SetEntryTimes(System.DateTime,System.DateTime,System.DateTime)" />
		public DateTime CreationTime
		{
			get
			{
				return _Ctime;
			}
			set
			{
				SetEntryTimes(value, _Atime, _Mtime);
			}
		}

		/// <summary>
		///   Specifies whether the Creation, Access, and Modified times for the given
		///   entry will be emitted in "Windows format" when the zip archive is saved.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   An application creating a zip archive can use this flag to explicitly
		///   specify that the file times for the entry should or should not be stored
		///   in the zip archive in the format used by Windows. The default value of
		///   this property is <c>true</c>.
		/// </para>
		///
		/// <para>
		///   When adding an entry from a file or directory, the Creation (<see cref="P:Ionic.Zip.ZipEntry.CreationTime" />), Access (<see cref="P:Ionic.Zip.ZipEntry.AccessedTime" />), and Modified
		///   (<see cref="P:Ionic.Zip.ZipEntry.ModifiedTime" />) times for the given entry are automatically
		///   set from the filesystem values. When adding an entry from a stream or
		///   string, all three values are implicitly set to DateTime.Now.  Applications
		///   can also explicitly set those times by calling <see cref="M:Ionic.Zip.ZipEntry.SetEntryTimes(System.DateTime,System.DateTime,System.DateTime)" />.
		/// </para>
		///
		/// <para>
		///   <see href="http://www.pkware.com/documents/casestudies/APPNOTE.TXT">PKWARE's
		///   zip specification</see> describes multiple ways to format these times in a
		///   zip file. One is the format Windows applications normally use: 100ns ticks
		///   since Jan 1, 1601 UTC.  The other is a format Unix applications typically
		///   use: seconds since January 1, 1970 UTC.  Each format can be stored in an
		///   "extra field" in the zip entry when saving the zip archive. The former
		///   uses an extra field with a Header Id of 0x000A, while the latter uses a
		///   header ID of 0x5455.
		/// </para>
		///
		/// <para>
		///   Not all zip tools and libraries can interpret these fields.  Windows
		///   compressed folders is one that can read the Windows Format timestamps,
		///   while I believe the <see href="http://www.info-zip.org/">Infozip</see>
		///   tools can read the Unix format timestamps. Although the time values are
		///   easily convertible, subject to a loss of precision, some tools and
		///   libraries may be able to read only one or the other. DotNetZip can read or
		///   write times in either or both formats.
		/// </para>
		///
		/// <para>
		///   The times stored are taken from <see cref="P:Ionic.Zip.ZipEntry.ModifiedTime" />, <see cref="P:Ionic.Zip.ZipEntry.AccessedTime" />, and <see cref="P:Ionic.Zip.ZipEntry.CreationTime" />.
		/// </para>
		///
		/// <para>
		///   This property is not mutually exclusive from the <see cref="P:Ionic.Zip.ZipEntry.EmitTimesInUnixFormatWhenSaving" /> property.  It is
		///   possible that a zip entry can embed the timestamps in both forms, one
		///   form, or neither.  But, there are no guarantees that a program running on
		///   Mac or Linux will gracefully handle NTFS Formatted times, or that a
		///   non-DotNetZip-powered application running on Windows will be able to
		///   handle file times in Unix format. When in doubt, test.
		/// </para>
		///
		/// <para>
		///   Normally you will use the <see cref="P:Ionic.Zip.ZipFile.EmitTimesInWindowsFormatWhenSaving">ZipFile.EmitTimesInWindowsFormatWhenSaving</see>
		///   property, to specify the behavior for all entries in a zip, rather than
		///   the property on each individual entry.
		/// </para>
		///
		/// </remarks>
		///
		/// <seealso cref="M:Ionic.Zip.ZipEntry.SetEntryTimes(System.DateTime,System.DateTime,System.DateTime)" />
		/// <seealso cref="P:Ionic.Zip.ZipEntry.EmitTimesInUnixFormatWhenSaving" />
		/// <seealso cref="P:Ionic.Zip.ZipEntry.CreationTime" />
		/// <seealso cref="P:Ionic.Zip.ZipEntry.AccessedTime" />
		/// <seealso cref="P:Ionic.Zip.ZipEntry.ModifiedTime" />
		public bool EmitTimesInWindowsFormatWhenSaving
		{
			get
			{
				return _emitNtfsTimes;
			}
			set
			{
				_emitNtfsTimes = value;
				_metadataChanged = true;
			}
		}

		/// <summary>
		///   Specifies whether the Creation, Access, and Modified times for the given
		///   entry will be emitted in "Unix(tm) format" when the zip archive is saved.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   An application creating a zip archive can use this flag to explicitly
		///   specify that the file times for the entry should or should not be stored
		///   in the zip archive in the format used by Unix. By default this flag is
		///   <c>false</c>, meaning the Unix-format times are not stored in the zip
		///   archive.
		/// </para>
		///
		/// <para>
		///   When adding an entry from a file or directory, the Creation (<see cref="P:Ionic.Zip.ZipEntry.CreationTime" />), Access (<see cref="P:Ionic.Zip.ZipEntry.AccessedTime" />), and Modified
		///   (<see cref="P:Ionic.Zip.ZipEntry.ModifiedTime" />) times for the given entry are automatically
		///   set from the filesystem values. When adding an entry from a stream or
		///   string, all three values are implicitly set to DateTime.Now.  Applications
		///   can also explicitly set those times by calling <see cref="M:Ionic.Zip.ZipEntry.SetEntryTimes(System.DateTime,System.DateTime,System.DateTime)" />.
		/// </para>
		///
		/// <para>
		///   <see href="http://www.pkware.com/documents/casestudies/APPNOTE.TXT">PKWARE's
		///   zip specification</see> describes multiple ways to format these times in a
		///   zip file. One is the format Windows applications normally use: 100ns ticks
		///   since Jan 1, 1601 UTC.  The other is a format Unix applications typically
		///   use: seconds since Jan 1, 1970 UTC.  Each format can be stored in an
		///   "extra field" in the zip entry when saving the zip archive. The former
		///   uses an extra field with a Header Id of 0x000A, while the latter uses a
		///   header ID of 0x5455.
		/// </para>
		///
		/// <para>
		///   Not all tools and libraries can interpret these fields.  Windows
		///   compressed folders is one that can read the Windows Format timestamps,
		///   while I believe the <see href="http://www.info-zip.org/">Infozip</see>
		///   tools can read the Unix format timestamps. Although the time values are
		///   easily convertible, subject to a loss of precision, some tools and
		///   libraries may be able to read only one or the other. DotNetZip can read or
		///   write times in either or both formats.
		/// </para>
		///
		/// <para>
		///   The times stored are taken from <see cref="P:Ionic.Zip.ZipEntry.ModifiedTime" />, <see cref="P:Ionic.Zip.ZipEntry.AccessedTime" />, and <see cref="P:Ionic.Zip.ZipEntry.CreationTime" />.
		/// </para>
		///
		/// <para>
		///   This property is not mutually exclusive from the <see cref="P:Ionic.Zip.ZipEntry.EmitTimesInWindowsFormatWhenSaving" /> property.  It is
		///   possible that a zip entry can embed the timestamps in both forms, one
		///   form, or neither.  But, there are no guarantees that a program running on
		///   Mac or Linux will gracefully handle NTFS Formatted times, or that a
		///   non-DotNetZip-powered application running on Windows will be able to
		///   handle file times in Unix format. When in doubt, test.
		/// </para>
		///
		/// <para>
		///   Normally you will use the <see cref="P:Ionic.Zip.ZipFile.EmitTimesInUnixFormatWhenSaving">ZipFile.EmitTimesInUnixFormatWhenSaving</see>
		///   property, to specify the behavior for all entries, rather than the
		///   property on each individual entry.
		/// </para>
		/// </remarks>
		///
		/// <seealso cref="M:Ionic.Zip.ZipEntry.SetEntryTimes(System.DateTime,System.DateTime,System.DateTime)" />
		/// <seealso cref="P:Ionic.Zip.ZipEntry.EmitTimesInWindowsFormatWhenSaving" />
		/// <seealso cref="P:Ionic.Zip.ZipFile.EmitTimesInUnixFormatWhenSaving" />
		/// <seealso cref="P:Ionic.Zip.ZipEntry.CreationTime" />
		/// <seealso cref="P:Ionic.Zip.ZipEntry.AccessedTime" />
		/// <seealso cref="P:Ionic.Zip.ZipEntry.ModifiedTime" />
		public bool EmitTimesInUnixFormatWhenSaving
		{
			get
			{
				return _emitUnixTimes;
			}
			set
			{
				_emitUnixTimes = value;
				_metadataChanged = true;
			}
		}

		/// <summary>
		/// The type of timestamp attached to the ZipEntry.
		/// </summary>
		///
		/// <remarks>
		/// This property is valid only for a ZipEntry that was read from a zip archive.
		/// It indicates the type of timestamp attached to the entry.
		/// </remarks>
		///
		/// <seealso cref="P:Ionic.Zip.ZipEntry.EmitTimesInWindowsFormatWhenSaving" />
		/// <seealso cref="P:Ionic.Zip.ZipEntry.EmitTimesInUnixFormatWhenSaving" />
		public ZipEntryTimestamp Timestamp => _timestamp;

		/// <summary>
		///   The file attributes for the entry.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   The <see cref="T:System.IO.FileAttributes">attributes</see> in NTFS include
		///   ReadOnly, Archive, Hidden, System, and Indexed.  When adding a
		///   <c>ZipEntry</c> to a ZipFile, these attributes are set implicitly when
		///   adding an entry from the filesystem.  When adding an entry from a stream
		///   or string, the Attributes are not set implicitly.  Regardless of the way
		///   an entry was added to a <c>ZipFile</c>, you can set the attributes
		///   explicitly if you like.
		/// </para>
		///
		/// <para>
		///   When reading a <c>ZipEntry</c> from a <c>ZipFile</c>, the attributes are
		///   set according to the data stored in the <c>ZipFile</c>. If you extract the
		///   entry from the archive to a filesystem file, DotNetZip will set the
		///   attributes on the resulting file accordingly.
		/// </para>
		///
		/// <para>
		///   The attributes can be set explicitly by the application.  For example the
		///   application may wish to set the <c>FileAttributes.ReadOnly</c> bit for all
		///   entries added to an archive, so that on unpack, this attribute will be set
		///   on the extracted file.  Any changes you make to this property are made
		///   permanent only when you call a <c>Save()</c> method on the <c>ZipFile</c>
		///   instance that contains the ZipEntry.
		/// </para>
		///
		/// <para>
		///   For example, an application may wish to zip up a directory and set the
		///   ReadOnly bit on every file in the archive, so that upon later extraction,
		///   the resulting files will be marked as ReadOnly.  Not every extraction tool
		///   respects these attributes, but if you unpack with DotNetZip, as for
		///   example in a self-extracting archive, then the attributes will be set as
		///   they are stored in the <c>ZipFile</c>.
		/// </para>
		///
		/// <para>
		///   These attributes may not be interesting or useful if the resulting archive
		///   is extracted on a non-Windows platform.  How these attributes get used
		///   upon extraction depends on the platform and tool used.
		/// </para>
		///
		/// </remarks>
		public FileAttributes Attributes
		{
			get
			{
				return (FileAttributes)_ExternalFileAttrs;
			}
			set
			{
				_ExternalFileAttrs = (int)value;
				_VersionMadeBy = 45;
				_metadataChanged = true;
			}
		}

		/// <summary>
		///   The name of the filesystem file, referred to by the ZipEntry.
		/// </summary>
		///
		/// <remarks>
		///  <para>
		///    This property specifies the thing-to-be-zipped on disk, and is set only
		///    when the <c>ZipEntry</c> is being created from a filesystem file.  If the
		///    <c>ZipFile</c> is instantiated by reading an existing .zip archive, then
		///    the LocalFileName will be <c>null</c> (<c>Nothing</c> in VB).
		///  </para>
		///
		///  <para>
		///    When it is set, the value of this property may be different than <see cref="P:Ionic.Zip.ZipEntry.FileName" />, which is the path used in the archive itself.  If you
		///    call <c>Zip.AddFile("foop.txt", AlternativeDirectory)</c>, then the path
		///    used for the <c>ZipEntry</c> within the zip archive will be different
		///    than this path.
		///  </para>
		///
		///  <para>
		///   If the entry is being added from a stream, then this is null (Nothing in VB).
		///  </para>
		///
		/// </remarks>
		/// <seealso cref="P:Ionic.Zip.ZipEntry.FileName" />
		internal string LocalFileName => _LocalFileName;

		/// <summary>
		///   The name of the file contained in the ZipEntry.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   This is the name of the entry in the <c>ZipFile</c> itself.  When creating
		///   a zip archive, if the <c>ZipEntry</c> has been created from a filesystem
		///   file, via a call to <see cref="M:Ionic.Zip.ZipFile.AddFile(System.String,System.String)" /> or <see cref="M:Ionic.Zip.ZipFile.AddItem(System.String,System.String)" />, or a related overload, the value
		///   of this property is derived from the name of that file. The
		///   <c>FileName</c> property does not include drive letters, and may include a
		///   different directory path, depending on the value of the
		///   <c>directoryPathInArchive</c> parameter used when adding the entry into
		///   the <c>ZipFile</c>.
		/// </para>
		///
		/// <para>
		///   In some cases there is no related filesystem file - for example when a
		///   <c>ZipEntry</c> is created using <see cref="M:Ionic.Zip.ZipFile.AddEntry(System.String,System.String)" /> or one of the similar overloads.  In this case, the value of
		///   this property is derived from the fileName and the directory path passed
		///   to that method.
		/// </para>
		///
		/// <para>
		///   When reading a zip file, this property takes the value of the entry name
		///   as stored in the zip file. If you extract such an entry, the extracted
		///   file will take the name given by this property.
		/// </para>
		///
		/// <para>
		///   Applications can set this property when creating new zip archives or when
		///   reading existing archives. When setting this property, the actual value
		///   that is set will replace backslashes with forward slashes, in accordance
		///   with <see href="http://www.pkware.com/documents/casestudies/APPNOTE.TXT">the Zip
		///   specification</see>, for compatibility with Unix(tm) and ... get
		///   this.... Amiga!
		/// </para>
		///
		/// <para>
		///   If an application reads a <c>ZipFile</c> via <see cref="M:Ionic.Zip.ZipFile.Read(System.String)" /> or a related overload, and then explicitly
		///   sets the FileName on an entry contained within the <c>ZipFile</c>, and
		///   then calls <see cref="M:Ionic.Zip.ZipFile.Save" />, the application will effectively
		///   rename the entry within the zip archive.
		/// </para>
		///
		/// <para>
		///   If an application sets the value of <c>FileName</c>, then calls
		///   <c>Extract()</c> on the entry, the entry is extracted to a file using the
		///   newly set value as the filename.  The <c>FileName</c> value is made
		///   permanent in the zip archive only <em>after</em> a call to one of the
		///   <c>ZipFile.Save()</c> methods on the <c>ZipFile</c> that contains the
		///   ZipEntry.
		/// </para>
		///
		/// <para>
		///   If an application attempts to set the <c>FileName</c> to a value that
		///   would result in a duplicate entry in the <c>ZipFile</c>, an exception is
		///   thrown.
		/// </para>
		///
		/// <para>
		///   When a <c>ZipEntry</c> is contained within a <c>ZipFile</c>, applications
		///   cannot rename the entry within the context of a <c>foreach</c> (<c>For
		///   Each</c> in VB) loop, because of the way the <c>ZipFile</c> stores
		///   entries.  If you need to enumerate through all the entries and rename one
		///   or more of them, use <see cref="P:Ionic.Zip.ZipFile.EntriesSorted">ZipFile.EntriesSorted</see> as the
		///   collection.  See also, <see cref="M:Ionic.Zip.ZipFile.GetEnumerator">ZipFile.GetEnumerator()</see>.
		/// </para>
		///
		/// </remarks>
		public string FileName
		{
			get
			{
				return _FileNameInArchive;
			}
			set
			{
				if (_container != null && _container.ZipFile == null)
				{
					throw new ZipException("Cannot rename; this is not supported in ZipOutputStream/ZipInputStream.");
				}
				if (string.IsNullOrEmpty(value))
				{
					throw new ZipException("The FileName must be non empty and non-null.");
				}
				string text = NameInArchive(value, null);
				if (!(_FileNameInArchive == text))
				{
					if (_container != null)
					{
						_container.ZipFile.RemoveEntry(this);
						_container.ZipFile.InternalAddEntry(text, this);
					}
					_FileNameInArchive = text;
					if (_container != null)
					{
						_container.ZipFile.NotifyEntryChanged();
					}
					_metadataChanged = true;
				}
			}
		}

		/// <summary>
		/// The stream that provides content for the ZipEntry.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   The application can use this property to set the input stream for an
		///   entry on a just-in-time basis. Imagine a scenario where the application
		///   creates a <c>ZipFile</c> comprised of content obtained from hundreds of
		///   files, via calls to <c>AddFile()</c>. The DotNetZip library opens streams
		///   on these files on a just-in-time basis, only when writing the entry out to
		///   an external store within the scope of a <c>ZipFile.Save()</c> call.  Only
		///   one input stream is opened at a time, as each entry is being written out.
		/// </para>
		///
		/// <para>
		///   Now imagine a different application that creates a <c>ZipFile</c>
		///   with content obtained from hundreds of streams, added through <see cref="M:Ionic.Zip.ZipFile.AddEntry(System.String,System.IO.Stream)" />.  Normally the
		///   application would supply an open stream to that call.  But when large
		///   numbers of streams are being added, this can mean many open streams at one
		///   time, unnecessarily.
		/// </para>
		///
		/// <para>
		///   To avoid this, call <see cref="M:Ionic.Zip.ZipFile.AddEntry(System.String,Ionic.Zip.OpenDelegate,Ionic.Zip.CloseDelegate)" /> and specify delegates that open and close the stream at
		///   the time of Save.
		/// </para>
		///
		///
		/// <para>
		///   Setting the value of this property when the entry was not added from a
		///   stream (for example, when the <c>ZipEntry</c> was added with <see cref="M:Ionic.Zip.ZipFile.AddFile(System.String)" /> or <see cref="M:Ionic.Zip.ZipFile.AddDirectory(System.String)" />, or when the entry was added by
		///   reading an existing zip archive) will throw an exception.
		/// </para>
		///
		/// </remarks>
		public Stream InputStream
		{
			get
			{
				return _sourceStream;
			}
			set
			{
				if (_Source != ZipEntrySource.Stream)
				{
					throw new ZipException("You must not set the input stream for this entry.");
				}
				_sourceWasJitProvided = true;
				_sourceStream = value;
			}
		}

		/// <summary>
		///   A flag indicating whether the InputStream was provided Just-in-time.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   When creating a zip archive, an application can obtain content for one or
		///   more of the <c>ZipEntry</c> instances from streams, using the <see cref="M:Ionic.Zip.ZipFile.AddEntry(System.String,System.IO.Stream)" /> method.  At the time
		///   of calling that method, the application can supply null as the value of
		///   the stream parameter.  By doing so, the application indicates to the
		///   library that it will provide a stream for the entry on a just-in-time
		///   basis, at the time one of the <c>ZipFile.Save()</c> methods is called and
		///   the data for the various entries are being compressed and written out.
		/// </para>
		///
		/// <para>
		///   In this case, the application can set the <see cref="P:Ionic.Zip.ZipEntry.InputStream" />
		///   property, typically within the SaveProgress event (event type: <see cref="F:Ionic.Zip.ZipProgressEventType.Saving_BeforeWriteEntry" />) for that entry.
		/// </para>
		///
		/// <para>
		///   The application will later want to call Close() and Dispose() on that
		///   stream.  In the SaveProgress event, when the event type is <see cref="F:Ionic.Zip.ZipProgressEventType.Saving_AfterWriteEntry" />, the application can
		///   do so.  This flag indicates that the stream has been provided by the
		///   application on a just-in-time basis and that it is the application's
		///   responsibility to call Close/Dispose on that stream.
		/// </para>
		///
		/// </remarks>
		/// <seealso cref="P:Ionic.Zip.ZipEntry.InputStream" />
		public bool InputStreamWasJitProvided => _sourceWasJitProvided;

		/// <summary>
		/// An enum indicating the source of the ZipEntry.
		/// </summary>
		public ZipEntrySource Source => _Source;

		/// <summary>
		/// The version of the zip engine needed to read the ZipEntry.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This is a readonly property, indicating the version of <a href="http://www.pkware.com/documents/casestudies/APPNOTE.TXT">the Zip
		///   specification</a> that the extracting tool or library must support to
		///   extract the given entry.  Generally higher versions indicate newer
		///   features.  Older zip engines obviously won't know about new features, and
		///   won't be able to extract entries that depend on those newer features.
		/// </para>
		///
		/// <list type="table">
		/// <listheader>
		/// <term>value</term>
		/// <description>Features</description>
		/// </listheader>
		///
		/// <item>
		/// <term>20</term>
		/// <description>a basic Zip Entry, potentially using PKZIP encryption.
		/// </description>
		/// </item>
		///
		/// <item>
		/// <term>45</term>
		/// <description>The ZIP64 extension is used on the entry.
		/// </description>
		/// </item>
		///
		/// <item>
		/// <term>46</term>
		/// <description> File is compressed using BZIP2 compression*</description>
		/// </item>
		///
		/// <item>
		/// <term>50</term>
		/// <description> File is encrypted using PkWare's DES, 3DES, (broken) RC2 or RC4</description>
		/// </item>
		///
		/// <item>
		/// <term>51</term>
		/// <description> File is encrypted using PKWare's AES encryption or corrected RC2 encryption.</description>
		/// </item>
		///
		/// <item>
		/// <term>52</term>
		/// <description> File is encrypted using corrected RC2-64 encryption**</description>
		/// </item>
		///
		/// <item>
		/// <term>61</term>
		/// <description> File is encrypted using non-OAEP key wrapping***</description>
		/// </item>
		///
		/// <item>
		/// <term>63</term>
		/// <description> File is compressed using LZMA, PPMd+, Blowfish, or Twofish</description>
		/// </item>
		///
		/// </list>
		///
		/// <para>
		///   There are other values possible, not listed here. DotNetZip supports
		///   regular PKZip encryption, and ZIP64 extensions.  DotNetZip cannot extract
		///   entries that require a zip engine higher than 45.
		/// </para>
		///
		/// <para>
		///   This value is set upon reading an existing zip file, or after saving a zip
		///   archive.
		/// </para>
		/// </remarks>
		public short VersionNeeded => _VersionNeeded;

		/// <summary>
		/// The comment attached to the ZipEntry.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   Each entry in a zip file can optionally have a comment associated to
		///   it. The comment might be displayed by a zip tool during extraction, for
		///   example.
		/// </para>
		///
		/// <para>
		///   By default, the <c>Comment</c> is encoded in IBM437 code page. You can
		///   specify an alternative with <see cref="P:Ionic.Zip.ZipEntry.AlternateEncoding" /> and
		///  <see cref="P:Ionic.Zip.ZipEntry.AlternateEncodingUsage" />.
		/// </para>
		/// </remarks>
		/// <seealso cref="P:Ionic.Zip.ZipEntry.AlternateEncoding" />
		/// <seealso cref="P:Ionic.Zip.ZipEntry.AlternateEncodingUsage" />
		public string Comment
		{
			get
			{
				return _Comment;
			}
			set
			{
				_Comment = value;
				_metadataChanged = true;
			}
		}

		/// <summary>
		/// Indicates whether the entry requires ZIP64 extensions.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   This property is null (Nothing in VB) until a <c>Save()</c> method on the
		///   containing <see cref="T:Ionic.Zip.ZipFile" /> instance has been called. The property is
		///   non-null (<c>HasValue</c> is true) only after a <c>Save()</c> method has
		///   been called.
		/// </para>
		///
		/// <para>
		///   After the containing <c>ZipFile</c> has been saved, the Value of this
		///   property is true if any of the following three conditions holds: the
		///   uncompressed size of the entry is larger than 0xFFFFFFFF; the compressed
		///   size of the entry is larger than 0xFFFFFFFF; the relative offset of the
		///   entry within the zip archive is larger than 0xFFFFFFFF.  These quantities
		///   are not known until a <c>Save()</c> is attempted on the zip archive and
		///   the compression is applied.
		/// </para>
		///
		/// <para>
		///   If none of the three conditions holds, then the <c>Value</c> is false.
		/// </para>
		///
		/// <para>
		///   A <c>Value</c> of false does not indicate that the entry, as saved in the
		///   zip archive, does not use ZIP64.  It merely indicates that ZIP64 is
		///   <em>not required</em>.  An entry may use ZIP64 even when not required if
		///   the <see cref="P:Ionic.Zip.ZipFile.UseZip64WhenSaving" /> property on the containing
		///   <c>ZipFile</c> instance is set to <see cref="F:Ionic.Zip.Zip64Option.Always" />, or if
		///   the <see cref="P:Ionic.Zip.ZipFile.UseZip64WhenSaving" /> property on the containing
		///   <c>ZipFile</c> instance is set to <see cref="F:Ionic.Zip.Zip64Option.AsNecessary" />
		///   and the output stream was not seekable.
		/// </para>
		///
		/// </remarks>
		/// <seealso cref="P:Ionic.Zip.ZipEntry.OutputUsedZip64" />
		public bool? RequiresZip64 => _entryRequiresZip64;

		/// <summary>
		///   Indicates whether the entry actually used ZIP64 extensions, as it was most
		///   recently written to the output file or stream.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   This Nullable property is null (Nothing in VB) until a <c>Save()</c>
		///   method on the containing <see cref="T:Ionic.Zip.ZipFile" /> instance has been
		///   called. <c>HasValue</c> is true only after a <c>Save()</c> method has been
		///   called.
		/// </para>
		///
		/// <para>
		///   The value of this property for a particular <c>ZipEntry</c> may change
		///   over successive calls to <c>Save()</c> methods on the containing ZipFile,
		///   even if the file that corresponds to the <c>ZipEntry</c> does not. This
		///   may happen if other entries contained in the <c>ZipFile</c> expand,
		///   causing the offset for this particular entry to exceed 0xFFFFFFFF.
		/// </para>
		/// </remarks>
		/// <seealso cref="P:Ionic.Zip.ZipEntry.RequiresZip64" />
		public bool? OutputUsedZip64 => _OutputUsesZip64;

		/// <summary>
		///   The bitfield for the entry as defined in the zip spec. You probably
		///   never need to look at this.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   You probably do not need to concern yourself with the contents of this
		///   property, but in case you do:
		/// </para>
		///
		/// <list type="table">
		/// <listheader>
		/// <term>bit</term>
		/// <description>meaning</description>
		/// </listheader>
		///
		/// <item>
		/// <term>0</term>
		/// <description>set if encryption is used.</description>
		/// </item>
		///
		/// <item>
		/// <term>1-2</term>
		/// <description>
		/// set to determine whether normal, max, fast deflation.  DotNetZip library
		/// always leaves these bits unset when writing (indicating "normal"
		/// deflation"), but can read an entry with any value here.
		/// </description>
		/// </item>
		///
		/// <item>
		/// <term>3</term>
		/// <description>
		/// Indicates that the Crc32, Compressed and Uncompressed sizes are zero in the
		/// local header.  This bit gets set on an entry during writing a zip file, when
		/// it is saved to a non-seekable output stream.
		/// </description>
		/// </item>
		///
		///
		/// <item>
		/// <term>4</term>
		/// <description>reserved for "enhanced deflating". This library doesn't do enhanced deflating.</description>
		/// </item>
		///
		/// <item>
		/// <term>5</term>
		/// <description>set to indicate the zip is compressed patched data.  This library doesn't do that.</description>
		/// </item>
		///
		/// <item>
		/// <term>6</term>
		/// <description>
		/// set if PKWare's strong encryption is used (must also set bit 1 if bit 6 is
		/// set). This bit is not set if WinZip's AES encryption is set.</description>
		/// </item>
		///
		/// <item>
		/// <term>7</term>
		/// <description>not used</description>
		/// </item>
		///
		/// <item>
		/// <term>8</term>
		/// <description>not used</description>
		/// </item>
		///
		/// <item>
		/// <term>9</term>
		/// <description>not used</description>
		/// </item>
		///
		/// <item>
		/// <term>10</term>
		/// <description>not used</description>
		/// </item>
		///
		/// <item>
		/// <term>11</term>
		/// <description>
		/// Language encoding flag (EFS).  If this bit is set, the filename and comment
		/// fields for this file must be encoded using UTF-8. This library currently
		/// does not support UTF-8.
		/// </description>
		/// </item>
		///
		/// <item>
		/// <term>12</term>
		/// <description>Reserved by PKWARE for enhanced compression.</description>
		/// </item>
		///
		/// <item>
		/// <term>13</term>
		/// <description>
		///   Used when encrypting the Central Directory to indicate selected data
		///   values in the Local Header are masked to hide their actual values.  See
		///   the section in <a href="http://www.pkware.com/documents/casestudies/APPNOTE.TXT">the Zip
		///   specification</a> describing the Strong Encryption Specification for
		///   details.
		/// </description>
		/// </item>
		///
		/// <item>
		/// <term>14</term>
		/// <description>Reserved by PKWARE.</description>
		/// </item>
		///
		/// <item>
		/// <term>15</term>
		/// <description>Reserved by PKWARE.</description>
		/// </item>
		///
		/// </list>
		///
		/// </remarks>
		public short BitField => _BitField;

		/// <summary>
		///   The compression method employed for this ZipEntry.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   <see href="http://www.pkware.com/documents/casestudies/APPNOTE.TXT">The
		///   Zip specification</see> allows a variety of compression methods.  This
		///   library supports just two: 0x08 = Deflate.  0x00 = Store (no compression),
		///   for reading or writing.
		/// </para>
		///
		/// <para>
		///   When reading an entry from an existing zipfile, the value you retrieve
		///   here indicates the compression method used on the entry by the original
		///   creator of the zip.  When writing a zipfile, you can specify either 0x08
		///   (Deflate) or 0x00 (None).  If you try setting something else, you will get
		///   an exception.
		/// </para>
		///
		/// <para>
		///   You may wish to set <c>CompressionMethod</c> to <c>CompressionMethod.None</c> (0)
		///   when zipping already-compressed data like a jpg, png, or mp3 file.
		///   This can save time and cpu cycles.
		/// </para>
		///
		/// <para>
		///   When setting this property on a <c>ZipEntry</c> that is read from an
		///   existing zip file, calling <c>ZipFile.Save()</c> will cause the new
		///   CompressionMethod to be used on the entry in the newly saved zip file.
		/// </para>
		///
		/// <para>
		///   Setting this property may have the side effect of modifying the
		///   <c>CompressionLevel</c> property. If you set the <c>CompressionMethod</c> to a
		///   value other than <c>None</c>, and <c>CompressionLevel</c> is previously
		///   set to <c>None</c>, then <c>CompressionLevel</c> will be set to
		///   <c>Default</c>.
		/// </para>
		/// </remarks>
		///
		/// <seealso cref="P:Ionic.Zip.ZipEntry.CompressionMethod" />
		///
		/// <example>
		///   In this example, the first entry added to the zip archive uses the default
		///   behavior - compression is used where it makes sense.  The second entry,
		///   the MP3 file, is added to the archive without being compressed.
		/// <code>
		/// using (ZipFile zip = new ZipFile(ZipFileToCreate))
		/// {
		///   ZipEntry e1= zip.AddFile(@"notes\Readme.txt");
		///   ZipEntry e2= zip.AddFile(@"music\StopThisTrain.mp3");
		///   e2.CompressionMethod = CompressionMethod.None;
		///   zip.Save();
		/// }
		/// </code>
		///
		/// <code lang="VB">
		/// Using zip As New ZipFile(ZipFileToCreate)
		///   zip.AddFile("notes\Readme.txt")
		///   Dim e2 as ZipEntry = zip.AddFile("music\StopThisTrain.mp3")
		///   e2.CompressionMethod = CompressionMethod.None
		///   zip.Save
		/// End Using
		/// </code>
		/// </example>
		public CompressionMethod CompressionMethod
		{
			get
			{
				return (CompressionMethod)_CompressionMethod;
			}
			set
			{
				if (value != (CompressionMethod)_CompressionMethod)
				{
					if (value != 0 && value != CompressionMethod.Deflate && value != CompressionMethod.BZip2)
					{
						throw new InvalidOperationException("Unsupported compression method.");
					}
					_CompressionMethod = (short)value;
					if (_CompressionMethod == 0)
					{
						_CompressionLevel = CompressionLevel.None;
					}
					else if (CompressionLevel == CompressionLevel.None)
					{
						_CompressionLevel = CompressionLevel.Default;
					}
					if (_container.ZipFile != null)
					{
						_container.ZipFile.NotifyEntryChanged();
					}
					_restreamRequiredOnSave = true;
				}
			}
		}

		/// <summary>
		///   Sets the compression level to be used for the entry when saving the zip
		///   archive. This applies only for CompressionMethod = DEFLATE.
		/// </summary>
		///
		/// <remarks>
		///  <para>
		///    When using the DEFLATE compression method, Varying the compression
		///    level used on entries can affect the size-vs-speed tradeoff when
		///    compression and decompressing data streams or files.
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
		///
		///  <para>
		///    When setting this property on a <c>ZipEntry</c> that is read from an
		///    existing zip file, calling <c>ZipFile.Save()</c> will cause the new
		///    <c>CompressionLevel</c> to be used on the entry in the newly saved zip file.
		///  </para>
		///
		///  <para>
		///    Setting this property may have the side effect of modifying the
		///    <c>CompressionMethod</c> property. If you set the <c>CompressionLevel</c>
		///    to a value other than <c>None</c>, <c>CompressionMethod</c> will be set
		///    to <c>Deflate</c>, if it was previously <c>None</c>.
		///  </para>
		///
		///  <para>
		///    Setting this property has no effect if the <c>CompressionMethod</c> is something
		///    other than <c>Deflate</c> or <c>None</c>.
		///  </para>
		/// </remarks>
		///
		/// <seealso cref="P:Ionic.Zip.ZipEntry.CompressionMethod" />
		public CompressionLevel CompressionLevel
		{
			get
			{
				return _CompressionLevel;
			}
			set
			{
				if ((_CompressionMethod != 8 && _CompressionMethod != 0) || (value == CompressionLevel.Default && _CompressionMethod == 8))
				{
					return;
				}
				_CompressionLevel = value;
				if (value != 0 || _CompressionMethod != 0)
				{
					if (_CompressionLevel == CompressionLevel.None)
					{
						_CompressionMethod = 0;
					}
					else
					{
						_CompressionMethod = 8;
					}
					if (_container.ZipFile != null)
					{
						_container.ZipFile.NotifyEntryChanged();
					}
					_restreamRequiredOnSave = true;
				}
			}
		}

		/// <summary>
		///   The compressed size of the file, in bytes, within the zip archive.
		/// </summary>
		///
		/// <remarks>
		///   When reading a <c>ZipFile</c>, this value is read in from the existing
		///   zip file. When creating or updating a <c>ZipFile</c>, the compressed
		///   size is computed during compression.  Therefore the value on a
		///   <c>ZipEntry</c> is valid after a call to <c>Save()</c> (or one of its
		///   overloads) in that case.
		/// </remarks>
		///
		/// <seealso cref="P:Ionic.Zip.ZipEntry.UncompressedSize" />
		public long CompressedSize => _CompressedSize;

		/// <summary>
		///   The size of the file, in bytes, before compression, or after extraction.
		/// </summary>
		///
		/// <remarks>
		///   When reading a <c>ZipFile</c>, this value is read in from the existing
		///   zip file. When creating or updating a <c>ZipFile</c>, the uncompressed
		///   size is computed during compression.  Therefore the value on a
		///   <c>ZipEntry</c> is valid after a call to <c>Save()</c> (or one of its
		///   overloads) in that case.
		/// </remarks>
		///
		/// <seealso cref="P:Ionic.Zip.ZipEntry.CompressedSize" />
		public long UncompressedSize => _UncompressedSize;

		/// <summary>
		/// The ratio of compressed size to uncompressed size of the ZipEntry.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This is a ratio of the compressed size to the uncompressed size of the
		///   entry, expressed as a double in the range of 0 to 100+. A value of 100
		///   indicates no compression at all.  It could be higher than 100 when the
		///   compression algorithm actually inflates the data, as may occur for small
		///   files, or uncompressible data that is encrypted.
		/// </para>
		///
		/// <para>
		///   You could format it for presentation to a user via a format string of
		///   "{3,5:F0}%" to see it as a percentage.
		/// </para>
		///
		/// <para>
		///   If the size of the original uncompressed file is 0, implying a
		///   denominator of 0, the return value will be zero.
		/// </para>
		///
		/// <para>
		///   This property is valid after reading in an existing zip file, or after
		///   saving the <c>ZipFile</c> that contains the ZipEntry. You cannot know the
		///   effect of a compression transform until you try it.
		/// </para>
		///
		/// </remarks>
		public double CompressionRatio
		{
			get
			{
				if (UncompressedSize == 0L)
				{
					return 0.0;
				}
				return 100.0 * (1.0 - 1.0 * (double)CompressedSize / (1.0 * (double)UncompressedSize));
			}
		}

		/// <summary>
		/// The 32-bit CRC (Cyclic Redundancy Check) on the contents of the ZipEntry.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para> You probably don't need to concern yourself with this. It is used
		/// internally by DotNetZip to verify files or streams upon extraction.  </para>
		///
		/// <para> The value is a <see href="http://en.wikipedia.org/wiki/CRC32">32-bit
		/// CRC</see> using 0xEDB88320 for the polynomial. This is the same CRC-32 used in
		/// PNG, MPEG-2, and other protocols and formats.  It is a read-only property; when
		/// creating a Zip archive, the CRC for each entry is set only after a call to
		/// <c>Save()</c> on the containing ZipFile. When reading an existing zip file, the value
		/// of this property reflects the stored CRC for the entry.  </para>
		///
		/// </remarks>
		public int Crc => _Crc32;

		/// <summary>
		/// True if the entry is a directory (not a file).
		/// This is a readonly property on the entry.
		/// </summary>
		public bool IsDirectory => _IsDirectory;

		/// <summary>
		/// A derived property that is <c>true</c> if the entry uses encryption.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This is a readonly property on the entry.  When reading a zip file,
		///   the value for the <c>ZipEntry</c> is determined by the data read
		///   from the zip file.  After saving a ZipFile, the value of this
		///   property for each <c>ZipEntry</c> indicates whether encryption was
		///   actually used (which will have been true if the <see cref="P:Ionic.Zip.ZipEntry.Password" /> was set and the <see cref="P:Ionic.Zip.ZipEntry.Encryption" /> property
		///   was something other than <see cref="F:Ionic.Zip.EncryptionAlgorithm.None" />.
		/// </para>
		/// </remarks>
		public bool UsesEncryption => _Encryption_FromZipFile != EncryptionAlgorithm.None;

		/// <summary>
		///   Set this to specify which encryption algorithm to use for the entry when
		///   saving it to a zip archive.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   Set this property in order to encrypt the entry when the <c>ZipFile</c> is
		///   saved. When setting this property, you must also set a <see cref="P:Ionic.Zip.ZipEntry.Password" /> on the entry.  If you set a value other than <see cref="F:Ionic.Zip.EncryptionAlgorithm.None" /> on this property and do not set a
		///   <c>Password</c> then the entry will not be encrypted. The <c>ZipEntry</c>
		///   data is encrypted as the <c>ZipFile</c> is saved, when you call <see cref="M:Ionic.Zip.ZipFile.Save" /> or one of its cousins on the containing
		///   <c>ZipFile</c> instance. You do not need to specify the <c>Encryption</c>
		///   when extracting entries from an archive.
		/// </para>
		///
		/// <para>
		///   The Zip specification from PKWare defines a set of encryption algorithms,
		///   and the data formats for the zip archive that support them, and PKWare
		///   supports those algorithms in the tools it produces. Other vendors of tools
		///   and libraries, such as WinZip or Xceed, typically support <em>a
		///   subset</em> of the algorithms specified by PKWare. These tools can
		///   sometimes support additional different encryption algorithms and data
		///   formats, not specified by PKWare. The AES Encryption specified and
		///   supported by WinZip is the most popular example. This library supports a
		///   subset of the complete set of algorithms specified by PKWare and other
		///   vendors.
		/// </para>
		///
		/// <para>
		///   There is no common, ubiquitous multi-vendor standard for strong encryption
		///   within zip files. There is broad support for so-called "traditional" Zip
		///   encryption, sometimes called Zip 2.0 encryption, as <see href="http://www.pkware.com/documents/casestudies/APPNOTE.TXT">specified
		///   by PKWare</see>, but this encryption is considered weak and
		///   breakable. This library currently supports the Zip 2.0 "weak" encryption,
		///   and also a stronger WinZip-compatible AES encryption, using either 128-bit
		///   or 256-bit key strength. If you want DotNetZip to support an algorithm
		///   that is not currently supported, call the author of this library and maybe
		///   we can talk business.
		/// </para>
		///
		/// <para>
		///   The <see cref="T:Ionic.Zip.ZipFile" /> class also has a <see cref="P:Ionic.Zip.ZipFile.Encryption" /> property.  In most cases you will use
		///   <em>that</em> property when setting encryption. This property takes
		///   precedence over any <c>Encryption</c> set on the <c>ZipFile</c> itself.
		///   Typically, you would use the per-entry Encryption when most entries in the
		///   zip archive use one encryption algorithm, and a few entries use a
		///   different one.  If all entries in the zip file use the same Encryption,
		///   then it is simpler to just set this property on the ZipFile itself, when
		///   creating a zip archive.
		/// </para>
		///
		/// <para>
		///   Some comments on updating archives: If you read a <c>ZipFile</c>, you can
		///   modify the Encryption on an encrypted entry: you can remove encryption
		///   from an entry that was encrypted; you can encrypt an entry that was not
		///   encrypted previously; or, you can change the encryption algorithm.  The
		///   changes in encryption are not made permanent until you call Save() on the
		///   <c>ZipFile</c>.  To effect changes in encryption, the entry content is
		///   streamed through several transformations, depending on the modification
		///   the application has requested. For example if the entry is not encrypted
		///   and the application sets <c>Encryption</c> to <c>PkzipWeak</c>, then at
		///   the time of <c>Save()</c>, the original entry is read and decompressed,
		///   then re-compressed and encrypted.  Conversely, if the original entry is
		///   encrypted with <c>PkzipWeak</c> encryption, and the application sets the
		///   <c>Encryption</c> property to <c>WinZipAes128</c>, then at the time of
		///   <c>Save()</c>, the original entry is decrypted via PKZIP encryption and
		///   decompressed, then re-compressed and re-encrypted with AES.  This all
		///   happens automatically within the library, but it can be time-consuming for
		///   large entries.
		/// </para>
		///
		/// <para>
		///   Additionally, when updating archives, it is not possible to change the
		///   password when changing the encryption algorithm.  To change both the
		///   algorithm and the password, you need to Save() the zipfile twice.  First
		///   set the <c>Encryption</c> to None, then call <c>Save()</c>.  Then set the
		///   <c>Encryption</c> to the new value (not "None"), then call <c>Save()</c>
		///   once again.
		/// </para>
		///
		/// <para>
		///   The WinZip AES encryption algorithms are not supported on the .NET Compact
		///   Framework.
		/// </para>
		/// </remarks>
		///
		/// <example>
		/// <para>
		///   This example creates a zip archive that uses encryption, and then extracts
		///   entries from the archive.  When creating the zip archive, the ReadMe.txt
		///   file is zipped without using a password or encryption.  The other file
		///   uses encryption.
		/// </para>
		/// <code>
		/// // Create a zip archive with AES Encryption.
		/// using (ZipFile zip = new ZipFile())
		/// {
		///     zip.AddFile("ReadMe.txt")
		///     ZipEntry e1= zip.AddFile("2008-Regional-Sales-Report.pdf");
		///     e1.Encryption= EncryptionAlgorithm.WinZipAes256;
		///     e1.Password= "Top.Secret.No.Peeking!";
		///     zip.Save("EncryptedArchive.zip");
		/// }
		///
		/// // Extract a zip archive that uses AES Encryption.
		/// // You do not need to specify the algorithm during extraction.
		/// using (ZipFile zip = ZipFile.Read("EncryptedArchive.zip"))
		/// {
		///     // Specify the password that is used during extraction, for
		///     // all entries that require a password:
		///     zip.Password= "Top.Secret.No.Peeking!";
		///     zip.ExtractAll("extractDirectory");
		/// }
		/// </code>
		///
		/// <code lang="VB">
		/// ' Create a zip that uses Encryption.
		/// Using zip As New ZipFile()
		///     zip.AddFile("ReadMe.txt")
		///     Dim e1 as ZipEntry
		///     e1= zip.AddFile("2008-Regional-Sales-Report.pdf")
		///     e1.Encryption= EncryptionAlgorithm.WinZipAes256
		///     e1.Password= "Top.Secret.No.Peeking!"
		///     zip.Save("EncryptedArchive.zip")
		/// End Using
		///
		/// ' Extract a zip archive that uses AES Encryption.
		/// ' You do not need to specify the algorithm during extraction.
		/// Using (zip as ZipFile = ZipFile.Read("EncryptedArchive.zip"))
		///     ' Specify the password that is used during extraction, for
		///     ' all entries that require a password:
		///     zip.Password= "Top.Secret.No.Peeking!"
		///     zip.ExtractAll("extractDirectory")
		/// End Using
		/// </code>
		///
		/// </example>
		///
		/// <exception cref="T:System.InvalidOperationException">
		/// Thrown in the setter if EncryptionAlgorithm.Unsupported is specified.
		/// </exception>
		///
		/// <seealso cref="P:Ionic.Zip.ZipEntry.Password">ZipEntry.Password</seealso>
		/// <seealso cref="P:Ionic.Zip.ZipFile.Encryption">ZipFile.Encryption</seealso>
		public EncryptionAlgorithm Encryption
		{
			get
			{
				return _Encryption;
			}
			set
			{
				if (value != _Encryption)
				{
					if (value == EncryptionAlgorithm.Unsupported)
					{
						throw new InvalidOperationException("You may not set Encryption to that value.");
					}
					_Encryption = value;
					_restreamRequiredOnSave = true;
					if (_container.ZipFile != null)
					{
						_container.ZipFile.NotifyEntryChanged();
					}
				}
			}
		}

		/// <summary>
		/// The Password to be used when encrypting a <c>ZipEntry</c> upon
		/// <c>ZipFile.Save()</c>, or when decrypting an entry upon Extract().
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This is a write-only property on the entry. Set this to request that the
		///   entry be encrypted when writing the zip archive, or set it to specify the
		///   password to be used when extracting an existing entry that is encrypted.
		/// </para>
		///
		/// <para>
		///   The password set here is implicitly used to encrypt the entry during the
		///   <see cref="M:Ionic.Zip.ZipFile.Save" /> operation, or to decrypt during the <see cref="M:Ionic.Zip.ZipEntry.Extract" /> or <see cref="M:Ionic.Zip.ZipEntry.OpenReader" /> operation.  If you set
		///   the Password on a <c>ZipEntry</c> after calling <c>Save()</c>, there is no
		///   effect.
		/// </para>
		///
		/// <para>
		///   Consider setting the <see cref="P:Ionic.Zip.ZipEntry.Encryption" /> property when using a
		///   password. Answering concerns that the standard password protection
		///   supported by all zip tools is weak, WinZip has extended the ZIP
		///   specification with a way to use AES Encryption to protect entries in the
		///   Zip file. Unlike the "PKZIP 2.0" encryption specified in the PKZIP
		///   specification, <see href="http://en.wikipedia.org/wiki/Advanced_Encryption_Standard">AES
		///   Encryption</see> uses a standard, strong, tested, encryption
		///   algorithm. DotNetZip can create zip archives that use WinZip-compatible
		///   AES encryption, if you set the <see cref="P:Ionic.Zip.ZipEntry.Encryption" /> property. But,
		///   archives created that use AES encryption may not be readable by all other
		///   tools and libraries. For example, Windows Explorer cannot read a
		///   "compressed folder" (a zip file) that uses AES encryption, though it can
		///   read a zip file that uses "PKZIP encryption."
		/// </para>
		///
		/// <para>
		///   The <see cref="T:Ionic.Zip.ZipFile" /> class also has a <see cref="P:Ionic.Zip.ZipFile.Password" />
		///   property.  This property takes precedence over any password set on the
		///   ZipFile itself.  Typically, you would use the per-entry Password when most
		///   entries in the zip archive use one password, and a few entries use a
		///   different password.  If all entries in the zip file use the same password,
		///   then it is simpler to just set this property on the ZipFile itself,
		///   whether creating a zip archive or extracting a zip archive.
		/// </para>
		///
		/// <para>
		///   Some comments on updating archives: If you read a <c>ZipFile</c>, you
		///   cannot modify the password on any encrypted entry, except by extracting
		///   the entry with the original password (if any), removing the original entry
		///   via <see cref="M:Ionic.Zip.ZipFile.RemoveEntry(Ionic.Zip.ZipEntry)" />, and then adding a new
		///   entry with a new Password.
		/// </para>
		///
		/// <para>
		///   For example, suppose you read a <c>ZipFile</c>, and there is an encrypted
		///   entry.  Setting the Password property on that <c>ZipEntry</c> and then
		///   calling <c>Save()</c> on the <c>ZipFile</c> does not update the password
		///   on that entry in the archive.  Neither is an exception thrown. Instead,
		///   what happens during the <c>Save()</c> is the existing entry is copied
		///   through to the new zip archive, in its original encrypted form. Upon
		///   re-reading that archive, the entry can be decrypted with its original
		///   password.
		/// </para>
		///
		/// <para>
		///   If you read a ZipFile, and there is an un-encrypted entry, you can set the
		///   <c>Password</c> on the entry and then call Save() on the ZipFile, and get
		///   encryption on that entry.
		/// </para>
		///
		/// </remarks>
		///
		/// <example>
		/// <para>
		///   This example creates a zip file with two entries, and then extracts the
		///   entries from the zip file.  When creating the zip file, the two files are
		///   added to the zip file using password protection. Each entry uses a
		///   different password.  During extraction, each file is extracted with the
		///   appropriate password.
		/// </para>
		/// <code>
		/// // create a file with encryption
		/// using (ZipFile zip = new ZipFile())
		/// {
		///     ZipEntry entry;
		///     entry= zip.AddFile("Declaration.txt");
		///     entry.Password= "123456!";
		///     entry = zip.AddFile("Report.xls");
		///     entry.Password= "1Secret!";
		///     zip.Save("EncryptedArchive.zip");
		/// }
		///
		/// // extract entries that use encryption
		/// using (ZipFile zip = ZipFile.Read("EncryptedArchive.zip"))
		/// {
		///     ZipEntry entry;
		///     entry = zip["Declaration.txt"];
		///     entry.Password = "123456!";
		///     entry.Extract("extractDir");
		///     entry = zip["Report.xls"];
		///     entry.Password = "1Secret!";
		///     entry.Extract("extractDir");
		/// }
		///
		/// </code>
		///
		/// <code lang="VB">
		/// Using zip As New ZipFile
		///     Dim entry as ZipEntry
		///     entry= zip.AddFile("Declaration.txt")
		///     entry.Password= "123456!"
		///     entry = zip.AddFile("Report.xls")
		///     entry.Password= "1Secret!"
		///     zip.Save("EncryptedArchive.zip")
		/// End Using
		///
		///
		/// ' extract entries that use encryption
		/// Using (zip as ZipFile = ZipFile.Read("EncryptedArchive.zip"))
		///     Dim entry as ZipEntry
		///     entry = zip("Declaration.txt")
		///     entry.Password = "123456!"
		///     entry.Extract("extractDir")
		///     entry = zip("Report.xls")
		///     entry.Password = "1Secret!"
		///     entry.Extract("extractDir")
		/// End Using
		///
		/// </code>
		///
		/// </example>
		///
		/// <seealso cref="P:Ionic.Zip.ZipEntry.Encryption" />
		/// <seealso cref="P:Ionic.Zip.ZipFile.Password">ZipFile.Password</seealso>
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
					_Encryption = EncryptionAlgorithm.None;
					return;
				}
				if (_Source == ZipEntrySource.ZipFile && !_sourceIsEncrypted)
				{
					_restreamRequiredOnSave = true;
				}
				if (Encryption == EncryptionAlgorithm.None)
				{
					_Encryption = EncryptionAlgorithm.PkzipWeak;
				}
			}
		}

		internal bool IsChanged => _restreamRequiredOnSave | _metadataChanged;

		/// <summary>
		/// The action the library should take when extracting a file that already exists.
		/// </summary>
		///
		/// <remarks>
		///   <para>
		///     This property affects the behavior of the Extract methods (one of the
		///     <c>Extract()</c> or <c>ExtractWithPassword()</c> overloads), when
		///     extraction would would overwrite an existing filesystem file. If you do
		///     not set this property, the library throws an exception when extracting
		///     an entry would overwrite an existing file.
		///   </para>
		///
		///   <para>
		///     This property has no effect when extracting to a stream, or when the file to be
		///     extracted does not already exist.
		///   </para>
		///
		/// </remarks>
		/// <seealso cref="P:Ionic.Zip.ZipFile.ExtractExistingFile" />
		///
		/// <example>
		///   This example shows how to set the <c>ExtractExistingFile</c> property in
		///   an <c>ExtractProgress</c> event, in response to user input. The
		///   <c>ExtractProgress</c> event is invoked if and only if the
		///   <c>ExtractExistingFile</c> property was previously set to
		///   <c>ExtractExistingFileAction.InvokeExtractProgressEvent</c>.
		/// <code lang="C#">
		/// public static void ExtractProgress(object sender, ExtractProgressEventArgs e)
		/// {
		///     if (e.EventType == ZipProgressEventType.Extracting_BeforeExtractEntry)
		///         Console.WriteLine("extract {0} ", e.CurrentEntry.FileName);
		///
		///     else if (e.EventType == ZipProgressEventType.Extracting_ExtractEntryWouldOverwrite)
		///     {
		///         ZipEntry entry = e.CurrentEntry;
		///         string response = null;
		///         // Ask the user if he wants overwrite the file
		///         do
		///         {
		///             Console.Write("Overwrite {0} in {1} ? (y/n/C) ", entry.FileName, e.ExtractLocation);
		///             response = Console.ReadLine();
		///             Console.WriteLine();
		///
		///         } while (response != null &amp;&amp; response[0]!='Y' &amp;&amp;
		///                  response[0]!='N' &amp;&amp; response[0]!='C');
		///
		///         if  (response[0]=='C')
		///             e.Cancel = true;
		///         else if (response[0]=='Y')
		///             entry.ExtractExistingFile = ExtractExistingFileAction.OverwriteSilently;
		///         else
		///             entry.ExtractExistingFile= ExtractExistingFileAction.DoNotOverwrite;
		///     }
		/// }
		/// </code>
		/// </example>
		public ExtractExistingFileAction ExtractExistingFile { get; set; }

		/// <summary>
		///   The action to take when an error is encountered while
		///   opening or reading files as they are saved into a zip archive.
		/// </summary>
		///
		/// <remarks>
		///  <para>
		///     Errors can occur within a call to <see cref="M:Ionic.Zip.ZipFile.Save">ZipFile.Save</see>, as the various files contained
		///     in a ZipFile are being saved into the zip archive.  During the
		///     <c>Save</c>, DotNetZip will perform a <c>File.Open</c> on the file
		///     associated to the ZipEntry, and then will read the entire contents of
		///     the file as it is zipped. Either the open or the Read may fail, because
		///     of lock conflicts or other reasons.  Using this property, you can
		///     specify the action to take when such errors occur.
		///  </para>
		///
		///  <para>
		///     Typically you will NOT set this property on individual ZipEntry
		///     instances.  Instead, you will set the <see cref="P:Ionic.Zip.ZipFile.ZipErrorAction">ZipFile.ZipErrorAction</see> property on
		///     the ZipFile instance, before adding any entries to the
		///     <c>ZipFile</c>. If you do this, errors encountered on behalf of any of
		///     the entries in the ZipFile will be handled the same way.
		///  </para>
		///
		///  <para>
		///     But, if you use a <see cref="E:Ionic.Zip.ZipFile.ZipError" /> handler, you will want
		///     to set this property on the <c>ZipEntry</c> within the handler, to
		///     communicate back to DotNetZip what you would like to do with the
		///     particular error.
		///  </para>
		///
		/// </remarks>
		/// <seealso cref="P:Ionic.Zip.ZipFile.ZipErrorAction" />
		/// <seealso cref="E:Ionic.Zip.ZipFile.ZipError" />
		public ZipErrorAction ZipErrorAction { get; set; }

		/// <summary>
		/// Indicates whether the entry was included in the most recent save.
		/// </summary>
		/// <remarks>
		/// An entry can be excluded or skipped from a save if there is an error
		/// opening or reading the entry.
		/// </remarks>
		/// <seealso cref="P:Ionic.Zip.ZipEntry.ZipErrorAction" />
		public bool IncludedInMostRecentSave => !_skippedDuringSave;

		/// <summary>
		///   A callback that allows the application to specify the compression to use
		///   for a given entry that is about to be added to the zip archive.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   See <see cref="P:Ionic.Zip.ZipFile.SetCompression" />
		/// </para>
		/// </remarks>
		public SetCompressionCallback SetCompression { get; set; }

		/// <summary>
		///   Set to indicate whether to use UTF-8 encoding for filenames and comments.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   If this flag is set, the comment and filename for the entry will be
		///   encoded with UTF-8, as described in <see href="http://www.pkware.com/documents/casestudies/APPNOTE.TXT">the Zip
		///   specification</see>, if necessary. "Necessary" means, the filename or
		///   entry comment (if any) cannot be reflexively encoded and decoded using the
		///   default code page, IBM437.
		/// </para>
		///
		/// <para>
		///   Setting this flag to true is equivalent to setting <see cref="P:Ionic.Zip.ZipEntry.ProvisionalAlternateEncoding" /> to <c>System.Text.Encoding.UTF8</c>.
		/// </para>
		///
		/// <para>
		///   This flag has no effect or relation to the text encoding used within the
		///   file itself.
		/// </para>
		///
		/// </remarks>
		[Obsolete("Beginning with v1.9.1.6 of DotNetZip, this property is obsolete.  It will be removed in a future version of the library. Your applications should  use AlternateEncoding and AlternateEncodingUsage instead.")]
		public bool UseUnicodeAsNecessary
		{
			get
			{
				if (AlternateEncoding == Encoding.GetEncoding("UTF-8"))
				{
					return AlternateEncodingUsage == ZipOption.AsNecessary;
				}
				return false;
			}
			set
			{
				if (value)
				{
					AlternateEncoding = Encoding.GetEncoding("UTF-8");
					AlternateEncodingUsage = ZipOption.AsNecessary;
				}
				else
				{
					AlternateEncoding = ZipFile.DefaultEncoding;
					AlternateEncodingUsage = ZipOption.Default;
				}
			}
		}

		/// <summary>
		///   The text encoding to use for the FileName and Comment on this ZipEntry,
		///   when the default encoding is insufficient.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   Don't use this property.  See <see cref="P:Ionic.Zip.ZipEntry.AlternateEncoding" />.
		/// </para>
		///
		/// </remarks>
		[Obsolete("This property is obsolete since v1.9.1.6. Use AlternateEncoding and AlternateEncodingUsage instead.", true)]
		public Encoding ProvisionalAlternateEncoding { get; set; }

		/// <summary>
		///   Specifies the alternate text encoding used by this ZipEntry
		/// </summary>
		/// <remarks>
		///   <para>
		///     The default text encoding used in Zip files for encoding filenames and
		///     comments is IBM437, which is something like a superset of ASCII.  In
		///     cases where this is insufficient, applications can specify an
		///     alternate encoding.
		///   </para>
		///   <para>
		///     When creating a zip file, the usage of the alternate encoding is
		///     governed by the <see cref="P:Ionic.Zip.ZipEntry.AlternateEncodingUsage" /> property.
		///     Typically you would set both properties to tell DotNetZip to employ an
		///     encoding that is not IBM437 in the zipfile you are creating.
		///   </para>
		///   <para>
		///     Keep in mind that because the ZIP specification states that the only
		///     valid encodings to use are IBM437 and UTF-8, if you use something
		///     other than that, then zip tools and libraries may not be able to
		///     successfully read the zip archive you generate.
		///   </para>
		///   <para>
		///     The zip specification states that applications should presume that
		///     IBM437 is in use, except when a special bit is set, which indicates
		///     UTF-8. There is no way to specify an arbitrary code page, within the
		///     zip file itself. When you create a zip file encoded with gb2312 or
		///     ibm861 or anything other than IBM437 or UTF-8, then the application
		///     that reads the zip file needs to "know" which code page to use. In
		///     some cases, the code page used when reading is chosen implicitly. For
		///     example, WinRar uses the ambient code page for the host desktop
		///     operating system. The pitfall here is that if you create a zip in
		///     Copenhagen and send it to Tokyo, the reader of the zipfile may not be
		///     able to decode successfully.
		///   </para>
		/// </remarks>
		/// <example>
		///   This example shows how to create a zipfile encoded with a
		///   language-specific encoding:
		/// <code>
		///   using (var zip = new ZipFile())
		///   {
		///      zip.AlternateEnoding = System.Text.Encoding.GetEncoding("ibm861");
		///      zip.AlternateEnodingUsage = ZipOption.Always;
		///      zip.AddFileS(arrayOfFiles);
		///      zip.Save("Myarchive-Encoded-in-IBM861.zip");
		///   }
		/// </code>
		/// </example>
		/// <seealso cref="P:Ionic.Zip.ZipFile.AlternateEncodingUsage" />
		public Encoding AlternateEncoding { get; set; }

		/// <summary>
		///   Describes if and when this instance should apply
		///   AlternateEncoding to encode the FileName and Comment, when
		///   saving.
		/// </summary>
		/// <seealso cref="P:Ionic.Zip.ZipFile.AlternateEncoding" />
		public ZipOption AlternateEncodingUsage { get; set; }

		/// <summary>
		///   Indicates whether an entry is marked as a text file. Be careful when
		///   using on this property. Unless you have a good reason, you should
		///   probably ignore this property.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   The ZIP format includes a provision for specifying whether an entry in
		///   the zip archive is a text or binary file.  This property exposes that
		///   metadata item. Be careful when using this property: It's not clear
		///   that this property as a firm meaning, across tools and libraries.
		/// </para>
		///
		/// <para>
		///   To be clear, when reading a zip file, the property value may or may
		///   not be set, and its value may or may not be valid.  Not all entries
		///   that you may think of as "text" entries will be so marked, and entries
		///   marked as "text" are not guaranteed in any way to be text entries.
		///   Whether the value is set and set correctly depends entirely on the
		///   application that produced the zip file.
		/// </para>
		///
		/// <para>
		///   There are many zip tools available, and when creating zip files, some
		///   of them "respect" the IsText metadata field, and some of them do not.
		///   Unfortunately, even when an application tries to do "the right thing",
		///   it's not always clear what "the right thing" is.
		/// </para>
		///
		/// <para>
		///   There's no firm definition of just what it means to be "a text file",
		///   and the zip specification does not help in this regard. Twenty years
		///   ago, text was ASCII, each byte was less than 127. IsText meant, all
		///   bytes in the file were less than 127.  These days, it is not the case
		///   that all text files have all bytes less than 127.  Any unicode file
		///   may have bytes that are above 0x7f.  The zip specification has nothing
		///   to say on this topic. Therefore, it's not clear what IsText really
		///   means.
		/// </para>
		///
		/// <para>
		///   This property merely tells a reading application what is stored in the
		///   metadata for an entry, without guaranteeing its validity or its
		///   meaning.
		/// </para>
		///
		/// <para>
		///   When DotNetZip is used to create a zipfile, it attempts to set this
		///   field "correctly." For example, if a file ends in ".txt", this field
		///   will be set. Your application may override that default setting.  When
		///   writing a zip file, you must set the property before calling
		///   <c>Save()</c> on the ZipFile.
		/// </para>
		///
		/// <para>
		///   When reading a zip file, a more general way to decide just what kind
		///   of file is contained in a particular entry is to use the file type
		///   database stored in the operating system.  The operating system stores
		///   a table that says, a file with .jpg extension is a JPG image file, a
		///   file with a .xml extension is an XML document, a file with a .txt is a
		///   pure ASCII text document, and so on.  To get this information on
		///   Windows, <see href="http://www.codeproject.com/KB/cs/GetFileTypeAndIcon.aspx"> you
		///   need to read and parse the registry.</see> </para>
		/// </remarks>
		///
		/// <example>
		/// <code>
		/// using (var zip = new ZipFile())
		/// {
		///     var e = zip.UpdateFile("Descriptions.mme", "");
		///     e.IsText = true;
		///     zip.Save(zipPath);
		/// }
		/// </code>
		///
		/// <code lang="VB">
		/// Using zip As New ZipFile
		///     Dim e2 as ZipEntry = zip.AddFile("Descriptions.mme", "")
		///     e.IsText= True
		///     zip.Save(zipPath)
		/// End Using
		/// </code>
		/// </example>
		public bool IsText
		{
			get
			{
				return _IsText;
			}
			set
			{
				_IsText = value;
			}
		}

		internal Stream ArchiveStream
		{
			get
			{
				if (_archiveStream == null)
				{
					if (_container.ZipFile != null)
					{
						ZipFile zipFile = _container.ZipFile;
						zipFile.Reset(false);
						_archiveStream = zipFile.StreamForDiskNumber(_diskNumber);
					}
					else
					{
						_archiveStream = _container.ZipOutputStream.OutputStream;
					}
				}
				return _archiveStream;
			}
		}

		internal long FileDataPosition
		{
			get
			{
				if (__FileDataPosition == -1)
				{
					SetFdpLoh();
				}
				return __FileDataPosition;
			}
		}

		private int LengthOfHeader
		{
			get
			{
				if (_LengthOfHeader == 0)
				{
					SetFdpLoh();
				}
				return _LengthOfHeader;
			}
		}

		internal void ResetDirEntry()
		{
			__FileDataPosition = -1L;
			_LengthOfHeader = 0;
			CopyHelper.Reset();
		}

		/// <summary>
		///   Reads one entry from the zip directory structure in the zip file.
		/// </summary>
		///
		/// <param name="zf">
		///   The zipfile for which a directory entry will be read.  From this param, the
		///   method gets the ReadStream and the expected text encoding
		///   (ProvisionalAlternateEncoding) which is used if the entry is not marked
		///   UTF-8.
		/// </param>
		///
		/// <param name="previouslySeen">
		///   a list of previously seen entry names; used to prevent duplicates.
		/// </param>
		///
		/// <returns>the entry read from the archive.</returns>
		internal static ZipEntry ReadDirEntry(ZipFile zf, Dictionary<string, object> previouslySeen)
		{
			Stream readStream = zf.ReadStream;
			Encoding encoding = ((zf.AlternateEncodingUsage == ZipOption.Always) ? zf.AlternateEncoding : ZipFile.DefaultEncoding);
			ZipEntry zipEntry;
			do
			{
				int num = SharedUtilities.ReadSignature(readStream);
				if (IsNotValidZipDirEntrySig(num))
				{
					readStream.Seek(-4L, SeekOrigin.Current);
					if ((long)num != 101010256 && (long)num != 101075792 && num != 67324752)
					{
						throw new BadReadException($"  Bad signature (0x{num:X8}) at position 0x{readStream.Position:X8}");
					}
					return null;
				}
				int num2 = 46;
				byte[] array = new byte[42];
				int num3 = readStream.Read(array, 0, array.Length);
				if (num3 != array.Length)
				{
					return null;
				}
				int num4 = 0;
				zipEntry = new ZipEntry();
				zipEntry.AlternateEncoding = encoding;
				zipEntry._Source = ZipEntrySource.ZipFile;
				zipEntry._container = new ZipContainer(zf);
				zipEntry._VersionMadeBy = (short)(array[num4++] + array[num4++] * 256);
				zipEntry._VersionNeeded = (short)(array[num4++] + array[num4++] * 256);
				zipEntry._BitField = (short)(array[num4++] + array[num4++] * 256);
				zipEntry._CompressionMethod = (short)(array[num4++] + array[num4++] * 256);
				zipEntry._TimeBlob = array[num4++] + array[num4++] * 256 + array[num4++] * 256 * 256 + array[num4++] * 256 * 256 * 256;
				zipEntry._LastModified = SharedUtilities.PackedToDateTime(zipEntry._TimeBlob);
				zipEntry._timestamp |= ZipEntryTimestamp.DOS;
				zipEntry._Crc32 = array[num4++] + array[num4++] * 256 + array[num4++] * 256 * 256 + array[num4++] * 256 * 256 * 256;
				zipEntry._CompressedSize = (uint)(array[num4++] + array[num4++] * 256 + array[num4++] * 256 * 256 + array[num4++] * 256 * 256 * 256);
				zipEntry._UncompressedSize = (uint)(array[num4++] + array[num4++] * 256 + array[num4++] * 256 * 256 + array[num4++] * 256 * 256 * 256);
				zipEntry._CompressionMethod_FromZipFile = zipEntry._CompressionMethod;
				zipEntry._filenameLength = (short)(array[num4++] + array[num4++] * 256);
				zipEntry._extraFieldLength = (short)(array[num4++] + array[num4++] * 256);
				zipEntry._commentLength = (short)(array[num4++] + array[num4++] * 256);
				zipEntry._diskNumber = (uint)(array[num4++] + array[num4++] * 256);
				zipEntry._InternalFileAttrs = (short)(array[num4++] + array[num4++] * 256);
				zipEntry._ExternalFileAttrs = array[num4++] + array[num4++] * 256 + array[num4++] * 256 * 256 + array[num4++] * 256 * 256 * 256;
				zipEntry._RelativeOffsetOfLocalHeader = (uint)(array[num4++] + array[num4++] * 256 + array[num4++] * 256 * 256 + array[num4++] * 256 * 256 * 256);
				zipEntry.IsText = (zipEntry._InternalFileAttrs & 1) == 1;
				array = new byte[zipEntry._filenameLength];
				num3 = readStream.Read(array, 0, array.Length);
				num2 += num3;
				if ((zipEntry._BitField & 0x800) == 2048)
				{
					zipEntry._FileNameInArchive = SharedUtilities.Utf8StringFromBuffer(array);
				}
				else
				{
					zipEntry._FileNameInArchive = SharedUtilities.StringFromBuffer(array, encoding);
				}
				while (!zf.IgnoreDuplicateFiles && previouslySeen.ContainsKey(zipEntry._FileNameInArchive))
				{
					zipEntry._FileNameInArchive = CopyHelper.AppendCopyToFileName(zipEntry._FileNameInArchive);
					zipEntry._metadataChanged = true;
				}
				if (zipEntry.AttributesIndicateDirectory)
				{
					zipEntry.MarkAsDirectory();
				}
				else if (zipEntry._FileNameInArchive.EndsWith("/"))
				{
					zipEntry.MarkAsDirectory();
				}
				zipEntry._CompressedFileDataSize = zipEntry._CompressedSize;
				if ((zipEntry._BitField & 1) == 1)
				{
					zipEntry._Encryption_FromZipFile = (zipEntry._Encryption = EncryptionAlgorithm.PkzipWeak);
					zipEntry._sourceIsEncrypted = true;
				}
				if (zipEntry._extraFieldLength > 0)
				{
					zipEntry._InputUsesZip64 = zipEntry._CompressedSize == uint.MaxValue || zipEntry._UncompressedSize == uint.MaxValue || zipEntry._RelativeOffsetOfLocalHeader == uint.MaxValue;
					num2 += zipEntry.ProcessExtraField(readStream, zipEntry._extraFieldLength);
					zipEntry._CompressedFileDataSize = zipEntry._CompressedSize;
				}
				if (zipEntry._Encryption == EncryptionAlgorithm.PkzipWeak)
				{
					zipEntry._CompressedFileDataSize -= 12L;
				}
				else if (zipEntry.Encryption == EncryptionAlgorithm.WinZipAes128 || zipEntry.Encryption == EncryptionAlgorithm.WinZipAes256)
				{
					zipEntry._CompressedFileDataSize = zipEntry.CompressedSize - (GetLengthOfCryptoHeaderBytes(zipEntry.Encryption) + 10);
					zipEntry._LengthOfTrailer = 10;
				}
				if ((zipEntry._BitField & 8) == 8)
				{
					if (zipEntry._InputUsesZip64)
					{
						zipEntry._LengthOfTrailer += 24;
					}
					else
					{
						zipEntry._LengthOfTrailer += 16;
					}
				}
				zipEntry.AlternateEncoding = (((zipEntry._BitField & 0x800) == 2048) ? Encoding.UTF8 : encoding);
				zipEntry.AlternateEncodingUsage = ZipOption.Always;
				if (zipEntry._commentLength > 0)
				{
					array = new byte[zipEntry._commentLength];
					num3 = readStream.Read(array, 0, array.Length);
					num2 += num3;
					if ((zipEntry._BitField & 0x800) == 2048)
					{
						zipEntry._Comment = SharedUtilities.Utf8StringFromBuffer(array);
					}
					else
					{
						zipEntry._Comment = SharedUtilities.StringFromBuffer(array, encoding);
					}
				}
			}
			while (zf.IgnoreDuplicateFiles && previouslySeen.ContainsKey(zipEntry._FileNameInArchive));
			return zipEntry;
		}

		/// <summary>
		/// Returns true if the passed-in value is a valid signature for a ZipDirEntry.
		/// </summary>
		/// <param name="signature">the candidate 4-byte signature value.</param>
		/// <returns>true, if the signature is valid according to the PKWare spec.</returns>
		internal static bool IsNotValidZipDirEntrySig(int signature)
		{
			return signature != 33639248;
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <remarks>
		/// Applications should never need to call this directly.  It is exposed to
		/// support COM Automation environments.
		/// </remarks>
		public ZipEntry()
		{
			_CompressionMethod = 8;
			_CompressionLevel = CompressionLevel.Default;
			_Encryption = EncryptionAlgorithm.None;
			_Source = ZipEntrySource.None;
			AlternateEncoding = Encoding.GetEncoding("IBM437");
			AlternateEncodingUsage = ZipOption.Default;
		}

		/// <summary>
		///   Sets the NTFS Creation, Access, and Modified times for the given entry.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   When adding an entry from a file or directory, the Creation, Access, and
		///   Modified times for the given entry are automatically set from the
		///   filesystem values. When adding an entry from a stream or string, the
		///   values are implicitly set to DateTime.Now.  The application may wish to
		///   set these values to some arbitrary value, before saving the archive, and
		///   can do so using the various setters.  If you want to set all of the times,
		///   this method is more efficient.
		/// </para>
		///
		/// <para>
		///   The values you set here will be retrievable with the <see cref="P:Ionic.Zip.ZipEntry.ModifiedTime" />, <see cref="P:Ionic.Zip.ZipEntry.CreationTime" /> and <see cref="P:Ionic.Zip.ZipEntry.AccessedTime" /> properties.
		/// </para>
		///
		/// <para>
		///   When this method is called, if both <see cref="P:Ionic.Zip.ZipEntry.EmitTimesInWindowsFormatWhenSaving" /> and <see cref="P:Ionic.Zip.ZipEntry.EmitTimesInUnixFormatWhenSaving" /> are false, then the
		///   <c>EmitTimesInWindowsFormatWhenSaving</c> flag is automatically set.
		/// </para>
		///
		/// <para>
		///   DateTime values provided here without a DateTimeKind are assumed to be Local Time.
		/// </para>
		///
		/// </remarks>
		/// <param name="created">the creation time of the entry.</param>
		/// <param name="accessed">the last access time of the entry.</param>
		/// <param name="modified">the last modified time of the entry.</param>
		///
		/// <seealso cref="P:Ionic.Zip.ZipEntry.EmitTimesInWindowsFormatWhenSaving" />
		/// <seealso cref="P:Ionic.Zip.ZipEntry.EmitTimesInUnixFormatWhenSaving" />
		/// <seealso cref="P:Ionic.Zip.ZipEntry.AccessedTime" />
		/// <seealso cref="P:Ionic.Zip.ZipEntry.CreationTime" />
		/// <seealso cref="P:Ionic.Zip.ZipEntry.ModifiedTime" />
		public void SetEntryTimes(DateTime created, DateTime accessed, DateTime modified)
		{
			_ntfsTimesAreSet = true;
			if (created == _zeroHour && created.Kind == _zeroHour.Kind)
			{
				created = _win32Epoch;
			}
			if (accessed == _zeroHour && accessed.Kind == _zeroHour.Kind)
			{
				accessed = _win32Epoch;
			}
			if (modified == _zeroHour && modified.Kind == _zeroHour.Kind)
			{
				modified = _win32Epoch;
			}
			_Ctime = created.ToUniversalTime();
			_Atime = accessed.ToUniversalTime();
			_Mtime = modified.ToUniversalTime();
			_LastModified = _Mtime;
			if (!_emitUnixTimes && !_emitNtfsTimes)
			{
				_emitNtfsTimes = true;
			}
			_metadataChanged = true;
		}

		internal static string NameInArchive(string filename, string directoryPathInArchive)
		{
			string text = null;
			text = ((directoryPathInArchive == null) ? filename : ((!string.IsNullOrEmpty(directoryPathInArchive)) ? Path.Combine(directoryPathInArchive, Path.GetFileName(filename)) : Path.GetFileName(filename)));
			return SharedUtilities.NormalizePathForUseInZipFile(text);
		}

		internal static ZipEntry CreateFromNothing(string nameInArchive)
		{
			return Create(nameInArchive, ZipEntrySource.None, null, null);
		}

		internal static ZipEntry CreateFromFile(string filename, string nameInArchive)
		{
			return Create(nameInArchive, ZipEntrySource.FileSystem, filename, null);
		}

		internal static ZipEntry CreateForStream(string entryName, Stream s)
		{
			return Create(entryName, ZipEntrySource.Stream, s, null);
		}

		internal static ZipEntry CreateForWriter(string entryName, WriteDelegate d)
		{
			return Create(entryName, ZipEntrySource.WriteDelegate, d, null);
		}

		internal static ZipEntry CreateForJitStreamProvider(string nameInArchive, OpenDelegate opener, CloseDelegate closer)
		{
			return Create(nameInArchive, ZipEntrySource.JitStream, opener, closer);
		}

		internal static ZipEntry CreateForZipOutputStream(string nameInArchive)
		{
			return Create(nameInArchive, ZipEntrySource.ZipOutputStream, null, null);
		}

		private static ZipEntry Create(string nameInArchive, ZipEntrySource source, object arg1, object arg2)
		{
			if (string.IsNullOrEmpty(nameInArchive))
			{
				throw new ZipException("The entry name must be non-null and non-empty.");
			}
			ZipEntry zipEntry = new ZipEntry();
			zipEntry._VersionMadeBy = 45;
			zipEntry._Source = source;
			zipEntry._Mtime = (zipEntry._Atime = (zipEntry._Ctime = DateTime.UtcNow));
			switch (source)
			{
			case ZipEntrySource.Stream:
				zipEntry._sourceStream = arg1 as Stream;
				break;
			case ZipEntrySource.WriteDelegate:
				zipEntry._WriteDelegate = arg1 as WriteDelegate;
				break;
			case ZipEntrySource.JitStream:
				zipEntry._OpenDelegate = arg1 as OpenDelegate;
				zipEntry._CloseDelegate = arg2 as CloseDelegate;
				break;
			case ZipEntrySource.None:
				zipEntry._Source = ZipEntrySource.FileSystem;
				break;
			default:
			{
				string text = arg1 as string;
				if (string.IsNullOrEmpty(text))
				{
					throw new ZipException("The filename must be non-null and non-empty.");
				}
				try
				{
					zipEntry._Mtime = File.GetLastWriteTime(text).ToUniversalTime();
					zipEntry._Ctime = File.GetCreationTime(text).ToUniversalTime();
					zipEntry._Atime = File.GetLastAccessTime(text).ToUniversalTime();
					if (File.Exists(text) || Directory.Exists(text))
					{
						zipEntry._ExternalFileAttrs = (int)File.GetAttributes(text);
					}
					zipEntry._ntfsTimesAreSet = true;
					zipEntry._LocalFileName = Path.GetFullPath(text);
				}
				catch (PathTooLongException innerException)
				{
					throw new ZipException($"The path is too long, filename={text}", innerException);
				}
				break;
			}
			case ZipEntrySource.ZipOutputStream:
				break;
			}
			zipEntry._LastModified = zipEntry._Mtime;
			zipEntry._FileNameInArchive = SharedUtilities.NormalizePathForUseInZipFile(nameInArchive);
			return zipEntry;
		}

		public ZipEntry CloneForNewZipFile(ZipFile newZipFile)
		{
			if (Source != ZipEntrySource.ZipFile)
			{
				throw new InvalidOperationException("The entry you are trying to add wasn't loaded from a zip file.");
			}
			if (IsChanged)
			{
				throw new InvalidOperationException("The entry you are trying to add was modified.");
			}
			if (IsDirectory)
			{
				throw new InvalidOperationException("The entry you are trying to add is a directory.");
			}
			if (!CompressionMethod.Equals(newZipFile.CompressionMethod))
			{
				throw new InvalidOperationException("The entry you are trying to add uses another compression method.");
			}
			if (ArchiveStream is ZipSegmentedStream)
			{
				throw new InvalidOperationException("The entry you are trying to add is from a multi-part zip.");
			}
			return new ZipEntry
			{
				__FileDataPosition = __FileDataPosition,
				_actualEncoding = _actualEncoding,
				_archiveStream = _archiveStream,
				_Atime = _Atime,
				_Comment = _Comment,
				_CompressedSize = _CompressedSize,
				_CompressionLevel = _CompressionLevel,
				_CompressionMethod = _CompressionMethod,
				_Crc32 = _Crc32,
				_Ctime = _Ctime,
				_emitNtfsTimes = _emitNtfsTimes,
				_emitUnixTimes = _emitUnixTimes,
				_Encryption = _Encryption,
				_Encryption_FromZipFile = _Encryption_FromZipFile,
				_entryRequiresZip64 = _entryRequiresZip64,
				_FileNameInArchive = _FileNameInArchive,
				_LastModified = _LastModified,
				_LengthOfHeader = _LengthOfHeader,
				_metadataChanged = _metadataChanged,
				_Mtime = _Mtime,
				_ntfsTimesAreSet = _ntfsTimesAreSet,
				_Password = _Password,
				_presumeZip64 = _presumeZip64,
				_RelativeOffsetOfLocalHeader = _RelativeOffsetOfLocalHeader,
				_restreamRequiredOnSave = _restreamRequiredOnSave,
				_sourceIsEncrypted = _sourceIsEncrypted,
				_TimeBlob = _TimeBlob,
				_timestamp = _timestamp,
				_UncompressedSize = _UncompressedSize,
				_VersionMadeBy = _VersionMadeBy,
				_VersionNeeded = _VersionNeeded,
				AlternateEncoding = AlternateEncoding,
				AlternateEncodingUsage = AlternateEncodingUsage,
				ExtractExistingFile = ExtractExistingFile,
				SetCompression = SetCompression,
				ZipErrorAction = ZipErrorAction,
				_Source = _Source
			};
		}

		internal void MarkAsDirectory()
		{
			_IsDirectory = true;
			if (!_FileNameInArchive.EndsWith("/"))
			{
				_FileNameInArchive += "/";
			}
		}

		/// <summary>Provides a string representation of the instance.</summary>
		/// <returns>a string representation of the instance.</returns>
		public override string ToString()
		{
			return $"ZipEntry::{FileName}";
		}

		private void SetFdpLoh()
		{
			long position = ArchiveStream.Position;
			try
			{
				ArchiveStream.Seek(_RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
			}
			catch (IOException innerException)
			{
				throw new BadStateException($"Exception seeking  entry({FileName}) offset(0x{_RelativeOffsetOfLocalHeader:X8}) len(0x{ArchiveStream.Length:X8})", innerException);
			}
			byte[] array = new byte[30];
			ArchiveStream.Read(array, 0, array.Length);
			short num = (short)(array[26] + array[27] * 256);
			short num2 = (short)(array[28] + array[29] * 256);
			ArchiveStream.Seek(num + num2, SeekOrigin.Current);
			_LengthOfHeader = 30 + num2 + num + GetLengthOfCryptoHeaderBytes(_Encryption_FromZipFile);
			__FileDataPosition = _RelativeOffsetOfLocalHeader + _LengthOfHeader;
			ArchiveStream.Seek(position, SeekOrigin.Begin);
		}

		private static int GetKeyStrengthInBits(EncryptionAlgorithm a)
		{
			switch (a)
			{
			case EncryptionAlgorithm.WinZipAes256:
				return 256;
			case EncryptionAlgorithm.WinZipAes128:
				return 128;
			default:
				return -1;
			}
		}

		internal static int GetLengthOfCryptoHeaderBytes(EncryptionAlgorithm a)
		{
			switch (a)
			{
			case EncryptionAlgorithm.None:
				return 0;
			case EncryptionAlgorithm.WinZipAes128:
			case EncryptionAlgorithm.WinZipAes256:
				return GetKeyStrengthInBits(a) / 8 / 2 + 2;
			case EncryptionAlgorithm.PkzipWeak:
				return 12;
			default:
				throw new ZipException("internal error");
			}
		}

		/// <summary>
		///   Extract the entry to the filesystem, starting at the current
		///   working directory.
		/// </summary>
		///
		/// <overloads>
		///   This method has a bunch of overloads! One of them is sure to
		///   be the right one for you... If you don't like these, check
		///   out the <c>ExtractWithPassword()</c> methods.
		/// </overloads>
		///
		/// <seealso cref="P:Ionic.Zip.ZipEntry.ExtractExistingFile" />
		/// <seealso cref="M:Ionic.Zip.ZipEntry.Extract(Ionic.Zip.ExtractExistingFileAction)" />
		///
		/// <remarks>
		///
		/// <para>
		///   This method extracts an entry from a zip file into the current
		///   working directory.  The path of the entry as extracted is the full
		///   path as specified in the zip archive, relative to the current
		///   working directory.  After the file is extracted successfully, the
		///   file attributes and timestamps are set.
		/// </para>
		///
		/// <para>
		///   The action taken when extraction an entry would overwrite an
		///   existing file is determined by the <see cref="P:Ionic.Zip.ZipEntry.ExtractExistingFile" /> property.
		/// </para>
		///
		/// <para>
		///   Within the call to <c>Extract()</c>, the content for the entry is
		///   written into a filesystem file, and then the last modified time of the
		///   file is set according to the <see cref="P:Ionic.Zip.ZipEntry.LastModified" /> property on
		///   the entry. See the remarks the <see cref="P:Ionic.Zip.ZipEntry.LastModified" /> property for
		///   some details about the last modified time.
		/// </para>
		///
		/// </remarks>
		public void Extract()
		{
			InternalExtractToBaseDir(".", null, _container, _Source, FileName);
		}

		/// <summary>
		///   Extract the entry to a file in the filesystem, using the specified
		///   behavior when extraction would overwrite an existing file.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   See the remarks on the <see cref="P:Ionic.Zip.ZipEntry.LastModified" /> property, for some
		///   details about how the last modified time of the file is set after
		///   extraction.
		/// </para>
		/// </remarks>
		///
		/// <param name="extractExistingFile">
		///   The action to take if extraction would overwrite an existing file.
		/// </param>
		public void Extract(ExtractExistingFileAction extractExistingFile)
		{
			ExtractExistingFile = extractExistingFile;
			InternalExtractToBaseDir(".", null, _container, _Source, FileName);
		}

		/// <summary>
		///   Extracts the entry to the specified stream.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   The caller can specify any write-able stream, for example a <see cref="T:System.IO.FileStream" />, a <see cref="T:System.IO.MemoryStream" />, or ASP.NET's
		///   <c>Response.OutputStream</c>.  The content will be decrypted and
		///   decompressed as necessary. If the entry is encrypted and no password
		///   is provided, this method will throw.
		/// </para>
		/// <para>
		///   The position on the stream is not reset by this method before it extracts.
		///   You may want to call stream.Seek() before calling ZipEntry.Extract().
		/// </para>
		/// </remarks>
		///
		/// <param name="stream">
		///   the stream to which the entry should be extracted.
		/// </param>
		public void Extract(Stream stream)
		{
			InternalExtractToStream(stream, null, _container, _Source, FileName);
		}

		/// <summary>
		///   Extract the entry to the filesystem, starting at the specified base
		///   directory.
		/// </summary>
		///
		/// <param name="baseDirectory">the pathname of the base directory</param>
		///
		/// <seealso cref="P:Ionic.Zip.ZipEntry.ExtractExistingFile" />
		/// <seealso cref="M:Ionic.Zip.ZipEntry.Extract(System.String,Ionic.Zip.ExtractExistingFileAction)" />
		///
		/// <example>
		/// This example extracts only the entries in a zip file that are .txt files,
		/// into a directory called "textfiles".
		/// <code lang="C#">
		/// using (ZipFile zip = ZipFile.Read("PackedDocuments.zip"))
		/// {
		///   foreach (string s1 in zip.EntryFilenames)
		///   {
		///     if (s1.EndsWith(".txt"))
		///     {
		///       zip[s1].Extract("textfiles");
		///     }
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
		///
		/// <remarks>
		///
		/// <para>
		///   Using this method, existing entries in the filesystem will not be
		///   overwritten. If you would like to force the overwrite of existing
		///   files, see the <see cref="P:Ionic.Zip.ZipEntry.ExtractExistingFile" /> property, or call
		///   <see cref="M:Ionic.Zip.ZipEntry.Extract(System.String,Ionic.Zip.ExtractExistingFileAction)" />.
		/// </para>
		///
		/// <para>
		///   See the remarks on the <see cref="P:Ionic.Zip.ZipEntry.LastModified" /> property, for some
		///   details about how the last modified time of the created file is set.
		/// </para>
		/// </remarks>
		public void Extract(string baseDirectory)
		{
			InternalExtractToBaseDir(baseDirectory, null, _container, _Source, FileName);
		}

		/// <summary>
		///   Extract the entry to the filesystem, starting at the specified base
		///   directory, and using the specified behavior when extraction would
		///   overwrite an existing file.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   See the remarks on the <see cref="P:Ionic.Zip.ZipEntry.LastModified" /> property, for some
		///   details about how the last modified time of the created file is set.
		/// </para>
		/// </remarks>
		///
		/// <example>
		/// <code lang="C#">
		/// String sZipPath = "Airborne.zip";
		/// String sFilePath = "Readme.txt";
		/// String sRootFolder = "Digado";
		/// using (ZipFile zip = ZipFile.Read(sZipPath))
		/// {
		///   if (zip.EntryFileNames.Contains(sFilePath))
		///   {
		///     // use the string indexer on the zip file
		///     zip[sFileName].Extract(sRootFolder,
		///                            ExtractExistingFileAction.OverwriteSilently);
		///   }
		/// }
		/// </code>
		///
		/// <code lang="VB">
		/// Dim sZipPath as String = "Airborne.zip"
		/// Dim sFilePath As String = "Readme.txt"
		/// Dim sRootFolder As String = "Digado"
		/// Using zip As ZipFile = ZipFile.Read(sZipPath)
		///   If zip.EntryFileNames.Contains(sFilePath)
		///     ' use the string indexer on the zip file
		///     zip(sFilePath).Extract(sRootFolder, _
		///                            ExtractExistingFileAction.OverwriteSilently)
		///   End If
		/// End Using
		/// </code>
		/// </example>
		///
		/// <param name="baseDirectory">the pathname of the base directory</param>
		/// <param name="extractExistingFile">
		/// The action to take if extraction would overwrite an existing file.
		/// </param>
		public void Extract(string baseDirectory, ExtractExistingFileAction extractExistingFile)
		{
			ExtractExistingFile = extractExistingFile;
			InternalExtractToBaseDir(baseDirectory, null, _container, _Source, FileName);
		}

		/// <summary>
		///   Extract the entry to the filesystem, using the current working directory
		///   and the specified password.
		/// </summary>
		///
		/// <overloads>
		///   This method has a bunch of overloads! One of them is sure to be
		///   the right one for you...
		/// </overloads>
		///
		/// <seealso cref="P:Ionic.Zip.ZipEntry.ExtractExistingFile" />
		/// <seealso cref="M:Ionic.Zip.ZipEntry.ExtractWithPassword(Ionic.Zip.ExtractExistingFileAction,System.String)" />
		///
		/// <remarks>
		///
		/// <para>
		///   Existing entries in the filesystem will not be overwritten. If you
		///   would like to force the overwrite of existing files, see the <see cref="P:Ionic.Zip.ZipEntry.ExtractExistingFile" />property, or call
		///   <see cref="M:Ionic.Zip.ZipEntry.ExtractWithPassword(Ionic.Zip.ExtractExistingFileAction,System.String)" />.
		/// </para>
		///
		/// <para>
		///   See the remarks on the <see cref="P:Ionic.Zip.ZipEntry.LastModified" /> property for some
		///   details about how the "last modified" time of the created file is
		///   set.
		/// </para>
		/// </remarks>
		///
		/// <example>
		///   In this example, entries that use encryption are extracted using a
		///   particular password.
		/// <code>
		/// using (var zip = ZipFile.Read(FilePath))
		/// {
		///     foreach (ZipEntry e in zip)
		///     {
		///         if (e.UsesEncryption)
		///             e.ExtractWithPassword("Secret!");
		///         else
		///             e.Extract();
		///     }
		/// }
		/// </code>
		/// <code lang="VB">
		/// Using zip As ZipFile = ZipFile.Read(FilePath)
		///     Dim e As ZipEntry
		///     For Each e In zip
		///         If (e.UsesEncryption)
		///           e.ExtractWithPassword("Secret!")
		///         Else
		///           e.Extract
		///         End If
		///     Next
		/// End Using
		/// </code>
		/// </example>
		/// <param name="password">The Password to use for decrypting the entry.</param>
		public void ExtractWithPassword(string password)
		{
			InternalExtractToBaseDir(".", password, _container, _Source, FileName);
		}

		/// <summary>
		///   Extract the entry to the filesystem, starting at the specified base
		///   directory, and using the specified password.
		/// </summary>
		///
		/// <seealso cref="P:Ionic.Zip.ZipEntry.ExtractExistingFile" />
		/// <seealso cref="M:Ionic.Zip.ZipEntry.ExtractWithPassword(System.String,Ionic.Zip.ExtractExistingFileAction,System.String)" />
		///
		/// <remarks>
		/// <para>
		///   Existing entries in the filesystem will not be overwritten. If you
		///   would like to force the overwrite of existing files, see the <see cref="P:Ionic.Zip.ZipEntry.ExtractExistingFile" />property, or call
		///   <see cref="M:Ionic.Zip.ZipEntry.ExtractWithPassword(Ionic.Zip.ExtractExistingFileAction,System.String)" />.
		/// </para>
		///
		/// <para>
		///   See the remarks on the <see cref="P:Ionic.Zip.ZipEntry.LastModified" /> property, for some
		///   details about how the last modified time of the created file is set.
		/// </para>
		/// </remarks>
		///
		/// <param name="baseDirectory">The pathname of the base directory.</param>
		/// <param name="password">The Password to use for decrypting the entry.</param>
		public void ExtractWithPassword(string baseDirectory, string password)
		{
			InternalExtractToBaseDir(baseDirectory, password, _container, _Source, FileName);
		}

		/// <summary>
		///   Extract the entry to a file in the filesystem, relative to the
		///   current directory, using the specified behavior when extraction
		///   would overwrite an existing file.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   See the remarks on the <see cref="P:Ionic.Zip.ZipEntry.LastModified" /> property, for some
		///   details about how the last modified time of the created file is set.
		/// </para>
		/// </remarks>
		///
		/// <param name="password">The Password to use for decrypting the entry.</param>
		///
		/// <param name="extractExistingFile">
		/// The action to take if extraction would overwrite an existing file.
		/// </param>
		public void ExtractWithPassword(ExtractExistingFileAction extractExistingFile, string password)
		{
			ExtractExistingFile = extractExistingFile;
			InternalExtractToBaseDir(".", password, _container, _Source, FileName);
		}

		/// <summary>
		///   Extract the entry to the filesystem, starting at the specified base
		///   directory, and using the specified behavior when extraction would
		///   overwrite an existing file.
		/// </summary>
		///
		/// <remarks>
		///   See the remarks on the <see cref="P:Ionic.Zip.ZipEntry.LastModified" /> property, for some
		///   details about how the last modified time of the created file is set.
		/// </remarks>
		///
		/// <param name="baseDirectory">the pathname of the base directory</param>
		///
		/// <param name="extractExistingFile">The action to take if extraction would
		/// overwrite an existing file.</param>
		///
		/// <param name="password">The Password to use for decrypting the entry.</param>
		public void ExtractWithPassword(string baseDirectory, ExtractExistingFileAction extractExistingFile, string password)
		{
			ExtractExistingFile = extractExistingFile;
			InternalExtractToBaseDir(baseDirectory, password, _container, _Source, FileName);
		}

		/// <summary>
		///   Extracts the entry to the specified stream, using the specified
		///   Password.  For example, the caller could extract to Console.Out, or
		///   to a MemoryStream.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   The caller can specify any write-able stream, for example a <see cref="T:System.IO.FileStream" />, a <see cref="T:System.IO.MemoryStream" />, or ASP.NET's
		///   <c>Response.OutputStream</c>.  The content will be decrypted and
		///   decompressed as necessary. If the entry is encrypted and no password
		///   is provided, this method will throw.
		/// </para>
		/// <para>
		///   The position on the stream is not reset by this method before it extracts.
		///   You may want to call stream.Seek() before calling ZipEntry.Extract().
		/// </para>
		/// </remarks>
		///
		///
		/// <param name="stream">
		///   the stream to which the entry should be extracted.
		/// </param>
		/// <param name="password">
		///   The password to use for decrypting the entry.
		/// </param>
		public void ExtractWithPassword(Stream stream, string password)
		{
			InternalExtractToStream(stream, password, _container, _Source, FileName);
		}

		/// <summary>
		///   Opens a readable stream corresponding to the zip entry in the
		///   archive.  The stream decompresses and decrypts as necessary, as it
		///   is read.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   DotNetZip offers a variety of ways to extract entries from a zip
		///   file.  This method allows an application to extract an entry by
		///   reading a <see cref="T:System.IO.Stream" />.
		/// </para>
		///
		/// <para>
		///   The return value is of type <see cref="T:Ionic.Crc.CrcCalculatorStream" />.  Use it as you would any
		///   stream for reading.  When an application calls <see cref="M:System.IO.Stream.Read(System.Byte[],System.Int32,System.Int32)" /> on that stream, it will
		///   receive data from the zip entry that is decrypted and decompressed
		///   as necessary.
		/// </para>
		///
		/// <para>
		///   <c>CrcCalculatorStream</c> adds one additional feature: it keeps a
		///   CRC32 checksum on the bytes of the stream as it is read.  The CRC
		///   value is available in the <see cref="P:Ionic.Crc.CrcCalculatorStream.Crc" /> property on the
		///   <c>CrcCalculatorStream</c>.  When the read is complete, your
		///   application
		///   <em>should</em> check this CRC against the <see cref="P:Ionic.Zip.ZipEntry.Crc" />
		///   property on the <c>ZipEntry</c> to validate the content of the
		///   ZipEntry. You don't have to validate the entry using the CRC, but
		///   you should, to verify integrity. Check the example for how to do
		///   this.
		/// </para>
		///
		/// <para>
		///   If the entry is protected with a password, then you need to provide
		///   a password prior to calling <see cref="M:Ionic.Zip.ZipEntry.OpenReader" />, either by
		///   setting the <see cref="P:Ionic.Zip.ZipEntry.Password" /> property on the entry, or the
		///   <see cref="P:Ionic.Zip.ZipFile.Password" /> property on the <c>ZipFile</c>
		///   itself. Or, you can use <see cref="M:Ionic.Zip.ZipEntry.OpenReader(System.String)" />, the
		///   overload of OpenReader that accepts a password parameter.
		/// </para>
		///
		/// <para>
		///   If you want to extract entry data into a write-able stream that is
		///   already opened, like a <see cref="T:System.IO.FileStream" />, do not
		///   use this method. Instead, use <see cref="M:Ionic.Zip.ZipEntry.Extract(System.IO.Stream)" />.
		/// </para>
		///
		/// <para>
		///   Your application may use only one stream created by OpenReader() at
		///   a time, and you should not call other Extract methods before
		///   completing your reads on a stream obtained from OpenReader().  This
		///   is because there is really only one source stream for the compressed
		///   content.  A call to OpenReader() seeks in the source stream, to the
		///   beginning of the compressed content.  A subsequent call to
		///   OpenReader() on a different entry will seek to a different position
		///   in the source stream, as will a call to Extract() or one of its
		///   overloads.  This will corrupt the state for the decompressing stream
		///   from the original call to OpenReader().
		/// </para>
		///
		/// <para>
		///    The <c>OpenReader()</c> method works only when the ZipEntry is
		///    obtained from an instance of <c>ZipFile</c>. This method will throw
		///    an exception if the ZipEntry is obtained from a <see cref="T:Ionic.Zip.ZipInputStream" />.
		/// </para>
		/// </remarks>
		///
		/// <example>
		///   This example shows how to open a zip archive, then read in a named
		///   entry via a stream. After the read loop is complete, the code
		///   compares the calculated during the read loop with the expected CRC
		///   on the <c>ZipEntry</c>, to verify the extraction.
		/// <code>
		/// using (ZipFile zip = new ZipFile(ZipFileToRead))
		/// {
		///   ZipEntry e1= zip["Elevation.mp3"];
		///   using (Ionic.Zlib.CrcCalculatorStream s = e1.OpenReader())
		///   {
		///     byte[] buffer = new byte[4096];
		///     int n, totalBytesRead= 0;
		///     do {
		///       n = s.Read(buffer,0, buffer.Length);
		///       totalBytesRead+=n;
		///     } while (n&gt;0);
		///      if (s.Crc32 != e1.Crc32)
		///       throw new Exception(string.Format("The Zip Entry failed the CRC Check. (0x{0:X8}!=0x{1:X8})", s.Crc32, e1.Crc32));
		///      if (totalBytesRead != e1.UncompressedSize)
		///       throw new Exception(string.Format("We read an unexpected number of bytes. ({0}!={1})", totalBytesRead, e1.UncompressedSize));
		///   }
		/// }
		/// </code>
		/// <code lang="VB">
		///   Using zip As New ZipFile(ZipFileToRead)
		///       Dim e1 As ZipEntry = zip.Item("Elevation.mp3")
		///       Using s As Ionic.Zlib.CrcCalculatorStream = e1.OpenReader
		///           Dim n As Integer
		///           Dim buffer As Byte() = New Byte(4096) {}
		///           Dim totalBytesRead As Integer = 0
		///           Do
		///               n = s.Read(buffer, 0, buffer.Length)
		///               totalBytesRead = (totalBytesRead + n)
		///           Loop While (n &gt; 0)
		///           If (s.Crc32 &lt;&gt; e1.Crc32) Then
		///               Throw New Exception(String.Format("The Zip Entry failed the CRC Check. (0x{0:X8}!=0x{1:X8})", s.Crc32, e1.Crc32))
		///           End If
		///           If (totalBytesRead &lt;&gt; e1.UncompressedSize) Then
		///               Throw New Exception(String.Format("We read an unexpected number of bytes. ({0}!={1})", totalBytesRead, e1.UncompressedSize))
		///           End If
		///       End Using
		///   End Using
		/// </code>
		/// </example>
		/// <seealso cref="M:Ionic.Zip.ZipEntry.Extract(System.IO.Stream)" />
		/// <returns>The Stream for reading.</returns>
		public CrcCalculatorStream OpenReader()
		{
			if (_container.ZipFile == null)
			{
				throw new InvalidOperationException("Use OpenReader() only with ZipFile.");
			}
			return InternalOpenReader(_Password ?? _container.Password);
		}

		/// <summary>
		///   Opens a readable stream for an encrypted zip entry in the archive.
		///   The stream decompresses and decrypts as necessary, as it is read.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   See the documentation on the <see cref="M:Ionic.Zip.ZipEntry.OpenReader" /> method for
		///   full details. This overload allows the application to specify a
		///   password for the <c>ZipEntry</c> to be read.
		/// </para>
		/// </remarks>
		///
		/// <param name="password">The password to use for decrypting the entry.</param>
		/// <returns>The Stream for reading.</returns>
		public CrcCalculatorStream OpenReader(string password)
		{
			if (_container.ZipFile == null)
			{
				throw new InvalidOperationException("Use OpenReader() only with ZipFile.");
			}
			return InternalOpenReader(password);
		}

		internal CrcCalculatorStream InternalOpenReader(string password)
		{
			ValidateCompression(_CompressionMethod_FromZipFile, FileName, GetUnsupportedCompressionMethod(_CompressionMethod));
			ValidateEncryption(Encryption, FileName, _UnsupportedAlgorithmId);
			SetupCryptoForExtract(password);
			if (_Source != ZipEntrySource.ZipFile)
			{
				throw new BadStateException("You must call ZipFile.Save before calling OpenReader");
			}
			long length = ((_CompressionMethod_FromZipFile == 0) ? _CompressedFileDataSize : UncompressedSize);
			ArchiveStream.Seek(FileDataPosition, SeekOrigin.Begin);
			_inputDecryptorStream = GetExtractDecryptor(ArchiveStream);
			return new CrcCalculatorStream(GetExtractDecompressor(_inputDecryptorStream), length);
		}

		private void OnExtractProgress(long bytesWritten, long totalBytesToWrite)
		{
			if (_container.ZipFile != null)
			{
				_ioOperationCanceled = _container.ZipFile.OnExtractBlock(this, bytesWritten, totalBytesToWrite);
			}
		}

		private static void OnBeforeExtract(ZipEntry zipEntryInstance, string path, ZipFile zipFile)
		{
			if (zipFile != null && !zipFile._inExtractAll)
			{
				zipFile.OnSingleEntryExtract(zipEntryInstance, path, true);
			}
		}

		private void OnAfterExtract(string path)
		{
			if (_container.ZipFile != null && !_container.ZipFile._inExtractAll)
			{
				_container.ZipFile.OnSingleEntryExtract(this, path, false);
			}
		}

		private void OnExtractExisting(string path)
		{
			if (_container.ZipFile != null)
			{
				_ioOperationCanceled = _container.ZipFile.OnExtractExisting(this, path);
			}
		}

		private static void ReallyDelete(string fileName)
		{
			if ((File.GetAttributes(fileName) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
			{
				File.SetAttributes(fileName, FileAttributes.Normal);
			}
			File.Delete(fileName);
		}

		private void WriteStatus(string format, params object[] args)
		{
			if (_container.ZipFile != null && _container.ZipFile.Verbose)
			{
				_container.ZipFile.StatusMessageTextWriter.WriteLine(format, args);
			}
		}

		/// <summary>
		/// Pass in either basedir or s, but not both.
		/// In other words, you can extract to a stream or to a directory (filesystem), but not both!
		/// The Password param is required for encrypted entries.
		/// </summary>
		private void InternalExtractToBaseDir(string baseDir, string password, ZipContainer zipContainer, ZipEntrySource zipEntrySource, string fileName)
		{
			if (baseDir == null)
			{
				throw new ArgumentNullException("baseDir");
			}
			if (zipContainer == null)
			{
				throw new BadStateException("This entry is an orphan");
			}
			if (zipContainer.ZipFile == null)
			{
				throw new InvalidOperationException("Use Extract() only with ZipFile.");
			}
			zipContainer.ZipFile.Reset(false);
			if (zipEntrySource != ZipEntrySource.ZipFile)
			{
				throw new BadStateException("You must call ZipFile.Save before calling any Extract method");
			}
			OnBeforeExtract(this, baseDir, zipContainer.ZipFile);
			_ioOperationCanceled = false;
			bool flag = false;
			bool checkLaterForResetDirTimes = false;
			string outFileName = null;
			try
			{
				ValidateCompression(_CompressionMethod_FromZipFile, fileName, GetUnsupportedCompressionMethod(_CompressionMethod));
				ValidateEncryption(Encryption, fileName, _UnsupportedAlgorithmId);
				if (IsDoneWithOutputToBaseDir(baseDir, out outFileName))
				{
					WriteStatus("extract dir {0}...", outFileName);
					OnAfterExtract(baseDir);
					return;
				}
				if (File.Exists(outFileName))
				{
					flag = true;
					int num = CheckExtractExistingFile(baseDir, outFileName);
					if (num == 2 || num == 1)
					{
						return;
					}
				}
				if (_Encryption_FromZipFile != 0)
				{
					EnsurePassword(password);
				}
				string path = SharedUtilities.InternalGetTempFileName();
				string tmpPath = Path.Combine(Path.GetDirectoryName(outFileName), path);
				WriteStatus("extract file {0}...", outFileName);
				using (Stream stream = OpenFileStream(tmpPath, ref checkLaterForResetDirTimes))
				{
					if (ExtractToStream(ArchiveStream, stream, Encryption, _Crc32))
					{
						return;
					}
					stream.Close();
				}
				MoveFileInPlace(flag, outFileName, tmpPath, checkLaterForResetDirTimes);
				OnAfterExtract(baseDir);
			}
			catch (Exception)
			{
				_ioOperationCanceled = true;
				throw;
			}
			finally
			{
				if (_ioOperationCanceled && outFileName != null && File.Exists(outFileName) && !flag)
				{
					File.Delete(outFileName);
				}
			}
		}

		/// <summary>
		/// Extract to a stream
		/// In other words, you can extract to a stream or to a directory (filesystem), but not both!
		/// The Password param is required for encrypted entries.
		/// </summary>
		private void InternalExtractToStream(Stream outStream, string password, ZipContainer zipContainer, ZipEntrySource zipEntrySource, string fileName)
		{
			if (zipContainer == null)
			{
				throw new BadStateException("This entry is an orphan");
			}
			if (zipContainer.ZipFile == null)
			{
				throw new InvalidOperationException("Use Extract() only with ZipFile.");
			}
			zipContainer.ZipFile.Reset(false);
			if (zipEntrySource != ZipEntrySource.ZipFile)
			{
				throw new BadStateException("You must call ZipFile.Save before calling any Extract method");
			}
			OnBeforeExtract(this, null, zipContainer.ZipFile);
			_ioOperationCanceled = false;
			try
			{
				ValidateCompression(_CompressionMethod_FromZipFile, fileName, GetUnsupportedCompressionMethod(_CompressionMethod));
				ValidateEncryption(Encryption, fileName, _UnsupportedAlgorithmId);
				if (IsDoneWithOutputToStream())
				{
					WriteStatus("extract dir {0}...", null);
					OnAfterExtract(null);
					return;
				}
				if (_Encryption_FromZipFile != 0)
				{
					EnsurePassword(password);
				}
				WriteStatus("extract entry {0} to stream...", fileName);
				Stream archiveStream = ArchiveStream;
				if (!ExtractToStream(archiveStream, outStream, Encryption, _Crc32))
				{
					OnAfterExtract(null);
				}
			}
			catch (Exception)
			{
				_ioOperationCanceled = true;
				throw;
			}
		}

		private bool ExtractToStream(Stream archiveStream, Stream output, EncryptionAlgorithm encryptionAlgorithm, int expectedCrc32)
		{
			if (_ioOperationCanceled)
			{
				return true;
			}
			try
			{
				int calculatedCrc = ExtractAndCrc(archiveStream, output, _CompressionMethod_FromZipFile, _CompressedFileDataSize, UncompressedSize);
				if (_ioOperationCanceled)
				{
					return true;
				}
				VerifyCrcAfterExtract(calculatedCrc, encryptionAlgorithm, expectedCrc32, archiveStream, UncompressedSize);
				return false;
			}
			finally
			{
				if (archiveStream is ZipSegmentedStream zipSegmentedStream)
				{
					zipSegmentedStream.Dispose();
					_archiveStream = null;
				}
			}
		}

		private void MoveFileInPlace(bool fileExistsBeforeExtraction, string targetFileName, string tmpPath, bool checkLaterForResetDirTimes)
		{
			string text = null;
			if (fileExistsBeforeExtraction)
			{
				text = targetFileName + Path.GetRandomFileName() + ".PendingOverwrite";
				File.Move(targetFileName, text);
			}
			File.Move(tmpPath, targetFileName);
			_SetTimes(targetFileName, true);
			if (text != null && File.Exists(text))
			{
				ReallyDelete(text);
			}
			if (checkLaterForResetDirTimes && FileName.Contains("/"))
			{
				string directoryName = Path.GetDirectoryName(FileName);
				if (_container.ZipFile[directoryName] == null)
				{
					_SetTimes(Path.GetDirectoryName(targetFileName), false);
				}
			}
			if ((_VersionMadeBy & 0xFF00) == 2560 || (_VersionMadeBy & 0xFF00) == 0)
			{
				File.SetAttributes(targetFileName, (FileAttributes)_ExternalFileAttrs);
			}
		}

		private void EnsurePassword(string password)
		{
			string text = password ?? _Password ?? _container.Password;
			if (text == null)
			{
				throw new BadPasswordException();
			}
			SetupCryptoForExtract(text);
		}

		private Stream OpenFileStream(string tmpPath, ref bool checkLaterForResetDirTimes)
		{
			string directoryName = Path.GetDirectoryName(tmpPath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			else if (_container.ZipFile != null)
			{
				checkLaterForResetDirTimes = _container.ZipFile._inExtractAll;
			}
			return new FileStream(tmpPath, FileMode.CreateNew);
		}

		internal void VerifyCrcAfterExtract(int calculatedCrc32, EncryptionAlgorithm encryptionAlgorithm, int expectedCrc32, Stream archiveStream, long uncompressedSize)
		{
			if (calculatedCrc32 != expectedCrc32 && ((encryptionAlgorithm != EncryptionAlgorithm.WinZipAes128 && encryptionAlgorithm != EncryptionAlgorithm.WinZipAes256) || _WinZipAesMethod != 2))
			{
				throw new BadCrcException("CRC error: the file being extracted appears to be corrupted. " + $"Expected 0x{expectedCrc32:X8}, Actual 0x{calculatedCrc32:X8}");
			}
			if (uncompressedSize != 0L && (encryptionAlgorithm == EncryptionAlgorithm.WinZipAes128 || encryptionAlgorithm == EncryptionAlgorithm.WinZipAes256))
			{
				WinZipAesCipherStream winZipAesCipherStream = _inputDecryptorStream as WinZipAesCipherStream;
				byte[] buffer = new byte[256];
				winZipAesCipherStream.Read(buffer, 0, 256);
				_aesCrypto_forExtract.CalculatedMac = winZipAesCipherStream.FinalAuthentication;
				_aesCrypto_forExtract.ReadAndVerifyMac(archiveStream);
			}
		}

		private int CheckExtractExistingFile(string baseDir, string targetFileName)
		{
			int num = 0;
			while (true)
			{
				switch (ExtractExistingFile)
				{
				case ExtractExistingFileAction.OverwriteSilently:
					WriteStatus("the file {0} exists; will overwrite it...", targetFileName);
					return 0;
				case ExtractExistingFileAction.DoNotOverwrite:
					WriteStatus("the file {0} exists; not extracting entry...", FileName);
					OnAfterExtract(baseDir);
					return 1;
				case ExtractExistingFileAction.InvokeExtractProgressEvent:
					if (num > 0)
					{
						throw new ZipException($"The file {targetFileName} already exists.");
					}
					OnExtractExisting(baseDir);
					if (_ioOperationCanceled)
					{
						return 2;
					}
					break;
				default:
					throw new ZipException($"The file {targetFileName} already exists.");
				}
				num++;
			}
		}

		private void _CheckRead(int nbytes)
		{
			if (nbytes == 0)
			{
				throw new BadReadException($"bad read of entry {FileName} from compressed archive.");
			}
		}

		private int ExtractAndCrc(Stream archiveStream, Stream targetOutput, short compressionMethod, long compressedFileDataSize, long uncompressedSize)
		{
			archiveStream.Seek(FileDataPosition, SeekOrigin.Begin);
			byte[] array = new byte[BufferSize];
			long num = ((compressionMethod != 0) ? uncompressedSize : compressedFileDataSize);
			_inputDecryptorStream = GetExtractDecryptor(archiveStream);
			Stream extractDecompressor = GetExtractDecompressor(_inputDecryptorStream);
			long num2 = 0L;
			using (CrcCalculatorStream crcCalculatorStream = new CrcCalculatorStream(extractDecompressor))
			{
				while (num > 0)
				{
					int count = (int)((num > array.Length) ? array.Length : num);
					int num3 = crcCalculatorStream.Read(array, 0, count);
					_CheckRead(num3);
					targetOutput.Write(array, 0, num3);
					num -= num3;
					num2 += num3;
					OnExtractProgress(num2, uncompressedSize);
					if (_ioOperationCanceled)
					{
						break;
					}
				}
				return crcCalculatorStream.Crc;
			}
		}

		private Stream GetExtractDecompressor(Stream input2)
		{
			if (input2 == null)
			{
				throw new ArgumentNullException("input2");
			}
			switch (_CompressionMethod_FromZipFile)
			{
			case 0:
				return input2;
			case 8:
				return new DeflateStream(input2, CompressionMode.Decompress, true);
			case 9:
				return new Deflate64Stream(input2, -1L);
			case 12:
				return new BZip2InputStream(input2, true);
			default:
				throw new Exception($"Failed to find decompressor matching {_CompressionMethod_FromZipFile}");
			}
		}

		private Stream GetExtractDecryptor(Stream input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (_Encryption_FromZipFile == EncryptionAlgorithm.PkzipWeak)
			{
				return new ZipCipherStream(input, _zipCrypto_forExtract, CryptoMode.Decrypt);
			}
			if (_Encryption_FromZipFile == EncryptionAlgorithm.WinZipAes128 || _Encryption_FromZipFile == EncryptionAlgorithm.WinZipAes256)
			{
				return new WinZipAesCipherStream(input, _aesCrypto_forExtract, _CompressedFileDataSize, CryptoMode.Decrypt);
			}
			return input;
		}

		internal void _SetTimes(string fileOrDirectory, bool isFile)
		{
			try
			{
				if (_ntfsTimesAreSet)
				{
					if (isFile)
					{
						if (File.Exists(fileOrDirectory))
						{
							File.SetCreationTimeUtc(fileOrDirectory, _Ctime);
							File.SetLastAccessTimeUtc(fileOrDirectory, _Atime);
							File.SetLastWriteTimeUtc(fileOrDirectory, _Mtime);
						}
					}
					else if (Directory.Exists(fileOrDirectory))
					{
						Directory.SetCreationTimeUtc(fileOrDirectory, _Ctime);
						Directory.SetLastAccessTimeUtc(fileOrDirectory, _Atime);
						Directory.SetLastWriteTimeUtc(fileOrDirectory, _Mtime);
					}
				}
				else
				{
					DateTime lastWriteTime = SharedUtilities.AdjustTime_Reverse(LastModified);
					if (isFile)
					{
						File.SetLastWriteTime(fileOrDirectory, lastWriteTime);
					}
					else
					{
						Directory.SetLastWriteTime(fileOrDirectory, lastWriteTime);
					}
				}
			}
			catch (IOException ex)
			{
				WriteStatus("failed to set time on {0}: {1}", fileOrDirectory, ex.Message);
			}
		}

		private static string GetUnsupportedAlgorithm(uint unsupportedAlgorithmId)
		{
			switch (unsupportedAlgorithmId)
			{
			case 0u:
				return "--";
			case 26113u:
				return "DES";
			case 26114u:
				return "RC2";
			case 26115u:
				return "3DES-168";
			case 26121u:
				return "3DES-112";
			case 26126u:
				return "PKWare AES128";
			case 26127u:
				return "PKWare AES192";
			case 26128u:
				return "PKWare AES256";
			case 26370u:
				return "RC2";
			case 26400u:
				return "Blowfish";
			case 26401u:
				return "Twofish";
			case 26625u:
				return "RC4";
			default:
				return $"Unknown (0x{unsupportedAlgorithmId:X4})";
			}
		}

		private static string GetUnsupportedCompressionMethod(short compressionMethod)
		{
			switch (compressionMethod)
			{
			case 0:
				return "Store";
			case 1:
				return "Shrink";
			case 8:
				return "DEFLATE";
			case 9:
				return "Deflate64";
			case 12:
				return "BZIP2";
			case 14:
				return "LZMA";
			case 19:
				return "LZ77";
			case 98:
				return "PPMd";
			default:
				return $"Unknown (0x{compressionMethod:X4})";
			}
		}

		private static void ValidateEncryption(EncryptionAlgorithm encryptionAlgorithm, string fileName, uint unsupportedAlgorithmId)
		{
			if (encryptionAlgorithm != EncryptionAlgorithm.PkzipWeak && encryptionAlgorithm != EncryptionAlgorithm.WinZipAes128 && encryptionAlgorithm != EncryptionAlgorithm.WinZipAes256 && encryptionAlgorithm != 0)
			{
				if (unsupportedAlgorithmId != 0)
				{
					throw new ZipException($"Cannot extract: Entry {fileName} is encrypted with an algorithm not supported by DotNetZip: {GetUnsupportedAlgorithm(unsupportedAlgorithmId)}");
				}
				throw new ZipException($"Cannot extract: Entry {fileName} uses an unsupported encryption algorithm ({(int)encryptionAlgorithm:X2})");
			}
		}

		private static void ValidateCompression(short compressionMethod, string fileName, string compressionMethodName)
		{
			if (compressionMethod != 0 && compressionMethod != 8 && compressionMethod != 9 && compressionMethod != 12)
			{
				throw new ZipException($"Entry {fileName} uses an unsupported compression method (0x{compressionMethod:X2}, {compressionMethodName})");
			}
		}

		private void SetupCryptoForExtract(string password)
		{
			if (_Encryption_FromZipFile == EncryptionAlgorithm.None)
			{
				return;
			}
			if (_Encryption_FromZipFile == EncryptionAlgorithm.PkzipWeak)
			{
				if (password == null)
				{
					throw new ZipException("Missing password.");
				}
				ArchiveStream.Seek(FileDataPosition - 12, SeekOrigin.Begin);
				_zipCrypto_forExtract = ZipCrypto.ForRead(password, this);
			}
			else if (_Encryption_FromZipFile == EncryptionAlgorithm.WinZipAes128 || _Encryption_FromZipFile == EncryptionAlgorithm.WinZipAes256)
			{
				if (password == null)
				{
					throw new ZipException("Missing password.");
				}
				if (_aesCrypto_forExtract != null)
				{
					_aesCrypto_forExtract.Password = password;
					return;
				}
				int lengthOfCryptoHeaderBytes = GetLengthOfCryptoHeaderBytes(_Encryption_FromZipFile);
				ArchiveStream.Seek(FileDataPosition - lengthOfCryptoHeaderBytes, SeekOrigin.Begin);
				int keyStrengthInBits = GetKeyStrengthInBits(_Encryption_FromZipFile);
				_aesCrypto_forExtract = WinZipAesCrypto.ReadFromStream(password, keyStrengthInBits, ArchiveStream);
			}
		}

		/// <summary>
		/// Validates that the args are consistent; returning whether the caller can return
		/// because it's done, or not (caller should continue)
		/// </summary>
		private bool IsDoneWithOutputToBaseDir(string baseDir, out string outFileName)
		{
			if (baseDir == null)
			{
				throw new ArgumentNullException("baseDir");
			}
			string text = FileName.Replace(Path.DirectorySeparatorChar, '/');
			if (text.IndexOf(':') == 1)
			{
				text = text.Substring(2);
			}
			if (text.StartsWith("/"))
			{
				text = text.Substring(1);
			}
			text = SharedUtilities.SanitizePath(text);
			outFileName = (_container.ZipFile.FlattenFoldersOnExtract ? Path.Combine(baseDir, text.Contains("/") ? Path.GetFileName(text) : text) : Path.Combine(baseDir, text));
			outFileName = outFileName.Replace('/', Path.DirectorySeparatorChar);
			if (IsDirectory || FileName.EndsWith("/"))
			{
				if (!Directory.Exists(outFileName))
				{
					Directory.CreateDirectory(outFileName);
					_SetTimes(outFileName, false);
				}
				else if (ExtractExistingFile == ExtractExistingFileAction.OverwriteSilently)
				{
					_SetTimes(outFileName, false);
				}
				return true;
			}
			return false;
		}

		/// <summary>
		/// Validates that the args are consistent; returning whether the caller can return
		/// because it's done, or not (caller should continue)
		/// </summary>
		private bool IsDoneWithOutputToStream()
		{
			if (!IsDirectory)
			{
				return FileName.EndsWith("/");
			}
			return true;
		}

		private void ReadExtraField()
		{
			_readExtraDepth++;
			long position = ArchiveStream.Position;
			ArchiveStream.Seek(_RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
			byte[] array = new byte[30];
			ArchiveStream.Read(array, 0, array.Length);
			int num = 26;
			short num2 = (short)(array[num++] + array[num++] * 256);
			short extraFieldLength = (short)(array[num++] + array[num++] * 256);
			ArchiveStream.Seek(num2, SeekOrigin.Current);
			ProcessExtraField(ArchiveStream, extraFieldLength);
			ArchiveStream.Seek(position, SeekOrigin.Begin);
			_readExtraDepth--;
		}

		private static bool ReadHeader(ZipEntry ze, Encoding defaultEncoding)
		{
			int num = 0;
			ze._RelativeOffsetOfLocalHeader = ze.ArchiveStream.Position;
			int num2 = SharedUtilities.ReadEntrySignature(ze.ArchiveStream);
			num += 4;
			if (IsNotValidSig(num2))
			{
				ze.ArchiveStream.Seek(-4L, SeekOrigin.Current);
				if (IsNotValidZipDirEntrySig(num2) && (long)num2 != 101010256)
				{
					throw new BadReadException($"  Bad signature (0x{num2:X8}) at position  0x{ze.ArchiveStream.Position:X8}");
				}
				return false;
			}
			byte[] array = new byte[26];
			int num3 = ze.ArchiveStream.Read(array, 0, array.Length);
			if (num3 != array.Length)
			{
				return false;
			}
			num += num3;
			int num4 = 0;
			ze._VersionNeeded = (short)(array[num4++] + array[num4++] * 256);
			ze._BitField = (short)(array[num4++] + array[num4++] * 256);
			ze._CompressionMethod_FromZipFile = (ze._CompressionMethod = (short)(array[num4++] + array[num4++] * 256));
			ze._TimeBlob = array[num4++] + array[num4++] * 256 + array[num4++] * 256 * 256 + array[num4++] * 256 * 256 * 256;
			ze._LastModified = SharedUtilities.PackedToDateTime(ze._TimeBlob);
			ze._timestamp |= ZipEntryTimestamp.DOS;
			if ((ze._BitField & 1) == 1)
			{
				ze._Encryption_FromZipFile = (ze._Encryption = EncryptionAlgorithm.PkzipWeak);
				ze._sourceIsEncrypted = true;
			}
			ze._Crc32 = array[num4++] + array[num4++] * 256 + array[num4++] * 256 * 256 + array[num4++] * 256 * 256 * 256;
			ze._CompressedSize = (uint)(array[num4++] + array[num4++] * 256 + array[num4++] * 256 * 256 + array[num4++] * 256 * 256 * 256);
			ze._UncompressedSize = (uint)(array[num4++] + array[num4++] * 256 + array[num4++] * 256 * 256 + array[num4++] * 256 * 256 * 256);
			if ((int)ze._CompressedSize == -1 || (int)ze._UncompressedSize == -1)
			{
				ze._InputUsesZip64 = true;
			}
			short num5 = (short)(array[num4++] + array[num4++] * 256);
			short extraFieldLength = (short)(array[num4++] + array[num4++] * 256);
			array = new byte[num5];
			num3 = ze.ArchiveStream.Read(array, 0, array.Length);
			num += num3;
			if ((ze._BitField & 0x800) == 2048)
			{
				ze.AlternateEncoding = Encoding.UTF8;
				ze.AlternateEncodingUsage = ZipOption.Always;
			}
			ze._FileNameInArchive = ze.AlternateEncoding.GetString(array);
			if (ze._FileNameInArchive.EndsWith("/"))
			{
				ze.MarkAsDirectory();
			}
			num += ze.ProcessExtraField(ze.ArchiveStream, extraFieldLength);
			ze._LengthOfTrailer = 0;
			if (!ze._FileNameInArchive.EndsWith("/") && (ze._BitField & 8) == 8)
			{
				long position = ze.ArchiveStream.Position;
				bool flag = true;
				long num6 = 0L;
				int num7 = 0;
				while (flag)
				{
					num7++;
					if (ze._container.ZipFile != null)
					{
						ze._container.ZipFile.OnReadBytes(ze);
					}
					long num8 = SharedUtilities.FindSignature(ze.ArchiveStream, 134695760);
					if (num8 == -1)
					{
						return false;
					}
					num6 += num8;
					if (ze._InputUsesZip64)
					{
						array = new byte[20];
						num3 = ze.ArchiveStream.Read(array, 0, array.Length);
						if (num3 != 20)
						{
							return false;
						}
						num4 = 0;
						ze._Crc32 = array[num4++] + array[num4++] * 256 + array[num4++] * 256 * 256 + array[num4++] * 256 * 256 * 256;
						ze._CompressedSize = BitConverter.ToInt64(array, num4);
						num4 += 8;
						ze._UncompressedSize = BitConverter.ToInt64(array, num4);
						num4 += 8;
					}
					else
					{
						array = new byte[12];
						num3 = ze.ArchiveStream.Read(array, 0, array.Length);
						if (num3 != 12)
						{
							return false;
						}
						num4 = 0;
						ze._Crc32 = array[num4++] + array[num4++] * 256 + array[num4++] * 256 * 256 + array[num4++] * 256 * 256 * 256;
						ze._CompressedSize = (uint)(array[num4++] + array[num4++] * 256 + array[num4++] * 256 * 256 + array[num4++] * 256 * 256 * 256);
						ze._UncompressedSize = (uint)(array[num4++] + array[num4++] * 256 + array[num4++] * 256 * 256 + array[num4++] * 256 * 256 * 256);
					}
					flag = num6 != ze._CompressedSize;
					if (flag)
					{
						ze.ArchiveStream.Seek(-12L, SeekOrigin.Current);
						num6 += 4;
					}
				}
				ze.ArchiveStream.Seek(position, SeekOrigin.Begin);
				ze._LengthOfTrailer += (ze._InputUsesZip64 ? 24 : 16);
			}
			ze._CompressedFileDataSize = ze._CompressedSize;
			if ((ze._BitField & 1) == 1)
			{
				if (ze.Encryption == EncryptionAlgorithm.WinZipAes128 || ze.Encryption == EncryptionAlgorithm.WinZipAes256)
				{
					int keyStrengthInBits = GetKeyStrengthInBits(ze._Encryption_FromZipFile);
					ze._aesCrypto_forExtract = WinZipAesCrypto.ReadFromStream(null, keyStrengthInBits, ze.ArchiveStream);
					num += ze._aesCrypto_forExtract.SizeOfEncryptionMetadata - 10;
					ze._CompressedFileDataSize -= ze._aesCrypto_forExtract.SizeOfEncryptionMetadata;
					ze._LengthOfTrailer += 10;
				}
				else
				{
					ze._WeakEncryptionHeader = new byte[12];
					num += ReadWeakEncryptionHeader(ze._archiveStream, ze._WeakEncryptionHeader);
					ze._CompressedFileDataSize -= 12L;
				}
			}
			ze._LengthOfHeader = num;
			ze._TotalEntrySize = ze._LengthOfHeader + ze._CompressedFileDataSize + ze._LengthOfTrailer;
			return true;
		}

		internal static int ReadWeakEncryptionHeader(Stream s, byte[] buffer)
		{
			int num = s.Read(buffer, 0, 12);
			if (num != 12)
			{
				throw new ZipException($"Unexpected end of data at position 0x{s.Position:X8}");
			}
			return num;
		}

		private static bool IsNotValidSig(int signature)
		{
			return signature != 67324752;
		}

		/// <summary>
		///   Reads one <c>ZipEntry</c> from the given stream.  The content for
		///   the entry does not get decompressed or decrypted.  This method
		///   basically reads metadata, and seeks.
		/// </summary>
		/// <param name="zc">the ZipContainer this entry belongs to.</param>
		/// <param name="first">
		///   true of this is the first entry being read from the stream.
		/// </param>
		/// <returns>the <c>ZipEntry</c> read from the stream.</returns>
		internal static ZipEntry ReadEntry(ZipContainer zc, bool first)
		{
			ZipFile zipFile = zc.ZipFile;
			Stream readStream = zc.ReadStream;
			Encoding alternateEncoding = zc.AlternateEncoding;
			ZipEntry zipEntry = new ZipEntry();
			zipEntry._Source = ZipEntrySource.ZipFile;
			zipEntry._container = zc;
			zipEntry._archiveStream = readStream;
			zipFile?.OnReadEntry(true, null);
			if (first)
			{
				HandlePK00Prefix(readStream);
			}
			if (!ReadHeader(zipEntry, alternateEncoding))
			{
				return null;
			}
			zipEntry.__FileDataPosition = zipEntry.ArchiveStream.Position;
			readStream.Seek(zipEntry._CompressedFileDataSize + zipEntry._LengthOfTrailer, SeekOrigin.Current);
			HandleUnexpectedDataDescriptor(zipEntry);
			if (zipFile != null)
			{
				zipFile.OnReadBytes(zipEntry);
				zipFile.OnReadEntry(false, zipEntry);
			}
			return zipEntry;
		}

		internal static void HandlePK00Prefix(Stream s)
		{
			if (SharedUtilities.ReadInt(s) != 808471376)
			{
				s.Seek(-4L, SeekOrigin.Current);
			}
		}

		private static void HandleUnexpectedDataDescriptor(ZipEntry entry)
		{
			Stream archiveStream = entry.ArchiveStream;
			if ((uint)SharedUtilities.ReadInt(archiveStream) == entry._Crc32)
			{
				if (SharedUtilities.ReadInt(archiveStream) == entry._CompressedSize)
				{
					if (SharedUtilities.ReadInt(archiveStream) != entry._UncompressedSize)
					{
						archiveStream.Seek(-12L, SeekOrigin.Current);
					}
				}
				else
				{
					archiveStream.Seek(-8L, SeekOrigin.Current);
				}
			}
			else
			{
				archiveStream.Seek(-4L, SeekOrigin.Current);
			}
		}

		/// <summary>
		///   Finds a particular segment in the given extra field.
		///   This is used when modifying a previously-generated
		///   extra field, in particular when removing the AES crypto
		///   segment in the extra field.
		/// </summary>
		internal static int FindExtraFieldSegment(byte[] extra, int offx, ushort targetHeaderId)
		{
			short num;
			for (int i = offx; i + 3 < extra.Length; i += num)
			{
				if ((ushort)(extra[i++] + extra[i++] * 256) == targetHeaderId)
				{
					return i - 2;
				}
				num = (short)(extra[i++] + extra[i++] * 256);
			}
			return -1;
		}

		/// <summary>
		///   At current cursor position in the stream, read the extra
		///   field, and set the properties on the ZipEntry instance
		///   appropriately.  This can be called when processing the
		///   Extra field in the Central Directory, or in the local
		///   header.
		/// </summary>
		internal int ProcessExtraField(Stream s, short extraFieldLength)
		{
			int num = 0;
			if (extraFieldLength > 0)
			{
				byte[] array = (_Extra = new byte[extraFieldLength]);
				num = s.Read(array, 0, array.Length);
				long posn = s.Position - num;
				int num2 = 0;
				while (num2 + 3 < array.Length)
				{
					int num3 = num2;
					ushort num4 = (ushort)(array[num2++] + array[num2++] * 256);
					ushort num5 = (ushort)(array[num2++] + array[num2++] * 256);
					switch (num4)
					{
					case 10:
						num2 = ProcessExtraFieldWindowsTimes(array, num2, num5, posn);
						break;
					case 21589:
						num2 = ProcessExtraFieldUnixTimes(array, num2, num5, posn);
						break;
					case 22613:
						num2 = ProcessExtraFieldInfoZipTimes(array, num2, num5, posn);
						break;
					case 1:
						num2 = ProcessExtraFieldZip64(array, num2, num5, posn);
						break;
					case 39169:
						num2 = ProcessExtraFieldWinZipAes(array, num2, num5, posn);
						break;
					case 23:
						num2 = ProcessExtraFieldPkwareStrongEncryption(array, num2);
						break;
					}
					num2 = num3 + num5 + 4;
				}
			}
			return num;
		}

		private int ProcessExtraFieldPkwareStrongEncryption(byte[] Buffer, int j)
		{
			j += 2;
			_UnsupportedAlgorithmId = (ushort)(Buffer[j++] + Buffer[j++] * 256);
			_Encryption_FromZipFile = (_Encryption = EncryptionAlgorithm.Unsupported);
			return j;
		}

		private int ProcessExtraFieldWinZipAes(byte[] buffer, int j, ushort dataSize, long posn)
		{
			if (_CompressionMethod == 99)
			{
				if ((_BitField & 1) != 1)
				{
					throw new BadReadException($"  Inconsistent metadata at position 0x{posn:X16}");
				}
				_sourceIsEncrypted = true;
				if (dataSize != 7)
				{
					throw new BadReadException($"  Inconsistent size (0x{dataSize:X4}) in WinZip AES field at position 0x{posn:X16}");
				}
				_WinZipAesMethod = BitConverter.ToInt16(buffer, j);
				j += 2;
				if (_WinZipAesMethod != 1 && _WinZipAesMethod != 2)
				{
					throw new BadReadException($"  Unexpected vendor version number (0x{_WinZipAesMethod:X4}) for WinZip AES metadata at position 0x{posn:X16}");
				}
				short num = BitConverter.ToInt16(buffer, j);
				j += 2;
				if (num != 17729)
				{
					throw new BadReadException($"  Unexpected vendor ID (0x{num:X4}) for WinZip AES metadata at position 0x{posn:X16}");
				}
				int num2 = ((buffer[j] == 1) ? 128 : ((buffer[j] == 3) ? 256 : (-1)));
				if (num2 < 0)
				{
					throw new BadReadException($"Invalid key strength ({num2})");
				}
				_Encryption_FromZipFile = (_Encryption = ((num2 == 128) ? EncryptionAlgorithm.WinZipAes128 : EncryptionAlgorithm.WinZipAes256));
				j++;
				_CompressionMethod_FromZipFile = (_CompressionMethod = BitConverter.ToInt16(buffer, j));
				j += 2;
			}
			return j;
		}

		private int ProcessExtraFieldZip64(byte[] buffer, int j, ushort dataSize, long posn)
		{
			_InputUsesZip64 = true;
			if (dataSize > 28)
			{
				throw new BadReadException($"  Inconsistent size (0x{dataSize:X4}) for ZIP64 extra field at position 0x{posn:X16}");
			}
			int remainingData = dataSize;
			Func<long> func = delegate
			{
				if (remainingData < 8)
				{
					throw new BadReadException($"  Missing data for ZIP64 extra field, position 0x{posn:X16}");
				}
				long result = BitConverter.ToInt64(buffer, j);
				j += 8;
				remainingData -= 8;
				return result;
			};
			if (_UncompressedSize == uint.MaxValue)
			{
				_UncompressedSize = func();
			}
			if (_CompressedSize == uint.MaxValue)
			{
				_CompressedSize = func();
			}
			if (_RelativeOffsetOfLocalHeader == uint.MaxValue)
			{
				_RelativeOffsetOfLocalHeader = func();
			}
			if (_diskNumber == 65535 && remainingData >= 4)
			{
				_diskNumber = BitConverter.ToUInt32(buffer, j);
				j += 4;
				remainingData -= 4;
			}
			return j;
		}

		private int ProcessExtraFieldInfoZipTimes(byte[] buffer, int j, ushort dataSize, long posn)
		{
			if (dataSize != 12 && dataSize != 8)
			{
				throw new BadReadException($"  Unexpected size (0x{dataSize:X4}) for InfoZip v1 extra field at position 0x{posn:X16}");
			}
			int num = BitConverter.ToInt32(buffer, j);
			_Mtime = _unixEpoch.AddSeconds(num);
			j += 4;
			num = BitConverter.ToInt32(buffer, j);
			_Atime = _unixEpoch.AddSeconds(num);
			j += 4;
			_Ctime = DateTime.UtcNow;
			_ntfsTimesAreSet = true;
			_timestamp |= ZipEntryTimestamp.InfoZip1;
			return j;
		}

		private int ProcessExtraFieldUnixTimes(byte[] buffer, int j, ushort dataSize, long posn)
		{
			if (dataSize != 13 && dataSize != 9 && dataSize != 5)
			{
				throw new BadReadException($"  Unexpected size (0x{dataSize:X4}) for Extended Timestamp extra field at position 0x{posn:X16}");
			}
			int remainingData = dataSize;
			Func<DateTime> func = delegate
			{
				int num = BitConverter.ToInt32(buffer, j);
				j += 4;
				remainingData -= 4;
				return _unixEpoch.AddSeconds(num);
			};
			if (dataSize == 13 || _readExtraDepth > 0)
			{
				byte b = buffer[j++];
				remainingData--;
				if (((uint)b & (true ? 1u : 0u)) != 0 && remainingData >= 4)
				{
					_Mtime = func();
				}
				_Atime = (((b & 2u) != 0 && remainingData >= 4) ? func() : DateTime.UtcNow);
				_Ctime = (((b & 4u) != 0 && remainingData >= 4) ? func() : DateTime.UtcNow);
				_timestamp |= ZipEntryTimestamp.Unix;
				_ntfsTimesAreSet = true;
				_emitUnixTimes = true;
			}
			else
			{
				ReadExtraField();
			}
			return j;
		}

		private int ProcessExtraFieldWindowsTimes(byte[] buffer, int j, ushort dataSize, long posn)
		{
			if (dataSize != 32)
			{
				throw new BadReadException($"  Unexpected size (0x{dataSize:X4}) for NTFS times extra field at position 0x{posn:X16}");
			}
			j += 4;
			short num = (short)(buffer[j] + buffer[j + 1] * 256);
			short num2 = (short)(buffer[j + 2] + buffer[j + 3] * 256);
			j += 4;
			if (num == 1 && num2 == 24)
			{
				long fileTime = BitConverter.ToInt64(buffer, j);
				_Mtime = DateTime.FromFileTimeUtc(fileTime);
				j += 8;
				fileTime = BitConverter.ToInt64(buffer, j);
				_Atime = DateTime.FromFileTimeUtc(fileTime);
				j += 8;
				fileTime = BitConverter.ToInt64(buffer, j);
				_Ctime = DateTime.FromFileTimeUtc(fileTime);
				j += 8;
				_ntfsTimesAreSet = true;
				_timestamp |= ZipEntryTimestamp.Windows;
				_emitNtfsTimes = true;
			}
			return j;
		}

		internal void WriteCentralDirectoryEntry(Stream s)
		{
			byte[] array = new byte[8192];
			int num = 0;
			array[num++] = 80;
			array[num++] = 75;
			array[num++] = 1;
			array[num++] = 2;
			array[num++] = (byte)((uint)_VersionMadeBy & 0xFFu);
			array[num++] = (byte)((_VersionMadeBy & 0xFF00) >> 8);
			short num2 = (short)((VersionNeeded != 0) ? VersionNeeded : 20);
			if (!_OutputUsesZip64.HasValue)
			{
				_OutputUsesZip64 = _container.Zip64 == Zip64Option.Always;
			}
			short num3 = (short)(_OutputUsesZip64.Value ? 45 : num2);
			if (CompressionMethod == CompressionMethod.BZip2)
			{
				num3 = 46;
			}
			array[num++] = (byte)((uint)num3 & 0xFFu);
			array[num++] = (byte)((num3 & 0xFF00) >> 8);
			array[num++] = (byte)((uint)_BitField & 0xFFu);
			array[num++] = (byte)((_BitField & 0xFF00) >> 8);
			array[num++] = (byte)((uint)_CompressionMethod & 0xFFu);
			array[num++] = (byte)((_CompressionMethod & 0xFF00) >> 8);
			if (Encryption == EncryptionAlgorithm.WinZipAes128 || Encryption == EncryptionAlgorithm.WinZipAes256)
			{
				num -= 2;
				array[num++] = 99;
				array[num++] = 0;
			}
			array[num++] = (byte)((uint)_TimeBlob & 0xFFu);
			array[num++] = (byte)((_TimeBlob & 0xFF00) >> 8);
			array[num++] = (byte)((_TimeBlob & 0xFF0000) >> 16);
			array[num++] = (byte)((_TimeBlob & 0xFF000000u) >> 24);
			array[num++] = (byte)((uint)_Crc32 & 0xFFu);
			array[num++] = (byte)((_Crc32 & 0xFF00) >> 8);
			array[num++] = (byte)((_Crc32 & 0xFF0000) >> 16);
			array[num++] = (byte)((_Crc32 & 0xFF000000u) >> 24);
			int num4 = 0;
			if (_OutputUsesZip64.Value)
			{
				for (num4 = 0; num4 < 8; num4++)
				{
					array[num++] = byte.MaxValue;
				}
			}
			else
			{
				array[num++] = (byte)(_CompressedSize & 0xFF);
				array[num++] = (byte)((_CompressedSize & 0xFF00) >> 8);
				array[num++] = (byte)((_CompressedSize & 0xFF0000) >> 16);
				array[num++] = (byte)((_CompressedSize & 0xFF000000u) >> 24);
				array[num++] = (byte)(_UncompressedSize & 0xFF);
				array[num++] = (byte)((_UncompressedSize & 0xFF00) >> 8);
				array[num++] = (byte)((_UncompressedSize & 0xFF0000) >> 16);
				array[num++] = (byte)((_UncompressedSize & 0xFF000000u) >> 24);
			}
			byte[] encodedFileNameBytes = GetEncodedFileNameBytes();
			short num5 = (short)encodedFileNameBytes.Length;
			array[num++] = (byte)((uint)num5 & 0xFFu);
			array[num++] = (byte)((num5 & 0xFF00) >> 8);
			_presumeZip64 = _OutputUsesZip64.Value;
			_Extra = ConstructExtraField(true);
			short num6 = (short)((_Extra != null) ? _Extra.Length : 0);
			array[num++] = (byte)((uint)num6 & 0xFFu);
			array[num++] = (byte)((num6 & 0xFF00) >> 8);
			int num7 = ((_CommentBytes != null) ? _CommentBytes.Length : 0);
			num += 2;
			if (_container.ZipFile != null && _container.ZipFile.MaxOutputSegmentSize64 != 0)
			{
				if (_presumeZip64 || _diskNumber > 65535)
				{
					array[num++] = byte.MaxValue;
					array[num++] = byte.MaxValue;
				}
				else
				{
					array[num++] = (byte)(_diskNumber & 0xFFu);
					array[num++] = (byte)((_diskNumber & 0xFF00) >> 8);
				}
			}
			else
			{
				array[num++] = 0;
				array[num++] = 0;
			}
			array[num++] = (byte)(_IsText ? 1u : 0u);
			array[num++] = 0;
			array[num++] = (byte)((uint)_ExternalFileAttrs & 0xFFu);
			array[num++] = (byte)((_ExternalFileAttrs & 0xFF00) >> 8);
			array[num++] = (byte)((_ExternalFileAttrs & 0xFF0000) >> 16);
			array[num++] = (byte)((_ExternalFileAttrs & 0xFF000000u) >> 24);
			if (_presumeZip64 || _RelativeOffsetOfLocalHeader > uint.MaxValue)
			{
				array[num++] = byte.MaxValue;
				array[num++] = byte.MaxValue;
				array[num++] = byte.MaxValue;
				array[num++] = byte.MaxValue;
			}
			else
			{
				array[num++] = (byte)(_RelativeOffsetOfLocalHeader & 0xFF);
				array[num++] = (byte)((_RelativeOffsetOfLocalHeader & 0xFF00) >> 8);
				array[num++] = (byte)((_RelativeOffsetOfLocalHeader & 0xFF0000) >> 16);
				array[num++] = (byte)((_RelativeOffsetOfLocalHeader & 0xFF000000u) >> 24);
			}
			Buffer.BlockCopy(encodedFileNameBytes, 0, array, num, num5);
			num += num5;
			if (_Extra != null)
			{
				byte[] extra = _Extra;
				int srcOffset = 0;
				Buffer.BlockCopy(extra, srcOffset, array, num, num6);
				num += num6;
			}
			if (num7 != 0)
			{
				if (num7 + num > array.Length)
				{
					num7 = array.Length - num;
				}
				Buffer.BlockCopy(_CommentBytes, 0, array, num, num7);
				num += num7;
			}
			array[32] = (byte)((uint)num7 & 0xFFu);
			array[33] = (byte)((num7 & 0xFF00) >> 8);
			s.Write(array, 0, num);
		}

		private byte[] ConstructExtraField(bool forCentralDirectory)
		{
			List<byte[]> list = new List<byte[]>();
			if (_container.Zip64 == Zip64Option.Always || (_container.Zip64 == Zip64Option.AsNecessary && (!forCentralDirectory || _entryRequiresZip64.Value)))
			{
				int num = 4 + (forCentralDirectory ? 28 : 16);
				byte[] array = new byte[num];
				int num2 = 0;
				if (_presumeZip64 || forCentralDirectory)
				{
					array[num2++] = 1;
					array[num2++] = 0;
				}
				else
				{
					array[num2++] = 153;
					array[num2++] = 153;
				}
				array[num2++] = (byte)(num - 4);
				array[num2++] = 0;
				Array.Copy(BitConverter.GetBytes(_UncompressedSize), 0, array, num2, 8);
				num2 += 8;
				Array.Copy(BitConverter.GetBytes(_CompressedSize), 0, array, num2, 8);
				num2 += 8;
				if (forCentralDirectory)
				{
					Array.Copy(BitConverter.GetBytes(_RelativeOffsetOfLocalHeader), 0, array, num2, 8);
					num2 += 8;
					Array.Copy(BitConverter.GetBytes(_diskNumber), 0, array, num2, 4);
				}
				list.Add(array);
			}
			if (Encryption == EncryptionAlgorithm.WinZipAes128 || Encryption == EncryptionAlgorithm.WinZipAes256)
			{
				byte[] array = new byte[11];
				int num3 = 0;
				array[num3++] = 1;
				array[num3++] = 153;
				array[num3++] = 7;
				array[num3++] = 0;
				array[num3++] = 1;
				array[num3++] = 0;
				array[num3++] = 65;
				array[num3++] = 69;
				switch (GetKeyStrengthInBits(Encryption))
				{
				case 128:
					array[num3] = 1;
					break;
				case 256:
					array[num3] = 3;
					break;
				default:
					array[num3] = byte.MaxValue;
					break;
				}
				num3++;
				array[num3++] = (byte)((uint)_CompressionMethod & 0xFFu);
				array[num3++] = (byte)((uint)_CompressionMethod & 0xFF00u);
				list.Add(array);
			}
			if (_ntfsTimesAreSet && _emitNtfsTimes)
			{
				byte[] array = new byte[36];
				int num4 = 0;
				array[num4++] = 10;
				array[num4++] = 0;
				array[num4++] = 32;
				array[num4++] = 0;
				num4 += 4;
				array[num4++] = 1;
				array[num4++] = 0;
				array[num4++] = 24;
				array[num4++] = 0;
				Array.Copy(BitConverter.GetBytes(_Mtime.ToFileTime()), 0, array, num4, 8);
				num4 += 8;
				Array.Copy(BitConverter.GetBytes(_Atime.ToFileTime()), 0, array, num4, 8);
				num4 += 8;
				Array.Copy(BitConverter.GetBytes(_Ctime.ToFileTime()), 0, array, num4, 8);
				num4 += 8;
				list.Add(array);
			}
			if (_ntfsTimesAreSet && _emitUnixTimes)
			{
				int num5 = 9;
				if (!forCentralDirectory)
				{
					num5 += 8;
				}
				byte[] array = new byte[num5];
				int num6 = 0;
				array[num6++] = 85;
				array[num6++] = 84;
				array[num6++] = (byte)(num5 - 4);
				array[num6++] = 0;
				array[num6++] = 7;
				Array.Copy(BitConverter.GetBytes((int)(_Mtime - _unixEpoch).TotalSeconds), 0, array, num6, 4);
				num6 += 4;
				if (!forCentralDirectory)
				{
					Array.Copy(BitConverter.GetBytes((int)(_Atime - _unixEpoch).TotalSeconds), 0, array, num6, 4);
					num6 += 4;
					Array.Copy(BitConverter.GetBytes((int)(_Ctime - _unixEpoch).TotalSeconds), 0, array, num6, 4);
					num6 += 4;
				}
				list.Add(array);
			}
			byte[] array2 = null;
			if (list.Count > 0)
			{
				int num7 = 0;
				int num8 = 0;
				for (int i = 0; i < list.Count; i++)
				{
					num7 += list[i].Length;
				}
				array2 = new byte[num7];
				for (int i = 0; i < list.Count; i++)
				{
					Array.Copy(list[i], 0, array2, num8, list[i].Length);
					num8 += list[i].Length;
				}
			}
			return array2;
		}

		private string NormalizeFileName()
		{
			string text = FileName.Replace("\\", "/");
			if (_TrimVolumeFromFullyQualifiedPaths && FileName.Length >= 3 && FileName[1] == ':' && text[2] == '/')
			{
				return text.Substring(3);
			}
			if (FileName.Length >= 4 && text[0] == '/' && text[1] == '/')
			{
				int num = text.IndexOf('/', 2);
				if (num == -1)
				{
					throw new ArgumentException("The path for that entry appears to be badly formatted");
				}
				return text.Substring(num + 1);
			}
			if (FileName.Length >= 3 && text[0] == '.' && text[1] == '/')
			{
				return text.Substring(2);
			}
			return text;
		}

		/// <summary>
		///   generate and return a byte array that encodes the filename
		///   for the entry.
		/// </summary>
		/// <remarks>
		///   <para>
		///     side effects: generate and store into _CommentBytes the
		///     byte array for any comment attached to the entry. Also
		///     sets _actualEncoding to indicate the actual encoding
		///     used. The same encoding is used for both filename and
		///     comment.
		///   </para>
		/// </remarks>
		private byte[] GetEncodedFileNameBytes()
		{
			string text = NormalizeFileName();
			switch (AlternateEncodingUsage)
			{
			case ZipOption.Always:
				if (_Comment != null && _Comment.Length != 0)
				{
					_CommentBytes = AlternateEncoding.GetBytes(_Comment);
				}
				_actualEncoding = AlternateEncoding;
				return AlternateEncoding.GetBytes(text);
			case ZipOption.Default:
				if (_Comment != null && _Comment.Length != 0)
				{
					_CommentBytes = ibm437.GetBytes(_Comment);
				}
				_actualEncoding = ibm437;
				return ibm437.GetBytes(text);
			default:
			{
				byte[] bytes = ibm437.GetBytes(text);
				string @string = ibm437.GetString(bytes);
				_CommentBytes = null;
				if (@string != text)
				{
					bytes = AlternateEncoding.GetBytes(text);
					if (_Comment != null && _Comment.Length != 0)
					{
						_CommentBytes = AlternateEncoding.GetBytes(_Comment);
					}
					_actualEncoding = AlternateEncoding;
					return bytes;
				}
				_actualEncoding = ibm437;
				if (_Comment == null || _Comment.Length == 0)
				{
					return bytes;
				}
				byte[] bytes2 = ibm437.GetBytes(_Comment);
				if (ibm437.GetString(bytes2, 0, bytes2.Length) != Comment)
				{
					bytes = AlternateEncoding.GetBytes(text);
					_CommentBytes = AlternateEncoding.GetBytes(_Comment);
					_actualEncoding = AlternateEncoding;
					return bytes;
				}
				_CommentBytes = bytes2;
				return bytes;
			}
			}
		}

		private bool WantReadAgain()
		{
			if (_UncompressedSize < 16)
			{
				return false;
			}
			if (_CompressionMethod == 0)
			{
				return false;
			}
			if (CompressionLevel == CompressionLevel.None)
			{
				return false;
			}
			if (_CompressedSize < _UncompressedSize)
			{
				return false;
			}
			if (_Source == ZipEntrySource.Stream && !_sourceStream.CanSeek)
			{
				return false;
			}
			if (_aesCrypto_forWrite != null && CompressedSize - _aesCrypto_forWrite.SizeOfEncryptionMetadata <= UncompressedSize + 16)
			{
				return false;
			}
			if (_zipCrypto_forWrite != null && CompressedSize - 12 <= UncompressedSize)
			{
				return false;
			}
			return true;
		}

		private void MaybeUnsetCompressionMethodForWriting(int cycle)
		{
			if (cycle > 1)
			{
				_CompressionMethod = 0;
			}
			else if (IsDirectory)
			{
				_CompressionMethod = 0;
			}
			else
			{
				if (_Source == ZipEntrySource.ZipFile)
				{
					return;
				}
				if (_Source == ZipEntrySource.Stream)
				{
					if (_sourceStream != null && _sourceStream.CanSeek && _sourceStream.Length == 0L)
					{
						_CompressionMethod = 0;
						return;
					}
				}
				else if (_Source == ZipEntrySource.FileSystem && SharedUtilities.GetFileLength(LocalFileName) == 0L)
				{
					_CompressionMethod = 0;
					return;
				}
				if (SetCompression != null)
				{
					CompressionLevel = SetCompression(LocalFileName, _FileNameInArchive);
				}
				if (CompressionLevel == CompressionLevel.None && CompressionMethod == CompressionMethod.Deflate)
				{
					_CompressionMethod = 0;
				}
			}
		}

		internal void WriteHeader(Stream s, int cycle)
		{
			_future_ROLH = (s as CountingStream)?.ComputedPosition ?? s.Position;
			int num = 0;
			int num2 = 0;
			byte[] array = new byte[30];
			array[num2++] = 80;
			array[num2++] = 75;
			array[num2++] = 3;
			array[num2++] = 4;
			_presumeZip64 = _container.Zip64 == Zip64Option.Always || (_container.Zip64 == Zip64Option.AsNecessary && !s.CanSeek);
			short num3 = (short)(_presumeZip64 ? 45 : 20);
			if (CompressionMethod == CompressionMethod.BZip2)
			{
				num3 = 46;
			}
			array[num2++] = (byte)((uint)num3 & 0xFFu);
			array[num2++] = (byte)((num3 & 0xFF00) >> 8);
			byte[] encodedFileNameBytes = GetEncodedFileNameBytes();
			short num4 = (short)encodedFileNameBytes.Length;
			if (_Encryption == EncryptionAlgorithm.None)
			{
				_BitField &= -2;
			}
			else
			{
				_BitField |= 1;
			}
			if (_actualEncoding.CodePage == Encoding.UTF8.CodePage)
			{
				_BitField |= 2048;
			}
			if (IsDirectory || cycle == 99)
			{
				_BitField &= -9;
				_BitField &= -2;
				Encryption = EncryptionAlgorithm.None;
				Password = null;
			}
			else if (!s.CanSeek)
			{
				_BitField |= 8;
			}
			array[num2++] = (byte)((uint)_BitField & 0xFFu);
			array[num2++] = (byte)((_BitField & 0xFF00) >> 8);
			if (__FileDataPosition == -1)
			{
				_CompressedSize = 0L;
				_crcCalculated = false;
			}
			MaybeUnsetCompressionMethodForWriting(cycle);
			array[num2++] = (byte)((uint)_CompressionMethod & 0xFFu);
			array[num2++] = (byte)((_CompressionMethod & 0xFF00) >> 8);
			if (cycle == 99)
			{
				SetZip64Flags();
			}
			else if (Encryption == EncryptionAlgorithm.WinZipAes128 || Encryption == EncryptionAlgorithm.WinZipAes256)
			{
				num2 -= 2;
				array[num2++] = 99;
				array[num2++] = 0;
			}
			if (_dontEmitLastModified)
			{
				_TimeBlob = 0;
			}
			else
			{
				_TimeBlob = SharedUtilities.DateTimeToPacked(LastModified);
			}
			array[num2++] = (byte)((uint)_TimeBlob & 0xFFu);
			array[num2++] = (byte)((_TimeBlob & 0xFF00) >> 8);
			array[num2++] = (byte)((_TimeBlob & 0xFF0000) >> 16);
			array[num2++] = (byte)((_TimeBlob & 0xFF000000u) >> 24);
			array[num2++] = (byte)((uint)_Crc32 & 0xFFu);
			array[num2++] = (byte)((_Crc32 & 0xFF00) >> 8);
			array[num2++] = (byte)((_Crc32 & 0xFF0000) >> 16);
			array[num2++] = (byte)((_Crc32 & 0xFF000000u) >> 24);
			if (_presumeZip64)
			{
				for (num = 0; num < 8; num++)
				{
					array[num2++] = byte.MaxValue;
				}
			}
			else
			{
				array[num2++] = (byte)(_CompressedSize & 0xFF);
				array[num2++] = (byte)((_CompressedSize & 0xFF00) >> 8);
				array[num2++] = (byte)((_CompressedSize & 0xFF0000) >> 16);
				array[num2++] = (byte)((_CompressedSize & 0xFF000000u) >> 24);
				array[num2++] = (byte)(_UncompressedSize & 0xFF);
				array[num2++] = (byte)((_UncompressedSize & 0xFF00) >> 8);
				array[num2++] = (byte)((_UncompressedSize & 0xFF0000) >> 16);
				array[num2++] = (byte)((_UncompressedSize & 0xFF000000u) >> 24);
			}
			array[num2++] = (byte)((uint)num4 & 0xFFu);
			array[num2++] = (byte)((num4 & 0xFF00) >> 8);
			_Extra = ConstructExtraField(false);
			short num5 = (short)((_Extra != null) ? _Extra.Length : 0);
			array[num2++] = (byte)((uint)num5 & 0xFFu);
			array[num2++] = (byte)((num5 & 0xFF00) >> 8);
			byte[] array2 = new byte[num2 + num4 + num5];
			Buffer.BlockCopy(array, 0, array2, 0, num2);
			Buffer.BlockCopy(encodedFileNameBytes, 0, array2, num2, encodedFileNameBytes.Length);
			num2 += encodedFileNameBytes.Length;
			if (_Extra != null)
			{
				Buffer.BlockCopy(_Extra, 0, array2, num2, _Extra.Length);
				num2 += _Extra.Length;
			}
			_LengthOfHeader = num2;
			ZipSegmentedStream zipSegmentedStream = s as ZipSegmentedStream;
			if (zipSegmentedStream != null)
			{
				zipSegmentedStream.ContiguousWrite = true;
				uint num6 = zipSegmentedStream.ComputeSegment(num2);
				if (num6 != zipSegmentedStream.CurrentSegment)
				{
					_future_ROLH = 0L;
				}
				else
				{
					_future_ROLH = zipSegmentedStream.Position;
				}
				_diskNumber = num6;
			}
			if (_container.Zip64 == Zip64Option.Default && (uint)_RelativeOffsetOfLocalHeader >= uint.MaxValue)
			{
				throw new ZipException("Offset within the zip archive exceeds 0xFFFFFFFF. Consider setting the UseZip64WhenSaving property on the ZipFile instance.");
			}
			s.Write(array2, 0, num2);
			if (zipSegmentedStream != null)
			{
				zipSegmentedStream.ContiguousWrite = false;
			}
			_EntryHeader = array2;
		}

		private int FigureCrc32()
		{
			if (!_crcCalculated)
			{
				Stream stream = null;
				if (_Source == ZipEntrySource.WriteDelegate)
				{
					CrcCalculatorStream crcCalculatorStream = new CrcCalculatorStream(Stream.Null);
					_WriteDelegate(FileName, crcCalculatorStream);
					_Crc32 = crcCalculatorStream.Crc;
				}
				else if (_Source != ZipEntrySource.ZipFile)
				{
					if (_Source == ZipEntrySource.Stream)
					{
						PrepSourceStream();
						stream = _sourceStream;
					}
					else if (_Source == ZipEntrySource.JitStream)
					{
						if (_sourceStream == null)
						{
							_sourceStream = _OpenDelegate(FileName);
						}
						PrepSourceStream();
						stream = _sourceStream;
					}
					else if (_Source != ZipEntrySource.ZipOutputStream)
					{
						stream = File.Open(LocalFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
					}
					CRC32 cRC = new CRC32();
					_Crc32 = cRC.GetCrc32(stream);
					if (_sourceStream == null)
					{
						stream.Dispose();
					}
				}
				_crcCalculated = true;
			}
			return _Crc32;
		}

		/// <summary>
		///   Stores the position of the entry source stream, or, if the position is
		///   already stored, seeks to that position.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This method is called in prep for reading the source stream.  If PKZIP
		///   encryption is used, then we need to calc the CRC32 before doing the
		///   encryption, because the CRC is used in the 12th byte of the PKZIP
		///   encryption header.  So, we need to be able to seek backward in the source
		///   when saving the ZipEntry. This method is called from the place that
		///   calculates the CRC, and also from the method that does the encryption of
		///   the file data.
		/// </para>
		///
		/// <para>
		///   The first time through, this method sets the _sourceStreamOriginalPosition
		///   field. Subsequent calls to this method seek to that position.
		/// </para>
		/// </remarks>
		private void PrepSourceStream()
		{
			if (_sourceStream == null)
			{
				throw new ZipException($"The input stream is null for entry '{FileName}'.");
			}
			if (_sourceStreamOriginalPosition.HasValue)
			{
				_sourceStream.Position = _sourceStreamOriginalPosition.Value;
			}
			else if (_sourceStream.CanSeek)
			{
				_sourceStreamOriginalPosition = _sourceStream.Position;
			}
			else if (Encryption == EncryptionAlgorithm.PkzipWeak && _Source != ZipEntrySource.ZipFile && (_BitField & 8) != 8)
			{
				throw new ZipException("It is not possible to use PKZIP encryption on a non-seekable input stream");
			}
		}

		/// <summary>
		/// Copy metadata that may have been changed by the app.  We do this when
		/// resetting the zipFile instance.  If the app calls Save() on a ZipFile, then
		/// tries to party on that file some more, we may need to Reset() it , which
		/// means re-reading the entries and then copying the metadata.  I think.
		/// </summary>
		internal void CopyMetaData(ZipEntry source)
		{
			__FileDataPosition = source.__FileDataPosition;
			CompressionMethod = source.CompressionMethod;
			_CompressionMethod_FromZipFile = source._CompressionMethod_FromZipFile;
			_CompressedFileDataSize = source._CompressedFileDataSize;
			_UncompressedSize = source._UncompressedSize;
			_BitField = source._BitField;
			_Source = source._Source;
			_LastModified = source._LastModified;
			_Mtime = source._Mtime;
			_Atime = source._Atime;
			_Ctime = source._Ctime;
			_ntfsTimesAreSet = source._ntfsTimesAreSet;
			_emitUnixTimes = source._emitUnixTimes;
			_emitNtfsTimes = source._emitNtfsTimes;
		}

		private void OnWriteBlock(long bytesXferred, long totalBytesToXfer)
		{
			if (_container.ZipFile != null)
			{
				_ioOperationCanceled = _container.ZipFile.OnSaveBlock(this, bytesXferred, totalBytesToXfer);
			}
		}

		private void _WriteEntryData(Stream s)
		{
			Stream input = null;
			long _FileDataPosition = -1L;
			try
			{
				_FileDataPosition = s.Position;
			}
			catch (Exception)
			{
			}
			try
			{
				long num = SetInputAndFigureFileLength(ref input);
				CountingStream countingStream = new CountingStream(s);
				Stream stream;
				Stream stream2;
				if (num != 0L)
				{
					stream = MaybeApplyEncryption(countingStream);
					stream2 = MaybeApplyCompression(stream, num);
				}
				else
				{
					stream = (stream2 = countingStream);
				}
				CrcCalculatorStream crcCalculatorStream = new CrcCalculatorStream(stream2, true);
				if (_Source == ZipEntrySource.WriteDelegate)
				{
					_WriteDelegate(FileName, crcCalculatorStream);
				}
				else
				{
					byte[] array = new byte[BufferSize];
					int count;
					while ((count = SharedUtilities.ReadWithRetry(input, array, 0, array.Length, FileName)) != 0)
					{
						crcCalculatorStream.Write(array, 0, count);
						OnWriteBlock(crcCalculatorStream.TotalBytesSlurped, num);
						if (_ioOperationCanceled)
						{
							break;
						}
					}
				}
				FinishOutputStream(s, countingStream, stream, stream2, crcCalculatorStream);
			}
			finally
			{
				if (_Source == ZipEntrySource.JitStream)
				{
					if (_CloseDelegate != null)
					{
						_CloseDelegate(FileName, input);
					}
					_sourceStream = null;
				}
				else if (input is FileStream)
				{
					input.Dispose();
				}
			}
			if (!_ioOperationCanceled)
			{
				__FileDataPosition = _FileDataPosition;
				PostProcessOutput(s);
			}
		}

		/// <summary>
		///   Set the input stream and get its length, if possible.  The length is
		///   used for progress updates, AND, to allow an optimization in case of
		///   a stream/file of zero length. In that case we skip the Encrypt and
		///   compression Stream. (like DeflateStream or BZip2OutputStream)
		/// </summary>
		private long SetInputAndFigureFileLength(ref Stream input)
		{
			long result = -1L;
			if (_Source == ZipEntrySource.Stream)
			{
				PrepSourceStream();
				input = _sourceStream;
				try
				{
					result = _sourceStream.Length;
					return result;
				}
				catch (NotSupportedException)
				{
					return result;
				}
			}
			if (_Source == ZipEntrySource.ZipFile)
			{
				string password = ((_Encryption_FromZipFile == EncryptionAlgorithm.None) ? null : (_Password ?? _container.Password));
				_sourceStream = InternalOpenReader(password);
				PrepSourceStream();
				input = _sourceStream;
				result = _sourceStream.Length;
			}
			else
			{
				if (_Source == ZipEntrySource.JitStream)
				{
					if (_sourceStream == null)
					{
						_sourceStream = _OpenDelegate(FileName);
					}
					PrepSourceStream();
					input = _sourceStream;
					try
					{
						result = _sourceStream.Length;
						return result;
					}
					catch (NotSupportedException)
					{
						return result;
					}
				}
				if (_Source == ZipEntrySource.FileSystem)
				{
					FileShare share = FileShare.ReadWrite | FileShare.Delete;
					input = File.Open(LocalFileName, FileMode.Open, FileAccess.Read, share);
					result = input.Length;
				}
			}
			return result;
		}

		internal void FinishOutputStream(Stream s, CountingStream entryCounter, Stream encryptor, Stream compressor, CrcCalculatorStream output)
		{
			if (output != null)
			{
				output.Close();
				if (compressor is DeflateStream)
				{
					compressor.Close();
				}
				else if (compressor is BZip2OutputStream)
				{
					compressor.Close();
				}
				else if (compressor is ParallelBZip2OutputStream)
				{
					compressor.Close();
				}
				else if (compressor is ParallelDeflateOutputStream)
				{
					compressor.Close();
				}
				encryptor.Flush();
				encryptor.Close();
				_LengthOfTrailer = 0;
				_UncompressedSize = output.TotalBytesSlurped;
				if (encryptor is WinZipAesCipherStream winZipAesCipherStream && _UncompressedSize > 0)
				{
					s.Write(winZipAesCipherStream.FinalAuthentication, 0, 10);
					_LengthOfTrailer += 10;
				}
				_CompressedFileDataSize = entryCounter.BytesWritten;
				_CompressedSize = _CompressedFileDataSize;
				_Crc32 = output.Crc;
				StoreRelativeOffset();
			}
		}

		internal void PostProcessOutput(Stream s)
		{
			CountingStream countingStream = s as CountingStream;
			if (_UncompressedSize == 0L && _CompressedSize == 0L)
			{
				if (_Source == ZipEntrySource.ZipOutputStream)
				{
					return;
				}
				if (_Password != null)
				{
					int num = 0;
					if (Encryption == EncryptionAlgorithm.PkzipWeak)
					{
						num = 12;
					}
					else if (Encryption == EncryptionAlgorithm.WinZipAes128 || Encryption == EncryptionAlgorithm.WinZipAes256)
					{
						num = _aesCrypto_forWrite._Salt.Length + _aesCrypto_forWrite.GeneratedPV.Length;
					}
					if (_Source == ZipEntrySource.ZipOutputStream && !s.CanSeek)
					{
						throw new ZipException("Zero bytes written, encryption in use, and non-seekable output.");
					}
					if (Encryption != 0)
					{
						s.Seek(-1 * num, SeekOrigin.Current);
						s.SetLength(s.Position);
						countingStream?.Adjust(num);
						_LengthOfHeader -= num;
						__FileDataPosition -= num;
					}
					_Password = null;
					_BitField &= -2;
					int num2 = 6;
					_EntryHeader[num2++] = (byte)((uint)_BitField & 0xFFu);
					_EntryHeader[num2++] = (byte)((_BitField & 0xFF00) >> 8);
					if (Encryption == EncryptionAlgorithm.WinZipAes128 || Encryption == EncryptionAlgorithm.WinZipAes256)
					{
						short num3 = (short)(_EntryHeader[26] + _EntryHeader[27] * 256);
						int offx = 30 + num3;
						int num4 = FindExtraFieldSegment(_EntryHeader, offx, 39169);
						if (num4 >= 0)
						{
							_EntryHeader[num4++] = 153;
							_EntryHeader[num4++] = 153;
						}
					}
				}
				CompressionMethod = CompressionMethod.None;
				Encryption = EncryptionAlgorithm.None;
			}
			else if (_zipCrypto_forWrite != null || _aesCrypto_forWrite != null)
			{
				if (Encryption == EncryptionAlgorithm.PkzipWeak)
				{
					_CompressedSize += 12L;
				}
				else if (Encryption == EncryptionAlgorithm.WinZipAes128 || Encryption == EncryptionAlgorithm.WinZipAes256)
				{
					_CompressedSize += _aesCrypto_forWrite.SizeOfEncryptionMetadata;
				}
			}
			int num5 = 8;
			_EntryHeader[num5++] = (byte)((uint)_CompressionMethod & 0xFFu);
			_EntryHeader[num5++] = (byte)((_CompressionMethod & 0xFF00) >> 8);
			num5 = 14;
			_EntryHeader[num5++] = (byte)((uint)_Crc32 & 0xFFu);
			_EntryHeader[num5++] = (byte)((_Crc32 & 0xFF00) >> 8);
			_EntryHeader[num5++] = (byte)((_Crc32 & 0xFF0000) >> 16);
			_EntryHeader[num5++] = (byte)((_Crc32 & 0xFF000000u) >> 24);
			SetZip64Flags();
			short num6 = (short)(_EntryHeader[26] + _EntryHeader[27] * 256);
			short num7 = (short)(_EntryHeader[28] + _EntryHeader[29] * 256);
			if (_OutputUsesZip64.Value)
			{
				_EntryHeader[4] = 45;
				_EntryHeader[5] = 0;
				for (int i = 0; i < 8; i++)
				{
					_EntryHeader[num5++] = byte.MaxValue;
				}
				num5 = 30 + num6;
				_EntryHeader[num5++] = 1;
				_EntryHeader[num5++] = 0;
				num5 += 2;
				Array.Copy(BitConverter.GetBytes(_UncompressedSize), 0, _EntryHeader, num5, 8);
				num5 += 8;
				Array.Copy(BitConverter.GetBytes(_CompressedSize), 0, _EntryHeader, num5, 8);
			}
			else
			{
				_EntryHeader[4] = 20;
				_EntryHeader[5] = 0;
				num5 = 18;
				_EntryHeader[num5++] = (byte)(_CompressedSize & 0xFF);
				_EntryHeader[num5++] = (byte)((_CompressedSize & 0xFF00) >> 8);
				_EntryHeader[num5++] = (byte)((_CompressedSize & 0xFF0000) >> 16);
				_EntryHeader[num5++] = (byte)((_CompressedSize & 0xFF000000u) >> 24);
				_EntryHeader[num5++] = (byte)(_UncompressedSize & 0xFF);
				_EntryHeader[num5++] = (byte)((_UncompressedSize & 0xFF00) >> 8);
				_EntryHeader[num5++] = (byte)((_UncompressedSize & 0xFF0000) >> 16);
				_EntryHeader[num5++] = (byte)((_UncompressedSize & 0xFF000000u) >> 24);
				if (num7 != 0)
				{
					num5 = 30 + num6;
					if ((short)(_EntryHeader[num5 + 2] + _EntryHeader[num5 + 3] * 256) == 16)
					{
						_EntryHeader[num5++] = 153;
						_EntryHeader[num5++] = 153;
					}
				}
			}
			if (Encryption == EncryptionAlgorithm.WinZipAes128 || Encryption == EncryptionAlgorithm.WinZipAes256)
			{
				num5 = 8;
				_EntryHeader[num5++] = 99;
				_EntryHeader[num5++] = 0;
				num5 = 30 + num6;
				do
				{
					ushort num8 = (ushort)(_EntryHeader[num5] + _EntryHeader[num5 + 1] * 256);
					short num9 = (short)(_EntryHeader[num5 + 2] + _EntryHeader[num5 + 3] * 256);
					if (num8 != 39169)
					{
						num5 += num9 + 4;
						continue;
					}
					num5 += 9;
					_EntryHeader[num5++] = (byte)((uint)_CompressionMethod & 0xFFu);
					_EntryHeader[num5++] = (byte)((uint)_CompressionMethod & 0xFF00u);
				}
				while (num5 < num7 - 30 - num6);
			}
			if ((_BitField & 8) != 8 || (_Source == ZipEntrySource.ZipOutputStream && s.CanSeek))
			{
				if (s is ZipSegmentedStream zipSegmentedStream && _diskNumber != zipSegmentedStream.CurrentSegment)
				{
					using (Stream stream = ZipSegmentedStream.ForUpdate(_container.ZipFile.Name, _diskNumber))
					{
						stream.Seek(_RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
						stream.Write(_EntryHeader, 0, _EntryHeader.Length);
					}
				}
				else
				{
					s.Seek(_RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
					s.Write(_EntryHeader, 0, _EntryHeader.Length);
					countingStream?.Adjust(_EntryHeader.Length);
					s.Seek(_CompressedSize, SeekOrigin.Current);
				}
			}
			if ((_BitField & 8) == 8 && !IsDirectory)
			{
				byte[] array = new byte[16 + (_OutputUsesZip64.Value ? 8 : 0)];
				num5 = 0;
				Array.Copy(BitConverter.GetBytes(134695760), 0, array, num5, 4);
				num5 += 4;
				Array.Copy(BitConverter.GetBytes(_Crc32), 0, array, num5, 4);
				num5 += 4;
				if (_OutputUsesZip64.Value)
				{
					Array.Copy(BitConverter.GetBytes(_CompressedSize), 0, array, num5, 8);
					num5 += 8;
					Array.Copy(BitConverter.GetBytes(_UncompressedSize), 0, array, num5, 8);
					num5 += 8;
				}
				else
				{
					array[num5++] = (byte)(_CompressedSize & 0xFF);
					array[num5++] = (byte)((_CompressedSize & 0xFF00) >> 8);
					array[num5++] = (byte)((_CompressedSize & 0xFF0000) >> 16);
					array[num5++] = (byte)((_CompressedSize & 0xFF000000u) >> 24);
					array[num5++] = (byte)(_UncompressedSize & 0xFF);
					array[num5++] = (byte)((_UncompressedSize & 0xFF00) >> 8);
					array[num5++] = (byte)((_UncompressedSize & 0xFF0000) >> 16);
					array[num5++] = (byte)((_UncompressedSize & 0xFF000000u) >> 24);
				}
				s.Write(array, 0, array.Length);
				_LengthOfTrailer += array.Length;
			}
		}

		private void SetZip64Flags()
		{
			_entryRequiresZip64 = _CompressedSize >= uint.MaxValue || _UncompressedSize >= uint.MaxValue || _RelativeOffsetOfLocalHeader >= uint.MaxValue;
			if (_container.Zip64 == Zip64Option.Default && _entryRequiresZip64.Value)
			{
				throw new ZipException("Compressed or Uncompressed size, or offset exceeds the maximum value. Consider setting the UseZip64WhenSaving property on the ZipFile instance.");
			}
			_OutputUsesZip64 = _container.Zip64 == Zip64Option.Always || _entryRequiresZip64.Value;
		}

		/// <summary>
		///   Prepare the given stream for output - wrap it in a CountingStream, and
		///   then in a CRC stream, and an encryptor and deflator as appropriate.
		/// </summary>
		/// <remarks>
		///   <para>
		///     Previously this was used in ZipEntry.Write(), but in an effort to
		///     introduce some efficiencies in that method I've refactored to put the
		///     code inline.  This method still gets called by ZipOutputStream.
		///   </para>
		/// </remarks>
		internal void PrepOutputStream(Stream s, long streamLength, out CountingStream outputCounter, out Stream encryptor, out Stream compressor, out CrcCalculatorStream output)
		{
			outputCounter = new CountingStream(s);
			if (streamLength != 0L)
			{
				encryptor = MaybeApplyEncryption(outputCounter);
				compressor = MaybeApplyCompression(encryptor, streamLength);
			}
			else
			{
				encryptor = (compressor = outputCounter);
			}
			output = new CrcCalculatorStream(compressor, true);
		}

		private Stream MaybeApplyCompression(Stream s, long streamLength)
		{
			if (_CompressionMethod == 8 && CompressionLevel != 0)
			{
				if (_container.ParallelDeflateThreshold == 0L || (streamLength > _container.ParallelDeflateThreshold && _container.ParallelDeflateThreshold > 0))
				{
					if (_container.ParallelDeflater == null)
					{
						_container.ParallelDeflater = new ParallelDeflateOutputStream(s, CompressionLevel, _container.Strategy, true);
						if (_container.CodecBufferSize > 0)
						{
							_container.ParallelDeflater.BufferSize = _container.CodecBufferSize;
						}
						if (_container.ParallelDeflateMaxBufferPairs > 0)
						{
							_container.ParallelDeflater.MaxBufferPairs = _container.ParallelDeflateMaxBufferPairs;
						}
					}
					ParallelDeflateOutputStream parallelDeflater = _container.ParallelDeflater;
					parallelDeflater.Reset(s);
					return parallelDeflater;
				}
				DeflateStream deflateStream = new DeflateStream(s, CompressionMode.Compress, CompressionLevel, true);
				if (_container.CodecBufferSize > 0)
				{
					deflateStream.BufferSize = _container.CodecBufferSize;
				}
				deflateStream.Strategy = _container.Strategy;
				return deflateStream;
			}
			if (_CompressionMethod == 12)
			{
				if (_container.ParallelDeflateThreshold == 0L || (streamLength > _container.ParallelDeflateThreshold && _container.ParallelDeflateThreshold > 0))
				{
					return new ParallelBZip2OutputStream(s, true);
				}
				return new BZip2OutputStream(s, true);
			}
			return s;
		}

		private Stream MaybeApplyEncryption(Stream s)
		{
			if (Encryption == EncryptionAlgorithm.PkzipWeak)
			{
				return new ZipCipherStream(s, _zipCrypto_forWrite, CryptoMode.Encrypt);
			}
			if (Encryption == EncryptionAlgorithm.WinZipAes128 || Encryption == EncryptionAlgorithm.WinZipAes256)
			{
				return new WinZipAesCipherStream(s, _aesCrypto_forWrite, CryptoMode.Encrypt);
			}
			return s;
		}

		private void OnZipErrorWhileSaving(Exception e)
		{
			if (_container.ZipFile != null)
			{
				_ioOperationCanceled = _container.ZipFile.OnZipErrorSaving(this, e);
			}
		}

		internal void Write(Stream s)
		{
			CountingStream countingStream = s as CountingStream;
			ZipSegmentedStream zipSegmentedStream = s as ZipSegmentedStream;
			bool flag = false;
			do
			{
				try
				{
					if (_Source == ZipEntrySource.ZipFile && !_restreamRequiredOnSave)
					{
						CopyThroughOneEntry(s);
						break;
					}
					if (IsDirectory)
					{
						WriteHeader(s, 1);
						StoreRelativeOffset();
						_entryRequiresZip64 = _RelativeOffsetOfLocalHeader >= uint.MaxValue;
						_OutputUsesZip64 = _container.Zip64 == Zip64Option.Always || _entryRequiresZip64.Value;
						if (zipSegmentedStream != null)
						{
							_diskNumber = zipSegmentedStream.CurrentSegment;
						}
						break;
					}
					bool flag2 = true;
					int num = 0;
					do
					{
						num++;
						WriteHeader(s, num);
						WriteSecurityMetadata(s);
						_WriteEntryData(s);
						_TotalEntrySize = _LengthOfHeader + _CompressedFileDataSize + _LengthOfTrailer;
						flag2 = num <= 1 && s.CanSeek && WantReadAgain();
						if (flag2)
						{
							if (zipSegmentedStream != null)
							{
								zipSegmentedStream.TruncateBackward(_diskNumber, _RelativeOffsetOfLocalHeader);
							}
							else
							{
								s.Seek(_RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
							}
							s.SetLength(s.Position);
							countingStream?.Adjust(_TotalEntrySize);
						}
					}
					while (flag2);
					_skippedDuringSave = false;
					flag = true;
				}
				catch (Exception ex)
				{
					ZipErrorAction zipErrorAction = ZipErrorAction;
					int num2 = 0;
					while (true)
					{
						if (ZipErrorAction == ZipErrorAction.Throw)
						{
							throw;
						}
						if (ZipErrorAction == ZipErrorAction.Skip || ZipErrorAction == ZipErrorAction.Retry)
						{
							long num3 = countingStream?.ComputedPosition ?? s.Position;
							long num4 = num3 - _future_ROLH;
							if (num4 > 0)
							{
								s.Seek(num4, SeekOrigin.Current);
								long position = s.Position;
								s.SetLength(s.Position);
								countingStream?.Adjust(num3 - position);
							}
							if (ZipErrorAction == ZipErrorAction.Skip)
							{
								WriteStatus("Skipping file {0} (exception: {1})", LocalFileName, ex.ToString());
								_skippedDuringSave = true;
								flag = true;
							}
							else
							{
								ZipErrorAction = zipErrorAction;
							}
							break;
						}
						if (num2 > 0)
						{
							throw;
						}
						if (ZipErrorAction == ZipErrorAction.InvokeErrorEvent)
						{
							OnZipErrorWhileSaving(ex);
							if (_ioOperationCanceled)
							{
								flag = true;
								break;
							}
						}
						num2++;
					}
				}
			}
			while (!flag);
		}

		internal void StoreRelativeOffset()
		{
			_RelativeOffsetOfLocalHeader = _future_ROLH;
		}

		internal void NotifySaveComplete()
		{
			_Encryption_FromZipFile = _Encryption;
			_CompressionMethod_FromZipFile = _CompressionMethod;
			_restreamRequiredOnSave = false;
			_metadataChanged = false;
			_Source = ZipEntrySource.ZipFile;
		}

		internal void WriteSecurityMetadata(Stream outstream)
		{
			if (Encryption == EncryptionAlgorithm.None)
			{
				return;
			}
			string password = _Password;
			if (_Source == ZipEntrySource.ZipFile && password == null)
			{
				password = _container.Password;
			}
			if (password == null)
			{
				_zipCrypto_forWrite = null;
				_aesCrypto_forWrite = null;
			}
			else if (Encryption == EncryptionAlgorithm.PkzipWeak)
			{
				_zipCrypto_forWrite = ZipCrypto.ForWrite(password);
				Random random = new Random();
				byte[] array = new byte[12];
				random.NextBytes(array);
				if ((_BitField & 8) == 8)
				{
					if (_dontEmitLastModified)
					{
						_TimeBlob = 0;
					}
					else
					{
						_TimeBlob = SharedUtilities.DateTimeToPacked(LastModified);
					}
					array[11] = (byte)((uint)(_TimeBlob >> 8) & 0xFFu);
				}
				else
				{
					FigureCrc32();
					array[11] = (byte)((uint)(_Crc32 >> 24) & 0xFFu);
				}
				byte[] array2 = _zipCrypto_forWrite.EncryptMessage(array, array.Length);
				outstream.Write(array2, 0, array2.Length);
				_LengthOfHeader += array2.Length;
			}
			else if (Encryption == EncryptionAlgorithm.WinZipAes128 || Encryption == EncryptionAlgorithm.WinZipAes256)
			{
				int keyStrengthInBits = GetKeyStrengthInBits(Encryption);
				_aesCrypto_forWrite = WinZipAesCrypto.Generate(password, keyStrengthInBits);
				outstream.Write(_aesCrypto_forWrite.Salt, 0, _aesCrypto_forWrite._Salt.Length);
				outstream.Write(_aesCrypto_forWrite.GeneratedPV, 0, _aesCrypto_forWrite.GeneratedPV.Length);
				_LengthOfHeader += _aesCrypto_forWrite._Salt.Length + _aesCrypto_forWrite.GeneratedPV.Length;
			}
		}

		private void CopyThroughOneEntry(Stream outStream)
		{
			if (LengthOfHeader == 0)
			{
				throw new BadStateException("Bad header length.");
			}
			if (_metadataChanged || ArchiveStream is ZipSegmentedStream || outStream is ZipSegmentedStream || (_InputUsesZip64 && _container.UseZip64WhenSaving == Zip64Option.Default) || (!_InputUsesZip64 && _container.UseZip64WhenSaving == Zip64Option.Always))
			{
				CopyThroughWithRecompute(outStream);
			}
			else
			{
				CopyThroughWithNoChange(outStream);
			}
			_entryRequiresZip64 = _CompressedSize >= uint.MaxValue || _UncompressedSize >= uint.MaxValue || _RelativeOffsetOfLocalHeader >= uint.MaxValue;
			_OutputUsesZip64 = _container.Zip64 == Zip64Option.Always || _entryRequiresZip64.Value;
		}

		private void CopyThroughWithRecompute(Stream outstream)
		{
			byte[] array = new byte[BufferSize];
			CountingStream countingStream = new CountingStream(ArchiveStream);
			long relativeOffsetOfLocalHeader = _RelativeOffsetOfLocalHeader;
			int lengthOfHeader = LengthOfHeader;
			WriteHeader(outstream, 0);
			StoreRelativeOffset();
			if (!FileName.EndsWith("/"))
			{
				long num = relativeOffsetOfLocalHeader + lengthOfHeader;
				int lengthOfCryptoHeaderBytes = GetLengthOfCryptoHeaderBytes(_Encryption_FromZipFile);
				num -= lengthOfCryptoHeaderBytes;
				_LengthOfHeader += lengthOfCryptoHeaderBytes;
				countingStream.Seek(num, SeekOrigin.Begin);
				long num2 = _CompressedSize;
				while (num2 > 0)
				{
					lengthOfCryptoHeaderBytes = (int)((num2 > array.Length) ? array.Length : num2);
					int num3 = countingStream.Read(array, 0, lengthOfCryptoHeaderBytes);
					_CheckRead(num3);
					outstream.Write(array, 0, num3);
					num2 -= num3;
					OnWriteBlock(countingStream.BytesRead, _CompressedSize);
					if (_ioOperationCanceled)
					{
						break;
					}
				}
				if ((_BitField & 8) == 8)
				{
					int num4 = 16;
					if (_InputUsesZip64)
					{
						num4 += 8;
					}
					byte[] buffer = new byte[num4];
					countingStream.Read(buffer, 0, num4);
					if (_InputUsesZip64 && _container.UseZip64WhenSaving == Zip64Option.Default)
					{
						outstream.Write(buffer, 0, 8);
						if (_CompressedSize > uint.MaxValue)
						{
							throw new InvalidOperationException("ZIP64 is required");
						}
						outstream.Write(buffer, 8, 4);
						if (_UncompressedSize > uint.MaxValue)
						{
							throw new InvalidOperationException("ZIP64 is required");
						}
						outstream.Write(buffer, 16, 4);
						_LengthOfTrailer -= 8;
					}
					else if (!_InputUsesZip64 && _container.UseZip64WhenSaving == Zip64Option.Always)
					{
						byte[] buffer2 = new byte[4];
						outstream.Write(buffer, 0, 8);
						outstream.Write(buffer, 8, 4);
						outstream.Write(buffer2, 0, 4);
						outstream.Write(buffer, 12, 4);
						outstream.Write(buffer2, 0, 4);
						_LengthOfTrailer += 8;
					}
					else
					{
						outstream.Write(buffer, 0, num4);
					}
				}
			}
			_TotalEntrySize = _LengthOfHeader + _CompressedFileDataSize + _LengthOfTrailer;
		}

		private void CopyThroughWithNoChange(Stream outstream)
		{
			byte[] array = new byte[BufferSize];
			CountingStream countingStream = new CountingStream(ArchiveStream);
			countingStream.Seek(_RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
			if (_TotalEntrySize == 0L)
			{
				_TotalEntrySize = _LengthOfHeader + _CompressedFileDataSize + _LengthOfTrailer;
			}
			_RelativeOffsetOfLocalHeader = (outstream as CountingStream)?.ComputedPosition ?? outstream.Position;
			long num = _TotalEntrySize;
			while (num > 0)
			{
				int count = (int)((num > array.Length) ? array.Length : num);
				int num2 = countingStream.Read(array, 0, count);
				_CheckRead(num2);
				outstream.Write(array, 0, num2);
				num -= num2;
				OnWriteBlock(countingStream.BytesRead, _TotalEntrySize);
				if (_ioOperationCanceled)
				{
					break;
				}
			}
		}

		[Conditional("Trace")]
		private void TraceWriteLine(string format, params object[] varParams)
		{
			lock (_outputLock)
			{
				int hashCode = Thread.CurrentThread.GetHashCode();
				Console.ForegroundColor = (ConsoleColor)(hashCode % 8 + 8);
				Console.Write("{0:000} ZipEntry.Write ", hashCode);
				Console.WriteLine(format, varParams);
				Console.ResetColor();
			}
		}
	}
}
