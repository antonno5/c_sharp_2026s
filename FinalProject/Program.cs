using FinalProject.Models;
using FinalProject.Services;
using FinalProject.UI;

const string dataFile = "tasks.json";

var repository = new TaskRepository();
using var fileService = new TaskFileService(dataFile);
using var deadlineMonitor = new DeadlineMonitorService(
    () => repository.All,
    message => Console.WriteLine($"\n{message}"),
    TimeSpan.FromSeconds(5));

var menu = new TaskHubMenu(repository, fileService, deadlineMonitor);

Console.OutputEncoding = System.Text.Encoding.UTF8;

try
{
    if (File.Exists(dataFile))
    {
        List<TaskItem> existing = await fileService.LoadAsync();
        repository.LoadFrom(existing);
        Console.WriteLine($"Загружено {existing.Count} задач из {dataFile}.");
        Console.WriteLine("Нажмите Enter для входа в меню...");
        Console.ReadLine();
    }

    await menu.RunAsync();
    await fileService.SaveAsync(repository.All);
    Console.WriteLine("Данные сохранены. До свидания!");
}
catch (Exception ex)
{
    Console.WriteLine($"Критическая ошибка: {ex.Message}");
}
