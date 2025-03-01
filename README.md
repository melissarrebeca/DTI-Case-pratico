# Documentação da Aplicação DTILivraria

## Visão Geral

A aplicação DTILivraria é um sistema para gerenciamento de livros, desenvolvido em formato de aplicação de console que permite catalogar, pesquisar e gerenciar o acervo de uma livraria. A aplicação implementa operações CRUD (Create, Read, Update, Delete) para o recurso Livro.

A arquitetura em camadas escolhida garante separação de responsabilidades, o que faclita a manutenção do sistema. Cada componente tem uma função bem definida dentro da aplicação, o que resulta em um código organizado e modular.

A implementação seguiu os princípios de Orientação a Objetos e os padrões SOLID, com a intenção de garantir um código de alta qualidade e de fácil manutenção.

Os diferenciais (Docker e Logs) foram implementados para que a aplicação ganhe uma forma mais profissional.


## Recurso Principal: Livro

O recurso principal gerenciado pela aplicação é o **Livro**, com as seguintes propriedades:

| Propriedade | Tipo | Obrigatoriedade | Descrição |
|-------------|------|-----------------|-----------|
| Id | int | Obrigatório | Identificador único do livro, gerado automaticamente |
| Titulo | string | Obrigatório | Título do livro |
| Autor | string | Obrigatório | Nome do autor do livro |
| ISBN | string | Obrigatório | Código ISBN do livro, deve ser único |
| AnoPublicacao | int? | Obrigatório | Ano em que o livro foi publicado |
| Editora | string | Opcional | Nome da editora do livro |
| Preco | decimal? | Opcional | Preço de venda do livro |
| Quantidade | int | Opcional | Quantidade disponível no estoque (padrão: 0) |
| DataAquisicao | DateTime? | Opcional | Data em que o livro foi adquirido pela livraria |
| Descricao | string | Opcional | Descrição ou sinopse do livro |
| Categoria | string | Opcional | Categoria ou gênero do livro |

## Tecnologias Utilizadas

- **Linguagem**: C# (.NET 9.0)
- **Banco de Dados**: SQLite
- **Arquitetura**: Arquitetura em camadas (Models, Repositories, Controllers, Views)
- **Diferenciais Implementados**:
  - Conteinerização com Docker
  - Sistema de Logs com Serilog

## Estrutura de Diretórios e suas camadas

```
DTILivraria/
│
├── Models/                # Camada de Modelo
│   └── Livro.cs           # Classe que representa o recurso Livro
│
├── Repositories/          # Camada de Acesso a Dados (Repository Pattern)
│   ├── Interfaces/        # Interfaces para os repositórios
│   │   └── ILivroRepository.cs
│   └── LivroRepository.cs # Implementação concreta do repositório
│
├── Controllers/           # Camada de Controle
│   └── LivroController.cs # Lógica para gerenciar operações com livros
│
├── Views/                 # Camada de Visualização
│   └── LivroView.cs       # Métodos para exibição e entrada de dados
│
├── Database/              # Configuração e scripts do banco de dados
│   └── DatabaseSetup.sql  # Script para criar tabela Livros no SQLite
│
├── Utils/                 # Utilitários e ferramentas auxiliares
│   ├── DatabaseHelper.cs  # Gerenciamento de conexão com o banco
│   └── Validators/        # Classes para validação de dados
│       └── LivroValidator.cs
│
├── Dockerfile             # Configuração para conteinerização Docker
├── docker-compose.yml     # Configuração do Docker Compose
└── Program.cs             # Ponto de entrada da aplicação
```

## Explicação das Camadas

### 1. Models (Camada de Modelo)
- **Propósito**: Representar os dados da aplicação
- **Conteúdo**: Classe Livro, que define as propriedades e comportamentos básicos

### 2. Repositories (Camada de Acesso a Dados)
- **Propósito**: Gerenciar o acesso ao banco de dados e abstrair operações CRUD
- **Interface**: Define as operações, como: GetAll(), GetById(), Add(), Update(), Delete()
- **Implementação**: LivroRepository implementa a interface ILivroRepository e contém o código que interage com o SQLite

### 3. Controllers (Camada de Controle)
- **Propósito**: Gerenciar o fluxo da aplicação
- **Responsabilidades**:
  - Receber solicitações da camada de View
  - Validar dados de entrada
  - Chamar métodos apropriados do repositório
  - Retornar resultados para a View

### 4. Views (Camada de Visualização)
- **Propósito**: Gerenciar interação com usuário no console
- **Responsabilidades**:
  - Mostrar menus e informações
  - Capturar entradas do usuário
  - Formatar resultados para exibição
  - Chamar métodos apropriados do Controller

### 5. Database
- **Propósito**: Manter scripts e configurações relacionadas ao banco de dados
- **Conteúdo**: Script SQL para criar a tabela Livros com todas as colunas necessárias

### 6. Utils
- **Propósito**: Fornecer funcionalidades auxiliares
- **Conteúdo**:
  - Gerenciamento de conexão com o SQLite
  - Validadores para garantir integridade dos dados

## Fluxo de Funcionamento
1. **Program.cs** inicializa a aplicação e apresenta o menu principal
2. O usuário escolhe uma operação no menu exibido pela **View**
3. A **View** pega os dados do usuário e passa para o **Controller**
4. O **Controller** valida os dados e chama o método apropriado do **Repository**
5. O **Repository** executa as operações no banco de dados SQLite
6. Os resultados são retornados ao **Controller**
7. O **Controller** processa os resultados e repassa para a **View**
8. A **View** exibe os resultados formatados para o usuário

## Princípios de Orientação a Objetos Explorados pela Aplicação

### 1. Encapsulamento
- Na classe `Livro`, os atributos são implementados como propriedades com getters e setters
- Os detalhes de implementação do repositório são encapsulados, deixando apenas a interface disponível
- O `DatabaseHelper` encapsula toda a lógica de conexão com o banco de dados

### 2. Abstração
- A interface `ILivroRepository` define uma abstração das operações disponíveis sem revelar como são implementadas
- Usuários do repositório interagem com a abstração (interface) em vez da implementação concreta

### 3. Herança
- A classe `LivroRepository` herda da interface `ILivroRepository`

### 4. Polimorfismo
- O sistema permite que diferentes implementações de `ILivroRepository` possam ser usadas
- Por exemplo, poderíamos criar um `MockLivroRepository` para testes ou um `ApiLivroRepository` que acessa uma API

### 5. Princípios SOLID explorados pela aplicação
- **S** (Single Responsibility): Cada classe tem uma única responsabilidade
- **O** (Open/Closed): O sistema é aberto para extensão através da interface
- **L** (Liskov Substitution): Diferentes implementações de `ILivroRepository` podem ser substituídas
- **I** (Interface Segregation): A interface define apenas os métodos necessários
- **D** (Dependency Inversion): O código depende de abstrações (interface), não implementações concretas

### 6. Padrão de Projeto
- **Repository Pattern**: Separação clara entre lógica e acesso a dados

## Instalação e Configuração

### Pré-requisitos

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/products/docker-desktop) (para execução em container)

### Instalação Local

1. Clone o repositório:
   ```bash
   git clone <url-do-repositorio>
   cd DTILivraria
   ```

2. Restaure as dependências:
   ```bash
   dotnet restore
   ```

3. Compile o projeto:
   ```bash
   dotnet build
   ```

4. Execute a aplicação:
   ```bash
   dotnet run
   ```

### Instalação via Docker

1. Clone o repositório:
   ```bash
   git clone <url-do-repositorio>
   cd DTILivraria
   ```

2. Construa a imagem Docker:
   ```bash
   docker build -t dtilivraria .
   ```

3. Execute o container:
   ```bash
   docker run -it --rm dtilivraria
   ```

4. Alternativamente, use o Docker Compose para persistência de dados:
   ```bash
   docker-compose up -d
   ```

## Uso da Aplicação

Ao iniciar a aplicação, você verá uma tela de boas-vindas seguida do menu principal:

```
╔═══════════════════════════════════════════════════════════╗
║                                                           ║
║              SISTEMA DE GERENCIAMENTO DE LIVROS           ║
║                                                           ║
║                       DTI Livraria                        ║
║                                                           ║
║                       Versão 1.0                          ║
║                                                           ║
╚═══════════════════════════════════════════════════════════╝
```

### Menu Principal

O menu principal oferece as seguintes opções:

**Listar Todos os Livros**: Exibe todos os livros cadastrados no sistema.
**Buscar Livro por ID**: Permite buscar um livro específico pelo seu ID.
**Buscar Livro por ISBN**: Permite buscar um livro específico pelo seu código ISBN.
**Buscar Livros por Autor**: Lista todos os livros de um determinado autor.
**Pesquisar Livros**: Pesquisa livros contendo um termo específico no título, autor, ISBN ou categoria.
**Cadastrar Novo Livro**: Adiciona um novo livro ao sistema.
**Atualizar Livro**: Modifica as informações de um livro existente.
**Excluir Livro**: Remove um livro do sistema.
**Sair**: Encerra a aplicação.

### Funcionalidades Detalhadas

#### Listar Todos os Livros

Esta opção exibe uma lista de todos os livros cadastrados no sistema, apresentando suas informações principais.

Exemplo:
```
ID: 1, Título: Dom Casmurro, Autor: Machado de Assis, ISBN: 9788525406293
ID: 2, Título: O Senhor dos Anéis, Autor: J.R.R. Tolkien, ISBN: 9788533613379
```

#### Buscar Livro por ID

Esta opção permite buscar um livro específico pelo seu ID numérico. Se encontrado, os detalhes completos do livro são exibidos.

Exemplo:
```
Digite o ID do livro: 1

Livro encontrado:
ID: 1
Título: Dom Casmurro
Autor: Machado de Assis
ISBN: 9788525406293
Ano de Publicação: 1899
Editora: Companhia das Letras
Preço: R$ 29,90
Quantidade: 5
Data de Aquisição: 10/01/2023
Categoria: Literatura Brasileira
Descrição: Romance clássico da literatura brasileira que narra a história de Bentinho e Capitu.
```

#### Buscar Livro por ISBN

Similar à busca por ID, mas utilizando o código ISBN do livro como critério de busca.

Exemplo:
```
Digite o ISBN do livro: 9788525406293

Livro encontrado:
[detalhes do livro]
```

#### Buscar Livros por Autor

Lista todos os livros de um determinado autor.

Exemplo:
```
Digite o nome do autor: Machado de Assis

Livros encontrados (2):
ID: 1, Título: Dom Casmurro, Autor: Machado de Assis, ISBN: 9788525406293
ID: 3, Título: Memórias Póstumas de Brás Cubas, Autor: Machado de Assis, ISBN: 9788525411693
```

#### Pesquisar Livros

Permite pesquisar livros usando um termo que pode estar presente no título, autor, ISBN ou categoria.

Exemplo:
```
Digite o termo de pesquisa: fantasia

Livros encontrados (2):
ID: 2, Título: O Senhor dos Anéis, Autor: J.R.R. Tolkien, ISBN: 9788533613379, Categoria: Fantasia
ID: 5, Título: Harry Potter e a Pedra Filosofal, Autor: J.K. Rowling, ISBN: 9788532511010, Categoria: Fantasia
```

#### Cadastrar Novo Livro

Esta opção guia o usuário através do processo de cadastro de um novo livro, solicitando todas as informações necessárias.

Exemplo:
```
=== Cadastro de Novo Livro ===

Título: O Hobbit
Autor: J.R.R. Tolkien
ISBN: 9788595084742
Ano de Publicação: 1937
Editora: HarperCollins
Preço: 45.90
Quantidade: 8
Data de Aquisição (dd/mm/aaaa): 15/02/2023
Categoria: Fantasia
Descrição: Romance de fantasia que precede os eventos de O Senhor dos Anéis.

Livro cadastrado com sucesso! ID: 6
```

#### Atualizar Livro

Permite modificar as informações de um livro existente. O sistema solicita o ID do livro a ser atualizado e, em seguida, apresenta os campos para edição.

Exemplo:
```
Digite o ID do livro que deseja atualizar: 6

=== Atualização de Livro (ID: 6) ===

Título atual: O Hobbit
Novo título (deixe em branco para manter o atual): O Hobbit: Uma Jornada Inesperada

[outros campos para edição]

Livro atualizado com sucesso!
```

#### Excluir Livro

Permite remover um livro do sistema. O sistema solicita confirmação antes de excluir o livro.

Exemplo:
```
Digite o ID do livro que deseja excluir: 6

Livro encontrado:
ID: 6, Título: O Hobbit: Uma Jornada Inesperada, Autor: J.R.R. Tolkien

Tem certeza que deseja excluir este livro? (S/N): S

Livro excluído com sucesso!
```

## Diferenciais Implementados

### 1. Conteinerização com Docker

A aplicação foi conteinerizada com Docker, permitindo sua execução em qualquer ambiente que suporte contêineres. Foram configurados:

- **Dockerfile**: Define como a imagem Docker é construída, usando a imagem base do .NET SDK.
- **docker-compose.yml**: Facilita a execução da aplicação com persistência de dados através de volumes.

Para executar a aplicação via Docker:
```bash
docker-compose up -d
```

A conteinerização garante que a aplicação será executada de forma consistente em qualquer ambiente.

### 2. Sistema de Logs

Foi implementado um sistema de logs abrangente utilizando a biblioteca Serilog. Os logs são gerados para todas as operações importantes do sistema, facilitando o monitoramento e a depuração.

Os logs são escritos tanto no console quanto em arquivos de texto, com rotação diária. Os arquivos de log são armazenados na pasta `Logs` do projeto.

Níveis de log implementados:
- **Debug**: Informações detalhadas para depuração
- **Information**: Eventos normais do fluxo da aplicação
- **Warning**: Situações potencialmente problemáticas
- **Error**: Erros que não interrompem a aplicação
- **Critical**: Erros graves que podem interromper a aplicação
