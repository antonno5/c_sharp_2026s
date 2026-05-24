using FinalProject.Core;
using FinalProject.Models;

namespace FinalProject.Services;

public class TaskRepository
{
    private readonly GenericStore<TaskItem> _store = new();
    private readonly Dictionary<Guid, TaskItem> _index = new();

    public IReadOnlyList<TaskItem> All => _store.Items;

    public TaskItem Create(string title, string description, TaskPriority priority, DateTime deadline, WorkStatus status)
    {
        var task = new TaskItem
        {
            Title = title,
            Description = description,
            Priority = priority,
            Deadline = deadline,
            Status = status
        };

        _store.Add(task);
        _index[task.Id] = task;
        return task;
    }

    public TaskItem? GetById(Guid id) => _index.GetValueOrDefault(id);

    public bool Update(Guid id, Action<TaskItem> updateAction)
    {
        TaskItem? task = GetById(id);
        if (task is null)
        {
            return false;
        }

        updateAction(task);
        return true;
    }

    public bool Delete(Guid id)
    {
        TaskItem? task = GetById(id);
        if (task is null)
        {
            return false;
        }

        _index.Remove(id);
        return _store.Remove(task);
    }

    public List<TaskItem> Filter(TaskFilterDelegate predicate) =>
        _store.FindAll(task => predicate(task));

    public List<TaskItem> SearchByTitle(string query) =>
        Filter(task => task.Title.Contains(query, StringComparison.OrdinalIgnoreCase));

    public List<TaskItem> SearchByStatus(WorkStatus status) =>
        Filter(task => task.Status == status);

    public List<TaskItem> SearchByPriority(TaskPriority priority) =>
        Filter(task => task.Priority == priority);

    public void LoadFrom(IEnumerable<TaskItem> tasks)
    {
        _store.ReplaceAll(tasks);
        _index.Clear();
        foreach (TaskItem task in _store.Items)
        {
            _index[task.Id] = task;
        }
    }
}
