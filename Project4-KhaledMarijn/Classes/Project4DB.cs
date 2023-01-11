using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project4_KhaledMarijn.Classes
{
    internal class Project4DB
    {
    
        private string connString =
        ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;


        public bool GetPizzas(ICollection<Pizza> pizzas)
        {
            if (pizzas == null)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van GetPizzas");
            }

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
                            Size = (string)reader["size"],


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
    }




}
