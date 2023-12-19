namespace nyr_api.Models;

public class NYRItem
{
    public long Id { get; set; } //goalId
    public required long User {get; set; }
    public required string GoalText { get; set; }
    public required string Cadence {get; set; }
    public required bool[] CheckIn { get; set; }
}
