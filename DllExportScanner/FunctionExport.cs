using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
