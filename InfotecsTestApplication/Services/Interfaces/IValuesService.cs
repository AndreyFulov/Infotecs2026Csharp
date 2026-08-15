using InfotecsTestApplication.DTO;
using InfotecsTestApplication.Models.Entity;

namespace InfotecsTestApplication.Services.Interfaces;

public interface IValuesService
{
    public Task<List<ValueModel>> GetValues();
    public Task SaveValuesFromFile(IFormFile file);
    
    public Task<List<ValueDTO>> GetValuesFromFile(string file);
}