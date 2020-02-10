using System.Collections.Generic;
using System.Diagnostics;

namespace DesktopClient.Modules.ApplicationManager.ProcessEqualityComparer
{
    internal class ProcessEqualityComparer : IEqualityComparer<Process>
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