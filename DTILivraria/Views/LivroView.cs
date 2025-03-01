using System;
using System.Collections.Generic;
using System.Linq;
using DTILivraria.Controllers;
using DTILivraria.Models;
using DTILivraria.Utils;

namespace DTILivraria.Views
{
    public class LivroView
    {
        private readonly LivroController _controller;

        public LivroView(LivroController controller)
        {
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        public void ExibirMenuPrincipal()
        {
            bool sair = false;

            while (!sair)
            {
                Console.Clear();
                Console.WriteLine("=================================================");
                Console.WriteLine("          SISTEMA DE GERENCIAMENTO DE LIVROS     ");
                Console.WriteLine("=================================================");
                Console.WriteLine();
                Console.WriteLine("1. Listar todos os livros");
                Console.WriteLine("2. Buscar livro por ID");
                Console.WriteLine("3. Buscar livro por ISBN");
                Console.WriteLine("4. Buscar livros por autor");
                Console.WriteLine("5. Pesquisar livros");
                Console.WriteLine("6. Cadastrar novo livro");
                Console.WriteLine("7. Atualizar livro");
                Console.WriteLine("8. Excluir livro");
                Console.WriteLine("0. Sair");
                Console.WriteLine();
                Console.Write("Escolha uma opção: ");

                if (int.TryParse(Console.ReadLine(), out int opcao))
                {
                    Console.Clear();
                    
                    switch (opcao)
                    {
                        case 1:
                            ListarTodosLivros();
                            break;
                        case 2:
                            BuscarLivroPorId();
                            break;
                        case 3:
                            BuscarLivroPorISBN();
                            break;
                        case 4:
                            BuscarLivrosPorAutor();
                            break;
                        case 5:
                            PesquisarLivros();
                            break;
                        case 6:
                            CadastrarNovoLivro();
                            break;
                        case 7:
                            AtualizarLivro();
                            break;
                        case 8:
                            ExcluirLivro();
                            break;
                        case 0:
                            sair = true;
                            Console.WriteLine("Encerrando o programa...");
                            break;
                        default:
                            ExibirMensagemErro("Opção inválida. Por favor, escolha uma opção entre 0 e 8.");
                            break;
                    }

                    if (!sair)
                    {
                        Console.WriteLine("\nPressione qualquer tecla para continuar...");
                        Console.ReadKey();
                    }
                }
                else
                {
                    ExibirMensagemErro("Entrada inválida. Por favor, digite um número.");
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        private void ListarTodosLivros()
        {
            bool voltar = false;
            
            while (!voltar)
            {
                Console.Clear();
                Console.WriteLine("=== LISTAGEM DE TODOS OS LIVROS ===\n");
                
                var resultado = _controller.ListarTodosLivros();
                
                if (resultado.Success)
                {
                    if (resultado.Data != null && resultado.Data.Any())
                    {
                        ExibirListaDeLivros(resultado.Data);
                    }
                    else
                    {
                        Console.WriteLine("Nenhum livro cadastrado.");
                    }
                    
                    ExibirMensagemSucesso(resultado.Message);
                }
                else
                {
                    ExibirMensagemErro(resultado.Message, resultado.Errors);
                }
                
                Console.WriteLine("\nOpções:");
                Console.WriteLine("1. Voltar ao menu principal");
                Console.WriteLine("0. Sair do sistema");
                Console.Write("\nEscolha uma opção: ");
                
                if (int.TryParse(Console.ReadLine(), out int opcao))
                {
                    switch (opcao)
                    {
                        case 1:
                            voltar = true;
                            break;
                        case 0:
                            Environment.Exit(0);
                            break;
                        default:
                            ExibirMensagemErro("Opção inválida. Por favor, escolha 1 para voltar ou 0 para sair.");
                            Console.WriteLine("Pressione qualquer tecla para continuar...");
                            Console.ReadKey();
                            break;
                    }
                }
                else
                {
                    ExibirMensagemErro("Entrada inválida. Por favor, digite um número.");
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        private void BuscarLivroPorId()
        {
            bool voltar = false;
            
            while (!voltar)
            {
                Console.Clear();
                Console.WriteLine("=== BUSCAR LIVRO POR ID ===\n");
                
                Console.WriteLine("Opções:");
                Console.WriteLine("Digite o ID do livro para buscar");
                Console.WriteLine("V. Voltar ao menu principal");
                Console.WriteLine("S. Sair do sistema");
                Console.Write("\nSua escolha: ");
                
                string entrada = Console.ReadLine()?.Trim();
                
                if (string.IsNullOrEmpty(entrada))
                {
                    ExibirMensagemErro("Entrada inválida. Por favor, digite um valor.");
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    continue;
                }
                
                if (entrada.Equals("v", StringComparison.OrdinalIgnoreCase))
                {
                    voltar = true;
                    continue;
                }
                
                if (entrada.Equals("s", StringComparison.OrdinalIgnoreCase))
                {
                    Environment.Exit(0);
                }
                
                if (int.TryParse(entrada, out int id))
                {
                    var resultado = _controller.BuscarLivroPorId(id);
                    
                    if (resultado.Success)
                    {
                        Console.WriteLine("\nLivro encontrado:\n");
                        ExibirDetalhesLivro(resultado.Data);
                        ExibirMensagemSucesso(resultado.Message);
                    }
                    else
                    {
                        ExibirMensagemErro(resultado.Message, resultado.Errors);
                    }
                    
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
                else
                {
                    ExibirMensagemErro("ID inválido. Por favor, digite um número inteiro.");
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        private void BuscarLivroPorISBN()
        {
            bool voltar = false;
            
            while (!voltar)
            {
                Console.Clear();
                Console.WriteLine("=== BUSCAR LIVRO POR ISBN ===\n");
                
                Console.WriteLine("Opções:");
                Console.WriteLine("Digite o ISBN do livro para buscar");
                Console.WriteLine("V. Voltar ao menu principal");
                Console.WriteLine("S. Sair do sistema");
                Console.Write("\nSua escolha: ");
                
                string entrada = Console.ReadLine()?.Trim();
                
                if (string.IsNullOrEmpty(entrada))
                {
                    ExibirMensagemErro("Entrada inválida. Por favor, digite um valor.");
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    continue;
                }
                
                if (entrada.Equals("v", StringComparison.OrdinalIgnoreCase))
                {
                    voltar = true;
                    continue;
                }
                
                if (entrada.Equals("s", StringComparison.OrdinalIgnoreCase))
                {
                    Environment.Exit(0);
                }
                
                // Considerando a entrada como ISBN
                string isbn = entrada;
                var resultado = _controller.BuscarLivroPorISBN(isbn);
                
                if (resultado.Success)
                {
                    Console.WriteLine("\nLivro encontrado:\n");
                    ExibirDetalhesLivro(resultado.Data);
                    ExibirMensagemSucesso(resultado.Message);
                }
                else
                {
                    ExibirMensagemErro(resultado.Message, resultado.Errors);
                }
                
                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }

        private void BuscarLivrosPorAutor()
        {
            bool voltar = false;
            
            while (!voltar)
            {
                Console.Clear();
                Console.WriteLine("=== BUSCAR LIVROS POR AUTOR ===\n");
                
                Console.WriteLine("Opções:");
                Console.WriteLine("Digite o nome do autor para buscar");
                Console.WriteLine("V. Voltar ao menu principal");
                Console.WriteLine("S. Sair do sistema");
                Console.Write("\nSua escolha: ");
                
                string entrada = Console.ReadLine()?.Trim();
                
                if (string.IsNullOrEmpty(entrada))
                {
                    ExibirMensagemErro("Entrada inválida. Por favor, digite um valor.");
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    continue;
                }
                
                if (entrada.Equals("v", StringComparison.OrdinalIgnoreCase))
                {
                    voltar = true;
                    continue;
                }
                
                if (entrada.Equals("s", StringComparison.OrdinalIgnoreCase))
                {
                    Environment.Exit(0);
                }
                
                // Considerando a entrada como nome do autor
                string autor = entrada;
                var resultado = _controller.BuscarLivrosPorAutor(autor);
                
                if (resultado.Success)
                {
                    if (resultado.Data != null && resultado.Data.Any())
                    {
                        Console.WriteLine($"\nLivros do autor '{autor}':\n");
                        ExibirListaDeLivros(resultado.Data);
                    }
                    else
                    {
                        Console.WriteLine($"Nenhum livro encontrado para o autor '{autor}'.");
                    }
                    
                    ExibirMensagemSucesso(resultado.Message);
                }
                else
                {
                    ExibirMensagemErro(resultado.Message, resultado.Errors);
                }
                
                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }

        private void PesquisarLivros()
        {
            bool voltar = false;
            
            while (!voltar)
            {
                Console.Clear();
                Console.WriteLine("=== PESQUISAR LIVROS ===\n");
                
                Console.WriteLine("Opções:");
                Console.WriteLine("Digite o termo de pesquisa (ou enter para listar todos)");
                Console.WriteLine("V. Voltar ao menu principal");
                Console.WriteLine("S. Sair do sistema");
                Console.Write("\nSua escolha: ");
                
                string entrada = Console.ReadLine()?.Trim();
                
                if (entrada == null)
                {
                    ExibirMensagemErro("Entrada inválida.");
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    continue;
                }
                
                if (entrada.Equals("v", StringComparison.OrdinalIgnoreCase))
                {
                    voltar = true;
                    continue;
                }
                
                if (entrada.Equals("s", StringComparison.OrdinalIgnoreCase))
                {
                    Environment.Exit(0);
                }
                
                // Considerando a entrada como termo de pesquisa
                string termo = entrada;
                var resultado = _controller.PesquisarLivros(termo);
                
                if (resultado.Success)
                {
                    if (resultado.Data != null && resultado.Data.Any())
                    {
                        Console.WriteLine($"\nResultados da pesquisa por '{termo}':\n");
                        ExibirListaDeLivros(resultado.Data);
                    }
                    else
                    {
                        Console.WriteLine($"Nenhum livro encontrado para a pesquisa '{termo}'.");
                    }
                    
                    ExibirMensagemSucesso(resultado.Message);
                }
                else
                {
                    ExibirMensagemErro(resultado.Message, resultado.Errors);
                }
                
                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }

        private void CadastrarNovoLivro()
        {
            bool voltar = false;
            
            while (!voltar)
            {
                Console.Clear();
                Console.WriteLine("=== CADASTRAR NOVO LIVRO ===\n");
                
                Console.WriteLine("Deseja prosseguir com o cadastro de um novo livro?");
                Console.WriteLine("1. Sim, prosseguir com o cadastro");
                Console.WriteLine("2. Voltar ao menu principal");
                Console.WriteLine("0. Sair do sistema");
                Console.Write("\nEscolha uma opção: ");
                
                if (int.TryParse(Console.ReadLine(), out int opcao))
                {
                    switch (opcao)
                    {
                        case 1:
                            RealizarCadastroLivro();
                            break;
                        case 2:
                            voltar = true;
                            break;
                        case 0:
                            Environment.Exit(0);
                            break;
                        default:
                            ExibirMensagemErro("Opção inválida.");
                            Console.WriteLine("Pressione qualquer tecla para continuar...");
                            Console.ReadKey();
                            break;
                    }
                }
                else
                {
                    ExibirMensagemErro("Entrada inválida. Por favor, digite um número.");
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        private void RealizarCadastroLivro()
        {
            Console.Clear();
            Console.WriteLine("=== CADASTRAR NOVO LIVRO ===\n");
            
            var livro = new Livro();
            
            Console.WriteLine("Preencha os dados do livro (campos com * são obrigatórios):");
            Console.WriteLine("(a qualquer momento, digite 'cancelar' para voltar ao menu anterior)\n");
            
            Console.Write("Título*: ");
            string titulo = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(titulo))
            {
                ExibirMensagemErro("Título não pode ser vazio.");
                return;
            }
            if (titulo.Equals("cancelar", StringComparison.OrdinalIgnoreCase))
                return;
            livro.Titulo = titulo;
            
            Console.Write("Autor*: ");
            string autor = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(autor))
            {
                ExibirMensagemErro("Autor não pode ser vazio.");
                return;
            }
            if (autor.Equals("cancelar", StringComparison.OrdinalIgnoreCase))
                return;
            livro.Autor = autor;
            
            Console.Write("ISBN*: ");
            string isbn = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(isbn))
            {
                ExibirMensagemErro("ISBN não pode ser vazio.");
                return;
            }
            if (isbn.Equals("cancelar", StringComparison.OrdinalIgnoreCase))
                return;
            livro.ISBN = isbn;
            
            Console.Write("Ano de Publicação*: ");
            string anoStr = Console.ReadLine()?.Trim();
            if (anoStr?.Equals("cancelar", StringComparison.OrdinalIgnoreCase) == true)
                return;
            if (string.IsNullOrEmpty(anoStr) || !int.TryParse(anoStr, out int ano))
            {
                ExibirMensagemErro("Ano de publicação inválido.");
                return;
            }
            livro.AnoPublicacao = ano;
            
            Console.Write("Editora (ou deixe em branco): ");
            string editora = Console.ReadLine()?.Trim();
            if (editora?.Equals("cancelar", StringComparison.OrdinalIgnoreCase) == true)
                return;
            livro.Editora = string.IsNullOrEmpty(editora) ? null : editora;
            
            Console.Write("Preço (ou deixe em branco): ");
            string precoStr = Console.ReadLine()?.Trim();
            if (precoStr?.Equals("cancelar", StringComparison.OrdinalIgnoreCase) == true)
                return;
            if (!string.IsNullOrEmpty(precoStr) && decimal.TryParse(precoStr, out decimal preco))
            {
                livro.Preco = preco;
            }
            
            Console.Write("Quantidade (ou deixe em branco para 0): ");
            string qtdStr = Console.ReadLine()?.Trim();
            if (qtdStr?.Equals("cancelar", StringComparison.OrdinalIgnoreCase) == true)
                return;
            if (!string.IsNullOrEmpty(qtdStr) && int.TryParse(qtdStr, out int quantidade))
            {
                livro.Quantidade = quantidade;
            }
            
            Console.Write("Data de Aquisição (dd/mm/aaaa ou deixe em branco): ");
            string dataStr = Console.ReadLine()?.Trim();
            if (dataStr?.Equals("cancelar", StringComparison.OrdinalIgnoreCase) == true)
                return;
            if (!string.IsNullOrEmpty(dataStr) && DateTime.TryParse(dataStr, out DateTime data))
            {
                livro.DataAquisicao = data;
            }
            
            Console.Write("Categoria (ou deixe em branco): ");
            string categoria = Console.ReadLine()?.Trim();
            if (categoria?.Equals("cancelar", StringComparison.OrdinalIgnoreCase) == true)
                return;
            livro.Categoria = string.IsNullOrEmpty(categoria) ? null : categoria;
            
            Console.Write("Descrição (ou deixe em branco): ");
            string descricao = Console.ReadLine()?.Trim();
            if (descricao?.Equals("cancelar", StringComparison.OrdinalIgnoreCase) == true)
                return;
            livro.Descricao = string.IsNullOrEmpty(descricao) ? null : descricao;
            
            Console.WriteLine("\nConfirmar cadastro do livro com os dados acima? (S/N)");
            string confirmacao = Console.ReadLine()?.Trim().ToUpper();
            if (confirmacao != "S")
            {
                Console.WriteLine("Cadastro cancelado pelo usuário.");
                return;
            }
            
            var resultado = _controller.CadastrarLivro(livro);
            
            if (resultado.Success)
            {
                ExibirMensagemSucesso($"Livro cadastrado com sucesso! ID: {resultado.Data}");
            }
            else
            {
                ExibirMensagemErro(resultado.Message, resultado.Errors);
            }
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        private void AtualizarLivro()
        {
            bool voltar = false;
            
            while (!voltar)
            {
                Console.Clear();
                Console.WriteLine("=== ATUALIZAR LIVRO ===\n");
                
                Console.WriteLine("Opções:");
                Console.WriteLine("Digite o ID do livro para atualizar");
                Console.WriteLine("V. Voltar ao menu principal");
                Console.WriteLine("S. Sair do sistema");
                Console.Write("\nSua escolha: ");
                
                string entrada = Console.ReadLine()?.Trim();
                
                if (string.IsNullOrEmpty(entrada))
                {
                    ExibirMensagemErro("Entrada inválida. Por favor, digite um valor.");
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    continue;
                }
                
                if (entrada.Equals("v", StringComparison.OrdinalIgnoreCase))
                {
                    voltar = true;
                    continue;
                }
                
                if (entrada.Equals("s", StringComparison.OrdinalIgnoreCase))
                {
                    Environment.Exit(0);
                }
                
                if (int.TryParse(entrada, out int id))
                {
                    var resultadoBusca = _controller.BuscarLivroPorId(id);
                    
                    if (resultadoBusca.Success)
                    {
                        RealizarAtualizacaoLivro(resultadoBusca.Data);
                    }
                    else
                    {
                        ExibirMensagemErro(resultadoBusca.Message, resultadoBusca.Errors);
                        Console.WriteLine("\nPressione qualquer tecla para continuar...");
                        Console.ReadKey();
                    }
                }
                else
                {
                    ExibirMensagemErro("ID inválido. Por favor, digite um número inteiro.");
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        private void RealizarAtualizacaoLivro(Livro livro)
        {
            Console.Clear();
            Console.WriteLine("=== ATUALIZAR LIVRO ===\n");
            
            Console.WriteLine("Dados atuais do livro:\n");
            ExibirDetalhesLivro(livro);
            
            Console.WriteLine("\nPreencha os novos dados (deixe em branco para manter o valor atual):");
            Console.WriteLine("(a qualquer momento, digite 'cancelar' para voltar ao menu anterior)\n");
            
            Console.Write($"Título* [{livro.Titulo}]: ");
            string titulo = Console.ReadLine()?.Trim();
            if (titulo?.Equals("cancelar", StringComparison.OrdinalIgnoreCase) == true)
                return;
            if (!string.IsNullOrEmpty(titulo))
            {
                livro.Titulo = titulo;
            }
            
            Console.Write($"Autor* [{livro.Autor}]: ");
            string autor = Console.ReadLine()?.Trim();
            if (autor?.Equals("cancelar", StringComparison.OrdinalIgnoreCase) == true)
                return;
            if (!string.IsNullOrEmpty(autor))
            {
                livro.Autor = autor;
            }
            
            Console.Write($"ISBN* [{livro.ISBN}]: ");
            string isbn = Console.ReadLine()?.Trim();
            if (isbn?.Equals("cancelar", StringComparison.OrdinalIgnoreCase) == true)
                return;
            if (!string.IsNullOrEmpty(isbn))
            {
                livro.ISBN = isbn;
            }
            
            Console.Write($"Ano de Publicação* [{livro.AnoPublicacao}]: ");
            string anoStr = Console.ReadLine()?.Trim();
            if (anoStr?.Equals("cancelar", StringComparison.OrdinalIgnoreCase) == true)
                return;
            if (!string.IsNullOrEmpty(anoStr) && int.TryParse(anoStr, out int ano))
            {
                livro.AnoPublicacao = ano;
            }
            
            Console.Write($"Editora [{livro.Editora ?? "Não informada"}]: ");
            string editora = Console.ReadLine()?.Trim();
            if (editora?.Equals("cancelar", StringComparison.OrdinalIgnoreCase) == true)
                return;
            if (!string.IsNullOrEmpty(editora))
            {
                if (editora.ToLower() == "limpar")
                {
                    livro.Editora = null;
                }
                else
                {
                    livro.Editora = editora;
                }
            }
            
            Console.Write($"Preço [{livro.Preco?.ToString("C") ?? "Não informado"}]: ");
            string precoStr = Console.ReadLine()?.Trim();
            if (precoStr?.Equals("cancelar", StringComparison.OrdinalIgnoreCase) == true)
                return;
            if (!string.IsNullOrEmpty(precoStr))
            {
                if (precoStr.ToLower() == "limpar")
                {
                    livro.Preco = null;
                }
                else if (decimal.TryParse(precoStr, out decimal preco))
                {
                    livro.Preco = preco;
                }
            }
            
            Console.Write($"Quantidade [{livro.Quantidade}]: ");
            string qtdStr = Console.ReadLine()?.Trim();
            if (qtdStr?.Equals("cancelar", StringComparison.OrdinalIgnoreCase) == true)
                return;
            if (!string.IsNullOrEmpty(qtdStr) && int.TryParse(qtdStr, out int quantidade))
            {
                livro.Quantidade = quantidade;
            }
            
            Console.Write($"Data de Aquisição [{livro.DataAquisicao?.ToString("dd/MM/yyyy") ?? "Não informada"}]: ");
            string dataStr = Console.ReadLine()?.Trim();
            if (dataStr?.Equals("cancelar", StringComparison.OrdinalIgnoreCase) == true)
                return;
            if (!string.IsNullOrEmpty(dataStr))
            {
                if (dataStr.ToLower() == "limpar")
                {
                    livro.DataAquisicao = null;
                }
                else if (DateTime.TryParse(dataStr, out DateTime data))
                {
                    livro.DataAquisicao = data;
                }
            }
            
            Console.Write($"Categoria [{livro.Categoria ?? "Não informada"}]: ");
            string categoria = Console.ReadLine()?.Trim();
            if (categoria?.Equals("cancelar", StringComparison.OrdinalIgnoreCase) == true)
                return;
            if (!string.IsNullOrEmpty(categoria))
            {
                if (categoria.ToLower() == "limpar")
                {
                    livro.Categoria = null;
                }
                else
                {
                    livro.Categoria = categoria;
                }
            }
            
            Console.Write($"Descrição [{livro.Descricao ?? "Não informada"}]: ");
            string descricao = Console.ReadLine()?.Trim();
            if (descricao?.Equals("cancelar", StringComparison.OrdinalIgnoreCase) == true)
                return;
            if (!string.IsNullOrEmpty(descricao))
            {
                if (descricao.ToLower() == "limpar")
                {
                    livro.Descricao = null;
                }
                else
                {
                    livro.Descricao = descricao;
                }
            }
            
            Console.WriteLine("\nConfirmar atualização do livro com os dados acima? (S/N)");
            string confirmacao = Console.ReadLine()?.Trim().ToUpper();
            if (confirmacao != "S")
            {
                Console.WriteLine("Atualização cancelada pelo usuário.");
                return;
            }
            
            var resultado = _controller.AtualizarLivro(livro);
            
            if (resultado.Success)
            {
                ExibirMensagemSucesso("Livro atualizado com sucesso!");
            }
            else
            {
                ExibirMensagemErro(resultado.Message, resultado.Errors);
            }
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        private void ExcluirLivro()
        {
            bool voltar = false;
            
            while (!voltar)
            {
                Console.Clear();
                Console.WriteLine("=== EXCLUIR LIVRO ===\n");
                
                Console.WriteLine("Opções:");
                Console.WriteLine("Digite o ID do livro para excluir");
                Console.WriteLine("V. Voltar ao menu principal");
                Console.WriteLine("S. Sair do sistema");
                Console.Write("\nSua escolha: ");
                
                string entrada = Console.ReadLine()?.Trim();
                
                if (string.IsNullOrEmpty(entrada))
                {
                    ExibirMensagemErro("Entrada inválida. Por favor, digite um valor.");
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    continue;
                }
                
                if (entrada.Equals("v", StringComparison.OrdinalIgnoreCase))
                {
                    voltar = true;
                    continue;
                }
                
                if (entrada.Equals("s", StringComparison.OrdinalIgnoreCase))
                {
                    Environment.Exit(0);
                }
                
                if (int.TryParse(entrada, out int id))
                {
                    var resultadoBusca = _controller.BuscarLivroPorId(id);
                    
                    if (resultadoBusca.Success)
                    {
                        Console.Clear();
                        Console.WriteLine("=== EXCLUIR LIVRO ===\n");
                        
                        Console.WriteLine("Dados do livro a ser excluído:\n");
                        ExibirDetalhesLivro(resultadoBusca.Data);
                        
                        Console.WriteLine("\nATENÇÃO: Esta operação não pode ser desfeita!");
                        Console.WriteLine("Opções:");
                        Console.WriteLine("1. Confirmar exclusão");
                        Console.WriteLine("2. Cancelar e voltar");
                        Console.WriteLine("0. Sair do sistema");
                        Console.Write("\nEscolha uma opção: ");
                        
                        if (int.TryParse(Console.ReadLine(), out int opcao))
                        {
                            switch (opcao)
                            {
                                case 1:
                                    var resultado = _controller.DeletarLivro(id);
                                    
                                    if (resultado.Success)
                                    {
                                        ExibirMensagemSucesso("Livro excluído com sucesso!");
                                    }
                                    else
                                    {
                                        ExibirMensagemErro(resultado.Message, resultado.Errors);
                                    }
                                    
                                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                                    Console.ReadKey();
                                    break;
                                case 2:
                                    Console.WriteLine("Exclusão cancelada pelo usuário.");
                                    break;
                                case 0:
                                    Environment.Exit(0);
                                    break;
                                default:
                                    ExibirMensagemErro("Opção inválida.");
                                    break;
                            }
                        }
                        else
                        {
                            ExibirMensagemErro("Entrada inválida. Por favor, digite um número.");
                        }
                    }
                    else
                    {
                        ExibirMensagemErro(resultadoBusca.Message, resultadoBusca.Errors);
                    }
                    
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
                else
                {
                    ExibirMensagemErro("ID inválido. Por favor, digite um número inteiro.");
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        private void ExibirListaDeLivros(List<Livro> livros)
        {
            Console.WriteLine($"{"ID",-5} | {"Título",-30} | {"Autor",-25} | {"ISBN",-15} | {"Qtd",-5} | {"Preço",-10}");
            Console.WriteLine(new string('-', 100));
            
            foreach (var livro in livros)
            {
                string titulo = livro.Titulo?.Length > 27 ? livro.Titulo.Substring(0, 27) + "..." : livro.Titulo;
                string autor = livro.Autor?.Length > 22 ? livro.Autor.Substring(0, 22) + "..." : livro.Autor;
                string preco = livro.Preco.HasValue ? livro.Preco.Value.ToString("C") : "N/A";
                
                Console.WriteLine($"{livro.Id,-5} | {titulo,-30} | {autor,-25} | {livro.ISBN,-15} | {livro.Quantidade,-5} | {preco,-10}");
            }
        }

        private void ExibirDetalhesLivro(Livro livro)
        {
            if (livro == null) return;
            
            Console.WriteLine($"ID:                 {livro.Id}");
            Console.WriteLine($"Título:             {livro.Titulo}");
            Console.WriteLine($"Autor:              {livro.Autor}");
            Console.WriteLine($"ISBN:               {livro.ISBN}");
            Console.WriteLine($"Ano de Publicação:  {livro.AnoPublicacao}");
            Console.WriteLine($"Editora:            {livro.Editora ?? "Não informada"}");
            Console.WriteLine($"Preço:              {(livro.Preco.HasValue ? livro.Preco.Value.ToString("C") : "Não informado")}");
            Console.WriteLine($"Quantidade:         {livro.Quantidade}");
            Console.WriteLine($"Data de Aquisição:  {(livro.DataAquisicao.HasValue ? livro.DataAquisicao.Value.ToString("dd/MM/yyyy") : "Não informada")}");
            Console.WriteLine($"Categoria:          {livro.Categoria ?? "Não informada"}");
            Console.WriteLine($"Descrição:          {livro.Descricao ?? "Não informada"}");
        }

        private void ExibirMensagemSucesso(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✓ {mensagem}");
            Console.ResetColor();
        }

        private void ExibirMensagemErro(string mensagem, List<string> detalhes = null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n✗ {mensagem}");
            
            if (detalhes != null && detalhes.Any())
            {
                Console.WriteLine("\nDetalhes:");
                foreach (var detalhe in detalhes)
                {
                    Console.WriteLine($"  - {detalhe}");
                }
            }
            
            Console.ResetColor();
        }
    }
}