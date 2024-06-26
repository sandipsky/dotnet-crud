using Microsoft.Data.Sqlite;

namespace DotnetCrud.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize(string connectionString)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var tableCommand = connection.CreateCommand();
            tableCommand.CommandText = @"
                CREATE TABLE IF NOT EXISTS Categories (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL
                );
                CREATE TABLE IF NOT EXISTS Brands (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL
                );
                CREATE TABLE IF NOT EXISTS Products (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Price REAL NOT NULL,
                    CategoryId INTEGER NOT NULL,
                    BrandId INTEGER NOT NULL,
                    Description TEXT,
                    Image TEXT,
                    IsFeatured BOOLEAN,
                    FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
                    FOREIGN KEY (BrandId) REFERENCES Brands(Id)
                );
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Username TEXT NOT NULL UNIQUE,
                    Email TEXT UNIQUE,
                    PasswordHash TEXT NOT NULL
                );
            ";

            tableCommand.ExecuteNonQuery();
        }
    }


}


