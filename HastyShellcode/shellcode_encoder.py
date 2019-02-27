from Crypto.Hash import MD5
from Crypto.Cipher import AES
import argparse
import pyscrypt
from os import urandom
from string import Template
import os

def color(string, color=None):
    """f
        Author: HarmJ0y, borrowed from Empire
        Change text color for the Linux terminal.
    """
    
    attr = []
    # bold
    attr.append('1')
    
    if color:
        if color.lower() == "red":
            attr.append('31')
        elif color.lower() == "green":
            attr.append('32')
        elif color.lower() == "blue":
            attr.append('34')
        return '\x1b[%sm%s\x1b[0m' % (';'.join(attr), string)
    
    else:
        if string.strip().startswith("[!]"):
            attr.append('31')
            return '\x1b[%sm%s\x1b[0m' % (';'.join(attr), string)
        elif string.strip().startswith("[+]"):
            attr.append('32')
            return '\x1b[%sm%s\x1b[0m' % (';'.join(attr), string)
        elif string.strip().startswith("[?]"):
            attr.append('33')
            return '\x1b[%sm%s\x1b[0m' % (';'.join(attr), string)
        elif string.strip().startswith("[*]"):
            attr.append('34')
            return '\x1b[%sm%s\x1b[0m' % (';'.join(attr), string)
        else:
            return string
#------------------------------------------------------------------------
# data as a bytearray
# key as a string
def xor(data, key):
    l = len(key)
    keyAsInt = map(ord, key)
    return bytes(bytearray((
                            (data[i] ^ keyAsInt[i % l]) for i in range(0,len(data))
                            )))

#------------------------------------------------------------------------
# data as a bytearray
def formatCPP(data, key, cipherType):
    shellcode = "\\x"
    shellcode += "\\x".join(format(ord(b),'02x') for b in data)
    result = convertFromTemplate({'shellcode': shellcode, 'key': key, 'cipherType': cipherType}, templates['cpp'])
    
    if result != None:
        try:
            fileName = os.path.splitext(resultFiles['cpp'])[0] + "_" + cipherType + os.path.splitext(resultFiles['cpp'])[1]
            with open(fileName,"w+") as f:
                f.write(result)
                f.close()
                print color("[+] C++ code file saved in [{}]".format(fileName))
        except IOError:
            print color("[!] Could not write C++ code  [{}]".format(fileName))

#======================================================================================================
#                                            MAIN FUNCTION
#======================================================================================================
if __name__ == '__main__':
    #------------------------------------------------------------------------
    # Parse arguments
    parser = argparse.ArgumentParser()
    parser.add_argument("shellcodeFile", help="File name containing the raw shellcode to be encoded/encrypted")
    parser.add_argument("key", help="Key used to transform (XOR or AES encryption) the shellcode")
    parser.add_argument("encryptionType", help="Encryption algorithm to apply to the shellcode", choices=['xor','aes'])
    parser.add_argument("-b64", "--base64", help="Display transformed shellcode as base64 encoded string", action="store_true")
    parser.add_argument("-cpp", "--cplusplus", help="Generates C++ file code", action="store_true")
    parser.add_argument("-cs", "--csharp", help="Generates C# file code", action="store_true")
    parser.add_argument("-py", "--python", help="Generates Python file code", action="store_true")
    args = parser.parse_args()
    
    #------------------------------------------------------------------------------
    # Check that required directories and path are available, if not create them
    if not os.path.isdir("./result"):
        os.makedirs("./result")
        print color("[+] Creating [./result] directory for resulting code files")
    
    #------------------------------------------------------------------------
    # Open shellcode file and read all bytes from it
    try:
        with open(args.shellcodeFile) as shellcodeFileHandle:
            shellcodeBytes = bytearray(shellcodeFileHandle.read())
            shellcodeFileHandle.close()
            print color("[*] Shellcode file [{}] successfully loaded".format(args.shellcodeFile))
    except IOError:
        print color("[!] Could not open or read file [{}]".format(args.shellcodeFile))
        quit()
    
    print color("[*] MD5 hash of the initial shellcode: [{}]".format(MD5.new(shellcodeBytes).hexdigest()))
    print color("[*] Shellcode size: [{}] bytes".format(len(shellcodeBytes)))

    #------------------------------------------------------------------------
    # Perform XOR transformation
    if args.encryptionType == 'xor':
        masterKey = args.key
        print color("[*] XOR encoding the shellcode with key [{}]".format(masterKey))
        transformedShellcode = xor(shellcodeBytes, masterKey)
        y = [int(str(x).encode('hex'), 16) for x in transformedShellcode]
        a = ','.join(str(x) for x in y)
        print(a)
        with open('results.txt', 'w') as f:
            f.write(a)

    #---------------------------- --------------------------------------------
    # Display interim results
    print "\n==================================== RESULT ====================================\n"
    print color("[*] Encrypted shellcode size: [{}] bytes".format(len(transformedShellcode)))
    #------------------------------------------------------------------------
    # Display formated output
    if args.base64:
        print color("[*] Transformed shellcode as a base64 encoded string")
        print formatB64(transformedShellcode)
        print ""

    if args.cplusplus:
        print color("[*] Generating C++ code file")
        formatCPP(transformedShellcode, masterKey, cipherType)
        print ""


    if args.csharp:
        print color("[*] Generating C# code file")
        formatCSharp(transformedShellcode, masterKey, cipherType)
        print ""
        
        if args.python:
            print color("[*] Generating Python code file")
            formatPy(transformedShellcode, masterKey, cipherType)
        print ""

