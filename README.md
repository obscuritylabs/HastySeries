

| Branch | Version | Status | License | Chat | Hitcount |
| :----: | :-----: | :----: | :----: | :----: | :----: | 
| Master |v0.1.0 | [![Build Status](https://travis-ci.com/obscuritylabs/HastySeries.svg?token=WijX13S3UsZRzVurRNNm&branch=master)](https://travis-ci.com/obscuritylabs/HastySeries) | [![License](https://img.shields.io/badge/License-BSD%203--Clause-blue.svg)](https://opensource.org/licenses/BSD-3-Clause) | [![Gitter](https://badges.gitter.im/HastySeries/community.svg)](https://gitter.im/HastySeries/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge) | [![HitCount](http://hits.dwyl.io/obscuritylabs/HastySeries.svg)](http://hits.dwyl.io/obscuritylabs/HastySeries)|

# HastySeries
A C# toolset to support offensive operators to triage, asses and make intelligent able decisions. Provided operators access to toolsets that can be integrated into other projects and workflow throughout a Red Team, Pentest or host investigation. We built this toolset over a period of a few days, hence the tool prefix of "Hasty".

## Authors & Development 
|                Name              |      Twitter Handle     |
| :------------------------------: | :---------------------: |
| Alexander Rymdeko-Harvey         | @killswitch-GUI         |
| Scottie Austin                   | @CheckyMander           |

## Current Release & Pre-Compiled Binaries *for those that into that kind of thing..*
All releases can be found on GitHub. We do include a compied set of bin's that may be used for testing and analysis.
Although it may be worth it to compile your self :)

Release can be found here`https://github.com/obscuritylabs/HastySeries/releases`

All releases contain:
* Debug Builds
* Release Builds
* Accompanied Strings

## Build 
All binaries in HastySeries are built targeting .NET 3.5, for windows 7+ support. The following build env should be used:

* Windows 10 - 1803
* Visual Studio 2017-2019
* .NET 3.5 
* `choco install sysinternals` or strings from SysInternals in your current path

ALL HastySeries compiled binaries can be found on the github page with the most recent releases. NOTE: THESE have many static sigs.. dont drop to disk unless you are sure they are cleared via PSP testing.

## OpSec
### Assembly Names 
One major chnage to the assembly namespace is the naming convention, all projects have been moved to the `Action` Namespace. 

### PDB
All PDB settings have been disabled for Release builds.


### Strings
To prevent some basic string matching, some basic precautions where taken. of course this is a example and if OpSec is upmost concern change static key and use the `HastyFixup` string fixup project to build new strings before re-compile.

1) All strings are XOR'd with a static key 
2) All strings are than encoded with Base64 
3) Strings are decoded at execution 
4) Strings are XOR'd with static key
5) String is presented to console 


## Tool TOC (Table of Contents)
|           Tool Code Name         |      Type     |  .NET Framework |                Usage/README            |
| :------------------------------: | :-----------: | :-------------: | :------------------------------------: |
| [HastyArp](#hastyarp)            |  ENUMERATION  |       3.5       |   [README.md](HastyArp/README.md)      |
| [HastyDrives](#hastydrives)      |  ENUMERATION  |       3.5       |   [README.md](HastyDrives/README.md)   |
| [HastyDump](#hastydump)          |  COLLECTION   |       3.5       |   [README.md](HastyDump/README.md)     |
| [HastyFixup](#hastyfixup)        |     OPSEC     |       3.5       |   [README.md](HastyFixup/README.md)    |
| [HastyLogging](#hastylogging)    |       SA      |       3.5       |   [README.md](HastyLogging/README.md)  |
| [HastyNslookup](#hastynslookup)  |  ENUMERATION  |       3.5       |   [README.md](HastyNslookup/README.md) |
| [HastyPing](#hastyping)          |  ENUMERATION  |       3.5       |   [README.md](HastyPing/README.md)     |
| [HastyShellcode](#hastyshellcode)|   EXECUTION   |       3.5       |   [README.md](HastyShellcode/README.md)|
| [HastyShot](#hastyshot)          |  COLLECTION   |       3.5       |   [README.md](HastyShot/README.md)     |
| [HastyStroke](#hastystroke)      |  COLLECTION   |       3.5       |   [README.md](HastyStroke/README.md)   |
| [HastyUptime](#hastyuptime)      |  ENUMERATION  |       3.5       |   [README.md](HastyUptime/README.md)   |

## HastyArp
**Type:** ENUMERATION  
**.NET Framework:** 3.5  
**Usage/README:** [README.md](HastyArp/README.md) 

This project aims to provide a user with the ability to perform arp requests. This project uses the `IpHlpApi.dll` for native functionality. 

## HastyDrives
**Type:** ENUMERATION  
**.NET Framework:** 3.5  
**Usage/README:** [README.md](HastyDrives/README.md) 

This project aims to provide a user with the ability to perform lists drives on execution system. This project uses `System.IO` namespace to easily accomplish this.

## HastyDump
**Type:** COLLECTION  
**.NET Framework:** 3.5  
**Usage/README:** [README.md](HastyDump/README.md) 

This project aims to provide a user with the ability to perform collection operations to perform MiniDumps of a process as well as a raw mem dump. This provides operators with multiple choices to achieve access to process memory space.

## HastyFixup
**Type:** OPSEC  
**.NET Framework:** 3.5  
**Usage/README:** [README.md](HastyFixup/README.md) 

This project aims to provide a user with the ability to perform OPSEC operations on the tools provided in this repo. The tools help XOR strings, PE Fixups etc.

## HastyLogging
**Type:** SA  
**.NET Framework:** 3.5  
**Usage/README:** [README.md](HastyLogging/README.md) 

This project aims to provide a user with the ability to perform situational awareness operations of auditing settings. This project uses a mix of built in and native functionality.

## HastyNslookup
**Type:** ENUMERATION  
**.NET Framework:** 3.5  
**Usage/README:** [README.md](HastyNslookup/README.md) 

This project aims to provide a user with the ability to perform nslookup enumeration operations. This project uses `System.Net` namespace to easily accomplish this.

## HastyPing
**Type:** ENUMERATION  
**.NET Framework:** 3.5  
**Usage/README:** [README.md](HastyPing/README.md) 

This project aims to provide a user with the ability to perform ping enumeration operations. This project uses `System.Net` namespace to easily accomplish this.

## HastyShellcode

## HastyShot
**Type:** COLLECTION  
**.NET Framework:** 3.5  
**Usage/README:** [README.md](HastyShot/README.md) 

This project aims to provide a user with the ability to perform collection operations of auditing settings. This project uses a mix of built in and native functionality.

## HastyStroke
**Type:** COLLECTION  
**.NET Framework:** 3.5  
**Usage/README:** [README.md](HastyShot/README.md) 

This project aims to provide a user with the ability to perform keystroke collection operations. This project uses a mix of built in and native functionality.

## HastyUptime
**Type:** SA  
**.NET Framework:** 3.5  
**Usage/README:** [README.md](HastyUptime/README.md) 

This project aims to provide a user with the ability to perform situational awareness operations of current system uptime. This project uses native functionality.
