using CommandLine;

public partial class Program
{
    public class Options
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        [Option('f', "frekafile", Required = true, HelpText = "Set path to frekafile.")]
        public string? FrekaFile { get; set; }
    }
}