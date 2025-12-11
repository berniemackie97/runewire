using Runewire.Core.Domain.Recipes;
using Runewire.Orchestrator.Orchestration;

namespace Runewire.Cli.Infrastructure;

/// <summary>
/// An injection engine implementation that does not perform any real
/// injection. Instead, it logs what would happen and returns success.
///
/// Useful for early CLI wiring, demos, and as a safe default in labs.
/// </summary>
public sealed class DryRunInjectionEngine : IInjectionEngine
{
    public Task<InjectionResult> ExecuteAsync(
        InjectionRequest request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        Console.WriteLine("Dry-run injection plan:");
        Console.WriteLine($"  Recipe:   {request.RecipeName}");
        if (!string.IsNullOrWhiteSpace(request.RecipeDescription))
        {
            Console.WriteLine($"  Summary:  {request.RecipeDescription}");
        }

        Console.WriteLine($"  Target:   {DescribeTarget(request)}");
        Console.WriteLine($"  Technique:{request.TechniqueName}");
        Console.WriteLine($"  Payload:  {request.PayloadPath}");
        Console.WriteLine(
            $"  Kernel:   {(request.AllowKernelDrivers ? "allowed" : "not allowed")}"
        );
        Console.WriteLine(
            $"  Consent:  {(request.RequireInteractiveConsent ? "required" : "not required")}"
        );

        DateTimeOffset now = DateTimeOffset.UtcNow;
        return Task.FromResult(InjectionResult.Succeeded(now, now));
    }

    private static string DescribeTarget(InjectionRequest request)
    {
        return request.Target.Kind switch
        {
            RecipeTargetKind.Self => "self (current Runewire process)",
            RecipeTargetKind.ProcessById => $"PID {request.Target.ProcessId}",
            RecipeTargetKind.ProcessByName => $"process \"{request.Target.ProcessName}\"",
            _ => $"unknown target kind {request.Target.Kind}",
        };
    }
}
