namespace HabitTracker.Model;

/// <summary>
/// Instance of habit
/// </summary>
public class Habit
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Instance of record of the habit
/// </summary>
public class HabitEntry
{
    public int Id { get; set; }
    public int HabitId { get; set; }
    public DateTime Date { get; set; }
    public bool Done { get; set; }
}

