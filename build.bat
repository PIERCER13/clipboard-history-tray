@echo off
cd /d "%~dp0"
set CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe
"%CSC%" /nologo /target:winexe /optimize+ /out:piercer_clipboard_tray.exe Program.cs
