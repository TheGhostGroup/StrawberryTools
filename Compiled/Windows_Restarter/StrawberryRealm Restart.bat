@ECHO OFF
CLS
if NOT exist StrawberryRealm.exe GoTO END
ECHO StrawberryRealm Started %time:~0,5% %date:~1% >> .\RealmRestarter.log
:RESTART 
StrawberryRealm.exe
ECHO StrawberryRealm Restarted %time:~0,5% %date:~1% >> .\RealmRestarter.log
ECHO. 
GOTO RESTART 
:END
ECHO StrawberryRealm.exe not found.
ECHO.
ECHO Restarter and Realm must be in the same folder.
pause
/* Copyright by Terra */