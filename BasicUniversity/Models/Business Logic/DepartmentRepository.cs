using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicUniversity.Models
{
    internal class DepartmentRepository<T> : GenericRepository<Department>, IDepartmentRepository<Department>
    {
        public DepartmentRepository(SchoolContext context) : base(context)
        {
            _context = context;
        }
    }
}
