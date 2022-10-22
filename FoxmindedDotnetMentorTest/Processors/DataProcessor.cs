using FoxmindedDotnetMentorTest.Abstractions;
using FoxmindedDotnetMentorTest.Models;

namespace FoxmindedDotnetMentorTest.Processors;

public class DataProcessor : IDataProcessor
{
    private readonly IFileProcessor _fileProcessor;
    private readonly ILineProcessor _lineProcessor;

    public DataProcessor(IFileProcessor fileProcessor, ILineProcessor lineProcessor)
    {
        _fileProcessor = fileProcessor;
        _lineProcessor = lineProcessor;
    }
    
    public async Task<DataProcessingResult> ProcessAsync(IEnumerable<string> paths)
    {
        var filesProcessingRes = (await Task.WhenAll(paths.Select(ProcessFileAsync))).ToList();

        return new DataProcessingResult
        {
            FilesProcessingResults = filesProcessingRes
        };
    }

    private async Task<FileProcessingResult> ProcessFileAsync(string path)
    {
        var lines = await _fileProcessor.ReadLinesAsync(path);
        var linesProcessingTasks = lines.Select((line, index) => _lineProcessor.ProcessLineAsync(line, index + 1));
        var processedLines = (await Task.WhenAll(linesProcessingTasks)).ToList();

        return PopulateFileProcessingResult(path, processedLines);
    }

    private FileProcessingResult PopulateFileProcessingResult(string filePath, List<LineInfo> lineInfos)
    {
        var fileProcessingResult = new FileProcessingResult {FilePath = filePath};

        var maxSum = decimal.MinValue;
        foreach (var lineInfo in lineInfos)
        {
            if (!lineInfo.IsValid)
            {
                fileProcessingResult.InvalidStringNumbers ??= new List<int>(); 

                fileProcessingResult.InvalidStringNumbers.Add(lineInfo.Number);
                continue;
            }

            if (lineInfo.Sum.HasValue && lineInfo.Sum > maxSum)
            {
                maxSum = lineInfo.Sum.Value;
                fileProcessingResult.LineWithMaxSumNumber = lineInfo.Number;
            }
        }

        return fileProcessingResult;
    }
}