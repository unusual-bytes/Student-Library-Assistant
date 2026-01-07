using Student_Library_Assistant.Domain.Entities;

namespace Student_Library_Assistant.Infrastructure.Repositories;

public class LoanRepository : BaseRepository<Loan>
{
    protected override string FilePath => "Infrastructure/Data/loans.json";
}