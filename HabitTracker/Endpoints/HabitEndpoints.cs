using HabitTracker.Data;
using HabitTracker.Model;
using HabitTracker.Helpers;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Endpoints;

public static class HabitEndpoints
{
    public static void MapHabitsEndpoints(this IEndpointRouteBuilder routes)
    {
        // Get all habits
        routes.MapGet("/habits", async (HabitContext db) => await db.Habits.ToListAsync());

        // Add a habit to the database
        routes.MapPost("/habits", async (Habit habit, HabitContext db) =>
        {
            db.Habits.Add(habit);
            await db.SaveChangesAsync();
            return Results.Created($"/habits/{habit.Id}", habit);
        });

        // Get a single habit by id
        routes.MapGet("/habits/{id:int}", async (int id, HabitContext db) =>
            await db.Habits.FindAsync(id) is Habit habit
                ? Results.Ok(habit)
                : Results.NotFound());

        // Update an existing habit 
        routes.MapPut("/habits/{id:int}", async (int id, Habit updatedHabit, HabitContext db) =>
        {
            var (habit, error) = await HabitHelper.FindHabitOrNotFoundAsync(db, id);
            if (error != null)
            {
                return error;
            }

            habit.Title = updatedHabit.Title;
            habit.Description = updatedHabit.Description;
            habit.UserId = updatedHabit.UserId;

            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        // Delete a habit
        routes.MapDelete("/habits/{id:int}", async (int id, HabitContext db) =>
        {
            var (habit, error) = await HabitHelper.FindHabitOrNotFoundAsync(db, id);
            if (error != null)
            {
                return error;
            }

            db.Habits.Remove(habit);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        // Add a habit entry
        routes.MapPost("/habits/{id:int}/entries", async (int id, HabitEntry habitEntry, HabitContext db) =>
        {
            var (habit, error) = await HabitHelper.FindHabitOrNotFoundAsync(db, id);
            if (error != null)
            {
                return error;
            }

            DateTime entryDate = habitEntry.Date.Date;

            bool alreadyMarkedDone = await db.HabitEntries.AnyAsync(e =>
                e.HabitId == id && e.Date.Date == entryDate);

            if (alreadyMarkedDone)
            {
                return Results.Conflict($"Habit entry for {entryDate.ToShortDateString()} already exists.");
            }

            habitEntry.HabitId = id;
            habitEntry.Date = entryDate;
            db.HabitEntries.Add(habitEntry);
            await db.SaveChangesAsync();

            return Results.Created($"/habits/{id}/entries/{habitEntry.Id}", habitEntry);
        });
        
        // Get all entries for a habit
        routes.MapGet("/habits/{id:int}/entries", async (int id, HabitContext db) =>
            await db.HabitEntries.Where(e => e.HabitId == id).ToListAsync());
    }
}