using System;

namespace Standard.Core.Dependency
{
    public static class ServiceDependency
    {
        public static IServiceProvider Current { get; set; }
    }
}