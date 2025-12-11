namespace Runewire.Core.Domain.Validation;

/// <summary>
/// Result of validating a recipe.
/// </summary>
public sealed record RecipeValidationResult(bool IsValid, IReadOnlyList<RecipeValidationError> Errors)
{
    public static RecipeValidationResult Success() => new(true, []);

    public static RecipeValidationResult Failure(IEnumerable<RecipeValidationError> errors)
    {
        List<RecipeValidationError> list = errors?.ToList() ?? [];
        return new(false, list);
    }
}
