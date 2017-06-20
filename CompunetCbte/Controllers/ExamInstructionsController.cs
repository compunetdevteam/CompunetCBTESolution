using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CompunetCbte.Models;
using ExamSolutionModel;

namespace CompunetCbte.Controllers
{
    public class ExamInstructionsController : Controller
    {
        private OnlineCbte db = new OnlineCbte();

        // GET: ExamInstructions
        public ActionResult Index()
        {
            return View(db.ExamInstructions.ToList());
        }

        //this page return the Instruction for each exams
        public ActionResult Instruction()
        {
            return View();
        }
        // GET: ExamInstructions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamInstruction examInstruction = db.ExamInstructions.Find(id);
            if (examInstruction == null)
            {
                return HttpNotFound();
            }
            return View(examInstruction);
        }

        // GET: ExamInstructions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExamInstructions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExamInstructionId,Instruction")] ExamInstruction examInstruction)
        {
            if (ModelState.IsValid)
            {
                db.ExamInstructions.Add(examInstruction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(examInstruction);
        }

        // GET: ExamInstructions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamInstruction examInstruction = db.ExamInstructions.Find(id);
            if (examInstruction == null)
            {
                return HttpNotFound();
            }
            return View(examInstruction);
        }

        // POST: ExamInstructions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExamInstructionId,Instruction")] ExamInstruction examInstruction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(examInstruction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(examInstruction);
        }

        // GET: ExamInstructions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamInstruction examInstruction = db.ExamInstructions.Find(id);
            if (examInstruction == null)
            {
                return HttpNotFound();
            }
            return View(examInstruction);
        }

        // POST: ExamInstructions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExamInstruction examInstruction = db.ExamInstructions.Find(id);
            db.ExamInstructions.Remove(examInstruction);
            db.SaveChanges();
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
