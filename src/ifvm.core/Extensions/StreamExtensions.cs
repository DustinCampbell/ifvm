using System;
using System.IO;
using System.Threading.Tasks;

namespace IFVM.Extensions
{
    public static class StreamExtensions
    {
        public static async Task<byte[]> ReadAllBytesAsync(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var buffer = new byte[1024];
            var read = 0;

            int chunk;
            while ((chunk = await stream.ReadAsync(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    Array.Resize(ref buffer, buffer.Length * 2);
                    buffer[read] = (byte)nextByte;
                    read++;
                }
            }

            Array.Resize(ref buffer, read);

            return buffer;
        }
    }
}
