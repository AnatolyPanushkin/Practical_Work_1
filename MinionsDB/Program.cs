using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace MinionsDB
{
    class Program
    {
        static string connectionString = "Server=.;Database=Minions;Trusted_Connection=True";
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            GoodSelect("Boby");
            //BadInsert();
            //GoodInsert("Inva", 54, 1);
            //GoodSelect("Ivan");
        }
        /// <summary>
        /// Самый простой запрос, выборка всех без фильтров
        /// </summary>
        static void Select()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using(connection)
            {
                string selectionCommandString = "SELECT * FROM Minions";
                SqlCommand command = new SqlCommand(selectionCommandString, connection);
                SqlDataReader reader = command.ExecuteReader();
                using(reader)
                {
                    while(reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write($"{reader[i]} ");
                        }
                        Console.WriteLine();
                    }
                }
            }
        }

        /// <summary>
        /// Вставка нескольких записей. Так делать плохо, потенциальная SQL инъекция!
        /// </summary>
        static void BadInsert()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using(connection)
            {
                string creationCommandString = "INSERT INTO Minions VALUES" +
                    "('Kent',22,1), " +
                    "('Paul',34,2)";
                SqlCommand createCommand = new SqlCommand(creationCommandString, connection);
                Console.WriteLine(createCommand.ExecuteNonQuery());
            }
        }

        /// <summary>
        /// Вставка с параметрами, защищаемся от SQL инъекции!
        /// </summary>
        static void GoodInsert(string name, int age, int townId)
        {
            
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using(connection)
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Minions " +
                    "(Name, Age, TownId) VALUES " +
                    "(@name, @age, @townId)", connection);

                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@age", age);
                cmd.Parameters.AddWithValue("@townId", townId);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Выборка с параметрами, защищаемся от SQL инъекции!
        /// </summary>
        static void GoodSelect(string name)
        {
            string selectionCommandString = $"SELECT * FROM Minions " +
                $"WHERE Name = @name";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(selectionCommandString, connection);
            SqlParameter parameter = new SqlParameter("@name", SqlDbType.NVarChar, 50) { Value = name };
            command.Parameters.Add(parameter);
            // Или параметр можно связать так
            // command.Parameters.AddWithValue("@name", name);

            connection.Open();
            using (connection)
            {
                SqlDataReader reader = command.ExecuteReader();
                using(reader)
                {
                    while (reader.Read())
                    {
                        for(int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write($"{reader[i]} ");
                        }
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}