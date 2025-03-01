using System.Collections.Generic;
using DTILivraria.Models;

namespace DTILivraria.Repositories.Interfaces
{
    public interface ILivroRepository
    {
        List<Livro> GetAll();
        Livro GetById(int id);
        int Add(Livro livro);
        bool Update(Livro livro);
        bool Delete(int id);

        Livro GetByISBN(string isbn);
        List<Livro> GetByAutor(string autor);
        List<Livro> Search(string termo);
    }
}