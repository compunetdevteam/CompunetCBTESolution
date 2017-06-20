using CompunetCbte.Models;
using ExamSolutionModel;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CompunetCbte.Controllers
{
    public class CoursesController : Controller
    {
        private readonly OnlineCbte _db;

        public CoursesController()
        {
            _db = new OnlineCbte();
        }

        // GET: Courses
        public async Task<ActionResult> Index()
        {
            var courses = _db.Courses.Include(c => c.Department);
            return View(await courses.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = await _db.Courses.FindAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new MultiSelectList(_db.Departments, "DepartmentId", "DeptName");
            ViewBag.SemesterId = new SelectList(_db.Semesters, "SemesterId", "SemesterName");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CourseId,CourseCode,CourseName,CourseDescription,CourseType,Credits,DepartmentId")] CourseVm model)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in model.DepartmentId)
                {
                    var course = new Course
                    {
                        CourseCode = model.CourseCode,
                        CourseName = model.CourseName,
                        CourseDescription = model.CourseDescription,
                        Credits = model.Credits,
                        //SemesterId = model.SemesterId,
                        DepartmentId = item
                    };
                    _db.Courses.Add(course);
                }

                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DeptName", model.DepartmentId);
            // ViewBag.SemesterId = new SelectList(db.Semesters, "SemesterId", "SemesterName", model.SemesterId);
            return View(model);
        }

        // GET: Courses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = await _db.Courses.FindAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DeptCode", course.DepartmentId);
            //ViewBag.SemesterId = new SelectList(db.Semesters, "SemesterId", "SemesterName", course.SemesterId);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CourseId,CourseCode,CourseName,CourseDescription,CourseType,Credits,SemesterId,DepartmentId")] Course course)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(course).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DeptCode", course.DepartmentId);
            // ViewBag.SemesterId = new SelectList(db.Semesters, "SemesterId", "SemesterName", course.SemesterId);
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = await _db.Courses.FindAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Course course = await _db.Courses.FindAsync(id);
            _db.Courses.Remove(course);
            await _db.SaveChangesAsync();
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
