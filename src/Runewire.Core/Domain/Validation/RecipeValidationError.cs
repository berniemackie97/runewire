namespace Runewire.Core.Domain.Validation
{
    /// <summary>
    /// Single recipe validation error.
    /// </summary>
    public sealed record RecipeValidationError(string Code, string Message);
}
