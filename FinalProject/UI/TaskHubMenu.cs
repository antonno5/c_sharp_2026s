using FinalProject.Helpers;
using FinalProject.Models;
using FinalProject.Services;

namespace FinalProject.UI;

public class TaskHubMenu
{
    private readonly TaskRepository _repository;
    private readonly TaskFileService _fileService;
    private readonly DeadlineMonitorService _deadlineMonitor;

    public TaskHubMenu(TaskRepository repository, TaskFileService fileService, DeadlineMonitorService deadlineMonitor)
    {
        _repository = repository;
        _fileService = fileService;
        _deadlineMonitor = deadlineMonitor;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        _deadlineMonitor.Start();

        bool running = true;
        while (running && !cancellationToken.IsCancellationRequested)
        {
            PrintMenu();
            Console.Write("Выберите пункт: ");
            string? choice = Console.ReadLine();

            try
            {
                running = await HandleChoiceAsync(choice, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            if (running)
            {
                Console.WriteLine();
                Console.WriteLine("Нажмите Enter для продолжения...");
                Console.ReadLine();
            }
        }
    }

    private static void PrintMenu()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine("║           TaskHub — менеджер задач   ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.WriteLine();
        Console.WriteLine(" 1. Создать задачу");
        Console.WriteLine(" 2. Показать все задачи");
        Console.WriteLine(" 3. Показать выполненные");
        Console.WriteLine(" 4. Показать невыполненные");
        Console.WriteLine(" 5. Показать с высоким приоритетом");
        Console.WriteLine(" 6. Редактировать задачу");
        Console.WriteLine(" 7. Удалить задачу");
        Console.WriteLine(" 8. Поиск по названию");
        Console.WriteLine(" 9. Поиск по статусу");
        Console.WriteLine("10. Поиск по приоритету");
        Console.WriteLine("11. Статистика");
        Console.WriteLine("12. Сохранить в файл");
        Console.WriteLine("13. Загрузить из файла");
        Console.WriteLine(" 0. Выход");
        Console.WriteLine();
    }

    private async Task<bool> HandleChoiceAsync(string? choice, CancellationToken cancellationToken)
    {
        switch (choice)
        {
            case "1":
                CreateTask();
                break;
            case "2":
                ConsoleInputHelper.PrintTasks(_repository.All, "Все задачи");
                break;
            case "3":
                ConsoleInputHelper.PrintTasks(
                    _repository.Filter(t => t.Status == WorkStatus.Done),
                    "Выполненные задачи");
                break;
            case "4":
                ConsoleInputHelper.PrintTasks(
                    _repository.Filter(t => t.Status != WorkStatus.Done),
                    "Невыполненные задачи");
                break;
            case "5":
                ConsoleInputHelper.PrintTasks(
                    _repository.Filter(t => t.Priority == TaskPriority.High),
                    "Задачи с высоким приоритетом");
                break;
            case "6":
                EditTask();
                break;
            case "7":
                DeleteTask();
                break;
            case "8":
                SearchByTitle();
                break;
            case "9":
                SearchByStatus();
                break;
            case "10":
                SearchByPriority();
                break;
            case "11":
                TaskStatisticsService.PrintStatistics(_repository.All);
                break;
            case "12":
                await SaveTasksAsync(cancellationToken);
                break;
            case "13":
                await LoadTasksAsync(cancellationToken);
                break;
            case "0":
                return false;
            default:
                Console.WriteLine("Неизвестный пункт меню.");
                break;
        }

        return true;
    }

    private void CreateTask()
    {
        string title = ConsoleInputHelper.ReadRequired("Название: ");
        string description = ConsoleInputHelper.ReadRequired("Описание: ");
        TaskPriority priority = ConsoleInputHelper.ReadPriority();
        DateTime deadline = ConsoleInputHelper.ReadDeadline();
        WorkStatus status = ConsoleInputHelper.ReadStatus();

        TaskItem task = _repository.Create(title, description, priority, deadline, status);
        Console.WriteLine($"Задача создана. ID: {task.Id}");
    }

    private void EditTask()
    {
        Guid id = ConsoleInputHelper.ReadTaskId();
        TaskItem? task = _repository.GetById(id);
        if (task is null)
        {
            Console.WriteLine("Задача не найдена.");
            return;
        }

        Console.WriteLine($"Редактирование: {task.Title}");
        Console.WriteLine("Оставьте поле пустым, чтобы не менять значение.");

        string title = ReadOptional("Новое название: ", task.Title);
        string description = ReadOptional("Новое описание: ", task.Description);

        Console.Write("Новый приоритет (Low/Medium/High, Enter — без изменений): ");
        string? priorityInput = Console.ReadLine();
        TaskPriority priority = string.IsNullOrWhiteSpace(priorityInput)
            ? task.Priority
            : Enum.Parse<TaskPriority>(priorityInput, ignoreCase: true);

        Console.Write("Новый статус (New/InProgress/Done, Enter — без изменений): ");
        string? statusInput = Console.ReadLine();
        WorkStatus status = string.IsNullOrWhiteSpace(statusInput)
            ? task.Status
            : Enum.Parse<WorkStatus>(statusInput, ignoreCase: true);

        bool updated = _repository.Update(id, t =>
        {
            t.Title = title;
            t.Description = description;
            t.Priority = priority;
            t.Status = status;
        });

        Console.WriteLine(updated ? "Задача обновлена." : "Не удалось обновить задачу.");
    }

    private static string ReadOptional(string prompt, string current)
    {
        Console.Write(prompt);
        string? value = Console.ReadLine();
        return string.IsNullOrWhiteSpace(value) ? current : value.Trim();
    }

    private void DeleteTask()
    {
        Guid id = ConsoleInputHelper.ReadTaskId();
        bool deleted = _repository.Delete(id);
        Console.WriteLine(deleted ? "Задача удалена." : "Задача не найдена.");
    }

    private void SearchByTitle()
    {
        string query = ConsoleInputHelper.ReadRequired("Фрагмент названия: ");
        ConsoleInputHelper.PrintTasks(_repository.SearchByTitle(query), "Результаты поиска");
    }

    private void SearchByStatus()
    {
        WorkStatus status = ConsoleInputHelper.ReadStatus();
        ConsoleInputHelper.PrintTasks(_repository.SearchByStatus(status), "Результаты поиска");
    }

    private void SearchByPriority()
    {
        TaskPriority priority = ConsoleInputHelper.ReadPriority();
        ConsoleInputHelper.PrintTasks(_repository.SearchByPriority(priority), "Результаты поиска");
    }

    private async Task SaveTasksAsync(CancellationToken cancellationToken)
    {
        await _fileService.SaveAsync(_repository.All, cancellationToken);
        Console.WriteLine($"Задачи сохранены в {_fileService.FilePath}");
    }

    private async Task LoadTasksAsync(CancellationToken cancellationToken)
    {
        List<TaskItem> tasks = await _fileService.LoadAsync(cancellationToken);
        _repository.LoadFrom(tasks);
        Console.WriteLine($"Загружено задач: {tasks.Count}");
    }
}
