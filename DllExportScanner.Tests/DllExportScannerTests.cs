using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace DllExportScanner.Tests
{
    public class Tests
    {
        private const string _nativeLibraryName = "DllExportScanner.Tests.Native";
        private string _nativeFileName;
        private string _nativeFilePath;

        [SetUp]
        public void Setup()
        {
            //Platform specific
            (_nativeFileName, _nativeFilePath) = RuntimeInformation.OSDescription switch
            {
                _ when RuntimeInformation.IsOSPlatform(OSPlatform.Windows) => ($"{_nativeLibraryName}.dll", "../../../../DllExportScanner.Tests.Native/out/install/bin/"),
                _ when RuntimeInformation.IsOSPlatform(OSPlatform.Linux) => ($"lib{_nativeLibraryName}.so", "../../../../DllExportScanner.Tests.Native/out/install/lib/"),
                _ => throw new PlatformNotSupportedException()
            };
     
            _nativeFilePath = Path.GetFullPath(_nativeFilePath);
            if (!Directory.Exists(_nativeFilePath))
                throw new DirectoryNotFoundException("Build the native binaries before running the tests.");
        }

        [Test]
        public void ShouldGetScanner()
        {
            var scanner = DllExportScannerFactory.GetScanner();

            Assert.That(scanner, Is.Not.Null);
        }

        [Test]
        public void ShouldListExports()
        {
            var scanner = DllExportScannerFactory.GetScanner();
            Assert.That(scanner, Is.Not.Null);

            var exports = scanner.ListExports(_nativeFileName, _nativeFilePath);

            exports = exports.Where(x => x.FunctionSignature.Contains("Test_Export")).ToList();

            Assert.That(exports, Is.Not.Empty);

            Assert.That(exports.Any(e => e.FunctionSignature == "Test_Export01" && e.LibraryName == _nativeLibraryName), Is.True);
            Assert.That(exports.Count(e => e.FunctionSignature.Contains("Test_Export02") && e.FunctionSignature.Contains("TEST_CPP_NAMESPACE") && e.LibraryName == _nativeLibraryName), Is.EqualTo(2));

            //Common tests
            foreach (var export in exports)
            {
                Assert.That(export.FunctionSignature, Is.Not.Null.And.Not.Empty);
                Assert.That(export.LibraryName, Is.Not.Null.And.Not.Empty);
                Assert.That(export.LibraryVersion, Is.Not.Null.And.Not.Empty);
            }

           
        }
    }
}