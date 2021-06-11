@echo off

echo You are about to delete the Bin folder; do you want to continue? (Y/N)
set INPUT=
set /P INPUT=Type input: %=%
if /I "%INPUT%"=="y" goto yes
if /I "%INPUT%"=="n" goto no

:yes
rd /s /q "Bin"
rd /s /q "Source\Krypton Components\Krypton.Docking\obj"
rd /s /q "Source\Krypton Components\Krypton.Navigator\obj"
rd /s /q "Source\Krypton Components\Krypton.Ribbon\obj"
rd /s /q "Source\Krypton Components\Krypton.Toolkit\obj"
rd /s /q "Source\Krypton Components\Krypton.Workspace\obj"
del /f build.log

:no
pause