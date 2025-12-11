using System.Runtime.InteropServices;

namespace Runewire.Orchestrator.NativeInterop;

/// <summary>
/// P/Invoke declarations for Runewire.Injector.dll.
/// </summary>
internal static class NativeMethods
{
    private const string DllName = "Runewire.Injector.dll";

    /// <summary>
    /// Managed declaration of the rw_inject entrypoint.
    /// This must match the C API defined in runewire_injector.h.
    /// </summary>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "rw_inject", ExactSpelling = true)]
    internal static extern int RwInject(in RwInjectionRequest request, out RwInjectionResult result);
}
