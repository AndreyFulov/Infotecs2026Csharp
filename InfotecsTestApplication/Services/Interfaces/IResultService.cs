using InfotecsTestApplication.DTO;
using InfotecsTestApplication.Models.Entity;

namespace InfotecsTestApplication.Services.Interfaces;

public interface IResultService
{
    public ResultModel CalculateResult(List<ValueModel> values, string fileName);
    public Task<List<ResultResponseDTO>> GetResults(ResultFilter filter);
}