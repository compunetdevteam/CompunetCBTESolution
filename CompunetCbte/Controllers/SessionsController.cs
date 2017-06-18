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
    public class SessionsController : Controller
    {
        private OnlineCbte db = new OnlineCbte();

        // GET: Sessions
        public async Task<ActionResult> Index()
        {
            return View(await db.Sessions.AsNoTracking().OrderBy(s => s.SessionName).ToListAsync());
            //var orderedList = cnt.OrderBy(x => x.GroupTypeId).ThenBy(x => x.ContentId);
        }

        // GET: Sessions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = await db.Sessions.FindAsync(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // GET: Sessions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sessions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SessionId,SessionName,StartDate,EndDate,ActiveSession")] Session session)
        {
            if (ModelState.IsValid)
            {
                if (session.ActiveSession.Equals(true))
                {
                    var current = await db.Sessions.AsNoTracking().Where(s => s.ActiveSession.Equals(true))
                        .CountAsync();
                    if (current >= 1)
                    {
                        ViewBag.Message = "You cant have more than ONE active Sessions";
                        return View("ErrorException");
                    }
                }

                session.SessionName = session.SessionName.Trim();
                db.Sessions.Add(session);
                await db.SaveChangesAsync();
                TempData["UserMessage"] = "Session Created Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Create");


            }

            return View(session);
        }

        // GET: Sessions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = await db.Sessions.FindAsync(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SessionId,SessionName,StartDate,EndDate,ActiveSession")] Session session)
        {
            if (ModelState.IsValid)
            {
                if (session.ActiveSession.Equals(true))
                {
                    var current = await db.Sessions.AsNoTracking().Where(s => s.ActiveSession.Equals(true)).CountAsync();
                    if (current >= 1)
                    {
                        ViewBag.Message = "You cant have more than ONE active Sessions";
                        return View("ErrorException");
                    }
                }
                try
                {
                    session.SessionName = session.SessionName.Trim();
                    db.Entry(session).State = System.Data.Entity.EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["UserMessage"] = "Session Updated Successfully.";
                    TempData["Title"] = "Success.";
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ViewBag.ErrorMessage = e.Message;
                    return View("ErrorException");
                }
            }
            return View(session);
        }

        // GET: Sessions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = await db.Sessions.FindAsync(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Session session = await db.Sessions.FindAsync(id);
            db.Sessions.Remove(session);
            await db.SaveChangesAsync();
            TempData["UserMessage"] = "Session Deleted Successfully.";
            TempData["Title"] = "Error.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
