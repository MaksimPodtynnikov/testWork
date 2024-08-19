
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using testWork.Services.Interfaces;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using testWork.Models;
using testWork.ViewModels;
using testWork.Repositories;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text;
using System.Net.Http;

namespace testWork.Controllers
{
	public class DepartmentsController : Controller
	{
		private readonly IDepartmentService _departments;
		
        public DepartmentsController(IDepartmentService departments)
		{
			_departments = departments;
		}
        public async Task<bool> getStatus(int Id)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("ApiKey", "6CBxzdYcEgNDrRhMbDpkBF7e4d4Kib46dwL9ZE5egiL0iL5Y3dzREUBSUYVUwUkN");
                return await httpClient.GetFromJsonAsync<bool>("https://localhost:7100/Status?Id=" + Id);
            }
            catch
            {
                return false;
            }
        }
        // GET: DepartmentsController - страница со списком
        public ActionResult Index()
		{
            HttpClient httpClient = new HttpClient();
            var response = _departments.GetDepartmentsRoot();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
				List<DepartmentViewModel> list = response.Data.Select(c => new DepartmentViewModel {  Name = c.Name, Id = c.Id,Status = getStatus(c.Id).Result, Departments = c.departments, Parent = c.Parent }).ToList();
				return View(list);
            }
            return View("Error", $"{response.Description}");
        }
        [HttpPost]
        public IActionResult Refresh()
        {
            var response = _departments.GetDepartmentsRoot();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                List<DepartmentViewModel> list = response.Data.Select(c => new DepartmentViewModel {  Name = c.Name, Id = c.Id, Status = getStatus(c.Id).Result, Departments = c.departments, Parent = c.Parent }).ToList();
                return PartialView("Departments", list);
            }
            return View("Error", $"{response.Description}");
        }
        [HttpGet("Departments/name/{id}")]
		public async Task<string> GetName(int id)
        {
            var response = await _departments.GetDepartmentName(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return response.Data;
            }
			return "";
        }
		[HttpGet("Departments/struct/{id}")]
		public async Task<string> GetStruct(int id)
		{
			var response = await _departments.Get(id);
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				JsonSerializerOptions options = new()
				{
					ReferenceHandler = ReferenceHandler.IgnoreCycles,
					Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
					WriteIndented = true
				};
				string json = JsonSerializer.Serialize(response.Data,options);
				return json;
			}
			return "";
		}
        [HttpGet("Departments/struct")]
        public string GetStruct()
        {
            var response = _departments.GetDepartmentsRoot();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                JsonSerializerOptions options = new()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize(response.Data, options);
                return json;
            }
            return "";
        }
        [HttpPost]
		public IActionResult Search(string search)
		{
			if (search == null) search = "";
			List<Department> departments = _departments.GetDepartments().Data.Where(c => c.Name.ToLower().Contains(search.ToLower())).ToList();
			List<DepartmentViewModel> list = departments.Select(c => new DepartmentViewModel { Name = c.Name, Id = c.Id, Status = getStatus(c.Id).Result, Departments=c.departments,Parent=c.Parent}).ToList();
			return PartialView("Departments", list);
		}
		[HttpPost]
		public async Task<IActionResult> Sync(IFormFile uploadedFile)
		{
            try
            {
                if (uploadedFile != null)
                {
                    var result = new StringBuilder();
                    using (var reader = new StreamReader(uploadedFile.OpenReadStream()))
                    {
                        while (reader.Peek() >= 0)
                            result.AppendLine(reader.ReadLine());
                    }
                    var fileDepartments = JsonSerializer.Deserialize<List<Department>>(result.ToString());
                    var rootDepartment = fileDepartments.Where(c => c.ParentId == null).ToList();
                    await checkUp(rootDepartment, null);
                }
            }
            catch(Exception ex)
            {
                return View("Error", $"{ex}");
            }
            return RedirectToAction(nameof(Index));
        }
		private async Task<bool> checkUp(List<Department> rootDepartment,int? ParentId)
		{
			foreach (Department department in rootDepartment)
			{
                department.ParentId = ParentId;
				var tempDepartment = await _departments.Get(department.Id);
                if (tempDepartment.StatusCode == Domain.Enum.StatusCode.OK)
                {
					if (tempDepartment != null)
						await _departments.Edit(tempDepartment.Data.Id, department);
                }
                else await _departments.Create(department);
                if (department.departments!=null)
                    await checkUp(department.departments,department.Id);
			}
            return true;
		}
    }
}
