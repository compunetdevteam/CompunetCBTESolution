using CompunetCbte.Models;
using CompunetCbte.ViewModels;
using System;
using System.Configuration;
using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;

namespace CompunetCbte.Controllers
{
    public class HomeController : Controller
    {
        private readonly OnlineCbte _db;

        public HomeController()
        {
            _db = new OnlineCbte();
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SchoolSetUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SchoolSetUp(SetUpVm model)
        {
            string _FileName = String.Empty;
            if (model.File?.ContentLength > 0)
            {
                _FileName = Path.GetFileName(model.File.FileName);
                string _path = HostingEnvironment.MapPath("~/Content/Images/") + _FileName;
                var directory = new DirectoryInfo(HostingEnvironment.MapPath("~/Content/Images/"));
                if (directory.Exists == false)
                {
                    directory.Create();
                }
                model.File.SaveAs(_path);
            }


            //ViewBag.Message = "File upload failed!!";
            //return View(model);

            Configuration objConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            AppSettingsSection objAppsettings = (AppSettingsSection)objConfig.GetSection("appSettings");
            //Edit
            if (objAppsettings != null)
            {
                objAppsettings.Settings["SchoolName"].Value = model.SchoolName;
                objAppsettings.Settings["SchoolTheme"].Value = model.SchoolTheme.ToString();
                if (!String.IsNullOrEmpty(_FileName))
                {
                    objAppsettings.Settings["SchoolImage"].Value = _FileName;
                }
                objConfig.Save();
            }
            return View("Index");
        }


    }
}