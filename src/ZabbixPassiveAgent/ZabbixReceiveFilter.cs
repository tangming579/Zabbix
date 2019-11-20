using SuperSocket.Facility.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZabbixPassiveAgent
{
    public class ZabbixReceiveFilter : FixedHeaderReceiveFilter<ZabbixRequestInfo>
    {
        public ZabbixReceiveFilter() : base(13)
        {

        }

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            var bodyLength = (int)header[offset + 2] * 256 + (int)header[offset + 3];

            return bodyLength * 2;
        }

        protected override ZabbixRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            if (bodyBuffer == null) return null;

            var body = bodyBuffer.Skip(offset).Take(length).ToArray();
            if (body.Length < 2) return null;
            var isReply = body.Length == 2;

            var totalBuffer = new List<byte>();
            totalBuffer.AddRange(header.Array);
            totalBuffer.AddRange(body);

            var info = new ZabbixRequestInfo(totalBuffer.ToArray());
            return info;
        }
    }
}
