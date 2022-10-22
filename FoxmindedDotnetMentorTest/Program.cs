using System.Text;
using FoxmindedDotnetMentorTest.Models;
using FoxmindedDotnetMentorTest.Processors;

namespace FoxmindedDotnetMentorTest;

internal static class Program
{
    static async Task Main(string[]? args)
    {
        var sourceBuilder = new DataSourceBuilder()
            .TryUseFilePathFromInputArgs(args)
            .UseFilePath(@"C:\Users\vbard\OneDrive\Рабочий стол\Test2.txt");

        var fileProcessor = new FileProcessor();
        var lineProcessor = new LineProcessor();

        var result = await new DataProcessor(fileProcessor, lineProcessor)
            .ProcessAsync(sourceBuilder.Build());

        PrintProcessingResult(result);
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
