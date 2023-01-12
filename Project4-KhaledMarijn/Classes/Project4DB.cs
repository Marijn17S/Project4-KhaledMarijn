using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    sql.CommandText = @"SELECT * FROM `pizzas` WHERE 1";
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
                    sql.CommandText = @"SELECT i.ingredientId, i.name, i.price, i.UnitId, u.name as unitName FROM ingredients i INNER JOIN units u ON u.UnitId = i.UnitId ORDER BY i.name";
                    MySqlDataReader reader = sql.ExecuteReader();
                    while (reader.Read())
                    {
                        Order order = new Order()
                        {
                            Id = (int)reader["orderID"],
                            Date = (DateTime)reader["IngredientId"],
                            User = new Customer()
                            {
                                //FirstName = (int)reader["NOGINVULLEN"],
                                //LastName = (string)reader["NOGINVULLEN"],
                                // Zelfde voor address, city, postcode
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

        // Niet af
        public bool CreateOrder(Order order)
        {
            bool result;
            if (order == null || order.Id <= 0 || order.User == null)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van CreateOrder");
            }
            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"INSERT INTO orders (date, userId) VALUES  (@date, @userId);";
                    sql.Parameters.AddWithValue("@date", order.Date);
                    sql.Parameters.AddWithValue("@userId", order.User.Id);
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
        }
    }
}
