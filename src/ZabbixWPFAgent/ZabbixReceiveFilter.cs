using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZabbixActiveAgent
{
    //<HEADER> - "ZBXD\x01" (5 bytes)
    //<DATALEN> - data length(8 bytes). 1 will be formatted as 01/00/00/00/00/00/00/00 (eight bytes in HEX, 64 bit number)

    public class ZabbixReceiveFilter : FixedHeaderReceiveFilter<MyPackageInfo>
    {
        public ZabbixReceiveFilter() : base(13)
        {

        }
        public override void Reset()
        {

        }

        public override MyPackageInfo ResolvePackage(IBufferStream bufferStream)
        {
            byte[] header = bufferStream.Buffers[0].ToArray();
            byte[] bodyBuffer = bufferStream.Buffers[1].ToArray();
            byte[] allBuffer = bufferStream.Buffers[0].Array.CloneRange(0, (int)bufferStream.Length);
            if (allBuffer.Length < 13) return null;
            var package = new MyPackageInfo(allBuffer);
            return package;
        }

        protected override int GetBodyLengthFromHeader(IBufferStream bufferStream, int length)
        {
            ArraySegment<byte> buffers = bufferStream.Buffers[0];
            byte[] array = buffers.ToArray();
            byte[] lengthBuffer = array.CloneRange(length - 8, length - 1);
            int len = (int)((int)lengthBuffer[5]) + (int)(((int)lengthBuffer[6]) << 8) + (int)(((int)lengthBuffer[7]) << 16) + (int)(((int)lengthBuffer[8]) << 24);
            return len;
        }
    }
}
