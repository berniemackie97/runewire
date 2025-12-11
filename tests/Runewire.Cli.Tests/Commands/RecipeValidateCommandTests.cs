using System.Text;

namespace Runewire.Cli.Tests.Commands;

public class RecipeValidateCommandTests
{
    [Fact]
    public void Validate_valid_recipe_returns_exit_code_0_and_success_output()
    {
        // Setup
        string recipePath = CreateTempFile(
            """
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
            """
        );

        // Run
        (int exitCode, string output) = RunWithCapturedOutput("validate", recipePath);

        // Assert
        Assert.Equal(0, exitCode);
        Assert.Contains("Recipe is valid", output);
        Assert.Contains("demo-recipe", output);
    }

    [Fact]
    public void Validate_invalid_recipe_returns_exit_code_1_and_lists_errors()
    {
        // Setup
        string recipePath = CreateTempFile(
            """
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
            """
        );

        // Run
        (int exitCode, string output) = RunWithCapturedOutput("validate", recipePath);

        // Assert
        Assert.Equal(1, exitCode);
        Assert.Contains("Recipe is invalid", output);
        Assert.Contains("RECIPE_NAME_REQUIRED", output);
        Assert.Contains("TARGET_PID_INVALID", output);
        Assert.Contains("TECHNIQUE_NAME_REQUIRED", output);
        Assert.Contains("PAYLOAD_PATH_REQUIRED", output);
        Assert.Contains("SAFETY_KERNEL_DRIVER_CONSENT_REQUIRED", output);
    }

    [Fact]
    public void Validate_missing_file_returns_exit_code_2_and_error_message()
    {
        // Setup
        string nonExistentPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".yaml");

        // Run
        (int exitCode, string output) = RunWithCapturedOutput("validate", nonExistentPath);

        // Assert
        Assert.Equal(2, exitCode);
        Assert.Contains("Recipe file not found", output);
    }

    [Fact]
    public void No_arguments_shows_help_and_returns_non_zero()
    {
        // Run
        (int exitCode, string output) = RunWithCapturedOutput([]);

        // Assert
        Assert.NotEqual(0, exitCode);
        Assert.Contains("Required command was not provided", output);
        Assert.Contains("Runewire process injection lab CLI", output);
        Assert.Contains("validate <recipe>", output);
    }

    private static (int exitCode, string stdout) RunWithCapturedOutput(params string[] args)
    {
        TextWriter originalOut = Console.Out;
        TextWriter originalErr = Console.Error;

        StringBuilder sb = new();
        using StringWriter writer = new(sb);

        Console.SetOut(writer);
        Console.SetError(writer);

        try
        {
            int exitCode = Program.Main(args);
            writer.Flush();
            return (exitCode, sb.ToString());
        }
        finally
        {
            Console.SetOut(originalOut);
            Console.SetError(originalErr);
        }
    }

    private static string CreateTempFile(string contents)
    {
        string path = Path.Combine(Path.GetTempPath(), $"runewire-test-{Guid.NewGuid():N}.yaml");
        File.WriteAllText(path, contents);
        return path;
    }
}
