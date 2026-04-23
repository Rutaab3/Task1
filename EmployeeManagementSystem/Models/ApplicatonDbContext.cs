using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Models
{
    public class ApplicatonDbContext : DbContext
    {
        public ApplicatonDbContext(DbContextOptions<ApplicatonDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }

    }
}
