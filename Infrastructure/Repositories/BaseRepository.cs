using System.Text.Json;

namespace Student_Library_Assistant.Infrastructure.Repositories;

public abstract class BaseRepository<T>
{
    protected List<T> Items = new(); // Items from Book, Loan, Student type

    protected BaseRepository()
    {
        Load();
    }

    protected abstract string FilePath { get; }

    protected void Load()
    {
        try
        {
            if (!File.Exists(FilePath))
            {
                Items = new List<T>();
                return;
            }

            var json = File.ReadAllText(FilePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                Items = new List<T>();
                return;
            }

            Items = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>(); // create empty list if none found
        }
        catch (JsonException)
        {
            Items = new List<T>();
        }
    }

    protected void Save()
    {
        var json = JsonSerializer.Serialize(
            Items,
            new JsonSerializerOptions { WriteIndented = true } // use JSON pretty printing
        );

        File.WriteAllText(FilePath, json);
    }

    public virtual void Add(T itemToAdd)
    {
        Items.Add(itemToAdd);
        Save();
    }

    public virtual void Delete(Func<T, bool> predicate)
    {
        Items.RemoveAll(i => predicate(i));
        Save();
    }

    public virtual void Update(Func<T, bool> predicate, T newItem)
    {
        var index = Items.FindIndex(i => predicate(i));
        if (index == -1)
            throw new Exception("Item has not been found");

        Items[index] = newItem;
        Save();
    }

    public IReadOnlyList<T> GetAll()
    {
        return Items.AsReadOnly();
    }
}