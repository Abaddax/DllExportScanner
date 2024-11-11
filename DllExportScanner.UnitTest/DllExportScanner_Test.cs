using System.Runtime.InteropServices;

namespace DllExportScanner.UnitTest
{
    [TestClass]
    public class DllExportScanner_Test
    {
        [TestMethod]
        public void T1_Factory_GetScanner()
        {
            var scanner = DllExportScannerFactory.GetScanner();

            Assert.IsNotNull(scanner);
        }

        [TestMethod]
        public void T2_Scanner_ListExports()
        {
            var scanner = DllExportScannerFactory.GetScanner();
            Assert.IsNotNull(scanner);

            List<FunctionExport> exports = null;

            //Platform specific test
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                exports = scanner.ListExports("dxgi.dll", @"C:\Windows\System32\");

                Assert.AreNotEqual(0, exports.Count);

                Assert.IsTrue(exports.Any(e => e.FunctionSignature == "CreateDXGIFactory" && e.LibraryName == "dxgi"));
                Assert.IsTrue(exports.Any(e => e.FunctionSignature == "CreateDXGIFactory1" && e.LibraryName == "dxgi"));
                Assert.IsTrue(exports.Any(e => e.FunctionSignature == "CreateDXGIFactory2" && e.LibraryName == "dxgi"));
            }
            else
            {
                Assert.Fail("Current operating system is not supported.");
            }

            //Common tests
            foreach (var export in exports)
            {
                Assert.IsFalse(string.IsNullOrEmpty(export.FunctionSignature));
                Assert.IsFalse(string.IsNullOrEmpty(export.LibraryName));
                Assert.IsFalse(string.IsNullOrEmpty(export.LibraryVersion));
            }
        }

    }
}