using Microsoft.EntityFrameworkCore;
using testWork.Models;

namespace testWork.Data
{
    public class AppDBContent: DbContext
    {
        public AppDBContent(DbContextOptions<AppDBContent> options): base(options) {

        }
        public DbSet<Department> Departments { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=departmentsDB;Username=postgres;Password=112233");
		}
	}
}
