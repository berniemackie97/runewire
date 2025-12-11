namespace Runewire.Core.Domain.Techniques;

/// <summary>
/// Stable identifier for a built in injection technique.
///
/// These IDs are intended to be durable over time and safe
/// to reference from recipes, docs, and GUIs.
/// </summary>
public enum InjectionTechniqueId
{
    /// <summary>
    /// Unknown or custom technique. Generally not used for
    /// built in techniques, but reserved for future extensibility.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Classic Windows DLL injection using CreateRemoteThread
    /// </summary>
    CreateRemoteThread = 1,

    // QueueUserAPC = 2,
    // NtCreateThreadEx = 3,
    // KernelDriverLoad = 10,
}
