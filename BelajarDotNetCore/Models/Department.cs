using BelajarDotNetCoreAPI.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BelajarDotNetCoreAPI.Models
{
    [Table("tb_m_department")]
    public class Department : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset UpdateDate { get; set; }
        public DateTimeOffset DeleteDate { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public bool IsDelete { get; set; }
    }
}
