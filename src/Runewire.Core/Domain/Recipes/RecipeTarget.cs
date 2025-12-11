namespace Runewire.Core.Domain.Recipes;

/// <summary>
/// Immutable description of a target for an injection recipe.
/// </summary>
public sealed record RecipeTarget
{
    public RecipeTargetKind Kind { get; init; }

    public int? ProcessId { get; init; }

    public string? ProcessName { get; init; }

    private RecipeTarget(RecipeTargetKind kind, int? processId, string? processName)
    {
        Kind = kind;
        ProcessId = processId;
        ProcessName = processName;
    }

    public static RecipeTarget Self() => new(RecipeTargetKind.Self, null, null);

    public static RecipeTarget ForProcessId(int processId) => new(RecipeTargetKind.ProcessById, processId, null);

    public static RecipeTarget ForProcessName(string processName) => new(RecipeTargetKind.ProcessByName, null, processName);
}
