namespace DllExportScanner
{
    public interface IDllExportScanner
    {
        List<FunctionExport> ListExports(string binaryName, params string[] binaryDirs);
    }
}
