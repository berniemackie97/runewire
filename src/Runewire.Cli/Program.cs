using System.CommandLine;
using Runewire.Cli.Commands;

namespace Runewire.Cli;

public static class Program
{
    public static int Main(string[] args)
    {
        RootCommand rootCommand = BuildRootCommand();

        ParseResult parseResult = rootCommand.Parse(args);
        return parseResult.Invoke();
    }

    private static RootCommand BuildRootCommand()
    {
        RootCommand root = new("Runewire process injection lab CLI");

        //   runewire validate <recipe.yaml>
        //   runewire run <recipe.yaml>
        root.Subcommands.Add(RecipeValidateCommand.Create());
        root.Subcommands.Add(RecipeRunCommand.Create());

        return root;
    }

}
