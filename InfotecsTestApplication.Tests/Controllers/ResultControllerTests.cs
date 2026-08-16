using InfotecsTestApplication.Controllers;
using InfotecsTestApplication.DTO;
using InfotecsTestApplication.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InfotecsTestApplication.Tests.Controllers;

public class ResultsControllerTests
{
    private readonly Mock<IResultService> _resultServiceMock;
    private readonly ResultsController _controller;

    public ResultsControllerTests()
    {
        _resultServiceMock = new Mock<IResultService>();

        _controller = new ResultsController(
            _resultServiceMock.Object);
    }

    [Fact]
    public async Task GetResults_ShouldReturnOk()
    {
        // Arrange
        var filter = new ResultFilter
        {
            name = "test"
        };

        var results = new List<ResultResponseDTO>
        {
            new()
            {
                Name = "test.csv"
            }
        };

        _resultServiceMock
            .Setup(x => x.GetResults(filter))
            .ReturnsAsync(results);

        // Act
        var response = await _controller.GetResults(filter);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(response.Result);

        var actualResults =
            Assert.IsType<List<ResultResponseDTO>>(okResult.Value);

        Assert.Single(actualResults);
        Assert.Equal("test.csv", actualResults[0].Name);
    }

    [Fact]
    public async Task GetResults_ShouldReturnEmptyList_WhenNoResultsFound()
    {
        // Arrange
        var filter = new ResultFilter
        {
            name = "unknown"
        };

        _resultServiceMock
            .Setup(x => x.GetResults(filter))
            .ReturnsAsync(new List<ResultResponseDTO>());

        // Act
        var response = await _controller.GetResults(filter);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(response.Result);

        var results =
            Assert.IsType<List<ResultResponseDTO>>(okResult.Value);

        Assert.Empty(results);
    }

    [Fact]
    public async Task GetResults_ShouldPassFilterToService()
    {
        // Arrange
        var filter = new ResultFilter
        {
            name = "test",
            valueFrom = 10,
            valueTo = 100
        };

        _resultServiceMock
            .Setup(x => x.GetResults(filter))
            .ReturnsAsync(new List<ResultResponseDTO>());

        // Act
        await _controller.GetResults(filter);

        // Assert
        _resultServiceMock.Verify(
            x => x.GetResults(filter),
            Times.Once);
    }
}