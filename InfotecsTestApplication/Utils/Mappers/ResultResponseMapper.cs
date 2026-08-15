using InfotecsTestApplication.DTO;
using InfotecsTestApplication.Models.Entity;

namespace InfotecsTestApplication.Utils.Mappers;

public static class ResultResponseMapper
{
    public static ResultResponseDTO ToDto(this ResultModel resultModel)
    {
        return new ResultResponseDTO
        {
            Name = resultModel.Name,
            DurationSeconds = resultModel.DurationSecons,
            StartedAt = DateTimeOffset.Now,
            AverageExecutionTime = resultModel.AverageExecutionTime,
            AverageValue = resultModel.AverageValue,
            MaxValue = resultModel.MaxValue,
            MinValue = resultModel.MinValue,
            MedianValue = resultModel.MedianValue
        };
    }
}