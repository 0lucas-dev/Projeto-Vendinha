using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SQLite;
using VendinhaCRUD.Data;
using VendinhaCRUD.Models;

namespace VendinhaCRUD.Services
{
    public class ClienteService
    {
        public List<Cliente> Listar(string busca = "", int page = 1, int pageSize = 10)
        {
            var lista = new List<Cliente>();
            int offset = (page - 1) * pageSize;

            string sql = @"
                SELECT c.Id, c.Nome, c.CPF, c.DataNascimento, c.Email,
                    COALESCE(d.Valor, 0) AS TotalDividas
                FROM Clientes c
                LEFT JOIN Dividas d ON d.ClienteId = c.Id AND d.Paga = 0
                WHERE c.Nome LIKE @busca
                ORDER BY TotalDividas DESC
                LIMIT @pageSize OFFSET @offset";

            using (var conn = DatabaseHelper.AbrirConexao())
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@busca", $"%{busca}%");
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@offset", offset);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        lista.Add(MapearCliente(reader));
                }
            }

            return lista;
        }

        public int ContarTotal(string busca = "")
        {
            string sql = "SELECT COUNT(*) FROM Clientes WHERE Nome LIKE @busca";

            using (var conn = DatabaseHelper.AbrirConexao())
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@busca", $"%{busca}%");
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public Cliente BuscarPorId(int id)
        {
            string sql = "SELECT *, 0 AS TotalDividas FROM Clientes WHERE Id = @id";

            using (var conn = DatabaseHelper.AbrirConexao())
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return MapearCliente(reader);
                }
            }
            return null;
        }

        public string Inserir(Cliente cliente)
        {
            string erro = Validar(cliente);
            if (erro != "") return erro;

            string sql = @"INSERT INTO Clientes (Nome, CPF, DataNascimento, Email) 
                           VALUES (@nome, @cpf, @nascimento, @email)";

            using (var conn = DatabaseHelper.AbrirConexao())
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                PreencherParametros(cmd, cliente);
                cmd.ExecuteNonQuery();
            }
            return "";
        }

        public string Atualizar(Cliente cliente)
        {
            string erro = Validar(cliente);
            if (erro != "") return erro;

            string sql = @"UPDATE Clientes 
                           SET Nome = @nome, CPF = @cpf, DataNascimento = @nascimento, Email = @email 
                           WHERE Id = @id";

            using (var conn = DatabaseHelper.AbrirConexao())
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", cliente.Id);
                PreencherParametros(cmd, cliente);
                cmd.ExecuteNonQuery();
            }
            return "";
        }

        public void Excluir(int id)
        {
            using (var conn = DatabaseHelper.AbrirConexao())
            {
                using (var cmd = new SQLiteCommand("DELETE FROM Dividas WHERE ClienteId = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = new SQLiteCommand("DELETE FROM Clientes WHERE Id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private bool ExisteCpf(string cpf, int ignorarId)
        {
            string sql = "SELECT COUNT(*) FROM Clientes WHERE CPF = @cpf AND Id != @id";

            using (var conn = DatabaseHelper.AbrirConexao())
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@cpf", cpf);
                cmd.Parameters.AddWithValue("@id", ignorarId);
                
                object resultadoDoBanco = cmd.ExecuteScalar();
                int totalEncontrado = Convert.ToInt32(resultadoDoBanco);
                return totalEncontrado > 0;
            }
        }

        private string Validar(Cliente cliente)
        {
            cliente.CPF = CpfHelper.Limpar(cliente.CPF);

            var resultados = new List<ValidationResult>();
            var contexto = new ValidationContext(cliente);

            if (!Validator.TryValidateObject(cliente, contexto, resultados, true))
                return resultados[0].ErrorMessage;

            if (!CpfHelper.Valido(cliente.CPF))
                return "CPF inválido. Verifique o número informado.";

            if (ExisteCpf(cliente.CPF, cliente.Id))
                return "Este CPF já está cadastrado para outro cliente.";

            return "";
        }

        private void PreencherParametros(SQLiteCommand cmd, Cliente cliente)
        {
            cmd.Parameters.AddWithValue("@nome", cliente.Nome);
            cmd.Parameters.AddWithValue("@cpf", cliente.CPF);
            cmd.Parameters.AddWithValue("@nascimento", cliente.DataNascimento);
            cmd.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(cliente.Email) ? DBNull.Value : cliente.Email);
        }

        private static Cliente MapearCliente(SQLiteDataReader reader)
        {
            return new Cliente
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nome = reader["Nome"].ToString(),
                CPF = reader["CPF"].ToString(),
                DataNascimento = DateTime.Parse(reader["DataNascimento"].ToString()),
                Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null,
                TotalDividas = Convert.ToDecimal(reader["TotalDividas"])
            };
        }
    }
}
