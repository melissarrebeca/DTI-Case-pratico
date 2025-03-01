using System;
using System.Text.RegularExpressions;

namespace DTILivraria.Models
{
    public class Livro
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Autor { get; set; }
        public string? ISBN { get; set; }
        public int? AnoPublicacao { get; set; }
        public string? Editora { get; set; }
        public decimal? Preco { get; set; }
        public int Quantidade { get; set; }
        public DateTime? DataAquisicao { get; set; }
        public string? Descricao { get; set; }
        public string? Categoria { get; set; }
        
        public Livro()
        {
            Quantidade = 0;
        }

        public Livro(string titulo, string autor, string isbn, int anoPublicacao)
        {
            Titulo = titulo;
            Autor = autor;
            ISBN = isbn;
            AnoPublicacao = anoPublicacao;
            Quantidade = 0;
        }

        public bool ValidarCamposObrigatorios()
        {
            return !string.IsNullOrEmpty(Titulo) &&
                   !string.IsNullOrEmpty(Autor) &&
                   !string.IsNullOrEmpty(ISBN) &&
                   AnoPublicacao.HasValue;
        }

        public bool ValidarISBN()
        {
            if (string.IsNullOrEmpty(ISBN))
                return false;

            string cleanISBN = Regex.Replace(ISBN, "[^0-9X]", "");

            return ValidarISBN10(cleanISBN) || ValidarISBN13(cleanISBN);
        }

        private bool ValidarISBN10(string isbn)
        {
            if (isbn.Length != 10)
                return false;

            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                if (!char.IsDigit(isbn[i]))
                    return false;

                sum += (10 - i) * (isbn[i] - '0');
            }

            char lastChar = isbn[9];
            if (lastChar == 'X')
                sum += 10;
            else if (char.IsDigit(lastChar))
                sum += lastChar - '0';
            else
                return false;

            return sum % 11 == 0;
        }

        private bool ValidarISBN13(string isbn)
        {
            if (isbn.Length != 13)
                return false;

            if (!isbn.All(char.IsDigit))
                return false;

            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                sum += (i % 2 == 0) ? isbn[i] - '0' : 3 * (isbn[i] - '0');
            }

            int checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit == (isbn[12] - '0');
        }

        public bool ValidarAnoPublicacao()
        {
            if (!AnoPublicacao.HasValue)
                return false;

            int anoAtual = DateTime.Now.Year;
            return AnoPublicacao.Value <= anoAtual && AnoPublicacao.Value >= 1450;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Título: {Titulo}, Autor: {Autor}, ISBN: {ISBN}";
        }

        public string ExibirDetalhes()
        {
            return $"ID: {Id}\n" +
                   $"Título: {Titulo}\n" +
                   $"Autor: {Autor}\n" +
                   $"ISBN: {ISBN}\n" +
                   $"Ano de Publicação: {AnoPublicacao}\n" +
                   $"Editora: {Editora}\n" +
                   $"Preço: {(Preco.HasValue ? Preco.Value.ToString("C") : "Não informado")}\n" +
                   $"Quantidade: {Quantidade}\n" +
                   $"Data de Aquisição: {(DataAquisicao.HasValue ? DataAquisicao.Value.ToString("dd/MM/yyyy") : "Não informada")}\n" +
                   $"Categoria: {Categoria ?? "Não informada"}\n" +
                   $"Descrição: {Descricao ?? "Não informada"}";
        }

        public int CalcularIdade()
        {
            if (!AnoPublicacao.HasValue)
                return 0;

            return DateTime.Now.Year - AnoPublicacao.Value;
        }

        public bool EstaDisponivel()
        {
            return Quantidade > 0;
        }
    }
}
