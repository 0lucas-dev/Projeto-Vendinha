using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace VendinhaCRUD.Data
{
    // Responsável por criar/abrir o banco e fornecer a conexão para os outros serviços
    public static class DatabaseHelper
    {
        private static readonly string _dbPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "vendinha.db");

        public static string ConnectionString => $"Data Source={_dbPath};Version=3;";

        public static void InicializarBanco()
        {
            bool dbNova = !File.Exists(_dbPath);

            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();

                if (dbNova)
                    CriarTabelas(conn);
            }
        }

        private static void CriarTabelas(SQLiteConnection conn)
        {
            string sql = @"
                CREATE TABLE IF NOT EXISTS Clientes (
                    Id              INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nome            TEXT    NOT NULL,
                    CPF             TEXT    NOT NULL UNIQUE,
                    DataNascimento  TEXT    NOT NULL,
                    Email           TEXT
                );

                CREATE TABLE IF NOT EXISTS Dividas (
                    Id              INTEGER PRIMARY KEY AUTOINCREMENT,
                    ClienteId       INTEGER NOT NULL,
                    Valor           REAL    NOT NULL,
                    Paga            INTEGER NOT NULL DEFAULT 0,
                    DataCriacao     TEXT    NOT NULL,
                    DataPagamento   TEXT,
                    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id)
                );";

            using (var cmd = new SQLiteCommand(sql, conn))
                cmd.ExecuteNonQuery();
        }

        public static SQLiteConnection AbrirConexao()
        {
            var conn = new SQLiteConnection(ConnectionString);
            conn.Open();
            return conn;
        }
    }
}
