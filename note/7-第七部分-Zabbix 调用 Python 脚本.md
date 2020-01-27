## Zabbix 调用 Python 脚本

1. 下载安装最新版Python：<https://www.python.org/downloads/>

   ```powershell
   #查看已安装Python位置
   where python
   ```

2. 下载 psutil：<https://pypi.org/project/psutil/#files>

3. cmd进入依赖存放的目录，确保 [Python路径]/scripts 在系统环境变量中，执行以下命令：

   ```
   pip install xxx.whl
   ```

4. 进入 Zabbix Agent 安装目录，打开 zabbix_agentd.conf

   ```
   UserParameter=window.cmd[*], $1
   UserParameter=process_cpu_pused[*],[python路径]\python d:\sysInfo.py $1
   ```

   注：python 路径必须是完整路径，否则提示找不大命令 ‘python' 

5. 在 Zabbix 中添加监控项

