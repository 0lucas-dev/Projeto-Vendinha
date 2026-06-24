using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SQLite;
using System.Linq;
using VendinhaCRUD.Data;
using VendinhaCRUD.Models;

namespace VendinhaCRUD.Services
{
    public class ClienteService
    {
        private List<Cliente> list = new List<Cliente>();

        public List<Cliente> Listar()
        {
            var connectionString = "Data Source=vendinha.db;" +
                "Version=3;";

            var comando = "SELECT Id, Nome, CPF, DataNascimento, Email, (SELECT COALESCE(SUM(Valor), 0) FROM Dividas WHERE ClienteId = Clientes.Id AND Paga = 0) FROM Clientes ORDER BY 6 DESC";

            var conexao = new SQLiteConnection(connectionString);
            conexao.Open();

            var sqlCommand = new SQLiteCommand(comando, conexao);

            var leitor = sqlCommand.ExecuteReader();

            var lista = new List<Cliente>();
            while (leitor.Read())
            {
                var a = new Cliente();
                a.Nome = "Teste a";
                var b = new Cliente { Nome = "Teste a" };

                var cliente = new Cliente
                {
                    Id = leitor.GetInt32(0),
                    Nome = leitor.GetString(1),
                    CPF = leitor.GetString(2),
                    DataNascimento = leitor.GetDateTime(3),
                    Email = leitor.IsDBNull(4) ? null : leitor.GetString(4),
                    TotalDividas = leitor.GetDecimal(5)
                };
                lista.Add(cliente);
            }

            return lista;
        }

        public Cliente BuscarPorId(int id)
        {
            var cliente = list.FirstOrDefault(
                (item) => item.Id == id
            );
            return cliente;
        }

        public List<Cliente> Pesquisa(string texto)
        {
            var resultado = list
                .Where(
                (item) => item.Nome.Contains(texto)
                    || (item.Email != null && item.Email.Contains(texto))
                    || item.CPF == texto
                )
                .OrderBy(item =>
                {
                    return item.CPF;
                });

            return resultado.ToList();
        }

        public List<Cliente> Listar(int pageSize, int page)
        {
            var take = pageSize;
            var skip = (page - 1) * pageSize;
            return list.Skip(skip).Take(take).ToList();
        }

        public int ContarTotal(string busca = "")
        {
            return string.IsNullOrEmpty(busca) ? list.Count : Pesquisa(busca).Count;
        }

        public bool Criar(Cliente cliente, out List<ValidationResult> erros)
        {
            if (!Validar(cliente, out erros))
            {
                return false;
            }
            var connectionString = "Data Source=vendinha.db;Version=3;";
            var comando = "INSERT INTO Clientes (Nome, CPF, DataNascimento, Email) "+
                $"VALUES (@Nome, @CPF, @DataNascimento, @Email)";

            var conexao = new SQLiteConnection(connectionString);
            conexao.Open();

            var sqlCommand = new SQLiteCommand(comando, conexao);

            var p1 = sqlCommand.Parameters.Add("@CPF", System.Data.DbType.String);
            p1.Value = cliente.CPF;

            var p2 = sqlCommand.Parameters.Add("@Nome", System.Data.DbType.String);
            p2.Value = cliente.Nome;

            var p3 = sqlCommand.Parameters.Add("@DataNascimento", System.Data.DbType.DateTime);
            p3.Value = cliente.DataNascimento;

            var p4 = sqlCommand.Parameters.Add("@Email", System.Data.DbType.String);
            p4.Value = string.IsNullOrWhiteSpace(cliente.Email) ? DBNull.Value : cliente.Email;

            sqlCommand.ExecuteScalar();

            return true;
        }

        public bool Atualizar(Cliente cliente, out List<ValidationResult> erros)
        {
            if (!Validar(cliente, out erros))
            {
                return false;
            }

            var connectionString = "Data Source=vendinha.db;Version=3;";
            var sql = @"UPDATE Clientes 
                           SET Nome = @Nome, CPF = @CPF, DataNascimento = @DataNascimento, Email = @Email 
                           WHERE Id = @id";

            var conexao = new SQLiteConnection(connectionString);
            conexao.Open();

            var sqlCommand = new SQLiteCommand(sql, conexao);

            var pId = sqlCommand.Parameters.Add("@id", System.Data.DbType.Int32);
            pId.Value = cliente.Id;

            var p1 = sqlCommand.Parameters.Add("@Nome", System.Data.DbType.String);
            p1.Value = cliente.Nome;

            var p2 = sqlCommand.Parameters.Add("@CPF", System.Data.DbType.String);
            p2.Value = cliente.CPF;

            var p3 = sqlCommand.Parameters.Add("@DataNascimento", System.Data.DbType.DateTime);
            p3.Value = cliente.DataNascimento;

            var p4 = sqlCommand.Parameters.Add("@Email", System.Data.DbType.String);
            p4.Value = string.IsNullOrWhiteSpace(cliente.Email) ? DBNull.Value : cliente.Email;

            sqlCommand.ExecuteScalar();

            return true;
        }

        public void Excluir(int id)
        {
            var connectionString = "Data Source=vendinha.db;Version=3;";
            var conexao = new SQLiteConnection(connectionString);
            conexao.Open();

            var sqlCommand1 = new SQLiteCommand("DELETE FROM Dividas WHERE ClienteId = @id", conexao);
            var pId1 = sqlCommand1.Parameters.Add("@id", System.Data.DbType.Int32);
            pId1.Value = id;
            sqlCommand1.ExecuteScalar();

            var sqlCommand2 = new SQLiteCommand("DELETE FROM Clientes WHERE Id = @id", conexao);
            var pId2 = sqlCommand2.Parameters.Add("@id", System.Data.DbType.Int32);
            pId2.Value = id;
            sqlCommand2.ExecuteScalar();
        }

        public bool Validar(Cliente a, out List<ValidationResult> erros)
        {
            a.CPF = CpfHelper.Limpar(a.CPF);
            var contexto = new ValidationContext(a);
            erros = new List<ValidationResult>();
            var objetoValido = Validator.
                    TryValidateObject(
                        a,
                        contexto,
                        erros,
                        true
                    );

            if (!string.IsNullOrEmpty(a.CPF))
            {
                var codigoExistente = list.Any(item => item.CPF == a.CPF && item.Id != a.Id);
                if (codigoExistente)
                {
                    erros.Add(new ValidationResult("Já existe outro cliente com esse CPF",
                    new[] { "CPF" }));
                    objetoValido = false;
                }
            }

            foreach (var erro in erros)
            {
                Console.WriteLine("{0}: {1}",
                    erro.MemberNames.First(),
                    erro.ErrorMessage);
            }
            return objetoValido;
        }
    }
}
