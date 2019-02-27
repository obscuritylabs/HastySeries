import pefile
import os
import json
import codecs
import string
import datetime
from struct import *
from pefile import debug_types
from termcolor import colored
import hashlib
import time
import magic
import ssdeep
import pyexifinfo as exif
from virus_total_apis import PublicApi as VirusTotalPublicApi 

_file_name = 'RickJames.exe'
_file_name_live = 'OfficeUpdateCheck.exe'
_file_compile_time = 1454680254
_file_major_linker_version = 10
_file_minor_linker_version = 1
_pdb_file_name = "fun"
API_KEY = '347bc53a0dca63e90fe7e7fd70aaa6f186cb32e4ad3c0e168bc775a819a2028f'


def strings(filename, min=4):
    with open(filename, errors="ignore") as f:  # Python 3.x
    # with open(filename, "rb") as f:           # Python 2.x
        result = ""
        for c in f.read():
            if c in string.printable:
                result += c
                continue
            if len(result) >= min:
                yield result
            result = ""
        if len(result) >= min:  # catch result at EOF
            yield result

for s in strings(_file_name):
    #print(s)
    pass


pe = pefile.PE(_file_name)
print("============= ORIGINAL FILE DATA =============")
print("|-* IF LIVE OPS SAVE THIS DATA TO OP SHARE *-|")
print("==============================================")
print("[*] EXE metadata:")
print(f" - File Name: {colored(_file_name, 'yellow')}")
print(f" - e_magic value: {hex(pe.DOS_HEADER.e_magic)}")
print(f" - Signature value: {hex(pe.NT_HEADERS.Signature)}")
print(f" - Imphash: {pe.get_imphash()}")
print(f" - Size of executable code: {int(pe.OPTIONAL_HEADER.SizeOfCode) / 1024}KB")
print(f" - Size of executable image : {int(pe.OPTIONAL_HEADER.SizeOfImage) / 1024}KB")
print("[*] FILE_HEADER:")
print(f" - Machine type value: {hex(pe.FILE_HEADER.Machine)}")
print(f" - TimeDateStamp value: '{datetime.datetime.fromtimestamp(int(pe.FILE_HEADER.TimeDateStamp)).strftime('%c')}' ")
print("[*] IMAGE_OPTIONAL_HEADER64:")
print(f" - Magic value: {hex(pe.OPTIONAL_HEADER.Magic)}")
print(f" - Major Linker Version: {hex(pe.OPTIONAL_HEADER.MajorImageVersion)}")
print(f" - Minor Linker Version: {hex(pe.OPTIONAL_HEADER.MajorLinkerVersion)}")
print(f" - Major OS Version: {hex(pe.OPTIONAL_HEADER.MajorOperatingSystemVersion)}")
print(f" - Minor OS Version: {hex(pe.OPTIONAL_HEADER.MinorOperatingSystemVersion)}")
print("-----------------------------------------------")
print("[*] DEBUG INFO:")
for x in pe.DIRECTORY_ENTRY_DEBUG:
    print(f"\t[*] Type: {debug_types[x.struct.Type]}")
    print(f"\t\t- TimeDateStamp value: '{datetime.datetime.fromtimestamp(int(x.struct.TimeDateStamp)).strftime('%c')}'")
    if x.entry:
        if x.entry.name == 'CV_INFO_PDB70':
            # print debug strings
            print(f"\t\t- PdbFileName value: '{colored(x.entry.PdbFileName,'red')}'")


print("-----------------------------------------------")
print("[*] Listing imported DLLs...")
_val = []
for entry in pe.DIRECTORY_ENTRY_IMPORT:
    _val.append(_val)
    print('\t' + colored(entry.dll.decode('utf-8'), 'magenta'))

for entry in pe.DIRECTORY_ENTRY_IMPORT:
    dll_name = entry.dll.decode('utf-8')
    if 'api' not in dll_name:
        print(f"[*] {colored(dll_name, 'magenta')} imports:")
        for func in entry.imports:
            print("\t%s at 0x%08x" % (colored(func.name.decode('utf-8'), 'blue'), func.address))
    else: 
        print(f"[-] Not printing imports of {dll_name} no need..")

## CUSTOM CODE 
with open(_file_name, 'rb') as f:
    raw = f.read()
with open(_file_name_live, 'wb') as f:
    f.write(raw)
with open(_file_name_live, 'rb') as f:
    live_raw = f.read()
print("============= PRE-FLIGHT CHECKS ==============")
print("|-* IF LIVE OPS SAVE THIS DATA TO OP SHARE *-|")
print("==============================================")
preflight = True
print("[*] File re-write checks:")
print(f" - SHA256 of non-cooked payload: {colored(hashlib.sha256(raw).hexdigest(), 'yellow')}")
print(f" - SHA256 of cooked payload: {colored(hashlib.sha256(live_raw).hexdigest(), 'yellow')}")
if hashlib.sha256(live_raw).hexdigest() != hashlib.sha256(raw).hexdigest():
    preflight = False
    print(colored(f" - SHA256 of cooked payload DOES NOT MATCH, somthing is wrong..", 'red'))
if preflight:
    print(f"Preflight checks: {colored('PASS', 'green')}")
status = False
with open(_file_name_live, 'r+b') as filehandle:
    filehandle_raw = filehandle.read()
    print("============= TAINTED FILE DATA ==============")
    print("|-* IF LIVE OPS SAVE THIS DATA TO OP SHARE *-|")
    print("==============================================")
    
    print("[*] Walking DOS_HEADER:")
    print(f" - Target e_lfanew offset value: {hex(pe.DOS_HEADER.e_lfanew)}")
    target_offset = pe.DOS_HEADER.e_lfanew+4 
    print(f" - Set e_lfanew offset + PE bytes: {hex(pe.DOS_HEADER.e_lfanew+4)}")
    print("-----------------------------------------------")
    # now add 4 bytes to the value for the ASCII string 'PE'
    print("[*] Walking IMAGE_FILE_HEADER:")
    ifh_tds = target_offset+4
    print(f" - TimeDateStamp offset value: {hex(ifh_tds)}")
    print(f" - TimeDateStamp hex value: {hex(unpack('L', raw[ifh_tds:ifh_tds+0x8])[0])}")
    print(f" - TimeDateStamp int value: {unpack('L', raw[ifh_tds:ifh_tds+0x8])[0]}")
    print(f" - TimeDateStamp time date value: {datetime.datetime.fromtimestamp(int(unpack( 'L', raw[ifh_tds:ifh_tds+0x8])[0]))}")
    filehandle.seek(ifh_tds, 0)
    filehandle.write(pack('L', _file_compile_time))
    print(colored(f" ==> TimeDateStamp stomped start write location: {hex(ifh_tds)}", "cyan"))
    print(colored(f" ==> Setting TimeDateStamp stomped int value to: {_file_compile_time}", "cyan"))
    print(colored(f" ==> Setting TimeDateStamp stomped hex value to: {hex(_file_compile_time)}", "cyan"))
    print(colored(f" ==> TimeDateStamp time date value: {datetime.datetime.fromtimestamp(int(_file_compile_time))}", "cyan"))
    # now stomp linker
    print("[*] Walking IMAGE_OPTIONAL_HEADER:")
    print(f" - Magic offset value: {hex(pe.OPTIONAL_HEADER.__file_offset__)}")
    print(f" - Magic hex value: {hex(unpack('H', raw[pe.OPTIONAL_HEADER.__file_offset__:pe.OPTIONAL_HEADER.__file_offset__+0x2])[0])}")
    print(f" - MajorLinkerVersion offset value: {hex(pe.OPTIONAL_HEADER.__file_offset__+pe.OPTIONAL_HEADER.__field_offsets__['MajorLinkerVersion'])}")
    print(f" - MajorLinkerVersion hex value: {hex(unpack('B', raw[pe.OPTIONAL_HEADER.__file_offset__+pe.OPTIONAL_HEADER.__field_offsets__['MajorLinkerVersion']:pe.OPTIONAL_HEADER.__file_offset__+pe.OPTIONAL_HEADER.__field_offsets__['MajorLinkerVersion']+0x1])[0])}")
    print(f" - MajorLinkerVersion int value: {unpack('B', raw[pe.OPTIONAL_HEADER.__file_offset__+pe.OPTIONAL_HEADER.__field_offsets__['MajorLinkerVersion']:pe.OPTIONAL_HEADER.__file_offset__+pe.OPTIONAL_HEADER.__field_offsets__['MajorLinkerVersion']+0x1])[0]}")
    print(f" - MinorLinkerVersion offset value: {hex(pe.OPTIONAL_HEADER.__file_offset__+pe.OPTIONAL_HEADER.__field_offsets__['MinorLinkerVersion'])}")
    print(f" - MinorLinkerVersion hex value: {hex(unpack('B', raw[pe.OPTIONAL_HEADER.__file_offset__+pe.OPTIONAL_HEADER.__field_offsets__['MinorLinkerVersion']:pe.OPTIONAL_HEADER.__file_offset__+pe.OPTIONAL_HEADER.__field_offsets__['MinorLinkerVersion']+0x1])[0])}")
    print(f" - MinorLinkerVersion int value: {unpack('B', raw[pe.OPTIONAL_HEADER.__file_offset__+pe.OPTIONAL_HEADER.__field_offsets__['MinorLinkerVersion']:pe.OPTIONAL_HEADER.__file_offset__+pe.OPTIONAL_HEADER.__field_offsets__['MinorLinkerVersion']+0x1])[0]}")
    
    print(colored(f" ==> MajorLinkerVersion stomped start write location: {hex(pe.OPTIONAL_HEADER.__file_offset__+pe.OPTIONAL_HEADER.__field_offsets__['MajorLinkerVersion'])}", "cyan"))
    print(colored(f" ==> Setting MajorLinkerVersion stomped int value to: {_file_major_linker_version}", "cyan"))
    print(colored(f" ==> Setting MajorLinkerVersion stomped hex value to: {hex(_file_major_linker_version)}", "cyan"))
    filehandle.seek(pe.OPTIONAL_HEADER.__file_offset__+pe.OPTIONAL_HEADER.__field_offsets__['MajorLinkerVersion'], 0)
    filehandle.write(pack('B', _file_major_linker_version))
    print("[*] DEBUG INFO:")
    for x in pe.DIRECTORY_ENTRY_DEBUG:
        print(f"\t[*] Type: {debug_types[x.struct.Type]}")
        print(f"\t\t- Debug TimeDateStamp offset value: {hex(x.struct.get_field_absolute_offset('TimeDateStamp'))}")
        print(f"\t\t- TimeDateStamp hex value: {hex(unpack('L', raw[x.struct.get_field_absolute_offset('TimeDateStamp'):x.struct.get_field_absolute_offset('TimeDateStamp')+0x8])[0])}")
        print(f"\t\t- TimeDateStamp int value: {unpack('L', raw[x.struct.get_field_absolute_offset('TimeDateStamp'):x.struct.get_field_absolute_offset('TimeDateStamp')+0x8])[0]}")
        print(f"\t\t- TimeDateStamp time date value: {datetime.datetime.fromtimestamp(int(unpack('L', raw[x.struct.get_field_absolute_offset('TimeDateStamp'):x.struct.get_field_absolute_offset('TimeDateStamp')+0x8])[0]))}")
        filehandle.seek(x.struct.get_field_absolute_offset('TimeDateStamp'), 0)
        filehandle.write(pack('L', _file_compile_time))
        print(colored(f"\t\t==> TimeDateStamp stomped start write location: {hex(x.struct.get_field_absolute_offset('TimeDateStamp'))}", "cyan"))
        print(colored(f"\t\t==> Setting TimeDateStamp stomped int value to: {_file_compile_time}", "cyan"))
        print(colored(f"\t\t==> Setting TimeDateStamp stomped hex value to: {hex(_file_compile_time)}", "cyan"))
        print(colored(f"\t\t==> TimeDateStamp time date value: {datetime.datetime.fromtimestamp(int(_file_compile_time))}", "cyan"))
        if x.entry:
            if x.entry.name == 'CV_INFO_PDB70':
                # print debug strings
                print(f"\t\t- PdbFileName offset value: {hex(x.entry.__file_offset__ + x.entry.__field_offsets__['PdbFileName'])}")
                print(f"\t\t- PdbFileName value: '{colored(x.entry.PdbFileName,'red')}'")
                filehandle.seek(x.entry.__file_offset__ + x.entry.__field_offsets__['PdbFileName'], 0)
                p = filehandle.read()
                chars = []
                for y in p:
                    chars.append(chr(y))
                    if y == 0:
                        break
                clean_chars = b''
                for y in chars:
                    clean_chars += b'\x00'
                print(f"\t\t- PdbFileName null-term string: '{colored(chars,'red')}'")
                print(colored(f"\t\t==> PdbFileName stomped start write location: {hex(x.entry.__file_offset__ + x.entry.__field_offsets__['PdbFileName'])}", "cyan"))
                print(colored(f"\t\t==> PdbFifleName stomped end write location: {hex(x.entry.__file_offset__ + x.entry.__field_offsets__['PdbFileName'] + len(chars))}", "cyan"))
                print(colored(f"\t\t==> Setting PdbFifleName stomped hex value to: {clean_chars}", "cyan"))
                filehandle.seek(x.entry.__file_offset__ + x.entry.__field_offsets__['PdbFileName'], 0)
                filehandle.write(clean_chars)
    filehandle.close()

with open(_file_name_live, 'r+b') as f:
    live_raw = f.read()
print("==============================================")
print("|-*          RUNTIME SANITY CHECKS         *-|")
print("==============================================")
if not hashlib.sha256(live_raw).hexdigest() == hashlib.sha256(raw).hexdigest():
    print(f"[*] SHA256 do not match, we have proper write: {colored('PASS', 'green')}")
else:
    print(f"[*] SHA256 MATCH, we DONT have proper write: {colored('FAIL', 'red')}")
if int(unpack('L', live_raw[ifh_tds:ifh_tds+0x8])[0]) == _file_compile_time:
    print(f"[*] TimeDateStamp stomped properly: {colored('PASS', 'green')}")
else: 
    print(f"[*] TimeDateStamp stomped: {colored('FAIL', 'red')}")
# test case for major linker version
if int(unpack('B', live_raw[pe.OPTIONAL_HEADER.__file_offset__+pe.OPTIONAL_HEADER.__field_offsets__['MajorLinkerVersion']:pe.OPTIONAL_HEADER.__file_offset__+pe.OPTIONAL_HEADER.__field_offsets__['MajorLinkerVersion']+0x1])[0]) == _file_major_linker_version:
    print(f"[*] MajorLinkerVersion stomped properly: {colored('PASS', 'green')}")
else:
    print(f"[*] MajorLinkerVersion stomped properly: {colored('FAIL', 'red')}")
# test to make sure debug dir are solid
for x in pe.DIRECTORY_ENTRY_DEBUG:
    if unpack('L', live_raw[x.struct.get_field_absolute_offset('TimeDateStamp'):x.struct.get_field_absolute_offset('TimeDateStamp')+0x8])[0] == _file_compile_time:
        print(f"[*] TimeDateStamp stomped properly for {debug_types[x.struct.Type]}: {colored('PASS', 'green')}")

pe = pefile.PE(_file_name_live)
ex = exif.get_json(_file_name_live)[0]
print("==============================================")
print("|-*         COOKED PAYLOAD METADATA        *-|")
print("==============================================")
print(f"[*] Filename of cooked payload: {colored(_file_name_live, 'green')}")
print(f"[*] MD5 of cooked payload: {colored(hashlib.md5(live_raw).hexdigest(), 'green')}")
print(f"[*] SHA1 of cooked payload: {colored(hashlib.sha1(live_raw).hexdigest(), 'green')}")
print(f"[*] SHA256 of cooked payload: {colored(hashlib.sha256(live_raw).hexdigest(), 'green')}")
print(f"[*] SHA512 of cooked payload: {colored(hashlib.sha512(live_raw).hexdigest(), 'green')}")
print(f"[*] Magic of cooked payload: {colored(magic.from_file(_file_name_live), 'green')}")
print(f"[*] Imphash of cooked payload: {colored(pe.get_imphash(), 'green')}")
print(f"[*] SSDeep of cooked payload: {colored(ssdeep.hash(live_raw), 'green')}")
print(f"[*] EXIF Data follows of cooked payload: {colored(ssdeep.hash(live_raw), 'green')}")
for x in ex:
    print(f"\t{x}: { colored(ex[x],'green')}")
print("==============================================")
print("|-*           RUNTIME BURNT CHECKS         *-|")
print("==============================================")
vt = VirusTotalPublicApi(API_KEY)
print(f"[*] Starting checks VirusTotal HASH ONLY checks")
# check non-cooked payload
while True:
    response = vt.get_file_report(hashlib.sha256(raw).hexdigest())
    if response['response_code'] == 204:
            time.sleep(10)
            continue
    if response['results']['response_code'] == 0:
        # we are safe bin has not been seen in wild
        print(f" - SHA256 of non-cooked payload is SAFE and NOT SEEN in VirusTotal: {colored(hashlib.sha256(raw).hexdigest(), 'green')}")
    if response['results']['response_code'] == 1:
        # TODO: warning for seen and alert for flagged
        print(f" - SHA256 of non-cooked payload is SAFE and SEEN in VirusTotal: {colored(hashlib.sha256(raw).hexdigest(), 'yellow')}")
    if response['results']['response_code'] and response['results']['positives']:
        # TODO: warning for seen and alert for flagged
        print(f" - SHA256 of non-cooked payload is NOT-SAFE and SEEN in VirusTotal: {colored(hashlib.sha256(raw).hexdigest(), 'red')}")
    # check new live payload
    break
while True:
    response = vt.get_file_report(hashlib.sha256(live_raw).hexdigest())
    if response['response_code'] == 204:
            time.sleep(10)
            continue
    if response['results']['response_code'] == 0:
        # we are safe bin has not been seen in wild
        print(f" - SHA256 of cooked payload is SAFE and NOT SEEN in VirusTotal: {colored(hashlib.sha256(live_raw).hexdigest(), 'green')}")
    if response['results']['response_code'] == 1:
        # TODO: warning for seen and alert for flagged
        print(f" - SHA256 of cooked payload is SAFE and SEEN in VirusTotal: {colored(hashlib.sha256(live_raw).hexdigest(), 'yellow')}")
    if response['results']['response_code'] and response['results']['positives']:
        # TODO: warning for seen and alert for flagged
        print(f" - SHA256 of cooked payload is NOT-SAFE and SEEN in VirusTotal: {colored(hashlib.sha256(live_raw).hexdigest(), 'red')}")
    # check of non-cooked payload sections (PE Sections)
    break

for x in pe.sections:
    while True:
        name = x.Name.rstrip(b'\x00').decode("utf-8")
        sha256 = x.get_hash_sha256()
        response = vt.get_file_report(sha256)
        if response['response_code'] == 204:
            time.sleep(10)
            continue
        if response['results']['response_code'] == 0:
            print(f" - SHA256 PE Section {name} of non-cooked payload is SAFE and NOT SEEN in VirusTotal: {colored(sha256, 'green')}")
        if response['results']['response_code'] == 1:
            print(f" - SHA256 PE Section {name} of non-cooked payload is SAFE and SEEN in VirusTotal: {colored(sha256, 'yellow')}")
        if response['results']['response_code'] and response['results']['positives']:
            print(f" - SHA256 PE Section {name} of non-cooked payload is NOT-SAFE and SEEN in VirusTotal: {colored(sha256, 'red')}")
        break

