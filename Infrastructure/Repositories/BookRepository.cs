using Student_Library_Assistant.Domain.Entities;

namespace Student_Library_Assistant.Infrastructure.Repositories;

public class BookRepository : BaseRepository<Book>
{
    protected override string FilePath => "../Data/books.json";
}