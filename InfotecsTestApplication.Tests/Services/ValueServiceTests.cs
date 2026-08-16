using InfotecsTestApplication.Data;
using InfotecsTestApplication.Models.Entity;
using InfotecsTestApplication.Services;
using InfotecsTestApplication.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace InfotecsTestApplication.Tests.Services;

public class ValuesServiceTests
{
    [Fact]
    public async Task SaveValuesFromFile_ShouldCreateNewResult()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        var context = factory.Context;

        var values = CreateValues(3);
        var file = CreateFile("test.csv");

        var fileProcessingMock = new Mock<IFileProcessingService>();
        var resultServiceMock = new Mock<IResultService>();

        fileProcessingMock
            .Setup(x => x.ParseCSVAsync(file))
            .ReturnsAsync(values);

        fileProcessingMock
            .Setup(x => x.ValidateNewData(values));

        var calculatedResult = CreateResult(
            "test.csv",
            values);

        resultServiceMock
            .Setup(x => x.CalculateResult(values, "test.csv"))
            .Returns(calculatedResult);

        var service = new ValuesService(
            fileProcessingMock.Object,
            context,
            resultServiceMock.Object);

        // Act
        await service.SaveValuesFromFile(file);

        // Assert
        var result = await context.Results
            .Include(x => x.Values)
            .SingleAsync();

        Assert.Equal("test.csv", result.Name);
        Assert.Equal(3, result.Values.Count);

        Assert.Equal(
            values.Select(x => x.Value),
            result.Values.Select(x => x.Value));

        fileProcessingMock.Verify(
            x => x.ParseCSVAsync(file),
            Times.Once);

        fileProcessingMock.Verify(
            x => x.ValidateNewData(values),
            Times.Once);

        resultServiceMock.Verify(
            x => x.CalculateResult(values, "test.csv"),
            Times.Once);
    }


    [Fact]
    public async Task SaveValuesFromFile_ShouldReplaceExistingResult()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        var context = factory.Context;

        var oldValues = CreateValues(2);

        var existingResult = CreateResult(
            "test.csv",
            oldValues);

        context.Results.Add(existingResult);
        await context.SaveChangesAsync();

        var newValues = CreateValues(3);
        var file = CreateFile("test.csv");

        var fileProcessingMock = new Mock<IFileProcessingService>();
        var resultServiceMock = new Mock<IResultService>();

        fileProcessingMock
            .Setup(x => x.ParseCSVAsync(file))
            .ReturnsAsync(newValues);

        var calculatedResult = CreateResult(
            "test.csv",
            newValues);

        resultServiceMock
            .Setup(x => x.CalculateResult(newValues, "test.csv"))
            .Returns(calculatedResult);

        var service = new ValuesService(
            fileProcessingMock.Object,
            context,
            resultServiceMock.Object);

        // Act
        await service.SaveValuesFromFile(file);

        // Assert
        var result = await context.Results
            .Include(x => x.Values)
            .SingleAsync();

        Assert.Equal("test.csv", result.Name);

        Assert.Equal(
            3,
            result.Values.Count);

        Assert.Equal(
            newValues.Select(x => x.Value),
            result.Values
                .OrderBy(x => x.Value)
                .Select(x => x.Value));

        Assert.Equal(
            calculatedResult.AverageValue,
            result.AverageValue);

        Assert.Equal(
            calculatedResult.MedianValue,
            result.MedianValue);

        Assert.Equal(
            calculatedResult.MaxValue,
            result.MaxValue);

        Assert.Equal(
            calculatedResult.MinValue,
            result.MinValue);

        resultServiceMock.Verify(
            x => x.CalculateResult(
                newValues,
                "test.csv"),
            Times.Once);

        fileProcessingMock.Verify(
            x => x.ValidateNewData(newValues),
            Times.Once);
    }


    [Fact]
    public async Task GetValues_ShouldReturnAllValues()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        var context = factory.Context;

        var values = CreateValues(5);

        var resultModel = CreateResult(
            "test.csv",
            values);

        context.Results.Add(resultModel);

        await context.SaveChangesAsync();

        var service = new ValuesService(
            Mock.Of<IFileProcessingService>(),
            context,
            Mock.Of<IResultService>());

        // Act
        var result = await service.GetValues();

        // Assert
        Assert.Equal(5, result.Count);
    }


    [Fact]
    public async Task GetValuesFromFile_ShouldReturnMaximum10Values()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        var context = factory.Context;

        var values = CreateValues(15);

        var resultModel = CreateResult(
            "test.csv",
            values);

        context.Results.Add(resultModel);

        await context.SaveChangesAsync();

        var service = new ValuesService(
            Mock.Of<IFileProcessingService>(),
            context,
            Mock.Of<IResultService>());

        // Act
        var result = await service.GetValuesFromFile("test");

        // Assert
        Assert.Equal(10, result.Count);
    }


    [Fact]
    public async Task GetValuesFromFile_ShouldReturnValuesSortedByDateDescending()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        var context = factory.Context;

        var values = new List<ValueModel>
        {
            new()
            {
                Date = new DateTimeOffset(
                    2026,
                    8,
                    15,
                    10,
                    0,
                    0,
                    TimeSpan.Zero),

                ExecutionTime = 1,
                Value = 10
            },

            new()
            {
                Date = new DateTimeOffset(
                    2026,
                    8,
                    15,
                    11,
                    0,
                    0,
                    TimeSpan.Zero),

                ExecutionTime = 1,
                Value = 20
            },

            new()
            {
                Date = new DateTimeOffset(
                    2026,
                    8,
                    15,
                    12,
                    0,
                    0,
                    TimeSpan.Zero),

                ExecutionTime = 1,
                Value = 30
            }
        };

        var resultModel = CreateResult(
            "test.csv",
            values);

        context.Results.Add(resultModel);

        await context.SaveChangesAsync();

        var service = new ValuesService(
            Mock.Of<IFileProcessingService>(),
            context,
            Mock.Of<IResultService>());

        // Act
        var result = await service.GetValuesFromFile("test");

        // Assert
        Assert.Equal(3, result.Count);

        Assert.Equal(30, result[0].Value);
        Assert.Equal(20, result[1].Value);
        Assert.Equal(10, result[2].Value);

        Assert.True(
            result[0].Date > result[1].Date);

        Assert.True(
            result[1].Date > result[2].Date);
    }


    [Fact]
    public async Task GetValuesFromFile_ShouldSearchByPartialFileName()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        var context = factory.Context;

        var values = CreateValues(3);

        var resultModel = CreateResult(
            "test-file.csv",
            values);

        context.Results.Add(resultModel);

        await context.SaveChangesAsync();

        var service = new ValuesService(
            Mock.Of<IFileProcessingService>(),
            context,
            Mock.Of<IResultService>());

        // Act
        var result = await service.GetValuesFromFile("test");

        // Assert
        Assert.Equal(3, result.Count);
    }


    [Fact]
    public async Task GetValuesFromFile_ShouldReturnEmpty_WhenFileNotFound()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        var context = factory.Context;

        var service = new ValuesService(
            Mock.Of<IFileProcessingService>(),
            context,
            Mock.Of<IResultService>());

        // Act
        var result = await service.GetValuesFromFile("unknown");

        // Assert
        Assert.Empty(result);
    }


    private static List<ValueModel> CreateValues(int count)
    {
        return Enumerable
            .Range(1, count)
            .Select(i => new ValueModel
            {
                Date = new DateTimeOffset(
                    2026,
                    8,
                    15,
                    10,
                    0,
                    i,
                    TimeSpan.Zero),

                ExecutionTime = i,

                Value = i * 10
            })
            .ToList();
    }


    private static ResultModel CreateResult(
        string fileName,
        List<ValueModel> values)
    {
        var minDate = values.Min(x => x.Date);
        var maxDate = values.Max(x => x.Date);

        var sortedValues = values
            .OrderBy(x => x.Value)
            .ToList();

        var middle = sortedValues.Count / 2;

        var median =
            sortedValues.Count % 2 == 0
                ? (
                    sortedValues[middle - 1].Value +
                    sortedValues[middle].Value
                ) / 2
                : sortedValues[middle].Value;

        return new ResultModel
        {
            Name = fileName,

            Values = values,

            StartedAt = minDate,

            DurationSecons =
                (long)(maxDate - minDate).TotalSeconds,

            AverageExecutionTime =
                values.Average(x => x.ExecutionTime),

            AverageValue =
                values.Average(x => x.Value),

            MedianValue = median,

            MinValue =
                values.Min(x => x.Value),

            MaxValue =
                values.Max(x => x.Value)
        };
    }


    private static IFormFile CreateFile(
        string fileName)
    {
        var stream = new MemoryStream();

        return new FormFile(
            stream,
            0,
            0,
            "file",
            fileName);
    }
}