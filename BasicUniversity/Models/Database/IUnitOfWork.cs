using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicUniversity.Models
{
    public interface IUnitOfWork : IDisposable
    {
        IStudentRepository<Student> Students { get; }
        IInstructorRepository<Instructor> Instructors { get; }
        IDepartmentRepository<Department> Departments { get; }
        ICourseRepository<Course> Courses { get; }
        void Save();
        Task<int> SaveAsync();
    }
}
