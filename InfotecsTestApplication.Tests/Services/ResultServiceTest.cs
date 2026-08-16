using InfotecsTestApplication.Data;
using InfotecsTestApplication.DTO;
using InfotecsTestApplication.Models.Entity;
using InfotecsTestApplication.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace InfotecsTestApplication.Tests.Services;

public class ResultServiceTest
{
    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public void CalculateResult_ShouldCalculateAllStatisticsCorrectly()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        var context = factory.Context;
        var service = new ResultService(context);

        var start = new DateTimeOffset(
            2026, 8, 15, 10, 0, 0, TimeSpan.Zero);

        var values = new List<ValueModel>
        {
            new()
            {
                Date = start,
                ExecutionTime = 2,
                Value = 10
            },
            new()
            {
                Date = start.AddSeconds(10),
                ExecutionTime = 4,
                Value = 20
            },
            new()
            {
                Date = start.AddSeconds(20),
                ExecutionTime = 6,
                Value = 30
            }
        };

        // Act
        var result = service.CalculateResult(values, "test.csv");

        // Assert
        Assert.Equal("test.csv", result.Name);

        Assert.Equal(start, result.StartedAt);

        Assert.Equal(20, result.DurationSecons);

        Assert.Equal(4, result.AverageExecutionTime);

        Assert.Equal(20, result.AverageValue);

        Assert.Equal(20, result.MedianValue);

        Assert.Equal(10, result.MinValue);

        Assert.Equal(30, result.MaxValue);

        Assert.Equal(values, result.Values);
    }

    [Fact]
    public void CalculateResult_ShouldCalculateEvenMedianCorrectly()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        var context = factory.Context;
        var service = new ResultService(context);

        var values = new List<ValueModel>
        {
            new() { Value = 10 },
            new() { Value = 20 },
            new() { Value = 30 },
            new() { Value = 40 }
        };

        // Act
        var result = service.CalculateResult(values, "test.csv");

        // Assert
        Assert.Equal(25, result.MedianValue);
    }

    [Fact]
    public void CalculateResult_ShouldCalculateOddMedianCorrectly()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        var context = factory.Context;
        var service = new ResultService(context);

        var values = new List<ValueModel>
        {
            new() { Value = 30 },
            new() { Value = 10 },
            new() { Value = 20 }
        };

        // Act
        var result = service.CalculateResult(values, "test.csv");

        // Assert
        Assert.Equal(20, result.MedianValue);
    }

    [Fact]
    public void CalculateResult_ShouldCalculateDurationInSeconds()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        var context = factory.Context;
        var service = new ResultService(context);

        var start = new DateTimeOffset(
            2026, 8, 15, 10, 0, 0, TimeSpan.Zero);

        var values = new List<ValueModel>
        {
            new()
            {
                Date = start,
                ExecutionTime = 1,
                Value = 10
            },
            new()
            {
                Date = start.AddMinutes(2),
                ExecutionTime = 1,
                Value = 20
            }
        };

        // Act
        var result = service.CalculateResult(values, "test.csv");

        // Assert
        Assert.Equal(120, result.DurationSecons);
    }

    [Fact]
    public void CalculateResult_ShouldUseEarliestDateAsStartedAt()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        var context = factory.Context;
        var service = new ResultService(context);

        var earliest = new DateTimeOffset(
            2026, 8, 15, 10, 0, 0, TimeSpan.Zero);

        var values = new List<ValueModel>
        {
            new()
            {
                Date = earliest.AddMinutes(10),
                ExecutionTime = 1,
                Value = 10
            },
            new()
            {
                Date = earliest,
                ExecutionTime = 1,
                Value = 20
            }
        };

        // Act
        var result = service.CalculateResult(values, "test.csv");

        // Assert
        Assert.Equal(earliest, result.StartedAt);
    }

    [Fact]
    public async Task GetResults_ShouldReturnAllResults_WhenFilterIsEmpty()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        var context = factory.Context;

        context.Results.AddRange(
            CreateResult("test.csv", 10, 100),
            CreateResult("another.csv", 20, 200)
        );

        await context.SaveChangesAsync();

        var service = new ResultService(context);

        var filter = new ResultFilter();

        // Act
        var result = await service.GetResults(filter);

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetResults_ShouldFilterByName()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        var context = factory.Context;

        context.Results.AddRange(
            CreateResult("test.csv", 10, 100),
            CreateResult("another.csv", 20, 200)
        );

        await context.SaveChangesAsync();

        var service = new ResultService(context);

        var filter = new ResultFilter
        {
            name = "test"
        };

        // Act
        var result = await service.GetResults(filter);

        // Assert
        var item = Assert.Single(result);

        Assert.Equal("test.csv", item.Name);
    }

    [Fact]
    public async Task GetResults_ShouldFilterByValueFrom()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        var context = factory.Context;

        context.Results.AddRange(
            CreateResult("low.csv", 10, 50),
            CreateResult("high.csv", 10, 100)
        );

        await context.SaveChangesAsync();

        var service = new ResultService(context);

        var filter = new ResultFilter
        {
            valueFrom = 80
        };

        // Act
        var result = await service.GetResults(filter);

        // Assert
        var item = Assert.Single(result);

        Assert.Equal("high.csv", item.Name);
    }

    [Fact]
    public async Task GetResults_ShouldFilterByValueTo()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        var context = factory.Context;

        context.Results.AddRange(
            CreateResult("low.csv", 10, 50),
            CreateResult("high.csv", 10, 100)
        );

        await context.SaveChangesAsync();

        var service = new ResultService(context);

        var filter = new ResultFilter
        {
            valueTo = 80
        };

        // Act
        var result = await service.GetResults(filter);

        // Assert
        var item = Assert.Single(result);

        Assert.Equal("low.csv", item.Name);
    }

    private static ResultModel CreateResult(
        string name,
        double averageExecutionTime,
        double averageValue)
    {
        return new ResultModel
        {
            Name = name,
            StartedAt = new DateTimeOffset(
                2026, 8, 15, 10, 0, 0, TimeSpan.Zero),

            DurationSecons = 100,

            AverageExecutionTime = averageExecutionTime,
            AverageValue = averageValue,

            MedianValue = averageValue,
            MinValue = averageValue,
            MaxValue = averageValue
        };
    }
}