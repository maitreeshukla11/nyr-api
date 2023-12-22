namespace nyr_api.Models;

public class NYRItem
{
    public long Id { get; set; } //goalId
    public required string Username {get; set; }
    public required string GoalText { get; set; }
    public required string Cadence {get; set; }
    public bool[]? CheckIn { get; set; }


}