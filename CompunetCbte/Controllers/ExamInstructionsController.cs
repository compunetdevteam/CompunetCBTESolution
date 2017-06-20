using CompunetCbte.Models;
using ExamSolutionModel;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CompunetCbte.Controllers
{
    public class ExamInstructionsController : Controller
    {
        private readonly OnlineCbte _db;

        public ExamInstructionsController()
        {
            _db = new OnlineCbte();
        }

        // GET: ExamInstructions
        public async Task<ActionResult> Index()
        {
            var examInstructions = _db.ExamInstructions.Include(e => e.Course);
            return View(await examInstructions.ToListAsync());
        }

        // GET: ExamInstructions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamInstruction examInstruction = await _db.ExamInstructions.FindAsync(id);
            if (examInstruction == null)
            {
                return HttpNotFound();
            }
            return View(examInstruction);
        }

        // GET: ExamInstructions/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "CourseCode");
            return View();
        }

        // POST: ExamInstructions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ExamInstructionId,Instruction,CourseId")] ExamInstruction examInstruction)
        {
            if (ModelState.IsValid)
            {
                _db.ExamInstructions.Add(examInstruction);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "CourseCode", examInstruction.CourseId);
            return View(examInstruction);
        }

        // GET: ExamInstructions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamInstruction examInstruction = await _db.ExamInstructions.FindAsync(id);
            if (examInstruction == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "CourseCode", examInstruction.CourseId);
            return View(examInstruction);
        }

        // POST: ExamInstructions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ExamInstructionId,Instruction,CourseId")] ExamInstruction examInstruction)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(examInstruction).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "CourseCode", examInstruction.CourseId);
            return View(examInstruction);
        }

        // GET: ExamInstructions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamInstruction examInstruction = await _db.ExamInstructions.FindAsync(id);
            if (examInstruction == null)
            {
                return HttpNotFound();
            }
            return View(examInstruction);
        }

        // POST: ExamInstructions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ExamInstruction examInstruction = await _db.ExamInstructions.FindAsync(id);
            if (examInstruction != null) _db.ExamInstructions.Remove(examInstruction);
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