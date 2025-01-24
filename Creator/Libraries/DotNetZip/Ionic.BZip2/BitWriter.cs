using System.IO;

namespace Ionic.BZip2
{
	internal class BitWriter
	{
		private uint accumulator;

		private int nAccumulatedBits;

		private Stream output;

		private int totalBytesWrittenOut;

		/// <summary>
		///   Delivers the remaining bits, left-aligned, in a byte.
		/// </summary>
		/// <remarks>
		///   <para>
		///     This is valid only if NumRemainingBits is less than 8;
		///     in other words it is valid only after a call to Flush().
		///   </para>
		/// </remarks>
		public byte RemainingBits => (byte)((accumulator >> 32 - nAccumulatedBits) & 0xFFu);

		public int NumRemainingBits => nAccumulatedBits;

		public int TotalBytesWrittenOut => totalBytesWrittenOut;

		public BitWriter(Stream s)
		{
			output = s;
		}

		/// <summary>
		///   Reset the BitWriter.
		/// </summary>
		/// <remarks>
		///   <para>
		///     This is useful when the BitWriter writes into a MemoryStream, and
		///     is used by a BZip2Compressor, which itself is re-used for multiple
		///     distinct data blocks.
		///   </para>
		/// </remarks>
		public void Reset()
		{
			accumulator = 0u;
			nAccumulatedBits = 0;
			totalBytesWrittenOut = 0;
			output.Seek(0L, SeekOrigin.Begin);
			output.SetLength(0L);
		}

		/// <summary>
		///   Write some number of bits from the given value, into the output.
		/// </summary>
		/// <remarks>
		///   <para>
		///     The nbits value should be a max of 25, for safety. For performance
		///     reasons, this method does not check!
		///   </para>
		/// </remarks>
		public void WriteBits(int nbits, uint value)
		{
			int num = nAccumulatedBits;
			uint num2 = accumulator;
			while (num >= 8)
			{
				output.WriteByte((byte)((num2 >> 24) & 0xFFu));
				totalBytesWrittenOut++;
				num2 <<= 8;
				num -= 8;
			}
			accumulator = num2 | (value << 32 - num - nbits);
			nAccumulatedBits = num + nbits;
		}

		/// <summary>
		///   Write a full 8-bit byte into the output.
		/// </summary>
		public void WriteByte(byte b)
		{
			WriteBits(8, b);
		}

		/// <summary>
		///   Write four 8-bit bytes into the output.
		/// </summary>
		public void WriteInt(uint u)
		{
			WriteBits(8, (u >> 24) & 0xFFu);
			WriteBits(8, (u >> 16) & 0xFFu);
			WriteBits(8, (u >> 8) & 0xFFu);
			WriteBits(8, u & 0xFFu);
		}

		/// <summary>
		///   Write all available byte-aligned bytes.
		/// </summary>
		/// <remarks>
		///   <para>
		///     This method writes no new output, but flushes any accumulated
		///     bits. At completion, the accumulator may contain up to 7
		///     bits.
		///   </para>
		///   <para>
		///     This is necessary when re-assembling output from N independent
		///     compressors, one for each of N blocks. The output of any
		///     particular compressor will in general have some fragment of a byte
		///     remaining. This fragment needs to be accumulated into the
		///     parent BZip2OutputStream.
		///   </para>
		/// </remarks>
		public void Flush()
		{
			WriteBits(0, 0u);
		}

		/// <summary>
		///   Writes all available bytes, and emits padding for the final byte as
		///   necessary. This must be the last method invoked on an instance of
		///   BitWriter.
		/// </summary>
		public void FinishAndPad()
		{
			Flush();
			if (NumRemainingBits > 0)
			{
				byte value = (byte)((accumulator >> 24) & 0xFFu);
				output.WriteByte(value);
				totalBytesWrittenOut++;
			}
		}
	}
}
