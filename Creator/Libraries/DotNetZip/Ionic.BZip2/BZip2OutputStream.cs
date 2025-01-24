using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Ionic.BZip2
{
	/// <summary>
	///   A write-only decorator stream that compresses data as it is
	///   written using the BZip2 algorithm.
	/// </summary>
	public class BZip2OutputStream : Stream
	{
		[Flags]
		private enum TraceBits : uint
		{
			None = 0u,
			Crc = 1u,
			Write = 2u,
			All = uint.MaxValue
		}

		private int totalBytesWrittenIn;

		private bool leaveOpen;

		private BZip2Compressor compressor;

		private uint combinedCRC;

		private Stream output;

		private BitWriter bw;

		private int blockSize100k;

		private TraceBits desiredTrace = TraceBits.Crc | TraceBits.Write;

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
		/// The return value should always be true, unless and until the
		/// object is disposed and closed.
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
		///   Constructs a new <c>BZip2OutputStream</c>, that sends its
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
		///           using (var compressor = new Ionic.BZip2.BZip2OutputStream(output))
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
		public BZip2OutputStream(Stream output)
			: this(output, BZip2.MaxBlockSize, false)
		{
		}

		/// <summary>
		///   Constructs a new <c>BZip2OutputStream</c> with specified blocksize.
		/// </summary>
		/// <param name="output">the destination stream.</param>
		/// <param name="blockSize">
		///   The blockSize in units of 100000 bytes.
		///   The valid range is 1..9.
		/// </param>
		public BZip2OutputStream(Stream output, int blockSize)
			: this(output, blockSize, false)
		{
		}

		/// <summary>
		///   Constructs a new <c>BZip2OutputStream</c>.
		/// </summary>
		///   <param name="output">the destination stream.</param>
		/// <param name="leaveOpen">
		///   whether to leave the captive stream open upon closing this stream.
		/// </param>
		public BZip2OutputStream(Stream output, bool leaveOpen)
			: this(output, BZip2.MaxBlockSize, leaveOpen)
		{
		}

		/// <summary>
		///   Constructs a new <c>BZip2OutputStream</c> with specified blocksize,
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
		public BZip2OutputStream(Stream output, int blockSize, bool leaveOpen)
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
			compressor = new BZip2Compressor(bw, blockSize);
			this.leaveOpen = leaveOpen;
			combinedCRC = 0u;
			EmitHeader();
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
			if (output != null)
			{
				Stream stream = output;
				Finish();
				if (!leaveOpen)
				{
					stream.Close();
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

		private void Finish()
		{
			try
			{
				_ = bw.TotalBytesWrittenOut;
				compressor.CompressAndWrite();
				combinedCRC = (combinedCRC << 1) | (combinedCRC >> 31);
				combinedCRC ^= compressor.Crc32;
				EmitTrailer();
			}
			finally
			{
				output = null;
				compressor = null;
				bw = null;
			}
		}

		/// <summary>
		///   Write data to the stream.
		/// </summary>
		/// <remarks>
		///
		/// <para>
		///   Use the <c>BZip2OutputStream</c> to compress data while writing:
		///   create a <c>BZip2OutputStream</c> with a writable output stream.
		///   Then call <c>Write()</c> on that <c>BZip2OutputStream</c>, providing
		///   uncompressed data as input.  The data sent to the output stream will
		///   be the compressed form of the input data.
		/// </para>
		///
		/// <para>
		///   A <c>BZip2OutputStream</c> can be used only for <c>Write()</c> not for <c>Read()</c>.
		/// </para>
		///
		/// </remarks>
		///
		/// <param name="buffer">The buffer holding data to write to the stream.</param>
		/// <param name="offset">the offset within that data array to find the first byte to write.</param>
		/// <param name="count">the number of bytes to write.</param>
		public override void Write(byte[] buffer, int offset, int count)
		{
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
			if (output == null)
			{
				throw new IOException("the stream is not open");
			}
			if (count == 0)
			{
				return;
			}
			int num = 0;
			int num2 = count;
			do
			{
				int num3 = compressor.Fill(buffer, offset, num2);
				if (num3 != num2)
				{
					_ = bw.TotalBytesWrittenOut;
					compressor.CompressAndWrite();
					combinedCRC = (combinedCRC << 1) | (combinedCRC >> 31);
					combinedCRC ^= compressor.Crc32;
					offset += num3;
				}
				num2 -= num3;
				num += num3;
			}
			while (num2 > 0);
			totalBytesWrittenIn += num;
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
		///   Calling this method always throws a <see cref="T:System.NotImplementedException" />.
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
				int hashCode = Thread.CurrentThread.GetHashCode();
				Console.ForegroundColor = (ConsoleColor)(hashCode % 8 + 10);
				Console.Write("{0:000} PBOS ", hashCode);
				Console.WriteLine(format, varParams);
				Console.ResetColor();
			}
		}
	}
}
