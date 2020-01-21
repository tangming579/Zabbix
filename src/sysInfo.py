import psutil
import re
import sys

# UserParameter=process_cpu_pused[*], python D:\Program Files\Zabbix Agent\sysInfo.py $1

def findProcessByName(processName):
    for proc in psutil.process_iter():
        try:
            if proc.name().lower() == processName.lower():
                return proc  # return if found one
        except psutil.AccessDenied:
            pass
        except psutil.NoSuchProcess:
            pass
    return None

process = findProcessByName(sys.argv[1])            
cpuPercent = process.cpu_percent(1)
print(cpuPercent)