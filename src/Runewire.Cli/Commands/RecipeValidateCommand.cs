using System.CommandLine;
using Runewire.Core.Domain.Recipes;
using Runewire.Core.Domain.Validation;
using Runewire.Core.Infrastructure.Recipes;

namespace Runewire.Cli.Commands;

/// <summary>
/// CLI command for validating recipe YAML files.
/// </summary>
public static class RecipeValidateCommand
{
    public const string CommandName = "validate";

    /// <summary>
    /// Creates the 'validate' command:
    ///   runewire validate <recipe.yaml>
    /// </summary>
    public static Command Create()
    {
        Argument<FileInfo> recipeArgument = new("recipe")
        {
            Description = "Path to the recipe YAML file to validate."
        };

        Command command = new(CommandName, description: "Validate a Runewire recipe YAML file.")
        {
            recipeArgument
        };

        command.SetAction(parseResult =>
        {
            FileInfo? recipeFile = parseResult.GetValue(recipeArgument);

            if (recipeFile is null)
            {
                WriteError("No recipe file specified.");
                return 2;
            }

            return Handle(recipeFile);
        });

        return command;
    }

    /// <summary>
    /// Core handler logic for validation. Returns an exit code:
    /// 0 = valid
    /// 1 = validation errors (semantic)
    /// 2 = load/structural/other error
    /// </summary>
    private static int Handle(FileInfo recipeFile)
    {
        if (!recipeFile.Exists)
        {
            WriteError($"Recipe file not found: {recipeFile.FullName}");
            return 2;
        }

        BasicRecipeValidator validator = new();
        YamlRecipeLoader loader = new(validator);

        try
        {
            RunewireRecipe recipe = loader.LoadFromFile(recipeFile.FullName);

            WriteSuccess($"Recipe is valid: {recipe.Name}");
            return 0;
        }
        catch (RecipeLoadException ex)
        {
            if (ex.ValidationErrors.Count > 0)
            {
                WriteHeader("Recipe is invalid.", ConsoleColor.Yellow);
                foreach (RecipeValidationError error in ex.ValidationErrors)
                {
                    WriteBullet($"[{error.Code}] {error.Message}", ConsoleColor.Yellow);
                }

                return 1;
            }

            // Structural / I/O / parse error.
            WriteHeader("Failed to load recipe.", ConsoleColor.Red);
            WriteError(ex.Message);

            if (ex.InnerException is not null)
            {
                WriteDetail($"Inner: {ex.InnerException.Message}");
            }

            return 2;
        }
        catch (Exception ex)
        {
            WriteHeader("Unexpected error while validating recipe.", ConsoleColor.Red);
            WriteError(ex.Message);
            return 2;
        }
    }

    #region Console helpers

    private static void WriteSuccess(string message) => WriteLineWithColor(message, ConsoleColor.Green);

    private static void WriteError(string message) => WriteLineWithColor(message, ConsoleColor.Red);

    private static void WriteDetail(string message) => WriteLineWithColor(message, ConsoleColor.DarkGray);

    private static void WriteHeader(string message, ConsoleColor color)
    {
        ConsoleColor original = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = original;
    }

    private static void WriteBullet(string message, ConsoleColor color)
    {
        ConsoleColor original = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine($" - {message}");
        Console.ForegroundColor = original;
    }

    private static void WriteLineWithColor(string message, ConsoleColor color)
    {
        ConsoleColor original = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = original;
    }

    #endregion
}
