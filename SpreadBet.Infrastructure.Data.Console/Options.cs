using CommandLine;
using CommandLine.Text;

namespace SpreadBet.Infrastructure.Data.Console
{
    public enum BuildType
    {
        Rebuild,
        Migrate
    }

    public class Options
    {
        [Option('d', Required = true, HelpText="Database update action i.e '-d migrate' (create if not exist else migrate to latest) or '-d rebuild' (drop and recreate to latest)")]
        public BuildType DatabaseBuildType { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(
                this, 
                (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current)
                );
        }
    }
}
