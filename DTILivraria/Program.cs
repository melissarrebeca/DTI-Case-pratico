﻿using System;
using System.IO;
using DTILivraria.Controllers;
using DTILivraria.Repositories;
using DTILivraria.Utils;
using DTILivraria.Views;

namespace DTILivraria
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Title = "Sistema de Gerenciamento de Livros";
                
                MostrarTelaDeBemVindo();
                
                LoggerService.InitializeLogger();
                LoggerService.Information("Aplicação iniciada");
                
                ConfigurarBancoDeDados();
                
                LoggerService.Debug("Configurando dependências...");
                var repository = new LivroRepository();
                var controller = new LivroController(repository);
                var view = new LivroView(controller);
                LoggerService.Debug("Dependências configuradas com sucesso");
                
                LoggerService.Information("Iniciando interface do usuário");
                view.ExibirMenuPrincipal();
                
                LoggerService.Information("Usuário encerrou a aplicação");
                MostrarTelaDeEncerramento();
            }
            catch (Exception ex)
            {
                LoggerService.Critical(ex, "Erro fatal na aplicação");
                TratarErroGlobal(ex);
            }
        }
        
        static void ConfigurarBancoDeDados()
        {
            Console.WriteLine("Inicializando banco de dados...");
            LoggerService.Information("Inicializando banco de dados");
            
            try
            {
                if (!Directory.Exists("Database"))
                {
                    Directory.CreateDirectory("Database");
                    LoggerService.Debug("Diretório Database criado");
                }
                
                DatabaseHelper.InitializeDatabase();
                
                bool tabelaExiste = DatabaseHelper.TableExists("Livros");
                
                if (tabelaExiste)
                {
                    Console.WriteLine("Banco de dados inicializado com sucesso!");
                    LoggerService.Information("Banco de dados inicializado com sucesso");
                }
                else
                {
                    LoggerService.Error("Falha ao criar a tabela de Livros no banco de dados");
                    throw new Exception("Falha ao criar a tabela de Livros no banco de dados.");
                }
            }
            catch (Exception ex)
            {
                LoggerService.Error(ex, "Erro ao configurar o banco de dados");
                throw new Exception($"Erro ao configurar o banco de dados: {ex.Message}", ex);
            }
            
            System.Threading.Thread.Sleep(1000);
        }
        
        static void MostrarTelaDeBemVindo()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
╔═══════════════════════════════════════════════════════════╗
║                                                           ║
║              SISTEMA DE GERENCIAMENTO DE LIVROS           ║
║                                                           ║
║                       DTI Livraria                        ║
║                                                           ║
║                       Versão 1.0                          ║
║                                                           ║
╚═══════════════════════════════════════════════════════════╝
");
            Console.ResetColor();
            
            Console.WriteLine("Inicializando o sistema...");
            System.Threading.Thread.Sleep(1500);
        }
        
        static void MostrarTelaDeEncerramento()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
╔═══════════════════════════════════════════════════════════╗
║                                                           ║
║                   Obrigado por utilizar                   ║
║                                                           ║
║              SISTEMA DE GERENCIAMENTO DE LIVROS           ║
║                                                           ║
║                                                           ║
╚═══════════════════════════════════════════════════════════╝
");
            Console.ResetColor();
            
            Console.WriteLine("Pressione qualquer tecla para encerrar...");
            Console.ReadKey();
        }
        
        static void TratarErroGlobal(Exception ex)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"
╔═══════════════════════════════════════════════════════════╗
║                                                           ║
║                    ERRO DO SISTEMA                        ║
║                                                           ║
╚═══════════════════════════════════════════════════════════╝
");
            Console.WriteLine("\nOcorreu um erro inesperado no sistema:");
            Console.WriteLine(ex.Message);
            
            if (ex.InnerException != null)
            {
                Console.WriteLine($"\nDetalhes adicionais: {ex.InnerException.Message}");
            }
            
            Console.ResetColor();
            Console.WriteLine("\n\nO programa será encerrado. Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}
