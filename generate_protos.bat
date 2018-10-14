setlocal

@rem enter this directory
cd /d %~dp0

@rem packages will be available in nuget cache directory once the project is built or after "dotnet restore"
set protobuf_tools=%UserProfile%\.nuget\packages\google.protobuf.tools\3.6.1\tools
set PROTOC=%protobuf_tools%\windows_x64\protoc.exe
set grpc_tools=%UserProfile%\.nuget\packages\grpc.tools\1.15.0\tools
set PLUGIN=%grpc_tools%\windows_x64\grpc_csharp_plugin.exe

%PROTOC% -IIpc.Definitions -I%protobuf_tools% --csharp_out Ipc.Definitions  Ipc.Definitions/AcquisitionManagerService.proto --grpc_out Ipc.Definitions --plugin=protoc-gen-grpc=%PLUGIN%

endlocal