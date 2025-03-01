using System;
using System.Collections.Generic;
using System.Data.SQLite;
using DTILivraria.Models;
using DTILivraria.Repositories.Interfaces;
using DTILivraria.Utils;

namespace DTILivraria.Repositories
{
    public class LivroRepository : ILivroRepository
    {
        private readonly string _connectionString;

        public LivroRepository()
        {
            _connectionString = "Data Source=Database/livraria.db;Version=3;";
        }

        private Livro MapToLivro(SQLiteDataReader reader)
        {
            var livro = new Livro
            {
                Id = Convert.ToInt32(reader["Id"]),
                Titulo = reader["Titulo"].ToString(),
                Autor = reader["Autor"].ToString(),
                ISBN = reader["ISBN"] != DBNull.Value ? reader["ISBN"].ToString() : null,
                AnoPublicacao = reader["AnoPublicacao"] != DBNull.Value ? Convert.ToInt32(reader["AnoPublicacao"]) : null,
                Editora = reader["Editora"] != DBNull.Value ? reader["Editora"].ToString() : null,
                Preco = reader["Preco"] != DBNull.Value ? Convert.ToDecimal(reader["Preco"]) : null,
                Quantidade = reader["Quantidade"] != DBNull.Value ? Convert.ToInt32(reader["Quantidade"]) : 0,
                DataAquisicao = reader["DataAquisicao"] != DBNull.Value ? DateTime.Parse(reader["DataAquisicao"].ToString()) : null,
                Descricao = reader["Descricao"] != DBNull.Value ? reader["Descricao"].ToString() : null,
                Categoria = reader["Categoria"] != DBNull.Value ? reader["Categoria"].ToString() : null
            };
            return livro;
        }

        public List<Livro> GetAll()
        {
            LoggerService.Debug("Iniciando busca de todos os livros");
            var livros = new List<Livro>();

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    LoggerService.Debug("Conexão com banco de dados aberta para busca de todos os livros");
                    
                    string query = "SELECT * FROM Livros ORDER BY Titulo";
                    
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                livros.Add(MapToLivro(reader));
                            }
                        }
                    }
                }

                LoggerService.Information($"Busca de todos os livros concluída: {livros.Count} livros encontrados");
                return livros;
            }
            catch (Exception ex)
            {
                LoggerService.Error(ex, "Erro ao buscar todos os livros");
                throw;
            }
        }

        public Livro GetById(int id)
        {
            LoggerService.Debug($"Iniciando busca de livro por ID: {id}");
            
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    LoggerService.Debug($"Conexão com banco de dados aberta para busca de livro por ID: {id}");
                    
                    string query = "SELECT * FROM Livros WHERE Id = @Id";
                    
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var livro = MapToLivro(reader);
                                LoggerService.Information($"Livro encontrado com ID {id}: '{livro.Titulo}'");
                                return livro;
                            }
                        }
                    }
                }

                LoggerService.Warning($"Nenhum livro encontrado com ID: {id}");
                return null;
            }
            catch (Exception ex)
            {
                LoggerService.Error(ex, $"Erro ao buscar livro por ID: {id}");
                throw;
            }
        }

        public int Add(Livro livro)
        {
            LoggerService.Debug($"Iniciando adição de novo livro: '{livro.Titulo}'");
            
            if (string.IsNullOrEmpty(livro.Titulo) || string.IsNullOrEmpty(livro.Autor))
            {
                LoggerService.Warning("Tentativa de adicionar livro com título ou autor vazios");
                throw new ArgumentException("Título e Autor são campos obrigatórios.");
            }

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    LoggerService.Debug("Conexão com banco de dados aberta para adição de novo livro");
                    
                    string query = @"
                        INSERT INTO Livros (Titulo, Autor, ISBN, AnoPublicacao, Editora, 
                                           Preco, Quantidade, DataAquisicao, Descricao, Categoria)
                        VALUES (@Titulo, @Autor, @ISBN, @AnoPublicacao, @Editora,
                               @Preco, @Quantidade, @DataAquisicao, @Descricao, @Categoria);
                        SELECT last_insert_rowid();";
                    
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Titulo", livro.Titulo);
                        command.Parameters.AddWithValue("@Autor", livro.Autor);
                        command.Parameters.AddWithValue("@ISBN", livro.ISBN as object ?? DBNull.Value);
                        command.Parameters.AddWithValue("@AnoPublicacao", livro.AnoPublicacao as object ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Editora", livro.Editora as object ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Preco", livro.Preco as object ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Quantidade", livro.Quantidade);
                        
                        object dataParam = DBNull.Value;
                        if (livro.DataAquisicao.HasValue)
                        {
                            dataParam = livro.DataAquisicao.Value.ToString("yyyy-MM-dd");
                        }
                        command.Parameters.AddWithValue("@DataAquisicao", dataParam);
                        
                        command.Parameters.AddWithValue("@Descricao", livro.Descricao as object ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Categoria", livro.Categoria as object ?? DBNull.Value);
                        
                        var result = command.ExecuteScalar();
                        int newId = Convert.ToInt32(result);
                        
                        LoggerService.Information($"Livro adicionado com sucesso: ID {newId}, Título '{livro.Titulo}', Autor '{livro.Autor}'");
                        return newId;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerService.Error(ex, $"Erro ao adicionar livro: '{livro.Titulo}'");
                throw;
            }
        }

        public bool Update(Livro livro)
        {
            LoggerService.Debug($"Iniciando atualização do livro ID {livro.Id}: '{livro.Titulo}'");
            
            if (livro.Id <= 0)
            {
                LoggerService.Warning($"Tentativa de atualizar livro com ID inválido: {livro.Id}");
                throw new ArgumentException("ID do livro inválido.");
            }
            
            if (string.IsNullOrEmpty(livro.Titulo) || string.IsNullOrEmpty(livro.Autor))
            {
                LoggerService.Warning("Tentativa de atualizar livro com título ou autor vazios");
                throw new ArgumentException("Título e Autor são campos obrigatórios.");
            }

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    LoggerService.Debug($"Conexão com banco de dados aberta para atualização do livro ID {livro.Id}");
                    
                    string query = @"
                        UPDATE Livros 
                        SET Titulo = @Titulo, 
                            Autor = @Autor, 
                            ISBN = @ISBN, 
                            AnoPublicacao = @AnoPublicacao, 
                            Editora = @Editora,
                            Preco = @Preco, 
                            Quantidade = @Quantidade, 
                            DataAquisicao = @DataAquisicao, 
                            Descricao = @Descricao, 
                            Categoria = @Categoria
                        WHERE Id = @Id";
                    
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", livro.Id);
                        command.Parameters.AddWithValue("@Titulo", livro.Titulo);
                        command.Parameters.AddWithValue("@Autor", livro.Autor);
                        command.Parameters.AddWithValue("@ISBN", livro.ISBN as object ?? DBNull.Value);
                        command.Parameters.AddWithValue("@AnoPublicacao", livro.AnoPublicacao as object ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Editora", livro.Editora as object ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Preco", livro.Preco as object ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Quantidade", livro.Quantidade);
                        
                        object dataParam = DBNull.Value;
                        if (livro.DataAquisicao.HasValue)
                        {
                            dataParam = livro.DataAquisicao.Value.ToString("yyyy-MM-dd");
                        }
                        command.Parameters.AddWithValue("@DataAquisicao", dataParam);
                        
                        command.Parameters.AddWithValue("@Descricao", livro.Descricao as object ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Categoria", livro.Categoria as object ?? DBNull.Value);
                        
                        int rowsAffected = command.ExecuteNonQuery();
                        
                        if (rowsAffected > 0)
                        {
                            LoggerService.Information($"Livro atualizado com sucesso: ID {livro.Id}, Título '{livro.Titulo}'");
                            return true;
                        }
                        else
                        {
                            LoggerService.Warning($"Nenhum livro foi atualizado com o ID: {livro.Id}");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerService.Error(ex, $"Erro ao atualizar livro ID {livro.Id}: '{livro.Titulo}'");
                throw;
            }
        }

        public bool Delete(int id)
        {
            LoggerService.Debug($"Iniciando exclusão do livro ID: {id}");
            
            if (id <= 0)
            {
                LoggerService.Warning($"Tentativa de excluir livro com ID inválido: {id}");
                throw new ArgumentException("ID do livro inválido.");
            }

            try
            {
                // Primeiro, obter o livro para registrar informações no log
                var livro = GetById(id);
                
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    LoggerService.Debug($"Conexão com banco de dados aberta para exclusão do livro ID: {id}");
                    
                    string query = "DELETE FROM Livros WHERE Id = @Id";
                    
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        
                        int rowsAffected = command.ExecuteNonQuery();
                        
                        if (rowsAffected > 0)
                        {
                            LoggerService.Information($"Livro excluído com sucesso: ID {id}" + 
                                (livro != null ? $", Título '{livro.Titulo}'" : ""));
                            return true;
                        }
                        else
                        {
                            LoggerService.Warning($"Nenhum livro foi excluído com o ID: {id}");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerService.Error(ex, $"Erro ao excluir livro ID: {id}");
                throw;
            }
        }

        public Livro GetByISBN(string isbn)
        {
            LoggerService.Debug($"Iniciando busca de livro por ISBN: {isbn}");
            
            if (string.IsNullOrEmpty(isbn))
            {
                LoggerService.Warning("Tentativa de buscar livro com ISBN vazio");
                throw new ArgumentException("ISBN não pode ser vazio.");
            }

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    LoggerService.Debug($"Conexão com banco de dados aberta para busca de livro por ISBN: {isbn}");
                    
                    string query = "SELECT * FROM Livros WHERE ISBN = @ISBN";
                    
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ISBN", isbn);
                        
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var livro = MapToLivro(reader);
                                LoggerService.Information($"Livro encontrado com ISBN {isbn}: '{livro.Titulo}'");
                                return livro;
                            }
                        }
                    }
                }

                LoggerService.Warning($"Nenhum livro encontrado com ISBN: {isbn}");
                return null;
            }
            catch (Exception ex)
            {
                LoggerService.Error(ex, $"Erro ao buscar livro por ISBN: {isbn}");
                throw;
            }
        }

        public List<Livro> GetByAutor(string autor)
        {
            LoggerService.Debug($"Iniciando busca de livros por autor: '{autor}'");
            
            var livros = new List<Livro>();

            if (string.IsNullOrEmpty(autor))
            {
                LoggerService.Warning("Tentativa de buscar livros com autor vazio");
                return livros;
            }

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    LoggerService.Debug($"Conexão com banco de dados aberta para busca de livros por autor: '{autor}'");
                    
                    string query = "SELECT * FROM Livros WHERE Autor LIKE @Autor ORDER BY Titulo";
                    
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Autor", "%" + autor + "%");
                        
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                livros.Add(MapToLivro(reader));
                            }
                        }
                    }
                }

                LoggerService.Information($"Busca por autor '{autor}' concluída: {livros.Count} livros encontrados");
                return livros;
            }
            catch (Exception ex)
            {
                LoggerService.Error(ex, $"Erro ao buscar livros por autor: '{autor}'");
                throw;
            }
        }

        public List<Livro> Search(string termo)
        {
            LoggerService.Debug($"Iniciando pesquisa de livros com termo: '{termo ?? "vazio"}'");
            
            var livros = new List<Livro>();

            if (string.IsNullOrEmpty(termo))
            {
                LoggerService.Information("Termo de pesquisa vazio, retornando todos os livros");
                return GetAll();
            }

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    LoggerService.Debug($"Conexão com banco de dados aberta para pesquisa de livros com termo: '{termo}'");
                    
                    string query = @"
                        SELECT * FROM Livros 
                        WHERE Titulo LIKE @Termo 
                        OR Autor LIKE @Termo 
                        OR ISBN LIKE @Termo 
                        OR Categoria LIKE @Termo
                        ORDER BY Titulo";
                    
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Termo", "%" + termo + "%");
                        
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                livros.Add(MapToLivro(reader));
                            }
                        }
                    }
                }

                LoggerService.Information($"Pesquisa com termo '{termo}' concluída: {livros.Count} livros encontrados");
                return livros;
            }
            catch (Exception ex)
            {
                LoggerService.Error(ex, $"Erro ao pesquisar livros com termo: '{termo}'");
                throw;
            }
        }
    }
}