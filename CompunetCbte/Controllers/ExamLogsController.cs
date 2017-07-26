using CompunetCbte.Models;
using ExamSolutionModel.CBTE;
using OfficeOpenXml;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CompunetCbte.Controllers
{
    public class ExamLogsController : Controller
    {
        private readonly OnlineCbte _db;

        public ExamLogsController()
        {
            _db = new OnlineCbte();
        }

        // GET: ExamLogs
        //public async Task<ActionResult> Index(string studentId, string courseId, string levelId,
        //                string semesterid, string sessionid)
        //{
        //    var examLogs = _db.ExamLogs.AsNoTracking().Include(e => e.Course).Include(e => e.ExamType)
        //                    .Include(e => e.Semester).Include(e => e.Sessions)
        //                    .Where(x => x.StudentId.Equals(studentId) && courseId.Equals(courseId) && levelId.Equals(levelId)
        //                    && semesterid.Equals(semesterid) && semesterid.Equals(semesterid));
        //    return View(await examLogs.ToListAsync());
        //}
        public async Task<ActionResult> Index()
        {
            var examLogs = _db.ExamLogs.AsNoTracking().Include(e => e.Course).Include(e => e.ExamType)
                .Include(e => e.Semester).Include(e => e.Sessions);
            return View(await examLogs.ToListAsync());
        }

        // GET: ExamLogs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamLog examLog = await _db.ExamLogs.FindAsync(id);
            if (examLog == null)
            {
                return HttpNotFound();
            }
            return View(examLog);
        }

        // GET: ExamLogs/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(_db.Courses.AsNoTracking(), "CourseId", "CourseCode");
            ViewBag.ExamTypeId = new SelectList(_db.ExamTypes.AsNoTracking(), "ExamTypeId", "ExamTypeId");

            ViewBag.SemesterId = new SelectList(_db.Semesters.AsNoTracking(), "SemesterId", "SemesterName");
            ViewBag.SessionId = new SelectList(_db.Sessions.AsNoTracking(), "SessionId", "SessionName");
            return View();
        }

        // POST: ExamLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ExamLogId,StudentId,CourseId,LevelId,SemesterId,SessionId,ExamTypeId,Score")] ExamLog examLog)
        {
            if (ModelState.IsValid)
            {
                _db.ExamLogs.Add(examLog);
                await _db.SaveChangesAsync();
                return RedirectToAction("Create");
            }

            ViewBag.CourseId = new SelectList(_db.Courses.AsNoTracking(), "CourseId", "CourseCode", examLog.CourseId);
            ViewBag.ExamTypeId = new SelectList(_db.ExamTypes.AsNoTracking(), "ExamTypeId", "ExamTypeId", examLog.ExamTypeId);
            ViewBag.SemesterId = new SelectList(_db.Semesters.AsNoTracking(), "SemesterId", "SemesterName", examLog.SemesterId);
            ViewBag.SessionId = new SelectList(_db.Sessions.AsNoTracking(), "SessionId", "SessionName", examLog.SessionId);
            return View(examLog);
        }

        // GET: ExamLogs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamLog examLog = await _db.ExamLogs.FindAsync(id);
            if (examLog == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(_db.Courses.AsNoTracking(), "CourseId", "CourseCode", examLog.CourseId);
            ViewBag.ExamTypeId = new SelectList(_db.ExamTypes.AsNoTracking(), "ExamTypeId", "ExamTypeId", examLog.ExamTypeId);
            ViewBag.SemesterId = new SelectList(_db.Semesters.AsNoTracking(), "SemesterId", "SemesterName", examLog.SemesterId);
            ViewBag.SessionId = new SelectList(_db.Sessions.AsNoTracking(), "SessionId", "SessionName", examLog.SessionId);
            return View(examLog);
        }

        // POST: ExamLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ExamLogId,StudentId,CourseId,LevelId,SemesterId,SessionId,ExamTypeId,Score")] ExamLog examLog)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(examLog).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(_db.Courses.AsNoTracking(), "CourseId", "CourseCode", examLog.CourseId);
            ViewBag.ExamTypeId = new SelectList(_db.ExamTypes, "ExamTypeId", "ExamTypeId", examLog.ExamTypeId);
            ViewBag.SemesterId = new SelectList(_db.Semesters.AsNoTracking(), "SemesterId", "SemesterName", examLog.SemesterId);
            ViewBag.SessionId = new SelectList(_db.Sessions.AsNoTracking(), "SessionId", "SessionName", examLog.SessionId);
            return View(examLog);
        }

        // GET: ExamLogs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamLog examLog = await _db.ExamLogs.FindAsync(id);
            if (examLog == null)
            {
                return HttpNotFound();
            }
            return View(examLog);
        }

        // POST: ExamLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ExamLog examLog = await _db.ExamLogs.FindAsync(id);
            if (examLog != null) _db.ExamLogs.Remove(examLog);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task DownloadResult()
        {
            //var facilityList = Db.Communications.AsNoTracking().ToList();
            char c1 = 'A';
            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Report");


            worksheet.Cells[$"{c1++}1"].Value = "Exam Log ID";
            worksheet.Cells[$"{c1++}1"].Value = "Full Name";
            worksheet.Cells[$"{c1++}1"].Value = "Reg No";
            worksheet.Cells[$"{c1++}1"].Value = "Subject Name";
            worksheet.Cells[$"{c1++}1"].Value = "Score";


            var results = await _db.ExamLogs.AsNoTracking().OrderBy(o => o.StudentId).ToListAsync();

            int rowStart = 2;
            char c2 = 'A';

            foreach (ExamLog t in results)
            {
                worksheet.Cells[$"A{rowStart}"].Value = t.ExamLogId;
                worksheet.Cells[$"B{rowStart}"].Value = t.Student.FullName;
                worksheet.Cells[$"C{rowStart}"].Value = t.StudentId;
                worksheet.Cells[$"D{rowStart}"].Value = t.Course.CourseName;
                worksheet.Cells[$"E{rowStart}"].Value = t.Score;
                rowStart++;
            }
            // var info = results.FirstOrDefault();
            worksheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + $"PreDegreeExamResult.xlsx");
            Response.BinaryWrite(package.GetAsByteArray());
            Response.End();

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
