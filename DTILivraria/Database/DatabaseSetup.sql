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