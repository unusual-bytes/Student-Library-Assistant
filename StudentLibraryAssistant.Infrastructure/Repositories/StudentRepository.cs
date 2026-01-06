namespace Student_Library_Assistant.StudentLibraryAssistant.Infrastructure.Repositories;

public class StudentRepository : BaseRepository<Student>
{
    protected override string FilePath => "../Data/students.json";
}