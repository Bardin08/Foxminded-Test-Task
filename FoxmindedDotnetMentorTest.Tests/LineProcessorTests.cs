using System.Threading.Tasks;
using FoxmindedDotnetMentorTest.Abstractions;
using FoxmindedDotnetMentorTest.Processors;
using Xunit;

namespace FoxmindedDotnetMentorTest.Tests;

public class LineProcessorTests
{
    private readonly ILineProcessor _sut;

    public LineProcessorTests()
    {
        _sut = new LineProcessor();
    }

    [Theory]
    [InlineData("1.2,3,10", "14,2")]
    public async Task ProcessLine_ValidLine_LineInfo(string line, string expectedSum)
    {
        var lineProcessingResult = await _sut.ProcessLineAsync(line, 1);
        
        Assert.NotNull(lineProcessingResult);
        Assert.Equal(line, lineProcessingResult.Line);
        Assert.Equal(1, lineProcessingResult.Number);
        Assert.Equal(decimal.Parse(expectedSum), lineProcessingResult.Sum);
        Assert.True(lineProcessingResult.IsValid);
    }
}