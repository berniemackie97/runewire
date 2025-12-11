using Runewire.Core.Domain.Validation;

namespace Runewire.Core.Infrastructure.Recipes;

/// <summary>
/// Represents a failure to load a recipe from a serialized source.
/// </summary>
public sealed class RecipeLoadException(string message, IReadOnlyList<RecipeValidationError>? validationErrors = null, Exception? innerException = null) : Exception(message, innerException)
{
    /// <summary>
    /// Validation errors associated with this failure.
    /// Empty when the failure was due to parsing, I/O, etc.
    /// </summary>
    public IReadOnlyList<RecipeValidationError> ValidationErrors { get; } = validationErrors ?? [];
}
