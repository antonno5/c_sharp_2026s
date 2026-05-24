namespace FinalProject.Models;

public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public TaskPriority Priority { get; set; }

    public DateTime Deadline { get; set; }

    public WorkStatus Status { get; set; }

    public bool IsOverdue(DateTime? now = null)
    {
        DateTime moment = now ?? DateTime.Now;
        return Status != WorkStatus.Done && Deadline < moment;
    }

    public override string ToString()
    {
        string overdueMark = IsOverdue() ? " [ПРОСРОЧЕНА]" : string.Empty;
        return $"[{Id:N}] {Title} | {Priority} | {Status} | до {Deadline:dd.MM.yyyy HH:mm}{overdueMark}\n  {Description}";
    }
}
