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

