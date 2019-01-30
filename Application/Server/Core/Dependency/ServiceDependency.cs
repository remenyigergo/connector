using System;

namespace Core.Dependency
{
    public static class ServiceDependency
    {
        public static IServiceProvider Current { get; set; }
    }
}