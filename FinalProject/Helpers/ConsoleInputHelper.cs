using FinalProject.Models;

namespace FinalProject.Helpers;

public static class ConsoleInputHelper
{
    public static string ReadRequired(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? value = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            Console.WriteLine("Значение не может быть пустым.");
        }
    }

    public static DateTime ReadDeadline(string prompt = "Дедлайн (дд.мм.гггг чч:мм): ")
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (DateTime.TryParse(input, out DateTime deadline))
            {
                return deadline;
            }

            Console.WriteLine("Неверный формат даты. Пример: 25.05.2026 18:00");
        }
    }

    public static TaskPriority ReadPriority(string prompt = "Приоритет (Low/Medium/High): ")
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (Enum.TryParse(input, ignoreCase: true, out TaskPriority priority))
            {
                return priority;
            }

            Console.WriteLine("Допустимые значения: Low, Medium, High");
        }
    }

    public static WorkStatus ReadStatus(string prompt = "Статус (New/InProgress/Done): ")
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (Enum.TryParse(input, ignoreCase: true, out WorkStatus status))
            {
                return status;
            }

            Console.WriteLine("Допустимые значения: New, InProgress, Done");
        }
    }

    public static Guid ReadTaskId(string prompt = "ID задачи: ")
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (Guid.TryParse(input, out Guid id))
            {
                return id;
            }

            Console.WriteLine("Неверный формат ID.");
        }
    }

    public static void PrintTasks(IEnumerable<TaskItem> tasks, string header)
    {
        var list = tasks.ToList();
        Console.WriteLine();
        Console.WriteLine($"=== {header} ({list.Count}) ===");
        if (list.Count == 0)
        {
            Console.WriteLine("Задач не найдено.");
            return;
        }

        foreach (TaskItem task in list)
        {
            Console.WriteLine(task);
            Console.WriteLine();
        }
    }
}
