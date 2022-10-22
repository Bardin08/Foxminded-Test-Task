using System.Collections.Concurrent;
using FoxmindedDotnetMentorTest.Abstractions;

namespace FoxmindedDotnetMentorTest.Processors;

public class FileProcessor : IFileProcessor
{
    private readonly ConcurrentDictionary<string, Task<string[]>> _filesRead = new();

    public Task<string[]> ReadLinesAsync(string path)
    {
        return _filesRead.TryGetValue(path, out var readFileTask)
            ? readFileTask
            : _filesRead.GetOrAdd(path, p => File.ReadAllLinesAsync(p));
    }
}