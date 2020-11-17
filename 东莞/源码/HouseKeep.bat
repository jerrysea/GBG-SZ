@ECHO OFF
REM 在E盘创建一个文件夹存储该文件E:\Tool
SET LOGFILE=E:\Tool\dummyHouseKeepRun.log

echo %date% %time% run HouseKeep.bat>> %LOGFILE%

echo %date% %time% clean Instinct output log>> %LOGFILE%
SET LOGFOLDER=E:\DECTECH SOLUTIONS\INSTINCT\CN\OUTPUT
E:
CD %LOGFOLDER%
forfiles /p "%LOGFOLDER%" /d -15 /c "cmd /c if @isdir==FALSE echo deleting file @path ... && del /f @path"
echo %date% %time% end to clean Instinct output log>> %LOGFILE%

echo %date% %time% clean ESB Interface log>> %LOGFILE%
SET LOGFOLDER=E:\DRC
forfiles /p "%LOGFOLDER%" /d -15 /c "cmd /c if @isdir==FALSE echo deleting file @path ... && del /f @path"
echo %date% %time% end to clean ESB Interface log>> %LOGFILE%

echo %date% %time% end run HouseKeep.bat>> %LOGFILE%
