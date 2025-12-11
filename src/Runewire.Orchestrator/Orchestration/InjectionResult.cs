namespace Runewire.Orchestrator.Orchestration;

/// <summary>
/// Result of an injection operation executed by the injection engine.
/// </summary>
public sealed record InjectionResult(bool Success, string? ErrorCode, string? ErrorMessage, DateTimeOffset StartedAtUtc, DateTimeOffset CompletedAtUtc)
{
    public static InjectionResult Succeeded(DateTimeOffset startedAtUtc, DateTimeOffset completedAtUtc) => new(true, null, null, startedAtUtc, completedAtUtc);

    public static InjectionResult Failed(string? errorCode, string? errorMessage, DateTimeOffset startedAtUtc, DateTimeOffset completedAtUtc)
        => new(false, errorCode, errorMessage, startedAtUtc, completedAtUtc);
}
