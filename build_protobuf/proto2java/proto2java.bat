rem @echo off
rem mode con:cols=120 lines=40

rem ���赱ǰ��CMD·�� 

rem ���ǽ�������������Ŀ¼��
cd /d %~dp0 

set currentPath=%cd%

echo.%currentPath%

REM _protoSrc �����proto�ļ�Ŀ¼��λ��
set _protoSrc=%currentPath%\proto

REM protoExe �����ڴ�proto����java��protoc.exe�����λ��
set protoExe=%currentPath%\protoc.exe

REM java_out_file ������ɵ�java�ļ�Ŀ¼��λ��
set java_out_file=%currentPath%\java\

for /R "%_protoSrc%" %%i in (*) do (
 rem set filename=%%~nxi 
 if "%%~xi"  == ".proto" (

 %protoExe% --proto_path=%_protoSrc% --java_out=%java_out_file% %%i

 REM echo.%%~nxi to java ok !!!
 )
)

rem pause