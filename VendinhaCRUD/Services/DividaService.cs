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
                SELECT d.Id, d.ClienteId, d.Valor, d.Paga, d.DataCriacao, d.DataPagamento
                FROM Dividas d
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

        public string Inserir(Divida divida)
        {
            if (divida.Valor <= 0)
                return "Informe um valor válido maior que zero.";

            if (ClientePossuiDividaAberta(divida.ClienteId))
                return "Este cliente já possui uma dívida em aberto.";

            string sql = @"INSERT INTO Dividas (ClienteId, Valor, Paga, DataCriacao, DataPagamento)
                           VALUES (@clienteId, @valor, 0, @dataCriacao, NULL)";

            using (var conn = DatabaseHelper.AbrirConexao())
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@clienteId", divida.ClienteId);
                cmd.Parameters.AddWithValue("@valor", divida.Valor);
                cmd.Parameters.AddWithValue("@dataCriacao", DateTime.Now);
                cmd.ExecuteNonQuery();
            }

            return "";
        }

        public void MarcarComoPaga(int dividaId)
        {
            string sql = @"UPDATE Dividas
                           SET Paga = 1, DataPagamento = @dataPgto
                           WHERE Id = @id";

            using (var conn = DatabaseHelper.AbrirConexao())
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@dataPgto", DateTime.Now);
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

        private static Divida MapearDivida(SQLiteDataReader reader)
        {
            return new Divida
            {
                Id = Convert.ToInt32(reader["Id"]),
                ClienteId = Convert.ToInt32(reader["ClienteId"]),
                Valor = Convert.ToDecimal(reader["Valor"]),
                Paga = Convert.ToInt32(reader["Paga"]) == 1,
                DataCriacao = Convert.ToDateTime(reader["DataCriacao"]),
                DataPagamento = reader["DataPagamento"] == System.DBNull.Value
                    ? (DateTime?)null
                    : DateTime.Parse(reader["DataPagamento"].ToString())
            };
        }
    }
}
