@echo off  
rem 关闭回显

rem @echo off
rem mode con:cols=120 lines=40

rem 重设当前的CMD路径 

rem 就是进入批处理所在目录了
cd /d %~dp0 

set currentPath=%cd%

rem echo.%currentPath%

set _protoSrc=%currentPath%\proto

set protoExe=%currentPath%\protoc.exe

set out_file=%currentPath%\byte

for /R "%_protoSrc%" %%i in (*) do (
 rem set filename=%%~nxi 
 if "%%~xi"  == ".proto" (
 rem	echo.%%~ni
rem echo.%protoExe% -o %out_file%\%%~ni.pb %%i  
rem %protoExe% -o %out_file%\%%~ni.pb %%i

rem echo.%protoExe% -o %%~ni.pb %%~ni.proto 
%protoExe% -o %%~ni.pb %%~ni.proto 
 )
)

pause