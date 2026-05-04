using System;
using System.Collections.Generic;

public interface IEntity
{
    int Id { get; }
}

public class Repository<T> where T : IEntity
{
    private readonly Dictionary<int, T> _items = new();

    public int Count => _items.Count;

    public void Add(T item)
    {
        if (_items.ContainsKey(item.Id))
        {
            throw new InvalidOperationException($"Элемент с Id={item.Id} уже существует.");
        }

        _items[item.Id] = item;
    }

    public bool Remove(int id)
    {
        return _items.Remove(id);
    }

    public T? GetById(int id)
    {
        return _items.TryGetValue(id, out T? item) ? item : default;
    }

    public IReadOnlyList<T> GetAll()
    {
        return new List<T>(_items.Values);
    }

    public IReadOnlyList<T> Find(Predicate<T> predicate)
    {
        var result = new List<T>();

        foreach (T item in _items.Values)
        {
            if (predicate(item))
            {
                result.Add(item);
            }
        }

        return result;
    }
}

public sealed class Product : IEntity
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }

    public override string ToString()
    {
        return $"{Id}: {Name} ({Price})";
    }
}

public sealed class User : IEntity
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int Age { get; init; }

    public override string ToString()
    {
        return $"{Id}: {Name}, {Age} лет";
    }
}

public static class CollectionUtils
{
    public static List<T> Distinct<T>(List<T> source)
    {
        var seen = new HashSet<T>();
        var result = new List<T>();

        foreach (T item in source)
        {
            if (seen.Add(item))
            {
                result.Add(item);
            }
        }

        return result;
    }

    public static Dictionary<TKey, List<TValue>> GroupBy<TValue, TKey>(
        List<TValue> source,
        Func<TValue, TKey> keySelector) where TKey : notnull
    {
        var result = new Dictionary<TKey, List<TValue>>();

        foreach (TValue item in source)
        {
            TKey key = keySelector(item);
            if (!result.TryGetValue(key, out List<TValue>? bucket))
            {
                bucket = new List<TValue>();
                result[key] = bucket;
            }

            bucket.Add(item);
        }

        return result;
    }

    public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
        Dictionary<TKey, TValue> first,
        Dictionary<TKey, TValue> second,
        Func<TValue, TValue, TValue> conflictResolver) where TKey : notnull
    {
        var result = new Dictionary<TKey, TValue>(first);

        foreach (KeyValuePair<TKey, TValue> pair in second)
        {
            if (result.TryGetValue(pair.Key, out TValue? existingValue))
            {
                result[pair.Key] = conflictResolver(existingValue, pair.Value);
            }
            else
            {
                result[pair.Key] = pair.Value;
            }
        }

        return result;
    }

    public static T MaxBy<T, TKey>(List<T> source, Func<T, TKey> selector)
        where TKey : IComparable<TKey>
    {
        if (source.Count == 0)
        {
            throw new InvalidOperationException("Коллекция пуста.");
        }

        T maxItem = source[0];
        TKey maxKey = selector(maxItem);

        for (int i = 1; i < source.Count; i++)
        {
            T currentItem = source[i];
            TKey currentKey = selector(currentItem);

            if (currentKey.CompareTo(maxKey) > 0)
            {
                maxItem = currentItem;
                maxKey = currentKey;
            }
        }

        return maxItem;
    }
}
