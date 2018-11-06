forfiles /P %1 /M *.* /D -%2 &&forfiles /P %1 /M *.* /D -%2 /C "cmd /c del /q @path" ||exit /b 0
