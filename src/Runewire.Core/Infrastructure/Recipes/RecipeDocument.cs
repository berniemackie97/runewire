namespace Runewire.Core.Infrastructure.Recipes
{
    internal sealed class RecipeDocument
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public RecipeTargetDocument? Target { get; set; }
        public RecipeTechniqueDocument? Technique { get; set; }
        public RecipePayloadDocument? Payload { get; set; }
        public RecipeSafetyDocument? Safety { get; set; }
    }
}
