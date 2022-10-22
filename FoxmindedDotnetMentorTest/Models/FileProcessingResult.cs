namespace FoxmindedDotnetMentorTest.Models;

public class FileProcessingResult
{
    public string? FilePath { get; set; }
    public int LineWithMaxSumNumber { get; set; }
    public List<int>? InvalidStringNumbers { get; set; }
}