using testWork.Domain.Enum;
using testWork.Domain.Response;
using System.Threading.Tasks;
using System;
using testWork.Models;
using testWork.Repositories;
using Microsoft.EntityFrameworkCore;
using testWork.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using testWork.ViewModels;
using System.Text.RegularExpressions;

namespace testWork.Services.Implementations
{
    public class DepartmentsService: IDepartmentService
	{
		private readonly IDepartments _departmentRepository;
		public DepartmentsService(IDepartments DepartmentRepository)
		{
			_departmentRepository = DepartmentRepository;
		}
		public async Task<IBaseResponse<string>> GetDepartmentName(long id)
		{
			try
			{
				var department = await _departmentRepository.Get(id);
				if (department == null)
				{
					throw new Exception("Нет данных");
				}

				return new BaseResponse<string>()
				{
					StatusCode = StatusCode.OK,
					Data = department.Name
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<string>()
				{
					Description = $"[GetStatus] : {ex.Message}",
					StatusCode = StatusCode.InternalServerError
				};
			}
		}

        public async Task<IBaseResponse<Department>> Create(Department model)
        {
            try
            {
                await _departmentRepository.Create(model);

                return new BaseResponse<Department>()
                {
                    StatusCode = StatusCode.OK,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Department>()
                {
                    Description = $"[Create] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        public async Task<IBaseResponse<Department>> Edit(long id, Department model)
        {
            try
            {
                var dep = await _departmentRepository.Get(id);
                if (dep == null)
                {
                    return new BaseResponse<Department>()
                    {
                        Description = "Group not found",
                        StatusCode = StatusCode.ObjectNotFound
                    };
                }
                //dep.Status =model.Status;
                dep.Name = model.Name;
                dep.ParentId = model.ParentId;
                await _departmentRepository.Edit(dep);


                return new BaseResponse<Department>()
                {
                    Data = dep,
                    StatusCode = StatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Department>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        public IBaseResponse<List<Department>> GetDepartments()
		{
			try
			{
				var departments = _departmentRepository.Departments.ToList();

				return new BaseResponse<List<Department>>()
				{
					Data = departments,
					StatusCode = StatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<List<Department>>()
				{
					Description = $"[Getdepartments] : {ex.Message}",
					StatusCode = StatusCode.InternalServerError
				};
			}
		}
        public async Task<IBaseResponse<Department>> GetDepartmentByName(string name)
        {
            try
            {
                var department = await _departmentRepository.GetDepartmentByName(name);

                return new BaseResponse<Department>()
                {
                    StatusCode = StatusCode.OK,
                    Data = department
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Department>()
                {
                    Description = $"[GetStatus] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        public IBaseResponse<List<Department>> GetDepartmentsRoot()
        {
            try
            {
                var departments =  _departmentRepository.GetDepartmentsRoot();

                return new BaseResponse<List<Department>>()
                {
                    Data = departments,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Department>>()
                {
                    Description = $"[Getdepartments] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Department>> Get(long id)
		{
			try
			{
				var department = await _departmentRepository.Get(id);
				if (department == null)
				{
					throw new Exception("Нет данных");
				}

				return new BaseResponse<Department>()
				{
					StatusCode = StatusCode.OK,
					Data = department
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<Department>()
				{
					Description = $"[GetStatus] : {ex.Message}",
					StatusCode = StatusCode.InternalServerError
				};
			}
		}

	}
}
