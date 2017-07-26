using CompunetCbte.Models;
using ExamSolutionModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CompunetCbte.Controllers
{
    public class StudentsController : Controller
    {
        private readonly OnlineCbte _db;

        public StudentsController()
        {
            _db = new OnlineCbte();
        }

        // GET: Students
        public async Task<ActionResult> Index()
        {
            var students = _db.Students.Include(s => s.Department);
            return View(await students.ToListAsync());
        }
        public ActionResult GetData()
        {
            // dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            var data = _db.Students.Select(s => new { s.FullName, s.StudentId, s.Department.DeptName, s.Gender, s.PhoneNumber, s.Password }).ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);

        }
        // GET: Students/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await _db.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DeptName");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _db.Students.Add(student);
                await _db.SaveChangesAsync();
                var store = new UserStore<ApplicationUser>(_db);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = student.FullName, Email = student.Email, EmailConfirmed = true };

                manager.Create(user, student.Password);
                manager.AddToRole(user.Id, "Student");
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DeptName", student.DepartmentId);
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await _db.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DeptName", student.DepartmentId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(student).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DeptName", student.DepartmentId);
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await _db.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Student student = await _db.Students.FindAsync(id);
            _db.Students.Remove(student);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> RenderImage(string studentId)
        {
            Student student = await _db.Students.FindAsync(studentId);

            byte[] photoBack = student.Passport;

            return File(photoBack, "image/png");
        }

        //public ActionResult UploadStudent()
        //{
        //    return View();
        //}

        //[AllowAnonymous]
        //[HttpPost]
        //public async Task<ActionResult> UploadStudent(HttpPostedFileBase excelfile)
        //{
        //    if (excelfile == null || excelfile.ContentLength == 0)
        //    {
        //        ViewBag.Error = "Please Select a excel file <br/>";
        //        TempData["UserMessage"] = "Please Select a excel file.";
        //        TempData["Title"] = "Error.";

        //        return View("Index");
        //    }
        //    HttpPostedFileBase file = Request.Files["excelfile"];
        //    if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
        //    {
        //        string lastrecord = "";
        //        int recordCount = 0;
        //        string message = "";
        //        string fileContentType = file.ContentType;
        //        byte[] fileBytes = new byte[file.ContentLength];
        //        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

        //        // Read data from excel file
        //        using (var package = new ExcelPackage(file.InputStream))
        //        {
        //            ExcelValidation myExcel = new ExcelValidation();
        //            var currentSheet = package.Workbook.Worksheets;
        //            var workSheet = currentSheet.First();
        //            var noOfCol = workSheet.Dimension.End.Column;
        //            var noOfRow = workSheet.Dimension.End.Row;
        //            int requiredField = 10;

        //            //string validCheck = myExcel.ValidateExcel(noOfRow, workSheet, requiredField);
        //            //if (!validCheck.Equals("Success"))
        //            //{
        //            //    //string row = "";
        //            //    //string column = "";
        //            //    string[] ssizes = validCheck.Split(' ');
        //            //    string[] myArray = new string[2];
        //            //    for (int i = 0; i < ssizes.Length; i++)
        //            //    {
        //            //        myArray[i] = ssizes[i];
        //            //        // myArray[i] = ssizes[];
        //            //    }
        //            //    string lineError = $"Line/Row number {myArray[0]}  and column {myArray[1]} is not rightly formatted, Please Check for anomalies ";
        //            //    //ViewBag.LineError = lineError;
        //            //    TempData["UserMessage"] = lineError;
        //            //    TempData["Title"] = "Error.";
        //            //    return View();
        //            //}
        //            for (int row = 2; row <= noOfRow; row++)
        //            {
        //                var studentId = workSheet.Cells[row, 1].Value.ToString().Trim();
        //                var pic = workSheet.Drawings[studentId] as ExcelPicture;

        //                string code = workSheet.Cells[row, 9].Value.ToString().Trim();
        //                var deptCode = await db.Departments.AsNoTracking()
        //                    .Where(x => x.DeptCode.ToUpper().Equals(code.ToUpper()))
        //                    .FirstOrDefaultAsync();
        //                if (deptCode == null)
        //                {
        //                    TempData["UserMessage"] = "The Department code in the excel doesn't exist";
        //                    TempData["Title"] = "Error.";
        //                    RedirectToAction("UploadStudent");
        //                }
        //                try
        //                {
        //                    //ExcelPicture picture = workSheet.Drawings;
        //                    var student = new Student()
        //                    {
        //                        StudentId = studentId,
        //                        FirstName = workSheet.Cells[row, 2].Value.ToString().Trim(),
        //                        MiddleName = workSheet.Cells[row, 3].Value.ToString().Trim(),
        //                        LastName = workSheet.Cells[row, 4].Value.ToString().Trim(),
        //                        Email = workSheet.Cells[row, 5].Value.ToString().Trim(),
        //                        PhoneNumber = workSheet.Cells[row, 6].Value.ToString().Trim(),
        //                        Gender = workSheet.Cells[row, 7].Value.ToString().Trim(),
        //                        Password = workSheet.Cells[row, 8].Value.ToString().Trim(),
        //                        DepartmentId = deptCode.DepartmentId,
        //                        Passport = ImageToByteArray(pic.Image),
        //                    };

        //                    db.Students.Add(student);
        //                    recordCount++;
        //                    lastrecord = $"The last Updated record has the Last Name {student.LastName} and First Name {student.FirstName} with Student Id {student.StudentId}";

        //                }
        //                catch (Exception ex)
        //                {
        //                    ViewBag.ErrorInfo = "The image not found";
        //                    ViewBag.ErrorMessage = ex.Message;
        //                    return View("ErrorException");
        //                }
        //            }
        //            for (int row = 2; row <= noOfRow; row++)
        //            {
        //                try
        //                {
        //                    var studentId = workSheet.Cells[row, 1].Value.ToString().Trim();
        //                    var email = workSheet.Cells[row, 5].Value.ToString().Trim();
        //                    var phoneNumber = workSheet.Cells[row, 6].Value.ToString().Trim();
        //                    var password = workSheet.Cells[row, 8].Value.ToString().Trim();


        //                    var store = new UserStore<ApplicationUser>(db);
        //                    var manager = new UserManager<ApplicationUser>(store);
        //                    var user = new ApplicationUser { Id=studentId, UserName = studentId, Email = email, PhoneNumber = phoneNumber, EmailConfirmed = true };

        //                    manager.Create(user, password);
        //                    manager.AddToRole(user.Id, "Student");

        //                }
        //                catch (Exception ex)
        //                {
        //                    ViewBag.ErrorInfo = "The image not found";
        //                    ViewBag.ErrorMessage = ex.Message;
        //                    return View("ErrorException");
        //                }
        //            }

        //            await db.SaveChangesAsync();
        //            message = $"You have successfully Uploaded {recordCount} records...  and {lastrecord}";
        //            TempData["UserMessage"] = message;
        //            TempData["Title"] = "Success.";

        //        }
        //        return RedirectToAction("Index", "Students");
        //    }

        //    ViewBag.Error = $"File type is Incorrect <br/>";
        //    return View("Index");
        //}



        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
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
