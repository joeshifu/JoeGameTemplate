@echo off
echo ** setting runtime variable

REM _protoSrc �����proto�ļ�Ŀ¼��λ��
set _protoSrc=F:\JoeWorkspace\Project\X-Game\Cfg_Build\build_protobuf\proto2java\proto

REM protoExe �����ڴ�proto����java��protoc.exe�����λ��
set protoExe=F:\JoeWorkspace\Project\X-Game\Cfg_Build\build_protobuf\proto2java\protoc.exe

REM java_out_file ������ɵ�Java�ļ�Ŀ¼��λ��
set java_out_file=F:\JoeWorkspace\Project\X-Game\Cfg_Build\build_protobuf\proto2java\java\

for /R "%_protoSrc%" %%i in (*) do ( 
	rem set filename=%%~nxi 
	if "%%~xi"  == ".proto" (
		%protoExe% --proto_path=%_protoSrc% --java_out=%java_out_file% %%i
	)
)

REM pause