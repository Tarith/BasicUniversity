using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicUniversity.Models
{
    internal class StudentRepository<T> : GenericRepository<Student>, IStudentRepository<Student>
    {
        public StudentRepository(SchoolContext context) : base(context)
        {
            _context = context;
        }
    }
}
