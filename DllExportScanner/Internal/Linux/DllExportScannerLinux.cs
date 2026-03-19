using Abaddax.DllExportScanner.Contracts;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace Abaddax.DllExportScanner.Internal.Linux
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

            List<FunctionExport> allExports = new();
            foreach (var binaryPath in binaryPaths)
            {
                var fullName = Path.GetFileNameWithoutExtension(binaryPath);
                var nameParts = fullName.Split('-');
                var name = fullName;
                var versionInfo = FileVersionInfo.GetVersionInfo(binaryPath);
                var version = $"{versionInfo.FileMajorPart}.{versionInfo.FileMinorPart}.{versionInfo.FileBuildPart}.{versionInfo.FilePrivatePart}";
                var exports = GetExports(binaryPath);
                foreach (var export in exports)
                {
                    allExports.Add(new FunctionExport()
                    {
                        FunctionSignature = export,
                        LibraryName = name,
                        LibraryVersion = version,
                    });
                }
                ;
            }
            return allExports;
        }

        private static IEnumerable<string> GetExports(string libraryPath)
        {
            string output;
            using (var process = new Process())
            {
                var startInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    FileName = "nm",
                    Arguments = $"--extern-only --defined-only --dynamic {libraryPath}"
                };

                try
                {

                    process.StartInfo = startInfo;
                    process.Start();
                }
                catch (Exception)
                {
                    Console.Error.WriteLine($"Failed to start '{startInfo.FileName} {startInfo.Arguments}'");
                    Console.Error.WriteLine("Please make sure 'binutils'/'nm' is installed.");
                    throw;
                }

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
