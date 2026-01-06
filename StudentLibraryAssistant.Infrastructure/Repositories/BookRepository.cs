namespace Student_Library_Assistant.StudentLibraryAssistant.Infrastructure.Repositories;

public class BookRepository : BaseRepository<Student>
{
    protected override string FilePath => "../Data/books.json";
}