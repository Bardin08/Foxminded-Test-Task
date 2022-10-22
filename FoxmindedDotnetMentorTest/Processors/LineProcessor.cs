using System.Globalization;
using FoxmindedDotnetMentorTest.Abstractions;
using FoxmindedDotnetMentorTest.Models;

namespace FoxmindedDotnetMentorTest.Processors;

public class LineProcessor : ILineProcessor
{
    public Task<LineInfo> ProcessLineAsync(string line, int lineNumber)
    {
        var lineInfo = new LineInfo {Line = line, Number = lineNumber};

        if (string.IsNullOrEmpty(line))
        {
            // Nothing mentioned in tech task, so empty line is invalid
            lineInfo.IsValid = false;
        }

        var lineChunks = line.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        foreach (var num in lineChunks)
        {
            // Maybe it makes sense to cache a valid numbers (as strings) in HashSet, but here benchmarks are required.
            // This should be useful for big files with a lot of duplicates
            if (decimal.TryParse(num, NumberStyles.Float, new NumberFormatInfo {NumberDecimalSeparator = "."},
                    out var parsedNumber))
            {
                lineInfo.Sum ??= decimal.Zero;
                lineInfo.Sum += parsedNumber;
            }
            else
            {
                lineInfo.Sum = null;
                lineInfo.IsValid = false;
                break;
            }
        }

        return Task.FromResult(lineInfo);
    }
}