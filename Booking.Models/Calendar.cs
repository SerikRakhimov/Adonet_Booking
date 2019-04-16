using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Models
{
    public class Calendar
    {
        public int Id { get; set; }             // уникальный код
        public DateTime DateIn { get; set; }    // дата заезда
        public DateTime DateOut { get; set; }   // дата выезда
        public int RoomId { get; set; }         // код комнаты
        public int Price { get; set; }          // стоимость номера
        public int Rezerv { get; set; }         // 1 - забронировано, 0 - незабронировано
        public int Pay { get; set; }            // 1 - оплачено, 0 - неоплачено
        public int UserId { get; set; }         // код пользователя, выполнивший бронирование и /или оплату
        public string CardNumber { get; set; }  // номер банковской карты

        public Calendar()  // конструктор класса по умолчанию
        {
            Id = 0;
            DateIn = new DateTime();
            DateOut = new DateTime();
            RoomId = 0;
            Price = 0;
            Rezerv = 0;
            Pay = 0;
            UserId = 0;
            CardNumber = "";
        }

        public bool NoNullId()  // проверка "Id != 0"
        {
            return Id != 0;
        }

    }
}
