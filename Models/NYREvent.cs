namespace nyr_api.Models;

public class NYREvent
{
    public long Id { get; set; } //eventId
    public required string Username {get; set; }
    public required string GoalText { get; set; }
    public required DateTime StartDate {get; set; }
    public required DateTime EndDate {get; set; }
    public required bool CheckIn { get; set; }
    
}