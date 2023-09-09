using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static DllExportScanner.Windows.DbgHelp;

namespace DllExportScanner
{
    internal class DllExportScannerWindows : IDllExportScanner
    {
        public List<FunctionExport> ListExports(string binaryName, params string[] binaryDirs)
        {
            List<string> binaryPaths = new();
            foreach (var binaryDir in binaryDirs)
            {
                var results = Directory.EnumerateFiles(binaryDir, binaryName);
                foreach (var result in results)
                {
                    binaryPaths.Add(result);
                }
            }

            List<FunctionExport> exports = new();
            foreach (var binaryPath in binaryPaths)
            {
                var fullName = Path.GetFileNameWithoutExtension(binaryPath);
                var nameParts = fullName.Split('-');
                var name = fullName;
                var versionInfo = FileVersionInfo.GetVersionInfo(binaryPath);
                var version = $"{versionInfo.FileMajorPart}.{versionInfo.FileMinorPart}.{versionInfo.FileBuildPart}.{versionInfo.FilePrivatePart}";
                var _exports = GetExports(binaryPath);
                foreach (var export in _exports)
                {
                    exports.Add(new FunctionExport()
                    {
                        FunctionSignature = export,
                        LibraryName = name,
                        LibraryVersion = version,
                    });
                };
            }
            return exports;
        }

        private static IEnumerable<string> GetExports(string libraryPath)
        {
            Process process = Process.GetCurrentProcess();

            //Set options to use decorated symbols
            var status_ = SymSetOptions(SymOpt.PUBLICS_ONLY);

            var status = SymInitialize(process.Handle,
                null,
                false);
            if (!status)
                throw new Exception("SymInitialize failed.");

            try
            {
                var baseOfDll = SymLoadModuleEx(process.Handle,
                    IntPtr.Zero,
                    libraryPath,
                    null,
                    IntPtr.Zero,
                    0,
                    IntPtr.Zero,
                    0);
                if (baseOfDll == 0)
                    throw new Exception($"SymLoadModuleEx failed for {libraryPath}.");

                var exports = new List<string>();                

                //Callback
                bool EnumSymProc(ref SYMBOL_INFO pSymInfo, UInt32 SymbolSize, IntPtr UserContext)
                {
                    //Console.WriteLine(pSymInfo.Address + " " + SymbolSize + " " + pSymInfo.Name);
                    exports.Add(pSymInfo.Name);
                    /*if (pSymInfo.Name.Contains("Test"))
                        ;
                    SymFlag exported = pSymInfo.Flags;*/

                    return true;
                }

                status = SymEnumSymbols(process.Handle,
                    baseOfDll,
                    "*",
                    EnumSymProc,
                    IntPtr.Zero);
                if (!status)
                    throw new Exception($"SymEnumSymbols failed. {Marshal.GetLastWin32Error()}");

                return exports;
            }
            finally
            {
                SymCleanup(process.Handle);
            }
        }
    }
}
