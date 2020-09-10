using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BelajarDotNetCoreAPI.Models
{
    [Table("tb_m_employee")]
    public class Employee
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset JoinDate { get; set; }
        public bool IsActive { get; set; }

        public User User { get; set; }
    }
}
