using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TAXIDIP1.Models
{
    public class RoleValAttribute : ValidationAttribute
    {
        //массив для хранения допустимых имен
        string[] _roles;

        public RoleValAttribute(string[] roles)
        {
            _roles = roles;
        }
        public override bool IsValid(object value)
        {
            if(value!=null)
            if (_roles.Contains(value.ToString()))
                return true;

            return false;
        }
    }
    public partial class Register
    {
        [Required(ErrorMessage = "Не указан логин")]
        public string Login { get; set; }

        public string valid { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        [RoleVal(new string[]{"driver","client"}, ErrorMessage = "Неверная роль")]
        public string Role { get; set; }

    }
}
