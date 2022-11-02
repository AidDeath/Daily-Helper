# Daily-Helper
Helper for everyday local network PCs monitoring

Simple App runs test tasks with delay according to settings

Main module is made with .Net 6, WCF service and windows service are mane with .Net Framework 3.5

Possible tasks:
  - Send ICMP request (ping)
  - Try to connect to a specified port of a server
  - Try to have access to a network shares of PC in local network
    -- also shows amount of free space on disks where shares are situated
  - Check if a state of a specified Windows service runnuning on a specified machine over network
  - Check if a specified process is responding on remote machine over network (Requires Windows Service installation to a target machine)
  - Check available logical drives free space on remote machine (Requires Windows Service installation to a target machine)
    
  Main window shows table with results of testings, including last success time and some other information.
  
  
  App made for personal use, there will be added specific queries and test, that couldn't be used in any networks except mine.
  
