namespace InfotecsTestApplication.Models.Entity;

public class ResultModel
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public List<ValueModel> Values { get; set; } = new List<ValueModel>();
    
    public long DurationSecons { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public double AverageValue { get; set; }
    public double AverageExecutionTime { get; set; }
    public double MedianValue { get; set; }
    public double MaxValue { get; set; }
    public double MinValue { get; set; }
}