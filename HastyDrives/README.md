# Hasty Ping 
A post-explotation command implemented in pure C# to list all drives and details that may be useful to a operator.

## Build 
All binaries in HastySeries are built targeting .NET 3.5, for windows 7+ support. The following build env should be used:

* Windows 10 - 1803
* Visual Studio 2017
* .NET 3.5 
* `choco install sysinternals` or strings from SysInternals in your current path

ALL HastySeries compiled binaries can be found on the github page with the most recent releases. NOTE: THESE have many static sigs.. dont drop to disk unless you are sure they are cleared via PSP testing.

## Operate
### Command Examples:
```cmd
C:\Users\rt\Desktop\HastySeries\bin\Release>HastyDrives.exe
```
### Expected Output:
```cmd
*-------------------------HastyDrives-------------------------*
|Drive C:\
|  Drive type: Fixed
|  Volume label:
|  File system: NTFS
|  Available space to current user:    21268123648 bytes
|  Total available space:              21268123648 bytes
|  Total size of drive:                63778582528 bytes
*--------------------------------------------------------------*
|Drive D:\
|  Drive type: CDRom
*--------------------------------------------------------------*
```

## OpSec
### Strings
To prevent some basic string matching, some basic precautions where taken. of course this is a example and if OpSec is upmost concern change static key and use the `HastyFixup` string fixup project to build new strings before re-compile.

1) All strings are XOR'd with a static key 
2) All strings are than encoded with Base64 
3) Strings are decoded at execution 
4) Strings are XOR'd with static key
5) String is presented to console 
