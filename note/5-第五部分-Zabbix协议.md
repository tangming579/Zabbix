## Zabbix 协议

某些场合下，我们更希望使用自己开发的客户端，即在自己的业务程序中内嵌 Zabbix 客户端，用自己写的客户端发送数据给 Zabbix-Server。Zabbix-Agent 和 Zabbix-Server 之间的通信是采用 Zabbix 协议来实现的。因此，我们完全可以写一个 Zabbix-Sender 内置到业务程序中，从而避免在系统中安装 Zabbix 官方提供的客户端程序。

### Zabbix 协议概述

Zabbix 协议是 Zabbix 各程序间通信的准则，如下展示了各模块的协议和版本支持情况：

<div>
    <image src="../img/zabbix-protocols.png"></image>
</div>

该表官方地址：<https://www.zabbix.org/wiki/Docs/protocols>

在Zabbix 各个程序间通信中，其协议传输数据的格式为 JSON。

### Zabbix-Sender 协议



### Zabbix-Get 协议



### Zabbix-Agent协议

zabbix agent的运行模式有以下两种：

1. 被动模式：此模式为zabbix默认的工作模式，由zabbix server 向zabbix agent 发出指令获取数据，zabbix agent被动地去获取数据并返回给zabbix server，zabbix server会周期性地向agent索取数据。此模式的最大问题就是会增加zabbix server的工作量，在大量的服务器环境下，zabbix server不能及时获取到最新的数据。
2. 主动模式：即由zabbix agent 主动采集数据并返回给zabbix server，不需要zabbix server 的另行干预，因此使用主动模式能在一定程序上减轻zabbix server的压力。

相关文档：[被动和主动代理检查](<https://www.zabbix.com/documentation/3.4/zh/manual/appendix/items/activepassive>)

**被动方式**

zabbix-server和zabbix-agent之间的通信是zabbix的专用协议，数据格式为JSON。默认情况下，zabbix-agent工作在被动模式下，工作的模式是由Key和zabbix_agentd.conf参数配置决定的。

被动模式的流程如下：

- Server打开一个TCP连接。
- Server发送一个key为agent.ping\n。
- Agent接收到这个请求，然后响应数据<HEADER><DATALEN>1.
- Server对接收到的数据进行处理。
- TCP连接关闭。

客户端的配置：/etc/zabbix/zabbix_agentd.conf配置文件中设置ServerActive=192.168.1.103（这个IP可以是server也可以是proxy的IP地址），然后重启zabbix_agentd服务。

　　服务端的配置：服务器端items的检测方式（Type）修改为Zabbix agent(active)

**主动方式的请求周期**

- Agent向Server建立一个TCP的连接。
- Agent请求需要检测的数据列表。
- Server响应Agent，发送一个Items列表（item key、delay）。
- Agent响应请求。
- TCP连接完成本次会话后关闭。
- Agent开始周期性的收集数据。

Agent要向Server发送数据：

- Agent向Server建立一个TCP连接。
- Agent发送在采集周期内，需要采集数据给Server.
- Server处理Agent发送的数据。
- TCP连接关闭。