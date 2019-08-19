# Hasty Dump 
A post-explotation process dumper implmenting multiple methods.

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
C:\Users\rt\Desktop\HastySeries\bin\Release>HastyDump.exe -help
C:\Users\rt\Desktop\HastySeries\bin\Release>HastyDump.exe 13028 "C:\\Users\\rt\\Desktop\\test.bin"
```
### Expected Output:
```cmd
[*] RUNTIME TARGET CHECKS:
        OperatingSystem Version: Microsoft Windows NT 6.2.9200.0
        Target MachineName: DESKTOP-1VRIH74
        Target DomainName: DESKTOP-1VRIH74
        Target UserName: rt
        Target Time Zone: Pacific Standard Time
        Target Time: 8/18/2019 9:56:55 PM
        Target ProcessorCount: 4
[*] SUCCESS: Obtained process image name
[*] SUCCESS: Obtained process image name
[*] INFO: target image: \Device\HarddiskVolume4\Windows\System32\cmd.exe
[*] SUCCESS: Creating file stream/handle: C:\\Users\\rt\\Desktop\\test.bin
[*] IMAGE TARGET DETAILS:
        Image size: 129872 KB
        Image location: C:\Users\rt\Desktop\test.bin
[*] INFO: Close file handle of: C:\\Users\\rt\\Desktop\\test.bin
[*] INFO: Close process handle of process ID: 13028
```

## OpSec
### Strings
To prevent some basic string matching, some basic precautions where taken. of course this is a example and if OpSec is upmost concern change static key and use the `HastyFixup` string fixup project to build new strings before re-compile.

1) All strings are XOR'd with a static key 
2) All strings are than encoded with Base64 
3) Strings are decoded at execution 
4) Strings are XOR'd with static key
5) String is presented to console 
