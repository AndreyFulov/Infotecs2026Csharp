using InfotecsTestApplication.Data;
using InfotecsTestApplication.Models.Entity;
using InfotecsTestApplication.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InfotecsTestApplication.Services;

public class ValuesService : IValuesService
{
    private readonly IFileProcessingService _fileProcessingService;
    private readonly AppDbContext _dbContext;
    private IValuesService _valuesServiceImplementation;

    public ValuesService(IFileProcessingService fileProcessingService, AppDbContext dbContext)
    {
        _fileProcessingService = fileProcessingService;
        _dbContext = dbContext;
    }
    public async Task<List<ValueModel>> GetValues()
    {
        return await _dbContext.Values.ToListAsync();
    }

    public async Task SaveValuesFromFile(IFormFile file)
    {
        List<ValueModel> values = await _fileProcessingService.ParseCSVAsync(file);
        foreach (var value in values)
        {
            await _dbContext.Values.AddAsync(value);
        }
        await _dbContext.SaveChangesAsync();
    }
}