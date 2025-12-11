using Runewire.Core.Domain.Recipes;

namespace Runewire.Orchestrator.Orchestration;

/// <summary>
/// Immutable description of a single injection operation.
/// </summary>
public sealed record InjectionRequest(
    string RecipeName,
    string? RecipeDescription,
    RecipeTarget Target,
    string TechniqueName,
    string PayloadPath,
    bool AllowKernelDrivers,
    bool RequireInteractiveConsent
);
