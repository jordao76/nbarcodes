using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace NBarCodes {

	/// <summary>
	/// A <see cref="Stream"/> decorator used to trace stream reads and writes.
	/// </summary>
	sealed class DebugStream : Stream {

		/// <summary>
		/// Creates a new instance of the <see cref="DebugStream"/> class.
		/// </summary>
		/// <param name="title">A name to distinguish this stream.</param>
		/// <param name="baseStream">The stream to be decorated by this stream.</param>
		/// <exception cref="ArgumentNullException">
		/// If the title or the base stream are null or the title is empty.
		/// </exception>
		public DebugStream(string title, Stream baseStream) {
			if (StringHelper.IsNullOrEmpty(title)) {
				throw new ArgumentNullException("title");
			}
			if (baseStream == null) {
				throw new ArgumentNullException("baseStream");
			}

			_baseStream = baseStream;
			_title = title;
		}

		/// <summary>
		/// Flushes the base stream's contents.
		/// </summary>
 		public override void Flush() {
			_baseStream.Flush();
		}

		/// <summary>
		/// Reads a chunk from the base stream.
		/// </summary>
		/// <param name="buffer">Buffer to store bytes read.</param>
		/// <param name="offset">Offset into buffer to begin storing bytes read.</param>
		/// <param name="count">Number of bytes to read.</param>
		/// <returns>Number of bytes actually read.</returns>
		public override int Read(byte[] buffer, int offset, int count) {
			int bytesRead = _baseStream.Read(buffer, offset, count);
			LogMessage("{0} bytes read from offset {1}.", bytesRead, offset);
			return bytesRead;
		}

		/// <summary>
		/// Sets the position within the base stream.
		/// </summary>
		/// <param name="offset">Size of the position seek operation.</param>
		/// <param name="origin">Where to seek from.</param>
		/// <returns>The position set on the base stream.</returns>
		public override long Seek(long offset, SeekOrigin origin) {
			return _baseStream.Seek(offset, origin);
		}

		/// <summary>
		/// Sets the length of the base stream.
		/// </summary>
		/// <param name="value">New length to set.</param>
		public override void SetLength(long value) {
			_baseStream.SetLength(value);
		}

		/// <summary>
		/// Writes a number of bytes to the base stream.
		/// </summary>
		/// <param name="buffer">Buffer with bytes for writing.</param>
		/// <param name="offset">Offset into the buffer.</param>
		/// <param name="count">Number of bytes to read from the buffer and write to the stream.</param>
		public override void Write(byte[] buffer, int offset, int count) {
			_baseStream.Write(buffer, offset, count);
			LogMessage("Written {0} bytes from offset {1} into buffer of length {2}.", count, offset, buffer.Length);
		}

		/// <summary>Determines whether the stream supports reading.</summary>
		public override bool CanRead { 
			get { return _baseStream.CanRead; } 
		}

		/// <summary>Determines whether the stream supports seeking.</summary>
		public override bool CanSeek {
			get { return _baseStream.CanSeek; } 
		}

		/// <summary>Determines whether the stream supports writing.</summary>
		public override bool CanWrite { 
			get { return _baseStream.CanWrite; } 
		}

		/// <summary>The length of the stream.</summary>
		public override long Length { 
			get { return _baseStream.Length; } 
		}

		/// <summary>The current position of the stream.</summary>
		public override long Position { 
			get { return _baseStream.Position; } 
			set { _baseStream.Position = value; }
		}

		// TODO: create helper classes for tracing.
		/// <summary>
		/// Logs a message to the <see cref="Debug"/> class.
		/// </summary>
		/// <param name="message">Message to log, in the format of <see cref="String.Format(string,object)"/>.</param>
		/// <param name="parameters">Parameters to the positional elements of the message.</param>
		[Conditional("DEBUG")]
		private void LogMessage(string message, params object[] parameters) {
			Debug.WriteLine(
				string.Format("[{0}][T#{1}][DebugStream][{2}] -- {3}",
					DateTime.Now.ToString("u"),
					Thread.CurrentThread.GetHashCode(),
					_title,
					string.Format(message, parameters))
			);
		}

		/// <summary>The decorated base stream.</summary>
		private Stream _baseStream;

		/// <summary>The title of this stream.</summary>
		private string _title;
	}
}
