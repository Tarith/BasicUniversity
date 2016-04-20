using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicUniversity.Models
{
    internal class InstructorRepository<T> : GenericRepository<Instructor>, IInstructorRepository<Instructor>
    {
        public InstructorRepository(SchoolContext context) : base(context)
        {
            _context = context;
        }

        public void UpdateCoursesList(string[] listOfCourses, Instructor instructor)
        {
            if (listOfCourses == null)
            {
                instructor.Courses = new List<Course>();
                return;
            }

            var selectedCoursesHS = new HashSet<string>(listOfCourses);
            var instructorCourses = new HashSet<int>(instructor.Courses.Select(c => c.Id));

            //get all the available courses from the database and compare them with the selected courses
            foreach (var course in _context.Courses) 
            {
                if (selectedCoursesHS.Contains(course.Id.ToString()))
                {
                    if (!instructorCourses.Contains(course.Id))
                    {
                        instructor.Courses.Add(course);
                    }
                }
                else
                {
                    if (instructorCourses.Contains(course.Id))
                    {
                        instructor.Courses.Remove(course);
                    }
                }
            }
        }
    }
}
