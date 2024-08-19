using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace testWork.Models
{
	public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
		[JsonIgnore]
		public virtual Department? Parent { get; set; }
        public List<Department>? departments { set; get; }
        [JsonIgnore]
        public int? ParentId { get; set; }
        
    }
}
