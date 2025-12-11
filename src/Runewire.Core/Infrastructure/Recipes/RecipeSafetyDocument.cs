namespace Runewire.Core.Infrastructure.Recipes
{
    internal sealed class RecipeSafetyDocument
    {
        public bool RequireInteractiveConsent { get; set; } = true;
        public bool AllowKernelDrivers { get; set; }
    }
}
