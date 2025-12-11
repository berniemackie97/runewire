using Runewire.Core.Domain.Techniques;

namespace Runewire.Core.Tests.Domain.Techniques;

public class BuiltInInjectionTechniqueRegistryTests
{
    [Fact]
    public void GetAll_contains_CreateRemoteThread_technique()
    {
        // Setup
        BuiltInInjectionTechniqueRegistry registry = new();

        // Run
        IEnumerable<InjectionTechniqueDescriptor> all = registry.GetAll();

        // Assert
        InjectionTechniqueDescriptor createRemoteThread = Assert.Single(all.Where(t => t.Id == InjectionTechniqueId.CreateRemoteThread));

        Assert.Equal("CreateRemoteThread", createRemoteThread.Name);
        Assert.Equal("CreateRemoteThread DLL Injection", createRemoteThread.DisplayName);
        Assert.False(createRemoteThread.RequiresKernelMode);
        Assert.False(string.IsNullOrWhiteSpace(createRemoteThread.Description));
        Assert.Equal("User-mode DLL injection", createRemoteThread.Category);
    }

    [Fact]
    public void GetById_returns_same_descriptor_as_in_GetAll()
    {
        BuiltInInjectionTechniqueRegistry registry = new();

        InjectionTechniqueDescriptor? byId = registry.GetById(InjectionTechniqueId.CreateRemoteThread);
        Assert.NotNull(byId);

        InjectionTechniqueDescriptor? byName = registry.GetByName("CreateRemoteThread");
        Assert.NotNull(byName);

        Assert.Same(byId, byName);
    }

    [Fact]
    public void GetByName_is_case_insensitive_and_null_safe()
    {
        BuiltInInjectionTechniqueRegistry registry = new();

        InjectionTechniqueDescriptor? upper = registry.GetByName("CREATEREMOTETHREAD");
        InjectionTechniqueDescriptor? mixed = registry.GetByName("CreateRemoteThread");
        InjectionTechniqueDescriptor? lower = registry.GetByName("createremotethread");

        Assert.NotNull(upper);
        Assert.NotNull(mixed);
        Assert.NotNull(lower);

        Assert.Same(upper, mixed);
        Assert.Same(mixed, lower);

        Assert.Null(registry.GetByName(string.Empty));
        Assert.Null(registry.GetByName("DoesNotExist"));
    }

    [Fact]
    public void Descriptor_ctor_throws_on_invalid_parameters()
    {
        Assert.Throws<ArgumentException>(() =>
            new InjectionTechniqueDescriptor(
                InjectionTechniqueId.Unknown,
                "",
                "Display",
                "Category",
                "Description",
                requiresKernelMode: false
            )
        );

        Assert.Throws<ArgumentException>(() =>
            new InjectionTechniqueDescriptor(
                InjectionTechniqueId.Unknown,
                "Name",
                "",
                "Category",
                "Description",
                requiresKernelMode: false
            )
        );

        Assert.Throws<ArgumentException>(() =>
            new InjectionTechniqueDescriptor(
                InjectionTechniqueId.Unknown,
                "Name",
                "Display",
                "",
                "Description",
                requiresKernelMode: false
            )
        );

        Assert.Throws<ArgumentException>(() =>
            new InjectionTechniqueDescriptor(
                InjectionTechniqueId.Unknown,
                "Name",
                "Display",
                "Category",
                "",
                requiresKernelMode: false
            )
        );
    }
}
