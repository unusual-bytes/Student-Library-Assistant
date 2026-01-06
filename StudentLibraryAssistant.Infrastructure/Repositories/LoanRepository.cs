namespace Student_Library_Assistant.StudentLibraryAssistant.Infrastructure.Repositories;

public class LoanRepository : BaseRepository<Student>
{
    protected override string FilePath => "../Data/loans.json";
}