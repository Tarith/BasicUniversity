namespace BasicUniversity.Migrations
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BasicUniversity.Models.SchoolContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BasicUniversity.Models.SchoolContext context)
        {
            var students = new List<Student>
            {
                new Student { FirstName = "Carson",   LastName = "Alexander",  EnrollmentDate = DateTime.Parse("2010-09-01") },
                new Student { FirstName = "Meredith", LastName = "Alonso",     EnrollmentDate = DateTime.Parse("2012-09-01") },
                new Student { FirstName = "Arturo",   LastName = "Anand",      EnrollmentDate = DateTime.Parse("2013-09-01") },
                new Student { FirstName = "Gytis",    LastName = "Barzdukas",  EnrollmentDate = DateTime.Parse("2012-09-01") },
                new Student { FirstName = "Yan",      LastName = "Li",         EnrollmentDate = DateTime.Parse("2012-09-01") },
                new Student { FirstName = "Peggy",    LastName = "Justice",    EnrollmentDate = DateTime.Parse("2011-09-01") },
                new Student { FirstName = "Laura",    LastName = "Norman",     EnrollmentDate = DateTime.Parse("2013-09-01") },
                new Student { FirstName = "Nino",     LastName = "Olivetto",   EnrollmentDate = DateTime.Parse("2005-08-11") }
            };
            students.ForEach(s => context.Students.AddOrUpdate(p => p.LastName, s));
            context.SaveChanges();

            var instructors = new List<Instructor>
            {
                new Instructor { FirstName = "Kim",     LastName = "Abercrombie", HireDate = DateTime.Parse("1995-03-11") },
                new Instructor { FirstName = "Fadi",    LastName = "Fakhouri", HireDate = DateTime.Parse("2002-07-06") },
                new Instructor { FirstName = "Roger",   LastName = "Harui", HireDate = DateTime.Parse("1998-07-01") },
                new Instructor { FirstName = "Candace", LastName = "Kapoor", HireDate = DateTime.Parse("2001-01-15") },
                new Instructor { FirstName = "Roger",   LastName = "Zheng", HireDate = DateTime.Parse("2004-02-12") }
            };
            instructors.ForEach(s => context.Instructors.AddOrUpdate(p => p.LastName, s));
            context.SaveChanges();

            var departments = new List<Department>
            {
                new Department { Name = "English",     Budget = 350000, StartDate = DateTime.Parse("2007-09-01"), InstructorId  = instructors.Single( i => i.LastName == "Abercrombie").Id },
                new Department { Name = "Mathematics", Budget = 100000, StartDate = DateTime.Parse("2007-09-01"), InstructorId  = instructors.Single( i => i.LastName == "Fakhouri").Id },
                new Department { Name = "Engineering", Budget = 350000, StartDate = DateTime.Parse("2007-09-01"), InstructorId  = instructors.Single( i => i.LastName == "Harui").Id },
                new Department { Name = "Economics",   Budget = 100000, StartDate = DateTime.Parse("2007-09-01"), InstructorId  = instructors.Single( i => i.LastName == "Kapoor").Id }
            };
            departments.ForEach(s => context.Departments.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            var courses = new List<Course>
            {
                new Course { Id = 1050, Title = "Chemistry",      Credits = 3, DepartmentId = departments.Single( s => s.Name == "Engineering").Id, Instructors = new List<Instructor>() },
                new Course { Id = 4022, Title = "Microeconomics", Credits = 3, DepartmentId = departments.Single( s => s.Name == "Economics").Id, Instructors = new List<Instructor>() },
                new Course { Id = 4041, Title = "Macroeconomics", Credits = 3, DepartmentId = departments.Single( s => s.Name == "Economics").Id, Instructors = new List<Instructor>() },
                new Course { Id = 1045, Title = "Calculus",       Credits = 4, DepartmentId = departments.Single( s => s.Name == "Mathematics").Id, Instructors = new List<Instructor>() },
                new Course { Id = 3141, Title = "Trigonometry",   Credits = 4, DepartmentId = departments.Single( s => s.Name == "Mathematics").Id, Instructors = new List<Instructor>() },
                new Course { Id = 2021, Title = "Composition",    Credits = 3, DepartmentId = departments.Single( s => s.Name == "English").Id, Instructors = new List<Instructor>() },
                new Course { Id = 2042, Title = "Literature",     Credits = 4, DepartmentId = departments.Single( s => s.Name == "English").Id, Instructors = new List<Instructor>() },
            };
            courses.ForEach(s => context.Courses.AddOrUpdate(p => p.Id, s));
            context.SaveChanges();

            var officeAssignments = new List<OfficeAssignment>
            {
                new OfficeAssignment { InstructorId = instructors.Single( i => i.LastName == "Fakhouri").Id, Location = "Smith 17" },
                new OfficeAssignment { InstructorId = instructors.Single( i => i.LastName == "Harui").Id, Location = "Gowan 27" },
                new OfficeAssignment { InstructorId = instructors.Single( i => i.LastName == "Kapoor").Id, Location = "Thompson 304" },
            };
            officeAssignments.ForEach(s => context.OfficeAssignments.AddOrUpdate(p => p.InstructorId, s));
            context.SaveChanges();

            AddOrUpdateInstructor(context, "Chemistry", "Kapoor");
            AddOrUpdateInstructor(context, "Chemistry", "Harui");
            AddOrUpdateInstructor(context, "Microeconomics", "Zheng");
            AddOrUpdateInstructor(context, "Macroeconomics", "Zheng");

            AddOrUpdateInstructor(context, "Calculus", "Fakhouri");
            AddOrUpdateInstructor(context, "Trigonometry", "Harui");
            AddOrUpdateInstructor(context, "Composition", "Abercrombie");
            AddOrUpdateInstructor(context, "Literature", "Abercrombie");

            context.SaveChanges();

            var enrollments = new List<Enrollment>
            {
                new Enrollment {
                    StudentId = students.Single(s => s.LastName == "Alexander").Id,
                    CourseId = courses.Single(c => c.Title == "Chemistry" ).Id,
                    Grade = Grade.A
                },
                 new Enrollment {
                    StudentId = students.Single(s => s.LastName == "Alexander").Id,
                    CourseId = courses.Single(c => c.Title == "Microeconomics" ).Id,
                    Grade = Grade.C
                 },
                 new Enrollment {
                    StudentId = students.Single(s => s.LastName == "Alexander").Id,
                    CourseId = courses.Single(c => c.Title == "Macroeconomics" ).Id,
                    Grade = Grade.B
                 },
                 new Enrollment {
                    StudentId = students.Single(s => s.LastName == "Alonso").Id,
                    CourseId = courses.Single(c => c.Title == "Calculus" ).Id,
                    Grade = Grade.B
                 },
                 new Enrollment {
                    StudentId = students.Single(s => s.LastName == "Alonso").Id,
                    CourseId = courses.Single(c => c.Title == "Trigonometry" ).Id,
                    Grade = Grade.B
                 },
                 new Enrollment {
                    StudentId = students.Single(s => s.LastName == "Alonso").Id,
                    CourseId = courses.Single(c => c.Title == "Composition" ).Id,
                    Grade = Grade.B
                 },
                 new Enrollment {
                    StudentId = students.Single(s => s.LastName == "Anand").Id,
                    CourseId = courses.Single(c => c.Title == "Chemistry" ).Id
                 },
                 new Enrollment {
                    StudentId = students.Single(s => s.LastName == "Anand").Id,
                    CourseId = courses.Single(c => c.Title == "Microeconomics").Id,
                    Grade = Grade.B
                 },
                new Enrollment {
                    StudentId = students.Single(s => s.LastName == "Barzdukas").Id,
                    CourseId = courses.Single(c => c.Title == "Chemistry").Id,
                    Grade = Grade.B
                 },
                 new Enrollment {
                    StudentId = students.Single(s => s.LastName == "Li").Id,
                    CourseId = courses.Single(c => c.Title == "Composition").Id,
                    Grade = Grade.B
                 },
                 new Enrollment {
                    StudentId = students.Single(s => s.LastName == "Justice").Id,
                    CourseId = courses.Single(c => c.Title == "Literature").Id,
                    Grade = Grade.B
                 }
            };

            foreach (Enrollment e in enrollments)
            {
                var enrollmentInDataBase = context.Enrollments.Where(s => s.Student.Id == e.StudentId && s.Course.Id == e.CourseId).SingleOrDefault();

                if (enrollmentInDataBase == null)
                {
                    context.Enrollments.Add(e);
                }
            }

            context.SaveChanges();
        }

        void AddOrUpdateInstructor(SchoolContext context, string courseTitle, string instructorName)
        {
            var crs = context.Courses.SingleOrDefault(c => c.Title == courseTitle);
            var inst = crs.Instructors.SingleOrDefault(i => i.LastName == instructorName);

            if (inst == null)
            {
                crs.Instructors.Add(context.Instructors.Single(i => i.LastName == instructorName));
            }
        }
    }
}
