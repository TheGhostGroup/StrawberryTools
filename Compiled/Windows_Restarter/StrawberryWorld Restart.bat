@ECHO OFF
CLS
if NOT exist StrawberryWorld.exe GoTO END
ECHO TrinityLogon Started %time:~0,5% %date:~1% >> .\RealmRestarter.log
:RESTART 
StrawberryWorld.exe
ECHO TrinityLogon Restarted %time:~0,5% %date:~1% >> .\RealmRestarter.log
ECHO. 
GOTO RESTART 
:END
ECHO StrawberryWorld.exe not found.
ECHO.
ECHO restarter and World must be in the same folder.
pause
/* Copyright by Terra */