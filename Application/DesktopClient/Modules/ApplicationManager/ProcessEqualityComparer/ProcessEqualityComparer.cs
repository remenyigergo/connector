using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Modules.ApplicationManager.ProcessEqualityComparer
{
    class ProcessEqualityComparer : IEqualityComparer<Process>
    {
        public bool Equals(Process x, Process y)
        {
            return x.MainModule.ModuleName == y.MainModule.ModuleName;
        }

        public int GetHashCode(Process obj)
        {
            return obj.MainModule.ModuleName.GetHashCode();
        }
    }
}
