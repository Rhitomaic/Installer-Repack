using System;

namespace Ionic.Zip.Deflate64
{
	internal sealed class InputBuffer
	{
		private byte[] _buffer;

		private int _start;

		private int _end;

		private uint _bitBuffer;

		private int _bitsInBuffer;

		/// <summary>Total bits available in the input buffer.</summary>
		public int AvailableBits => _bitsInBuffer;

		/// <summary>Total bytes available in the input buffer.</summary>
		public int AvailableBytes => _end - _start + _bitsInBuffer / 8;

		/// <summary>Ensure that count bits are in the bit buffer.</summary>
		/// <param name="count">Can be up to 16.</param>
		/// <returns>Returns false if input is not sufficient to make this true.</returns>
		public bool EnsureBitsAvailable(int count)
		{
			if (_bitsInBuffer < count)
			{
				if (NeedsInput())
				{
					return false;
				}
				_bitBuffer |= (uint)(_buffer[_start++] << _bitsInBuffer);
				_bitsInBuffer += 8;
				if (_bitsInBuffer < count)
				{
					if (NeedsInput())
					{
						return false;
					}
					_bitBuffer |= (uint)(_buffer[_start++] << _bitsInBuffer);
					_bitsInBuffer += 8;
				}
			}
			return true;
		}

		/// <summary>
		/// This function will try to load 16 or more bits into bitBuffer.
		/// It returns whatever is contained in bitBuffer after loading.
		/// The main difference between this and GetBits is that this will
		/// never return -1. So the caller needs to check AvailableBits to
		/// see how many bits are available.
		/// </summary>
		public uint TryLoad16Bits()
		{
			if (_bitsInBuffer < 8)
			{
				if (_start < _end)
				{
					_bitBuffer |= (uint)(_buffer[_start++] << _bitsInBuffer);
					_bitsInBuffer += 8;
				}
				if (_start < _end)
				{
					_bitBuffer |= (uint)(_buffer[_start++] << _bitsInBuffer);
					_bitsInBuffer += 8;
				}
			}
			else if (_bitsInBuffer < 16 && _start < _end)
			{
				_bitBuffer |= (uint)(_buffer[_start++] << _bitsInBuffer);
				_bitsInBuffer += 8;
			}
			return _bitBuffer;
		}

		private uint GetBitMask(int count)
		{
			return (uint)((1 << count) - 1);
		}

		/// <summary>Gets count bits from the input buffer. Returns -1 if not enough bits available.</summary>
		public int GetBits(int count)
		{
			if (!EnsureBitsAvailable(count))
			{
				return -1;
			}
			uint result = _bitBuffer & GetBitMask(count);
			_bitBuffer >>= count;
			_bitsInBuffer -= count;
			return (int)result;
		}

		/// <summary>
		/// Copies length bytes from input buffer to output buffer starting at output[offset].
		/// You have to make sure, that the buffer is byte aligned. If not enough bytes are
		/// available, copies fewer bytes.
		/// </summary>
		/// <returns>Returns the number of bytes copied, 0 if no byte is available.</returns>
		public int CopyTo(byte[] output, int offset, int length)
		{
			int num = 0;
			while (_bitsInBuffer > 0 && length > 0)
			{
				output[offset++] = (byte)_bitBuffer;
				_bitBuffer >>= 8;
				_bitsInBuffer -= 8;
				length--;
				num++;
			}
			if (length == 0)
			{
				return num;
			}
			int num2 = _end - _start;
			if (length > num2)
			{
				length = num2;
			}
			Array.Copy(_buffer, _start, output, offset, length);
			_start += length;
			return num + length;
		}

		/// <summary>
		/// Return true is all input bytes are used.
		/// This means the caller can call SetInput to add more input.
		/// </summary>
		public bool NeedsInput()
		{
			return _start == _end;
		}

		/// <summary>
		/// Set the byte array to be processed.
		/// All the bits remained in bitBuffer will be processed before the new bytes.
		/// We don't clone the byte array here since it is expensive.
		/// The caller should make sure after a buffer is passed in.
		/// It will not be changed before calling this function again.
		/// </summary>
		public void SetInput(byte[] buffer, int offset, int length)
		{
			if (_start == _end)
			{
				_buffer = buffer;
				_start = offset;
				_end = offset + length;
			}
		}

		/// <summary>Skip n bits in the buffer.</summary>
		public void SkipBits(int n)
		{
			_bitBuffer >>= n;
			_bitsInBuffer -= n;
		}

		/// <summary>Skips to the next byte boundary.</summary>
		public void SkipToByteBoundary()
		{
			_bitBuffer >>= _bitsInBuffer % 8;
			_bitsInBuffer -= _bitsInBuffer % 8;
		}
	}
}
