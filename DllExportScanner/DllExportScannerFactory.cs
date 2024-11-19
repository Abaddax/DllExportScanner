using System.Runtime.InteropServices;

namespace DllExportScanner
{
    public static class DllExportScannerFactory
    {
        private static IDllExportScanner _scanner = null;
        public static IDllExportScanner GetScanner()
        {
            if (_scanner != null)
                return _scanner;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _scanner = new DllExportScannerWindows();
                return _scanner;
            }
            throw new NotSupportedException("Current operating system is not supported.");
        }
    }
}
