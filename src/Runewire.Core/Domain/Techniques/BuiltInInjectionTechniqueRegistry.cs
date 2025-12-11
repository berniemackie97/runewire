using System.Collections.Immutable;

namespace Runewire.Core.Domain.Techniques;

/// <summary>
/// In-memory registry of built-in injection techniques.
///
/// This is intentionally simple and deterministic:
///  - The set is fixed at construction time.
///  - Lookups are case-insensitive on the 'Name' field.
/// </summary>
public sealed class BuiltInInjectionTechniqueRegistry : IInjectionTechniqueRegistry
{
    private readonly ImmutableDictionary<InjectionTechniqueId, InjectionTechniqueDescriptor> _byId;
    private readonly ImmutableDictionary<string, InjectionTechniqueDescriptor> _byName;

    public BuiltInInjectionTechniqueRegistry()
    {
        // For now we just seed a couple of techniques. We will
        // expand this list as the native injector grows.
        InjectionTechniqueDescriptor[] techniques =
        [
            new InjectionTechniqueDescriptor(
                id: InjectionTechniqueId.CreateRemoteThread,
                name: "CreateRemoteThread",
                displayName: "CreateRemoteThread DLL Injection",
                category: "User-mode DLL injection",
                description: "Injects a DLL into a target process and starts execution using CreateRemoteThread (or equivalent).",
                requiresKernelMode: false
            ),

            // new InjectionTechniqueDescriptor(
            //     InjectionTechniqueId.QueueUserApc,
            //     "QueueUserAPC",
            //     "QueueUserAPC DLL Injection",
            //     "User-mode DLL injection",
            //     "Injects a DLL and schedules its execution via QueueUserAPC.",
            //     requiresKernelMode: false),
        ];

        _byId = techniques.ToImmutableDictionary(t => t.Id);
        _byName = techniques.ToImmutableDictionary(t => t.Name, t => t, StringComparer.OrdinalIgnoreCase);
    }

    public IEnumerable<InjectionTechniqueDescriptor> GetAll() => _byId.Values;

    public InjectionTechniqueDescriptor? GetById(InjectionTechniqueId id)
    {
        return _byId.TryGetValue(id, out InjectionTechniqueDescriptor? descriptor) ? descriptor : null;
    }

    public InjectionTechniqueDescriptor? GetByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        return _byName.TryGetValue(name, out InjectionTechniqueDescriptor? descriptor) ? descriptor : null;
    }
}
