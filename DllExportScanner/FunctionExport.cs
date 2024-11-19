using System.Diagnostics;

namespace DllExportScanner
{
    [DebuggerDisplay("{FunctionSignature}, {LibraryName}-{LibraryVersion}")]
    public struct FunctionExport
    {
        public string FunctionSignature { get; init; }
        public string LibraryName { get; init; }
        public string? LibraryVersion { get; init; }
    }
}
