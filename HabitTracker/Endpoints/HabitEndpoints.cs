using HabitTracker.Data;
using HabitTracker.Model;
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
        routes.MapGet("/habits/{id}", async (int id, HabitContext db) =>
            await db.Habits.FindAsync(id) is Habit habit
                ? Results.Ok(habit)
                : Results.NotFound());

        // Update an existing habit 
        routes.MapPut("/habits/{id}", async (int id, Habit updatedHabit, HabitContext db) =>
        {
            Habit habit = await db.Habits.FindAsync(id);
            if (habit == null)
            {
                return Results.NotFound();
            }

            habit.Title = updatedHabit.Title;
            habit.Description = updatedHabit.Description;
            habit.UserId = updatedHabit.UserId;

            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        // Delete a habit
        routes.MapDelete("/habits/{id}", async (int id, HabitContext db) =>
        {
            Habit habit = await db.Habits.FindAsync(id);
            if (habit == null)
            {
                return Results.NotFound();
            }

            db.Habits.Remove(habit);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
        
        // Add a habit entry
        routes.MapPost("/habits/{habitsId}/entries", async (int habitId, HabitEntry habitEntry, HabitContext db) =>
        {
            Habit habit = await db.Habits.FindAsync(habitId);
            if (habit == null)
            {
                return Results.NotFound("Habit not found");
            }
            habitEntry.HabitId = habitId;
            db.HabitEntries.Add(habitEntry);
            await db.SaveChangesAsync();
            return Results.Created($"/habits/{habitId}/entries/{habitEntry.Id}", habitEntry);
        });
    }
}