namespace FinalProject.Core;

public class GenericStore<T> where T : class
{
    private readonly List<T> _items = [];

    public IReadOnlyList<T> Items => _items;

    public void Add(T item) => _items.Add(item);

    public bool Remove(T item) => _items.Remove(item);

    public void Clear() => _items.Clear();

    public void ReplaceAll(IEnumerable<T> items)
    {
        _items.Clear();
        _items.AddRange(items);
    }

    public List<T> FindAll(Predicate<T> match) => _items.FindAll(match);
}
