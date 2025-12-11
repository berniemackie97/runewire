# Runewire

A programmable, testable orchestration layer for **Windows process injection experiments**.

Runewire is designed for:

- Authorized security research
- Detection engineering
- Reverse engineering on systems you own or are authorized to test

It is **not** a malware builder or an "AV bypass" toolkit.

## Projects

- `src/Runewire.Core` — domain model + orchestration abstractions.
- `src/Runewire.Cli` — CLI front-end for running recipes.
- `tests/Runewire.Core.Tests` — xUnit tests for the core domain.
- `native/Runewire.Injector` — C++20 native injection engine (CMake-based).

## Building

```bash
dotnet build Runewire.sln
```
