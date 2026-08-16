using InfotecsTestApplication.Controllers;
using InfotecsTestApplication.DTO;
using InfotecsTestApplication.Models.Entity;
using InfotecsTestApplication.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InfotecsTestApplication.Tests.Controllers;

public class ValuesControllerTests
{
    private readonly Mock<IValuesService> _valuesServiceMock;
    private readonly ValuesController _controller;

    public ValuesControllerTests()
    {
        _valuesServiceMock = new Mock<IValuesService>();

        _controller = new ValuesController(
            _valuesServiceMock.Object);
    }

    [Fact]
    public async Task GetValues_ShouldReturnOkWithValues()
    {
        // Arrange
        const string fileName = "test";

        var values = new List<ValueDTO>
        {
            new()
            {
                Date = DateTimeOffset.UtcNow,
                ExecutionTime = 1.5,
                Value = 100,
                resultFileName = "test.csv"
            },
            new()
            {
                Date = DateTimeOffset.UtcNow.AddSeconds(1),
                ExecutionTime = 2.5,
                Value = 200,
                resultFileName = "test.csv"
            }
        };

        _valuesServiceMock
            .Setup(x => x.GetValuesFromFile(fileName))
            .ReturnsAsync(values);

        // Act
        var response = await _controller.GetValues(fileName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(response.Result);

        var result = Assert.IsType<List<ValueDTO>>(okResult.Value);

        Assert.Equal(2, result.Count);
        Assert.Equal(100, result[0].Value);
        Assert.Equal(200, result[1].Value);

        _valuesServiceMock.Verify(
            x => x.GetValuesFromFile(fileName),
            Times.Once);
    }

    [Fact]
    public async Task GetValues_ShouldReturnEmptyList_WhenNoValuesFound()
    {
        // Arrange
        const string fileName = "unknown";

        _valuesServiceMock
            .Setup(x => x.GetValuesFromFile(fileName))
            .ReturnsAsync(new List<ValueDTO>());

        // Act
        var response = await _controller.GetValues(fileName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(response.Result);

        var result = Assert.IsType<List<ValueDTO>>(okResult.Value);

        Assert.Empty(result);

        _valuesServiceMock.Verify(
            x => x.GetValuesFromFile(fileName),
            Times.Once);
    }

    [Fact]
    public async Task GetValues_ShouldPassFileNameToService()
    {
        // Arrange
        const string fileName = "my-test";

        _valuesServiceMock
            .Setup(x => x.GetValuesFromFile(fileName))
            .ReturnsAsync(new List<ValueDTO>());

        // Act
        await _controller.GetValues(fileName);

        // Assert
        _valuesServiceMock.Verify(
            x => x.GetValuesFromFile(fileName),
            Times.Once);
    }

    [Fact]
    public async Task Upload_ShouldReturnOk()
    {
        // Arrange
        var file = CreateFile("test.csv");

        _valuesServiceMock
            .Setup(x => x.SaveValuesFromFile(file))
            .Returns(Task.CompletedTask);

        // Act
        var response = await _controller.Upload(file);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(response);

        Assert.Equal("File processed!", okResult.Value);

        _valuesServiceMock.Verify(
            x => x.SaveValuesFromFile(file),
            Times.Once);
    }

    [Fact]
    public async Task Upload_ShouldPassFileToService()
    {
        // Arrange
        var file = CreateFile("test.csv");

        _valuesServiceMock
            .Setup(x => x.SaveValuesFromFile(file))
            .Returns(Task.CompletedTask);

        // Act
        await _controller.Upload(file);

        // Assert
        _valuesServiceMock.Verify(
            x => x.SaveValuesFromFile(file),
            Times.Once);
    }

    [Fact]
    public async Task Upload_ShouldPropagateExceptionFromService()
    {
        // Arrange
        var file = CreateFile("invalid.csv");

        _valuesServiceMock
            .Setup(x => x.SaveValuesFromFile(file))
            .ThrowsAsync(new InvalidOperationException("Invalid file"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _controller.Upload(file));

        Assert.Equal("Invalid file", exception.Message);

        _valuesServiceMock.Verify(
            x => x.SaveValuesFromFile(file),
            Times.Once);
    }

    [Fact]
    public async Task GetValues_ShouldPropagateExceptionFromService()
    {
        // Arrange
        const string fileName = "test";

        _valuesServiceMock
            .Setup(x => x.GetValuesFromFile(fileName))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _controller.GetValues(fileName));

        Assert.Equal("Database error", exception.Message);

        _valuesServiceMock.Verify(
            x => x.GetValuesFromFile(fileName),
            Times.Once);
    }

    private static IFormFile CreateFile(string fileName)
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