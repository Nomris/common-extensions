using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public sealed class StreamPackage : Stream, IStreamable
    {
        private Stream buffer = new MemoryStream();

        public Stream BaseStream { get; }

        public override bool CanRead => buffer.CanRead;

        public override bool CanSeek => buffer.CanSeek;

        public override bool CanWrite => buffer.CanWrite;

        public override long Length => buffer.Length;

        public override long Position { get => buffer.Position; set => buffer.Position = value; }


        public StreamPackage(Stream stream = null, Stream buffer = null)
        {
            BaseStream = stream;
#if NET5_0_OR_GREATER
            buffer ??= new MemoryStream();
#else
            if (buffer == null) buffer = new MemoryStream();
#endif
            if (!buffer.CanSeek) throw new ArgumentException("The buffer stream object must be able to seek", nameof(buffer));
            if (!buffer.CanWrite) throw new ArgumentException("The buffer stream object must be able to be written to", nameof(buffer));

            this.buffer = buffer;
        }

        public override void Flush()
        {
            buffer.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this.buffer.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return buffer.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            buffer.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.buffer.Write(buffer, offset, count);
        }

        public void Submit(Stream target = null)
        {
#if NET5_0_OR_GREATER
            target ??= BaseStream;
#else
            if (target == null) target = BaseStream;
#endif

            lock (target)
            {
                long bufferPos = buffer.Position;
                int lastRead;
                buffer.Position = 0;
                byte[] transferBuffer = new byte[4096];
                while (buffer.Position < bufferPos)
                {
                    lastRead = buffer.Read(transferBuffer, 0, (int)Math.Min(bufferPos - buffer.Position, 4096L));
                    target.Write(transferBuffer, 0, lastRead);
                }
                buffer.Position = bufferPos;
            }
        }

        public void Reset()
        {
            long bufferPos = buffer.Position;
            buffer.Position = 0;
            while (--bufferPos > 0) buffer.WriteByte(0);
            buffer.Position = 0;
        }

        public new void Dispose()
        {
            buffer.Dispose();
        }

        public void ToStream(Stream stream)
        {
            Submit(stream);
        }

        public byte[] ToByteArray()
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                Submit(memStream);
                memStream.Position = 0;
                return memStream.ToArray();
            }
        }

        ~StreamPackage()
        {
            Dispose();
        }
    }
}
