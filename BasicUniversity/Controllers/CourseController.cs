using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BasicUniversity.Models;
using System.Data.Entity.Infrastructure;

namespace BasicUniversity.Controllers
{
    public class CourseController : Controller
    {
        private IUnitOfWork _db = new UnitOfWork();

        // GET: Couse
        public ActionResult Index()
        {
            var courses = _db.Courses.Get(includeProperties: "Department");
            return View(courses.ToList());
        }

        // GET: Couse/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = _db.Courses.GetById(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Couse/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(_db.Departments.Get(), "Id", "Name");
            return View();
        }

        // POST: Couse/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Credits,DepartmentId")] Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _db.Courses.Insert(course);
                    _db.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            ViewBag.DepartmentId = new SelectList(_db.Departments.Get(), "Id", "Name", course.DepartmentId);
            return View(course);
        }

        // GET: Couse/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = _db.Courses.GetById(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(_db.Departments.Get(), "Id", "Name", course.DepartmentId);
            return View(course);
        }

        // POST: Couse/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost([Bind(Include = "Id,Title,Credits,DepartmentId")] Course course)
        {
            if (ModelState.IsValid)
            {
                _db.Courses.Update(course);
                _db.Save();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(_db.Departments.Get(), "Id", "Name", course.DepartmentId);
            return View(course);
        }

        // GET: Couse/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = _db.Courses.GetById(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Couse/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _db.Courses.Delete(id);
            _db.Save();
            return RedirectToAction("Index");
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
