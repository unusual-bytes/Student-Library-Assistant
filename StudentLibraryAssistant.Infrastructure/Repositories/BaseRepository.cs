using System.Text.Json;

namespace Student_Library_Assistant.StudentLibraryAssistant.Infrastructure.Repositories;

public abstract class BaseRepository<T>
{
    protected List<T> Items = new(); // Items from Book, Loan, Student type
    
    protected abstract string FilePath { get; }
    
    protected BaseRepository()
    {
        Load();
    }
    
    protected void Load()
    {
        if (!File.Exists(FilePath))
        {
            Items = new List<T>();
            return;
        }

        var json = File.ReadAllText(FilePath);
        Items = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>(); // create empty list if none loaded 
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
    
    public virtual void Delete(T itemToDelete)
    {
        Items.Remove(Items.FirstOrDefault(itemToDelete));
        Save();
    }
    
    public IReadOnlyList<T> GetAll()
    {
        return Items.AsReadOnly();
    }
    
}