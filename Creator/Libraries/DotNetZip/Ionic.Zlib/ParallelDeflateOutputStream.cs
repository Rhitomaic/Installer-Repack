using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Ionic.Crc;

namespace Ionic.Zlib
{
	/// <summary>
	///   A class for compressing streams using the
	///   Deflate algorithm with multiple threads.
	/// </summary>
	///
	/// <remarks>
	/// <para>
	///   This class performs DEFLATE compression through writing.  For
	///   more information on the Deflate algorithm, see IETF RFC 1951,
	///   "DEFLATE Compressed Data Format Specification version 1.3."
	/// </para>
	///
	/// <para>
	///   This class is similar to <see cref="T:Ionic.Zlib.DeflateStream" />, except
	///   that this class is for compression only, and this implementation uses an
	///   approach that employs multiple worker threads to perform the DEFLATE.  On
	///   a multi-cpu or multi-core computer, the performance of this class can be
	///   significantly higher than the single-threaded DeflateStream, particularly
	///   for larger streams.  How large?  Anything over 10mb is a good candidate
	///   for parallel compression.
	/// </para>
	///
	/// <para>
	///   The tradeoff is that this class uses more memory and more CPU than the
	///   vanilla DeflateStream, and also is less efficient as a compressor. For
	///   large files the size of the compressed data stream can be less than 1%
	///   larger than the size of a compressed data stream from the vanialla
	///   DeflateStream.  For smaller files the difference can be larger.  The
	///   difference will also be larger if you set the BufferSize to be lower than
	///   the default value.  Your mileage may vary. Finally, for small files, the
	///   ParallelDeflateOutputStream can be much slower than the vanilla
	///   DeflateStream, because of the overhead associated to using the thread
	///   pool.
	/// </para>
	///
	/// </remarks>
	/// <seealso cref="T:Ionic.Zlib.DeflateStream" />
	public class ParallelDeflateOutputStream : Stream
	{
		[Flags]
		private enum TraceBits : uint
		{
			None = 0u,
			NotUsed1 = 1u,
			EmitLock = 2u,
			EmitEnter = 4u,
			EmitBegin = 8u,
			EmitDone = 0x10u,
			EmitSkip = 0x20u,
			EmitAll = 0x3Au,
			Flush = 0x40u,
			Lifecycle = 0x80u,
			Session = 0x100u,
			Synch = 0x200u,
			Instance = 0x400u,
			Compress = 0x800u,
			Write = 0x1000u,
			WriteEnter = 0x2000u,
			WriteTake = 0x4000u,
			All = uint.MaxValue
		}

		private static readonly int IO_BUFFER_SIZE_DEFAULT = 65536;

		private static readonly int BufferPairsPerCore = 4;

		private List<WorkItem> _pool;

		private bool _leaveOpen;

		private bool emitting;

		private Stream _outStream;

		private int _maxBufferPairs;

		private int _bufferSize = IO_BUFFER_SIZE_DEFAULT;

		private AutoResetEvent _newlyCompressedBlob;

		private object _outputLock = new object();

		private bool _isClosed;

		private bool _firstWriteDone;

		private int _currentlyFilling;

		private int _lastFilled;

		private int _lastWritten;

		private int _latestCompressed;

		private int _Crc32;

		private CRC32 _runningCrc;

		private object _latestLock = new object();

		private Queue<int> _toWrite;

		private Queue<int> _toFill;

		private long _totalBytesProcessed;

		private CompressionLevel _compressLevel;

		private volatile Exception _pendingException;

		private bool _handlingException;

		private object _eLock = new object();

		private TraceBits _DesiredTrace = TraceBits.EmitAll | TraceBits.EmitEnter | TraceBits.Session | TraceBits.Compress | TraceBits.WriteEnter | TraceBits.WriteTake;

		/// <summary>
		///   The ZLIB strategy to be used during compression.
		/// </summary>
		public CompressionStrategy Strategy { get; private set; }

		/// <summary>
		///   The maximum number of buffer pairs to use.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This property sets an upper limit on the number of memory buffer
		///   pairs to create.  The implementation of this stream allocates
		///   multiple buffers to facilitate parallel compression.  As each buffer
		///   fills up, this stream uses <see cref="M:System.Threading.ThreadPool.QueueUserWorkItem(System.Threading.WaitCallback)">
		///   ThreadPool.QueueUserWorkItem()</see>
		///   to compress those buffers in a background threadpool thread. After a
		///   buffer is compressed, it is re-ordered and written to the output
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
		///   number of buffer pairs, S is the size of each buffer (<see cref="P:Ionic.Zlib.ParallelDeflateOutputStream.BufferSize" />).  By default, DotNetZip allocates 4 buffer
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
		///   The application can set this value at any time, but it is effective
		///   only before the first call to Write(), which is when the buffers are
		///   allocated.
		/// </para>
		/// </remarks>
		public int MaxBufferPairs
		{
			get
			{
				return _maxBufferPairs;
			}
			set
			{
				if (value < 4)
				{
					throw new ArgumentException("MaxBufferPairs", "Value must be 4 or greater.");
				}
				_maxBufferPairs = value;
			}
		}

		/// <summary>
		///   The size of the buffers used by the compressor threads.
		/// </summary>
		/// <remarks>
		///
		/// <para>
		///   The default buffer size is 128k. The application can set this value
		///   at any time, but it is effective only before the first Write().
		/// </para>
		///
		/// <para>
		///   Larger buffer sizes implies larger memory consumption but allows
		///   more efficient compression. Using smaller buffer sizes consumes less
		///   memory but may result in less effective compression.  For example,
		///   using the default buffer size of 128k, the compression delivered is
		///   within 1% of the compression delivered by the single-threaded <see cref="T:Ionic.Zlib.DeflateStream" />.  On the other hand, using a
		///   BufferSize of 8k can result in a compressed data stream that is 5%
		///   larger than that delivered by the single-threaded
		///   <c>DeflateStream</c>.  Excessively small buffer sizes can also cause
		///   the speed of the ParallelDeflateOutputStream to drop, because of
		///   larger thread scheduling overhead dealing with many many small
		///   buffers.
		/// </para>
		///
		/// <para>
		///   The total amount of storage space allocated for buffering will be
		///   (N*S*2), where N is the number of buffer pairs, and S is the size of
		///   each buffer (this property). There are 2 buffers used by the
		///   compressor, one for input and one for output.  By default, DotNetZip
		///   allocates 4 buffer pairs per CPU core, so if your machine has 4
		///   cores, then the number of buffer pairs used will be 16. If you
		///   accept the default value of this property, 128k, then the
		///   ParallelDeflateOutputStream will use 16 * 2 * 128kb of buffer memory
		///   in total, or 4mb, in blocks of 128kb.  If you set this property to
		///   64kb, then the number will be 16 * 2 * 64kb of buffer memory, or
		///   2mb.
		/// </para>
		///
		/// </remarks>
		public int BufferSize
		{
			get
			{
				return _bufferSize;
			}
			set
			{
				if (value < 1024)
				{
					throw new ArgumentOutOfRangeException("BufferSize", "BufferSize must be greater than 1024 bytes");
				}
				_bufferSize = value;
			}
		}

		/// <summary>
		/// The CRC32 for the data that was written out, prior to compression.
		/// </summary>
		/// <remarks>
		/// This value is meaningful only after a call to Close().
		/// </remarks>
		public int Crc32 => _Crc32;

		/// <summary>
		/// The total number of uncompressed bytes processed by the ParallelDeflateOutputStream.
		/// </summary>
		/// <remarks>
		/// This value is meaningful only after a call to Close().
		/// </remarks>
		public long BytesProcessed => _totalBytesProcessed;

		/// <summary>
		/// Indicates whether the stream supports Seek operations.
		/// </summary>
		/// <remarks>
		/// Always returns false.
		/// </remarks>
		public override bool CanSeek => false;

		/// <summary>
		/// Indicates whether the stream supports Read operations.
		/// </summary>
		/// <remarks>
		/// Always returns false.
		/// </remarks>
		public override bool CanRead => false;

		/// <summary>
		/// Indicates whether the stream supports Write operations.
		/// </summary>
		/// <remarks>
		/// Returns true if the provided stream is writable.
		/// </remarks>
		public override bool CanWrite => _outStream.CanWrite;

		/// <summary>
		/// Reading this property always throws a NotSupportedException.
		/// </summary>
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Returns the current position of the output stream.
		/// </summary>
		/// <remarks>
		///   <para>
		///     Because the output gets written by a background thread,
		///     the value may change asynchronously.  Setting this
		///     property always throws a NotSupportedException.
		///   </para>
		/// </remarks>
		public override long Position
		{
			get
			{
				return _outStream.Position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Create a ParallelDeflateOutputStream.
		/// </summary>
		/// <remarks>
		///
		/// <para>
		///   This stream compresses data written into it via the DEFLATE
		///   algorithm (see RFC 1951), and writes out the compressed byte stream.
		/// </para>
		///
		/// <para>
		///   The instance will use the default compression level, the default
		///   buffer sizes and the default number of threads and buffers per
		///   thread.
		/// </para>
		///
		/// <para>
		///   This class is similar to <see cref="T:Ionic.Zlib.DeflateStream" />,
		///   except that this implementation uses an approach that employs
		///   multiple worker threads to perform the DEFLATE.  On a multi-cpu or
		///   multi-core computer, the performance of this class can be
		///   significantly higher than the single-threaded DeflateStream,
		///   particularly for larger streams.  How large?  Anything over 10mb is
		///   a good candidate for parallel compression.
		/// </para>
		///
		/// </remarks>
		///
		/// <example>
		///
		/// This example shows how to use a ParallelDeflateOutputStream to compress
		/// data.  It reads a file, compresses it, and writes the compressed data to
		/// a second, output file.
		///
		/// <code>
		/// byte[] buffer = new byte[WORKING_BUFFER_SIZE];
		/// int n= -1;
		/// String outputFile = fileToCompress + ".compressed";
		/// using (System.IO.Stream input = System.IO.File.OpenRead(fileToCompress))
		/// {
		///     using (var raw = System.IO.File.Create(outputFile))
		///     {
		///         using (Stream compressor = new ParallelDeflateOutputStream(raw))
		///         {
		///             while ((n= input.Read(buffer, 0, buffer.Length)) != 0)
		///             {
		///                 compressor.Write(buffer, 0, n);
		///             }
		///         }
		///     }
		/// }
		/// </code>
		/// <code lang="VB">
		/// Dim buffer As Byte() = New Byte(4096) {}
		/// Dim n As Integer = -1
		/// Dim outputFile As String = (fileToCompress &amp; ".compressed")
		/// Using input As Stream = File.OpenRead(fileToCompress)
		///     Using raw As FileStream = File.Create(outputFile)
		///         Using compressor As Stream = New ParallelDeflateOutputStream(raw)
		///             Do While (n &lt;&gt; 0)
		///                 If (n &gt; 0) Then
		///                     compressor.Write(buffer, 0, n)
		///                 End If
		///                 n = input.Read(buffer, 0, buffer.Length)
		///             Loop
		///         End Using
		///     End Using
		/// End Using
		/// </code>
		/// </example>
		/// <param name="stream">The stream to which compressed data will be written.</param>
		public ParallelDeflateOutputStream(Stream stream)
			: this(stream, CompressionLevel.Default, CompressionStrategy.Default, false)
		{
		}

		/// <summary>
		///   Create a ParallelDeflateOutputStream using the specified CompressionLevel.
		/// </summary>
		/// <remarks>
		///   See the <see cref="M:Ionic.Zlib.ParallelDeflateOutputStream.#ctor(System.IO.Stream)" />
		///   constructor for example code.
		/// </remarks>
		/// <param name="stream">The stream to which compressed data will be written.</param>
		/// <param name="level">A tuning knob to trade speed for effectiveness.</param>
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level)
			: this(stream, level, CompressionStrategy.Default, false)
		{
		}

		/// <summary>
		/// Create a ParallelDeflateOutputStream and specify whether to leave the captive stream open
		/// when the ParallelDeflateOutputStream is closed.
		/// </summary>
		/// <remarks>
		///   See the <see cref="M:Ionic.Zlib.ParallelDeflateOutputStream.#ctor(System.IO.Stream)" />
		///   constructor for example code.
		/// </remarks>
		/// <param name="stream">The stream to which compressed data will be written.</param>
		/// <param name="leaveOpen">
		///    true if the application would like the stream to remain open after inflation/deflation.
		/// </param>
		public ParallelDeflateOutputStream(Stream stream, bool leaveOpen)
			: this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		/// <summary>
		/// Create a ParallelDeflateOutputStream and specify whether to leave the captive stream open
		/// when the ParallelDeflateOutputStream is closed.
		/// </summary>
		/// <remarks>
		///   See the <see cref="M:Ionic.Zlib.ParallelDeflateOutputStream.#ctor(System.IO.Stream)" />
		///   constructor for example code.
		/// </remarks>
		/// <param name="stream">The stream to which compressed data will be written.</param>
		/// <param name="level">A tuning knob to trade speed for effectiveness.</param>
		/// <param name="leaveOpen">
		///    true if the application would like the stream to remain open after inflation/deflation.
		/// </param>
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, bool leaveOpen)
			: this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		/// <summary>
		/// Create a ParallelDeflateOutputStream using the specified
		/// CompressionLevel and CompressionStrategy, and specifying whether to
		/// leave the captive stream open when the ParallelDeflateOutputStream is
		/// closed.
		/// </summary>
		/// <remarks>
		///   See the <see cref="M:Ionic.Zlib.ParallelDeflateOutputStream.#ctor(System.IO.Stream)" />
		///   constructor for example code.
		/// </remarks>
		/// <param name="stream">The stream to which compressed data will be written.</param>
		/// <param name="level">A tuning knob to trade speed for effectiveness.</param>
		/// <param name="strategy">
		///   By tweaking this parameter, you may be able to optimize the compression for
		///   data with particular characteristics.
		/// </param>
		/// <param name="leaveOpen">
		///    true if the application would like the stream to remain open after inflation/deflation.
		/// </param>
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, CompressionStrategy strategy, bool leaveOpen)
		{
			_outStream = stream;
			_compressLevel = level;
			Strategy = strategy;
			_leaveOpen = leaveOpen;
			MaxBufferPairs = 16;
		}

		private void _InitializePoolOfWorkItems()
		{
			_toWrite = new Queue<int>();
			_toFill = new Queue<int>();
			_pool = new List<WorkItem>();
			int val = BufferPairsPerCore * Environment.ProcessorCount;
			val = Math.Min(val, _maxBufferPairs);
			for (int i = 0; i < val; i++)
			{
				_pool.Add(new WorkItem(_bufferSize, _compressLevel, Strategy, i));
				_toFill.Enqueue(i);
			}
			_newlyCompressedBlob = new AutoResetEvent(false);
			_runningCrc = new CRC32();
			_currentlyFilling = -1;
			_lastFilled = -1;
			_lastWritten = -1;
			_latestCompressed = -1;
		}

		/// <summary>
		///   Write data to the stream.
		/// </summary>
		///
		/// <remarks>
		///
		/// <para>
		///   To use the ParallelDeflateOutputStream to compress data, create a
		///   ParallelDeflateOutputStream with CompressionMode.Compress, passing a
		///   writable output stream.  Then call Write() on that
		///   ParallelDeflateOutputStream, providing uncompressed data as input.  The
		///   data sent to the output stream will be the compressed form of the data
		///   written.
		/// </para>
		///
		/// <para>
		///   To decompress data, use the <see cref="T:Ionic.Zlib.DeflateStream" /> class.
		/// </para>
		///
		/// </remarks>
		/// <param name="buffer">The buffer holding data to write to the stream.</param>
		/// <param name="offset">the offset within that data array to find the first byte to write.</param>
		/// <param name="count">the number of bytes to write.</param>
		public override void Write(byte[] buffer, int offset, int count)
		{
			bool mustWait = false;
			if (_isClosed)
			{
				throw new InvalidOperationException();
			}
			if (_pendingException != null)
			{
				_handlingException = true;
				Exception pendingException = _pendingException;
				_pendingException = null;
				throw pendingException;
			}
			if (count == 0)
			{
				return;
			}
			if (!_firstWriteDone)
			{
				_InitializePoolOfWorkItems();
				_firstWriteDone = true;
			}
			do
			{
				EmitPendingBuffers(false, mustWait);
				mustWait = false;
				int num = -1;
				if (_currentlyFilling >= 0)
				{
					num = _currentlyFilling;
				}
				else
				{
					if (_toFill.Count == 0)
					{
						mustWait = true;
						continue;
					}
					num = _toFill.Dequeue();
					_lastFilled++;
				}
				WorkItem workItem = _pool[num];
				int num2 = ((workItem.buffer.Length - workItem.inputBytesAvailable > count) ? count : (workItem.buffer.Length - workItem.inputBytesAvailable));
				workItem.ordinal = _lastFilled;
				Buffer.BlockCopy(buffer, offset, workItem.buffer, workItem.inputBytesAvailable, num2);
				count -= num2;
				offset += num2;
				workItem.inputBytesAvailable += num2;
				if (workItem.inputBytesAvailable == workItem.buffer.Length)
				{
					if (!ThreadPool.QueueUserWorkItem(_DeflateOne, workItem))
					{
						throw new Exception("Cannot enqueue workitem");
					}
					_currentlyFilling = -1;
				}
				else
				{
					_currentlyFilling = num;
				}
				_ = 0;
			}
			while (count > 0);
		}

		private void _FlushFinish()
		{
			byte[] array = new byte[128];
			ZlibCodec zlibCodec = new ZlibCodec();
			int num = zlibCodec.InitializeDeflate(_compressLevel, false);
			zlibCodec.InputBuffer = null;
			zlibCodec.NextIn = 0;
			zlibCodec.AvailableBytesIn = 0;
			zlibCodec.OutputBuffer = array;
			zlibCodec.NextOut = 0;
			zlibCodec.AvailableBytesOut = array.Length;
			num = zlibCodec.Deflate(FlushType.Finish);
			if (num != 1 && num != 0)
			{
				throw new Exception("deflating: " + zlibCodec.Message);
			}
			if (array.Length - zlibCodec.AvailableBytesOut > 0)
			{
				_outStream.Write(array, 0, array.Length - zlibCodec.AvailableBytesOut);
			}
			zlibCodec.EndDeflate();
			_Crc32 = _runningCrc.Crc32Result;
		}

		private void _Flush(bool lastInput)
		{
			if (_isClosed)
			{
				throw new InvalidOperationException();
			}
			if (!emitting)
			{
				if (_currentlyFilling >= 0)
				{
					WorkItem wi = _pool[_currentlyFilling];
					_DeflateOne(wi);
					_currentlyFilling = -1;
				}
				if (lastInput)
				{
					EmitPendingBuffers(true, false);
					_FlushFinish();
				}
				else
				{
					EmitPendingBuffers(false, false);
				}
			}
		}

		/// <summary>
		/// Flush the stream.
		/// </summary>
		public override void Flush()
		{
			if (_pendingException != null)
			{
				_handlingException = true;
				Exception pendingException = _pendingException;
				_pendingException = null;
				throw pendingException;
			}
			if (!_handlingException)
			{
				_Flush(false);
			}
		}

		/// <summary>
		/// Close the stream.
		/// </summary>
		/// <remarks>
		/// You must call Close on the stream to guarantee that all of the data written in has
		/// been compressed, and the compressed data has been written out.
		/// </remarks>
		public override void Close()
		{
			InnerClose();
		}

		private void InnerClose()
		{
			if (_pendingException != null)
			{
				_handlingException = true;
				Exception pendingException = _pendingException;
				_pendingException = null;
				throw pendingException;
			}
			if (!_handlingException && !_isClosed)
			{
				_Flush(true);
				if (!_leaveOpen)
				{
					_outStream.Dispose();
				}
				_isClosed = true;
			}
		}

		/// <summary>Dispose the object</summary>
		/// <remarks>
		///   <para>
		///     Because ParallelDeflateOutputStream is IDisposable, the
		///     application must call this method when finished using the instance.
		///   </para>
		///   <para>
		///     This method is generally called implicitly upon exit from
		///     a <c>using</c> scope in C# (<c>Using</c> in VB).
		///   </para>
		/// </remarks>
		public new void Dispose()
		{
			_pool = null;
			Dispose(true);
		}

		/// <summary>The Dispose method</summary>
		/// <param name="disposing">
		///   indicates whether the Dispose method was invoked by user code.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			InnerClose();
		}

		/// <summary>
		///   Resets the stream for use with another stream.
		/// </summary>
		/// <remarks>
		///   Because the ParallelDeflateOutputStream is expensive to create, it
		///   has been designed so that it can be recycled and re-used.  You have
		///   to call Close() on the stream first, then you can call Reset() on
		///   it, to use it again on another stream.
		/// </remarks>
		///
		/// <param name="stream">
		///   The new output stream for this era.
		/// </param>
		///
		/// <example>
		/// <code>
		/// ParallelDeflateOutputStream deflater = null;
		/// foreach (var inputFile in listOfFiles)
		/// {
		///     string outputFile = inputFile + ".compressed";
		///     using (System.IO.Stream input = System.IO.File.OpenRead(inputFile))
		///     {
		///         using (var outStream = System.IO.File.Create(outputFile))
		///         {
		///             if (deflater == null)
		///                 deflater = new ParallelDeflateOutputStream(outStream,
		///                                                            CompressionLevel.Best,
		///                                                            CompressionStrategy.Default,
		///                                                            true);
		///             deflater.Reset(outStream);
		///
		///             while ((n= input.Read(buffer, 0, buffer.Length)) != 0)
		///             {
		///                 deflater.Write(buffer, 0, n);
		///             }
		///         }
		///     }
		/// }
		/// </code>
		/// </example>
		public void Reset(Stream stream)
		{
			if (!_firstWriteDone)
			{
				return;
			}
			_toWrite.Clear();
			_toFill.Clear();
			foreach (WorkItem item in _pool)
			{
				_toFill.Enqueue(item.index);
				item.ordinal = -1;
			}
			_firstWriteDone = false;
			_totalBytesProcessed = 0L;
			_runningCrc = new CRC32();
			_isClosed = false;
			_currentlyFilling = -1;
			_lastFilled = -1;
			_lastWritten = -1;
			_latestCompressed = -1;
			_outStream = stream;
		}

		private void EmitPendingBuffers(bool doAll, bool mustWait)
		{
			if (emitting)
			{
				return;
			}
			emitting = true;
			if ((doAll && _latestCompressed != _lastFilled) || mustWait)
			{
				_newlyCompressedBlob.WaitOne();
			}
			do
			{
				int num = -1;
				int num2 = (doAll ? 200 : (mustWait ? (-1) : 0));
				int num3 = -1;
				do
				{
					if (Monitor.TryEnter(_toWrite, num2))
					{
						num3 = -1;
						try
						{
							if (_toWrite.Count > 0)
							{
								num3 = _toWrite.Dequeue();
							}
						}
						finally
						{
							Monitor.Exit(_toWrite);
						}
						if (num3 < 0)
						{
							continue;
						}
						WorkItem workItem = _pool[num3];
						if (workItem.ordinal != _lastWritten + 1)
						{
							lock (_toWrite)
							{
								_toWrite.Enqueue(num3);
							}
							if (num == num3)
							{
								_newlyCompressedBlob.WaitOne();
								num = -1;
							}
							else if (num == -1)
							{
								num = num3;
							}
							continue;
						}
						num = -1;
						_outStream.Write(workItem.compressed, 0, workItem.compressedBytesAvailable);
						_runningCrc.Combine(workItem.crc, workItem.inputBytesAvailable);
						_totalBytesProcessed += workItem.inputBytesAvailable;
						workItem.inputBytesAvailable = 0;
						_lastWritten = workItem.ordinal;
						_toFill.Enqueue(workItem.index);
						if (num2 == -1)
						{
							num2 = 0;
						}
					}
					else
					{
						num3 = -1;
					}
				}
				while (num3 >= 0);
			}
			while (doAll && (_lastWritten != _latestCompressed || _lastWritten != _lastFilled));
			emitting = false;
		}

		private void _DeflateOne(object wi)
		{
			WorkItem workItem = (WorkItem)wi;
			try
			{
				_ = workItem.index;
				CRC32 cRC = new CRC32();
				cRC.SlurpBlock(workItem.buffer, 0, workItem.inputBytesAvailable);
				DeflateOneSegment(workItem);
				workItem.crc = cRC.Crc32Result;
				lock (_latestLock)
				{
					if (workItem.ordinal > _latestCompressed)
					{
						_latestCompressed = workItem.ordinal;
					}
				}
				lock (_toWrite)
				{
					_toWrite.Enqueue(workItem.index);
				}
				_newlyCompressedBlob.Set();
			}
			catch (Exception pendingException)
			{
				lock (_eLock)
				{
					if (_pendingException != null)
					{
						_pendingException = pendingException;
					}
				}
			}
		}

		private bool DeflateOneSegment(WorkItem workitem)
		{
			ZlibCodec compressor = workitem.compressor;
			compressor.ResetDeflate();
			compressor.NextIn = 0;
			compressor.AvailableBytesIn = workitem.inputBytesAvailable;
			compressor.NextOut = 0;
			compressor.AvailableBytesOut = workitem.compressed.Length;
			do
			{
				compressor.Deflate(FlushType.None);
			}
			while (compressor.AvailableBytesIn > 0 || compressor.AvailableBytesOut == 0);
			compressor.Deflate(FlushType.Sync);
			workitem.compressedBytesAvailable = (int)compressor.TotalBytesOut;
			return true;
		}

		[Conditional("Trace")]
		private void TraceOutput(TraceBits bits, string format, params object[] varParams)
		{
			if ((bits & _DesiredTrace) != 0)
			{
				lock (_outputLock)
				{
					int hashCode = Thread.CurrentThread.GetHashCode();
					Console.ForegroundColor = (ConsoleColor)(hashCode % 8 + 8);
					Console.Write("{0:000} PDOS ", hashCode);
					Console.WriteLine(format, varParams);
					Console.ResetColor();
				}
			}
		}

		/// <summary>
		/// This method always throws a NotSupportedException.
		/// </summary>
		/// <param name="buffer">
		///   The buffer into which data would be read, IF THIS METHOD
		///   ACTUALLY DID ANYTHING.
		/// </param>
		/// <param name="offset">
		///   The offset within that data array at which to insert the
		///   data that is read, IF THIS METHOD ACTUALLY DID
		///   ANYTHING.
		/// </param>
		/// <param name="count">
		///   The number of bytes to write, IF THIS METHOD ACTUALLY DID
		///   ANYTHING.
		/// </param>
		/// <returns>nothing.</returns>
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// This method always throws a NotSupportedException.
		/// </summary>
		/// <param name="offset">
		///   The offset to seek to....
		///   IF THIS METHOD ACTUALLY DID ANYTHING.
		/// </param>
		/// <param name="origin">
		///   The reference specifying how to apply the offset....  IF
		///   THIS METHOD ACTUALLY DID ANYTHING.
		/// </param>
		/// <returns>nothing. It always throws.</returns>
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// This method always throws a NotSupportedException.
		/// </summary>
		/// <param name="value">
		///   The new value for the stream length....  IF
		///   THIS METHOD ACTUALLY DID ANYTHING.
		/// </param>
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}
	}
}
