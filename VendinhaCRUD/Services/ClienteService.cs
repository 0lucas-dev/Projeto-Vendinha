using System;
using System.Collections.Generic;
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
                       IFNULL(SUM(CASE WHEN d.Paga = 0 THEN d.Valor ELSE 0 END), 0) AS TotalDividas
                FROM Clientes c
                LEFT JOIN Dividas d ON d.ClienteId = c.Id
                WHERE c.Nome LIKE @busca
                GROUP BY c.Id
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
                    {
                        lista.Add(MapearCliente(reader));
                    }
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
            string sql = "SELECT Id, Nome, CPF, DataNascimento, Email FROM Clientes WHERE Id = @id";

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

        public bool CPFJaCadastrado(string cpf, int ignorarId = 0)
        {
            string cpfLimpo = CpfHelper.Limpar(cpf);
            string sql = "SELECT COUNT(*) FROM Clientes WHERE CPF = @cpf AND Id != @ignorarId";

            using (var conn = DatabaseHelper.AbrirConexao())
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@cpf", cpfLimpo);
                cmd.Parameters.AddWithValue("@ignorarId", ignorarId);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public void Inserir(Cliente c)
        {
            string sql = @"INSERT INTO Clientes (Nome, CPF, DataNascimento, Email)
                           VALUES (@nome, @cpf, @dataNasc, @email)";

            using (var conn = DatabaseHelper.AbrirConexao())
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@nome", c.Nome);
                cmd.Parameters.AddWithValue("@cpf", CpfHelper.Limpar(c.CPF));
                cmd.Parameters.AddWithValue("@dataNasc", c.DataNascimento.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@email", c.Email ?? (object)DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }

        public void Atualizar(Cliente c)
        {
            string sql = @"UPDATE Clientes
                           SET Nome = @nome, CPF = @cpf, DataNascimento = @dataNasc, Email = @email
                           WHERE Id = @id";

            using (var conn = DatabaseHelper.AbrirConexao())
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@nome", c.Nome);
                cmd.Parameters.AddWithValue("@cpf", CpfHelper.Limpar(c.CPF));
                cmd.Parameters.AddWithValue("@dataNasc", c.DataNascimento.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@email", c.Email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@id", c.Id);
                cmd.ExecuteNonQuery();
            }
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

        private Cliente MapearCliente(SQLiteDataReader reader)
        {
            var c = new Cliente
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nome = reader["Nome"].ToString(),
                CPF = reader["CPF"].ToString(),
                DataNascimento = DateTime.Parse(reader["DataNascimento"].ToString()),
                Email = reader["Email"] == DBNull.Value ? "" : reader["Email"].ToString()
            };


            try { c.TotalDividas = Convert.ToDecimal(reader["TotalDividas"]); }
            catch { c.TotalDividas = 0; }

            return c;
        }
    }
}
