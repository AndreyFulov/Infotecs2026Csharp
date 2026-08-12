using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using InfotecsTestApplication.DTO;
using InfotecsTestApplication.Exceptions;
using InfotecsTestApplication.Models.Entity;
using InfotecsTestApplication.Services.Interfaces;

namespace InfotecsTestApplication.Services;

public class FileProcessing : IFileProcessingService
{
    private IFileProcessingService _fileProcessingServiceImplementation;

    public async Task<List<ValueModel>> ParseCSVAsync(IFormFile csv)
    {
        const int bufferSize = 64 * 1024;

        await using var fileStream = csv.OpenReadStream();

        using var stream = new StreamReader(
            fileStream,
            bufferSize: bufferSize);

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = true,
        };
        using var csvReader = new CsvReader(stream, config);
        var results = new List<ValueModel>();

        try
        {
            await foreach (var record in csvReader.GetRecordsAsync<CsvResult>())
            {
                results.Add(new ValueModel
                {
                    Date = record.Date,
                    ExecutionTime = record.ExecutionTime,
                    Value = record.Value,
                });
            }
        }
        catch (CsvHelperException e)
        {
            throw new InvalidCsvException($"Invalid CSV format:{e.Message}");
        }

        return results;
    }

    public async Task ValidateNewData(List<ValueModel> results)
    {
        _fileProcessingServiceImplementation.ValidateNewData(results);
    }
}