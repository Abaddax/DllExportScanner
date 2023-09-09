using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
