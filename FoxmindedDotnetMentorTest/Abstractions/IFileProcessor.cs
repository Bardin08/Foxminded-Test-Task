namespace FoxmindedDotnetMentorTest.Abstractions;

public interface IFileProcessor
{
    Task<string[]> ReadLinesAsync(string path);
}