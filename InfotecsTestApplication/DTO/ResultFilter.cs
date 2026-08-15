namespace InfotecsTestApplication.DTO;

public class ResultFilter
{
    public string? Name { get; set; }
    
    public DateTimeOffset? DateFrom { get; set; }
    public DateTimeOffset? DateTo { get; set; }
    
    public double? valueFrom { get; set; }
    public double? valueTo { get; set; }
    
    public double? executionTimeFrom { get; set; }
    public double? executionTimeTo { get; set; }
}