using CompunetCbte.Models;
using CompunetCbte.Services;
using ExamSolutionModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OfficeOpenXml;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CompunetCbte.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly OnlineCbte _db;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
            _db = new OnlineCbte();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            _db = new OnlineCbte();
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        public ActionResult LoginUsers()
        {
            //var users = await _db.Students.Where(x => x.IsLogin.Equals(true)).ToListAsync();
            //ViewBag.LoginUserNumber = users.Count();
            //return View(users);
            return View();
        }
        [AllowAnonymous]
        public ActionResult GetLoginUsers()
        {
            // dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            var data = _db.Students.AsNoTracking().Where(x => x.IsLogin.Equals(true)).ToList()
                .Select(s => new { s.StudentId, s.FullName, s.Department.DeptName });
            //ViewBag.LoginUserNumber = data.Count();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);

        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(c => c.UserName.Equals(model.Email));
            var users = _db.Students.FirstOrDefault(x => x.StudentId.Equals(model.Email));
            if (users != null && users.IsLogin == true)
            {
                ModelState.AddModelError("", @"User is alredy Login, Please check you Student ID and try again");
                TempData["UserMessage"] = $"Login Fails..., Please Check your UserName and Password Or Click on ForgetPassword";
                TempData["Title"] = "Error.";
                return View(model);
            }
            if (user == null)
            {
                ModelState.AddModelError("", @"User doesn't Exist.");
                TempData["UserMessage"] = $"Login Fails..., Please Check your UserName and Password Or Click on ForgetPassword";
                TempData["Title"] = "Error.";
                return View(model);
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            //var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            var result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("CustomDashborad", new { username = user.UserName });
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        public async Task<ActionResult> CustomDashborad(string username)
        {


            if (User.IsInRole(RoleName.Admin))
            {
                TempData["UserMessage"] = $"Login Successful, Welcome {username}";
                TempData["Title"] = "Success.";
                return RedirectToAction("DashBoard", "Home");
                // return RedirectToAction("AdminDashboard", "Home");
            }

            if (User.IsInRole(RoleName.Student))
            {
                var model = await _db.Students.Where(x => x.StudentId.Equals(username)).FirstOrDefaultAsync();
                model.IsLogin = true;
                _db.Entry(model).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                //IdentityResult result = await UserManager.UpdateAsync(model);
                TempData["UserMessage"] = $"Login Successful, Welcome {username}";
                TempData["Title"] = "Success.";
                return RedirectToAction("SelectExamIndex", "TakeExam");
            }

            //if (User.IsInRole(RoleName.SuperAdmin))
            //{
            //    TempData["UserMessage"] = $"Login Successful, Welcome {username}";
            //    TempData["Title"] = "Success.";
            //    return RedirectToAction("SuperAdminDashBoard", "Students");
            //}
            return RedirectToAction("Index", "Home");
        }

        //// GET: /Account/Register
        //[AllowAnonymous]
        //public ActionResult ExamRegistration()
        //{
        //    return View();
        //}

        ////
        //// POST: /Account/Register
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> AddmissionRegistration(AddmisionRegistrationVm model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUser { UserName = $"{model.LastName} {model.FirstName}", Email = model.Email };
        //        var result = await UserManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {

        //        }
        //        AddErrors(result);
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        // GET: Students/Create
        public ActionResult CreateStudent()
        {
            ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DeptName");
            var mygender = from Gender s in Enum.GetValues(typeof(Gender))
                           select new { ID = s, Name = s.ToString() };

            ViewBag.Gender = new SelectList(mygender, "Name", "Name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                _db.Students.Add(student);
                await _db.SaveChangesAsync();
                var user = new ApplicationUser { UserName = student.StudentId, Email = student.Email, PhoneNumber = student.PhoneNumber };
                var result = await UserManager.CreateAsync(user, student.Password);
                if (result.Succeeded)
                {
                    await this.UserManager.AddToRoleAsync(user.Id, "Student");
                }
                return RedirectToAction("Index", "Students");
            }
            var mygender = from Gender s in Enum.GetValues(typeof(Gender))
                           select new { ID = s, Name = s.ToString() };

            ViewBag.Gender = new SelectList(mygender, "Name", "Name");
            ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DeptName", student.DepartmentId);
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<ActionResult> EditStudent(string id)
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
            var mygender = from Gender s in Enum.GetValues(typeof(Gender))
                           select new { ID = s, Name = s.ToString() };

            ViewBag.Gender = new SelectList(mygender, "Name", "Name");
            ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DeptName", student.DepartmentId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(student).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Students");
            }
            var mygender = from Gender s in Enum.GetValues(typeof(Gender))
                           select new { ID = s, Name = s.ToString() };

            ViewBag.Gender = new SelectList(mygender, "Name", "Name");
            ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DeptName", student.DepartmentId);
            return View(student);
        }


        [AllowAnonymous]
        public ActionResult UploadStudent()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> UploadStudent(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please Select a excel file <br/>";
                TempData["UserMessage"] = "Please Select a excel file.";
                TempData["Title"] = "Error.";

                return View("UploadStudent");
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
                    int requiredField = 9;

                    //string validCheck = myExcel.ValidateExcel(noOfRow, workSheet, requiredField);
                    //if (!validCheck.Equals("Success"))
                    //{
                    //    //string row = "";
                    //    //string column = "";
                    //    string[] ssizes = validCheck.Split(' ');
                    //    string[] myArray = new string[2];
                    //    for (int i = 0; i < ssizes.Length; i++)
                    //    {
                    //        myArray[i] = ssizes[i];
                    //        // myArray[i] = ssizes[];
                    //    }
                    //    string lineError = $"Line/Row number {myArray[0]}  and column {myArray[1]} is not rightly formatted, Please Check for anomalies ";
                    //    //ViewBag.LineError = lineError;
                    //    TempData["UserMessage"] = lineError;
                    //    TempData["Title"] = "Error.";
                    //    return View();
                    //}
                    for (int row = 2; row <= noOfRow; row++)
                    {
                        var regNo = workSheet.Cells[row, 3].Value.ToString().Trim();
                        //var pic = workSheet.Drawings[studentId] as ExcelPicture;

                        string code = workSheet.Cells[row, 8].Value.ToString().Trim();
                        var deptCode = await _db.Departments.AsNoTracking()
                            .Where(x => x.DeptName.ToUpper().Equals(code.ToUpper()))
                            .FirstOrDefaultAsync();
                        if (deptCode == null)
                        {
                            TempData["UserMessage"] = "The Department code in the excel doesn't exist";
                            TempData["Title"] = "Error.";
                            RedirectToAction("UploadStudent");
                        }
                        try
                        {
                            //ExcelPicture picture = workSheet.Drawings;
                            var student = new Student()
                            {
                                FullName = workSheet.Cells[row, 1].Value.ToString().Trim(),
                                Gender = workSheet.Cells[row, 2].Value.ToString().Trim(),
                                StudentId = regNo,
                                JambRegNo = workSheet.Cells[row, 4].Value.ToString().Trim(),
                                LGA = workSheet.Cells[row, 5].Value.ToString().Trim(),
                                State = workSheet.Cells[row, 6].Value.ToString().Trim(),
                                PhoneNumber = workSheet.Cells[row, 7].Value.ToString().Trim(),
                                DepartmentId = deptCode.DepartmentId,
                                Password = workSheet.Cells[row, 9].Value.ToString().Trim(),
                            };

                            _db.Students.Add(student);
                            recordCount++;
                            lastrecord = $"The last Updated record has the Name {student.FullName} with Student Id {student.StudentId}";

                        }
                        catch (Exception ex)
                        {
                            ViewBag.ErrorInfo = "The image not found";
                            ViewBag.ErrorMessage = ex.Message;
                            return View("ErrorException");
                        }
                    }
                    await _db.SaveChangesAsync();

                    for (int row = 2; row <= noOfRow; row++)
                    {
                        try
                        {
                            var studentId = workSheet.Cells[row, 3].Value.ToString().Trim();
                            var fullName = workSheet.Cells[row, 1].Value.ToString().Trim();
                            //var email = workSheet.Cells[row, 5].Value.ToString().Trim();
                            var phoneNumber = workSheet.Cells[row, 6].Value.ToString().Trim();
                            var password = workSheet.Cells[row, 8].Value.ToString().Trim();
                            var user = new ApplicationUser
                            {
                                UserName = studentId,
                                //Email = email,
                                PhoneNumber = phoneNumber,
                                FullName = fullName

                            };
                            var result = await UserManager.CreateAsync(user, password);
                            if (result.Succeeded)
                            {
                                await this.UserManager.AddToRoleAsync(user.Id, "Student");
                            }

                        }
                        catch (Exception ex)
                        {
                            ViewBag.ErrorInfo = "The image not found";
                            ViewBag.ErrorMessage = ex.Message;
                            return View("ErrorException");
                        }
                    }


                    message = $"You have successfully Uploaded {recordCount} records...  and {lastrecord}";
                    TempData["UserMessage"] = message;
                    TempData["Title"] = "Success.";

                }
                return RedirectToAction("Index", "Students");
            }

            ViewBag.Error = $"File type is Incorrect <br/>";
            return RedirectToAction("UploadStudent", "Account");
        }

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> LogOff()
        {
            string username = User.Identity.GetUserName();
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            var model = await _db.Students.Where(x => x.StudentId.Equals(username)).FirstOrDefaultAsync();
            model.IsLogin = false;
            _db.Entry(model).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        // GET: Students/Delete/5
        public async Task<ActionResult> DeleteStudent(string id)
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
        [HttpPost, ActionName("DeleteStudent")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Student student = await _db.Students.FindAsync(id);
            if (student != null) _db.Students.Remove(student);
            //await _db.SaveChangesAsync();
            var user = await _db.Users.FirstOrDefaultAsync(c => c.UserName.Equals(id));
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Students");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }

            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}