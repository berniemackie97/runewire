namespace Runewire.Core.Domain.Techniques;

/// <summary>
/// Read-only registry of known injection techniques.
/// 
/// The orchestrator, CLI, and GUI use this to:
///  - Enumerate techniques (e.g. for 'list-techniques').
///  - Validate that recipe technique names are known.
///  - Drive documentation / UX.
/// </summary>
public interface IInjectionTechniqueRegistry
{
    /// <summary>
    /// All known techniques in this registry.
    /// </summary>
    IEnumerable<InjectionTechniqueDescriptor> GetAll();

    /// <summary>
    /// Lookup a technique by its stable ID.
    /// </summary>
    InjectionTechniqueDescriptor? GetById(InjectionTechniqueId id);

    /// <summary>
    /// Lookup a technique by its canonical name case insensitive.
    /// </summary>
    InjectionTechniqueDescriptor? GetByName(string name);
}
