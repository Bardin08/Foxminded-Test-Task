using FoxmindedDotnetMentorTest.Abstractions;

namespace FoxmindedDotnetMentorTest;

public class DataSourceBuilder : IDataSourceBuilder
{
    private readonly List<string> _paths = new();

    public IDataSourceBuilder UseFilePath(string path)
    {
        if (!File.Exists(path))
        {
            const string fileNotFoundFormat = "File {0} not found";
            throw new FileNotFoundException(string.Format(fileNotFoundFormat, path));
        }

        _paths.Add(path);

        return this;
    }

    public IDataSourceBuilder TryUseFilePathFromInputArgs(IEnumerable<string>? args)
    {
        const string dataFilePaths = "dataPaths";

        if (args is null)
        {
            return this;
        }

        var enumerable = args as string[] ?? args.ToArray();
        var cleanedInputs = enumerable.Where(x => x.StartsWith(dataFilePaths))
            .Select(x => x[(dataFilePaths.Length + 1)..]);
        var paths = string.Join(",", cleanedInputs)
            .Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        foreach (var path in paths)
        {
            try
            {
                UseFilePath(path);
            }
            catch (FileNotFoundException)
            {
                // Nothing mentions in tech-task about files that's not exists.
                // So, for now if file not exists - just skip it
            }
        }

        return this;
    }

    public IReadOnlyList<string> Build()
    {
        return _paths.AsReadOnly();
    }
}