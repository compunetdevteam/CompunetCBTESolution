using CompunetCbte.Models;
using CompunetCbte.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CompunetCbte
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            SmsServicesCustom _smsService = new SmsServicesCustom();

            string body = message.Body;
            string destination = message.Destination;
            SMS sms = new SMS()
            {
                SenderId = "SWIFTKAMPUS",
                Message = body,
                Numbers = destination
            };
            bool isSuccess = false;
            string errMsg = null;
            string response = _smsService.Send(sms); //Send sms

            string code = _smsService.GetResponseMessage(response, out isSuccess, out errMsg);

            if (!isSuccess)
            {
                isSuccess = false;
            }
            else
            {
                isSuccess = true;
            }

            return Task.FromResult(true);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<OnlineCbte>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }

    public class CustomSms
    {
        private readonly OnlineCbte _db;

        public CustomSms()
        {
            _db = new OnlineCbte();
        }

        public async Task<Task> SendStudentMsgAsync(SmsToStudent message)
        {
            // Plug in your SMS service here to send a text message.
            SmsServicesCustom _smsService = new SmsServicesCustom();

            string body = message.Body;
            string destination = await _db.Users.AsNoTracking().Where(x => x.UserName.Equals(message.Destination))
                .Select(c => c.PhoneNumber).FirstOrDefaultAsync();
            SMS sms = new SMS()
            {
                SenderId = ConfigurationManager.AppSettings["SchoolName"],
                Message = body,
                Numbers = destination
            };
            bool isSuccess = false;
            string errMsg = null;
            string response = _smsService.Send(sms); //Send sms

            string code = _smsService.GetResponseMessage(response, out isSuccess, out errMsg);

            if (!isSuccess)
            {
                isSuccess = false;
            }
            else
            {
                isSuccess = true;
            }

            return Task.FromResult(true);
        }

        public async Task<Task> SendUnknowMsgAsync(SmsToStudent message)
        {
            // Plug in your SMS service here to send a text message.
            SmsServicesCustom _smsService = new SmsServicesCustom();

            string body = message.Body;

            SMS sms = new SMS()
            {
                SenderId = ConfigurationManager.AppSettings["SchoolName"],
                Message = body,
                Numbers = message.Destination
            };
            bool isSuccess = false;
            string errMsg = null;
            string response = _smsService.Send(sms); //Send sms

            string code = _smsService.GetResponseMessage(response, out isSuccess, out errMsg);

            if (!isSuccess)
            {
                isSuccess = false;
            }
            else
            {
                isSuccess = true;
            }

            return Task.FromResult(true);
        }
    }
}