using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BelajarDotNetCoreAPI.Base
{
    public interface BaseModel
    {
        int Id { get; set; }
        string Name { get; set; }
        DateTimeOffset UpdateDate { get; set; }
        DateTimeOffset DeleteDate { get; set; }
        DateTimeOffset CreateDate { get; set; }
        bool IsDelete { get; set; }
    }
}
