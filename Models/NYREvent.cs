namespace nyr_api_serverless.Models;

public class NYREvent
{
    public long Id { get; set; } //eventId
    public string Username {get; set; }
    public  string GoalText { get; set; }
    public  DateTime StartDate {get; set; }
    public  DateTime EndDate {get; set; }
    public bool CheckIn { get; set; }
    
}