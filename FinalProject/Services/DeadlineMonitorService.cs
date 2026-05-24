using FinalProject.Models;

namespace FinalProject.Services;

public sealed class DeadlineMonitorService : IDisposable
{
    private readonly Func<IReadOnlyList<TaskItem>> _tasksProvider;
    private readonly Action<string> _notify;
    private readonly TimeSpan _interval;
    private readonly HashSet<Guid> _notified = [];
    private CancellationTokenSource? _cts;
    private Task? _worker;

    public DeadlineMonitorService(
        Func<IReadOnlyList<TaskItem>> tasksProvider,
        Action<string> notify,
        TimeSpan interval)
    {
        _tasksProvider = tasksProvider;
        _notify = notify;
        _interval = interval;
    }

    public void Start()
    {
        if (_worker is not null)
        {
            return;
        }

        _cts = new CancellationTokenSource();
        _worker = Task.Run(() => MonitorLoopAsync(_cts.Token));
    }

    public void Stop()
    {
        _cts?.Cancel();
        try
        {
            _worker?.Wait(TimeSpan.FromSeconds(2));
        }
        catch (AggregateException)
        {
            // ignored on shutdown
        }

        _worker = null;
        _cts?.Dispose();
        _cts = null;
    }

    private async Task MonitorLoopAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                CheckDeadlines();
            }
            catch (Exception ex)
            {
                _notify($"[Монитор] Ошибка проверки: {ex.Message}");
            }

            try
            {
                await Task.Delay(_interval, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                break;
            }
        }
    }

    private void CheckDeadlines()
    {
        IReadOnlyList<TaskItem> tasks = _tasksProvider();
        DateTime now = DateTime.Now;

        foreach (TaskItem task in tasks)
        {
            if (!task.IsOverdue(now) || _notified.Contains(task.Id))
            {
                continue;
            }

            _notified.Add(task.Id);
            _notify($"⚠ Просрочена задача: \"{task.Title}\" (дедлайн {task.Deadline:dd.MM.yyyy HH:mm})");
        }

        foreach (TaskItem task in tasks.Where(t => t.Status == WorkStatus.Done))
        {
            _notified.Remove(task.Id);
        }
    }

    public void Dispose()
    {
        Stop();
        GC.SuppressFinalize(this);
    }
}
