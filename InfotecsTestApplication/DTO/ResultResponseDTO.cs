namespace InfotecsTestApplication.DTO;

public class ResultResponseDTO
{
    public string Name { get; set; }
    public long DurationSeconds  { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public double AverageValue { get; set; }
    public double AverageExecutionTime { get; set; }
    public double MedianValue { get; set; }
    public double MaxValue { get; set; }
    public double MinValue { get; set; }
}