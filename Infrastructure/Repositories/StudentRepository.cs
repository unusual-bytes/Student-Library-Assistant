using Student_Library_Assistant.Domain.Entities;

namespace Student_Library_Assistant.Infrastructure.Repositories;

public class StudentRepository : BaseRepository<Student>
{
    protected override string FilePath => "../../../Infrastructure/Data/students.json";
}