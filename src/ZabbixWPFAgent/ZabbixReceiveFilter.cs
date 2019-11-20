using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZabbixWPFAgent
{
    public class ZabbixReceiveFilter : FixedHeaderReceiveFilter<MyPackageInfo>
    {
        public ZabbixReceiveFilter() : base(4)
        {

        }
        public override void Reset()
        {

        }

        public override MyPackageInfo ResolvePackage(IBufferStream bufferStream)
        {
            //第三个参数用0,1都可以
            byte[] header = bufferStream.Buffers[0].ToArray();
            byte[] bodyBuffer = bufferStream.Buffers[1].ToArray();
            byte[] allBuffer = bufferStream.Buffers[0].Array.CloneRange(0, (int)bufferStream.Length);
            if (allBuffer.Length < 6) return null;
            var isReply = allBuffer.Length == 6;
            var package = new MyPackageInfo(allBuffer, isReply);
            return package;
        }

        protected override int GetBodyLengthFromHeader(IBufferStream bufferStream, int length)
        {
            ArraySegment<byte> buffers = bufferStream.Buffers[0];
            byte[] array = buffers.ToArray();
            int len = array[length - 2] * 256 + array[length - 1];
            //文档中定义的长度是指一个字，实际按字节算，所以需要乘以2
            return len * 2;
        }
    }
}
