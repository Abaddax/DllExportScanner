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

                Assert.AreEqual(true, exports.Any(e => e.FunctionSignature == "CreateDXGIFactory"));
                Assert.AreEqual(true, exports.Any(e => e.FunctionSignature == "CreateDXGIFactory1"));
                Assert.AreEqual(true, exports.Any(e => e.FunctionSignature == "CreateDXGIFactory2"));
            }
            else
            {
                Assert.Fail("Current operating system is not supported.");
            }

            //Common tests
            foreach (var export in exports)
            {
                Assert.AreEqual(false, string.IsNullOrEmpty(export.FunctionSignature));
                Assert.AreEqual(false, string.IsNullOrEmpty(export.LibraryName));
                Assert.AreEqual(false, string.IsNullOrEmpty(export.LibraryVersion));
            }
        }

    }
}