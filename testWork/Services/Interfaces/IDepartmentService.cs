using testWork.Domain.Response;
using System.Collections.Generic;
using System.Threading.Tasks;
using testWork.Models;
using testWork.ViewModels;
using Microsoft.Extensions.Logging;

namespace testWork.Services.Interfaces
{
    public interface IDepartmentService
	{
		IBaseResponse<List<Department>> GetDepartments();
        Task<IBaseResponse<Department>> GetDepartmentByName(string name);
       	Task<IBaseResponse<string>> GetDepartmentName(long id);
		Task<IBaseResponse<Department>> Get(long id);
        IBaseResponse<List<Department>> GetDepartmentsRoot();
        Task<IBaseResponse<Department>> Create(Department evento);
        Task<IBaseResponse<Department>> Edit(long id, Department model);

    }
}
