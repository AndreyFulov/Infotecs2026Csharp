using InfotecsTestApplication.Models.Entity;

namespace InfotecsTestApplication.Services.Interfaces;

public interface IFileProcessingService
{
    public Task<List<ValueModel>> ParseCSVAsync(IFormFile csv);
    public void ValidateNewData(List<ValueModel> results);
}