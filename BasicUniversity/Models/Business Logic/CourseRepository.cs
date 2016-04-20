using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicUniversity.Models
{
    internal class CourseRepository<T> : GenericRepository<Course>, ICourseRepository<Course>
    {
        public CourseRepository(SchoolContext context) : base(context)
        {
            _context = context;
        }
    }
}
