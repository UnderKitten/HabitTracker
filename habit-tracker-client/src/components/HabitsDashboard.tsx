import React, {useState, useEffect} from "react";
import {getHabits, addHabit, Habit} from "../habits";

export default function HabitDashboard() {
    const [habits, setHabits] = useState<Habit[]>([]);
    const [newHabitTitle, setNewHabitTitle] = useState("");
    const [newHabitDescription, setNewHabitDescription] = useState("");
    const [newHabitCreatedAt, setNewHabitCreatedAt] = useState(() => {
        // default to today in YYYY-MM-DD format for date input
        return new Date().toISOString().split("T")[0];
    });

    useEffect(() => {
        async function fetchData() {
            try {
                const data = await getHabits();
                setHabits(data);
            } catch (error) {
                console.error("Error fetching habits:", error);
            }
        }

        fetchData();
    }, []);

    // on submit or add habit
    const handleAddHabit = async () => {
        if (!newHabitTitle.trim()) return;

        try {
            const added = await addHabit({
                title: newHabitTitle,
                description: newHabitDescription || undefined,
                createdAt: newHabitCreatedAt,
            });

            // Append the new habit to the habits state array
            setHabits((prevHabits) => [...prevHabits, added]);

            // Clear the input fields
            setNewHabitTitle("");
            setNewHabitDescription("");
            setNewHabitCreatedAt(new Date().toISOString().split("T")[0]);
        } catch (error) {
            console.error("Error adding habit:", error);
        }
    };

    return (
        <div>
            <h1>My Habits</h1>

            {/* Habit List */}
            <ul>
                {habits.length === 0 && <li>No habits found.</li>}
                {habits.map((habit) => (
                    <li key={habit.id}>
                        <strong>{habit.title}</strong>
                        {habit.description && ` – ${habit.description}`}
                    </li>
                ))}
            </ul>
            <input
                type="text"
                placeholder="Title"
                value={newHabitTitle}
                onChange={(e) => setNewHabitTitle(e.target.value)}
            />
            <input
                type="text"
                placeholder="Description (optional)"
                value={newHabitDescription}
                onChange={(e) => setNewHabitDescription(e.target.value)}
            />
            <input
                type="date"
                value={newHabitCreatedAt}
                onChange={(e) => setNewHabitCreatedAt(e.target.value)}
            />
            <button onClick={handleAddHabit}>Add Habit</button>
        </div>
    );
}