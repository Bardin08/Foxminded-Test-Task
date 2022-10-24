using System.Text;
using FoxmindedDotnetMentorTest.Abstractions;
using FoxmindedDotnetMentorTest.Models;
using FoxmindedDotnetMentorTest.Processors;

namespace FoxmindedDotnetMentorTest;

internal static class Program
{
    private static readonly IFileProcessor FileProcessor = new FileProcessor();
    public static readonly ILineProcessor _lineProcessor = new LineProcessor();

    static async Task Main(string[]? args)
    {
        await Start(args);
    }

    private static async Task Start(string[]? args)
    {
        while (true)
        {
            var paths = TryGetPaths(args);

            var processingResult = await new DataProcessor(FileProcessor, _lineProcessor).ProcessAsync(paths);

            Console.Clear();

            PrintProcessingResult(processingResult);
        }
    }

    private static IEnumerable<string> TryGetPaths(string[]? args)
    {
        var dataSource = new DataSourceBuilder()
            .TryUseFilePathFromInputArgs(args);

        if (dataSource.PathsCount > 0)
        {
            return dataSource.Build();
        }

        while (true)
        {
            Console.WriteLine("Please enter a path to the file: ");
            var path = Console.ReadLine();

            if (File.Exists(path))
            {
                dataSource.UseFilePath(path);
                return dataSource.Build();
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid path!");
            Console.ResetColor();
        }
    }

    private static void PrintProcessingResult(DataProcessingResult processingResult)
    {
        if (processingResult.FilesProcessingResults is null ||
            !processingResult.FilesProcessingResults.Any())
        {
            Console.WriteLine("0 files were processed...");
            return;
        }

        var sb = new StringBuilder();
        foreach (var fileProcessingResult in processingResult.FilesProcessingResults)
        {
            sb.Append("\\\\ --- //")
                .AppendLine();

            sb.Append($"File {fileProcessingResult.FilePath}")
                .AppendLine()
                .AppendLine();

            sb.Append($"Line with max sum: {fileProcessingResult.LineWithMaxSumNumber}")
                .AppendLine();

            if (fileProcessingResult.InvalidStringNumbers != null)
            {
                sb.Append($"Invalid lines amount: {fileProcessingResult.InvalidStringNumbers.Count}")
                    .AppendLine();
                sb.Append($"Invalid lines numbers: {string.Join(",", fileProcessingResult.InvalidStringNumbers)}")
                    .AppendLine();
            }

            sb.Append("// --- \\\\")
                .AppendLine();

            Console.WriteLine(sb.ToString());
            sb.Clear();
        }
    }
}
