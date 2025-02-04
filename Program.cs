// https://learn.microsoft.com/ru-ru/sql/connect/ado-net/introduction-microsoft-data-sqlclient-namespace?view=sql-server-ver16
using Microsoft.Data.SqlClient;  // новый пакет для подключения к SQL Server в .NET 9

// скрипт БД: https://gist.github.com/sunmeat/59dc33337af869024a7b18602b556b00

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8; // в БД есть украинские символы

        // строка подключения к базе данных
        string connectionString = "Server=localhost; Database=Store; Integrated Security=True; TrustServerCertificate=True;";

        // SQL-запрос для выборки данных из таблицы Product
        string query = "SELECT id, name, price FROM Product";

        // создание подключения
        using (var connection = new SqlConnection(connectionString))
        {
            try
            {
                // открытие соединения с базой данных
                connection.Open();

                // создание команды для выполнения запроса
                using (var command = new SqlCommand(query, connection))
                {
                    // выполнение команды и чтение данных с помощью SqlDataReader
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // проверка, есть ли данные
                        if (reader.HasRows)
                        {
                            // чтение строк данных и вывод на экран
                            while (reader.Read())
                            {
                                // чтение значений из текущей строки
                                int id = reader.GetInt32(0);  // чтение id
                                string name = reader.GetString(1);  // чтение name
                                double price = reader.GetDouble(2);  // чтение price

                                Console.WriteLine($"ID: {id}, Name: {name}, Price: {price}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Нема данных в таблице.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }

        Console.ReadLine();
    }
}

/*
В данном примере соединение с базой данных работает в присоединённом режиме (или "connected" mode):
- В коде строка подключения Integrated Security=True указывает на использование Windows Authentication,
что предполагает создание постоянного соединения с сервером для выполнения запросов. Это означает, что запросы
выполняются на сервере в присоединённом режиме, где запросы выполняются непосредственно к базе данных,
а не через локальные кэшированные данные или "отключённый" режим.
- Используются методы, которые выполняют запросы непосредственно к базе данных, например ExecuteReader().
Это типичное поведение для присоединённого режима, когда все данные получаются "на лету" и необходимо поддерживать
постоянное соединение с сервером.
- В коде используется конструкция using, которая автоматически управляет подключением. Это также является
характерным для "присоединённого" режима, потому что каждое подключение открывается и закрывается в рамках
одного запроса, а не сохраняется на протяжении всей сессии.

Присоединённый режим:
- Открывает соединение с базой данных.
- Запросы выполняются в реальном времени.
- Ответы приходят с сервера сразу.
- Требуется постоянное подключение к серверу.

Отключённый режим:
- Сначала извлекаются данные в память, а потом с ними можно работать локально.
- Не требуется постоянное подключение.
- Используется, например, DataSet или DataTable.
*/