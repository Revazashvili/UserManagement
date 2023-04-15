using System.Reflection;

namespace Application;

internal static class AssemblyReference
{
    private static Assembly? _assembly;
    internal static Assembly Assembly
    {
        get
        {
            if(_assembly is not null)
                return _assembly;

            _assembly = Assembly.GetExecutingAssembly();
            return _assembly;
        }
    }
}