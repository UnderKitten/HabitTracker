import axios from 'axios';

export interface Habit {
    id: number;
    userId?: string;
    title: string;
    description?: string;
    createdAt: string; 
}

const baseUrl = "https://localhost:7113/habits";

/**
 * Fetch all habits from the backend API.
 */
export const getHabits = async (): Promise<Habit[]> => {
    const response = await axios.get(baseUrl);
    return response.data;
};

// export const addHabit = async (
//     habit: Omit<Habit, "id" | "created">
// ): Promise<Habit> => {
//     const response = await axios.post(baseUrl, {
//         ...habit,
//         created: new Date().toISOString(),
//     });
//     return response.data;
// };

export const addHabit = async (habit: {
    title: string;
    description?: string;
    userId?: string;
    createdAt: string; // ISO string
}) => {
    const res = await axios.post(baseUrl, habit);
    return res.data;
};