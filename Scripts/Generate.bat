@echo off
rem with snk file, use command below
rem IDLToCLSCompiler.exe -snk Key.snk SzMi hi.idl
..\bin\IDLToCLSCompiler.exe SzMi hi.idl
move SzMi.dll ..\Lib
echo Done!