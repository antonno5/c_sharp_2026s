using System.Text.Json;
using FinalProject.Models;

namespace FinalProject.Services;

public sealed class TaskFileService : IDisposable
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    private bool _disposed;

    public string FilePath { get; }

    public TaskFileService(string filePath)
    {
        FilePath = filePath;
    }

    public async Task SaveAsync(IEnumerable<TaskItem> tasks, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        try
        {
            string json = JsonSerializer.Serialize(tasks, JsonOptions);
            await File.WriteAllTextAsync(FilePath, json, cancellationToken);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
        {
            throw new InvalidOperationException($"Не удалось сохранить задачи в файл: {FilePath}", ex);
        }
    }

    public async Task<List<TaskItem>> LoadAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        if (!File.Exists(FilePath))
        {
            return [];
        }

        try
        {
            string json = await File.ReadAllTextAsync(FilePath, cancellationToken);
            return JsonSerializer.Deserialize<List<TaskItem>>(json, JsonOptions) ?? [];
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
        {
            throw new InvalidOperationException($"Не удалось загрузить задачи из файла: {FilePath}", ex);
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        GC.SuppressFinalize(this);
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(TaskFileService));
        }
    }
}
