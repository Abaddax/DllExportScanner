using Abaddax.DllExportScanner.Contracts;
using Abaddax.DllExportScanner.Internal.Linux;
using Abaddax.DllExportScanner.Internal.Windows;
using System.Runtime.InteropServices;

namespace Abaddax.DllExportScanner
{
    public static class DllExportScannerFactory
    {
        private static IDllExportScanner? _Scanner = null;
        public static IDllExportScanner GetScanner()
        {
            return _Scanner ??= RuntimeInformation.OSDescription switch
            {
                _ when RuntimeInformation.IsOSPlatform(OSPlatform.Windows) => new DllExportScannerWindows(),
                _ when RuntimeInformation.IsOSPlatform(OSPlatform.Linux) => new DllExportScannerLinux(),
                _ => throw new PlatformNotSupportedException("Current operating system is not supported.")
            };
        }
    }
}
