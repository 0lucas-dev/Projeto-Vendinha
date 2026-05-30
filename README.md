# Vendinha Plena – Sistema de Controle de Dívidas

Trabalho acadêmico desenvolvido para a disciplina de **Desenvolvimento de Sistemas** do 3º termo de Análise e Desenvolvimento de Sistemas.

---

## 📋 Descrição

Aplicação desktop desenvolvida em **C# com Windows Forms** para gerenciamento de clientes e controle de dívidas de uma vendinha. O sistema substitui o controle manual em papel, permitindo cadastrar clientes, registrar dívidas e acompanhar os pagamentos.

---

## 🛠️ Tecnologias Utilizadas

| Tecnologia | Versão | Finalidade |
|------------|--------|-----------|
| C# / .NET  | 8.0    | Linguagem e plataforma |
| Windows Forms | - | Interface gráfica desktop |
| SQLite | - | Banco de dados local (arquivo `.db`) |
| ADO.NET | - | Acesso ao banco de dados |
| System.Data.SQLite | 1.0.118 | Driver SQLite para .NET |

---

## 📁 Estrutura do Projeto

```
VendinhaCRUD/
├── Models/
│   ├── Cliente.cs       → Entidade Cliente (dados + cálculo de idade)
│   └── Divida.cs        → Entidade Dívida
├── Data/
│   ├── DatabaseHelper.cs → Conexão e criação do banco
│   └── schema.sql        → Script SQL de referência
├── Services/
│   ├── ClienteService.cs → CRUD de clientes (banco de dados)
│   ├── DividaService.cs  → CRUD de dívidas (banco de dados)
│   └── CpfHelper.cs      → Validação e formatação de CPF
├── Forms/
│   ├── FrmPrincipal.cs       → Tela principal (listagem + busca + paginação)
│   ├── FrmCadastroCliente.cs → Cadastro e edição de clientes
│   ├── FrmDividas.cs         → Listagem de dívidas do cliente
│   └── FrmCadastroDivida.cs  → Cadastro de nova dívida
├── Program.cs           → Ponto de entrada da aplicação
└── VendinhaCRUD.csproj  → Arquivo do projeto
```

---

## ⚙️ Pré-requisitos

- Windows 10 ou superior
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 **ou** VS Code com extensão C#

---

## ▶️ Como Executar

### Opção 1 – VScode

1. Abra o arquivo `VendinhaCRUD.sln`
2. Aguarde o Visual Studio restaurar os pacotes NuGet automaticamente
3. Pressione **F5** para executar

### Opção 2 – Linha de comando

```bash
# Clone o repositório
git clone https://github.com/seu-usuario/VendinhaCRUD.git
cd VendinhaCRUD

# Restaure os pacotes e execute
cd VendinhaCRUD
dotnet restore
dotnet run
```

> O banco de dados `vendinha.db` é criado automaticamente na primeira execução. Não é necessário instalar nenhum servidor de banco de dados.

---

## 🖥️ Funcionalidades

### Clientes
- ✅ Cadastrar cliente (nome, CPF, data de nascimento, e-mail)
- ✅ Editar dados do cliente
- ✅ Excluir cliente (remove as dívidas junto)
- ✅ Buscar por nome (filtro de texto)
- ✅ Listar ordenado do maior devedor para o menor
- ✅ Paginação (10 clientes por página)
- ✅ Idade calculada automaticamente pela data de nascimento

### Dívidas
- ✅ Cadastrar dívida para um cliente
- ✅ Visualizar todas as dívidas do cliente
- ✅ Marcar dívida como paga (registra data de pagamento)
- ✅ Excluir dívida
- ✅ Total em aberto exibido na tela

### Regras de Negócio
- ✅ CPF deve ser válido (algoritmo da Receita Federal)
- ✅ Não é permitido dois clientes com o mesmo CPF
- ✅ Um cliente só pode ter **uma dívida em aberto** por vez
- ✅ E-mail validado quando informado

---

## 🗃️ Banco de Dados

O banco SQLite é criado automaticamente em `vendinha.db` na pasta de execução.

```sql
CREATE TABLE Clientes (
    Id              INTEGER PRIMARY KEY AUTOINCREMENT,
    Nome            TEXT    NOT NULL,
    CPF             TEXT    NOT NULL UNIQUE,
    DataNascimento  TEXT    NOT NULL,
    Email           TEXT
);

CREATE TABLE Dividas (
    Id             INTEGER PRIMARY KEY AUTOINCREMENT,
    ClienteId      INTEGER NOT NULL,
    Valor          REAL    NOT NULL,
    Paga           INTEGER NOT NULL DEFAULT 0,
    DataCriacao    TEXT    NOT NULL,
    DataPagamento  TEXT,
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id)
);
```

---

## 👨‍💻 Autores

- **Nome do Aluno** – RA: XXXXXXX
- Curso: Análise e Desenvolvimento de Sistemas – 3º Termo
- Disciplina: Desenvolvimento de Sistemas

---

## 📝 Observações

- O banco de dados é um arquivo local (`vendinha.db`), não precisa de servidor.
- O arquivo `.db` está no `.gitignore` e não é versionado.
- O projeto foi desenvolvido com foco em clareza e simplicidade, seguindo os conceitos de POO ensinados em aula.
