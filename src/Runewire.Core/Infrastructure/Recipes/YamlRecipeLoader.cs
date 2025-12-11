using Runewire.Core.Domain.Recipes;
using Runewire.Core.Domain.Validation;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Runewire.Core.Infrastructure.Recipes;

/// <summary>
/// Concrete loader that parses YAML into <see cref="RunewireRecipe"/> and
/// validates it using an <see cref="IRecipeValidator"/>.
/// </summary>
public sealed class YamlRecipeLoader(IRecipeValidator validator) : IYamlRecipeLoader
{
    private readonly IDeserializer _deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).IgnoreUnmatchedProperties().Build();
    private readonly IRecipeValidator _validator = validator ?? throw new ArgumentNullException(nameof(validator));

    public RunewireRecipe LoadFromString(string yaml)
    {
        ArgumentNullException.ThrowIfNull(yaml);

        RecipeDocument? document;

        try
        {
            document = _deserializer.Deserialize<RecipeDocument>(yaml);
        }
        catch (Exception ex)
        {
            throw new RecipeLoadException("Failed to parse recipe YAML.", null, ex);
        }

        if (document is null)
        {
            throw new RecipeLoadException("Recipe YAML content is empty or invalid.");
        }

        RunewireRecipe recipe = MapToDomain(document);

        RecipeValidationResult validationResult = _validator.Validate(recipe);
        if (!validationResult.IsValid)
        {
            throw new RecipeLoadException("Recipe failed validation.", validationResult.Errors);
        }

        return recipe;
    }

    public RunewireRecipe LoadFromFile(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Path must not be null or whitespace.", nameof(path));
        }

        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Recipe file not found.", path);
        }

        string yaml;
        try
        {
            yaml = File.ReadAllText(path);
        }
        catch (Exception ex)
        {
            throw new RecipeLoadException($"Failed to read recipe file '{path}'.", null, ex);
        }

        return LoadFromString(yaml);
    }

    private static RunewireRecipe MapToDomain(RecipeDocument doc)
    {
        if (doc.Target is null)
        {
            throw new RecipeLoadException("Recipe 'target' section is required.");
        }

        if (doc.Technique is null)
        {
            throw new RecipeLoadException("Recipe 'technique' section is required.");
        }

        if (doc.Payload is null)
        {
            throw new RecipeLoadException("Recipe 'payload' section is required.");
        }

        // No throws here for empty/invalid values—that's the validator's job.
        string name = doc.Name ?? string.Empty;
        string? description = doc.Description;

        string techniqueName = doc.Technique.Name ?? string.Empty;
        InjectionTechnique technique = new(techniqueName);

        RecipeTargetKind targetKind = ParseTargetKind(doc.Target.Kind);
        RecipeTarget target = targetKind switch
        {
            RecipeTargetKind.Self => RecipeTarget.Self(),
            RecipeTargetKind.ProcessById => RecipeTarget.ForProcessId(doc.Target.ProcessId ?? 0),
            RecipeTargetKind.ProcessByName => RecipeTarget.ForProcessName(doc.Target.ProcessName ?? string.Empty),
            _ => throw new RecipeLoadException($"Unsupported target kind '{doc.Target.Kind}'.")
        };

        string payloadPath = doc.Payload.Path ?? string.Empty;

        RecipeSafetyDocument safety = doc.Safety ?? new RecipeSafetyDocument();

        return new RunewireRecipe(name, description, target, technique, payloadPath, safety.RequireInteractiveConsent, safety.AllowKernelDrivers);
    }


    private static RecipeTargetKind ParseTargetKind(string? rawKind)
    {
        if (string.IsNullOrWhiteSpace(rawKind))
        {
            throw new RecipeLoadException("Recipe target 'kind' is required.");
        }

        string normalized = rawKind.Trim().ToLowerInvariant();
        return normalized switch
        {
            "self" => RecipeTargetKind.Self,
            "processbyid" => RecipeTargetKind.ProcessById,
            "process_id" => RecipeTargetKind.ProcessById,
            "processid" => RecipeTargetKind.ProcessById,
            "pid" => RecipeTargetKind.ProcessById,
            "processbyname" => RecipeTargetKind.ProcessByName,
            "process_name" => RecipeTargetKind.ProcessByName,
            "processname" => RecipeTargetKind.ProcessByName,
            "image" => RecipeTargetKind.ProcessByName,
            _ => throw new RecipeLoadException($"Unknown recipe target kind '{rawKind}'."),
        };
    }
}
