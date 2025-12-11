namespace Runewire.Core.Domain.Validation;

using Runewire.Core.Domain.Recipes;

public interface IRecipeValidator
{
    RecipeValidationResult Validate(RunewireRecipe recipe);
}
