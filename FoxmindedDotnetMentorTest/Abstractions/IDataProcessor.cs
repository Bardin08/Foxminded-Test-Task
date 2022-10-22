using FoxmindedDotnetMentorTest.Models;

namespace FoxmindedDotnetMentorTest.Abstractions;

public interface IDataProcessor
{
    Task<DataProcessingResult> ProcessAsync(IEnumerable<string> paths);
}