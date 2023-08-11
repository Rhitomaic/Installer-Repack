using System;
using System.IO;

namespace Ionic.Zip.Deflate64
{
	internal sealed class Deflate64Stream : Stream
	{
		internal const int DefaultBufferSize = 8192;

		private Stream _stream;

		private InflaterManaged _inflater;

		private readonly byte[] _buffer;

		public override bool CanRead
		{
			get
			{
				if (_stream == null)
				{
					return false;
				}
				return _stream.CanRead;
			}
		}

		public override bool CanWrite => false;

		public override bool CanSeek => false;

		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		internal Deflate64Stream(Stream stream, long uncompressedSize = -1L)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (!stream.CanRead)
			{
				throw new ArgumentException("NotSupported_UnreadableStream", "stream");
			}
			_inflater = new InflaterManaged(null, true, uncompressedSize);
			_stream = stream;
			_buffer = new byte[8192];
		}

		public override void Flush()
		{
			EnsureNotDisposed();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override int Read(byte[] array, int offset, int count)
		{
			ValidateParameters(array, offset, count);
			EnsureNotDisposed();
			int num = offset;
			int num2 = count;
			while (true)
			{
				int num3 = _inflater.Inflate(array, num, num2);
				num += num3;
				num2 -= num3;
				if (num2 == 0 || _inflater.Finished())
				{
					break;
				}
				int num4 = _stream.Read(_buffer, 0, _buffer.Length);
				if (num4 <= 0)
				{
					break;
				}
				if (num4 > _buffer.Length)
				{
					throw new InvalidDataException();
				}
				_inflater.SetInput(_buffer, 0, num4);
			}
			return count - num2;
		}

		private void ValidateParameters(byte[] array, int offset, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (array.Length - offset < count)
			{
				throw new ArgumentException("InvalidArgumentOffsetCount");
			}
		}

		private void EnsureNotDisposed()
		{
			if (_stream == null)
			{
				ThrowStreamClosedException();
			}
		}

		private static void ThrowStreamClosedException()
		{
			throw new ObjectDisposedException(null, "ObjectDisposed_StreamClosed");
		}

		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			throw new NotImplementedException();
		}

		public override int EndRead(IAsyncResult asyncResult)
		{
			throw new NotImplementedException();
		}

		public override void Write(byte[] array, int offset, int count)
		{
			throw new InvalidOperationException("CannotWriteToDeflateStream");
		}

		private void PurgeBuffers(bool disposing)
		{
			if (disposing && _stream != null)
			{
				Flush();
			}
		}

		protected override void Dispose(bool disposing)
		{
			try
			{
				PurgeBuffers(disposing);
			}
			finally
			{
				try
				{
					if (disposing && _stream != null)
					{
						_stream.Dispose();
					}
				}
				finally
				{
					_stream = null;
					try
					{
						if (_inflater != null)
						{
							_inflater.Dispose();
						}
					}
					finally
					{
						_inflater = null;
						base.Dispose(disposing);
					}
				}
			}
		}
	}
}
