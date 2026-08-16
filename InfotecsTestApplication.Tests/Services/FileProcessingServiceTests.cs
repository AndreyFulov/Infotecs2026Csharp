using System.Text;
using InfotecsTestApplication.Exceptions;
using InfotecsTestApplication.Services;
using Microsoft.AspNetCore.Http;

namespace InfotecsTestApplication.Tests.Services;

public class FileProcessingTests
{
    private readonly FileProcessing _fileProcessing = new();

    [Fact]
    public async Task ParseCSVAsync_ShouldParseValidCsv()
    {
        // Arrange
        var csv = """
                  Date;ExecutionTime;Value
                  2026-08-15T10:00:00.0000Z;10;25.5
                  2026-08-15T10:00:10.0000Z;20;30.5
                  """;

        var file = CreateFile(csv);

        // Act
        var result = await _fileProcessing.ParseCSVAsync(file);

        // Assert
        Assert.Equal(2, result.Count);

        Assert.Equal(
            new DateTimeOffset(
                2026, 8, 15,
                10, 0, 0,
                TimeSpan.Zero),
            result[0].Date);

        Assert.Equal(10, result[0].ExecutionTime);
        Assert.Equal(25.5, result[0].Value);

        Assert.Equal(20, result[1].ExecutionTime);
        Assert.Equal(30.5, result[1].Value);
    }

    [Fact]
    public async Task ParseCSVAsync_ShouldReturnEmptyList_WhenCsvContainsOnlyHeader()
    {
        // Arrange
        var csv = """
                  Date;ExecutionTime;Value
                  """;

        var file = CreateFile(csv);

        // Act
        var result = await _fileProcessing.ParseCSVAsync(file);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task ParseCSVAsync_ShouldThrowInvalidCsvException_WhenValueHasInvalidType()
    {
        // Arrange
        var csv = """
                  Date;ExecutionTime;Value
                  2026-08-15T10:00:00.0000Z;10;hello
                  """;

        var file = CreateFile(csv);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCsvException>(
            () => _fileProcessing.ParseCSVAsync(file));
    }

    [Fact]
    public async Task ParseCSVAsync_ShouldThrowInvalidCsvException_WhenExecutionTimeHasInvalidType()
    {
        // Arrange
        var csv = """
                  Date;ExecutionTime;Value
                  2026-08-15T10:00:00.0000Z;hello;25.5
                  """;

        var file = CreateFile(csv);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCsvException>(
            () => _fileProcessing.ParseCSVAsync(file));
    }

    [Fact]
    public async Task ParseCSVAsync_ShouldThrowInvalidCsvException_WhenDateHasInvalidFormat()
    {
        // Arrange
        var csv = """
                  Date;ExecutionTime;Value
                  not-a-date;10;25.5
                  """;

        var file = CreateFile(csv);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCsvException>(
            () => _fileProcessing.ParseCSVAsync(file));
    }

    [Fact]
    public async Task ParseCSVAsync_ShouldThrowInvalidCsvException_WhenColumnIsMissing()
    {
        // Arrange
        var csv = """
                  Date;ExecutionTime;Value
                  2026-08-15T10:00:00.0000Z;10
                  """;

        var file = CreateFile(csv);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCsvException>(
            () => _fileProcessing.ParseCSVAsync(file));
    }

    [Fact]
    public async Task ParseCSVAsync_ShouldThrowInvalidCsvException_WhenDelimiterIsIncorrect()
    {
        // Arrange
        var csv = """
                  Date,ExecutionTime,Value
                  2026-08-15T10:00:00.0000Z,10,25.5
                  """;

        var file = CreateFile(csv);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCsvException>(
            () => _fileProcessing.ParseCSVAsync(file));
    }

    private static IFormFile CreateFile(string content)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        var stream = new MemoryStream(bytes);

        return new FormFile(
            stream,
            0,
            bytes.Length,
            "file",
            "test.csv")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/csv"
        };
    }
}