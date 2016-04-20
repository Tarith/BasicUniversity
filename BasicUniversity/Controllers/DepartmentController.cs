using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BasicUniversity.Models;
using System.Data.Entity.Infrastructure;

namespace BasicUniversity.Controllers
{
    public class DepartmentController : Controller
    {
        private IUnitOfWork _db = new UnitOfWork();
        
        // GET: Department
        public async Task<ActionResult> Index()
        {
            var departments = _db.Departments.Get(includeProperties: "Administrator");
            return View(await departments.ToListAsync());
        }

        // GET: Department/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Department department = await _db.Departments.GetByIdAsync(id);

            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: Department/Create
        public ActionResult Create()
        {
            ViewBag.InstructorId = new SelectList(_db.Instructors.Get(), "Id", "FullName");
            return View();
        }

        // POST: Department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Budget,StartDate,InstructorId")] Department department)
        {
            if (ModelState.IsValid)
            {
                _db.Departments.Insert(department);
                await _db.SaveAsync();
                return RedirectToAction("Index");
            }

            ViewBag.InstructorId = new SelectList(_db.Instructors.Get(), "Id", "LastName", department.InstructorId);
            return View(department);
        }

        // GET: Department/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = await _db.Departments.GetByIdAsync(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.InstructorId = new SelectList(_db.Instructors.Get(), "Id", "FullName", department.InstructorId);
            return View(department);
        }

        // POST: Department/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Budget,StartDate,InstructorID,RowVersion")] Department department)
        {
            if (department == null)
            {
                Department deletedDepartment = new Department();
                ModelState.AddModelError(string.Empty, "Unable to save changes. The department was deleted by another user.");
                ViewBag.InstructorID = new SelectList(_db.Instructors.Get(), "ID", "FullName", deletedDepartment.InstructorId);
                return View(deletedDepartment);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Departments.Update(department);
                    await _db.SaveAsync();

                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Department)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Department)databaseEntry.ToObject();

                        if (databaseValues.Name != clientValues.Name)
                            ModelState.AddModelError("Name", "Current value: " + databaseValues.Name);
                        if (databaseValues.Budget != clientValues.Budget)
                            ModelState.AddModelError("Budget", "Current value: " + String.Format("{0:c}", databaseValues.Budget));
                        if (databaseValues.StartDate != clientValues.StartDate)
                            ModelState.AddModelError("StartDate", "Current value: " + String.Format("{0:d}", databaseValues.StartDate));
                        if (databaseValues.InstructorId != clientValues.InstructorId)
                            ModelState.AddModelError("InstructorID", "Current value: " + _db.Instructors.GetById(databaseValues.InstructorId).FullName);

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                            + "was modified by another user after you got the original value. The "
                            + "edit operation was canceled and the current values in the database "
                            + "have been displayed. If you still want to edit this record, click "
                            + "the Save button again. Otherwise click the Back to List hyperlink.");

                        //departmentToUpdate.RowVersion = databaseValues.RowVersion;
                        department.RowVersion = databaseValues.RowVersion;

                        //add a "show refresh button = true" or something like it or not, because the new values are already shown as error messages
                    }
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            ViewBag.InstructorID = new SelectList(_db.Instructors.Get(), "ID", "FullName", department.InstructorId /*departmentToUpdate.InstructorId*/);
            return View(/*departmentToUpdate*/ department);
        }

        // GET: Department/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Department department = await _db.Departments.GetByIdAsync(id);

            if (department == null)
            {
                if (concurrencyError.GetValueOrDefault())
                {
                    return RedirectToAction("Index");
                }

                return HttpNotFound();
            }
            
            if (concurrencyError.GetValueOrDefault())
            {
                ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete "
                + "was modified by another user after you got the original values. "
                + "The delete operation was canceled and the current values in the "
                + "database have been displayed. If you still want to delete this "
                + "record, click the Delete button again. Otherwise "
                + "click the Back to List hyperlink.";
            }
            
            return View(department);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Department department)
        {
            try
            {
                _db.Departments.Delete(department);
                await _db.SaveAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = department.Id }); 
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
                return View(department);
            }

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
