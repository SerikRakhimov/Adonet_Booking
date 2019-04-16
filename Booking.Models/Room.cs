using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Models
{
    public class Room
    {
        public int Id { get; set; }       // уникальный код
        public string Name { get; set; }  // наименование
        public int HotelId { get; set; }  // код отеля

        public Room()  // конструктор класса по умолчанию
        {
            Id = 0;
            Name = "";
            HotelId = 0;
        }

        public bool NoNullId()  // проверка "Id != 0"
        {
            return Id != 0;
        }

    }
}
