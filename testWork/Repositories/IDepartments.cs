using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testWork.Models;

namespace testWork.Repositories
{
    public interface IDepartments
	{
        IEnumerable<Department?> Departments { get; }
        Task Create(Department department);
        Task<Department> Edit(Department department);
        Task<Department?> Get(long id);
        Task<Department> GetDepartmentByName(string name);
        List<Department> GetDepartmentsRoot();
        Task<Department?> GetStruct(long id);
	}
}
