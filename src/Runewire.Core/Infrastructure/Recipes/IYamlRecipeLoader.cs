using Runewire.Core.Domain.Recipes;

namespace Runewire.Core.Infrastructure.Recipes;

/// <summary>
/// Loads <see cref="RunewireRecipe"/> instances from YAML.
/// </summary>
public interface IYamlRecipeLoader
{
    /// <summary>
    /// Parse a recipe from a YAML string and validate it.
    /// </summary>
    RunewireRecipe LoadFromString(string yaml);

    /// <summary>
    /// Read a file from disk, parse the YAML, and validate it.
    /// </summary>
    RunewireRecipe LoadFromFile(string path);
}
