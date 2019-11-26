using SuperSocket.Facility.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZabbixPassiveAgent
{
    //<HEADER> - "ZBXD\x01" (5 bytes)
    //<DATALEN> - data length(8 bytes). 1 will be formatted as 01/00/00/00/00/00/00/00 (eight bytes in HEX, 64 bit number)

    public class ZabbixReceiveFilter : FixedHeaderReceiveFilter<ZabbixRequestInfo>
    {
        public ZabbixReceiveFilter() : base(13)
        {

        }

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            byte[] lengthByte = new byte[8];
            Buffer.BlockCopy(header, offset + 5, lengthByte, 0, 8);
            var bodyLength = BitConverter.ToInt32(lengthByte, 0);
            return bodyLength;
        }

        protected override ZabbixRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            if (bodyBuffer == null) return null;

            var body = bodyBuffer.Skip(offset).Take(length).ToArray();
            var totalBuffer = new List<byte>();
            totalBuffer.AddRange(header.Array);
            totalBuffer.AddRange(body);

            var info = new ZabbixRequestInfo(totalBuffer.ToArray());
            return info;
        }
    }
}
