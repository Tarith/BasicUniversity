using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BasicUniversity.Models;
using BasicUniversity.ViewModels;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace BasicUniversity.Controllers
{
    public class InstructorController : Controller
    {
        private IUnitOfWork _db = new UnitOfWork();

        // GET: Instructor
        public  ActionResult Index(int? id, int? courseId)
        {
            var viewModel = new InstructorIndexData();
            
            viewModel.Instructors =  _db.Instructors.Get(includeProperties: "OfficeAssignment", orderBy: i => i.OrderBy(d => d.LastName)).Include(i => i.Courses.Select(c => c.Department));

            if (id != null)
            {
                ViewBag.InstructorId = id.Value;
                viewModel.Courses = viewModel.Instructors.Where(i => i.Id == id.Value).Single().Courses;
            }

            if (courseId != null)
            {
                ViewBag.CourseId = courseId.Value;
                viewModel.Enrollments = viewModel.Courses.Where(x => x.Id == courseId).Single().Enrollments;
            }

            return View(viewModel);
        }

        // GET: Instructor/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Instructor instructor = await _db.Instructors.GetByIdAsync(id);

            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // GET: Instructor/Create
        public ActionResult Create()
        {
            var instructor = new Instructor
            {
                Courses = new List<Course>()
            };

            PopulateAssignedCourseData(instructor);

            return View();
        }

        // POST: Instructor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,LastName,FirstName,HireDate,OfficeAssignment")] Instructor instructor, string[] selectedCourses)
        {
            if (selectedCourses != null)
            {
                instructor.Courses = new List<Course>();

                foreach (var course in selectedCourses)
                {
                    var courseToAdd = await _db.Courses.GetByIdAsync(int.Parse(course));
                    instructor.Courses.Add(courseToAdd);
                }
            }
            
            if (ModelState.IsValid)
            {
                _db.Instructors.Insert(instructor);
                await _db.SaveAsync();
                return RedirectToAction("Index");
            }

            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        // GET: Instructor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Instructor instructor = _db.Instructors.Get(includeProperties: "OfficeAssignment, Courses", filter: i => i.Id == id).Single();
            
            PopulateAssignedCourseData(instructor);

            if (instructor == null)
            {
                return HttpNotFound();
            }

            return View(instructor);
        }

        // POST: Instructor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, string[] selectedCourses)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var instructorToUpdate = _db.Instructors.Get(includeProperties: "OfficeAssignment, Courses", filter: i => i.Id == id).Single();
            
            if (TryUpdateModel(instructorToUpdate, "", new string[] { "LastName", "FirstMidName", "HireDate", "OfficeAssignment" }))
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment.Location))
                    {
                        instructorToUpdate.OfficeAssignment = null;
                    }

                    _db.Instructors.UpdateCoursesList(selectedCourses, instructorToUpdate);

                    //UpdateInstructorCourses(selectedCourses, instructorToUpdate);

                    await _db.SaveAsync();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateAssignedCourseData(instructorToUpdate);
            return View(instructorToUpdate);
        }

        // GET: Instructor/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Instructor instructor = await _db.Instructors.GetByIdAsync(id);

            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            //SchoolContext db = new SchoolContext();

            var department = await _db.Departments.Get(filter: d => d.InstructorId == id).SingleOrDefaultAsync();
            var instructor = await _db.Instructors.Get(filter: i => i.Id == id, includeProperties: "OfficeAssignment").SingleOrDefaultAsync();

            if (department != null)
            {
                department.InstructorId = null;
            }
            
            instructor.OfficeAssignment = null;
            
            _db.Instructors.Delete(id);
            await _db.SaveAsync();
            return RedirectToAction("Index");
        }


        private void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = _db.Courses.Get();
            var instructorCourses = new HashSet<int>(instructor.Courses.Select(c => c.Id));
            var viewModel = new List<AssignedCourseData>();

            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseId = course.Id,
                    Title = course.Title,
                    Assigned = instructorCourses.Contains(course.Id)
                });
            }
            ViewBag.Courses = viewModel;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
