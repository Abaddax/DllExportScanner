using DllExportScanner.Contracts;
using DllExportScanner.Internal.Linux;
using DllExportScanner.Internal.Windows;
using System.Runtime.InteropServices;

namespace DllExportScanner
{
    public static class DllExportScannerFactory
    {
        private static IDllExportScanner? _scanner = null;
        public static IDllExportScanner GetScanner()
        {
            return _scanner ??= RuntimeInformation.OSDescription switch
            {
                _ when RuntimeInformation.IsOSPlatform(OSPlatform.Windows) => new DllExportScannerWindows(),
                _ when RuntimeInformation.IsOSPlatform(OSPlatform.Linux) => new DllExportScannerLinux(),
                _ => throw new PlatformNotSupportedException("Current operating system is not supported.")
            };
        }
    }
}
