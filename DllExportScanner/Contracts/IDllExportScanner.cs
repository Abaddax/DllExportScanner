namespace Abaddax.DllExportScanner.Contracts
{
    public interface IDllExportScanner
    {
        List<FunctionExport> ListExports(string binaryName, params string[] binaryDirs);
    }
}
