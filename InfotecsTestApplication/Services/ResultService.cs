using InfotecsTestApplication.Data;
using InfotecsTestApplication.DTO;
using InfotecsTestApplication.Models.Entity;
using InfotecsTestApplication.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace InfotecsTestApplication.Services.Interfaces;

public class ResultService : IResultService
{
    private readonly AppDbContext _dbContext;
    
    public ResultService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public ResultModel CalculateResult(
        List<ValueModel> values,
        string fileName)
    {
        var maxDate = values.Max(x => x.Date);
        var minDate = values.Min(x => x.Date);

        return new ResultModel
        {
            Name = fileName,
            Values = values,

            StartedAt = minDate,
            DurationSecons = CalculateDuration(minDate, maxDate),

            AverageExecutionTime = values.Average(x => x.ExecutionTime),
            AverageValue = values.Average(x => x.Value),

            MedianValue = CalculateMedian(values),

            MaxValue = values.Max(x => x.Value),
            MinValue = values.Min(x => x.Value)
        };
    }

    public async Task<List<ResultResponseDTO>> GetResults(ResultFilter filter)
    {
        var query = _dbContext.Results.AsQueryable();

        if (filter.name != null)
        {
            query = query.Where(x => x.Name.Contains(filter.name));
        }
        if (filter.dateFrom.HasValue)
        {
            query = query.Where(x =>
                x.StartedAt >= filter.dateFrom.Value);
        }

        if (filter.dateTo.HasValue)
        {
            query = query.Where(x =>
                x.StartedAt <= filter.dateTo.Value);
        }

        if (filter.valueFrom.HasValue)
        {
            query = query.Where(x =>
                x.AverageValue >= filter.valueFrom.Value);
        }

        if (filter.valueTo.HasValue)
        {
            query = query.Where(x =>
                x.AverageValue <= filter.valueTo.Value);
        }

        if (filter.executionTimeFrom.HasValue)
        {
            query = query.Where(x =>
                x.AverageExecutionTime >= filter.executionTimeFrom.Value);
        }

        if (filter.executionTimeTo.HasValue)
        {
            query = query.Where(x =>
                x.AverageValue <= filter.executionTimeTo.Value);
        }

        return await query.Select(x=>x.ToDto()).ToListAsync();
    }

    private long CalculateDuration(DateTimeOffset start, DateTimeOffset end)
    {
        return (long)(end - start).TotalSeconds;
    }
    private double CalculateMedian(List<ValueModel> values)
    {
        var sortedValues = values
            .Select(x => x.Value)
            .OrderBy(x => x)
            .ToList();

        var middle = sortedValues.Count / 2;

        return sortedValues.Count % 2 == 0
            ? (sortedValues[middle - 1] + sortedValues[middle]) / 2
            : sortedValues[middle];
    }
}