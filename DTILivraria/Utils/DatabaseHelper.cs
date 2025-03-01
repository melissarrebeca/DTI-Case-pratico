using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace DTILivraria.Utils
{
    public static class DatabaseHelper
    {
        private static readonly string ConnectionString = "Data Source=Database/livraria.db;Version=3;";

        public static void InitializeDatabase()
        {
            string dbPath = "Database/livraria.db";
            
            string dbDirectory = "Database";
            if (!Directory.Exists(dbDirectory))
            {
                Directory.CreateDirectory(dbDirectory);
            }
            
            bool databaseExists = File.Exists(dbPath);
            
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                
                string createTableSql = @"
                    CREATE TABLE IF NOT EXISTS Livros (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Titulo TEXT NOT NULL,
                        Autor TEXT NOT NULL,
                        ISBN TEXT UNIQUE,
                        AnoPublicacao INTEGER,
                        Editora TEXT,
                        Preco REAL,
                        Quantidade INTEGER DEFAULT 0,
                        DataAquisicao TEXT,
                        Descricao TEXT,
                        Categoria TEXT
                    );
                    
                    CREATE INDEX IF NOT EXISTS idx_livros_titulo ON Livros(Titulo);
                    CREATE INDEX IF NOT EXISTS idx_livros_autor ON Livros(Autor);
                    CREATE INDEX IF NOT EXISTS idx_livros_isbn ON Livros(ISBN);
                    CREATE INDEX IF NOT EXISTS idx_livros_categoria ON Livros(Categoria);
                ";
                
                using (var command = new SQLiteCommand(createTableSql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        
        public static SQLiteConnection GetConnection()
        {
            var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            return connection;
        }
        
        public static void ExecuteNonQuery(string sql, SQLiteParameter[]? parameters = null)
        {
            using (var connection = GetConnection())
            {
                using (var command = new SQLiteCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    
                    command.ExecuteNonQuery();
                }
            }
        }
        
        public static DataTable ExecuteQuery(string sql, SQLiteParameter[]? parameters = null)
        {
            var dataTable = new DataTable();
            
            using (var connection = GetConnection())
            {
                using (var command = new SQLiteCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    
                    using (var adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            
            return dataTable;
        }
        
        public static object ExecuteScalar(string sql, SQLiteParameter[]? parameters = null)
        {
            using (var connection = GetConnection())
            {
                using (var command = new SQLiteCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    
                    return command.ExecuteScalar();
                }
            }
        }
        
        public static bool TableExists(string tableName)
        {
            string sql = "SELECT name FROM sqlite_master WHERE type='table' AND name=@TableName;";
            var parameters = new[] { new SQLiteParameter("@TableName", tableName) };
            
            var result = ExecuteScalar(sql, parameters);
            return result != null;
        }
    }
}