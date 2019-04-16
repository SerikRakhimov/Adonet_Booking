using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booking.DataAccess;
using Booking.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Twilio.TwiML;


namespace Booking.Services
{
    public class Choice
    {

        private string check;
        private int i, number;

        public User LoginUser()
        {
            bool result;
            string login, password, phoneNumberInput, menu;

            User resultUser = new User();  // вызов конструктора нужен, чтобы Id присвоилось 0

            var dataService = new DataService();

            Dictionary<string, User> users;

            while (true)
            {
                users = dataService.GetUsers();

                Console.WriteLine("\n\t~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine("\n\tВвод пользователя:");
                Console.WriteLine("\n\t1 - Регистрация");
                Console.WriteLine("\t2 - Вход");
                Console.WriteLine("\t0 - Выход");
                Console.Write("\n\tВаш выбор = ");
                menu = Console.ReadLine();

                if (menu == "0")
                {
                    break;
                }

                if (menu == "1" || menu == "2")
                {

                    do
                    {
                        Console.Write("\tВаш логин = ");
                        login = Console.ReadLine();
                    } while (login == "");

                    do
                    {
                        Console.Write("\tВаш пароль = ");
                        password = Console.ReadLine();
                    } while (password == "");


                    if (menu == "1") // регистрация
                    {

                        if (users.ContainsKey(login))
                        {
                            Console.WriteLine($"\tТакой пользователь уже зарегистрирован в базе данных.");
                            continue;
                        }
                        else
                        {
                            do
                            {
                                Console.Write("\tВаш номер телефона = ");
                                phoneNumberInput = Console.ReadLine();
                            } while (phoneNumberInput == "");

                            result = dataService.AddUser(login, password, phoneNumberInput);
                            if (result)
                            {
                                users = dataService.GetUsers();
                                resultUser = users[login];
                                break;
                            }
                            else
                            {
                                continue;
                            }
                            
                        }
                    }


                    if (menu == "2") // вход
                    {

                        if (users.ContainsKey(login))
                        {
                            if (password == users[login].Password)
                            {

                                Console.WriteLine($"\tПароль введен правильно.");
                                resultUser = users[login];
                                break;

                            }
                            else
                            {
                                Console.WriteLine($"\tПароль введен неправильно.");
                                continue;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"\tИмя пользователя и пароль введены неправильно.");
                            continue;
                        }

                    }

                }    

            }

            return resultUser;
        }

        public bool SendSms(User user)
        {
            bool result;
            result = false;
            Random rand = new Random();
            //  var accountSid = "AC2ec5f1cd0280d496beef7f07a50a2b89";
            //  var authToken = "ec5f9a40c06da9049d54d4237ed90962";
            var accountSid = "ACfdf00a6ae53bd8984b4f0240c813b547";
            var authToken = "50809289c1e82fc43b8c3a0da8cfdb15";
            string code = Convert.ToString(rand.Next(1000, 9999));
            TwilioClient.Init(accountSid, authToken);
            var to = new PhoneNumber(user.PhoneNumber);
            // var from = new PhoneNumber("+17372270725");
            var from = new PhoneNumber("+12568277590");
            Console.WriteLine("\tПересылка кода на ваш телефон...");
            try
            {
                var message = MessageResource.Create(
                    to: to,
                    from: from,
                    body: code);
                Console.WriteLine($"\t{message.Body}");
                Console.Write("\tВведите свой код: ");
                string checkCode = Console.ReadLine();
                if (checkCode == code)
                {
                    Console.WriteLine("\tКод введен правильно.");
                    result = true;
                }
                else
                {
                    Console.WriteLine("\tКод введен неправильно.");
                    result = false;
                }
            }
            catch (Exception ext)
            {
                Console.WriteLine("\tНе удалось отправить сообщение на ваш телефон.");
                Console.WriteLine($"\tСообщение: {ext.Message}");
                result = true;
            }

            return result;
            }

        public Hotel ChoiceHotel(User user)
            {
            Hotel resultHotel = new Hotel();  // вызов конструктора нужен, чтобы Id присвоилось 0

            var dataService = new DataService();

            List<Hotel> listHotels = dataService.GetHotels();

            Console.WriteLine("\n\t~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine($"\n\tПользователь: {user.Login}");
            if (listHotels.Count == 0)
            {
                Console.WriteLine($"\tСписок отелей пуст.");
            }
            else
            {
                while (true)
                {
                    Console.WriteLine("\n\tСписок отелей:\n");
                    i = 0;
                    foreach (var elem in listHotels)
                    {
                        i++;
                        Console.WriteLine($"\t{i} - {elem.Name}");
                    };

                    Console.WriteLine($"\n\t0 - выход");
                    Console.Write($"\n\tВведите номер отеля (1-{listHotels.Count}) = ");
                    check = Console.ReadLine();

                    try
                    {
                        number = int.Parse(check);
                        if (number == 0)  // выход с программы
                        {
                            break;
                        }
                        if (1 <= number && number <= listHotels.Count)
                        {
                            break;
                        }

                    }
                    catch
                    {
                    }
                }

                if (number != 0)
                {
                    number--;
                    resultHotel = listHotels[number];
                }

            }

            return resultHotel;
        }
        public Room ChoiceRoom(User user, Hotel hotel)
        {
            Room resultRoom = new Room();  // вызов конструктора нужен, чтобы Id присвоилось 0

            Console.WriteLine("\n\t~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine($"\n\tПользователь: {user.Login}");
            Console.WriteLine($"\tОтель: {hotel.Name}");

            var dataService = new DataService();

            List<Room> listRooms = dataService.GetRooms(hotel);

            if (listRooms.Count == 0)
            {
                Console.WriteLine($"\tСписок номеров пуст.");
            }
            else
            {
                while (true)
                {
                    Console.WriteLine("\n\tСписок номеров:\n");
                    i = 0;
                    foreach (var elem in listRooms)
                    {
                        i++;
                        Console.WriteLine($"\t{i} - {elem.Name}");
                    };

                    Console.WriteLine($"\n\t0 - выход");
                    Console.Write($"\n\tВведите номер номера(1-{listRooms.Count}) = ");
                    check = Console.ReadLine();

                    try
                    {
                        number = int.Parse(check);
                        if (number == 0)  // выход с программы
                        {
                            break;
                        }
                        if (1 <= number && number <= listRooms.Count)
                        {
                            break;
                        }

                    }
                    catch
                    {
                    }
                }

                if (number != 0)
                {
                    number--;
                    resultRoom = listRooms[number];
                }

            }

            return resultRoom;
        }
        public Calendar ChoiceCalendar(User user, Hotel hotel, Room room)
        {
            Calendar resultCalendar = new Calendar();  // вызов конструктора нужен, чтобы Id присвоилось 0

            Console.WriteLine("\n\t~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine($"\n\tПользователь: {user.Login}");
            Console.WriteLine($"\tОтель: {hotel.Name}");
            Console.WriteLine($"\tНомер: {room.Name}");

            var dataService = new DataService();

            List<Calendar> listCalendar = dataService.GetCalendar(room);

            if (listCalendar.Count == 0)
            {
                Console.WriteLine($"\tСписок по датам пуст.");
            }
            else
            {
                while (true)
                {
                    Console.WriteLine("\n\tКалендарь дат (с ценами):\n");
                    i = 0;
                    foreach (var elem in listCalendar)
                    {
                        i++;
                        Console.WriteLine($"\t{i} - {elem.DateIn.ToShortDateString()} - {elem.DateOut.ToShortDateString()} - {elem.Price} тнг{((elem.Rezerv == 1) ? " - бронь" : "        ")}{((elem.Pay == 1) ? " - оплачено" : "           ")}");
                    };

                    Console.WriteLine($"\n\t0 - выход");
                    Console.Write($"\n\tВведите вариант даты (1-{listCalendar.Count}) = ");
                    check = Console.ReadLine();

                    try
                    {
                        number = int.Parse(check);
                        if (number == 0)  // выход с программы
                        {
                            break;
                        }
                        if (1 <= number && number <= listCalendar.Count)
                        {
                            break;
                        }

                    }
                    catch
                    {
                    }
                }

                if (number != 0)
                {
                    number--;
                    resultCalendar = listCalendar[number];
                }

            }

            return resultCalendar;
        }

        public void RoomActions(User user, Hotel hotel, Room room, Calendar calendar)
        {
            bool result;
            string menu, cardNumber;

            var dataService = new DataService();

            Console.WriteLine("\n\t~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine($"\n\tПользователь: {user.Login}");
            Console.WriteLine($"\tОтель: {hotel.Name}");
            Console.WriteLine($"\tНомер: {room.Name}");
            Console.WriteLine($"\tДаты заезда/выезда: {calendar.DateIn.ToShortDateString()} - {calendar.DateOut.ToShortDateString()}");
            Console.WriteLine($"\tСтоимость: {calendar.Price} тнг");

            if (calendar.Rezerv == 0 && calendar.Pay == 0)
            {
                while (true)
                {
                    Console.WriteLine("\n\t1 - Забронировать номер");
                    Console.WriteLine("\t0 - Выход");
                    Console.Write("\n\tВаше выбор = ");
                    menu = Console.ReadLine();

                    if (menu == "0")
                    {
                        break;
                    }
                    if (menu == "1")   // забронировать отель
                    {
                        result = dataService.RezervUpdate(1, user, calendar);
                        if (result)
                        {
                            Console.WriteLine("\n\tНомер забронирован.");
                            Console.WriteLine("\t*******************");
                        }
                        else
                        {
                            Console.WriteLine("\n\tНомер незабронирован.");
                            Console.WriteLine("\t*********************");
                        }
                        break;
                    }
                    continue;
                }
            }
            else if (calendar.Rezerv == 1 && calendar.Pay == 0 && calendar.UserId == user.Id)
            {
                while (true)
                {
                    Console.WriteLine("\n\t1 - Оплатить");
                    Console.WriteLine("\t2 - Снять бронь с номера");
                    Console.WriteLine("\t0 - Выход\n");
                    Console.Write("\tВаш выбор = ");
                    menu = Console.ReadLine();

                    if (menu == "0")
                    {
                        break;
                    }
                    if (menu == "1")   // оплатить
                    {
                        Console.WriteLine("\n\tНомер Вашей банковской карточки, с которой будет произведена");
                        Console.Write($"\tоплата в размере {calendar.Price} тенге (0 - отказ) = ");
                        cardNumber = Console.ReadLine();

                        if (cardNumber == "" || cardNumber == "0")
                        {
                            Console.WriteLine("\n\tНомер карты не введен, оплата не произведена.");
                            Console.WriteLine("\t*********************************************");
                        }
                        else
                        {
                            result = dataService.PayUpdate(user, calendar, cardNumber);
                            if (result)
                            {
                                Console.WriteLine("\n\tОплата произведена.");
                                Console.WriteLine("\t*******************");
                            }
                            else
                            {
                                Console.WriteLine("\n\tОплата не произведена.");
                                Console.WriteLine("\t**********************");
                            }
                        };

                        break;
                    }
                    if (menu == "2")   // снять бронь с номера
                    {
                        result = dataService.RezervUpdate(0, user, calendar);
                        if (result)
                        {
                            Console.WriteLine("\n\tНомер снят с брони.");
                            Console.WriteLine("\t*******************");
                        }
                        else
                        {
                            Console.WriteLine("\n\tНомер не снят с брони.");
                            Console.WriteLine("\t**********************");
                        }
                        break;
                    }
                    continue;
                }
            }
            else if (calendar.Rezerv == 1 && calendar.Pay == 0 && calendar.UserId != user.Id)
            {
                Console.WriteLine("\n\tНомер забронирован другим пользователем.");
                Console.WriteLine("\t----------------------------------------");


            }
            else if (calendar.Rezerv == 1 && calendar.Pay == 1)
            {
                Console.WriteLine("\n\tНомер уже забронирован и оплачен.");
                Console.WriteLine("\t---------------------------------");
            };
        }
    }
}
