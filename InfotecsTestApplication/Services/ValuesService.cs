using System.Security.Cryptography.Xml;
using InfotecsTestApplication.Data;
using InfotecsTestApplication.DTO;
using InfotecsTestApplication.Models.Entity;
using InfotecsTestApplication.Services.Interfaces;
using InfotecsTestApplication.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace InfotecsTestApplication.Services;

public class ValuesService : IValuesService
{
    private readonly IFileProcessingService _fileProcessingService;
    private readonly IResultService _resultService;
    private readonly AppDbContext _dbContext;

    public ValuesService(
        IFileProcessingService fileProcessingService,
        AppDbContext dbContext,
        IResultService resultService)
    {
        _fileProcessingService = fileProcessingService;
        _dbContext = dbContext;
        _resultService = resultService;
    }

    public async Task<List<ValueModel>> GetValues()
    {
        return await _dbContext.Values
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task SaveValuesFromFile(IFormFile file)
    {
        var values = await _fileProcessingService.ParseCSVAsync(file);

        _fileProcessingService.ValidateNewData(values);

        var fileName = file.FileName;

        await using var transaction =
            await _dbContext.Database.BeginTransactionAsync();

        try
        {
            var result = await _dbContext.Results
                .Include(x => x.Values)
                .FirstOrDefaultAsync(x => x.Name == fileName);

            if (result == null)
            {
                result = _resultService.CalculateResult(values, fileName);

                await _dbContext.Results.AddAsync(result);
            }
            else
            {
                _dbContext.Values.RemoveRange(result.Values);

                var newResult = _resultService.CalculateResult(values, fileName);

                result.StartedAt = newResult.StartedAt;
                result.DurationSecons = newResult.DurationSecons;
                result.AverageExecutionTime = newResult.AverageExecutionTime;
                result.AverageValue = newResult.AverageValue;
                result.MedianValue = newResult.MedianValue;
                result.MaxValue = newResult.MaxValue;
                result.MinValue = newResult.MinValue;

                result.Values = values;
            }

            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<ValueDTO>> GetValuesFromFile(string file)
    {
        var query = _dbContext.Values.AsQueryable();
        var result = await query.Include(x=> x.Result)
            .Where(x=> x.Result.Name.Contains(file))
            .OrderByDescending(x=> x.Date)
            .Take(10)
            .Select(x=> x.ToDto())
            .ToListAsync();
        return result;
    }
}