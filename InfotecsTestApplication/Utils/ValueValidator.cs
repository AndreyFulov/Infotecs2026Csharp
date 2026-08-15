using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
using InfotecsTestApplication.Models.Entity;

namespace InfotecsTestApplication.Utils;

public class ValueValidator
{
    private static readonly DateTime DATE_FROM = new DateTime(2000, 1, 1);
    private static readonly int ROWS_MIN = 1;
    private static readonly int ROWS_MAX = 10000;

    public static void ValidateNewData(IEnumerable<ValueModel> values)
    {
        if (values.Count()  < ROWS_MIN || values.Count() > ROWS_MAX)
        {
            throw new ValidationException();
        }
        foreach (var result in values)
        {
            ValidateDate(result);
            ValidateExecutionTime(result);
            ValidateValue(result);
        }
    }

    private static void ValidateDate(ValueModel result)
    {
        var now = DateTime.Now;
        if (result.Date < DATE_FROM || result.Date > now)
        {
            throw new ValidationException($"Date must be in range of: ${DATE_FROM.ToString("dd/MM/yyyy")} - ${now.ToString("dd/MM/yyyy")}");
        }
    }

    private static void ValidateValue(ValueModel result)
    {
        if (result.Value < 0)
        {
            throw new ValidationException("Value must be greater than or equal to zero");
        }
    }

    private static void ValidateExecutionTime(ValueModel result)
    {
        if (result.ExecutionTime < 0)
        {
            throw new ValidationException("Execution time must be greater than or equal to zero");
        }
    }
}