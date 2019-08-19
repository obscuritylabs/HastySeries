# HastyArp 
A post-explotation ARP.exe command implemented in pure C#.

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
C:\Users\rt\Desktop\HastySeries\bin\Release>HastyArp.exe
```
### Expected Output:
```cmd
Interface: INT: 1 --- TYPE: 4 --- IP: 224.0.0.22
        224.0.0.22          00-00-00-00-00-00
        239.255.255.250     00-00-00-00-00-00
Interface: INT: 3 --- TYPE: 4 --- IP: 224.0.0.22
        224.0.0.22          01-00-5E-00-00-16
Interface: INT: 7 --- TYPE: 3 --- IP: 192.168.137.2
        192.168.137.2       00-50-56-E9-02-48
        192.168.137.254     00-50-56-E3-0F-55
        192.168.137.255     FF-FF-FF-FF-FF-FF
        224.0.0.22          01-00-5E-00-00-16
        224.0.0.251         01-00-5E-00-00-FB
        224.0.0.252         01-00-5E-00-00-FC
        255.255.255.255     FF-FF-FF-FF-FF-FF
Interface: INT: 8 --- TYPE: 2 --- IP: 169.254.169.254
        169.254.169.254     00-00-00-00-00-00
        169.254.241.22      00-00-00-00-00-00
        169.254.255.255     FF-FF-FF-FF-FF-FF
        224.0.0.22          01-00-5E-00-00-16
        224.0.0.251         01-00-5E-00-00-FB
        224.0.0.252         01-00-5E-00-00-FC
        239.255.255.250     01-00-5E-7F-FF-FA
        255.255.255.255     FF-FF-FF-FF-FF-F
```

## OpSec
### Strings
To prevent some basic string matching, some basic precautions where taken. of course this is a example and if OpSec is upmost concern change static key and use the `HastyFixup` string fixup project to build new strings before re-compile.

1) All strings are XOR'd with a static key 
2) All strings are than encoded with Base64 
3) Strings are decoded at execution 
4) Strings are XOR'd with static key
5) String is presented to console 
