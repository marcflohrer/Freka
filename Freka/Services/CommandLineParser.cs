using CommandLine;
using Freka.Services.Models;
using static Program;

namespace Freka.Services;

public record CommandLineInfo(bool verbose, FrekaFile? frekaFile, DirectoryInfo ProjectDir);
public static class CommandLineParser
{
    public static CommandLineInfo Parse(string[] args)
    {
        if (args == null)
        {
            Console.WriteLine("args is null");
        }
        else
        {
            var verbose = false;
            FrekaFile? frekaFile = null;
            DirectoryInfo? projectDir = null;
            // Step 2: print length, and loop over all arguments.
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed(o =>
                   {
                       if (o.Verbose)
                       {
                           Console.WriteLine($"Verbose output enabled. Given Arguments: -v {o.Verbose}");

                           verbose = true;
                       }
                       else
                       {
                           Console.WriteLine($"Given Arguments: -v {o.Verbose}");

                       }
                       Console.Write("Quick Start Example!");
                       if (verbose)
                       {
                           Console.Write($" App is in Verbose mode!{Environment.NewLine}");
                       }

                       if (!string.IsNullOrWhiteSpace(o.FrekaFile))
                       {
                           FileInfo tempFile = new FileInfo(o.FrekaFile);
                           if (!tempFile.Exists)
                           {
                               throw new ArgumentException($"Freka file does not exist at {tempFile.Directory?.FullName}!");
                           }
                           frekaFile = FrekaParser.Parse(tempFile.FullName);
                           projectDir = tempFile.Directory!;
                       }
                       else
                       {
                           throw new ArgumentException("Path to freka file is not set.");
                       }
                   });
            return new CommandLineInfo(verbose, frekaFile, projectDir!);
        }
        throw new ArgumentException("Missing arguments!");
    }
}
