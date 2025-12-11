namespace Runewire.Core.Domain.Recipes;

/// <summary>
/// How a recipe selects its target process.
/// </summary>
public enum RecipeTargetKind
{
    Self = 0,
    ProcessById = 1,
    ProcessByName = 2,
}
