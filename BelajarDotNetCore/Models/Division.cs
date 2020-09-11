using BelajarDotNetCoreAPI.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BelajarDotNetCoreAPI.Models
{
    public class Division : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset UpdateDate { get; set; }
        public DateTimeOffset DeleteDate { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public bool IsDelete { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

    }
}
