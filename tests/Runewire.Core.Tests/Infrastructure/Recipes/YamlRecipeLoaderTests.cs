using Runewire.Core.Domain.Recipes;
using Runewire.Core.Domain.Validation;
using Runewire.Core.Infrastructure.Recipes;

namespace Runewire.Core.Tests.Infrastructure.Recipes;

public class YamlRecipeLoaderTests
{
    private static YamlRecipeLoader CreateLoader() => new(new BasicRecipeValidator());

    [Fact]
    public void LoadFromString_parses_valid_yaml_and_validates()
    {
        // Setup
        const string yaml = """
            name: demo-recipe
            description: Demo injection into explorer
            target:
              kind: processByName
              processName: explorer.exe
            technique:
              name: CreateRemoteThread
            payload:
              path: C:\lab\payloads\demo.dll
            safety:
              requireInteractiveConsent: true
              allowKernelDrivers: false
            """;

        YamlRecipeLoader loader = CreateLoader();

        // Run
        RunewireRecipe recipe = loader.LoadFromString(yaml);

        // Assert
        Assert.Equal("demo-recipe", recipe.Name);
        Assert.Equal("Demo injection into explorer", recipe.Description);
        Assert.Equal(RecipeTargetKind.ProcessByName, recipe.Target.Kind);
        Assert.Equal("explorer.exe", recipe.Target.ProcessName);
        Assert.Equal("CreateRemoteThread", recipe.Technique.Name);
        Assert.Equal(@"C:\lab\payloads\demo.dll", recipe.PayloadPath);
        Assert.True(recipe.RequireInteractiveConsent);
        Assert.False(recipe.AllowKernelDrivers);
    }

    [Fact]
    public void LoadFromString_throws_on_invalid_yaml()
    {
        // Setup
        const string yaml = """
            name: demo-recipe
            target:
              kind: processByName
              processName:
                nested: thing   # invalid type for string
            """;

        YamlRecipeLoader loader = CreateLoader();

        // Run and assert
        RecipeLoadException ex = Assert.Throws<RecipeLoadException>(() => loader.LoadFromString(yaml));
        Assert.Contains("Failed to parse recipe YAML", ex.Message);
    }

    [Fact]
    public void LoadFromString_throws_on_missing_required_sections()
    {
        // Setup
        const string yaml = """
            name: demo-recipe
            # missing target, technique, payload
            """;

        YamlRecipeLoader loader = CreateLoader();

        // Run and assert
        RecipeLoadException ex = Assert.Throws<RecipeLoadException>(() => loader.LoadFromString(yaml));

        Assert.Contains("target", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void LoadFromString_throws_on_unknown_target_kind()
    {
        // Setup
        const string yaml = """
            name: demo-recipe
            target:
              kind: galacticCore
            technique:
              name: CreateRemoteThread
            payload:
              path: C:\lab\payloads\demo.dll
            """;

        YamlRecipeLoader loader = CreateLoader();

        // Run and assert
        RecipeLoadException ex = Assert.Throws<RecipeLoadException>(() => loader.LoadFromString(yaml));
        Assert.Contains("Unknown recipe target kind", ex.Message);
    }

    [Fact]
    public void LoadFromString_throws_with_validation_errors()
    {
        // Setup
        const string yaml = """
            name: ''
            target:
              kind: processById
              processId: 0
            technique:
              name: ''
            payload:
              path: ''
            safety:
              allowKernelDrivers: true
              requireInteractiveConsent: false
            """;

        YamlRecipeLoader loader = CreateLoader();

        // Run and assert
        RecipeLoadException ex = Assert.Throws<RecipeLoadException>(() => loader.LoadFromString(yaml));

        Assert.NotNull(ex.ValidationErrors);
        Assert.Contains(ex.ValidationErrors!, e => e.Code == "RECIPE_NAME_REQUIRED");
        Assert.Contains(ex.ValidationErrors!, e => e.Code == "TARGET_PID_INVALID");
        Assert.Contains(ex.ValidationErrors!, e => e.Code == "TECHNIQUE_NAME_REQUIRED");
        Assert.Contains(ex.ValidationErrors!, e => e.Code == "PAYLOAD_PATH_REQUIRED");
        Assert.Contains(ex.ValidationErrors!, e => e.Code == "SAFETY_KERNEL_DRIVER_CONSENT_REQUIRED");
    }
}
