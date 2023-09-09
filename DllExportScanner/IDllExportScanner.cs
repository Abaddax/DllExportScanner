using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DllExportScanner
{
    public interface IDllExportScanner
    {
        List<FunctionExport> ListExports(string binaryName, params string[] binaryDirs);
    }
}
