rem @echo off
set d=%date:~0,10%
set t=%time:~0,8%
set filename=D:/QZZC/DATA/YJXX
set apppath=E:\QZZC\APP
set datapath=E:\QZZC\DATA\YJXX
@echo %d% %t% :���ݵ��뿪ʼ>>yjxxrunninglog.txt
@echo call %apppath%\BuilkInsert.exe Staging_RWMS_CUSTOMER_STATUS "%datapath%\RWMS_CUSTOMER_STATUS.del">>yjxxrunninglog.txt

call %apppath%\BuilkInsert.exe Staging_RWMS_CUSTOMER_STATUS "%datapath%\RWMS_CUSTOMER_STATUS.del"

@echo %d% %t% :���ݵ������>>yjxxrunninglog.txt