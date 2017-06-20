using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using CompunetCbte.Models;
using ExamSolutionModel;

namespace CompunetCbte.Controllers
{
    public class SemestersController : Controller
    {
        private readonly OnlineCbte _db;

        public SemestersController()
        {
            _db = new OnlineCbte();
        }

        // GET: Semesters
        public async Task<ActionResult> Index()
        {
            return View(await _db.Semesters.AsNoTracking().ToListAsync());
        }

        // GET: Semesters/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semester semester = await _db.Semesters.FindAsync(id);
            if (semester == null)
            {
                return HttpNotFound();
            }
            return View(semester);
        }

        // GET: Semesters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Semesters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SemesterId,SemesterName,ActiveSemester")] Semester semester)
        {
            if (ModelState.IsValid)
            {
                if (semester.ActiveSemester.Equals(true))
                {
                    var current = await _db.Semesters.AsNoTracking().Where(s => s.ActiveSemester.Equals(true)).CountAsync();
                    if (current >= 1)
                    {
                        ViewBag.Message = "You cant have more than ONE active Semesters";
                        return View("ErrorException");
                    }
                }
                semester.SemesterName = semester.SemesterName.ToUpper().Trim();
                _db.Semesters.Add(semester);
                await _db.SaveChangesAsync();
                TempData["UserMessage"] = "Semester Created Successfully.";
                TempData["Title"] = "Success.";

                return RedirectToAction("Create");
            }

            return View(semester);
        }

        // GET: Semesters/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semester semester = await _db.Semesters.FindAsync(id);
            if (semester == null)
            {
                return HttpNotFound();
            }
            return View(semester);
        }

        // POST: Semesters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SemesterId,SemesterName,ActiveSemester")] Semester semester)
        {
            if (ModelState.IsValid)
            {
                if (semester.ActiveSemester.Equals(true))
                {
                    var current = await _db.Semesters.AsNoTracking().Where(s => s.ActiveSemester.Equals(true))
                        .CountAsync();
                    if (current >= 1)
                    {
                        ViewBag.Message = "You cant have more than ONE active Sessions";
                        return View("ErrorException");
                    }
                    try
                    {
                        semester.SemesterName = semester.SemesterName.ToUpper().Trim();
                        _db.Entry(semester).State = EntityState.Modified;
                        await _db.SaveChangesAsync();
                        TempData["UserMessage"] = "Semester Updated Successfully.";
                        TempData["Title"] = "Success.";
                        return RedirectToAction("Index");
                    }
                    catch (Exception e)
                    {
                        ViewBag.ErrorMessage = e.Message;
                        return View("ErrorException");
                    }
                }


            }
            return View(semester);
        }

        // GET: Semesters/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semester semester = await _db.Semesters.FindAsync(id);
            if (semester == null)
            {
                return HttpNotFound();
            }
            return View(semester);
        }

        // POST: Semesters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Semester semester = await _db.Semesters.FindAsync(id);
            if (semester != null) _db.Semesters.Remove(semester);
            await _db.SaveChangesAsync();
            TempData["UserMessage"] = "Semester Deleted Successfully.";
            TempData["Title"] = "Error.";

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
