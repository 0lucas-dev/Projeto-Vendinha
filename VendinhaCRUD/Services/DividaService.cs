using System;
using System.Collections.Generic;
using System.Data.SQLite;
using VendinhaCRUD.Data;
using VendinhaCRUD.Models;

namespace VendinhaCRUD.Services
{

    public class DividaService
    {

        public List<Divida> ListarPorCliente(int clienteId)
        {
            var lista = new List<Divida>();

            string sql = @"
                SELECT d.Id, d.ClienteId, c.Nome AS ClienteNome,
                       d.Valor, d.Paga, d.DataCriacao, d.DataPagamento
                FROM Dividas d
                INNER JOIN Clientes c ON c.Id = d.ClienteId
                WHERE d.ClienteId = @clienteId
                ORDER BY d.Paga ASC, d.DataCriacao DESC";

            using (var conn = DatabaseHelper.AbrirConexao())
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@clienteId", clienteId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        lista.Add(MapearDivida(reader));
                }
            }

            return lista;
        }


        public bool ClientePossuiDividaAberta(int clienteId)
        {
            string sql = "SELECT COUNT(*) FROM Dividas WHERE ClienteId = @clienteId AND Paga = 0";

            using (var conn = DatabaseHelper.AbrirConexao())
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@clienteId", clienteId);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public void Inserir(Divida d)
        {
            string sql = @"INSERT INTO Dividas (ClienteId, Valor, Paga, DataCriacao, DataPagamento)
                           VALUES (@clienteId, @valor, 0, @dataCriacao, NULL)";

            using (var conn = DatabaseHelper.AbrirConexao())
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@clienteId", d.ClienteId);
                cmd.Parameters.AddWithValue("@valor", d.Valor);
                cmd.Parameters.AddWithValue("@dataCriacao", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
            }
        }

        public void Atualizar(Divida d)
        {
            string sql = @"UPDATE Dividas
                           SET Valor = @valor
                           WHERE Id = @id AND Paga = 0";

            using (var conn = DatabaseHelper.AbrirConexao())
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@valor", d.Valor);
                cmd.Parameters.AddWithValue("@id", d.Id);
                cmd.ExecuteNonQuery();
            }
        }


        public void MarcarComoPaga(int dividaId)
        {
            string sql = @"UPDATE Dividas
                           SET Paga = 1, DataPagamento = @dataPgto
                           WHERE Id = @id";

            using (var conn = DatabaseHelper.AbrirConexao())
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@dataPgto", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@id", dividaId);
                cmd.ExecuteNonQuery();
            }
        }

        public void Excluir(int dividaId)
        {
            string sql = "DELETE FROM Dividas WHERE Id = @id";

            using (var conn = DatabaseHelper.AbrirConexao())
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", dividaId);
                cmd.ExecuteNonQuery();
            }
        }

        private Divida MapearDivida(SQLiteDataReader reader)
        {
            return new Divida
            {
                Id = Convert.ToInt32(reader["Id"]),
                ClienteId = Convert.ToInt32(reader["ClienteId"]),
                ClienteNome = reader["ClienteNome"].ToString(),
                Valor = Convert.ToDecimal(reader["Valor"]),
                Paga = Convert.ToInt32(reader["Paga"]) == 1,
                DataCriacao = DateTime.Parse(reader["DataCriacao"].ToString()),
                DataPagamento = reader["DataPagamento"] == DBNull.Value
                    ? (DateTime?)null
                    : DateTime.Parse(reader["DataPagamento"].ToString())
            };
        }
    }
}
