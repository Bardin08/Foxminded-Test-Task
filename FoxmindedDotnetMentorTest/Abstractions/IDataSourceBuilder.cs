namespace FoxmindedDotnetMentorTest.Abstractions;

public interface IDataSourceBuilder
{
    IDataSourceBuilder UseFilePath(string path);
    IDataSourceBuilder TryUseFilePathFromInputArgs(IEnumerable<string>? args);
    IReadOnlyList<string> Build();
}