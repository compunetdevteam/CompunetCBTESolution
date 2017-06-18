
using System;
using SwiftKampusModel.CBTE;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CompunetCbte.Models;
using CompunetCbte.Services;
using CompunetCbte.ViewModels.CBTE;
using ExamSolutionModel;
using ExamSolutionModel.CBTE;
using OfficeOpenXml;

namespace SwiftKampus.Controllers
{
    public class QuestionAnswersController : Controller
    {
        private readonly OnlineCbte _db = new OnlineCbte();

        // GET: QuestionAnswers
        public async Task<ActionResult> Index()
        {
            var questionAnswers = _db.QuestionAnswers.AsNoTracking().Include(q => q.Course);
            return View(await questionAnswers.ToListAsync());
        }

        // GET: QuestionAnswers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionAnswer questionAnswer = await _db.QuestionAnswers.FindAsync(id);
            if (questionAnswer == null)
            {
                return HttpNotFound();
            }
            return View(questionAnswer);
        }

        // GET: QuestionAnswers/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(_db.Courses.AsNoTracking(), "CourseId", "CourseCode");
            ViewBag.ExamTypeId = new SelectList(_db.ExamTypes.AsNoTracking(), "ExamTypeId", "ExamName");
            //var questionType = from QuestionType s in Enum.GetValues(typeof(QuestionType))
            //    select new { ID = s, Name = s.ToString() };
           
            //ViewBag.QuestionType = new SelectList(questionType, "Name", "Name");
          
            return View();
        }

        // POST: QuestionAnswers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(QuestionAnswerVm model)
        {
            if (ModelState.IsValid)
            {
                var questionAnswer = new QuestionAnswer
                {
                    CourseId = model.CourseId,
                    LevelId = model.LevelId,
                    ExamTypeId = model.ExamTypeId,
                    Question = model.Question,
                    Option1 = model.Option1,
                    Option2 = model.Option2,
                    Option3 = model.Option3,
                    Option4 = model.Option4,
                    Answer = model.Answer,
                    QuestionHint = model.QuestionHint,
                    QuestionType = model.QuestionType.ToString(),

                };
                _db.QuestionAnswers.Add(questionAnswer);
                await _db.SaveChangesAsync();
                TempData["UserMessage"] = "Question is Added Successfully.";
                TempData["Title"] = "Success.";

                return RedirectToAction("Create");

            }

            ViewBag.CourseId = new SelectList(_db.Courses.AsNoTracking(), "CourseId", "CourseCode", model.CourseId);
            ViewBag.ExamTypeId = new SelectList(_db.ExamTypes.AsNoTracking(), "ExamTypeId", "ExamName");
            return View(model);
        }

        // GET: QuestionAnswers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionAnswer questionAnswer = await _db.QuestionAnswers.FindAsync(id);
            if (questionAnswer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(_db.Courses.AsNoTracking(), "CourseId", "CourseCode", questionAnswer.CourseId);
            ViewBag.ExamTypeId = new SelectList(_db.ExamTypes.AsNoTracking(), "ExamTypeId", "ExamName");
            var model = new QuestionAnswerVm()
            {
                QuestionAnswerId = questionAnswer.QuestionAnswerId,
                Question = questionAnswer.Question,
                Option1 = questionAnswer.Option1,
                Option2 = questionAnswer.Option2,
                Option3 = questionAnswer.Option3,
                Option4 = questionAnswer.Option4,
                Answer = questionAnswer.Answer,
                QuestionHint = questionAnswer.QuestionHint
            };
            return View(model);
        }

        // POST: QuestionAnswers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(QuestionAnswerVm model)
        {
            if (ModelState.IsValid)
            {
                var questionAnswer = new QuestionAnswer
                {
                    QuestionAnswerId = model.QuestionAnswerId,
                    CourseId = model.CourseId,
                    ExamTypeId = model.ExamTypeId,
                    LevelId = model.LevelId,
                    Question = model.Question,
                    Option1 = model.Option1,
                    Option2 = model.Option2,
                    Option3 = model.Option3,
                    Option4 = model.Option4,
                    Answer = model.Answer,
                    QuestionHint = model.QuestionHint,
                    QuestionType = model.QuestionType.ToString(),

                };
                _db.Entry(questionAnswer).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                TempData["UserMessage"] = "Question is Updated Successfully.";
                TempData["Title"] = "Success.";

                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(_db.Courses.AsNoTracking(), "CourseId", "CourseCode", model.CourseId);
            ViewBag.ExamTypeId = new SelectList(_db.ExamTypes.AsNoTracking(), "ExamTypeId", "ExamName");
            return View(model);
        }

        // GET: QuestionAnswers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionAnswer questionAnswer = await _db.QuestionAnswers.FindAsync(id);
            if (questionAnswer == null)
            {
                return HttpNotFound();
            }
            return View(questionAnswer);
        }

        // POST: QuestionAnswers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            QuestionAnswer questionAnswer = await _db.QuestionAnswers.FindAsync(id);
            if (questionAnswer != null) _db.QuestionAnswers.Remove(questionAnswer);
            await _db.SaveChangesAsync();
            TempData["UserMessage"] = "Question is Deleted Successfully.";
            TempData["Title"] = "Error.";

            return RedirectToAction("Index");
        }

        public ActionResult UploadQuestion()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> UploadQuestion(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please Select a excel file <br/>";
                TempData["UserMessage"] = "Please Select a excel file.";
                TempData["Title"] = "Error.";

                return View("Index");
            }
            HttpPostedFileBase file = Request.Files["excelfile"];
            if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
            {
                string lastrecord = "";
                int recordCount = 0;
                string message = "";
                string fileContentType = file.ContentType;
                byte[] fileBytes = new byte[file.ContentLength];
                var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                // Read data from excel file
                using (var package = new ExcelPackage(file.InputStream))
                {
                    ExcelValidation myExcel = new ExcelValidation();
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;
                    int requiredField = 10;

                    string validCheck = myExcel.ValidateExcel(noOfRow, workSheet, requiredField);
                    if (!validCheck.Equals("Success"))
                    {
                        //string row = "";
                        //string column = "";
                        string[] ssizes = validCheck.Split(' ');
                        string[] myArray = new string[2];
                        for (int i = 0; i < ssizes.Length; i++)
                        {
                            myArray[i] = ssizes[i];
                            // myArray[i] = ssizes[];
                        }
                        string lineError = $"Line/Row number {myArray[0]}  and column {myArray[1]} is not rightly formatted, Please Check for anomalies ";
                        //ViewBag.LineError = lineError;
                        TempData["UserMessage"] = lineError;
                        TempData["Title"] = "Error.";
                        return View();
                    }
                    for (int row = 2; row <= noOfRow; row++)
                    {
                        string questionType = workSheet.Cells[row, 9].Value.ToString().Trim();
                        string code = workSheet.Cells[row, 1].Value.ToString().Trim();
                        string exam = workSheet.Cells[row, 2].Value.ToString().Trim().ToUpper();
                        var courseCode = await _db.Courses.AsNoTracking()
                            .Where(x => x.CourseCode.ToUpper().Equals(code.ToUpper()))
                            .FirstOrDefaultAsync();
                        var examType = await _db.ExamTypes.Where(x => x.ExamName.ToUpper().Equals(exam))
                                            .FirstOrDefaultAsync();
                        if (courseCode == null  && examType == null)
                        {
                            TempData["UserMessage"] = "The Department code or Exam Type in the excel doesn't exist";
                            TempData["Title"] = "Error.";
                            RedirectToAction("UploadQuestion");
                        }
                        try
                        {

                            var questionAnswer = new QuestionAnswer
                            {
                                CourseId = courseCode.CourseId,
                                ExamTypeId = examType.ExamTypeId,
                                Question = workSheet.Cells[row, 3].Value.ToString().Trim(),
                                Option1 = workSheet.Cells[row, 4].Value.ToString().Trim(),
                                Option2 = workSheet.Cells[row, 5].Value.ToString().Trim(),
                                Option3 = workSheet.Cells[row, 6].Value.ToString().Trim(),
                                Option4 = workSheet.Cells[row, 7].Value.ToString().Trim(),
                                Answer = workSheet.Cells[row, 8].Value.ToString().Trim(),
                                QuestionType = questionType,
                                QuestionHint = workSheet.Cells[row, 10].Value.ToString().Trim(),

                            };
                            _db.QuestionAnswers.Add(questionAnswer);
                           
                            recordCount++;
                            lastrecord = $"The last Updated record has the Course  {courseCode.CourseName} and Question Type is {questionType}";

                        }
                        catch (Exception ex)
                        {
                            ViewBag.ErrorInfo = "The Department code in the excel doesn't exist";
                            ViewBag.ErrorMessage = ex.Message;
                            return View("ErrorException");
                        }
                    }
                    await _db.SaveChangesAsync();
                    message = $"You have successfully Uploaded {recordCount} records...  and {lastrecord}";
                    TempData["UserMessage"] = message;
                    TempData["Title"] = "Success.";

                }
                return RedirectToAction("Index", "Students");
            }

            ViewBag.Error = $"File type is Incorrect <br/>";
            return View("Index");
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
