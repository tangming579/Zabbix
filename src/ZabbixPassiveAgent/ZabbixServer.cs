using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZabbixPassiveAgent
{
    public class ZabbixServer: AppServer<ZabbixSession, ZabbixRequestInfo>
    {
        public ZabbixServer()
           : base(new DefaultReceiveFilterFactory<ZabbixReceiveFilter, ZabbixRequestInfo>())
        {
            this.NewSessionConnected += MyServer_NewSessionConnected;
            this.SessionClosed += MyServer_SessionClosed;
        }

        public ZabbixServer(SessionHandler<ZabbixSession> NewSessionConnected, SessionHandler<ZabbixSession, CloseReason> SessionClosed)
            : base(new DefaultReceiveFilterFactory<ZabbixReceiveFilter, ZabbixRequestInfo>())
        {
            this.NewSessionConnected += NewSessionConnected;
            this.SessionClosed += SessionClosed;
        }

        protected override void OnStarted()
        {
            //启动成功
        }

        void MyServer_NewSessionConnected(ZabbixSession session)
        {
            //连接成功
        }

        void MyServer_SessionClosed(ZabbixSession session, CloseReason value)
        {

        }
    }
}
