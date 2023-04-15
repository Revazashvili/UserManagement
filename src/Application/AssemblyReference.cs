using System;
using System.Reflection;

namespace Application;

internal static class AssemblyReference
{
    private static readonly Lazy<Assembly> LazyAssembly =
        new(Assembly.GetExecutingAssembly());
    internal static Assembly Assembly => LazyAssembly.Value;
}