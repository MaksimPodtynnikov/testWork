using testWork.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testWork.Models;

namespace testWork.Repositories
{
    public class DepartmentsRepository : IDepartments
	{
		private readonly AppDBContent appDBContent;
		public DepartmentsRepository(AppDBContent appDBContent)
		{
			this.appDBContent = appDBContent;
		}
		public IEnumerable<Department> Departments => appDBContent.Departments;
        public async Task Create(Department department)
        {
            await appDBContent.Departments.AddAsync(department);
            await appDBContent.SaveChangesAsync();

        }
        public async Task<Department> Edit(Department department)
        {
            appDBContent.Departments.Update(department);
            await appDBContent.SaveChangesAsync();

            return department;
        }
        public Task<Department> Get(long id) => appDBContent.Departments.FirstOrDefaultAsync(p => p.Id == id);

        public Task<Department> GetDepartmentByName(string name) => appDBContent.Departments.FirstOrDefaultAsync(x => x.Name == name);

        public List<Department> GetDepartmentsRoot() => appDBContent.Departments.AsEnumerable().Where(c => c.ParentId == null).ToList();
        public Task<Department> GetStruct(long id) => appDBContent.Departments.Include(c=>c.departments).FirstOrDefaultAsync(p => p.Id == id);
	}
}
