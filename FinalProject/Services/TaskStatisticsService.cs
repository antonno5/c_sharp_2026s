using FinalProject.Models;

namespace FinalProject.Services;

public static class TaskStatisticsService
{
    public static void PrintStatistics(IReadOnlyList<TaskItem> tasks)
    {
        int total = tasks.Count;
        int completed = tasks.Count(t => t.Status == WorkStatus.Done);
        int overdue = tasks.Count(t => t.IsOverdue());
        Dictionary<TaskPriority, int> byPriority = BuildPriorityStats(tasks);

        Console.WriteLine();
        Console.WriteLine("=== Статистика ===");
        Console.WriteLine($"Всего задач: {total}");
        Console.WriteLine($"Выполнено: {completed}");
        Console.WriteLine($"Просрочено: {overdue}");
        Console.WriteLine("По приоритетам:");
        foreach (KeyValuePair<TaskPriority, int> pair in byPriority)
        {
            Console.WriteLine($"  {pair.Key}: {pair.Value}");
        }

        Console.WriteLine();
    }

    public static Dictionary<TaskPriority, int> BuildPriorityStats(IReadOnlyList<TaskItem> tasks)
    {
        var stats = new Dictionary<TaskPriority, int>
        {
            [TaskPriority.Low] = 0,
            [TaskPriority.Medium] = 0,
            [TaskPriority.High] = 0
        };

        foreach (TaskItem task in tasks)
        {
            stats[task.Priority]++;
        }

        return stats;
    }
}
