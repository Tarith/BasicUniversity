using System;
using System.Threading.Tasks;

namespace BasicUniversity.Models
{
    public class UnitOfWork: IUnitOfWork, IDisposable
    {
        // TODO: Inversion of control
        private SchoolContext _context = new SchoolContext();
        private IStudentRepository<Student> _studentRepository;
        private IInstructorRepository<Instructor> _instuctorRepository;
        private IDepartmentRepository<Department> _departmentRepository;
        private ICourseRepository<Course> _courseRepository;
        private bool _disposed = false;

        public IStudentRepository<Student> Students
        {
            get
            {
                if (_studentRepository == null)
                {
                    // TODO: Inversion of control
                    _studentRepository = new StudentRepository<Student>(_context);
                }

                return _studentRepository;
            }
        }
        public IInstructorRepository<Instructor> Instructors
        {
            get
            {
                if (_instuctorRepository == null)
                {
                    // TODO: Inversion of control
                    _instuctorRepository = new InstructorRepository<Instructor>(_context);
                }

                return _instuctorRepository;
            }
        }
        public IDepartmentRepository<Department> Departments
        {
            get
            {
                if (_departmentRepository == null)
                {
                    // TODO: Inversion of control
                    _departmentRepository = new DepartmentRepository<Department>(_context);
                }

                return _departmentRepository;
            }
        }
        public ICourseRepository<Course> Courses
        {
            get
            {
                if (_courseRepository == null)
                {
                    // TODO: inversion of control
                    _courseRepository = new CourseRepository<Course>(_context);
                }

                return _courseRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
