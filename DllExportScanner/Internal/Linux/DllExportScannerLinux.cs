using DllExportScanner.Contracts;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace DllExportScanner.Internal.Linux
{
    [SupportedOSPlatform("linux")]
    internal sealed class DllExportScannerLinux : IDllExportScanner
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
                }
                ;
            }
            return exports;
        }

        private static IEnumerable<string> GetExports(string libraryPath)
        {
            string output;
            using (var process = new Process())
            {
                var startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.FileName = "nm";
                startInfo.Arguments = $"--extern-only --defined-only --dynamic {libraryPath}";

                process.StartInfo = startInfo;
                process.Start();

                process.WaitForExit(TimeSpan.FromSeconds(5));

                output = process.StandardOutput.ReadToEnd();
                string errors = process.StandardError.ReadToEnd();


                if (process.ExitCode != 0)
                    throw new Exception("Command failed with exit code: " + process.ExitCode, new Exception(errors));
            }

            using (var stringReader = new StringReader(output))
            {
                string? line;
                while ((line = stringReader.ReadLine()) != null)
                {
                    //Line: 00000000000010f9 T Test_Export01
                    var parts = line.Split(' ');
                    if (parts.Length != 3)
                        throw new Exception($"Unexpected output: {line}");
                    yield return parts[2];
                }
            }
        }
    }
}
