using Microsoft.AspNetCore.SignalR;

public class AstronautDetailDto
{
    public string Name { get; set; } = null!;
    public string CurrentRank { get; set; } = "";
    public string CurrentDutyTitle { get; set; } = "";
    public DateTime CareerStartDate { get; set; }

    public DateTime? CareerEndDate { get; set; }
}