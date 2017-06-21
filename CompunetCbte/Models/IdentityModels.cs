using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using ExamSolutionModel;
using ExamSolutionModel.CBTE;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SwiftKampusModel.CBTE;

namespace CompunetCbte.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class OnlineCbte : IdentityDbContext<ApplicationUser>
    {
        public OnlineCbte()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static OnlineCbte Create()
        {
            return new OnlineCbte();
        }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Session> Sessions { get; set; }

        public DbSet<Student> Students { get; set; }
        public DbSet<ExamRule> ExamRules { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public DbSet<StudentQuestion> StudentQuestions { get; set; }

        public DbSet<ExamSetting> ExamSettings { get; set; }

        public DbSet<ExamType> ExamTypes { get; set; }
        public DbSet<ExamLog> ExamLogs { get; set; }
        public DbSet<ExamInstruction> ExamInstructions { get; set; }

        public System.Data.Entity.DbSet<ExamSolutionModel.DepartmentCourse> DepartmentCourses { get; set; }
    }
}