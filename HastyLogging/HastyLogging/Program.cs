using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HastyLogging
{
    class Program
    {
        static public bool CheckCmdLineAudit()
        {
            /*
             * Registry Hive: HKEY_LOCAL_MACHINE 
             * Registry Path: \SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit\
             * Value Name: ProcessCreationIncludeCmdLine_Enabled
             * Value Type: REG_DWORD
             * Value: 1
             */
            RegistryKey reg = Registry.LocalMachine;
            RegistryKey subKey = reg.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System\\Audit");
            if (subKey != null)
            {
                object val = subKey.GetValue("ProcessCreationIncludeCmdLine_Enabled");
                // Make sure we have a val before we access it
                if (val != null)
                {
                    // Check if ProcessCreationIncludeCmdLine_Enabled is 1
                    if (Convert.ToInt32(val) == 1)
                    {
                        // we have a postive hit for cli auditing
                        return true;
                    }
                }
            }
            return false;
        }
        static public bool CheckPowershellScriptBlockLogging()
        {
            /*
             * Registry Hive: HKEY_LOCAL_MACHINE 
             * Registry Path: \SOFTWARE\ Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging\
             * Value Name: EnableScriptBlockLogging
             * Value Type: REG_DWORD
             * Value: 1
             */
            RegistryKey reg = Registry.LocalMachine;
            RegistryKey subKey = reg.OpenSubKey("SOFTWARE\\Policies\\Microsoft\\Windows\\PowerShell\\ScriptBlockLogging");
            if (subKey != null)
            {
                object val = subKey.GetValue("EnableScriptBlockLogging");
                // Make sure we have a val before we access it
                if (val != null)
                {
                    // Check if ProcessCreationIncludeCmdLine_Enabled is 1
                    if (Convert.ToInt32(val) == 1)
                    {
                        // we have a postive hit for cli auditing
                        return true;
                    }
                }
            }
            return false;
        }
        static public bool CheckPowershellTranscriptLogging()
        {
            /*
             * Registry Hive	HKEY_LOCAL_MACHINE or HKEY_CURRENT_USER
             * Registry Path	Software\Policies\Microsoft\Windows\PowerShell\Transcription
             * Value Name	EnableTranscripting
             * Value Type	REG_DWORD
             * Enabled Value	1
             * Disabled Value	0
             */
            RegistryKey reg = Registry.LocalMachine;
            RegistryKey subKey = reg.OpenSubKey("SOFTWARE\\Policies\\Microsoft\\Windows\\PowerShell\\Transcription");
            if (subKey != null)
            {
                object val = subKey.GetValue("EnableTranscripting");
                // Make sure we have a val before we access it
                if (val != null)
                {
                    // Check if ProcessCreationIncludeCmdLine_Enabled is 1
                    if (Convert.ToInt32(val) == 1)
                    {
                        // we have a postive hit for cli auditing
                        return true;
                    }
                }
            }
            return false;
        }
        static public string CheckPowershellTranscriptLoggingLocation()
        {
            /*
             * Registry Hive	HKEY_LOCAL_MACHINE or HKEY_CURRENT_USER
             * Registry Path	Software\Policies\Microsoft\Windows\PowerShell\Transcription
             * Value Name	OutputDirectory
             * Value Type	REG_SZ
             * Default Value	
             */
            string retVal = "";
            RegistryKey reg = Registry.LocalMachine;
            RegistryKey subKey = reg.OpenSubKey("SOFTWARE\\Policies\\Microsoft\\Windows\\PowerShell\\Transcription");
            if (subKey != null)
            {
                object val = subKey.GetValue("OutputDirectory");
                // Make sure we have a val before we access it
                if (val != null)
                {
                    // Return Location.. if default its empty
                    return val.ToString();
                }
            }
            return retVal;
        }
        static public bool CheckPowershellModuleLogging()
        {
            /*
             * Registry Hive	HKEY_LOCAL_MACHINE or HKEY_CURRENT_USER
             * Registry Path	Software\Policies\Microsoft\Windows\PowerShell\ModuleLogging
             * Value Name	EnableModuleLogging
             * Value Type	REG_DWORD
             * Enabled Value	1
             * Disabled Value	0
             */
            RegistryKey reg = Registry.LocalMachine;
            RegistryKey subKey = reg.OpenSubKey("Software\\Policies\\Microsoft\\Windows\\PowerShell\\ModuleLogging");
            if (subKey != null)
            {
                object val = subKey.GetValue("EnableModuleLogging");
                // Make sure we have a val before we access it
                if (val != null)
                {
                    // Check if ProcessCreationIncludeCmdLine_Enabled is 1
                    if (Convert.ToInt32(val) == 1)
                    {
                        // we have a postive hit for cli auditing
                        return true;
                    }
                }
            }
            return false;
        }

        static void Main(string[] args)
        {
            string result = new String('-', 25);
            Console.WriteLine("*" + result + "HastyLogging Checks" + result + "*");
            Console.WriteLine("| PROCCESS CREATION AUDTING:");
            Console.Write("|\t" + "-> PROCCESS CREATION INCLUDES CMD LINE: ");
            if (CheckCmdLineAudit())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("TRUE");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("FALSE");
                Console.ResetColor();
            }
            // Powershell Checks
            Console.WriteLine("| POWERSHELL AUDTING:");
            Console.Write("|\t" + "-> POWERSHELL MODULE LOGGING: ");
            if (CheckPowershellModuleLogging())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("TRUE");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("FALSE");
                Console.ResetColor();
            }
            Console.Write("|\t" + "-> POWERSHELL SCRIPT BLOCK LOGGING: ");
            if (CheckPowershellScriptBlockLogging())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("TRUE");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("FALSE");
                Console.ResetColor();
            }
            Console.Write("|\t" + "-> POWERSHELL TRANSSCRIPT LOGGING: ");
            if (CheckPowershellTranscriptLogging())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("TRUE");
                Console.ResetColor();
                Console.WriteLine("|\t\t" + "-> POWERSHELL TRANSSCRIPT LOGGING LOCATION: {0}", CheckPowershellTranscriptLoggingLocation());
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("FALSE");
                Console.ResetColor();
            }
            Console.WriteLine("*" + result + "-------------------" + result + "*");

        }
    }
}
