using System.Collections.Generic;
using testWork.Models;

namespace testWork.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public Department? Parent { get; set; }
        public List<Department>? Departments { set; get; }
        public string GetStatus => Status ? "Активно" : "Заблокировано";
        public DepartmentViewModel getParent()
        {
            if (Parent == null)
                return new DepartmentViewModel();
            else
                return new DepartmentViewModel { Id = Parent.Id, Name = Parent.Name, Parent = Parent.Parent, Departments = Parent.departments };
        }
        public List<DepartmentViewModel> getDepartments()
        {
            return Departments.Select(c => new DepartmentViewModel
            {
                Name = c.Name,
                Id = c.Id,
                Departments = c.departments,
                Parent = c.Parent
            }).ToList();
        }
    }
}
