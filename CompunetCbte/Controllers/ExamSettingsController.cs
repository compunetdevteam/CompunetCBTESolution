﻿
using SwiftKampusModel.CBTE;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using CompunetCbte.Models;
using ExamSolutionModel.CBTE;

namespace SwiftKampus.Controllers
{
    public class ExamSettingsController : Controller
    {
        private readonly OnlineCbte _db = new OnlineCbte();

        // GET: ExamSettings
        public async Task<ActionResult> Index()
        {
            var examSettings = _db.ExamSettings.AsNoTracking().Include(e => e.Course).Include(e => e.ExamType)
                .Include(e => e.Semester).Include(e => e.Sessions);
            return View(await examSettings.ToListAsync());
        }

        // GET: ExamSettings/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamSetting examSetting = await _db.ExamSettings.FindAsync(id);
            if (examSetting == null)
            {
                return HttpNotFound();
            }
            return View(examSetting);
        }

        // GET: ExamSettings/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(_db.Courses.AsNoTracking(), "CourseId", "CourseCode");
            ViewBag.ExamTypeId = new SelectList(_db.ExamTypes.AsNoTracking(), "ExamTypeId", "ExamName");
            ViewBag.SemesterId = new SelectList(_db.Semesters.AsNoTracking(), "SemesterId", "SemesterName");
            ViewBag.SessionId = new SelectList(_db.Sessions.AsNoTracking(), "SessionId", "SessionName");
            return View();
        }

        // POST: ExamSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ExamSettingId,CourseId,LevelId,SemesterId,SessionId,ExamDate,EndDate,ExamTypeId,ExamStartTime,ExamEndTime")] ExamSetting examSetting)
        {
            if (ModelState.IsValid)
            {
                var subjectName = await _db.Courses.AsNoTracking().Where(x => x.CourseId.Equals(examSetting.CourseId))
                                        .FirstOrDefaultAsync();
                examSetting.SubjectName = subjectName.CourseName;
                _db.ExamSettings.Add(examSetting);
                await _db.SaveChangesAsync();
                TempData["UserMessage"] = "Exam Settings Added Successfully.";
                TempData["Title"] = "Success.";

                return RedirectToAction("Create");
            }

            ViewBag.CourseId = new SelectList(_db.Courses.AsNoTracking(), "CourseId", "CourseCode", examSetting.CourseId);
            ViewBag.ExamTypeId = new SelectList(_db.ExamTypes.AsNoTracking(), "ExamTypeId", "ExamName", examSetting.ExamTypeId);
            ViewBag.SemesterId = new SelectList(_db.Semesters.AsNoTracking(), "SemesterId", "SemesterName", examSetting.SemesterId);
            ViewBag.SessionId = new SelectList(_db.Sessions.AsNoTracking(), "SessionId", "SessionName", examSetting.SessionId);
            return View(examSetting);
        }

        // GET: ExamSettings/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamSetting examSetting = await _db.ExamSettings.FindAsync(id);
            if (examSetting == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(_db.Courses.AsNoTracking(), "CourseId", "CourseCode", examSetting.CourseId);
            ViewBag.ExamTypeId = new SelectList(_db.ExamTypes.AsNoTracking(), "ExamTypeId", "ExamName", examSetting.ExamTypeId);
            ViewBag.SemesterId = new SelectList(_db.Semesters.AsNoTracking(), "SemesterId", "SemesterName", examSetting.SemesterId);
            ViewBag.SessionId = new SelectList(_db.Sessions.AsNoTracking(), "SessionId", "SessionName", examSetting.SessionId);
            return View(examSetting);
        }

        // POST: ExamSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ExamSettingId,CourseId,LevelId,SemesterId,SessionId,ExamDate,EndDate,ExamTypeId,ExamStartTime,ExamEndTime")] ExamSetting examSetting)
        {
            if (ModelState.IsValid)
            {
                var subjectName = await _db.Courses.AsNoTracking().Where(x => x.CourseId.Equals(examSetting.CourseId))
                    .FirstOrDefaultAsync();
                examSetting.SubjectName = subjectName.CourseName;
                _db.Entry(examSetting).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                TempData["UserMessage"] = "Exam Settings Updated Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(_db.Courses.AsNoTracking(), "CourseId", "CourseCode", examSetting.CourseId);
            ViewBag.ExamTypeId = new SelectList(_db.ExamTypes.AsNoTracking(), "ExamTypeId", "ExamName", examSetting.ExamTypeId);
            ViewBag.SemesterId = new SelectList(_db.Semesters.AsNoTracking(), "SemesterId", "SemesterName", examSetting.SemesterId);
            ViewBag.SessionId = new SelectList(_db.Sessions.AsNoTracking(), "SessionId", "SessionName", examSetting.SessionId);
            return View(examSetting);
        }

        // GET: ExamSettings/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamSetting examSetting = await _db.ExamSettings.FindAsync(id);
            if (examSetting == null)
            {
                return HttpNotFound();
            }
            return View(examSetting);
        }

        // POST: ExamSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ExamSetting examSetting = await _db.ExamSettings.FindAsync(id);
            if (examSetting != null) _db.ExamSettings.Remove(examSetting);
            await _db.SaveChangesAsync();
            TempData["UserMessage"] = "Exam Setting is Deleted Successfully.";
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
