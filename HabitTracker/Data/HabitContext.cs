using HabitTracker.Model;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Data;

public class HabitContext : DbContext
{
    public HabitContext(DbContextOptions<HabitContext> options) : base(options)
    {
    }

    public DbSet<Habit> Habits { get; set; }
    public DbSet<HabitEntry> HabitEntries { get; set; }
}