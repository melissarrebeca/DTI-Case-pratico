using System;
using System.Collections.Generic;
using DTILivraria.Models;

namespace DTILivraria.Utils.Validators
{
    public class LivroValidator
    {
        public bool ValidarLivro(Livro livro, out List<string> erros)
        {
            erros = new List<string>();

            if (livro == null)
            {
                erros.Add("Livro não pode ser nulo.");
                return false;
            }

            // Validar campos obrigatórios
            if (string.IsNullOrEmpty(livro.Titulo))
                erros.Add("Título é obrigatório.");

            if (string.IsNullOrEmpty(livro.Autor))
                erros.Add("Autor é obrigatório.");
                
            if (string.IsNullOrEmpty(livro.ISBN))
                erros.Add("ISBN é obrigatório.");
            else if (!livro.ValidarISBN())
                erros.Add("ISBN inválido. Formato deve ser ISBN-10 ou ISBN-13 válido.");

            if (!livro.AnoPublicacao.HasValue)
                erros.Add("Ano de publicação é obrigatório.");
            else if (!livro.ValidarAnoPublicacao())
                erros.Add($"Ano de publicação deve estar entre 1450 e {DateTime.Now.Year}.");

            // Validar campos opcionais quando preenchidos
            if (livro.Preco.HasValue && livro.Preco < 0)
                erros.Add("Preço não pode ser negativo.");

            if (livro.Quantidade < 0)
                erros.Add("Quantidade não pode ser negativa.");

            if (livro.DataAquisicao.HasValue && livro.DataAquisicao > DateTime.Now)
                erros.Add("Data de aquisição não pode ser no futuro.");

            return erros.Count == 0;
        }

        public bool ValidarExclusao(Livro livro, out List<string> erros)
        {
            erros = new List<string>();

            if (livro == null)
            {
                erros.Add("Livro não pode ser nulo.");
                return false;
            }

            // Aqui poderiam ser adicionadas regras de negócio específicas
            // Por exemplo, verificar se o livro está emprestado antes de excluir

            return erros.Count == 0;
        }
    }
}