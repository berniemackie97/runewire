namespace Runewire.Core.Domain.Recipes;

/// <summary>
/// Description of a single injection experiment.
/// </summary>
public sealed record RunewireRecipe(
    string Name,
    string? Description,
    RecipeTarget Target,
    InjectionTechnique Technique,
    string PayloadPath,
    bool RequireInteractiveConsent,
    bool AllowKernelDrivers
);
