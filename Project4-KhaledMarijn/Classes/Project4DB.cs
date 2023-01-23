using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project4_KhaledMarijn.Classes
{
    public class Project4DB
    {

        private string connString =
        ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;


        public bool GetPizzas(ICollection<Pizza> pizzas)
        {
            if (pizzas == null)
                throw new ArgumentException("Ongeldig argument bij gebruik van GetPizzas");

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open(); MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"SELECT * FROM `pizzas`";
                    MySqlDataReader reader = sql.ExecuteReader();
                    while (reader.Read())
                    {
                        Pizza pizza = new Pizza()
                        {
                            PizzaID = (int)reader["pizzaID"],
                            Name = (string)reader["name"],
                            Price = (decimal)reader["price"],
                            PriceLabel = $" €{(decimal)reader["price"]}",
                        }; pizzas.Add(pizza);
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(GetPizzas)); Console.Error.WriteLine(e.Message);
                }
            }
            return true;
        }

        // Niet af
        public bool GetOrders(ICollection<Order> orders)
        {
            bool result;
            if (orders == null)
                throw new ArgumentException("Ongeldig argument bij gebruik van GetOrders");

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open(); MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"SELECT o.orderID, o.date, u.userID, o.status, u.firstname firstname, u.lastname lastname, u.address address, u.postalcode postalcode, u.city city FROM orders o INNER JOIN users u ON u.userID = o.userId";
                    MySqlDataReader reader = sql.ExecuteReader();
                    while (reader.Read())
                    {
                        Order order = new Order()
                        {
                            Id = (int)reader["orderID"],
                            Date = (DateTime)reader["date"],
                            UserId = (int)reader["userId"],
                            Status = (string)reader["status"],
                            User = new Customer()
                            {
                                FirstName = (string)reader["firstname"],
                                LastName = (string)reader["lastname"],
                                Address = (string)reader["address"],
                                PostalCode = (string)reader["postalcode"],
                                City = (string)reader["city"],
                            }
                        };
                        orders.Add(order);
                    }
                    result = true;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(GetOrders)); Console.Error.WriteLine(e.Message);
                    result = false;
                }
            }
            return result;
        }

        public bool GetSizes(ICollection<PizzaSize> sizes)
        {
            if (sizes == null)
                throw new ArgumentException("Ongeldig argument bij gebruik van GetSizes");

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open(); MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"SELECT * FROM `sizes`";
                    MySqlDataReader reader = sql.ExecuteReader();
                    while (reader.Read())
                    {
                        PizzaSize size = new PizzaSize()
                        {
                            SizeID = (int)reader["sizeID"],
                            Size = (string)reader["size"],
                        }; sizes.Add(size);
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(GetPizzas)); Console.Error.WriteLine(e.Message);
                }
            }
            return true;
        }

        // Medewerkers
        /*public bool CreatePizza(OrderPizza pizza)
        {
            bool result;
            if (pizza == null || pizza.Price <= 0 || string.IsNullOrEmpty(pizza.Name))
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van CreatePizza");
            }
            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"INSERT INTO pizzas (name, price) VALUES (@name, @price);";
                    sql.Parameters.AddWithValue("@name", pizza.Name);
                    sql.Parameters.AddWithValue("@price", pizza.Price);
                    if (sql.ExecuteNonQuery() == 1)
                        result = true;
                    else
                        result = false;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(CreateOrder));
                    Console.Error.WriteLine(e.Message);
                    result = false;
                }
            }
            return result;
        }*/

        public (bool, long) CreateUser(Customer user)
        {
            bool result;
            long id = 0;
            if (user == null)
                throw new ArgumentException("Ongeldig argument bij gebruik van CreateUser");
            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"INSERT INTO users (firstname, lastname, address, postalcode, city) VALUES (@firstname, @lastname, @address, @postalcode, @city);";
                    sql.Parameters.AddWithValue("@firstname", user.FirstName);
                    sql.Parameters.AddWithValue("@lastname", user.LastName);
                    sql.Parameters.AddWithValue("@address", user.Address);
                    sql.Parameters.AddWithValue("@postalcode", user.PostalCode);
                    sql.Parameters.AddWithValue("@city", user.City);
                    if (sql.ExecuteNonQuery() == 1)
                    {
                        id = sql.LastInsertedId;
                        result = true;
                    }
                    else
                        result = false;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(CreateUser));
                    Console.Error.WriteLine(e.Message);
                    result = false;
                }
            }
            return (result, id);
        }

        // Niet af
        public (bool, long) CreateOrder(Order order)
        {
            bool result;
            long id = 0;
            //if (order == null || order.UserId <= 0)
                //throw new ArgumentException("Ongeldig argument bij gebruik van CreateOrder");
            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"INSERT INTO orders (userId, date, status) VALUES (@userId, @date, @status);";
                    sql.Parameters.AddWithValue("@date", order.Date.ToUniversalTime());
                    sql.Parameters.AddWithValue("@userId", order.UserId);
                    sql.Parameters.AddWithValue("@status", order.Status);
                    if (sql.ExecuteNonQuery() == 1)
                    {
                        id = sql.LastInsertedId;
                        result = true;
                    }
                    else
                        result = false;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(CreateOrder));
                    Console.Error.WriteLine(e.Message);
                    result = false;
                }
            }
            return (result, id);
        }

        public bool CreateOrder_Pizza(OrderPizza pizza, int orderId)
        {
            bool result;
            if (pizza == null)
                throw new ArgumentException("Ongeldig argument bij gebruik van CreateOrder_Pizza");
            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"INSERT INTO order_pizza (orderId, pizzaId, sizeId, amount) VALUES (@orderId, @pizzaId, @sizeId, @amount);";
                    sql.Parameters.AddWithValue("@orderId", orderId);
                    sql.Parameters.AddWithValue("@pizzaId", pizza.PizzaID);
                    sql.Parameters.AddWithValue("@sizeId", pizza.SizeId);
                    sql.Parameters.AddWithValue("@amount", pizza.Amount);
                    if (sql.ExecuteNonQuery() == 1)
                        result = true;
                    else
                        result = false;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(CreateOrder_Pizza));
                    Console.Error.WriteLine(e.Message);
                    result = false;
                }
            }
            return result;
        }
    }
}
