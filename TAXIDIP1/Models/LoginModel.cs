using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAXIDIP1.Models
{
    public partial class LoginModel //: IValidatableObject
    {
        [Required(ErrorMessage = "Не указан логин")]
        public string Login { get; set; }

        public string valid { get; set; }

       [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        /*
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(this.Login))
            {
                errors.Add(new ValidationResult("Введите Логин!", new List<string>() { "Login" }));
            }
            if (string.IsNullOrWhiteSpace(this.Password))
            {
                errors.Add(new ValidationResult("Введите пароль!", new List<string>() { "Password" }));
            }
            return errors;
        }
        */
    }
}
