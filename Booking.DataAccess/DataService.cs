using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.Common;
using Booking.Models;

namespace Booking.DataAccess
{
    public class DataService
    {

        private readonly string _connectionString;
        private readonly string _providerName;
        private readonly DbProviderFactory _providerFactory;


        public DataService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["testConnectionString"].ConnectionString;
            _providerName = ConfigurationManager.ConnectionStrings["testConnectionString"].ProviderName;
            _providerFactory = DbProviderFactories.GetFactory(_providerName);
        }

        public Dictionary<string, User> GetUsers()
        {
            var data = new Dictionary<string, User>();

            using (var connection = _providerFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();
                    command.CommandText = "select * from Users";
                    var dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        int id = (int)dataReader["Id"];
                        string login = dataReader["Login"].ToString();
                        string password = dataReader["Password"].ToString();
                        string phonenumber = dataReader["PhoneNumber"].ToString();
                        data.Add(login,
                            new User
                            {
                                Id = id,
                                Login = login,
                                Password = password,
                                PhoneNumber = phonenumber
                            });
                    }
                    dataReader.Close();
                }
                catch (DbException exception)
                {
                    //TODO обработка ошибки
                    throw;
                }
                catch (Exception exception)
                {
                    //TODO обработка ошибки
                    throw;
                }
            }
            return data;
        }

        public List<Hotel> GetHotels()
        {
            var data = new List<Hotel>();

            using (var connection = _providerFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();
                    command.CommandText = "select * from Hotels";
                    var dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        int id = (int)dataReader["Id"];
                        string name = dataReader["Name"].ToString();
                        data.Add(new Hotel
                        {
                            Id = id,
                            Name = name
                        });
                    }
                    dataReader.Close();
                }
                catch (DbException exception)
                {
                    //TODO обработка ошибки
                    throw;
                }
                catch (Exception exception)
                {
                    //TODO обработка ошибки
                    throw;
                }
            }
            return data;
        }

        public List<Room> GetRooms(Hotel hotel)
        {
            var data = new List<Room>();

            using (var connection = _providerFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();
                    command.CommandText = $"select * from Rooms where HotelId = {hotel.Id}";
                    var dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        int id = (int)dataReader["Id"];
                        string name = dataReader["Name"].ToString();
                        int hotelId = (int)dataReader["HotelId"];
                        data.Add(new Room
                        {
                            Id = id,
                            Name = name,
                            HotelId = hotelId
                        });
                    }
                    dataReader.Close();
                }
                catch (DbException exception)
                {
                    //TODO обработка ошибки
                    throw;
                }
                catch (Exception exception)
                {
                    //TODO обработка ошибки
                    throw;
                }
            }
            return data;
        }

        public List<Calendar> GetCalendar(Room room)
        {
            var data = new List<Calendar>();

            using (var connection = _providerFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();
                    command.CommandText = $"select * from Calendar where RoomId = {room.Id}";
                    var dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        int id = (int)dataReader["Id"];
                        DateTime dateIn = Convert.ToDateTime(dataReader["DateIn"]);
                        DateTime dateOut = Convert.ToDateTime(dataReader["DateOut"]);
                        int roomId = (int)dataReader["RoomId"];
                        int price = (int)dataReader["Price"];
                        int rezerv = (int)dataReader["Rezerv"];
                        int pay = (int)dataReader["Pay"];
                        int userId;
                        try
                        {
                            userId = (int)dataReader["UserId"];
                        }
                        catch
                        {
                            userId = 0;  // если поле UserId = null
                        }

                        string cardNumber = dataReader["CardNumber"].ToString();
                        data.Add(new Calendar
                        {
                            Id = id,
                            DateIn = dateIn,
                            DateOut = dateOut,
                            RoomId = roomId,
                            Price = price,
                            Rezerv = rezerv,
                            Pay = pay,
                            UserId = userId,
                            CardNumber = cardNumber
                        });
                    }
                    dataReader.Close();
                }
                catch (DbException exception)
                {
                    //TODO обработка ошибки
                    throw;
                }
                catch (Exception exception)
                {
                    //TODO обработка ошибки
                    throw;
                }
            }
            return data;
        }

        public bool RezervUpdate(int rezerv, User user, Calendar calendar)
        {
            using (var connection = _providerFactory.CreateConnection())
            using (var command = connection.CreateCommand())

            {
                DbTransaction transaction = null;

                try
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    command.Transaction = transaction;

                    DbParameter rezervParameter = command.CreateParameter();
                    rezervParameter.ParameterName = "@rezerv_param";
                    rezervParameter.Value = rezerv;
                    rezervParameter.DbType = System.Data.DbType.Int32;
                    rezervParameter.IsNullable = true;

                    DbParameter idParameter = command.CreateParameter();
                    idParameter.ParameterName = "@id_param";
                    idParameter.Value = calendar.Id;
                    idParameter.DbType = System.Data.DbType.Int32;
                    idParameter.IsNullable = true;

                    DbParameter userIdParameter = command.CreateParameter();
                    userIdParameter.ParameterName = "@userid_param";
                    if (rezerv == 0)  // снятие брони
                    {
                        userIdParameter.Value = 0;
                    }
                    else  // бронирование
                    {
                        userIdParameter.Value = user.Id;
                    }
                    userIdParameter.DbType = System.Data.DbType.Int32;
                    userIdParameter.IsNullable = true;

                    command.Parameters.AddRange(new DbParameter[] { rezervParameter, idParameter, userIdParameter });
                    command.CommandText = $"update Calendar set Rezerv = @rezerv_param, UserId = @userid_param where Id = @id_param";
                    var affectRows = command.ExecuteNonQuery();
                    if (affectRows < 1)
                    {
                        throw new Exception("Обновление не было произведено");
                    }

                    command.CommandText = $"update Calendar set Pay = 0, CardNumber = '' where Id = @id_param";
                    affectRows = command.ExecuteNonQuery();
                    if (affectRows < 1)
                    {
                        throw new Exception("Обновление не было произведено");
                    }

                    transaction.Commit();
                    transaction.Dispose();
                    return true;  // true - без ошибок
                }
                catch (DbException exception)
                {
                    transaction?.Rollback();
                    transaction.Dispose();
                    //throw;
                    return false;
                }
                catch (Exception exception)
                {
                    transaction?.Rollback();
                    transaction.Dispose();
                    //throw;
                    return false;
                }
            }
        }

        public bool PayUpdate(User user, Calendar calendar, string cardNumber)
        {
            using (var connection = _providerFactory.CreateConnection())
            using (var command = connection.CreateCommand())

            {
                DbTransaction transaction = null;

                try
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    command.Transaction = transaction;

                    DbParameter rezervParameter = command.CreateParameter();
                    rezervParameter.ParameterName = "@cardnumber_param";
                    rezervParameter.Value = cardNumber;
                    rezervParameter.DbType = System.Data.DbType.String;
                    rezervParameter.IsNullable = true;

                    DbParameter idParameter = command.CreateParameter();
                    idParameter.ParameterName = "@id_param";
                    idParameter.Value = calendar.Id;
                    idParameter.DbType = System.Data.DbType.Int32;
                    idParameter.IsNullable = true;

                    DbParameter userIdParameter = command.CreateParameter();
                    userIdParameter.ParameterName = "@userid_param";
                    userIdParameter.Value = user.Id;
                    userIdParameter.DbType = System.Data.DbType.Int32;
                    userIdParameter.IsNullable = true;

                    command.Parameters.AddRange(new DbParameter[] { rezervParameter, idParameter, userIdParameter });
                    command.CommandText = $"update Calendar set Pay = 1, UserId = @userid_param where Id = @id_param";
                    var affectRows = command.ExecuteNonQuery();
                    if (affectRows < 1)
                    {
                        throw new Exception("Обновление не было произведено");
                    }

                    command.CommandText = $"update Calendar set CardNumber = @cardNumber_param where Id = @id_param";
                    affectRows = command.ExecuteNonQuery();
                    if (affectRows < 1)
                    {
                        throw new Exception("Обновление не было произведено");
                    }

                    transaction.Commit();
                    transaction.Dispose();
                    return true;  // true - без ошибок
                }
                catch (DbException exception)
                {
                    transaction?.Rollback();
                    transaction.Dispose();
                    //throw;
                    return false;
                }
                catch (Exception exception)
                {
                    transaction?.Rollback();
                    transaction.Dispose();
                    //throw;
                    return false;
                }
            }
        }

        public bool AddUser(string login, string password, string phoneNumber)
        {
            using (var connection = _providerFactory.CreateConnection())
            using (var command = connection.CreateCommand())

            {
                DbTransaction transaction = null;

                try
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    command.Transaction = transaction;

                    DbParameter loginParameter = command.CreateParameter();
                    loginParameter.ParameterName = "@login_param";
                    loginParameter.Value = login;
                    loginParameter.DbType = System.Data.DbType.String;
                    loginParameter.IsNullable = true;

                    DbParameter passwordParameter = command.CreateParameter();
                    passwordParameter.ParameterName = "@password_param";
                    passwordParameter.Value = password;
                    passwordParameter.DbType = System.Data.DbType.String;
                    passwordParameter.IsNullable = true;

                    DbParameter phonenumberParameter = command.CreateParameter();
                    phonenumberParameter.ParameterName = "@phonenumber_param";
                    phonenumberParameter.Value = phoneNumber;
                    phonenumberParameter.DbType = System.Data.DbType.String;
                    phonenumberParameter.IsNullable = true;

                    command.Parameters.AddRange(new DbParameter[] { loginParameter, passwordParameter, phonenumberParameter });
                    command.CommandText = $"insert into users(Login, Password, PhoneNumber) values( @login_param, @password_param, @phonenumber_param)";

                    var affectRows = command.ExecuteNonQuery();
                    if (affectRows < 1)
                    {
                        throw new Exception("Обновление не было произведено");
                    }


                    transaction.Commit();
                    transaction.Dispose();
                    return true;  // true - без ошибок
                }
                catch (DbException exception)
                {
                    transaction?.Rollback();
                    transaction.Dispose();
                    //throw;
                    return false;
                }
                catch (Exception exception)
                {
                    transaction?.Rollback();
                    transaction.Dispose();
                    //throw;
                    return false;
                }
            }
        }


    }
}
