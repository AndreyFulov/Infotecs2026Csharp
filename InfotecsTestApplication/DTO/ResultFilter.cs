namespace InfotecsTestApplication.DTO;

public class ResultFilter
{
    public string? name { get; set; }
    
    public DateTimeOffset? dateFrom { get; set; }
    public DateTimeOffset? dateTo { get; set; }
    
    public double? valueFrom { get; set; }
    public double? valueTo { get; set; }
    
    public double? executionTimeFrom { get; set; }
    public double? executionTimeTo { get; set; }
}