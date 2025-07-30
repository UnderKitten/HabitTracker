using HabitTracker.Data;
using HabitTracker.Model;

namespace HabitTracker.Helpers;

public static class HabitHelper
{
    public static async Task<(Habit?, IResult?)> FindHabitOrNotFoundAsync(HabitContext db, int id)
    {
        var habit = await db.Habits.FindAsync(id);
        if (habit == null)
        {
            return (null, Results.NotFound("Habit not found"));
        }
        return (habit, null);
    }
}