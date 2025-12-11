using System.Text;

namespace Runewire.Cli.Tests.Commands;

public class RecipeRunCommandTests
{
    [Fact]
    public void Run_valid_recipe_returns_exit_code_0_and_reports_success()
    {
        // Setup
        string recipePath = CreateTempFile(
            """
            name: demo-run
            description: Demo run execution.
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
        (int exitCode, string output) = RunWithCapturedOutput("run", recipePath);

        // Assert
        Assert.Equal(0, exitCode);
        Assert.Contains("Injection succeeded", output);
        Assert.Contains("demo-run", output);
    }

    [Fact]
    public void Run_invalid_recipe_returns_exit_code_1_and_lists_errors()
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
        (int exitCode, string output) = RunWithCapturedOutput("run", recipePath);

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
    public void Run_missing_file_returns_exit_code_2_and_error_message()
    {
        // Setup
        string nonExistentPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".yaml");

        // Run
        (int exitCode, string output) = RunWithCapturedOutput("run", nonExistentPath);

        // Assert
        Assert.Equal(2, exitCode);
        Assert.Contains("Recipe file not found", output);
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
        string path = Path.Combine(Path.GetTempPath(), $"runewire-run-test-{Guid.NewGuid():N}.yaml");
        File.WriteAllText(path, contents);
        return path;
    }
}
