using System.Linq;
using System.Threading.Tasks;
using FoxmindedDotnetMentorTest.Abstractions;
using FoxmindedDotnetMentorTest.Models;
using FoxmindedDotnetMentorTest.Processors;
using Moq;
using Xunit;

namespace FoxmindedDotnetMentorTest.Tests;

public class DataProcessorTests
{
    private const string ValidLine = "123,1.2,3";
    private const string InvalidLine = "abracadabra";

    private readonly IDataProcessor _sut;

    private readonly Mock<IFileProcessor> _fileProcessorMock;
    private readonly Mock<ILineProcessor> _lineProcessorMock;

    public DataProcessorTests()
    {
        _fileProcessorMock = new Mock<IFileProcessor>();
        _lineProcessorMock = new Mock<ILineProcessor>();

        _sut = new DataProcessor(_fileProcessorMock.Object, _lineProcessorMock.Object);
    }

    [Fact]
    public async Task ProcessFiles_Success_FilesProcessingResult()
    {
        _fileProcessorMock.Setup(x => x.ReadLinesAsync(It.IsAny<string>()))
            .ReturnsAsync(new[] {ValidLine, InvalidLine});

        _lineProcessorMock.Setup(x => x.ProcessLineAsync(It.IsAny<string>(), It.IsAny<int>()))
            .Returns(GetLineInfo);

        var r = await _sut.ProcessAsync(new []{"test-pass-to-file|doesn't really matters"});

        Assert.NotNull(r.FilesProcessingResults);
        
        var firstFileResult = r.FilesProcessingResults!.FirstOrDefault();
        
        Assert.NotNull(firstFileResult);
        Assert.Equal(1, firstFileResult!.LineWithMaxSumNumber);
    }

    private Task<LineInfo> GetLineInfo(string line, int number)
    {
        var res = new LineInfo { Line = line, Number = number };
        if (line == ValidLine)
        {
            res.Sum = 1;
            res.IsValid = true;
        }
        
        return Task.FromResult(res);
    }
}