using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BelajarDotNetCoreAPI.ViewModel.DivisionViewModel
{
    public class DivisionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset UpdateDate { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}
