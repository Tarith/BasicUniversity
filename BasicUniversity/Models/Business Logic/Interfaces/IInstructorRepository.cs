using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicUniversity.Models
{
    public interface IInstructorRepository<T> : IRepository<Instructor>
    {
        void UpdateCoursesList(string[] listOfCourses, Instructor instructor);
    }
}
