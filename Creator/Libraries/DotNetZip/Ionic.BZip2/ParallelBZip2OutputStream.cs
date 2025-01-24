using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Ionic.BZip2
{
	/// <summary>
	///   A write-only decorator stream that compresses data as it is
	///   written using the BZip2 algorithm. This stream compresses by
	///   block using multiple threads.
	/// </summary>
	/// <para>
	///   This class performs BZIP2 compression through writing.  For
	///   more information on the BZIP2 algorithm, see
	///   <see href="http://en.wikipedia.org/wiki/BZIP2" />.
	/// </para>
	///
	/// <para>
	///   This class is similar to <see cref="T:Ionic.BZip2.BZip2OutputStream" />,
	///   except that this implementation uses an approach that employs multiple
	///   worker threads to perform the compression.  On a multi-cpu or multi-core
	///   computer, the performance of this class can be significantly higher than
	///   the single-threaded BZip2OutputStream, particularly for larger streams.
	///   How large?  Anything over 10mb is a good candidate for parallel
	///   compression.
	/// </para>
	///
	/// <para>
	///   The tradeoff is that this class uses more memory and more CPU than the
	///   vanilla <c>BZip2OutputStream</c>. Also, for small files, the
	///   <c>ParallelBZip2OutputStream</c> can be much slower than the vanilla
	///   <c>BZip2OutputStream</c>, because of the overhead associated to using the
	///   thread pool.
	/// </para>
	///
	/// <seealso cref="T:Ionic.BZip2.BZip2OutputStream" />
	public class ParallelBZip2OutputStream : Stream
	{
		[Flags]
		private enum TraceBits : uint
		{
			None = 0u,
			Crc = 1u,
			Write = 2u,
			All = uint.MaxValue
		}

		private static readonly int BufferPairsPerCore = 4;

		private int _maxWorkers;

		private bool firstWriteDone;

		private int lastFilled;

		private int lastWritten;

		private int latestCompressed;

		private int currentlyFilling;

		private volatile Exception pendingException;

		private bool handlingException;

		private bool emitting;

		private Queue<int> toWrite;

		private Queue<int> toFill;

		private List<WorkItem> pool;

		private object latestLock = new object();

		private object eLock = new object();

		private object outputLock = new object();

		private AutoResetEvent newlyCompressedBlob;

		private long totalBytesWrittenIn;

		private long totalBytesWrittenOut;

		private bool leaveOpen;

		private uint combinedCRC;

		private Stream output;

		private BitWriter bw;

		private int blockSize100k;

		private TraceBits desiredTrace = TraceBits.Crc | TraceBits.Write;

		/// <summary>
		///   The maximum number of concurrent compression worker threads to use.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This property sets an upper limit on the number of concurrent worker
		///   threads to employ for compression. The implementation of this stream
		///   employs multiple threads from the .NET thread pool, via <see cref="M:System.Threading.ThreadPool.QueueUserWorkItem(System.Threading.WaitCallback)">
		///   ThreadPool.QueueUserWorkItem()</see>, to compress the incoming data by
		///   block.  As each block of data is compressed, this stream re-orders the
		///   compressed blocks and writes them to the output stream.
		/// </para>
		///
		/// <para>
		///   A higher number of workers enables a higher degree of
		///   parallelism, which tends to increase the speed of compression on
		///   multi-cpu computers.  On the other hand, a higher number of buffer
		///   pairs also implies a larger memory consumption, more active worker
		///   threads, and a higher cpu utilization for any compression. This
		///   property enables the application to limit its memory consumption and
		///   CPU utilization behavior depending on requirements.
		/// </para>
		///
		/// <para>
		///   By default, DotNetZip allocates 4 workers per CPU core, subject to the
		///   upper limit specified in this property. For example, suppose the
		///   application sets this property to 16.  Then, on a machine with 2
		///   cores, DotNetZip will use 8 workers; that number does not exceed the
		///   upper limit specified by this property, so the actual number of
		///   workers used will be 4 * 2 = 8.  On a machine with 4 cores, DotNetZip
		///   will use 16 workers; again, the limit does not apply. On a machine
		///   with 8 cores, DotNetZip will use 16 workers, because of the limit.
		/// </para>
		///
		/// <para>
		///   For each compression "worker thread" that occurs in parallel, there is
		///   up to 2mb of memory allocated, for buffering and processing. The
		///   actual number depends on the <see cref="P:Ionic.BZip2.ParallelBZip2OutputStream.BlockSize" /> property.
		/// </para>
		///
		/// <para>
		///   CPU utilization will also go up with additional workers, because a
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
		///   The application can set this value at any time, but it is effective
		///   only before the first call to Write(), which is when the buffers are
		///   allocated.
		/// </para>
		/// </remarks>
		public int MaxWorkers
		{
			get
			{
				return _maxWorkers;
			}
			set
			{
				if (value < 4)
				{
					throw new ArgumentException("MaxWorkers", "Value must be 4 or greater.");
				}
				_maxWorkers = value;
			}
		}

		/// <summary>
		///   The blocksize parameter specified at construction time.
		/// </summary>
		public int BlockSize => blockSize100k;

		/// <summary>
		/// Indicates whether the stream can be read.
		/// </summary>
		/// <remarks>
		/// The return value is always false.
		/// </remarks>
		public override bool CanRead => false;

		/// <summary>
		/// Indicates whether the stream supports Seek operations.
		/// </summary>
		/// <remarks>
		/// Always returns false.
		/// </remarks>
		public override bool CanSeek => false;

		/// <summary>
		/// Indicates whether the stream can be written.
		/// </summary>
		/// <remarks>
		/// The return value depends on whether the captive stream supports writing.
		/// </remarks>
		public override bool CanWrite
		{
			get
			{
				if (output == null)
				{
					throw new ObjectDisposedException("BZip2Stream");
				}
				return output.CanWrite;
			}
		}

		/// <summary>
		/// Reading this property always throws a <see cref="T:System.NotImplementedException" />.
		/// </summary>
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>
		/// The position of the stream pointer.
		/// </summary>
		///
		/// <remarks>
		///   Setting this property always throws a <see cref="T:System.NotImplementedException" />. Reading will return the
		///   total number of uncompressed bytes written through.
		/// </remarks>
		public override long Position
		{
			get
			{
				return totalBytesWrittenIn;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>
		/// The total number of bytes written out by the stream.
		/// </summary>
		/// <remarks>
		/// This value is meaningful only after a call to Close().
		/// </remarks>
		public long BytesWrittenOut => totalBytesWrittenOut;

		/// <summary>
		///   Constructs a new <c>ParallelBZip2OutputStream</c>, that sends its
		///   compressed output to the given output stream.
		/// </summary>
		///
		/// <param name="output">
		///   The destination stream, to which compressed output will be sent.
		/// </param>
		///
		/// <example>
		///
		///   This example reads a file, then compresses it with bzip2 file,
		///   and writes the compressed data into a newly created file.
		///
		///   <code>
		///   var fname = "logfile.log";
		///   using (var fs = File.OpenRead(fname))
		///   {
		///       var outFname = fname + ".bz2";
		///       using (var output = File.Create(outFname))
		///       {
		///           using (var compressor = new Ionic.BZip2.ParallelBZip2OutputStream(output))
		///           {
		///               byte[] buffer = new byte[2048];
		///               int n;
		///               while ((n = fs.Read(buffer, 0, buffer.Length)) &gt; 0)
		///               {
		///                   compressor.Write(buffer, 0, n);
		///               }
		///           }
		///       }
		///   }
		///   </code>
		/// </example>
		public ParallelBZip2OutputStream(Stream output)
			: this(output, BZip2.MaxBlockSize, false)
		{
		}

		/// <summary>
		///   Constructs a new <c>ParallelBZip2OutputStream</c> with specified blocksize.
		/// </summary>
		/// <param name="output">the destination stream.</param>
		/// <param name="blockSize">
		///   The blockSize in units of 100000 bytes.
		///   The valid range is 1..9.
		/// </param>
		public ParallelBZip2OutputStream(Stream output, int blockSize)
			: this(output, blockSize, false)
		{
		}

		/// <summary>
		///   Constructs a new <c>ParallelBZip2OutputStream</c>.
		/// </summary>
		///   <param name="output">the destination stream.</param>
		/// <param name="leaveOpen">
		///   whether to leave the captive stream open upon closing this stream.
		/// </param>
		public ParallelBZip2OutputStream(Stream output, bool leaveOpen)
			: this(output, BZip2.MaxBlockSize, leaveOpen)
		{
		}

		/// <summary>
		///   Constructs a new <c>ParallelBZip2OutputStream</c> with specified blocksize,
		///   and explicitly specifies whether to leave the wrapped stream open.
		/// </summary>
		///
		/// <param name="output">the destination stream.</param>
		/// <param name="blockSize">
		///   The blockSize in units of 100000 bytes.
		///   The valid range is 1..9.
		/// </param>
		/// <param name="leaveOpen">
		///   whether to leave the captive stream open upon closing this stream.
		/// </param>
		public ParallelBZip2OutputStream(Stream output, int blockSize, bool leaveOpen)
		{
			if (blockSize < BZip2.MinBlockSize || blockSize > BZip2.MaxBlockSize)
			{
				throw new ArgumentException($"blockSize={blockSize} is out of range; must be between {BZip2.MinBlockSize} and {BZip2.MaxBlockSize}", "blockSize");
			}
			this.output = output;
			if (!this.output.CanWrite)
			{
				throw new ArgumentException("The stream is not writable.", "output");
			}
			bw = new BitWriter(this.output);
			blockSize100k = blockSize;
			this.leaveOpen = leaveOpen;
			combinedCRC = 0u;
			MaxWorkers = 16;
			EmitHeader();
		}

		private void InitializePoolOfWorkItems()
		{
			toWrite = new Queue<int>();
			toFill = new Queue<int>();
			pool = new List<WorkItem>();
			int val = BufferPairsPerCore * Environment.ProcessorCount;
			val = Math.Min(val, MaxWorkers);
			for (int i = 0; i < val; i++)
			{
				pool.Add(new WorkItem(i, blockSize100k));
				toFill.Enqueue(i);
			}
			newlyCompressedBlob = new AutoResetEvent(false);
			currentlyFilling = -1;
			lastFilled = -1;
			lastWritten = -1;
			latestCompressed = -1;
		}

		/// <summary>
		///   Close the stream.
		/// </summary>
		/// <remarks>
		///   <para>
		///     This may or may not close the underlying stream.  Check the
		///     constructors that accept a bool value.
		///   </para>
		/// </remarks>
		public override void Close()
		{
			if (pendingException != null)
			{
				handlingException = true;
				Exception ex = pendingException;
				pendingException = null;
				throw ex;
			}
			if (!handlingException && output != null)
			{
				Stream stream = output;
				try
				{
					FlushOutput(true);
				}
				finally
				{
					output = null;
					bw = null;
				}
				if (!leaveOpen)
				{
					stream.Close();
				}
			}
		}

		private void FlushOutput(bool lastInput)
		{
			if (!emitting)
			{
				if (currentlyFilling >= 0)
				{
					WorkItem wi = pool[currentlyFilling];
					CompressOne(wi);
					currentlyFilling = -1;
				}
				if (lastInput)
				{
					EmitPendingBuffers(true, false);
					EmitTrailer();
				}
				else
				{
					EmitPendingBuffers(false, false);
				}
			}
		}

		/// <summary>
		///   Flush the stream.
		/// </summary>
		public override void Flush()
		{
			if (output != null)
			{
				FlushOutput(false);
				bw.Flush();
				output.Flush();
			}
		}

		private void EmitHeader()
		{
			byte[] obj = new byte[4] { 66, 90, 104, 0 };
			obj[3] = (byte)(48 + blockSize100k);
			byte[] array = obj;
			output.Write(array, 0, array.Length);
		}

		private void EmitTrailer()
		{
			bw.WriteByte(23);
			bw.WriteByte(114);
			bw.WriteByte(69);
			bw.WriteByte(56);
			bw.WriteByte(80);
			bw.WriteByte(144);
			bw.WriteInt(combinedCRC);
			bw.FinishAndPad();
		}

		/// <summary>
		///   Write data to the stream.
		/// </summary>
		/// <remarks>
		///
		/// <para>
		///   Use the <c>ParallelBZip2OutputStream</c> to compress data while
		///   writing: create a <c>ParallelBZip2OutputStream</c> with a writable
		///   output stream.  Then call <c>Write()</c> on that
		///   <c>ParallelBZip2OutputStream</c>, providing uncompressed data as
		///   input.  The data sent to the output stream will be the compressed
		///   form of the input data.
		/// </para>
		///
		/// <para>
		///   A <c>ParallelBZip2OutputStream</c> can be used only for
		///   <c>Write()</c> not for <c>Read()</c>.
		/// </para>
		///
		/// </remarks>
		///
		/// <param name="buffer">The buffer holding data to write to the stream.</param>
		/// <param name="offset">the offset within that data array to find the first byte to write.</param>
		/// <param name="count">the number of bytes to write.</param>
		public override void Write(byte[] buffer, int offset, int count)
		{
			bool mustWait = false;
			if (output == null)
			{
				throw new IOException("the stream is not open");
			}
			if (pendingException != null)
			{
				handlingException = true;
				Exception ex = pendingException;
				pendingException = null;
				throw ex;
			}
			if (offset < 0)
			{
				throw new IndexOutOfRangeException($"offset ({offset}) must be > 0");
			}
			if (count < 0)
			{
				throw new IndexOutOfRangeException($"count ({count}) must be > 0");
			}
			if (offset + count > buffer.Length)
			{
				throw new IndexOutOfRangeException($"offset({offset}) count({count}) bLength({buffer.Length})");
			}
			if (count == 0)
			{
				return;
			}
			if (!firstWriteDone)
			{
				InitializePoolOfWorkItems();
				firstWriteDone = true;
			}
			int num = 0;
			int num2 = count;
			do
			{
				EmitPendingBuffers(false, mustWait);
				mustWait = false;
				int num3 = -1;
				if (currentlyFilling >= 0)
				{
					num3 = currentlyFilling;
				}
				else
				{
					if (toFill.Count == 0)
					{
						mustWait = true;
						continue;
					}
					num3 = toFill.Dequeue();
					lastFilled++;
				}
				WorkItem workItem = pool[num3];
				workItem.ordinal = lastFilled;
				int num4 = workItem.Compressor.Fill(buffer, offset, num2);
				if (num4 != num2)
				{
					if (!ThreadPool.QueueUserWorkItem(CompressOne, workItem))
					{
						throw new Exception("Cannot enqueue workitem");
					}
					currentlyFilling = -1;
					offset += num4;
				}
				else
				{
					currentlyFilling = num3;
				}
				num2 -= num4;
				num += num4;
			}
			while (num2 > 0);
			totalBytesWrittenIn += num;
		}

		private void EmitPendingBuffers(bool doAll, bool mustWait)
		{
			if (emitting)
			{
				return;
			}
			emitting = true;
			if (doAll || mustWait)
			{
				newlyCompressedBlob.WaitOne();
			}
			do
			{
				int num = -1;
				int num2 = (doAll ? 200 : (mustWait ? (-1) : 0));
				int num3 = -1;
				do
				{
					if (Monitor.TryEnter(toWrite, num2))
					{
						num3 = -1;
						try
						{
							if (toWrite.Count > 0)
							{
								num3 = toWrite.Dequeue();
							}
						}
						finally
						{
							Monitor.Exit(toWrite);
						}
						if (num3 < 0)
						{
							continue;
						}
						WorkItem workItem = pool[num3];
						if (workItem.ordinal != lastWritten + 1)
						{
							lock (toWrite)
							{
								toWrite.Enqueue(num3);
							}
							if (num == num3)
							{
								newlyCompressedBlob.WaitOne();
								num = -1;
							}
							else if (num == -1)
							{
								num = num3;
							}
							continue;
						}
						num = -1;
						BitWriter bitWriter = workItem.bw;
						bitWriter.Flush();
						MemoryStream ms = workItem.ms;
						ms.Seek(0L, SeekOrigin.Begin);
						long num4 = 0L;
						byte[] array = new byte[1024];
						int num5;
						while ((num5 = ms.Read(array, 0, array.Length)) > 0)
						{
							for (int i = 0; i < num5; i++)
							{
								bw.WriteByte(array[i]);
							}
							num4 += num5;
						}
						if (bitWriter.NumRemainingBits > 0)
						{
							bw.WriteBits(bitWriter.NumRemainingBits, bitWriter.RemainingBits);
						}
						combinedCRC = (combinedCRC << 1) | (combinedCRC >> 31);
						combinedCRC ^= workItem.Compressor.Crc32;
						totalBytesWrittenOut += num4;
						bitWriter.Reset();
						lastWritten = workItem.ordinal;
						workItem.ordinal = -1;
						toFill.Enqueue(workItem.index);
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
			while (doAll && lastWritten != latestCompressed);
			emitting = false;
		}

		private void CompressOne(object wi)
		{
			WorkItem workItem = (WorkItem)wi;
			try
			{
				workItem.Compressor.CompressAndWrite();
				lock (latestLock)
				{
					if (workItem.ordinal > latestCompressed)
					{
						latestCompressed = workItem.ordinal;
					}
				}
				lock (toWrite)
				{
					toWrite.Enqueue(workItem.index);
				}
				newlyCompressedBlob.Set();
			}
			catch (Exception ex)
			{
				lock (eLock)
				{
					if (pendingException != null)
					{
						pendingException = ex;
					}
				}
			}
		}

		/// <summary>
		/// Calling this method always throws a <see cref="T:System.NotImplementedException" />.
		/// </summary>
		/// <param name="offset">this is irrelevant, since it will always throw!</param>
		/// <param name="origin">this is irrelevant, since it will always throw!</param>
		/// <returns>irrelevant!</returns>
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Calling this method always throws a <see cref="T:System.NotImplementedException" />.
		/// </summary>
		/// <param name="value">this is irrelevant, since it will always throw!</param>
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Calling this method always throws a <see cref="T:System.NotImplementedException" />.
		/// </summary>
		/// <param name="buffer">this parameter is never used</param>
		/// <param name="offset">this parameter is never used</param>
		/// <param name="count">this parameter is never used</param>
		/// <returns>never returns anything; always throws</returns>
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		[Conditional("Trace")]
		private void TraceOutput(TraceBits bits, string format, params object[] varParams)
		{
			if ((bits & desiredTrace) != 0)
			{
				lock (outputLock)
				{
					int hashCode = Thread.CurrentThread.GetHashCode();
					Console.ForegroundColor = (ConsoleColor)(hashCode % 8 + 10);
					Console.Write("{0:000} PBOS ", hashCode);
					Console.WriteLine(format, varParams);
					Console.ResetColor();
				}
			}
		}
	}
}
