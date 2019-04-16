using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booking.DataAccess;
using Booking.Models;
using Booking.Services;

namespace Booking
{
    class Program
    {
        private static void Main(string[] args)
        {
            string menu;
            User user, user_buffer;          // выбранный и дополнительный пользователи
            Hotel hotel;                     // выбранный отель
            Room room;                       // выбранная комната
            Calendar calendar;               // выбранный календарь
            bool cicle;

            var choice = new Choice();
            Console.WriteLine("\n\t\t Б Р О Н И Р О В А Н И Е   О Т Е Л Е Й");

            user = new User();
            user_buffer = choice.LoginUser();

            cicle = false;

            if (user_buffer.NoNullId())
            {
                cicle = choice.SendSms(user_buffer);
                if (cicle)
                {
                   user = user_buffer;
                }
            }

            while (cicle)
            {
                Console.WriteLine("\n\t~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine($"\tПользователь: {user.Login}");
                Console.WriteLine("\n\t\tГлавное меню:\n");
                Console.WriteLine("\t1 - Забронировать отель");
                Console.WriteLine("\t2 - Сменить пользователя");
                Console.WriteLine("\t0 - Выход\n");
                Console.Write("\tВаше выбор = ");
                menu = Console.ReadLine();

                if (menu == "0")
                {
                    break;
                }
                if (menu == "1")   // забронировать отель
                {
                     hotel = choice.ChoiceHotel(user);
                     if (hotel.NoNullId())
                     {
                         room = choice.ChoiceRoom(user, hotel);
                         if (room.NoNullId())
                         {
                             calendar = choice.ChoiceCalendar(user, hotel, room);
                             if (calendar.NoNullId())
                             {
                                choice.RoomActions(user, hotel, room, calendar);
                             }
                         }
                     }
                }
                if (menu == "2")   // сменить пользователя
                {

                    user_buffer = choice.LoginUser();

                    if (user_buffer.NoNullId())
                    {
                        if (choice.SendSms(user_buffer))
                        {
                            user = user_buffer;
                        }
                    }
                }
            }
        }
    }
}
