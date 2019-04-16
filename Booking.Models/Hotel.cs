using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Models
{
    public class Hotel
    {
        public int Id { get; set; }       // уникальный код
        public string Name { get; set; }  // наименование

        public Hotel()  // конструктор класса по умолчанию
        {
            Id = 0;
            Name = "";
        }

        public bool NoNullId()  // проверка "Id != 0"
        {
            return Id != 0;
        }

    }
}
