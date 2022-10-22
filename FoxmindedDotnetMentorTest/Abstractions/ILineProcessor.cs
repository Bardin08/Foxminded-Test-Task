using FoxmindedDotnetMentorTest.Models;

namespace FoxmindedDotnetMentorTest.Abstractions;

public interface ILineProcessor
{
    public Task<LineInfo> ProcessLineAsync(string line, int lineNumber);
}