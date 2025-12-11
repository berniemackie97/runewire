namespace Runewire.Core.Domain.Techniques;

/// <summary>
/// Describes a single injection technique supported by Runewire.
/// This is domain metadata only – no implementation details here.
/// </summary>
public sealed class InjectionTechniqueDescriptor
{
    public InjectionTechniqueDescriptor(InjectionTechniqueId id, string name, string displayName, string category, string description, bool requiresKernelMode)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Technique name is required.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new ArgumentException("Display name is required.", nameof(displayName));
        }

        if (string.IsNullOrWhiteSpace(category))
        {
            throw new ArgumentException("Category is required.", nameof(category));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Description is required.", nameof(description));
        }

        Id = id;
        Name = name;
        DisplayName = displayName;
        Category = category;
        Description = description;
        RequiresKernelMode = requiresKernelMode;
    }

    /// <summary>
    /// Stable ID for this built in technique.
    /// </summary>
    public InjectionTechniqueId Id { get; }

    /// <summary>
    /// Canonical technique name, as referenced in recipes.
    /// Example: "CreateRemoteThread".
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Human-friendly display name for UIs / docs.
    /// </summary>
    public string DisplayName { get; }

    /// <summary>
    /// Logical category of the technique.
    /// </summary>
    public string Category { get; }

    /// <summary>
    /// Short description of what this technique does at a high level.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Indicates that this technique requires kernel-mode capabilities
    /// </summary>
    public bool RequiresKernelMode { get; }
}
