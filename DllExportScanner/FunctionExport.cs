using System.Diagnostics;

namespace Abaddax.DllExportScanner
{
    [DebuggerDisplay("{FunctionSignature}, {LibraryName}-{LibraryVersion}")]
    public readonly record struct FunctionExport
    {
        public string FunctionSignature { get; init; }
        public string LibraryName { get; init; }
        public string? LibraryVersion { get; init; }
    }
}
