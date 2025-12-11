namespace Runewire.Orchestrator.Orchestration;

/// <summary>
/// Abstraction over the native injection runtime.
///
/// The .NET orchestrator maps recipes into <see cref="InjectionRequest"/>
/// instances and the engine is responsible for executing them using the
/// configured native injector (e.g. Runewire.Injector).
/// </summary>
public interface IInjectionEngine
{
    /// <summary>
    /// Execute an injection request, and should return
    /// a detailed result for logging and telemetry.
    /// </summary>
    Task<InjectionResult> ExecuteAsync(InjectionRequest request, CancellationToken cancellationToken = default);
}
