rem @echo off
set d=%date:~0,10%
set t=%time:~0,8%
set filename=D:/QZZC/DATA/SJJS
set apppath=E:\QZZC\APP
set datapath=E:\QZZC\DATA\SJJS
@echo %d% %t% :数据导入开始>>sjjsrunninglog.txt

call %apppath%\BuilkInsert.exe Staging_QZZC_APPC_ENT_INFO "%datapath%\QZZC_APPC_ENT_INFO.del"
call %apppath%\BuilkInsert.exe Staging_QZZC_UBNK_CRDT_INFO "%datapath%\QZZC_UBNK_CRDT_INFO.del"
call %apppath%\BuilkInsert.exe Staging_QZZC_UBNK_LOAN_BIZ "%datapath%\QZZC_UBNK_LOAN_BIZ.del"
call %apppath%\BuilkInsert.exe Staging_RW_IND_BASIC_INFO_TAB "%datapath%\RW_IND_BASIC_INFO_TAB.del"	
call %apppath%\BuilkInsert.exe Staging_T01_CUST_RELATIVE_G "%datapath%\T01_CUST_RELATIVE_G.del"
call %apppath%\BuilkInsert.exe Staging_T01_IC_ALI_OWE_LOAN_C "%datapath%\T01_IC_ALI_OWE_LOAN_C.del"
call %apppath%\BuilkInsert.exe Staging_T01_IC_BREAK_DEBETOR_C "%datapath%\T01_IC_BREAK_DEBETOR_C.del"
call %apppath%\BuilkInsert.exe Staging_T01_IC_DEBETOR_C "%datapath%\T01_IC_DEBETOR_C.del"	
call %apppath%\BuilkInsert.exe Staging_T01_IND_CUST_BIZ_INFO_P "%datapath%\T01_IND_CUST_BIZ_INFO_P.del"	
call %apppath%\BuilkInsert.exe Staging_T01_IND_CUST_CORE_INFO_P "%datapath%\T01_IND_CUST_CORE_INFO_P.del"
call %apppath%\BuilkInsert.exe Staging_T01_IND_ERA_PAY_STAT_P "%datapath%\T01_IND_ERA_PAY_STAT_P.del"

@echo %d% %t% :数据导入结束>>sjjsrunninglog.txt