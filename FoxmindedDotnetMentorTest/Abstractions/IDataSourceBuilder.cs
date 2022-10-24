namespace FoxmindedDotnetMentorTest.Abstractions;

public interface IDataSourceBuilder
{
    public int PathsCount { get; }
    IDataSourceBuilder UseFilePath(string path);
    IDataSourceBuilder TryUseFilePathFromInputArgs(IEnumerable<string>? args);
    IReadOnlyList<string> Build();
}