using CompunetCbte.Models;
using ExamSolutionModel;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CompunetCbte.Controllers
{
    public class DepartmentCoursesController : Controller
    {
        private readonly OnlineCbte _db;

        public DepartmentCoursesController()
        {
            _db = new OnlineCbte();
        }

        // GET: DepartmentCourses
        public async Task<ActionResult> Index()
        {
            var departmentCourses = _db.DepartmentCourses.Include(d => d.Course).Include(d => d.Department);
            return View(await departmentCourses.ToListAsync());
        }

        // GET: DepartmentCourses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepartmentCourse departmentCourse = await _db.DepartmentCourses.FindAsync(id);
            if (departmentCourse == null)
            {
                return HttpNotFound();
            }
            return View(departmentCourse);
        }

        // GET: DepartmentCourses/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new MultiSelectList(_db.Courses, "CourseId", "CourseName");
            ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DeptName");
            return View();
        }

        // POST: DepartmentCourses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DepartmentCourseId,DepartmentId,CourseId")] DepartmentCourseVm model)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in model.CourseId)
                {
                    var deptCourse = new DepartmentCourse
                    {
                        CourseId = item,
                        DepartmentId = model.DepartmentId
                    };
                    _db.DepartmentCourses.Add(deptCourse);
                }
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new MultiSelectList(_db.Courses, "CourseId", "CourseName", model.CourseId);
            ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DeptName", model.DepartmentId);
            return View(model);
        }

        // GET: DepartmentCourses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepartmentCourse departmentCourse = await _db.DepartmentCourses.FindAsync(id);
            if (departmentCourse == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new MultiSelectList(_db.Courses, "CourseId", "CourseName");
            ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DeptName", departmentCourse.DepartmentId);
            return View(departmentCourse);
        }

        // POST: DepartmentCourses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "DepartmentCourseId,DepartmentId,CourseId")] DepartmentCourse departmentCourse)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(departmentCourse).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new MultiSelectList(_db.Courses, "CourseId", "CourseName");
            ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DeptName");
            return View(departmentCourse);
        }

        // GET: DepartmentCourses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepartmentCourse departmentCourse = await _db.DepartmentCourses.FindAsync(id);
            if (departmentCourse == null)
            {
                return HttpNotFound();
            }
            return View(departmentCourse);
        }

        // POST: DepartmentCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DepartmentCourse departmentCourse = await _db.DepartmentCourses.FindAsync(id);
            _db.DepartmentCourses.Remove(departmentCourse);
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
