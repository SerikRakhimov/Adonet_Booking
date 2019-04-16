using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Models
{
    public class User
    {
        public int Id { get; set; }              // уникальный код
        public string Login { get; set; }        // имя для входа
        public string Password { get; set; }     // пароль
        public string PhoneNumber { get; set; }  // номер телефона

        public User()  // конструктор класса по умолчанию
        {
            Id = 0;
            Login = "";
            Password = "";
            PhoneNumber = "";
        }

        public bool NoNullId()  // проверка "Id != 0"
        {
            return Id != 0;
        }
    }
}
