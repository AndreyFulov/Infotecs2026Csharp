using InfotecsTestApplication.DTO;
using InfotecsTestApplication.Models.Entity;

namespace InfotecsTestApplication.Utils.Mappers;

public static class ValueResponseMapper
{
    public static ValueDTO ToDto(this ValueModel value)
    {
        return new ValueDTO
        {
            Date = value.Date,
            ExecutionTime = value.ExecutionTime,
            Value = value.Value,
            resultFileName = value.Result.Name
        };
    }
}