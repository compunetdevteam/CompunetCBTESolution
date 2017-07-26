using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Web;

namespace ExamSolutionModel
{
    public abstract class Person
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        public string Gender { get; set; }
        public string LGA { get; set; }
        public string State { get; set; }

        // [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [DataType(DataType.PhoneNumber)]
        //[Required(ErrorMessage = "Your Phone Number is required")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }



        public byte[] Passport { get; set; }

        [Display(Name = "Upload A Passport/Picture")]
        [ValidateFile(ErrorMessage = "Please select a PNG/JPEG image smaller than 20kb")]
        [NotMapped]
        public HttpPostedFileBase File
        {
            get
            {
                return null;
            }

            set
            {
                try
                {
                    MemoryStream target = new MemoryStream();

                    if (value.InputStream == null)
                        return;

                    value.InputStream.CopyTo(target);
                    Passport = target.ToArray();
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }
        }

    }
}

