using System;
using System.Collections.Generic;
using DTILivraria.Models;
using DTILivraria.Repositories.Interfaces;
using DTILivraria.Utils;
using DTILivraria.Utils.Validators;

namespace DTILivraria.Controllers
{
    public class LivroController
    {
        private readonly ILivroRepository _repository;
        private readonly LivroValidator _validator;

        public LivroController(ILivroRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _validator = new LivroValidator();
            LoggerService.Debug("LivroController inicializado");
        }

        public OperationResult<List<Livro>> ListarTodosLivros()
        {
            LoggerService.Debug("Controller: Solicitação para listar todos os livros");
            
            try
            {
                var livros = _repository.GetAll();
                
                if (livros.Count == 0)
                {
                    LoggerService.Information("Controller: Nenhum livro encontrado na listagem");
                    return OperationResult<List<Livro>>.SuccessResult(livros, "Nenhum livro encontrado.");
                }
                
                LoggerService.Information($"Controller: {livros.Count} livros listados com sucesso");
                return OperationResult<List<Livro>>.SuccessResult(livros, $"Foram encontrados {livros.Count} livros.");
            }
            catch (Exception ex)
            {
                LoggerService.Error(ex, "Controller: Erro ao listar todos os livros");
                return OperationResult<List<Livro>>.FailureResult("Erro ao listar livros.", ex.Message);
            }
        }

        public OperationResult<Livro> BuscarLivroPorId(int id)
        {
            LoggerService.Debug($"Controller: Solicitação para buscar livro por ID: {id}");
            
            if (id <= 0)
            {
                LoggerService.Warning($"Controller: ID inválido fornecido: {id}");
                return OperationResult<Livro>.FailureResult("ID inválido.", "O ID deve ser maior que zero.");
            }

            try
            {
                var livro = _repository.GetById(id);
                
                if (livro == null)
                {
                    LoggerService.Warning($"Controller: Nenhum livro encontrado com ID: {id}");
                    return OperationResult<Livro>.FailureResult("Livro não encontrado.", $"Nenhum livro encontrado com o ID {id}.");
                }
                
                LoggerService.Information($"Controller: Livro encontrado com ID {id}: '{livro.Titulo}'");
                return OperationResult<Livro>.SuccessResult(livro, "Livro encontrado com sucesso.");
            }
            catch (Exception ex)
            {
                LoggerService.Error(ex, $"Controller: Erro ao buscar livro por ID: {id}");
                return OperationResult<Livro>.FailureResult("Erro ao buscar livro.", ex.Message);
            }
        }

        public OperationResult<Livro> BuscarLivroPorISBN(string isbn)
        {
            LoggerService.Debug($"Controller: Solicitação para buscar livro por ISBN: {isbn}");
            
            if (string.IsNullOrEmpty(isbn))
            {
                LoggerService.Warning("Controller: ISBN vazio fornecido");
                return OperationResult<Livro>.FailureResult("ISBN inválido.", "O ISBN não pode ser vazio.");
            }

            try
            {
                var livro = _repository.GetByISBN(isbn);
                
                if (livro == null)
                {
                    LoggerService.Warning($"Controller: Nenhum livro encontrado com ISBN: {isbn}");
                    return OperationResult<Livro>.FailureResult("Livro não encontrado.", $"Nenhum livro encontrado com o ISBN {isbn}.");
                }
                
                LoggerService.Information($"Controller: Livro encontrado com ISBN {isbn}: '{livro.Titulo}'");
                return OperationResult<Livro>.SuccessResult(livro, "Livro encontrado com sucesso.");
            }
            catch (Exception ex)
            {
                LoggerService.Error(ex, $"Controller: Erro ao buscar livro por ISBN: {isbn}");
                return OperationResult<Livro>.FailureResult("Erro ao buscar livro por ISBN.", ex.Message);
            }
        }

        public OperationResult<List<Livro>> BuscarLivrosPorAutor(string autor)
        {
            LoggerService.Debug($"Controller: Solicitação para buscar livros por autor: '{autor}'");
            
            if (string.IsNullOrEmpty(autor))
            {
                LoggerService.Warning("Controller: Autor vazio fornecido");
                return OperationResult<List<Livro>>.FailureResult("Autor inválido.", "O nome do autor não pode ser vazio.");
            }

            try
            {
                var livros = _repository.GetByAutor(autor);
                
                if (livros.Count == 0)
                {
                    LoggerService.Information($"Controller: Nenhum livro encontrado para o autor: '{autor}'");
                    return OperationResult<List<Livro>>.SuccessResult(livros, $"Nenhum livro encontrado para o autor '{autor}'.");
                }
                
                LoggerService.Information($"Controller: {livros.Count} livros encontrados para o autor '{autor}'");
                return OperationResult<List<Livro>>.SuccessResult(livros, $"Foram encontrados {livros.Count} livros do autor '{autor}'.");
            }
            catch (Exception ex)
            {
                LoggerService.Error(ex, $"Controller: Erro ao buscar livros por autor: '{autor}'");
                return OperationResult<List<Livro>>.FailureResult("Erro ao buscar livros por autor.", ex.Message);
            }
        }

        public OperationResult<List<Livro>> PesquisarLivros(string termo)
        {
            LoggerService.Debug($"Controller: Solicitação para pesquisar livros com termo: '{termo ?? "vazio"}'");
            
            if (string.IsNullOrEmpty(termo))
            {
                LoggerService.Information("Controller: Termo vazio, listando todos os livros");
                return ListarTodosLivros();
            }

            try
            {
                var livros = _repository.Search(termo);
                
                if (livros.Count == 0)
                {
                    LoggerService.Information($"Controller: Nenhum livro encontrado para o termo: '{termo}'");
                    return OperationResult<List<Livro>>.SuccessResult(livros, $"Nenhum livro encontrado para o termo '{termo}'.");
                }
                
                LoggerService.Information($"Controller: {livros.Count} livros encontrados para o termo '{termo}'");
                return OperationResult<List<Livro>>.SuccessResult(livros, $"Foram encontrados {livros.Count} livros para o termo '{termo}'.");
            }
            catch (Exception ex)
            {
                LoggerService.Error(ex, $"Controller: Erro ao pesquisar livros com termo: '{termo}'");
                return OperationResult<List<Livro>>.FailureResult("Erro ao pesquisar livros.", ex.Message);
            }
        }

        public OperationResult<int> CadastrarLivro(Livro livro)
        {
            LoggerService.Debug($"Controller: Solicitação para cadastrar livro: '{livro?.Titulo ?? "null"}'");
            
            if (livro == null)
            {
                LoggerService.Warning("Controller: Tentativa de cadastrar livro nulo");
                return OperationResult<int>.FailureResult("Dados inválidos.", "O livro não pode ser nulo.");
            }

            if (!_validator.ValidarLivro(livro, out List<string> erros))
            {
                LoggerService.Warning($"Controller: Validação falhou ao cadastrar livro: {string.Join(", ", erros)}");
                return OperationResult<int>.FailureResult("Erro de validação.", erros);
            }

            try
            {
                // Verificar se já existe um livro com o mesmo ISBN
                if (!string.IsNullOrEmpty(livro.ISBN))
                {
                    var livroExistente = _repository.GetByISBN(livro.ISBN);
                    if (livroExistente != null)
                    {
                        LoggerService.Warning($"Controller: Tentativa de cadastrar livro com ISBN já existente: {livro.ISBN}");
                        return OperationResult<int>.FailureResult("ISBN já cadastrado.", $"Já existe um livro cadastrado com o ISBN {livro.ISBN}.");
                    }
                }

                int id = _repository.Add(livro);
                LoggerService.Information($"Controller: Livro cadastrado com sucesso. ID: {id}, Título: '{livro.Titulo}'");
                return OperationResult<int>.SuccessResult(id, "Livro cadastrado com sucesso.");
            }
            catch (Exception ex)
            {
                LoggerService.Error(ex, $"Controller: Erro ao cadastrar livro: '{livro.Titulo}'");
                return OperationResult<int>.FailureResult("Erro ao cadastrar livro.", ex.Message);
            }
        }

        public OperationResult<bool> AtualizarLivro(Livro livro)
        {
            LoggerService.Debug($"Controller: Solicitação para atualizar livro ID {livro?.Id}: '{livro?.Titulo ?? "null"}'");
            
            if (livro == null)
            {
                LoggerService.Warning("Controller: Tentativa de atualizar livro nulo");
                return OperationResult<bool>.FailureResult("Dados inválidos.", "O livro não pode ser nulo.");
            }

            if (livro.Id <= 0)
            {
                LoggerService.Warning($"Controller: ID inválido para atualização: {livro.Id}");
                return OperationResult<bool>.FailureResult("ID inválido.", "O ID do livro deve ser maior que zero.");
            }

            if (!_validator.ValidarLivro(livro, out List<string> erros))
            {
                LoggerService.Warning($"Controller: Validação falhou ao atualizar livro: {string.Join(", ", erros)}");
                return OperationResult<bool>.FailureResult("Erro de validação.", erros);
            }

            try
            {
                // Verificar se o livro existe
                var livroExistente = _repository.GetById(livro.Id);
                if (livroExistente == null)
                {
                    LoggerService.Warning($"Controller: Tentativa de atualizar livro inexistente ID: {livro.Id}");
                    return OperationResult<bool>.FailureResult("Livro não encontrado.", $"Nenhum livro encontrado com o ID {livro.Id}.");
                }

                // Verificar se o ISBN foi alterado e, se sim, se já existe outro livro com o mesmo ISBN
                if (!string.IsNullOrEmpty(livro.ISBN) && livro.ISBN != livroExistente.ISBN)
                {
                    var livroComMesmoISBN = _repository.GetByISBN(livro.ISBN);
                    if (livroComMesmoISBN != null && livroComMesmoISBN.Id != livro.Id)
                    {
                        LoggerService.Warning($"Controller: Tentativa de atualizar livro com ISBN já cadastrado em outro livro: {livro.ISBN}");
                        return OperationResult<bool>.FailureResult("ISBN já cadastrado.", $"Já existe um livro diferente cadastrado com o ISBN {livro.ISBN}.");
                    }
                }

                bool sucesso = _repository.Update(livro);
                if (sucesso)
                {
                    LoggerService.Information($"Controller: Livro atualizado com sucesso ID: {livro.Id}, Título: '{livro.Titulo}'");
                    return OperationResult<bool>.SuccessResult(true, "Livro atualizado com sucesso.");
                }
                else
                {
                    LoggerService.Warning($"Controller: Falha ao atualizar livro ID: {livro.Id}");
                    return OperationResult<bool>.FailureResult("Falha ao atualizar.", "Não foi possível atualizar o livro.");
                }
            }
            catch (Exception ex)
            {
                LoggerService.Error(ex, $"Controller: Erro ao atualizar livro ID: {livro.Id}");
                return OperationResult<bool>.FailureResult("Erro ao atualizar livro.", ex.Message);
            }
        }

        public OperationResult<bool> DeletarLivro(int id)
        {
            LoggerService.Debug($"Controller: Solicitação para deletar livro ID: {id}");
            
            if (id <= 0)
            {
                LoggerService.Warning($"Controller: ID inválido para exclusão: {id}");
                return OperationResult<bool>.FailureResult("ID inválido.", "O ID deve ser maior que zero.");
            }

            try
            {
                // Verificar se o livro existe
                var livro = _repository.GetById(id);
                if (livro == null)
                {
                    LoggerService.Warning($"Controller: Tentativa de excluir livro inexistente ID: {id}");
                    return OperationResult<bool>.FailureResult("Livro não encontrado.", $"Nenhum livro encontrado com o ID {id}.");
                }

                // Validar regras de negócio para exclusão, se necessário
                if (!_validator.ValidarExclusao(livro, out List<string> erros))
                {
                    LoggerService.Warning($"Controller: Validação de exclusão falhou para livro ID {id}: {string.Join(", ", erros)}");
                    return OperationResult<bool>.FailureResult("Não é possível excluir o livro.", erros);
                }

                bool sucesso = _repository.Delete(id);
                if (sucesso)
                {
                    LoggerService.Information($"Controller: Livro excluído com sucesso ID: {id}, Título: '{livro.Titulo}'");
                    return OperationResult<bool>.SuccessResult(true, "Livro excluído com sucesso.");
                }
                else
                {
                    LoggerService.Warning($"Controller: Falha ao excluir livro ID: {id}");
                    return OperationResult<bool>.FailureResult("Falha ao excluir.", "Não foi possível excluir o livro.");
                }
            }
            catch (Exception ex)
            {
                LoggerService.Error(ex, $"Controller: Erro ao excluir livro ID: {id}");
                return OperationResult<bool>.FailureResult("Erro ao excluir livro.", ex.Message);
            }
        }
    }
}